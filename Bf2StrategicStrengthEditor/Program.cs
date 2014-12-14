/// <summary>
/// ---------------------------------------
/// Battlefield 2 AI Editor
/// ---------------------------------------
/// Created By: Steven Wilson <Wilson212>
/// Copyright (C) 2014, Steven Wilson. All Rights Reserved
/// ---------------------------------------
/// </summary>
using System;
using System.Threading;
using System.Windows.Forms;

namespace Battlefield2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Set exception Handler
            Application.ThreadException += new ThreadExceptionEventHandler(ExceptionHandler.OnThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler.OnUnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
