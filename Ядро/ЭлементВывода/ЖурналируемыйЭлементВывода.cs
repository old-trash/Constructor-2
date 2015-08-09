/*
	Класс, реализующий запись и воспроизведение лога
*/


using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;


class ЖурналируемыйЭлементВывода : ЭлементВывода
{
	static readonly string папкаЖурналы = Path.Combine(Application.StartupPath, "Журналы");

	public bool ЗаписыватьПаузы = true;
	public bool ЗаписыватьЦвета = true;

	StreamWriter streamWriter = null;
	List<Строка> записываемыеСтроки = new List<Строка>(256);
	long предыдущееВремя = 0;

	public bool ЖурналОткрыт
	{
		get
		{
			return (streamWriter != null);
		}
	}

	string ДатаИВремя
	{
		get
		{
			return DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss");
		}
	}

	string Дата
	{
		get
		{
			return DateTime.Now.ToString("yyyy.MM.dd");
		}
	}

	void Записать()
	{
		if (!ЖурналОткрыт)
		{
			Вывести("### Журнал не открыт.");
			return;
		}
		long интервал = -1;
		foreach (Строка строка in записываемыеСтроки)
		{
			if (интервал == -1)
			{
				long текущееВремя = DateTime.Now.Ticks;
				интервал = текущееВремя - предыдущееВремя;
				предыдущееВремя = текущееВремя;
				интервал /= 10000;
				if (интервал < 100)
					интервал = 0;
				if (интервал > 0xFFFF)
					интервал = 0xFFFF;
			}
			StringBuilder stringBuilder = new StringBuilder(256);
			if (ЗаписыватьПаузы)
				stringBuilder.Append(интервал.ToString("X4") + " ");
			if (!ЗаписыватьЦвета)
				stringBuilder.Append(строка.ToString());
			else
				stringBuilder.Append(ЗакодироватьСтроку(строка));
			streamWriter.WriteLine(stringBuilder.ToString());
			интервал = 0;
		}
		записываемыеСтроки = new List<Строка>(256);
		streamWriter.Flush();
	}

	public override void Добавить(params Строка[] добавляемыеСтроки)
	{
		lock (this)
		{
			base.Добавить(добавляемыеСтроки);
			if (ЖурналОткрыт)
				записываемыеСтроки.AddRange(добавляемыеСтроки);
		}
	}

	public void ЗакрытьЖурнал()
	{
		if (!ЖурналОткрыт)
		{
			Вывести("### Журнал не открыт.");
			return;
		}
		string строка = "### Конец записи: " + ДатаИВремя + ".";
		if (ЗаписыватьПаузы)
			streamWriter.Write("0000 ");
		if (ЗаписыватьЦвета)
			streamWriter.Write("\u001B07");
		streamWriter.WriteLine(строка);
		streamWriter.Close();
		streamWriter = null;
		записываемыеСтроки = new List<Строка>(256);
		предыдущееВремя = 0;
		Вывести("### Журнал закрыт.");
	}

	public override void Вывести()
	{
		lock (this)
		{
			base.Вывести();
			if (ЖурналОткрыт && записываемыеСтроки.Count > 0)
				Записать();
		}
	}

	string ЗакодироватьСтроку(Строка строка)
	{
		if (строка.Длина == 0)
			return "";
		StringBuilder результат = new StringBuilder(строка.Длина * 2);
		byte цвет = строка[0].Цвет;
		StringBuilder подстрока = new StringBuilder(строка.Длина);
		подстрока.Append(строка[0].Значение);
		for (int i = 1; i < строка.Длина; i++)
		{
			if (строка[i].Цвет != цвет)
			{
				результат.Append("\u001B" + цвет.ToString("X2"));
				результат.Append(подстрока);
				цвет = строка[i].Цвет;
				подстрока = new StringBuilder(строка.Длина);
			}
			подстрока.Append(строка[i].Значение);
		}
		результат.Append("\u001B" + цвет.ToString("X2"));
		результат.Append(подстрока);
		return результат.ToString();
	}

	public void ОткрытьЖурнал(string название, bool приписатьДату)
	{
		if (ЖурналОткрыт)
		{
			Вывести("### Журнал уже открыт.");
			return;
		}
		if (ПроигрывательВключен)
		{
			Вывести("### Нельзя открыть журнал во время воспроизведения.");
			return;
		}
		if (!Directory.Exists(папкаЖурналы))
			Directory.CreateDirectory(папкаЖурналы);
		if (приписатьДату)
			название = название + " " + Дата;
		название = название.Trim();
		if (название == "")
			название = ДатаИВремя;
		int числоПопыток = 0;
		StreamWriter sw = null;
	ОткрытиеФайла:
		числоПопыток++;
		string путь = Path.Combine(папкаЖурналы, название + ".txt");
		try
		{
			sw = new StreamWriter(путь, true, Encoding.GetEncoding(1251));
		}
		catch
		{
			if (числоПопыток >= 10)
			{
				Вывести("### Не удалось открыть журнал.");
				return;
			}
			название += "_";
			goto ОткрытиеФайла;
		}
		предыдущееВремя = DateTime.Now.Ticks;
		Вывести("### Начата запись лога в файл \"Журналы\\" + название + ".txt\".");
		streamWriter = sw;
		string строка = "### Начало записи: " + ДатаИВремя + ".";
		if (ЗаписыватьПаузы)
			streamWriter.Write("0000 ");
		if (ЗаписыватьЦвета)
			streamWriter.Write("\u001B07");
		streamWriter.WriteLine(строка);
		НачатьФормированиеНовогоЭха();
	}

	public void ОткрытьЖурнал()
	{
		ОткрытьЖурнал(ДатаИВремя, false);
	}

	public void ОткрытьЖурнал(string название)
	{
		ОткрытьЖурнал(название, false);
	}

	bool пауза = false;
	int скорость = 100;
	string[] линии = null;
	Thread поток = null;

	void Сбросить()
	{
		пауза = false;
		скорость = 100;
		линии = null;
		if (поток != null)
		{
			поток.Abort();
			поток.Join();
			поток = null;
		}
	}

	public bool ПроигрывательВключен
	{
		get
		{
			return (линии != null);
		}
	}

	public bool Пауза
	{
		get
		{
			return пауза;
		}
		set
		{
			if (!ПроигрывательВключен)
			{
				Вывести("### Воспроизведение не начато.");
				return;
			}
			пауза = value;
			if (пауза)
				Вывести("### Воспроизведение приостановлено.");
			else
				Вывести("### Воспроизведение продолжено.");
		}
	}

	public int Скорость
	{
		get
		{
			return скорость;
		}
		set
		{
			if (!ПроигрывательВключен)
			{
				Вывести("### Воспроизведение не начато.");
				return;
			}
			скорость = value;
			if (скорость < 1)
				скорость = 1;
			if (скорость > 10000)
				скорость = 10000;
			Вывести("### Установлена скорость воспроизведения: " + скорость + "%.");
		}
	}

	public void Стоп()
	{
		if (!ПроигрывательВключен)
		{
			Вывести("### Воспроизведение не начато.");
			return;
		}
		Вывести("### Воспроизведение прервано.");
		Сбросить();
	}

	byte Преобразовать(char символ)
	{
		if (символ >= '0' && символ <= '9')
			return (byte)(символ - '0');
		if (символ >= 'a' && символ <= 'f')
			return (byte)(символ - 'a' + 10);
		if (символ >= 'A' && символ <= 'F')
			return (byte)(символ - 'A' + 10);
		return 0;
	}

	Строка Декодировать(string строка)
	{
		Строка результат = new Строка(строка.Length);
		byte цветПереднегоПлана = 7;
		byte цветФона = 0;
		int состояние = 0;
		foreach (char символ in строка)
		{
			if (состояние == 1)
			{
				цветФона = Преобразовать(символ);
				состояние = 2;
				continue;
			}
			if (состояние == 2)
			{
				цветПереднегоПлана = Преобразовать(символ);
				состояние = 0;
				continue;
			}
			if (символ == '\u001B')
			{
				состояние = 1;
				continue;
			}
			результат.Добавить(new Символ(символ, цветПереднегоПлана, цветФона));
		}
		return результат;
	}

	void ПотоковаяПроцедура()
	{
		List<Строка> выводимыеСтроки = new List<Строка>(линии.Length + 2);
		выводимыеСтроки.Add(new Строка("### Воспроизведение начато."));
		foreach (string линия in линии)
		{
			if (линия.Length < 5 || линия[4] != ' ')
			{
				выводимыеСтроки.Add(Декодировать(линия));
				continue;
			}
			int задержка = 0;
			try
			{
				задержка = int.Parse(линия.Substring(0, 4), NumberStyles.AllowHexSpecifier);
			}
			catch
			{
				выводимыеСтроки.Add(Декодировать(линия));
				continue;
			}
			if (задержка != 0)
			{
				Вывести(выводимыеСтроки.ToArray());
				выводимыеСтроки = new List<Строка>(линии.Length);
				long началоЗадержки = DateTime.Now.Ticks / 10000;
				while (true)
				{
					long текущееВремя = DateTime.Now.Ticks / 10000;
					if (текущееВремя - началоЗадержки >= задержка * 100 / Скорость)
						break;
					Thread.Sleep(1);
				}
				while (Пауза)
					Thread.Sleep(1);
			}
			выводимыеСтроки.Add(Декодировать(линия.Substring(5)));
		}
		выводимыеСтроки.Add(new Строка("### Воспроизведение завершено."));
		Вывести(выводимыеСтроки.ToArray());
		Сбросить();
	}

	string НайтиЖурнал(string название)
	{
		if (File.Exists(название))
			return название;
		string путь = название + ".txt";
		if (File.Exists(путь))
			return путь;
		путь = Path.Combine(Application.StartupPath, название);
		if (File.Exists(путь))
			return путь;
		путь += ".txt";
		if (File.Exists(путь))
			return путь;
		путь = Path.Combine(папкаЖурналы, название);
		if (File.Exists(путь))
			return путь;
		путь += ".txt";
		if (File.Exists(путь))
			return путь;
		return null;
	}

	public void Воспроизвести(string путь)
	{
		if (ПроигрывательВключен)
		{
			Вывести("### Воспроизведение уже начато.");
			return;
		}
		if (ЖурналОткрыт)
		{
			Вывести("### Нельзя начать воспроизведение в процессе записи лога.");
			return;
		}
		try
		{
			линии = File.ReadAllLines(НайтиЖурнал(путь), Encoding.GetEncoding(1251));
		}
		catch
		{
			Вывести("### Не удалось открыть файл.");
			return;
		}
		поток = new Thread(new ThreadStart(ПотоковаяПроцедура));
		поток.Start();
	}
}
