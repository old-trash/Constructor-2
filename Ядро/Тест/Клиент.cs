/*
	ѕростейший клиент
*/


using System;
using System.Windows.Forms;


class  лиент : ядро
{
	 лиент()
	{
		Text = "ѕростейший клиент";
		WindowState = FormWindowState.Maximized;
	}

	public override bool ќбработатьЌажатие лавиши( лавиша клавиша, bool ctrl, bool alt, bool shift)
	{
		if (!ctrl && !alt && !shift)
		{
			switch (клавиша)
			{
				case  лавиша.F1:
					ƒублироватьЁлемент¬ывода = !ƒублироватьЁлемент¬ывода;
					return true;
				case  лавиша.F2:
					Ёлемент¬вода.јвтоочистка = !Ёлемент¬вода.јвтоочистка;
					return true;
				case  лавиша.F3:
					Ёлемент¬ывода. омпактный¬ывод = !Ёлемент¬ывода. омпактный¬ывод;
					return true;
				case  лавиша.F5:
					Ёлемент¬ывода.ќткрыть∆урнал("Ћог");
					return true;
				case  лавиша.F6:
					Ёлемент¬ывода.«акрыть∆урнал();
					return true;
				case  лавиша.F7:
					Ёлемент¬ывода.¬оспроизвести("Ћог");
					return true;
				case  лавиша.F8:
					Ёлемент¬ывода.—топ();
					return true;
				case  лавиша.F9:
					Ёлемент¬ывода.ѕауза = !Ёлемент¬ывода.ѕауза;
					return true;
				case  лавиша.F10:
					“аймеры.ќтсрочить(2000, "смотр");
					return true;
				case  лавиша.F11:
					“аймеры.«апустить“аймер("“аймер1", 1000, 1000, 5, "огл");
					return true;
				case  лавиша.F12:
					“аймеры.¬ывести—писок();
					return true;
				case  лавиша.Num8:
					ќбработать¬веденный“екст("n");
					return true;
				case  лавиша.Num2:
					ќбработать¬веденный“екст("s");
					return true;
				case  лавиша.Num4:
					ќбработать¬веденный“екст("w");
					return true;
				case  лавиша.Num6:
					ќбработать¬веденный“екст("e");
					return true;
				case  лавиша.Num9:
					ќбработать¬веденный“екст("u");
					return true;
				case  лавиша.Num3:
					ќбработать¬веденный“екст("d");
					return true;
				case  лавиша.Num7:
					ќбработать¬веденный“екст("см");
					return true;
				case  лавиша.Num5:
					ќбработать¬веденный“екст("огл");
					return true;
				case  лавиша.Num1:
					ќбработать¬веденный“екст("вых");
					return true;
			}
		}
		if (ctrl && !alt && !shift)
		{
			switch (клавиша)
			{
				case  лавиша.Ћ:
					—оединение.”становить("localhost", 4000);
					return true;
				case  лавиша.Ѕ:
					—оединение.”становить("mud.ru", 4000);
					return true;
				case  лавиша.’:
					—оединение.”становить("hiervard.ru", 4000);
					return true;
				case  лавиша.ќ:
					—оединение.–азорвать();
					return true;
			}
		}
		return base.ќбработатьЌажатие лавиши(клавиша, ctrl, alt, shift);
	}

	[STAThread]
	static void Main()
	{
		Application.Run(new  лиент());
	}
}