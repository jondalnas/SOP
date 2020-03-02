using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace Arduino_Timer {
	class Program {
		public static SerialPort port;

		static void Main(string[] args) {
			string[] ports = SerialPort.GetPortNames();

			for (int i = 0; i < ports.Length; i++) {
				Console.WriteLine(i + ") " + ports[i]);
			}

			port:
			Console.WriteLine("Which port? ");

			string portChoice = Console.ReadLine();

			int intPortChoice;
			if (!Int32.TryParse(portChoice, out intPortChoice)) goto port;

			baud:
			Console.WriteLine("What baud rate? ");

			string baudChoice = Console.ReadLine();

			int intBaudChoice;
			if (!Int32.TryParse(baudChoice, out intBaudChoice)) goto baud;

			port = new SerialPort(ports[intPortChoice], intBaudChoice, Parity.None, 8, StopBits.One);
			port.Open();

			Thread thread = new Thread(Read);
			thread.Start();

			message:
			Console.Write(">");

			port.Write(Console.ReadLine());

			Thread.Sleep(1000);

			goto message;
		}

		public static void Read() {
			while (true) {
				Console.Write((char) port.ReadChar());
			}
		}
	}
}
