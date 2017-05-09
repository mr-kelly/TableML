using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotLiquid;

namespace TableML.Compiler
{
    /// <summary>
    /// 用于liquid模板
    /// </summary>
    public class TableTemplateVars
    {
        public delegate string CustomClassNameDelegate(string originClassName, string filePath);

        /// <summary>
        /// You can custom class name
        /// </summary>
        public static TableTemplateVars.CustomClassNameDelegate CustomClassNameFunc;

        public List<string> RelativePaths = new List<string>();

        /// <summary>
        ///  构建成一个数组"aaa", "bbb"
        /// </summary>
        public string TabFilePaths
        {
            get
            {
                var paths = "\"" + string.Join("\", \"", RelativePaths.ToArray()) + "\"";
                return paths;
            }
        }

        public string TabFileNames { get; set; }

        public string ClassName { get; set; }
        public List<TableColumnVars> FieldsInternal { get; set; } // column + type

        public string PrimaryKey { get; set; }

        public List<Hash> Fields
        {
            get { return (from f in FieldsInternal select Hash.FromAnonymousObject(f)).ToList(); }
        }

        /// <summary>
        /// Get primary key, the first column field
        /// </summary>
        public Hash PrimaryKeyField
        {
            get
            {   //如果加上判断会导致部分生成的 ParsePrimaryKey() 没有类型
                //if (Fields != null && Fields.Count > 0)
                //{
                //    return Fields[0];
                //}
                //return null;
                return Fields[0];
            }
        }

        /// <summary>
        /// Custom extra strings
        /// </summary>
        public string Extra { get; private set; }

        public List<Hash> Columns2DefaultValus { get; set; } // column + Default Values

        public TableTemplateVars(TableCompileResult compileResult, string extraString)
            : base()
        {
            var tabFileRelativePath = compileResult.TabFileRelativePath;
            RelativePaths.Add(compileResult.TabFileRelativePath);

            TabFileNames = compileResult.ExcelFile.ExcelFileName;
            ClassName = DefaultClassNameParse(tabFileRelativePath);
            // 可自定义Class Name
            if (CustomClassNameFunc != null)
                ClassName = CustomClassNameFunc(ClassName, tabFileRelativePath);

            FieldsInternal = compileResult.FieldsInternal;
            PrimaryKey = compileResult.PrimaryKey;
            Columns2DefaultValus = new List<Hash>();

            Extra = extraString;
        }

        /// <summary>
        /// get a class name from tab file path, default strategy
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string DefaultClassNameParse(string tabFilePath)
        {
            // 未处理路径的类名, 去掉后缀扩展名
            var classNameOrigin = Path.ChangeExtension(tabFilePath, null);

            // 子目录合并，首字母大写, 组成class name
            var className = classNameOrigin.Replace("/", "_").Replace("\\", "_");
            className = className.Replace(" ", "");
            className = string.Join("", (from name
                    in className.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                    select (name[0].ToString().ToUpper() + name.Substring(1, name.Length - 1)))
                .ToArray());

            // 去掉+或#号后面的字符
            var plusSignIndex = className.IndexOf("+");
            className = className.Substring(0, plusSignIndex == -1 ? className.Length : plusSignIndex);
            plusSignIndex = className.IndexOf("#");
            className = className.Substring(0, plusSignIndex == -1 ? className.Length : plusSignIndex);

            return className;

        }
    }
}