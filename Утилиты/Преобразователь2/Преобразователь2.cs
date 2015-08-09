/*
	Преобразователь с графическим интерфейсом
*/


using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


class Преобразователь2 : Form
{
	static readonly Encoding кодировка = Encoding.GetEncoding(1251);

	ListBox список = new ListBox();
	Button очистить = new Button();
	Button преобразовать = new Button();
	Button информация = new Button();
	ToolTip подсказка = new ToolTip();

	Преобразователь2()
	{
		Text = "Преобразователь 2";
		TopMost = true;
		ClientSize = new Size(320, 240);
		список.IntegralHeight = false;
		список.AllowDrop = true;
		список.SelectionMode = SelectionMode.MultiExtended;
		очистить.Text = "Очистить";
		преобразовать.Text = "Преобразовать";
		информация.Text = "Справка...";
		список.Parent = this;
		очистить.Parent = this;
		преобразовать.Parent = this;
		информация.Parent = this;
		подсказка.SetToolTip(список, "Список файлов");
		подсказка.SetToolTip(очистить, "Очистка логов от цветовых кодов и пауз");
		подсказка.SetToolTip(преобразовать, "Преобразование логов в HTML");
		подсказка.SetToolTip(информация, "Информация...");
		список.DragOver += new DragEventHandler(список_DragOver);
		список.DragDrop += new DragEventHandler(список_DragDrop);
		список.KeyDown += new KeyEventHandler(список_KeyDown);
		очистить.Click += new EventHandler(очистить_Click);
		преобразовать.Click += new EventHandler(преобразовать_Click);
		информация.Click += new EventHandler(информация_Click);
	}

	static void ОчиститьЛог(string файл)
	{
		Console.WriteLine("Файл: " + файл);
		StreamWriter sw = null;
		try
		{
			string[] линии = File.ReadAllLines(файл, кодировка);
			int i = 1;
			while(true)
			{
				string новоеИмя = файл + ".old";
				if (i != 1)
					новоеИмя += i;
				if (!File.Exists(новоеИмя))
				{
					File.Move(файл, новоеИмя);
					break;
				}
				i++;
			}
			sw = new StreamWriter(файл, false, кодировка);
			for (i = 0; i < линии.Length; i++)
			{
				string линия = линии[i];
				линия = Regex.Replace(линия, @"^[\da-fA-F]{4} ", "");
				линия = Regex.Replace(линия, @"\u001B[\da-fA-F]{2}", "");
				sw.WriteLine(линия);
			}
		}
		catch (Exception e)
		{
			MessageBox.Show(e.Message);
		}
		if (sw != null)
			sw.Close();
	}

	static void ПреобразоватьЛог(string файл)
	{
		Console.WriteLine("Файл: " + файл);
		StreamWriter sw = null;
		try
		{
			string[] линии = File.ReadAllLines(файл, кодировка);
			файл = Path.Combine(Path.GetDirectoryName(файл), Path.GetFileNameWithoutExtension(файл));
			string новыйФайл;
			int i = 1;
			while(true)
			{
				новыйФайл = файл;
				if (i != 1)
					новыйФайл += " (" + i + ")";
				новыйФайл += ".htm";
				if (!File.Exists(новыйФайл))
					break;
				i++;
			}
			sw = new StreamWriter(новыйФайл, false, кодировка);
			sw.WriteLine("<html><head>");
			sw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1251\">");
			sw.WriteLine("<link href=\"mystyles.css\" rel=\"stylesheet\" type=\"text/css\">");
			sw.Write("</head><body><pre><a class=07>");
			for (i = 0; i < линии.Length; i++)
			{
				string линия = линии[i];
				линия = линия.Replace("&", "&amp;");
				линия = линия.Replace("<", "&lt;");
				линия = линия.Replace(">", "&gt;");
				линия = линия.Replace("\"", "&quot;");
				линия = Regex.Replace(линия, @"^[\da-fA-F]{4} ", "");
				линия = Regex.Replace(линия, @"\u001B([\da-fA-F]{2})", "</a><a class=$1>");
				sw.WriteLine(линия);
			}
			sw.WriteLine("</a></pre></body></html>");
		}
		catch (Exception e)
		{
			MessageBox.Show(e.Message);
		}
		if (sw != null)
			sw.Close();
	}

	protected override void OnResize(EventArgs e)
	{
		список.Height = ClientSize.Height - очистить.Height;
		список.Width = ClientSize.Width;
		очистить.Top = преобразовать.Top = информация.Top = список.Bottom;
		очистить.Width = преобразовать.Width = ClientSize.Width / 3;
		информация.Width = ClientSize.Width - 2 * очистить.Width;
		преобразовать.Left = очистить.Right;
		информация.Left = преобразовать.Right;
	}

	static bool IsFileDrop(IDataObject d)
	{
		foreach (string str in d.GetFormats())
		{
			if (str == DataFormats.FileDrop)
				return true;
		}
		return false;
	}

	void список_DragOver(object sender, DragEventArgs e)
	{
		e.Effect = IsFileDrop(e.Data) ? DragDropEffects.All : DragDropEffects.None;
	}

	void информация_Click(object sender, EventArgs e)
	{
		MessageBox.Show
		(
			"Утилита для преобразования логов в HTML и очистки их от цветовых кодов и пауз\n" +
			"Использование: перетащите необходимые файлы (папки) в список и нажмите кнопку Очистить или кнопку Преобразовать\n\n" +
			"Примечания:\n" +
			"1) Клавишей Delete можно удалить лишние файлы из списка\n" +
			"2) Web-страничка, сформированная данной программой, требует таблицу стилей mystyles.css, которую можно создать с помощью программы Стили",
			"Преобразователь 2"
		);
	}

	void очистить_Click(object sender, EventArgs e)
	{
		foreach (string path in список.Items)
			ОчиститьЛог(path);
		список.Items.Clear();
	}

	void преобразовать_Click(object sender, EventArgs e)
	{
		foreach (string path in список.Items)
			ПреобразоватьЛог(path);
		список.Items.Clear();
	}

	void ДобавитьПапку(string папка)
	{
		список.Items.AddRange(Directory.GetFiles(папка));
		string[] subDirs = Directory.GetDirectories(папка);
		foreach (string subDir in subDirs)
			ДобавитьПапку(subDir);
	}

	void список_DragDrop(object sender, DragEventArgs e)
	{
		string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
		foreach (string path in paths)
		{
			if (File.Exists(path))
				список.Items.Add(path);
			else if(Directory.Exists(path))
				ДобавитьПапку(path);
		}
	}

	void список_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Delete)
		{
			object[] selectedItems = new object[список.SelectedItems.Count];
			список.SelectedItems.CopyTo(selectedItems, 0);
			foreach (object obj in selectedItems)
				список.Items.Remove(obj);
		}
	}

	[STAThread]
	static void Main() 
	{
		Application.Run(new Преобразователь2());
	}
}
