using System.Windows.Forms;
using UPG_SP_2024.Primitives;

namespace UPG_SP_2024
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            int n;
            int gridX = 50;
            int gridY = 50;
            if (args.Length != 1)
            {
                n = 0;
            }
            else
            {
                n = int.Parse(args[0]);
                if (n < 0 || n > 6)
                {
                    n = 0;
                }
            }
            int scenario_num = n;
            CreateBaseFiles();
            Console.WriteLine("Creating scenario #" + scenario_num + "...");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(scenario_num, gridX, gridY));
        }

        static void CreateBaseFiles()
        {
            StreamWriter sw = new StreamWriter("scen0.upg");
            string s = "sonda:0;0;1;3.14159 / 6\nnaboj:1;0;0";
            sw.Write(s);
            sw.Close();
            sw = new StreamWriter("scen1.upg");
            s = "sonda:0;0;1;3.14159 / 6\nnaboj:1;-1;0\nnaboj:1;1;0";
            sw.Write(s);
            sw.Close();
            sw = new StreamWriter("scen2.upg");
            s = "sonda:0;0;1;3.14159 / 6\nnaboj:-1;-1;0\nnaboj:2;1;0";
            sw.Write(s);
            sw.Close();
            sw = new StreamWriter("scen3.upg");
            s = "sonda:0;0;1;3.14159 / 6\nnaboj:1;-1;-1\nnaboj:2;1;-1\nnaboj:-3;1;1\nnaboj:-4;-1;1";
            sw.Write(s);
            sw.Close();
            sw = new StreamWriter("scen4.upg");
            s = "sonda:0;0;1;3.14159 / 6\nnaboj:1 + 0.5 * Sin(0.5 * 3.14159 * [t]);-1;0\nnaboj:1 - 0.5 * Sin(0.5 * 3.14159 * [t]);1;0";
            sw.Write(s);
            sw.Close();
            sw = new StreamWriter("scen5.upg");
            s = "sonda:0;0;1;3.14159 / 6\nnaboj:-4;-1;1\nnaboj:Sin(3.14159 * [t]);Sin(3.14159 * [t] * 0.05);Cos(3.14159 * [t] * 0.1)";
            sw.Write(s);
            sw.Close();
        }
    }
}