/*
	‘ункции дл€ разбора строк
*/


using System;
using System.Collections.Generic;
using System.Text;


static class јнализатор
{
	public const char –азделитель лавиш = '+';
	public const char –азделитель оманд = ';';
	public const char ЌачалоЅлока = '{';
	public const char  онецЅлока = '}';
	public const char  омандный—имвол = '#';

	public static bool —опоставить(string строка, string образец)
	{
		if (строка == null || образец == null || строка == "" || образец == "")
			return false;
		if (строка.Length > образец.Length)
			return false;
		return (string.Compare(строка, 0, образец, 0, строка.Length, true) == 0);
	}

	public static bool —равнить(string строка1, string строка2)
	{
		return (string.Compare(строка1, строка2, true) == 0);
	}

	public static string[] –азделить оманды(string строка)
	{
		List<string> результат = new List<string>(8);
		StringBuilder команда = new StringBuilder(64);
		int вложенностьЅлока = 0;
		bool копировать—ледующий—имвол = false;
		for (int i = 0; i < строка.Length; i++)
		{
			if (копировать—ледующий—имвол)
			{
				команда.Append(строка[i]);
				копировать—ледующий—имвол = false;
				continue;
			}
			if (строка[i] == –азделитель оманд && вложенностьЅлока == 0)
			{
				результат.Add(команда.ToString());
				команда = new StringBuilder(64);
				continue;
			}
			команда.Append(строка[i]);
			if (строка[i] == '\\')
				копировать—ледующий—имвол = true;
			else if (строка[i] == ЌачалоЅлока)
				вложенностьЅлока++;
			else if (строка[i] ==  онецЅлока)
				вложенностьЅлока = Math.Max(0, вложенностьЅлока - 1);
		}
		результат.Add(команда.ToString());
		return результат.ToArray();
	}

	public static void –азделить оманду(string команда, out string действие, out string аргументы)
	{
		действие = команда.Trim();
		аргументы = "";
		int позици€ = -1;
		for (int i = 0; i < действие.Length; i++)
		{
			if (действие[i] == ' ' || действие[i] == ЌачалоЅлока)
			{
				позици€ = i;
				break;
			}
		}
		if (позици€ != -1)
		{
			аргументы = действие.Substring(позици€).Trim();
			действие = действие.Substring(0, позици€);
		}
	}

	public static string[] –азделитьјргументы(string строка)
	{
		List<string> результат = new List<string>(8);
		StringBuilder аргумент = new StringBuilder(64);
		int вложенностьЅлока = 0;
		bool копировать—ледующий—имвол = false;
		for (int i = 0; i < строка.Length; i++)
		{
			if (копировать—ледующий—имвол)
			{
				аргумент.Append(строка[i]);
				копировать—ледующий—имвол = false;
				continue;
			}
			if (строка[i] == '\\')
			{
				аргумент.Append(строка[i]);
				копировать—ледующий—имвол = true;
				continue;
			}
			if (строка[i] == ЌачалоЅлока)
			{
				if (вложенностьЅлока > 0)
				{
					аргумент.Append(строка[i]);
				}
				else if (аргумент.Length != 0)
				{
					результат.Add(аргумент.ToString());
					аргумент = new StringBuilder(64);
				}
				вложенностьЅлока++;
				continue;
			}
			if (строка[i] ==  онецЅлока)
			{
				вложенностьЅлока--;
				if (вложенностьЅлока < 0)
				{
					вложенностьЅлока = 0;
					аргумент.Append(строка[i]);
				}
				else if (вложенностьЅлока > 0)
				{
					аргумент.Append(строка[i]);
				}
				else
				{
					результат.Add(аргумент.ToString());
					аргумент = new StringBuilder(64);
				}
				continue;
			}
			if (строка[i] == ' ')
			{
				if (вложенностьЅлока > 0)
				{
					аргумент.Append(строка[i]);
				}
				else if (аргумент.Length != 0)
				{
					результат.Add(аргумент.ToString());
					аргумент = new StringBuilder(64);
				}
				continue;
			}
			аргумент.Append(строка[i]);
		}
		if (аргумент.Length != 0)
			результат.Add(аргумент.ToString());
		return результат.ToArray();
	}

	public static string ”братьќбратные—лэши(string строка)
	{
		StringBuilder результат = new StringBuilder(строка.Length);
		bool копировать—ледующий—имвол = false;
		for (int i = 0; i < строка.Length; i++)
		{
			if (копировать—ледующий—имвол)
			{
				результат.Append(строка[i]);
				копировать—ледующий—имвол = false;
				continue;
			}
			if (строка[i] == '\\')
			{
				копировать—ледующий—имвол = true;
				continue;
			}
			результат.Append(строка[i]);
		}
		if (копировать—ледующий—имвол)
			результат.Append('\\');
		return результат.ToString();
	}

	public static bool явл€етс€„ислом(string строка)
	{
		try
		{
			int.Parse(строка);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static bool явл€ютс€„ислами(params string[] строки)
	{
		foreach (string строка in строки)
		{
			if (!явл€етс€„ислом(строка))
				return false;
		}
		return true;
	}
}
