using System;
using CommandLine;
using CommandLine.Text;
using TableML.Compiler;

namespace TableCompilerConsole
{

	class Option
	{
		[Option('f', "file", Required = true,
			HelpText = "Input excel or tsv file to be processed.")]
		public string InputFile { get; set; }

		[Option('v', "verbose", DefaultValue = true,
		  HelpText = "Prints all messages to standard output.")]
		public bool Verbose { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
	class TableCompilerConsole
	{
		public static void Main(string[] args)
		{
			var options = new Option();
			if (CommandLine.Parser.Default.ParseArguments(args, options))
			{
				// Values are available here
				if (options.Verbose) Console.WriteLine("Filename: {0}", options.InputFile);

				var compiler = new Compiler();
				var result = compiler.Compile(options.InputFile);
				Console.WriteLine(string.Format("Compile excel file: {0} , to {1}", options.InputFile, result.TabFilePath));
			}
		}
	}
}
