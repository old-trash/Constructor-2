using System.Text.RegularExpressions;


class Вещи : Подключение
{
	public override bool ОбработатьНажатиеКлавиши(Клавиша клавиша, bool ctrl, bool alt, bool shift)
	{
		return base.ОбработатьНажатиеКлавиши(клавиша, ctrl, alt, shift);
	}

	public override string ОбработатьСтроку(Строка строка, bool статуснаяСтрока)
	{
		Match match;
		string значение = строка.ToString();
		if (значение.StartsWith("Кровушка стынет в жилах"))
		{
			ЭлементВывода.Добавить(строка);
			if (Переменные["ЛУТ"] != null)
				return "взят все;взят все все.труп;брос все.труп";
			return null;
		}
		return base.ОбработатьСтроку(строка, статуснаяСтрока);
	}

	public override string ОбработатьКоманду(string команда)
	{
		return base.ОбработатьКоманду(команда);
	}
}
