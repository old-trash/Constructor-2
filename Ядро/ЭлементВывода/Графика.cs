/*
	–исование с помощью Windows GDI
*/


using System;
using System.Drawing;
using System.Runtime.InteropServices;


class √рафика
{
	const string названиеЎрифта = "Fixedsys";

	public const int ¬ысота—имвола = 16;
	public const int Ўирина—имвола = 8;

	static readonly uint[] цвета = new uint[16]
	{
		//BBGGRR
		0x000000,
		0x0000C0,
		0x00C000,
		0x00C0C0,
		0xC00000,
		0xC000C0,
		0xC0C000,
		0xC0C0C0,
		0x808080,
		0x0000FF,
		0x00FF00,
		0x00FFFF,
		0xFF0000,
		0xFF00FF,
		0xFFFF00,
		0xFFFFFF,
	};

	[DllImport("Gdi32.dll")]
	static extern IntPtr CreateFont(int height, int width, int escapement, int orientation, int weight, uint italic, uint underline, uint strikeout, uint charSet, uint outputPrecision, uint clipPrecision, uint quality, uint pitchAndFamily, string face);

	[DllImport("Gdi32.dll")]
	static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

	[DllImport("Gdi32.dll")]
	static extern bool DeleteObject(IntPtr obj);

	[DllImport("Gdi32.dll")]
	static extern uint SetTextColor(IntPtr hdc, uint color);

	[DllImport("Gdi32.dll")]
	static extern uint SetBkColor(IntPtr hdc, uint color);

	[DllImport("Gdi32.dll")]
	static extern bool TextOut(IntPtr hdc, int x, int y, string str, int len);

	[DllImport("Gdi32.dll")]
	static extern IntPtr CreateSolidBrush(uint color);

	[DllImport("User32.dll")]
	static extern int FillRect(IntPtr hdc, ref Rect rect, IntPtr brush);

	[DllImport("User32.dll")]
	static extern bool InvertRect(IntPtr hdc, ref Rect rect);

	IntPtr фонова€ исть = CreateSolidBrush(0);
	IntPtr шрифт = CreateFont(¬ысота—имвола, Ўирина—имвола, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, названиеЎрифта);

	~√рафика()
	{
		DeleteObject(фонова€ исть);
		DeleteObject(шрифт);
	}

	Graphics графика = null;
	IntPtr hdc = IntPtr.Zero;
	IntPtr исходныйЎрифт = IntPtr.Zero;

	public void Ќачать–исование(Graphics графика)
	{
		this.графика = графика;
		hdc = графика.GetHdc();
		исходныйЎрифт = SelectObject(hdc, шрифт);
	}

	public void «авершить–исование()
	{
		SelectObject(hdc, исходныйЎрифт);
		графика.ReleaseHdc(hdc);
		графика = null;
		hdc = IntPtr.Zero;
		исходныйЎрифт = IntPtr.Zero;
	}

	public void Ќарисовать—троку(string значение, byte цвет, int x, int y)
	{
		int цветѕереднегоѕлана = цвет & 0x0F;
		int цвет‘она = цвет >> 4;
		SetTextColor(hdc, цвета[цветѕереднегоѕлана]);
		SetBkColor(hdc, цвета[цвет‘она]);
		TextOut(hdc, x, y, значение, значение.Length);
	}

	public void »нвертироватьѕр€моугольник(int x, int y, int ширина, int высота)
	{
		Rect rect = new Rect(x, y, x + ширина, y + высота);
		InvertRect(hdc, ref rect);
	}

	public void «акраситьѕр€моугольник(int x, int y, int ширина, int высота)
	{
		Rect rect = new Rect(x, y, x + ширина, y + высота);
		FillRect(hdc, ref rect, фонова€ исть);
	}
}
