using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;

public partial class FileHelper
{
	/// <summary>
	/// 递归获取所有的目录
	/// </summary>
	/// <param name="strPath"></param>
	/// <param name="lstDirect"></param>
	public static void GetAllDirectorys(string strPath, ref List<string> lstDirect)
	{
		if (Directory.Exists(strPath) == false)
		{
			Console.WriteLine("请检查，路径不存在：{0}", strPath);
			return;
		}
		DirectoryInfo diFliles = new DirectoryInfo(strPath);
		DirectoryInfo[] directories = diFliles.GetDirectories();
		var max = directories.Length;
		for (int dirIdx = 0; dirIdx < max; dirIdx++)
		{
			try
			{
				var dir = directories[dirIdx];
				//dir.FullName是某个子目录的绝对地址，把它记录起来
				lstDirect.Add(dir.FullName);
				GetAllDirectorys(dir.FullName, ref lstDirect);
			}
			catch
			{
				continue;
			}
		}
	}

	/// <summary>  
	/// 遍历当前目录及子目录，获取所有文件 
	/// </summary>  
	/// <param name="strPath">文件路径</param>  
	/// <returns>所有文件</returns>  
	public static IList<FileInfo> GetAllFiles(string strPath)
	{
		List<FileInfo> lstFiles = new List<FileInfo>();
		List<string> lstDirect = new List<string>();
		lstDirect.Add(strPath);
		DirectoryInfo diFliles = null;
		GetAllDirectorys(strPath, ref lstDirect);

		var max = lstDirect.Count;
		for (int idx = 0; idx < max; idx++)
		{
			try
			{
				diFliles = new DirectoryInfo(lstDirect[idx]);
				lstFiles.AddRange(diFliles.GetFiles());
			}
			catch
			{
				continue;
			}
		}
		return lstFiles;
	}

	/// <summary>
	/// 目录复制
	/// </summary>
	/// <param name="sourceDir">源目录</param>
	/// <param name="targetDir">目标目录</param>
	public static void CopyFolder(string sourceDir, string targetDir)
	{
		DirectoryInfo targetDirInfo;
		if (!Directory.Exists(targetDir))
		{
			targetDirInfo = Directory.CreateDirectory(targetDir);
			Console.WriteLine("目标文件夹不存在，已创建");
		}
		else
		{
			targetDirInfo = new DirectoryInfo(targetDir);
		}

		DirectoryInfo sourceDirInfo = new DirectoryInfo(sourceDir);
		if (sourceDirInfo.Exists == false)
		{
			Console.WriteLine("error:源目录不存在！");
			return;
		}

		if (targetDirInfo.FullName.StartsWith(sourceDirInfo.FullName, StringComparison.CurrentCultureIgnoreCase))
		{
			Console.WriteLine("error:父目录不能拷贝到子目录！");
			return;
		} 

		FileInfo[] files = sourceDirInfo.GetFiles();
		var fileMax = files.Length;
		for (int fileIdx = 0; fileIdx < fileMax; fileIdx++)
		{
			files[fileIdx].CopyTo(Path.Combine(targetDir, files[fileIdx].Name), true);
		}

		DirectoryInfo[] direcInfoArr = sourceDirInfo.GetDirectories();
		var dirMax = direcInfoArr.Length;
		for (int dirIdx = 0; dirIdx < dirMax; dirIdx++)
		{
			CopyFolder(Path.Combine(sourceDir, direcInfoArr[dirIdx].Name), Path.Combine(targetDir, direcInfoArr[dirIdx].Name));
		}
	}

	public static void CopyFile(string sourceFile, string targetFile, bool overwrite = true)
	{
		if (File.Exists(sourceFile) == false)
		{
			Console.WriteLine("{0}找不到", sourceFile);
			return;
		}
		DirectoryInfo targetDir = new DirectoryInfo(targetFile);
		if (targetDir.Parent != null && targetDir.Exists == false)
		{
			Directory.CreateDirectory(targetDir.Parent.FullName);
		}
		File.Copy(sourceFile, targetFile, overwrite);

	}

	/// <summary>
	/// 给文件添加完全控制权限
	/// </summary>
	/// <param name="fileName"></param>
	public static void AddTopPermissionToFile(string fileName)
	{
		//给文件添加"Everyone,Users"用户组的完全控制权限
		FileInfo fileInfo = new FileInfo(fileName);
		System.Security.AccessControl.FileSecurity fileSecurity = fileInfo.GetAccessControl();
		fileSecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));
		fileSecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
		fileInfo.SetAccessControl(fileSecurity);
	}

	/// <summary>
	/// 给文件夹添加完全控制权限
	/// </summary>
	/// <param name="directoryPath"></param>
	public static void AddTopPermissionToDirectory(string directoryPath)
	{
		if (string.IsNullOrEmpty(directoryPath)) return;
		//给文件所在目录添加"Everyone,Users"用户组的完全控制权限
		DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(directoryPath));
		if (directoryInfo == null) return;
		System.Security.AccessControl.DirectorySecurity dirSecurity = directoryInfo.GetAccessControl();
		dirSecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));
		dirSecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
		directoryInfo.SetAccessControl(dirSecurity);
	}

	/// <summary>
	/// 设置文件和文件夹的属性为正常
	/// </summary>
	/// <param name="fileName"></param>
	public static void SetPermissionToNormal(string fileName)
	{
		if (string.IsNullOrEmpty(fileName) || File.Exists(fileName) == false)
		{
			Console.WriteLine("文件不存在");
			return;
		}

		FileInfo fileInfo = new FileInfo(fileName);
		DirectoryInfo directoryInfo = fileInfo.Directory;
		if (directoryInfo != null)
		{
			directoryInfo.Attributes = FileAttributes.Normal;
		}
		fileInfo.Attributes = FileAttributes.Normal;
	}
	
	/// <summary>
	/// 文件大小格式化转换
	/// </summary>
	/// <param name="pContentLength"></param>
	/// <returns></returns>
	public static string FormatSizeType(float pContentLength)
	{
		string type = "";
		float size;
		if (pContentLength >= 1073741824)
		{
			size = pContentLength / 1073741824.00F;
			type = "GB";
		}
		else if (pContentLength >= 1048576)
		{
			size = pContentLength / 1048576.00F;
			type = "MB";
		}
		else
		{
			size = pContentLength / 1024.00F;
			type = "KB";
		}
		return size.ToString("0.0") + type;
	}
}