using System;
using System.IO;
using CommandLine;
using CommandLine.Text;
using TableML.Compiler;

namespace TableCompilerConsole
{
    /// <summary>
    /// 调试代码
    /// </summary>
    class LocalDebug
    {
        public static void Main(string[] args)
        {
            //CompileOne();
            CompileAll();
        }

        /// <summary>
        /// 编译单个excel
        /// </summary>
        public static void CompileOne()
        {
            var startPath = Environment.CurrentDirectory;
            //源excel文件路径
            var srcFile = Path.Combine(startPath, "settingsrc", "Test.xlsx");
            //输出tml文件路径
            var OutputDirectory = Path.Combine(startPath, "setting", "Test.tml");
            //生成的代码路径
            var CodeFilePath = "Code.cs";
            if (File.Exists(srcFile) == false)
            {
                Console.WriteLine("{0} 源文件不存在！", srcFile);
                return;
            }
            Console.WriteLine("当前编译的Excel：{0}", srcFile);
            //TODO 代码的重新生成
            Compiler compiler = new Compiler();
            compiler.Compile(srcFile, OutputDirectory);


            Console.WriteLine("Done!");
        }

        /// <summary>
        /// 编译整个目录的excel，每个表生成一个cs文件
        /// </summary>
        public static void CompileAll()
        {
            var startPath = Environment.CurrentDirectory;
            Console.WriteLine("当前目录：{0}", startPath);
            //源excel文件路径
            var srcDirectory = "settingsrc";
            //输出tml文件路径
            var OutputDirectory = "setting";
            //生成的代码路径
            var CodeFilePath = "GenCode\\";
            var batchCompiler = new BatchCompiler();

            string templateString = DefaultTemplate.GenSingleClassCodeTemplate;

            var results = batchCompiler.CompileTableMLAllInSingleFile(srcDirectory, OutputDirectory, CodeFilePath,
               templateString, "AppSettings", ".tml", null, !string.IsNullOrEmpty(CodeFilePath));

            Console.WriteLine("Done!");
        }
    }
}
