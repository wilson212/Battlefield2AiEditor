using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Battlefield2
{
    public class AiFileParser
    {
        // ==== Parser Token Types
        public const string RemComment = "RemComment";
        public const string ObjectStart = "ObjectStart";
        public const string ObjectProperty = "ObjectProperty";
        // ====

        /// <summary>
        /// Parses an AiTemplate file, loading all of its objects into the ObjectManager
        /// </summary>
        /// <param name="FilePath">The full path to the extracted AiTemplate.ai file</param>
        public static void Parse(AiFile ConFile)
        {
            // Split our contents into an object araray
            IEnumerable<Token> FileTokens = Tokenize(
                String.Join(Environment.NewLine, File.ReadAllLines(ConFile.FilePath).Where(x => !String.IsNullOrWhiteSpace(x)))
            );

            // Create our needed objects
            RemComment Comment = new RemComment();

            // Create an empty object template
            ObjectTemplate template = new ObjectTemplate();

            // Add our file objects
            ConFile.Objects = new Dictionary<string, ObjectTemplate>();

            // proceed
            foreach (Token Tkn in FileTokens)
            {
                // Clean line up
                string TokenValue = Tkn.Value.TrimStart().TrimEnd(Environment.NewLine.ToCharArray());

                // Handle Rem Comments
                if (Tkn.Kind == RemComment)
                {
                    StringBuilder CommentBuilder = new StringBuilder();
                    CommentBuilder.AppendLine(TokenValue);
                    Comment.Position = Tkn.Position;
                    Comment.Value += CommentBuilder.ToString();
                }

                // Handle Object Starts
                else if (Tkn.Kind == ObjectStart)
                {
                    // Split line into function call followed by and arguments
                    string[] funcArgs = TokenValue.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    // Get function info [0] => templateName [1] => methodOrVariable
                    string[] funcInfo = funcArgs[0].Split(new char[] { '.' });

                    // Get our template type
                    TemplateType Type = (TemplateType)Enum.Parse(typeof(TemplateType), funcInfo[0], true);

                    // Create the new object
                    template = (Type == TemplateType.AiTemplate) ? new AiTemplate() : new ObjectTemplate();
                    template.Name = funcArgs.Last();
                    template.TemplateTypeString = funcInfo[0];
                    template.ObjectType = (funcArgs.Length > 2) ? funcArgs[1] : "";
                    template.TemplateType = Type;
                    template.Comment = Comment;
                    template.Position = Tkn.Position;
                    template.Properties = new Dictionary<string, List<ObjectProperty>>();
                    template.File = ConFile;

                    // Add object to our objects array, and our object manager
                    ConFile.Objects.Add(template.Name, template);
                    ObjectManager.RegisterObject(template.Name, ConFile);

                    // Reset comment
                    Comment = new RemComment();
                }

                // handle properties
                else if (Tkn.Kind == ObjectProperty)
                {
                    // Split line into function call followed by and arguments
                    string[] funcArgs = TokenValue.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    // Get function info [0] => templateName [1] => methodOrVariable
                    string[] funcInfo = funcArgs[0].Split(new char[] { '.' });

                    // Sub object?
                    if (funcInfo[1] == "addPlugIn")
                    {
                        // Throw an exception if we are invalidly adding a plugin
                        if (template.TemplateType != TemplateType.AiTemplate)
                            throw new Exception("Attempting to add a plugin to object type: " + template.TemplateType.ToString());

                        // Fetch the plugin
                        ObjectTemplate plugin = ObjectManager.GetObjectByName(funcArgs[1]);

                        // Add template plugin
                        (template as AiTemplate).Plugins.Add(
                            (AiTemplatePluginType)Enum.Parse(typeof(AiTemplatePluginType), plugin.ObjectType, true),
                            plugin
                        );
                    }

                    // Create the object property
                    ObjectProperty prop = new ObjectProperty()
                    {
                        Name = funcInfo[1],
                        Comment = Comment,
                        Position = Tkn.Position,
                        Values = funcArgs.Skip(1).ToArray(),
                    };

                    // Add template property if we dont have one
                    if (!template.Properties.ContainsKey(funcInfo[1]))
                        template.Properties.Add(funcInfo[1], new List<ObjectProperty>());

                    // Add the porperty
                    template.Properties[funcInfo[1]].Add(prop);

                    // Reset comment
                    Comment = new RemComment();
                }
            }
        }

        /// <summary>
        /// Converts the parsed Ai Templates back to the format they have to be in the file.
        /// </summary>
        /// <param name="templates"></param>
        /// <returns></returns>
        public static string ToFileFormat(AiFile File)
        {
            StringBuilder Output = new StringBuilder();

            foreach (ObjectTemplate Template in File.Objects.Values)
            {
                // Add the object
                BuildObjectString(Template, ref Output);
            }

            return Output.ToString().TrimEnd();
        }

        /// <summary>
        /// Converts the given ObjectTemplate to its File Format
        /// </summary>
        /// <param name="Template"></param>
        /// <param name="Output"></param>
        protected static void BuildObjectString(ObjectTemplate Template, ref StringBuilder Output)
        {
            // Add comment if we have one
            if (!String.IsNullOrWhiteSpace(Template.Comment.Value))
            {
                Output.Append(Template.Comment.Value.TrimEnd());
                Output.AppendLine();
            }

            // Add the create command
            Output.Append(Template.TemplateTypeString + ".create ");

            // Add object type if we have one
            if (Template.ObjectType != "")
                Output.Append(Template.ObjectType + " ");

            // Append the template name
            Output.Append(Template.Name);
            Output.AppendLine();

            // Add properties
            foreach (List<ObjectProperty> Properties in Template.Properties.Values)
            {
                foreach (ObjectProperty Property in Properties)
                {
                    // Add comment if we have one
                    if (!String.IsNullOrWhiteSpace(Property.Comment.Value))
                    {
                        Output.Append(Property.Comment.Value.TrimEnd());
                        Output.AppendLine();
                    }

                    // Entry start
                    Output.Append(Template.TemplateTypeString + "." + Property.Name);
                    foreach (string value in Property.Values)
                    {
                        Output.Append(" " + value);
                    }

                    // Line
                    Output.AppendLine();
                }
            }

            // Trailing lines
            Output.AppendLine();
        }

        /// <summary>
        /// Breaks an input string into recognizable tokens
        /// </summary>
        /// <param name="source">The input string to break up</param>
        /// <returns>The set of tokens located within the string</returns>
        protected static IEnumerable<Token> Tokenize(string source)
        {
            var tokens = new List<Token>();
            var sourceParts = new[] { new KeyValuePair<string, int>(source, 0) };

            tokens.AddRange(Tokenize(RemComment, @"^rem([\s|\t]+)(?<value>.*)?$", ref sourceParts));
            tokens.AddRange(Tokenize(ObjectStart, @"^(?<type>[a-z]+)\.create([\s|\t]+)(?<value>.*)$", ref sourceParts));
            // tokens.AddRange(Tokenize(ComponentStart, @"^(?<type>[a-z]+)\.createComponent([\s|\t]+)(?<value>.*)$", ref sourceParts));
            tokens.AddRange(Tokenize(ObjectProperty, @"^(?<type>[a-z]+)\.(?<property>[a-z]+)([\s|\t]+)(?<value>.*)?$", ref sourceParts));

            return tokens.OrderBy(x => x.Position);
        }

        /// <summary>
        /// Performs tokenization of a collection of non-tokenized data parts with a specific pattern
        /// </summary>
        /// <param name="tokenKind">The name to give the located tokens</param>
        /// <param name="pattern">The pattern to use to match the tokens</param>
        /// <param name="untokenizedParts">The portions of the input that have yet to be tokenized (organized as text vs. position in source)</param>
        /// <returns>The set of tokens matching the given pattern located in the untokenized portions of the input, 
        /// <paramref name="untokenizedParts"/> is updated as a result of this call</returns>
        protected static IEnumerable<Token> Tokenize(string tokenKind, string pattern, ref KeyValuePair<string, int>[] untokenizedParts)
        {
            //Do a bit of setup
            var resultParts = new List<KeyValuePair<string, int>>();
            var resultTokens = new List<Token>();
            var regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

            //Look through all of our currently untokenized data
            foreach (var part in untokenizedParts)
            {
                //Find all of our available matches
                var matches = regex.Matches(part.Key).OfType<Match>().ToList();

                //If we don't have any, keep the data as untokenized and move to the next chunk
                if (matches.Count == 0)
                {
                    resultParts.Add(part);
                    continue;
                }

                //Store the untokenized data in a working copy and save the absolute index it reported itself at in the source file
                var workingPart = part.Key;
                var index = part.Value;

                //Look through each of the matches that were found within this untokenized segment
                foreach (var match in matches)
                {
                    //Calculate the effective start of the match within the working copy of the data
                    var effectiveStart = match.Index - (part.Key.Length - workingPart.Length);
                    resultTokens.Add(Token.Create(tokenKind, match, part.Value));

                    //If we didn't match at the beginning, save off the first portion to the set of untokenized data we'll give back
                    if (effectiveStart > 0)
                    {
                        var value = workingPart.Substring(0, effectiveStart);
                        resultParts.Add(new KeyValuePair<string, int>(value, index));
                    }

                    //Get rid of the portion of the working copy we've already used
                    if (match.Index + match.Length < part.Key.Length)
                    {
                        workingPart = workingPart.Substring(effectiveStart + match.Length);
                    }
                    else
                    {
                        workingPart = string.Empty;
                    }

                    //Update the current absolute index in the source file we're reporting to be at
                    index += effectiveStart + match.Length;
                }

                //If we've got remaining data in the working copy, add it back to the untokenized data
                if (!string.IsNullOrEmpty(workingPart))
                {
                    resultParts.Add(new KeyValuePair<string, int>(workingPart, index));
                }
            }

            //Update the untokenized data to contain what we couldn't process with this pattern
            untokenizedParts = resultParts.ToArray();
            //Return the tokens we were able to extract
            return resultTokens;
        }
    }
}
