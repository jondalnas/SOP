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

			port.Write(" ");

			//string csv = "Baud,2 byte (sec),4 byte (sec),8 byte (sec)\n";
			string csv = "";

			//uint[] bauds = { /*300, 600, 1200, 2400, 4800, */9600, 14400, 19200, 28800, 38400, 57600, 76800, 115200, 230400, 250000, 500000 };
			//for (int baudI = 1; baudI < 11; baudI++) {
			//	uint baud = bauds[baudI - 1];
			uint baud = 9600;
			csv += baud;
			Console.Write(baud);

			port.Close();
			port.BaudRate = (int) baud;
			port.Open();

			uint samples = baud;

			for (int bytes = 0; bytes < 3; bytes++) {
				//port.Write(" ");

				var sw = System.Diagnostics.Stopwatch.StartNew();
				char chr = ' ';
				while ((chr = (char) port.ReadByte()) != ';' && chr != 0) {
					if (bytes == 1) port.ReadByte();
					else if (bytes == 2) {
						port.ReadByte();
						port.ReadByte();
					}
				}
				sw.Stop();
				Console.Write("," + ((sw.ElapsedMilliseconds / 1000.0) / samples));

				csv += "," + ((sw.ElapsedMilliseconds / 1000.0) / samples);
			}

			csv += "\n";
			Console.Write("\n");

			port.Close();

			File.AppendAllText("Data.csv", csv.ToString());
			
			port.Close();

			Console.ReadLine();
		}
	}
}
