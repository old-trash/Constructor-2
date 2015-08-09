/*
	Класс, реализующий поддержку MCCP обеих версий
*/


using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip.Compression;


class Декомпрессор
{
	const byte mccp_Compress  = 85;
	const byte mccp_Compress2 = 86;

	const int состояние_Обычное           = 0;
	const int состояние_Iac               = 1;
	const int состояние_IacWill           = 2;
	const int состояние_IacSb             = 3;
	const int состояние_IacSbCompress     = 4;
	const int состояние_IacSbCompressWill = 5;
	const int состояние_IacSbCompress2    = 6;
	const int состояние_IacSbCompress2Iac = 7;

	Inflater inflater = null;
	int версияMccp = 0;
	int состояние = состояние_Обычное;
	List<byte> последовательность = new List<byte>(8);

	public byte[] Распаковать(byte[] байты, int длина, out byte[] ответСерверу)
	{
		byte[] буфер = new byte[длина * 16];
		int смещение = 0;
		List<byte> результат = new List<byte>(буфер.Length);
		List<byte> ответ = new List<byte>(8);
	Начало:
		if (inflater != null)
		{
			inflater.SetInput(байты, смещение, длина - смещение);
			while (true)
			{
				int числоБайт = inflater.Inflate(буфер);
				if (числоБайт > 0)
				{
					for (int i = 0; i < числоБайт; i++)
						результат.Add(буфер[i]);
					continue;
				}
				if (inflater.IsFinished)
				{
					смещение = длина - inflater.RemainingInput;
					inflater = null;
					версияMccp = 0;
					goto Начало;
				}
				break;
			}
		}
		else
		{
			while (смещение < длина)
			{
				byte байт = байты[смещение];
				последовательность.Add(байт);
				смещение++;
				if (состояние == состояние_Обычное && байт == Telnet.Iac)
				{
					состояние = состояние_Iac;
					continue;
				}
				if (состояние == состояние_Iac && байт == Telnet.Will)
				{
					состояние = состояние_IacWill;
					continue;
				}
				if (состояние == состояние_IacWill && байт == mccp_Compress)
				{
					состояние = состояние_Обычное;
					последовательность = new List<byte>(8);
					ответ.Add(Telnet.Iac);
					ответ.Add(Telnet.Do);
					ответ.Add(mccp_Compress);
					continue;
				}
				if (состояние == состояние_IacWill && байт == mccp_Compress2)
				{
					состояние = состояние_Обычное;
					последовательность = new List<byte>(8);
					ответ.Add(Telnet.Iac);
					ответ.Add(Telnet.Do);
					ответ.Add(mccp_Compress2);
					continue;
				}
				if (состояние == состояние_Iac && байт == Telnet.Sb)
				{
					состояние = состояние_IacSb;
					continue;
				}
				if (состояние == состояние_IacSb && байт == mccp_Compress)
				{
					состояние = состояние_IacSbCompress;
					continue;
				}
				if (состояние == состояние_IacSb && байт == mccp_Compress2)
				{
					состояние = состояние_IacSbCompress2;
					continue;
				}
				if (состояние == состояние_IacSbCompress && байт == Telnet.Will)
				{
					состояние = состояние_IacSbCompressWill;
					continue;
				}
				if (состояние == состояние_IacSbCompress2 && байт == Telnet.Iac)
				{
					состояние = состояние_IacSbCompress2Iac;
					continue;
				}
				if (состояние == состояние_IacSbCompressWill && байт == Telnet.Se)
				{
					состояние = состояние_Обычное;
					последовательность = new List<byte>(8);
					версияMccp = 1;
					inflater = new Inflater();
					goto Начало;
				}
				if (состояние == состояние_IacSbCompress2Iac && байт == Telnet.Se)
				{
					состояние = состояние_Обычное;
					последовательность = new List<byte>(8);
					версияMccp = 2;
					inflater = new Inflater();
					goto Начало;
				}
				состояние = состояние_Обычное;
				результат.AddRange(последовательность);
				последовательность = new List<byte>(8);
			}
		}
		ответСерверу = ответ.ToArray();
		return результат.ToArray();
	}

	public int ВерсияMccp
	{
		get
		{
			return версияMccp;
		}
	}

	public float КоэффициентСжатия
	{
		get
		{
			if (inflater == null || inflater.TotalIn == 0)
				return 1;
			return (float)inflater.TotalOut / inflater.TotalIn;
		}
	}
}
