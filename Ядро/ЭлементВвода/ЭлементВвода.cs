/*
	Ёлемент дл€ ввода пользовательских команд
*/


using System;
using System.Drawing;
using System.Windows.Forms;


delegate void ќбработчик—обыти€¬вод(string строка);
delegate bool ќбработчик—обыти€Ќажатие лавиши( лавиша клавиша, bool ctrl, bool alt, bool shift);


class Ёлемент¬вода : TextBox, IMessageFilter
{
	 руговойЅуфер<string> истори€ = new  руговойЅуфер<string>(100);
	int позици€¬»стории = 0;
	bool игнорировать»зменение“екста = false;

	public event ќбработчик—обыти€¬вод ¬вод = null;
	public event ќбработчик—обыти€Ќажатие лавиши Ќажатие лавиши = null;
	public bool јвтоочистка = false;

	public Ёлемент¬вода()
	{
		Font = new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular);
		BackColor = Color.White;
		ForeColor = Color.Black;
		BorderStyle = BorderStyle.None;
		Multiline = true;
		WordWrap = false;
		Application.AddMessageFilter(this);
	}

	new public void SelectAll()
	{
		base.SelectAll();
		ScrollToCaret();
	}

	protected virtual void ќбработать—обытие¬вод(string строка)
	{
		if (¬вод != null)
			¬вод(строка);
	}

	protected virtual bool ќбработать—обытиеЌажатие лавиши( лавиша клавиша, bool ctrl, bool alt, bool shift)
	{
		if (Ќажатие лавиши != null)
			return Ќажатие лавиши(клавиша, ctrl, alt, shift);
		return false;
	}

	// »зменение свойства Text без возникновени€ событи€ OnTextChanged.
	void »зменить“екст(string текст)
	{
		игнорировать»зменение“екста = true;
		Text = текст;
		игнорировать»зменение“екста = false;
	}

	protected override void OnTextChanged(EventArgs e)
	{
		if (игнорировать»зменение“екста)
			return;
		позици€¬»стории = истори€.„ислоЁлементов;
		// ¬ставка многострочного текста.
		string[] строки = Text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
		if (строки.Length == 1)
			return;
		for (int i = 0; i < строки.Length - 1; i++)
		{
			if (строки[i].Length == 0)
				continue;
			if (истори€.„ислоЁлементов == 0 || истори€[истори€.„ислоЁлементов - 1] != строки[i])
				истори€.ƒобавить(строки[i]);
		}
		if (строки[строки.Length - 1] != "" || јвтоочистка || строки[строки.Length - 2].Length == 0)
		{
			позици€¬»стории = истори€.„ислоЁлементов;
			»зменить“екст(строки[строки.Length - 1]);
			SelectionStart = Text.Length;
			ScrollToCaret();
		}
		else
		{
			позици€¬»стории = истори€.„ислоЁлементов - 1;
			»зменить“екст(истори€[позици€¬»стории]);
			SelectAll();
		}
		for (int i = 0; i < строки.Length - 1; i++)
			ќбработать—обытие¬вод(строки[i]);
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		// Ќажатие Enter'а.
		if (e.KeyChar != '\r')
			return;
		e.Handled = true;
		string строка = Text;
		if (строка.Length > 0)
		{
			if (истори€.„ислоЁлементов == 0 || истори€[истори€.„ислоЁлементов - 1] != строка)
				истори€.ƒобавить(строка);
			if (јвтоочистка)
			{
				позици€¬»стории = истори€.„ислоЁлементов;
				»зменить“екст("");
			}
			else
			{
				позици€¬»стории = истори€.„ислоЁлементов - 1;
				SelectAll();
			}
		}
		ќбработать—обытие¬вод(строка);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (e.KeyData == Keys.Up)
		{
			if (позици€¬»стории > 0)
				позици€¬»стории--;
		}
		else if (e.KeyData == Keys.Down)
		{
			if (позици€¬»стории < истори€.„ислоЁлементов)
				позици€¬»стории++;
		}
		else
		{
			return;
		}
		e.Handled = true;
		if (позици€¬»стории == истори€.„ислоЁлементов)
			»зменить“екст("");
		else
			»зменить“екст(истори€[позици€¬»стории]);
		SelectAll();
	}

	public bool PreFilterMessage(ref Message m)
	{
		if (IsDisposed || m.HWnd != Handle)
			return false;
		if (m.Msg != 0x0100 && m.Msg != 0x0104)
			return false;
		uint код лавиши = (uint)m.LParam;
		код лавиши = код лавиши >> 16;
		код лавиши = код лавиши & 0x1FF;
		 лавиша клавиша = ( лавиша)код лавиши;
		bool ctrl  = ((ModifierKeys & Keys.Control) == Keys.Control);
		bool alt   = ((ModifierKeys & Keys.Alt) == Keys.Alt);
		bool shift = ((ModifierKeys & Keys.Shift) == Keys.Shift);
		return ќбработать—обытиеЌажатие лавиши(клавиша, ctrl, alt, shift);
	}
}
