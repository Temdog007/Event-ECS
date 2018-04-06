using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaCompiler
{
    class Program
    {
        private const string LuaCompiler = "bin2c.exe";

        static void Main(string[] args)
        {
            ProcessStartInfo info = new ProcessStartInfo(LuaCompiler);
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;

            StringBuilder code = new StringBuilder();
            foreach(string arg in args)
            {
                string filename = Path.GetFileName(arg);
                string file = Path.GetFileNameWithoutExtension(arg);

                info.Arguments = string.Format(@"""{0}""", arg, file);
                Console.WriteLine("Compiling {0}", file);
                using (Process process = Process.Start(info))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    if(!string.IsNullOrEmpty(output))
                    {
                        output = output.Replace(arg, filename);
                        output = output.Replace("B1", file.Replace(".", "_").ToUpper());
                        code.AppendLine(output);
                    }
                    string err = process.StandardError.ReadToEnd();
                    if(!string.IsNullOrEmpty(err))
                    {
                        Console.WriteLine(err);
                    }
                    else
                    {
                        Console.WriteLine("Compiled {0}", file);
                    }
                    process.WaitForExit();
                }
            }

            Console.WriteLine("Writing to compiledCode.h");
            File.WriteAllText("compiledCode.h", code.ToString());
            Console.WriteLine("Wrote to compiledCode.h");

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
