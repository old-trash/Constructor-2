/*
	Прямоугольник
*/


using System.Runtime.InteropServices;


[StructLayout(LayoutKind.Sequential)]
struct Rect
{
	public int Left;
	public int Top;
	public int Right;
	public int Bottom;

	public Rect(int left, int top, int right, int bottom)
	{
		Left = left;
		Top = top;
		Right = right;
		Bottom = bottom;
	}
}
