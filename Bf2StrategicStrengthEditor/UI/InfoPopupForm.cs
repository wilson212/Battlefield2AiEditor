using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;

namespace Battlefield2
{
    public partial class InfoPopupForm : Form
    {
        public InfoPopupForm(XmlNode ItemNode)
        {
            InitializeComponent();

            // Set form values
            NameLabel.Text = ItemNode.Attributes["name"].InnerText.ToLowerInvariant();
            linkLabel1.Text = ItemNode.SelectSingleNode("//object[@name='" + NameLabel.Text + "']/link").InnerText.Trim();
            textBox1.Text = FixString(ItemNode.SelectSingleNode("//object[@name='" + NameLabel.Text + "']/desc").InnerText);
        }

        /// <summary>
        /// Removes excess whitespace in a string, and removes all new line breaks
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static string FixString(string text)
        {
            return String.Join(" ", text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).Replace(System.Environment.NewLine, " ");
        }

        /// <summary>
        /// Event fired when the link label is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel1.Text);
        }
    }
}
