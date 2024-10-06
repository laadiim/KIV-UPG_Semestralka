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
            if (args.Length != 1)
            {
                throw new ArgumentException("chybi cislo scenaria");
            }
            int scenario_num = int.Parse(args[0]);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(scenario_num));
        }
    }
}