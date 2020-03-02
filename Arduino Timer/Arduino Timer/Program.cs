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

			uint samples = 1000;
			var sw = System.Diagnostics.Stopwatch.StartNew();
			for (int i = 0; i < samples; i++) {
				port.Write(new byte[] { 170 }, 0, 1);
				port.ReadByte();
			}
			sw.Stop();
			Console.WriteLine("It took " + (sw.ElapsedMilliseconds / 1000.0) + " seconds");

			Console.ReadLine();
		}
	}
}
