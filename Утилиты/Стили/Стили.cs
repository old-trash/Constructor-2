/*
	Утилита для создания таблицы стилей
*/


using System.IO;
using System.Text;


static class Стили
{
	static string[] цвета = new string[16]
	{
		//RRGGBB
		"#000000",
		"#C00000",
		"#00C000",
		"#C0C000",
		"#0000C0",
		"#C000C0",
		"#00C0C0",
		"#C0C0C0",
		"#808080",
		"#FF0000",
		"#00FF00",
		"#FFFF00",
		"#0000FF",
		"#FF00FF",
		"#00FFFF",
		"#FFFFFF",
	};

	static void Main()
	{
		StreamWriter sw = new StreamWriter("mystyles.css", false, Encoding.ASCII);
		sw.WriteLine("body {background: #000000}");
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
				sw.WriteLine("a." + i.ToString("X") + j.ToString("X") + " {font-family: fixedsys; color: " + цвета[j] + "; background: " + цвета[i] + "}");
		}
		sw.Close();
	}
}
