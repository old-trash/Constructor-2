/*
	Базовый класс для конфига
*/


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;


using А = Анализатор;


partial class Каркас : Ядро
{
	public MainMenu Меню = new MainMenu();
	public НаборПеременных Переменные = new НаборПеременных();
	public НаборГорячихКлавиш ГорячиеКлавиши = new НаборГорячихКлавиш();
	public НаборМакросов Макросы = new НаборМакросов();
	public НаборТриггеров Триггеры = new НаборТриггеров();
	public string Файл = ""; // место хранения профиля

	public Каркас()
	{
		Text = "";
		MenuItem профиль = Меню.MenuItems.Add("Профиль");
		профиль.MergeOrder = 0;
		профиль.MergeType = MenuMerge.MergeItems;
		MenuItem профиль_Сохранить = new MenuItem("Сохранить", new EventHandler(ОбработатьНажатие_Профиль_Сохранить), Shortcut.CtrlS);
		профиль.MenuItems.Add(профиль_Сохранить);
		MenuItem профиль_СохранитьКак = new MenuItem("Сохранить как...", new EventHandler(ОбработатьНажатие_Профиль_СохранитьКак));
		профиль.MenuItems.Add(профиль_СохранитьКак);
		Menu = Меню;
		Переменные.Родитель = this;
		ГорячиеКлавиши.Родитель = this;
		Макросы.Родитель = this;
		Триггеры.Родитель = this;
	}

	public virtual string ОбработатьКоманду(string команда)
	{
		if (команда.Length > 0 && команда[0] == Анализатор.КомандныйСимвол)
		{
			ЭлементВывода.ДобавитьЭхо(команда);
			Интерпретировать(команда.Substring(1));
		}
		else
		{
			string команды = Макросы[команда];
			if (команды != null)
				return команды;
			команда = Анализатор.УбратьОбратныеСлэши(команда);
			ЭлементВывода.ДобавитьЭхо(команда);
			Соединение.Записать(команда);
		}
		return null;
	}

	void РекурсивноОбработатьКоманды(string текст)
	{
		if (текст == null)
			return;
		текст = Переменные.ПодставитьЗначения(текст);
		string[] команды = Анализатор.РазделитьКоманды(текст);
		foreach (string команда in команды)
			РекурсивноОбработатьКоманды(ОбработатьКоманду(команда));
	}

	public void ОбработатьКоманды(string текст)
	{
		lock (this)
		{
			ЭлементВывода.НачатьФормированиеНовогоЭха();
			РекурсивноОбработатьКоманды(текст);
		}
	}

	public override void ОбработатьВведенныйТекст(string текст)
	{
		lock (this)
		{
			ОбработатьКоманды(текст);
			ЭлементВывода.Вывести();
			Соединение.ПередатьДанные();
		}
	}

	public virtual string ОбработатьСтроку(Строка строка, bool статуснаяСтрока)
	{
		ЭлементВывода.Добавить(строка);
		if (строка.Длина > 0)
			return Триггеры[строка.ToString()];
		return null;
	}

	public override void ОбработатьПолученныйТекст(params Строка[] строки)
	{
		foreach (Строка строка in строки)
		{
			if (строка.Длина > 0 && строка[строка.Длина - 1].Значение == ' ')
			{
				строка.УбратьПробелыВКонце();
				ОбработатьКоманды(ОбработатьСтроку(строка, true));
			}
			else
			{
				ОбработатьКоманды(ОбработатьСтроку(строка, false));
			}
		}
		ЭлементВывода.Вывести();
		Соединение.ПередатьДанные();
	}

	public void ВывестиТаблицуЦветов()
	{
		for (byte i = 0; i < 16; i++)
		{
			Строка строка = new Строка(32);
			for (byte j = 0; j < 16; j++)
				строка.Добавить(i.ToString("X") + j.ToString("X"), j, i);
			ЭлементВывода.Добавить(строка);
		}
	}

	public static string ПапкаПрофили
	{
		get
		{
			return Path.Combine(Application.StartupPath, "Профили");
		}
	}

	public static string СократитьПуть(string путь)
	{
		return путь.Replace("/", "\\").Replace(Application.StartupPath + "\\", "");
	}

	public static string УточнитьПуть(string путь)
	{
		if (!Path.IsPathRooted(путь))
			путь = Path.Combine(Application.StartupPath, путь);
		return путь;
	}

	public static void СоздатьПапкуПрофили()
	{
		if (!Directory.Exists(ПапкаПрофили))
			Directory.CreateDirectory(ПапкаПрофили);
	}

	public static string НайтиПрофиль(string файл)
	{
		СоздатьПапкуПрофили();
		if (File.Exists(файл))
			return файл;
		string путь = УточнитьПуть(файл);
		if (File.Exists(путь))
			return путь;
		путь += ".txt";
		if (File.Exists(путь))
			return путь;
		путь = Path.Combine(ПапкаПрофили, файл);
		if (File.Exists(путь))
			return путь;
		путь += ".txt";
		if (File.Exists(путь))
			return путь;
		return null;
	}

	public bool СохранитьПрофиль(string путь)
	{
		СоздатьПапкуПрофили();
		List<string> строки = new List<string>(256);
		if (Переменные.Имя != "")
			строки.Add(string.Format("{0}имя {1}{2}{3}", А.КомандныйСимвол, А.НачалоБлока, Переменные.Имя, А.КонецБлока));
		строки.AddRange(Переменные.ПолучитьСписок());
		строки.AddRange(ГорячиеКлавиши.ПолучитьСписок());
		строки.AddRange(Макросы.ПолучитьСписок());
		строки.AddRange(Триггеры.ПолучитьСписок());
		путь = УточнитьПуть(путь);
		try
		{
			File.WriteAllLines(путь, строки.ToArray(), Encoding.GetEncoding(1251));
		}
		catch
		{
			ЭлементВывода.Добавить("### Не удалось сохранить профиль.");
			return false;
		}
		Файл = СократитьПуть(путь);
		ЭлементВывода.Добавить("### Профиль сохранен в файл \"" + Файл + "\".");
		return true;
	}

	public bool СохранитьПрофильКак()
	{
		СоздатьПапкуПрофили();
		SaveFileDialog saveFileDialog = new SaveFileDialog();
		saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
		saveFileDialog.FilterIndex = 1;
		saveFileDialog.RestoreDirectory = true;
		saveFileDialog.AddExtension = true;
		if (Файл != null && Файл != "")
			saveFileDialog.FileName = Файл;
		else if (Переменные.Имя != "")
			saveFileDialog.FileName = Переменные.Имя;
		saveFileDialog.InitialDirectory = ПапкаПрофили;
		if (saveFileDialog.ShowDialog() == DialogResult.OK)
			return СохранитьПрофиль(saveFileDialog.FileName);
		return false;
	}

	public bool СохранитьПрофиль()
	{
		if (Файл != null && Файл != "" && СохранитьПрофиль(Файл))
			return true;
		return СохранитьПрофильКак();
	}

	public bool Выполнить(string путь, bool загрузкаПрофиля)
	{
		СоздатьПапкуПрофили();
		путь = НайтиПрофиль(путь);
		string[] строки;
		try
		{
			строки = File.ReadAllLines(путь, Encoding.GetEncoding(1251));
		}
		catch
		{
			ЭлементВывода.Добавить("### Не удалось открыть файл.");
			return false;
		}
		путь = СократитьПуть(путь);
		if (загрузкаПрофиля)
			ЭлементВывода.Добавить("### Загрузка профиля \"" + путь + "\"...");
		else
			ЭлементВывода.Добавить("### Выполнение файла \"" + путь + "\"...");
		foreach (string строка in строки)
			ОбработатьКоманды(строка);
		if (загрузкаПрофиля)
		{
			Файл = путь;
			ЭлементВывода.Добавить("### Профиль загружен.");
		}
		else
		{
			ЭлементВывода.Добавить("### Файл выполнен.");
		}
		return true;
	}

	public bool Выполнить(bool загрузкаПрофиля)
	{
		СоздатьПапкуПрофили();
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.InitialDirectory = ПапкаПрофили;
		openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
		openFileDialog.FilterIndex = 1;
		openFileDialog.RestoreDirectory = true;
		if(openFileDialog.ShowDialog() == DialogResult.OK)
			return Выполнить(openFileDialog.FileName, загрузкаПрофиля);
		return false;
	}

	public override bool ОбработатьНажатиеКлавиши(Клавиша клавиша, bool ctrl, bool alt, bool shift)
	{
		СочетаниеКлавиш сочетание = new СочетаниеКлавиш(клавиша, ctrl, alt, shift);
		string команды = ГорячиеКлавиши[сочетание];
		if (команды == null)
			return base.ОбработатьНажатиеКлавиши(клавиша, ctrl, alt, shift);
		ОбработатьВведенныйТекст(команды);
		return true;
	}

	void ОбработатьНажатие_Профиль_Сохранить(object sender, EventArgs e)
	{
		СохранитьПрофиль();
		ЭлементВывода.Вывести();
	}

	void ОбработатьНажатие_Профиль_СохранитьКак(object sender, EventArgs e)
	{
		СохранитьПрофильКак();
		ЭлементВывода.Вывести();
	}
}
