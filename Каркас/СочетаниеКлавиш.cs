/*
	Комбинация клавиш
*/


using System;
using System.Collections.Generic;


struct СочетаниеКлавиш
{
	static Dictionary<string, Клавиша> словарь = new Dictionary<string, Клавиша>(256);

	static СочетаниеКлавиш()
	{
		string[] имена = Enum.GetNames(typeof(Клавиша));
		foreach (string имя in имена)
		{
			Клавиша код = (Клавиша)Enum.Parse(typeof(Клавиша), имя);
			словарь.Add(имя.ToUpper(), код);
		}
		словарь.Add("`", Клавиша.Тильда);
		словарь.Add("~", Клавиша.Тильда);
		словарь.Add("1", Клавиша.D1);
		словарь.Add("!", Клавиша.D1);
		словарь.Add("2", Клавиша.D2);
		словарь.Add("@", Клавиша.D2);
		словарь.Add("3", Клавиша.D3);
		словарь.Add("#", Клавиша.D3);
		словарь.Add("4", Клавиша.D4);
		словарь.Add("$", Клавиша.D4);
		словарь.Add("5", Клавиша.D5);
		словарь.Add("%", Клавиша.D5);
		словарь.Add("6", Клавиша.D6);
		словарь.Add("^", Клавиша.D6);
		словарь.Add("7", Клавиша.D7);
		словарь.Add("&", Клавиша.D7);
		словарь.Add("8", Клавиша.D8);
		словарь.Add("*", Клавиша.D8);
		словарь.Add("9", Клавиша.D9);
		словарь.Add("(", Клавиша.D9);
		словарь.Add("0", Клавиша.D0);
		словарь.Add(")", Клавиша.D0);
		словарь.Add("-", Клавиша.Минус);
		словарь.Add("_", Клавиша.Минус);
		словарь.Add("=", Клавиша.Плюс);
		словарь.Add("+", Клавиша.Плюс);
		словарь.Add("\\", Клавиша.Backslash);
		словарь.Add("|", Клавиша.Backslash);
		словарь.Add("[", Клавиша.OpenBrack);
		словарь.Add("{", Клавиша.OpenBrack);
		словарь.Add("]", Клавиша.CloseBrack);
		словарь.Add("}", Клавиша.CloseBrack);
		словарь.Add(";", Клавиша.Semicolon);
		словарь.Add(":", Клавиша.Semicolon);
		словарь.Add("'", Клавиша.Quote);
		словарь.Add("\"", Клавиша.Quote);
		словарь.Add(",", Клавиша.Comma);
		словарь.Add("<", Клавиша.Comma);
		словарь.Add(".", Клавиша.Period);
		словарь.Add(">", Клавиша.Period);
		словарь.Add("/", Клавиша.Question);
		словарь.Add("?", Клавиша.Question);
		словарь.Add(" ", Клавиша.Пробел);
	}

	Клавиша клавиша;
	bool ctrl;
	bool alt;
	bool shift;

	public СочетаниеКлавиш(Клавиша клавиша, bool ctrl, bool alt, bool shift)
	{
		if (!Enum.IsDefined(typeof(Клавиша), клавиша))
		{
			this.клавиша = 0;
			this.ctrl = this.alt = this.shift = false;
		}
		else
		{
			this.клавиша = клавиша;
			this.ctrl = ctrl;
			this.alt = alt;
			this.shift = shift;
		}
	}

	public Клавиша Клавиша
	{
		get
		{
			return клавиша;
		}
	}

	public bool Ctrl
	{
		get
		{
			return ctrl;
		}
	}

	public bool Alt
	{
		get
		{
			return alt;
		}
	}

	public bool Shift
	{
		get
		{
			return shift;
		}
	}

	public override string ToString()
	{
		if (!Enum.IsDefined(typeof(Клавиша), клавиша))
			return "НЕТ";
		string результат = "";
		if (Ctrl)
			результат += "CTRL" + Анализатор.РазделительКлавиш;
		if (Alt)
			результат += "ALT" + Анализатор.РазделительКлавиш;
		if (Shift)
			результат += "SHIFT" + Анализатор.РазделительКлавиш;
		результат += клавиша.ToString().ToUpper();
		return результат;
	}

	public СочетаниеКлавиш(string строка)
	{
		клавиша = 0;
		ctrl = alt = shift = false;
		строка = строка.Trim().ToUpper();
		string разделитель = Анализатор.РазделительКлавиш.ToString();
		строка = строка.Replace(разделитель + разделитель, разделитель + "разделитель");
		if (строка.Length == 0)
			return;
		string[] фрагменты = строка.Split(Анализатор.РазделительКлавиш);
		try
		{
			string имяКлавиши = фрагменты[фрагменты.Length - 1];
			if (имяКлавиши == "разделитель")
				имяКлавиши = разделитель;
			if (имяКлавиши == "")
				имяКлавиши = "ПРОБЕЛ";
			клавиша = словарь[имяКлавиши];
		}
		catch
		{
			goto Сброс;
		}
		for (int i = 0; i < фрагменты.Length - 1; i++)
		{
			if (фрагменты[i] == "CTRL")
			{
				if (ctrl)
					goto Сброс;
				ctrl = true;
			}
			else if (фрагменты[i] == "ALT")
			{
				if (alt)
					goto Сброс;
				alt = true;
			}
			else if (фрагменты[i] == "SHIFT")
			{
				if (shift)
					goto Сброс;
				shift = true;
			}
			else
			{
				goto Сброс;
			}
		}
		return;
	Сброс:
		клавиша = 0;
		ctrl = alt = shift = false;
	}

	public static bool operator ==(СочетаниеКлавиш сочетание1, СочетаниеКлавиш сочетание2)
	{
		if (сочетание1.Клавиша != сочетание2.Клавиша)
			return false;
		if (сочетание1.Ctrl != сочетание2.Ctrl)
			return false;
		if (сочетание1.Alt != сочетание2.Alt)
			return false;
		if (сочетание1.Shift != сочетание2.Shift)
			return false;
		return true;
	}

	public static bool operator !=(СочетаниеКлавиш сочетание1, СочетаниеКлавиш сочетание2)
	{
		return !(сочетание1 == сочетание2);
	}

	public override bool Equals(object obj)
	{
		try 
		{
			return (this == (СочетаниеКлавиш)obj);
		}
		catch 
		{
			return false;
		}
	}

	public override int GetHashCode()
	{
		int код = (int)клавиша;
		if (ctrl)
			код |= 0x200;
		if (alt)
			код |= 0x400;
		if (shift)
			код |= 0x800;
		return код;
	}
}
