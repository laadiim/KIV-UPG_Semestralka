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
            int n = 0;
            int gridX = 50; // Default grid X spacing
            int gridY = 50; // Default grid Y spacing

            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    // Parse scenario number
                    if (int.TryParse(arg, out int scenario))
                    {
                        n = scenario;
                        if (n < 0 || n > 6)
                        {
                            n = 0; // Default to scenario 0 if out of bounds
                        }
                    }
                    else if (arg.StartsWith("-g"))
                    {
                        // Parse grid spacing argument
                        string[] parts = arg.Substring(2).Split('x');
                        if (parts.Length == 2 &&
                            int.TryParse(parts[0], out int x) &&
                            int.TryParse(parts[1], out int y) &&
                            x > 0 && y > 0) // Ensure positive values
                        {
                            gridX = x;
                            gridY = y;
                        }
                        else
                        {
                            Console.WriteLine("Invalid grid parameter. Using default values.");
                        }
                    }
                }
            }
            else
            {
                n = 0; // Default to scenario 0 if no arguments provided
            }

            int scenario_num = n;
            CreateBaseFiles();
            Console.WriteLine($"Creating scenario #{scenario_num} with grid spacing {gridX}x{gridY}...");
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
        }
    }
}