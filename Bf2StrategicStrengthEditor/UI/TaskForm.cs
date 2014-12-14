using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Battlefield2
{
    public partial class TaskForm : Form
    {
        const int WM_SYSCOMMAND = 0x0112;

        const int SC_MOVE = 0xF010;

        /// <summary>
        /// Gets or Sets whether the task is cancelable
        /// </summary>
        protected bool Cancelable = true;

        /// <summary>
        /// Returns whether the Task form is already open and running
        /// </summary>
        /// <returns></returns>
        public static bool IsOpen
        {
            get { return (Instance != null && !Instance.IsDisposed); }
        }

        /// <summary>
        /// The task dialog's instance
        /// </summary>
        private static TaskForm Instance;

        /// <summary>
        /// Private constructor... Use the Show() method rather
        /// </summary>
        private TaskForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open and displays the task form.
        /// </summary>
        /// <param name="Parent">The calling form, so the task form can be centered</param>
        /// <param name="WindowTitle">The task dialog window title</param>
        /// <param name="InstructionText">Instruction text displayed after the info icon. Leave null
        /// to hide the instruction text and icon.</param>
        /// <param name="Style">The progress bar style</param>
        /// <exception cref="Exception">Thrown if the Task form is already open and running. Use the IsOpen property
        /// to determine if the form is already running</exception>
        public static void Show(Form Parent, string WindowTitle, string InstructionText, ProgressBarStyle Style, int Steps)
        {
            Show(Parent, WindowTitle, InstructionText, null, true, Style, Steps);
        }

        /// <summary>
        /// Open and displays the task form.
        /// </summary>
        /// <param name="Parent">The calling form, so the task form can be centered</param>
        /// <param name="WindowTitle">The task dialog window title</param>
        /// <param name="InstructionText">Instruction text displayed after the info icon. Leave null
        /// to hide the instruction text and icon.</param>
        /// <param name="Cancelable">Specifies whether the operation can be canceled</param>
        /// <param name="Style">The progress bar style</param>
        /// <exception cref="Exception">Thrown if the Task form is already open and running. Use the IsOpen property
        /// to determine if the form is already running</exception>
        public static void Show(Form Parent, string WindowTitle, string InstructionText, bool Cancelable, ProgressBarStyle Style, int Steps)
        {
            Show(Parent, WindowTitle, InstructionText, null, Cancelable, Style, Steps);
        }

        /// <summary>
        /// Open and displays the task form.
        /// </summary>
        /// <param name="Parent">The calling form, so the task form can be centered</param>
        /// <param name="WindowTitle">The task dialog window title</param>
        /// <param name="InstructionText">Instruction text displayed after the info icon. Leave null
        /// to hide the instruction text and icon.</param>
        /// <param name="Cancelable">Specifies whether the operation can be canceled</param>
        /// <exception cref="Exception">Thrown if the Task form is already open and running. Use the IsOpen property
        /// to determine if the form is already running</exception>
        public static void Show(Form Parent, string WindowTitle, string InstructionText, bool Cancelable)
        {
            Show(Parent, WindowTitle, InstructionText, null, Cancelable, ProgressBarStyle.Marquee, 0);
        }

        /// <summary>
        /// Open and displays the task form.
        /// </summary>
        /// <param name="Parent">The calling form, so the task form can be centered</param>
        /// <param name="WindowTitle">The task dialog window title</param>
        /// <param name="InstructionText">Instruction text displayed after the info icon. Leave null
        /// to hide the instruction text and icon.</param>
        /// <param name="SubMessage">Detail text that is displayed just above the progress bar</param>
        public static void Show(Form Parent, string WindowTitle, string InstructionText, string SubMessage)
        {
            Show(Parent, WindowTitle, InstructionText, SubMessage, true, ProgressBarStyle.Marquee, 0);
        }

        /// <summary>
        /// Open and displays the task form.
        /// </summary>
        /// <param name="Parent">The calling form, so the task form can be centered</param>
        /// <param name="WindowTitle">The task dialog window title</param>
        /// <param name="InstructionText">Instruction text displayed after the info icon. Leave null
        /// to hide the instruction text and icon.</param>
        /// <param name="SubMessage">Detail text that is displayed just above the progress bar</param>
        /// <param name="Cancelable">Specifies whether the operation can be canceled</param>
        /// <param name="Style">The progress bar style</param>
        /// <exception cref="Exception">Thrown if the Task form is already open and running. Use the IsOpen property
        /// to determine if the form is already running</exception>
        public static void Show(Form Parent, string WindowTitle, string InstructionText, string SubMessage, bool Cancelable, ProgressBarStyle Style, int ProgressBarSteps)
        {
            // Make sure we dont have an already active form
            if (Instance != null && !Instance.IsDisposed)
                throw new Exception("Task Form is already being displayed!");

            // Create new instance
            Instance = new TaskForm();
            Instance.Text = WindowTitle;
            Instance.labelInstructionText.Text = InstructionText;
            Instance.labelContent.Text = SubMessage;
            Instance.Cancelable = Cancelable;
            Instance.progressBar.Style = Style;

            // Setup progress bar
            if (ProgressBarSteps > 0)
            {
                double Percent = (100 / ProgressBarSteps);
                Instance.progressBar.Step = (int)Math.Round(Percent, 0);
                Instance.progressBar.Maximum = Instance.progressBar.Step * ProgressBarSteps;
            }

            // Hide Instruction panel if Instruction Text is empty
            if (String.IsNullOrWhiteSpace(InstructionText))
            {
                Instance.panelMain.Hide();
                Instance.labelContent.Location = new Point(10, 15);
                Instance.labelContent.MaximumSize = new Size(410, 0);
                Instance.labelContent.Size = new Size(410, 0);
                Instance.progressBar.Location = new Point(10, 1);
                Instance.progressBar.Size = new Size(410, 18);
            }

            // Hide Cancel
            if (!Cancelable)
            {
                Instance.panelButton.Hide();
                Instance.Padding = new Padding(0, 0, 0, 15);
                Instance.BackColor = Color.White;
            }

            // Set window position to center parent
            double H = Parent.Location.Y + (Parent.Height / 2) - (Instance.Height / 2);
            double W = Parent.Location.X + (Parent.Width / 2) - (Instance.Width / 2);
            Instance.Location = new Point((int)Math.Round(W, 0), (int)Math.Round(H, 0));

            // Run form in a new thread
            Thread thread = new Thread(new ThreadStart(ShowForm));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            Thread.Sleep(100); // Wait for Run to work
        }

        /// <summary>
        /// Closes the Task dialog
        /// </summary>
        public static void CloseForm()
        {
            if (Instance == null || Instance.IsDisposed)
                throw new Exception("Invalid Operation. Please use the Show method before calling any operational methods");

            try
            {
                Instance.Invoke((Action)delegate()
                {
                    Instance.Close();
                });
            }
            catch { }
        }

        /// <summary>
        /// Runs the form in a new application, to prevent thread lock
        /// </summary>
        protected static void ShowForm()
        {
            Application.Run(Instance);
        }

        /// <summary>
        /// Updates the instruction text on the task dialog
        /// </summary>
        /// <param name="Message"></param>
        public static void UpdateInstructionText(string Message)
        {
            if (Instance == null || Instance.IsDisposed)
                throw new Exception("Invalid Operation. Please use the Show method before calling any operational methods");

            Instance.Invoke((Action)delegate()
            {
                Instance.labelInstructionText.Text = Message;
            });
        }

        /// <summary>
        /// Updates the detail text above the progress bar
        /// </summary>
        /// <param name="Message"></param>
        public static void UpdateStatus(string Message)
        {
            if (Instance == null || Instance.IsDisposed)
                throw new Exception("Invalid Operation. Please use the Show method before calling any operational methods");

            Instance.Invoke((Action)delegate()
            {
                Instance.labelContent.Text = Message;
            });
        }

        /// <summary>
        /// Updates the progress bar's value
        /// </summary>
        /// <param name="Percent"></param>
        public static void ProgressBarStep()
        {
            if (Instance == null || Instance.IsDisposed)
                throw new Exception("Invalid Operation. Please use the Show method before calling any operational methods");

            Instance.Invoke((Action)delegate()
            {
                Instance.progressBar.PerformStep();
            });
        }

        #region Non Static

        new public void Show()
        {
            base.Show();
        }

        new public void Show(IWin32Window owner)
        {
            base.Show(owner);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TaskForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Instance = null;
        }

        #endregion
    }
}
