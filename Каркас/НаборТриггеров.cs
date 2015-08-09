/*
	Менеджер триггеров
*/


using System.Collections.Generic;
using System.Text.RegularExpressions;


using А = Анализатор;


class НаборТриггеров
{
	Dictionary<string, string> словарь = new Dictionary<string, string>(256);

	public Каркас Родитель = null;

	void Вывести(params string[] строки)
	{
		if (Родитель != null)
			Родитель.ЭлементВывода.Добавить(строки);
	}

	public void Очистить()
	{
		if (словарь.Count == 0)
		{
			Вывести("### Нет ни одного триггера.");
			return;
		}
		словарь.Clear();
		Вывести("### Удалены все триггеры.");
	}

	public void Добавить(string шаблон, string реакция)
	{
		this[шаблон] = реакция;
	}

	public string this[string строка]
	{
		get
		{
			Dictionary<string, string>.Enumerator enumerator = словарь.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string шаблон = enumerator.Current.Key;
				if (Родитель != null)
					шаблон = Родитель.Переменные.ПодставитьЗначения(шаблон);
				Match match = Regex.Match(строка, шаблон);
				if (match.Success)
					return match.Result(enumerator.Current.Value);
			}
			return null;
		}
		set
		{
			if (строка == "" || строка == null)
			{
				Вывести("### Неправильный шаблон.");
				return;
			}
			if (value == null)
			{
				Удалить(строка);
				return;
			}
			словарь[строка] = value;
			string сообщение = "### Теперь при получении строки, соответствующей шаблону \"" + строка + "\" выполняется ";
			сообщение += "\"" + value + "\".";
			Вывести(сообщение);
		}
	}

	public void Удалить(string шаблон)
	{
		if (!словарь.ContainsKey(шаблон))
		{
			Вывести("### Такого триггера не существует.");
			return;
		}
		словарь.Remove(шаблон);
		Вывести("### Триггер удален.");
	}

	public string[] ПолучитьСписок()
	{
		List<string> результат = new List<string>(словарь.Count);
		Dictionary<string, string>.Enumerator enumerator = словарь.GetEnumerator();
		while (enumerator.MoveNext())
		{
			string строка = А.КомандныйСимвол + "триггер " + А.НачалоБлока + enumerator.Current.Key + А.КонецБлока;
			строка += " " + А.НачалоБлока + enumerator.Current.Value + А.КонецБлока;
			результат.Add(строка);
		}
		return результат.ToArray();
	}

	public void ВывестиСписок()
	{
		if (словарь.Count == 0)
		{
			Вывести("### Нет ни одного триггера.");
			return;
		}
		string[] список = ПолучитьСписок();
		for (int i = 0; i < список.Length; i++)
			Вывести("### " + список[i] + ".");
	}
}
