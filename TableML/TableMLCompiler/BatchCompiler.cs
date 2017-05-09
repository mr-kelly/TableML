#region Copyright (c) 2015 KEngine / Kelly <http://github.com/mr-kelly>, All rights reserved.

// KEngine - Toolset and framework for Unity3D
// ===================================
// 
// Filename: SettingModuleEditor.cs
// Date:     2015/12/03
// Author:  Kelly
// Email: 23110388@qq.com
// Github: https://github.com/mr-kelly/KEngine
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3.0 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DotLiquid;

namespace TableML.Compiler
{
    /// <summary>
    /// BatchCompiler, compile
    /// </summary>
    public partial class BatchCompiler
    {
        /// <summary>
        /// 是否自动在编译配置表时生成静态代码，如果不需要，外部设置false
        /// </summary>
        //public static bool AutoGenerateCode = true;

        /// <summary>
        /// 当生成的类名，包含数组中字符时，不生成代码
        /// </summary>
        /// <example>
        /// GenerateCodeFilesFilter = new []
        /// {
        ///     "SubdirSubSubDirExample3",
        /// };
        /// </example>
        public string[] GenerateCodeFilesFilter = null;

        /// <summary>
        /// 条件编译变量
        /// </summary>
        public string[] CompileSettingConditionVars;

        /// <summary>
        /// 可以为模板提供额外生成代码块！返回string即可！
        /// 自定义[InitializeOnLoad]的类并设置这个委托
        /// </summary>
        public CustomExtraStringDelegate CustomExtraString;
        public delegate string CustomExtraStringDelegate(TableCompileResult tableCompileResult);

        /// <summary>
        /// Generate static code from settings
        /// </summary>
        /// <param name="templateString"></param>
        /// <param name="genCodeFilePath"></param>
        /// <param name="nameSpace"></param>
        /// <param name="files"></param>
		void GenerateCode(string templateString, string genCodeFilePath, string nameSpace, List<Hash> files)
        {

            var codeTemplates = new Dictionary<string, string>()
            {
                {templateString, genCodeFilePath},
            };

            foreach (var kv in codeTemplates)
            {
                var templateStr = kv.Key;
                var exportPath = kv.Value;

                // 生成代码
                var template = Template.Parse(templateStr);
                var topHash = new Hash();
                topHash["NameSpace"] = nameSpace;
                topHash["Files"] = files;

                if (!string.IsNullOrEmpty(exportPath))
                {
                    var genCode = template.Render(topHash);
                    if (File.Exists(exportPath)) // 存在，比较是否相同
                    {
                        if (File.ReadAllText(exportPath) != genCode)
                        {
                            //EditorUtility.ClearProgressBar();
                            // 不同，会触发编译，强制停止Unity后再继续写入
                            //if (EditorApplication.isPlaying)
                            {
                                //Console.WriteLine("[CAUTION]AppSettings code modified! Force stop Unity playing");
                                //EditorApplication.isPlaying = false;
                            }
                            File.WriteAllText(exportPath, genCode, Encoding.UTF8);
                            Console.WriteLine("{0} update code file complete", exportPath);
                        }
                    }
                    else
                    {
                        //判断目录是否存在
                        var exportDir = Path.GetDirectoryName(exportPath);
                        if (!string.IsNullOrEmpty(exportDir) && Directory.Exists(exportDir) == false)
                        {
                            Directory.CreateDirectory(exportDir);
                        }
                        File.WriteAllText(exportPath, genCode, Encoding.UTF8);
                        Console.WriteLine("{0} code file gen complete", exportPath);
                    }
                }
            }
            // make unity compile
            //AssetDatabase.Refresh();
        }

        /// <summary>
        /// Compile one directory 's all settings, and return behaivour results
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="compilePath"></param>
        /// <param name="genCodeFilePath"></param>
        /// <param name="changeExtension"></param>
        /// <param name="forceAll">no diff! only force compile will generate code</param>
        /// <returns></returns>
        public List<TableCompileResult> CompileTableMLAll(string sourcePath, string compilePath,
                                                          string genCodeFilePath, string genCodeTemplateString = null, string nameSpace = "AppSettings", string changeExtension = ".tml", string settingCodeIgnorePattern = null, bool forceAll = false)
        {
            var results = new List<TableCompileResult>();
            var compileBaseDir = compilePath;
            // excel compiler
            var compiler = new Compiler(new CompilerConfig() { ConditionVars = CompileSettingConditionVars });

            var excelExt = new HashSet<string>() { ".xls", ".xlsx", ".tsv" };
            var copyExt = new HashSet<string>() { ".txt" };
            if (Directory.Exists(sourcePath) == false)
            {
                Console.WriteLine("Error! {0} 路径不存在！{0}", sourcePath);
                return results;
            }
            var findDir = sourcePath;
            try
            {
                var allFiles = Directory.GetFiles(findDir, "*.*", SearchOption.AllDirectories);
                var allFilesCount = allFiles.Length;
                var nowFileIndex = -1; // 开头+1， 起始为0
                foreach (var excelPath in allFiles)
                {
                    nowFileIndex++;
                    var ext = Path.GetExtension(excelPath);
                    var fileName = Path.GetFileNameWithoutExtension(excelPath);

                    var relativePath = excelPath.Replace(findDir, "").Replace("\\", "/");
                    if (relativePath.StartsWith("/"))
                        relativePath = relativePath.Substring(1);
                    if (excelExt.Contains(ext) && !fileName.StartsWith("~")) // ~开头为excel临时文件，不要读
                    {
                        // it's an excel file

                        var compileToPath = string.Format("{0}/{1}", compileBaseDir,
                            Path.ChangeExtension(relativePath, changeExtension));
                        var srcFileInfo = new FileInfo(excelPath);

                        Console.WriteLine("Compiling Excel to Tab..." + string.Format("{0} -> {1}", excelPath, compileToPath));

                        // 如果已经存在，判断修改时间是否一致，用此来判断是否无需compile，节省时间
                        bool doCompile = true;
                        if (File.Exists(compileToPath))
                        {
                            var toFileInfo = new FileInfo(compileToPath);

                            if (!forceAll && srcFileInfo.LastWriteTime == toFileInfo.LastWriteTime)
                            {
                                //Log.DoLog("Pass!SameTime! From {0} to {1}", excelPath, compileToPath);
                                doCompile = false;
                            }
                        }
                        if (doCompile)
                        {
                            Console.WriteLine("[SettingModule]Compile from {0} to {1}", excelPath, compileToPath);
                            Console.WriteLine();//美观一下 打印空白行
                            var compileResult = compiler.Compile(excelPath, compileToPath, compileBaseDir, doCompile);

                            // 添加模板值
                            results.Add(compileResult);

                            var compiledFileInfo = new FileInfo(compileToPath);
                            compiledFileInfo.LastWriteTime = srcFileInfo.LastWriteTime;

                        }
                    }
                    else if (copyExt.Contains(ext)) // .txt file, just copy
                    {
                        // just copy the files with these ext
                        var compileToPath = string.Format("{0}/{1}", compileBaseDir,
                            relativePath);
                        var compileToDir = Path.GetDirectoryName(compileToPath);
                        if (!Directory.Exists(compileToDir))
                            Directory.CreateDirectory(compileToDir);
                        File.Copy(excelPath, compileToPath, true);

                        Console.WriteLine("Copy File ..." + string.Format("{0} -> {1}", excelPath, compileToPath));
                    }
                }

                // 根据模板生成所有代码,  如果不是强制重建，无需进行代码编译
                //if (!AutoGenerateCode)
                //{
                //	Log.Warning("Ignore Gen Settings code");
                //}
                //else if (!force)
                //{
                //	Log.Warning("Ignore Gen Settings Code, not a forcing compiling");
                //}
                //else
                {

                    // 根据编译结果，构建vars，同class名字的，进行合并
                    var templateVars = new Dictionary<string, TableTemplateVars>();
                    foreach (var compileResult in results)
                    {
                        if (!string.IsNullOrEmpty(settingCodeIgnorePattern))
                        {
                            var ignoreRegex = new Regex(settingCodeIgnorePattern);
                            if (ignoreRegex.IsMatch(compileResult.TabFileRelativePath))
                                continue; // ignore this 
                        }

                        var customExtraStr = CustomExtraString != null ? CustomExtraString(compileResult) : null;

                        var templateVar = new TableTemplateVars(compileResult, customExtraStr);

                        // 尝试类过滤
                        var ignoreThisClassName = false;
                        if (GenerateCodeFilesFilter != null)
                        {
                            for (var i = 0; i < GenerateCodeFilesFilter.Length; i++)
                            {
                                var filterClass = GenerateCodeFilesFilter[i];
                                if (templateVar.ClassName.Contains(filterClass))
                                {
                                    ignoreThisClassName = true;
                                    break;
                                }

                            }
                        }
                        if (!ignoreThisClassName)
                        {
                            if (!templateVars.ContainsKey(templateVar.ClassName))
                                templateVars.Add(templateVar.ClassName, templateVar);
                            else
                            {
                                templateVars[templateVar.ClassName].RelativePaths.Add(compileResult.TabFileRelativePath);
                            }
                        }

                    }

                    // 整合成字符串模版使用的List
                    var templateHashes = new List<Hash>();
                    foreach (var kv in templateVars)
                    {
                        var templateVar = kv.Value;
                        var renderTemplateHash = Hash.FromAnonymousObject(templateVar);
                        templateHashes.Add(renderTemplateHash);
                    }


                    if (forceAll)
                    {
                        // force 才进行代码编译
                        GenerateCode(genCodeTemplateString, genCodeFilePath, nameSpace, templateHashes);
                    }
                }

            }
            finally
            {
                //EditorUtility.ClearProgressBar();
            }
            return results;
        }
    }
}