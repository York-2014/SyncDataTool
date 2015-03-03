using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SyncDataTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (false == AppRunAlready())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                Application.Exit();
            }
        }
        public static bool AppRunAlready()
        {//只能启动一个实例
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            return processes.Length > 1;
        }
    }
}
