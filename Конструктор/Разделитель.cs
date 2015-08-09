/*
	Горизонтальный разделитель
*/


using System.Drawing;
using System.Windows.Forms;


class Разделитель : Splitter
{
	public Разделитель()
	{
		BackColor = Color.Red;
		Dock = DockStyle.Bottom;
		BorderStyle = BorderStyle.None;
		MinSize = 0;
	}
}
