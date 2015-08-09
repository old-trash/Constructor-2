/*
	√лавный класс приложени€
*/


using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


class  онструктор : Form, IMessageFilter
{
	public static  онструктор √лавноеќкно = new  онструктор();

	[STAThread]
	static void Main()
	{
		Application.Run(√лавноеќкно);
	}

	int x, y, ширина, высота;
	FormWindowState состо€ние;

	public √лавноећеню √лавноећеню = new √лавноећеню();
	public –азделитель –азделитель = new –азделитель();
	public Ёлемент¬ывода Ёлемент¬ывода = new Ёлемент¬ывода();

	 онструктор()
	{
		Text = " онструктор 2.15";
		IsMdiContainer = true;
		StartPosition = FormStartPosition.Manual;
		Menu = √лавноећеню;
		–азделитель.Parent = this;
		Ёлемент¬ывода.Dock = DockStyle.Bottom;
		Ёлемент¬ывода.Parent = this;
		«агрузить–азмеры();
		Application.AddMessageFilter(this);
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		—охранить–азмеры();
		string списокѕрофилей = "";
		foreach (ќкно окно in MdiChildren)
		{
			if (окно.‘айл == null || окно.‘айл == "")
				continue;
			if (списокѕрофилей != "")
				списокѕрофилей += "|";
			списокѕрофилей += окно.‘айл;
		}
		Ini.—охранить(null, "ѕараметры", "ѕрофили", списокѕрофилей);
		base.OnClosing(e);
	}

	protected override void OnLoad(EventArgs e)
	{
		string списокѕрофилей = Ini.«агрузить(null, "ѕараметры", "ѕрофили", "").Trim();
		if (списокѕрофилей == "")
			return;
		string[] файлы = списокѕрофилей.Split('|');
		foreach (string файл in файлы)
			ќткрытьѕрофиль(файл);
		base.OnLoad(e);
	}

	protected override void OnResize(EventArgs e)
	{
		if (WindowState == FormWindowState.Normal)
		{
			ширина = Width;
			высота = Height;
		}
		if (WindowState != FormWindowState.Minimized)
			состо€ние = WindowState;
		base.OnResize(e);
	}

	protected override void OnMove(EventArgs e)
	{
		if (WindowState == FormWindowState.Normal)
		{
			x = Left;
			y = Top;
		}
		if (WindowState != FormWindowState.Minimized)
			состо€ние = WindowState;
		base.OnMove(e);
	}

	void «агрузить–азмеры()
	{
		x = Ini.«агрузить(null, "ѕараметры", "X", 100);
		y = Ini.«агрузить(null, "ѕараметры", "Y", 100);
		Location = new Point(x, y);
		ширина = Ini.«агрузить(null, "ѕараметры", "Ўирина", 400);
		высота = Ini.«агрузить(null, "ѕараметры", "¬ысота", 300);
		Size = new Size(ширина, высота);
		состо€ние = Ini.«агрузить(null, "ѕараметры", "—осто€ние", FormWindowState.Maximized);
		WindowState = состо€ние;
		Ёлемент¬ывода.Height = Ini.«агрузить(null, "ѕараметры", "¬ысотаЁлемента¬ывода", √рафика.¬ысота—имвола * 3);
	}

	void —охранить–азмеры()
	{
		Ini.—охранить(null, "ѕараметры", "X", x);
		Ini.—охранить(null, "ѕараметры", "Y", y);
		Ini.—охранить(null, "ѕараметры", "Ўирина", ширина);
		Ini.—охранить(null, "ѕараметры", "¬ысота", высота);
		Ini.—охранить(null, "ѕараметры", "—осто€ние", состо€ние);
		Ini.—охранить(null, "ѕараметры", "¬ысотаЁлемента¬ывода", Ёлемент¬ывода.Height);
	}

	public void —оздатьѕрофиль()
	{
		ќкно окно = new ќкно();
		окно.MdiParent = this;
		if (MdiChildren.Length == 1)
			окно.WindowState = FormWindowState.Maximized;
		окно.Show();
	}

	public void ќткрытьѕрофиль()
	{
		ќкно окно = new ќкно();
		if (!окно.¬ыполнить(true))
			return;
		окно.Ёлемент¬ывода.¬ывести();
		окно.—оединение.ѕередатьƒанные();
		окно.MdiParent = this;
		if (MdiChildren.Length == 1)
			окно.WindowState = FormWindowState.Maximized;
		окно.Show();
	}

	public void ќткрытьѕрофиль(string путь)
	{
		ќкно окно = new ќкно();
		if (!окно.¬ыполнить(путь, true))
			return;
		окно.Ёлемент¬ывода.¬ывести();
		окно.—оединение.ѕередатьƒанные();
		окно.MdiParent = this;
		if (MdiChildren.Length == 1)
			окно.WindowState = FormWindowState.Maximized;
		окно.Show();
	}

	public void јктивироватьќкно(ќкно окно)
	{
		if (окно == null)
			return;
		окно.Activate();
		if (окно.WindowState == FormWindowState.Minimized)
			окно.WindowState = FormWindowState.Normal;
	}

	public void јктивироватьќкно(int номер)
	{
		јктивироватьќкно(Ќайтиќкно(номер));
	}

	public void јктивироватьќкно(string им€)
	{
		јктивироватьќкно(Ќайтиќкно(им€));
	}

	public bool PreFilterMessage(ref Message m)
	{
		if (m.Msg != 0x0100 && m.Msg != 0x0104)
			return false;
		uint код лавиши = (uint)m.LParam;
		код лавиши = код лавиши >> 16;
		код лавиши = код лавиши & 0x1FF;
		 лавиша клавиша = ( лавиша)код лавиши;
		bool ctrl  = ((ModifierKeys & Keys.Control) == Keys.Control);
		bool alt   = ((ModifierKeys & Keys.Alt) == Keys.Alt);
		bool shift = ((ModifierKeys & Keys.Shift) == Keys.Shift);
		if (ctrl && !alt && !shift)
		{
			switch (клавиша)
			{
				case  лавиша.D1:
					јктивироватьќкно(1);
					return true;
				case  лавиша.D2:
					јктивироватьќкно(2);
					return true;
				case  лавиша.D3:
					јктивироватьќкно(3);
					return true;
				case  лавиша.D4:
					јктивироватьќкно(4);
					return true;
				case  лавиша.D5:
					јктивироватьќкно(5);
					return true;
				case  лавиша.D6:
					јктивироватьќкно(6);
					return true;
				case  лавиша.D7:
					јктивироватьќкно(7);
					return true;
				case  лавиша.D8:
					јктивироватьќкно(8);
					return true;
				case  лавиша.D9:
					јктивироватьќкно(9);
					return true;
				case  лавиша.D0:
					јктивироватьќкно(10);
					return true;
			}
		}
		return false;
	}

	public void ѕриказать¬сем(string приказ,  аркас кроме)
	{
		if (приказ == null)
			return;
		foreach ( аркас окно in MdiChildren)
		{
			if (окно != кроме)
				окно.ќбработать¬веденный“екст(приказ);
		}
	}

	public void ѕриказать(int номерќкна, string приказ)
	{
		if (приказ == null)
			return;
		ќкно окно = Ќайтиќкно(номерќкна);
		if (окно != null)
			окно.ќбработать¬веденный“екст(приказ);
	}

	public void ѕриказать(string им€, string приказ)
	{
		if (приказ == null)
			return;
		ќкно окно = Ќайтиќкно(им€);
		if (окно != null)
			окно.ќбработать¬веденный“екст(приказ);
	}

	public ќкно Ќайтиќкно(int номерќкна)
	{
		if (номерќкна == 0)
			номерќкна = 9;
		else
			номерќкна--;
		if (номерќкна < 0 || номерќкна >= MdiChildren.Length)
			return null;
		return (ќкно)MdiChildren[номерќкна];
	}

	public ќкно Ќайтиќкно(string им€)
	{
		if (им€ == null || им€ == "")
			return null;
		foreach (ќкно окно in MdiChildren)
		{
			if (string.Compare(окно.ѕеременные.»м€, им€, true) == 0)
				return окно;
		}
		return null;
	}
}
