/*
	Менеджер горячих клавиш
*/


using System.Collections.Generic;


using А = Анализатор;


class НаборГорячихКлавиш
{
	Dictionary<СочетаниеКлавиш, string> словарь = new Dictionary<СочетаниеКлавиш, string>(256);

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
			Вывести("### Нет назначенных горячих клавиш.");
			return;
		}
		словарь.Clear();
		Вывести("### Удалены все горячие клавиши.");
	}

	public void Добавить(СочетаниеКлавиш сочетание, string команды)
	{
		this[сочетание] = команды;
	}

	public void Добавить(Клавиша клавиша, bool ctrl, bool alt, bool shift, string команды)
	{
		this[new СочетаниеКлавиш(клавиша, ctrl, alt, shift)] = команды;
	}

	public void Добавить(string сочетание, string команды)
	{
		this[new СочетаниеКлавиш(сочетание)] = команды;
	}

	public string this[СочетаниеКлавиш сочетание]
	{
		get
		{
			try
			{
				return словарь[сочетание];
			}
			catch
			{
				return null;
			}
		}
		set
		{
			if (сочетание.Клавиша == 0)
			{
				Вывести("### Неправильная комбинация клавиш.");
				return;
			}
			if (value == null)
			{
				Удалить(сочетание);
				return;
			}
			словарь[сочетание] = value;
			string сообщение = "### Теперь при нажатии " + сочетание.ToString() + " выполняется ";
			сообщение += "\"" + value + "\".";
			Вывести(сообщение);
		}
	}

	public void Удалить(СочетаниеКлавиш сочетание)
	{
		if (!словарь.ContainsKey(сочетание))
		{
			Вывести("### Такой горячей клавиши не существует.");
			return;
		}
		словарь.Remove(сочетание);
		string сообщение = "### При нажатии " + сочетание.ToString() + " теперь ничего не выполняется.";
		Вывести(сообщение);
	}

	public void Удалить(Клавиша клавиша, bool ctrl, bool alt, bool shift)
	{
		Удалить(new СочетаниеКлавиш(клавиша, ctrl, alt, shift));
	}

	public void Удалить(string сочетание)
	{
		Удалить(new СочетаниеКлавиш(сочетание));
	}

	public string[] ПолучитьСписок()
	{
		List<string> результат = new List<string>(словарь.Count);
		Dictionary<СочетаниеКлавиш, string>.Enumerator enumerator = словарь.GetEnumerator();
		while (enumerator.MoveNext())
		{
			string строка = А.КомандныйСимвол + "сочетаниеКлавиш " + enumerator.Current.Key.ToString();
			строка += " " + А.НачалоБлока + enumerator.Current.Value + А.КонецБлока;
			результат.Add(строка);
		}
		return результат.ToArray();
	}

	public void ВывестиСписок()
	{
		if (словарь.Count == 0)
		{
			Вывести("### Нет назначенных горячих клавиш.");
			return;
		}
		string[] список = ПолучитьСписок();
		for (int i = 0; i < список.Length; i++)
			Вывести("### " + список[i] + ".");
	}
}
