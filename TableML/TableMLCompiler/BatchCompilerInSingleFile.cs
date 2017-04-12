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
using System.Linq;
using System.Text.RegularExpressions;
using DotLiquid;

namespace TableML.Compiler
{
    /// <summary>
    /// BatchCompiler , compile
    /// 扩展BatchCopiler：每张表对应一个代码文件
    /// </summary>
    public partial class BatchCompiler
    {
        /// <summary>
        /// 缺省时，默认生成代码存放的路径
        /// </summary>
        public const string DefaultGenCodeDir = "GenCode\\";
        /// <summary>
        /// tab表所有单例类的Manger Class
        /// </summary>
        public const string ManagerClassName = "SettingsManager.cs";
        /// <summary>
        /// 生成代码的文件名=表名+后缀+.cs，建议和模版中的一致
        /// </summary>
        public const string FileNameSuffix = "Setting";
        
        /// <summary>
        /// 生成代码文件
        /// </summary>
        /// <param name="results"></param>
        /// <param name="settingCodeIgnorePattern"></param>
        /// <param name="forceAll"></param>
        /// <param name="genManagerClass"></param>
        /// <param name="templateVars">如果是生成Manager Class 一定要在外部初始化此字段</param>
        void GenSingleClass(TableCompileResult compileResult, string genCodeTemplateString, string genCodeFilePath,
            string nameSpace = "AppSettings", string changeExtension = ".tml", string settingCodeIgnorePattern = null, bool forceAll = false, bool genManagerClass = false, Dictionary<string, TableTemplateVars> templateVars = null)
        {
            //代码文件名=tml的文件名
            // 根据编译结果，构建vars，同class名字的，进行合并
            if (!genManagerClass)
            {
                templateVars = new Dictionary<string, TableTemplateVars>();
            }
            if (!string.IsNullOrEmpty(settingCodeIgnorePattern))
            {
                var ignoreRegex = new Regex(settingCodeIgnorePattern);
                if (ignoreRegex.IsMatch(compileResult.TabFileRelativePath))
                    return; // ignore this 
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

            if (!genManagerClass)
            {
                //首字母大写，符合微软命名规范
                var filenName = compileResult.TabFileRelativePath.Replace(changeExtension, "");
                var newFileName = string.Concat(filenName.Substring(0, 1).ToUpper(),filenName.Substring(1),FileNameSuffix,".cs");
                if (string.IsNullOrEmpty(genCodeFilePath))
                {
                    genCodeFilePath += string.Concat(DefaultGenCodeDir, newFileName);
                }
                else
                {
                    genCodeFilePath += newFileName;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(genCodeFilePath))
                {
                    genCodeFilePath += string.Concat(DefaultGenCodeDir, ManagerClassName);
                }
                else
                {
                    genCodeFilePath += ManagerClassName;
                }
            }


            // 整合成字符串模版使用的List
            var templateHashes = new List<Hash>();
            foreach (var kv in templateVars)
            {
                var templateVar2 = kv.Value;
                var renderTemplateHash = Hash.FromAnonymousObject(templateVar2);
                templateHashes.Add(renderTemplateHash);
            }

            if (forceAll)
            {
                // force 才进行代码编译
                GenerateCode(genCodeTemplateString, genCodeFilePath, nameSpace, templateHashes);
            }
        }


        void GenManagerClass(List<TableCompileResult> results, string genCodeTemplateString, string genCodeFilePath,
            string nameSpace = "AppSettings", string changeExtension = ".tml", string settingCodeIgnorePattern = null, bool forceAll = false)
        {
            //保存所有的编译结果，用来生成ManagerClass
            var templateVars = new Dictionary<string, TableTemplateVars>();
            foreach (var compileResult in results)
            {
                GenSingleClass(compileResult, genCodeTemplateString, genCodeFilePath, nameSpace, changeExtension, settingCodeIgnorePattern, forceAll, true, templateVars);
            }
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
        public List<TableCompileResult> CompileTableMLAllInSingleFile(string sourcePath, string compilePath,
            string genCodeFilePath, string genCodeTemplateString = null, string nameSpace = "AppSettings",
            string changeExtension = ".tml", string settingCodeIgnorePattern = null, bool forceAll = false)
        {
            var results = new List<TableCompileResult>();
            var compileBaseDir = compilePath;
            // excel compiler
            var compiler = new Compiler(new CompilerConfig() { ConditionVars = CompileSettingConditionVars });

            var excelExt = new HashSet<string>() { ".xls", ".xlsx", ".tsv" };
            var copyExt = new HashSet<string>() { ".txt" };
            if (Directory.Exists(sourcePath) == false)
            {
#if UNITY_5_3_OR_NEWER

#endif
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
                        /*
                         * NOTE 开始编译Excel 成 tml文件
                         * 每编译一个Excel就生成一个代码文件
                        */
                        var compileToPath = string.Format("{0}/{1}", compileBaseDir,
                            Path.ChangeExtension(relativePath, changeExtension));
                        var srcFileInfo = new FileInfo(excelPath);

                        Console.WriteLine("Compiling Excel to Tab..." +
                                          string.Format("{0} -> {1}", excelPath, compileToPath));

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
                            Console.WriteLine(); //美观一下 打印空白行
                            var compileResult = compiler.Compile(excelPath, compileToPath, compileBaseDir, doCompile);

                            // 添加模板值
                            results.Add(compileResult);

                            var compiledFileInfo = new FileInfo(compileToPath);
                            compiledFileInfo.LastWriteTime = srcFileInfo.LastWriteTime;
                            //仅仅是生成单个Class，只需要当前的CompileResult
                            GenSingleClass(compileResult, genCodeTemplateString, genCodeFilePath, nameSpace, changeExtension, settingCodeIgnorePattern, forceAll);

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

                //生成Manager class
                GenManagerClass(results, DefaultTemplate.GenManagerCodeTemplate, genCodeFilePath, nameSpace, changeExtension, settingCodeIgnorePattern, forceAll);
            }
            finally
            {
                //EditorUtility.ClearProgressBar();
            }
            return results;
        }


    }
}