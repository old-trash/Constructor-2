/*
	Главное меню приложения
*/


using System;
using System.Windows.Forms;


class ГлавноеМеню : MainMenu
{
	public ГлавноеМеню()
	{
		MenuItem профиль = MenuItems.Add("Профиль");
		профиль.MergeOrder = 0;
		профиль.MergeType = MenuMerge.MergeItems;
		MenuItem профиль_Создать = new MenuItem("Создать", new EventHandler(ОбработатьНажатие_Профиль_Создать), Shortcut.CtrlN);
		профиль.MenuItems.Add(профиль_Создать);
		MenuItem профиль_Открыть = new MenuItem("Открыть", new EventHandler(ОбработатьНажатие_Профиль_Открыть), Shortcut.CtrlO);
		профиль.MenuItems.Add(профиль_Открыть);
	}

	void ОбработатьНажатие_Профиль_Создать(object sender, EventArgs e)
	{
		Конструктор.ГлавноеОкно.СоздатьПрофиль();
	}

	void ОбработатьНажатие_Профиль_Открыть(object sender, EventArgs e)
	{
		Конструктор.ГлавноеОкно.ОткрытьПрофиль();
	}
}