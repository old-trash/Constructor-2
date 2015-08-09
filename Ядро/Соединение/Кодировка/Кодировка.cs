/*
	Утилита для создания кодовой таблицы
*/


using System.Text;
using System.IO;


static class Кодировка
{
	const int номерКодировки = 1251;

	static void Main()
	{
		Encoding кодировка = Encoding.GetEncoding(номерКодировки);
		StreamWriter sw = new StreamWriter("Cp" + номерКодировки + ".txt", false, Encoding.ASCII);
		sw.WriteLine("char[] cp" + номерКодировки + " = new char[256]");
		sw.WriteLine("{");
		for (int i = 0; i < 16; i++)
		{
			sw.Write("\t");
			for (int j = 0; j < 16; j++)
			{
				byte байт = (byte)(i * 16 + j);
				char символ = кодировка.GetChars(new byte[] { байт })[0];
				string код = ((int)символ).ToString("X4");
				sw.Write("'\\u" + код + "',");
				if (j != 15)
					sw.Write(" ");
			}
			sw.WriteLine();
		}
		sw.WriteLine("};");
		sw.Close();
	}
}

