/*
	Цветной символ
*/


struct Символ
{
	public char Значение;
	public byte Цвет;

	public Символ(char значение)
	{
		Значение = значение;
		Цвет = 0x07;
	}

	public Символ(char значение, byte цвет)
	{
		Значение = значение;
		Цвет = цвет;
	}

	public Символ(char значение, byte цветПереднегоПлана, byte цветФона)
	{
		Значение = значение;
		цветПереднегоПлана = (byte)(цветПереднегоПлана & 0x0F);
		цветФона = (byte)(цветФона << 4);
		Цвет = (byte)(цветПереднегоПлана | цветФона);
	}

	public byte ЦветФона
	{
		get
		{
			return (byte)(Цвет >> 4);
		}
	}

	public byte ЦветПереднегоПлана
	{
		get
		{
			return (byte)(Цвет & 0x0F);
		}
	}

	public override string ToString()
	{
		return Значение.ToString();
	}
}
