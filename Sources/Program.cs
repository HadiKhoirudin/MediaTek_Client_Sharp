using System;
using System.Windows.Forms;

namespace mtkclient
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main());
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}
