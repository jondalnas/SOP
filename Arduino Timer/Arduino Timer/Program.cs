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

			port = new SerialPort(ports[intPortChoice], 300, Parity.None, 8, StopBits.One);
			port.Open();

			StringBuilder csv = new StringBuilder();

			uint[] bauds = { 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 57600, 76800, 115200, 230400, 250000, 500000 };
			for (int baudI = 1; baudI < 17; baudI++) {
				uint baud = bauds[baudI - 1];

				uint samples = baud;
				var sw = System.Diagnostics.Stopwatch.StartNew();
				for (int i = 0; i < samples; i++) {
					port.ReadByte();
				}
				sw.Stop();
				Console.WriteLine(baud + "," + ((sw.ElapsedMilliseconds / 1000.0) / samples));

				csv.AppendLine(baud + "," + ((sw.ElapsedMilliseconds / 1000.0) / samples));

				port.Close();
				port.BaudRate = (int)bauds[baudI];
				port.Open();
			}

			File.WriteAllText("Data.csv", csv.ToString());

			Console.ReadLine();

			port.Close();
		}
	}
}
