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
            var startPath = Environment.CurrentDirectory;
            Console.WriteLine("当前目录：{0}", startPath);
            string TemplateFilePath = null;
            //源excel文件路径
            var Directory = "settingsrc";
            //输出tml文件路径
            var OutputDirectory = "setting";
            //生成的代码路径
            var CodeFilePath = "Code.cs";
            var batchCompiler = new BatchCompiler();

            string templateString = DefaultTemplate.GenCodeTemplate;
            if (!string.IsNullOrEmpty(TemplateFilePath))
            {
                Console.WriteLine(TemplateFilePath);
                templateString = System.IO.File.ReadAllText(TemplateFilePath);
            }

            var results = batchCompiler.CompileTableMLAll(Directory, OutputDirectory, CodeFilePath,
               templateString, "AppSettings", ".tml", null, !string.IsNullOrEmpty(CodeFilePath));

            Console.WriteLine("Done!");
        }
    }
}
