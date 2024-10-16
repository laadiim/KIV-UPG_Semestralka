using System.Windows.Forms;

namespace UPG_SP_2024
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            int n;
            if (args.Length != 1)
            {
                n = 0;
            }
            else
            {
                n = int.Parse(args[0]);
            }
            int scenario_num = n;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(scenario_num));
        }
    }
}