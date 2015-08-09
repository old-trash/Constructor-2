/*
	Основа для конфига
*/


class Окно : Каркас
{
	public override bool ОбработатьНажатиеКлавиши(Клавиша клавиша, bool ctrl, bool alt, bool shift)
	{
		return base.ОбработатьНажатиеКлавиши(клавиша, ctrl, alt, shift);
	}

	public override string ОбработатьСтроку(Строка строка, bool статуснаяСтрока)
	{
		return base.ОбработатьСтроку(строка, статуснаяСтрока);
	}

	public override string ОбработатьКоманду(string команда)
	{
		return base.ОбработатьКоманду(команда);
	}
}
