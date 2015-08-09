/*
	Утилита для преобразования логов в HTML и очистки их от цветовых кодов и пауз
*/


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;


[StructLayout(LayoutKind.Sequential)]
public class Win32FindData 
{
	public uint FileAttributes = 0;
	public uint CreationTime_LowDateTime = 0;
	public uint CreationTime_HighDateTime = 0;
	public uint LastAccessTime_LowDateTime = 0;
	public uint LastAccessTime_HighDateTime = 0;
	public uint LastWriteTime_LowDateTime = 0;
	public uint LastWriteTime_HighDateTime = 0;
	public uint FileSizeHigh = 0;
	public uint FileSizeLow = 0;
	public uint Reserved0 = 0;
	public uint Reserved1 = 0;
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	public string FileName = null;
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
	public string AlternateFileName = null;
}


static class Преобразователь
{
	static readonly Encoding кодировка = Encoding.GetEncoding(1251);

	[DllImport("Kernel32.dll")]
	static extern IntPtr FindFirstFile(string fileName, [In, Out] Win32FindData findFileData);

	[DllImport("Kernel32.dll")]
	static extern bool FindNextFile(IntPtr findFile, [In, Out] Win32FindData findFileData);

	[DllImport("Kernel32.dll")]
	static extern bool FindClose(IntPtr findFile);

	static string[] НайтиФайлы(string маска)
	{
		string путь;
		try
		{
			путь = Path.GetDirectoryName(маска);
		}
		catch
		{
			return new string[0];
		}
		Win32FindData fd = new Win32FindData();
		IntPtr handle = FindFirstFile(маска, fd);
		if (handle == new IntPtr(-1))
			return new string[0];
		List<string> файлы = new List<string>();
		while (true)
		{
			файлы.Add(Path.Combine(путь, fd.FileName));
			if (!FindNextFile(handle, fd))
				break;
		}
		FindClose(handle);
		return файлы.ToArray();
	}

	static void ОчиститьЛог(string файл)
	{
		Console.WriteLine("Файл: " + файл);
		StreamWriter sw = null;
		try
		{
			string[] линии = File.ReadAllLines(файл, кодировка);
			int i = 1;
			while(true)
			{
				string новоеИмя = файл + ".old";
				if (i != 1)
					новоеИмя += i;
				if (!File.Exists(новоеИмя))
				{
					File.Move(файл, новоеИмя);
					break;
				}
				i++;
			}
			sw = new StreamWriter(файл, false, кодировка);
			for (i = 0; i < линии.Length; i++)
			{
				string линия = линии[i];
				линия = Regex.Replace(линия, @"^[\da-fA-F]{4} ", "");
				линия = Regex.Replace(линия, @"\u001B[\da-fA-F]{2}", "");
				sw.WriteLine(линия);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}
		if (sw != null)
			sw.Close();
	}

	static void ПреобразоватьЛог(string файл)
	{
		Console.WriteLine("Файл: " + файл);
		StreamWriter sw = null;
		try
		{
			string[] линии = File.ReadAllLines(файл, кодировка);
			файл = Path.Combine(Path.GetDirectoryName(файл), Path.GetFileNameWithoutExtension(файл));
			string новыйФайл;
			int i = 1;
			while(true)
			{
				новыйФайл = файл;
				if (i != 1)
					новыйФайл += " (" + i + ")";
				новыйФайл += ".htm";
				if (!File.Exists(новыйФайл))
					break;
				i++;
			}
			sw = new StreamWriter(новыйФайл, false, кодировка);
			sw.WriteLine("<html><head>");
			sw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1251\">");
			sw.WriteLine("<link href=\"mystyles.css\" rel=\"stylesheet\" type=\"text/css\">");
			sw.Write("</head><body><pre><a class=07>");
			for (i = 0; i < линии.Length; i++)
			{
				string линия = линии[i];
				линия = линия.Replace("&", "&amp;");
				линия = линия.Replace("<", "&lt;");
				линия = линия.Replace(">", "&gt;");
				линия = линия.Replace("\"", "&quot;");
				линия = Regex.Replace(линия, @"^[\da-fA-F]{4} ", "");
				линия = Regex.Replace(линия, @"\u001B([\da-fA-F]{2})", "</a><a class=$1>");
				sw.WriteLine(линия);
			}
			sw.WriteLine("</a></pre></body></html>");
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}
		if (sw != null)
			sw.Close();
	}

	static void Main(string[] аргументы)
	{
		if (аргументы.Length < 2 || (аргументы[0] != "о" && аргументы[0] != "О" && аргументы[0] != "п" && аргументы[0] != "П"))
		{
			Console.WriteLine("Утилита для преобразования логов в HTML и очистки их от цветовых кодов и пауз");
			Console.WriteLine("Таблицу стилей для HTML можно создать с помощью программы Стили");
			Console.WriteLine();
			Console.WriteLine("Использование:");
			Console.WriteLine("    Преобразователь.exe <п|о|?> <маска файлов>[ <маска файлов>[ ...]]");
			Console.WriteLine("    п - преобразование логов в HTML");
			Console.WriteLine("    о - очистка логов");
			Console.WriteLine("    ? - вывод этой справки");
			Console.WriteLine();
			Console.WriteLine("Примечание:");
			Console.WriteLine("    Если в маске имеются пробелы, заключите ее в кавычки");
			Console.WriteLine();
			Console.WriteLine("Примеры:");
			Console.WriteLine("    Преобразователь.exe п *.txt");
			Console.WriteLine("    Преобразователь.exe о Зонинг.txt C:\\Логи\\*.txt");
			Console.WriteLine("    Преобразователь.exe П \"D:\\Мои логи\\*.txt\" C:/Журналы/*.txt");
			Console.WriteLine("    Преобразователь.exe О \"Гифур 2007.01.*.*\"");
			return;
		}
		for (int i = 1; i < аргументы.Length; i++)
		{
			Console.WriteLine("Маска: " + аргументы[i]);
			string[] файлы = НайтиФайлы(аргументы[i]);
			if (файлы.Length == 0)
			{
				Console.WriteLine("Не найдено ни одного файла, соответствующего этой маске.");
				continue;
			}
			if (аргументы[0] == "п" || аргументы[0] == "П")
			{
				foreach (string файл in файлы)
					ПреобразоватьЛог(файл);
			}
			else
			{
				foreach (string файл in файлы)
					ОчиститьЛог(файл);
			}
		}
	}
}
