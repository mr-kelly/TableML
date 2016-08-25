using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableML;

namespace TableMLTests
{
    [TestClass]
    public class TableMLTest
    {
        public string TableString1 = @"Id	Value
int	string
1	hi
2	f
3	abc 
4	temp
";
        public string TableString2 = @"Id	Value
int	string
10	hi
20	f
30	abc 
40	temp
";
        public string TableString1Plus2 = @"Id	Value
int	string
1	hi
2	f
3	abc 
4	temp
10	hi
20	f
30	abc 
40	temp
";
        [TestMethod]
        public void TestLoad()
        {
            var table = TableFile.LoadFromString(TableString1, TableString2);
            var row = table.GetByPrimaryKey("1");
            Assert.AreEqual("hi", row["Value"]);
        }

        /// <summary>
        /// 传入多个Table
        /// </summary>
        [TestMethod]
        public void TestMultString()
        {
            var table = TableFile.LoadFromString(TableString1, TableString2);
            var row = table.GetByPrimaryKey("1");
            Assert.AreEqual("hi", row["Value"]);
        }
        [TestMethod]
        public void TestMultStringTableObj()
        {
            var tableObj = TableObject.LoadFromString(TableString1, TableString2);
            var objRow = tableObj.GetByPrimaryKey(10);
            Assert.AreEqual("hi", objRow["Value"]);
        }
        [TestMethod]
        public void TestWrite()
        {
            var tableObj = TableFile.LoadFromString(TableString1, TableString2);
            var writer = new TableFileWriter(tableObj);
            Assert.AreEqual(true, writer.Save("write.txt"));
            var result = File.ReadAllText("write.txt");
            var expect = TableString1Plus2.Replace("\r", "");
            Assert.AreEqual(expect, result);

        }
    }
}
