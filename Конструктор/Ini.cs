/*
	–абота с ini-файлами
*/


using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


class Ini
{
	[DllImport("Kernel32.dll")]
	static extern bool WritePrivateProfileString(string section, string key, string value, string file);

	[DllImport("Kernel32.dll")]
	static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder value, int maxValueLen, string file);

	static string ”точнитьѕуть(string путь)
	{
		if (путь == "" || путь == null)
			путь = " онструктор.ini";
		if (!Path.IsPathRooted(путь))
			путь = Path.Combine(Application.StartupPath, путь);
		return путь;
	}

	public static void —охранить(string путь, string секци€, string им€, string значение)
	{
		WritePrivateProfileString(секци€, им€, значение, ”точнитьѕуть(путь));
	}

	public static string «агрузить(string путь, string секци€, string им€, string значениеѕо”молчанию)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		GetPrivateProfileString(секци€, им€, значениеѕо”молчанию, stringBuilder, stringBuilder.Capacity, ”точнитьѕуть(путь));
		return stringBuilder.ToString();
	}

	public static void —охранить(string путь, string секци€, string им€, int значение)
	{
		—охранить(путь, секци€, им€, значение.ToString());
	}

	public static int «агрузить(string путь, string секци€, string им€, int значениеѕо”молчанию)
	{
		string строка = «агрузить(путь, секци€, им€, значениеѕо”молчанию.ToString());
		try
		{
			return int.Parse(строка);
		}
		catch
		{
			return значениеѕо”молчанию;
		}
	}

	public static void —охранить(string путь, string секци€, string им€, bool значение)
	{
		—охранить(путь, секци€, им€, значение ? 1 : 0);
	}

	public static bool «агрузить(string путь, string секци€, string им€, bool значениеѕо”молчанию)
	{
		int число = «агрузить(путь, секци€, им€, значениеѕо”молчанию ? 1 : 0);
		return число != 0;
	}

	public static void —охранить(string путь, string секци€, string им€, FormWindowState значение)
	{
		—охранить(путь, секци€, им€, (int)значение);
	}

	public static FormWindowState «агрузить(string путь, string секци€, string им€, FormWindowState значениеѕо”молчанию)
	{
		int число = «агрузить(путь, секци€, им€, (int)значениеѕо”молчанию);
		if (число < 0 || число > 2)
			return значениеѕо”молчанию;
		return (FormWindowState)число;
	}
}