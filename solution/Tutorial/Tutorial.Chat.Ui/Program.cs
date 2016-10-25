using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tutorial.Chat.Ui
{
    /// <summary>
    /// This class serves as the main entry point of the application.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MultiFormContext(new ChatForm("Client 1"), new ChatForm("Client 2")));
        }
    }

    /// <summary>
    /// This class facilitates running multiple instances of the Form.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.ApplicationContext" />
    internal class MultiFormContext : ApplicationContext
    {
        /// <summary>
        /// The open forms{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        private int _openForms;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiFormContext" /> class.
        /// </summary>
        /// <param name="forms">The forms.</param>
        public MultiFormContext(params Form[] forms)
        {
            _openForms = forms.Length;

            foreach (var form in forms)
            {
                form.FormClosed += (s, args) =>
                {
                    //When we have closed the last of the "starting" forms, 
                    //end the program.
                    if (Interlocked.Decrement(ref _openForms) == 0)
                        ExitThread();
                };

                form.Show();
            }
        }
    }
}
