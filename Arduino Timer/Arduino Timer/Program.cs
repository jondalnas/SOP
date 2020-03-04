using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.IO;

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

			/*baud:
			Console.WriteLine("What baud rate? ");

			string baudChoice = Console.ReadLine();

			int intBaudChoice;
			if (!Int32.TryParse(baudChoice, out intBaudChoice)) goto baud;*/

			port = new SerialPort(ports[intPortChoice], 9600, Parity.None, 8, StopBits.One);
			port.Open();

			string csv = "Baud,2 byte (sec)\n";
			
			uint[] bauds = { 9600, 14400, 19200, 38400, 57600, 76800, 115200, 230400, 250000, 500000, 1000000, 2000000 };
			for (int baudI = 0; baudI < 12; baudI++) {
				uint baud = bauds[baudI];
				Handshake(baudI, baud);

				Console.Write(baud);
				csv += baud;

				uint samples = baud<<2;

				char chr;
				uint curr = 0;
				var sw = System.Diagnostics.Stopwatch.StartNew();
				while ((chr = (char)port.ReadChar()) != 0) {
					curr = (curr % 9) + 1;

					if (curr != (int)chr) {
						Console.WriteLine(baud + ", E: " + (int)chr);
						curr = (uint)chr;
					}
				}
				sw.Stop();

				Console.Write("," + ((sw.ElapsedMilliseconds / 1000.0) / samples) + "\n");
				csv += "," + ((sw.ElapsedMilliseconds / 1000.0) / samples) + "\n";
			}

			port.Close();

			File.WriteAllText("Data.csv", csv.ToString());

			Console.ReadLine();
		}

		private static void Handshake(int baudI, uint baud) {
			while (port.ReadByte() != baudI);

			port.DiscardInBuffer();
			port.Close();
			port.BaudRate = (int)baud;
			port.Open();

			port.Write(new Byte[] { (byte)baudI }, 0, 1);
		}
	}
}
