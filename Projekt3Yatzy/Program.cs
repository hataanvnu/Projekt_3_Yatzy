using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt3Yatzy
{
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
            
            Application.Run(new FormGameBoard());
            Client myClient = new Client();

            Thread clientThread = new Thread(myClient.Start);
            clientThread.Start();
            clientThread.Join();
        }
    }
}
