/*
	Менеджер переменных
*/


using System;
using System.Collections.Generic;


using А = Анализатор;


class НаборПеременных
{
	Dictionary<string, string> словарь = new Dictionary<string, string>(256);
	string имя = "";

	public Каркас Родитель = null;

	void Вывести(params string[] строки)
	{
		if (Родитель != null)
			Родитель.ЭлементВывода.Добавить(строки);
	}

	public int Количество
	{
		get
		{
			return словарь.Count;
		}
	}

	public string Имя
	{
		get
		{
			if (имя == null)
				return "";
			return имя;
		}
		set
		{
			имя = value;
			if (имя == null)
				имя = "";
			Вывести("### Переменной \"ИМЯ\" присвоено значение \"" + имя + "\".");
			if (Родитель != null)
				Родитель.Text = имя;
		}
	}

	public void Очистить()
	{
		if (словарь.Count == 0)
		{
			Вывести("### Нет ни одной переменной.");
			return;
		}
		словарь.Clear();
		Вывести("### Удалены все переменные.");
	}

	public void Добавить(string название, string значение)
	{
		this[название] = значение;
	}

	public string this[string название]
	{
		get
		{
			try
			{
				return словарь[название];
			}
			catch
			{
				return null;
			}
		}
		set
		{
			if (название == null || название == "")
			{
				Вывести("### Неправильное название переменной.");
				return;
			}
			if (value == null)
			{
				Удалить(название);
				return;
			}
			словарь[название] = value;
			Вывести("### Переменной \"" + название + "\" присвоено значение \"" + value + "\".");
		}
	}

	public void Удалить(string название)
	{
		if (!словарь.ContainsKey(название))
		{
			Вывести("### Такой переменной не существует.");
			return;
		}
		словарь.Remove(название);
		Вывести("### Переменная \"" + название + "\" удалена.");
	}

	public string[] ПолучитьСписок()
	{
		List<string> результат = new List<string>(словарь.Count);
		Dictionary<string, string>.Enumerator enumerator = словарь.GetEnumerator();
		while (enumerator.MoveNext())
		{
			string строка = А.КомандныйСимвол + "переменная " + А.НачалоБлока + enumerator.Current.Key + А.КонецБлока;
			строка += " " + А.НачалоБлока + enumerator.Current.Value + А.КонецБлока;
			результат.Add(строка);
		}
		return результат.ToArray();
	}

	public void ВывестиСписок()
	{
		if (словарь.Count == 0)
		{
			Вывести("### Нет ни одной переменной.");
			return;
		}
		string[] список = ПолучитьСписок();
		for (int i = 0; i < список.Length; i++)
			Вывести("### " + список[i] + ".");
	}

	public string ПодставитьЗначения(string строка)
	{
		if (строка == null || строка == "")
			return строка;
		строка = строка.Replace("$ИМЯ", Имя);
		строка = строка.Replace("$ДАТА", DateTime.Now.ToString("yyyy.MM.dd"));
		строка = строка.Replace("$ВРЕМЯ", DateTime.Now.ToString("HH-mm-ss"));
		Dictionary<string, string>.Enumerator enumerator = словарь.GetEnumerator();
		while (enumerator.MoveNext())
			строка = строка.Replace("$" + enumerator.Current.Key, enumerator.Current.Value);
		return строка;
	}
}
