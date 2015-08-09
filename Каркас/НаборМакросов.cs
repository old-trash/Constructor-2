/*
	Менеджер макросов
*/


using System.Collections.Generic;
using System.Text.RegularExpressions;


using А = Анализатор;


class НаборМакросов
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
			Вывести("### Нет ни одного макроса.");
			return;
		}
		словарь.Clear();
		Вывести("### Удалены все макросы.");
	}

	public void Добавить(string макрос, string команды)
	{
		this[макрос] = команды;
	}

	public string this[string макрос]
	{
		get
		{
			Dictionary<string, string>.Enumerator enumerator = словарь.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Match match = Regex.Match(макрос, enumerator.Current.Key);
				if (match.Success)
					return match.Result(enumerator.Current.Value);
			}
			return null;
		}
		set
		{
			if (макрос == "" || макрос == null)
			{
				Вывести("### Неправильный макрос.");
				return;
			}
			if (value == null)
			{
				Удалить(макрос);
				return;
			}
			словарь[макрос] = value;
			string сообщение = "### Теперь вводе строки, соответствующей шаблону \"" + макрос + "\" выполняется ";
			сообщение += "\"" + value + "\".";
			Вывести(сообщение);
		}
	}

	public void Удалить(string макрос)
	{
		if (!словарь.ContainsKey(макрос))
		{
			Вывести("### Такого макроса не существует.");
			return;
		}
		словарь.Remove(макрос);
		Вывести("### Макрос удален.");
	}

	public string[] ПолучитьСписок()
	{
		List<string> результат = new List<string>(словарь.Count);
		Dictionary<string, string>.Enumerator enumerator = словарь.GetEnumerator();
		while (enumerator.MoveNext())
		{
			string строка = А.КомандныйСимвол + "макрос " + А.НачалоБлока + enumerator.Current.Key + А.КонецБлока;
			строка += " " + А.НачалоБлока + enumerator.Current.Value + А.КонецБлока;
			результат.Add(строка);
		}
		return результат.ToArray();
	}

	public void ВывестиСписок()
	{
		if (словарь.Count == 0)
		{
			Вывести("### Нет ни одного макроса.");
			return;
		}
		string[] список = ПолучитьСписок();
		for (int i = 0; i < список.Length; i++)
			Вывести("### " + список[i] + ".");
	}
}