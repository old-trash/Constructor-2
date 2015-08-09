/*
	”тилита дл€ определени€ кодов клавиш
*/


using System.Drawing;
using System.IO;
using System.Windows.Forms;


class  лавиша : Form, IMessageFilter
{
	const int wmSysKeyDown = 0x0104;
	const int wmKeyDown    = 0x0100;

	StreamWriter streamWriter = new StreamWriter("Ќажатые клавиши.txt");
	uint код лавиши = 0;

	 лавиша()
	{
		ClientSize = new Size(400, 0);
		FormBorderStyle = FormBorderStyle.FixedSingle;
		Text = "Ќажимайте клавиши...";
		MaximizeBox = false;
		StartPosition = FormStartPosition.CenterScreen;
		Application.AddMessageFilter(this);
		streamWriter.AutoFlush = true;
	}

	public bool PreFilterMessage(ref Message m)
	{
		if (m.Msg == wmSysKeyDown || m.Msg == wmKeyDown)
		{
			код лавиши = (uint)m.LParam;
			код лавиши = код лавиши >> 16;
			код лавиши = код лавиши & 0x1FF;
		}
		return false;
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		string сообщение = e.KeyCode.ToString() + " = 0x" + код лавиши.ToString("X3");
		Text = сообщение;
		streamWriter.WriteLine(сообщение);
	}

	static void Main()
	{
		Application.Run(new  лавиша());
	}
}
