using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;

namespace Battlefield2
{
    /// <summary>
    /// A simple object to handle exceptions thrown during runtime
    /// </summary>
    public sealed class ExceptionHandler
    {
        private ExceptionHandler() { }

        /// <summary>
        /// Handles an exception on the main thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="t"></param>
        public static void OnThreadException(object sender, ThreadExceptionEventArgs t)
        {
            // Create Trace Log
            string FileName = Path.Combine(Application.StartupPath, "ExceptionLog_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt");
            try
            {
                // Try to generate a trace log
                GenerateExceptionLog(FileName, t.Exception);
            }
            catch { }

            // Display the Exception Form
            ExceptionForm EForm = new ExceptionForm(t.Exception, true);
            EForm.Message = "An unhandled exception was thrown while trying to preform the requested task.\r\n"
                + "If you click Continue, the application will attempt to ignore this error, and continue. "
                + "If you click Quit, the application will close immediatly.";
            EForm.TraceLog = FileName;
            DialogResult Result = EForm.ShowDialog();

            // Kill the form on abort
            if (Result == DialogResult.Abort)
                Application.Exit();
        }

        /// <summary>
        /// Handles cross thread exceptions, that are unrecoverable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Create Trace Log
            string FileName = Path.Combine(Application.StartupPath, "ExceptionLog_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt");
            Exception Ex = e.ExceptionObject as Exception;
            ExceptionForm EForm = new ExceptionForm(Ex, false);

            try
            {
                // Try to generate a trace log
                GenerateExceptionLog(FileName, Ex);

                // Display the Exception Form
                EForm.Message = "An unhandled exception was thrown while trying to preform the requested task.\r\n"
                    + "A trace log was generated the program root folder, to "
                    + "assist with debugging, and getting help with this error.";
                EForm.TraceLog = FileName;
            }
            catch
            {
                EForm.Message = "An unhandled exception was thrown while trying to preform the requested task.\r\n"
                    + "A trace log was unable to be generated because that threw another exception :(. The error message "
                    + "for the trace log was stored in the program error log for debugging purposes.";
            }
            finally
            {
                EForm.ShowDialog();
                Application.Exit();
            }
        }

        /// <summary>
        /// Generates a trace log for an exception. If an exception is thrown here, The error
        /// will automatically be logged in the programs error log
        /// </summary>
        public static void GenerateExceptionLog(Exception E)
        {
            string FileName = Path.Combine(Application.StartupPath, "ExceptionLog_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt");
            GenerateExceptionLog(FileName, E);
        }

        /// <summary>
        /// Generates a trace log for an exception. If an exception is thrown here, The error
        /// will automatically be logged in the programs error log
        /// </summary>
        /// <param name="FileName">The tracelog filepath (Must not exist yet)</param>
        /// <param name="E">The exception to log</param>
        public static void GenerateExceptionLog(string FileName, Exception E)
        {
            // Try to write to the log
            try
            {
                // Generate the tracelog
                using (StreamWriter Log = new StreamWriter(File.Open(FileName, FileMode.Create, FileAccess.Write)))
                {
                    // Write the header data
                    Log.WriteLine("-------- BF2 Ai Editor Exception Trace Log --------");
                    Log.WriteLine("Exception Date: " + DateTime.Now.ToString());
                    Log.WriteLine("Os Version: " + Environment.OSVersion.VersionString);
                    Log.WriteLine("System Type: " + ((Environment.Is64BitOperatingSystem) ? "64 Bits" : "32 Bits"));
                    Log.WriteLine();
                    Log.WriteLine("-------- Exception --------");

                    // Log each exception
                    int i = 0;
                    while(true)
                    {
                        // Create a stack trace
                        StackTrace trace = new StackTrace(E, true);
                        StackFrame frame = trace.GetFrame(0);

                        // Log the current exception
                        Log.WriteLine("Type: " + E.GetType().FullName);
                        Log.WriteLine("Message: " + E.Message.Replace("\n", "\n\t"));
                        Log.WriteLine("Target Method: " + frame.GetMethod().Name);
                        Log.WriteLine("File: " + frame.GetFileName());
                        Log.WriteLine("Line: " + frame.GetFileLineNumber());
                        Log.WriteLine("StackTrace:");
                        Log.WriteLine(E.StackTrace.TrimEnd());

                        // If we have no more inner exceptions, end the logging
                        if (E.InnerException == null)
                            break;

                        // Prepare next inner exception data
                        Log.WriteLine();
                        Log.WriteLine("-------- Inner Exception ({0}) --------", i++);
                        E = E.InnerException;
                    }

                    Log.Flush();
                }
            }
            catch (Exception Ex)
            {
                //Program.ErrorLog.Write("FATAL: Unable to write tracelog!!! : " + Ex.ToString());
                throw;
            }
        }
    }
}
