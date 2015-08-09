/*
	”ничтожаемый сокет
*/


using System.Net.Sockets;


class —окет : Socket
{
	bool уничтожен = false;

	public —окет() : base (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
	{
	}

	public bool ”ничтожен
	{
		get
		{
			return уничтожен;
		}
	}

	public void ”ничтожить()
	{
		уничтожен = true;
		Dispose(true);
	}
}
