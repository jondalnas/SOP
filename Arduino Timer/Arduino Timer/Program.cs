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
			//Get list of serial prots on computer and print them
			string[] ports = SerialPort.GetPortNames();

			for (int i = 0; i < ports.Length; i++) {
				Console.WriteLine(i + ") " + ports[i]);
			}

			port:
			Console.WriteLine("Which port? ");

			//Read users choice, if it isn't a number, then ask for their choice again
			string portChoice = Console.ReadLine();

			int intPortChoice;
			if (!Int32.TryParse(portChoice, out intPortChoice)) goto port;

			//Setup new port, with the same settings as the Arduino and open it
			port = new SerialPort(ports[intPortChoice], 9600, Parity.None, 8, StopBits.One);
			port.Open();
			
			//Create or override contens of Data.csv file
			File.WriteAllText("Data.csv", "Baud,2 bytes (sec)\n");

			//Go through list of baud rates and perform test
			uint[] bauds = { 9600, 14400, 19200, 38400, 57600, 76800, 115200, 230400, 250000, 500000, 1000000, 2000000, 4000000, 8000000, 16000000 };
			for (int baudI = 0; baudI < 12; baudI++) {
				uint baud = bauds[baudI];
				//Check if current baudrate is the same as the Arduino's
				Handshake(baudI, baud);

				uint samples = baud;
				uint frac = samples/10;

				char chr;
				uint curr = 0;
				uint i = 0;
				var sw = System.Diagnostics.Stopwatch.StartNew();
				//Recieve character from Arduino, if it's a stop-byte or if it has recieved the right amount of bytes, then jump out of while-loop
				while ((chr = (char)port.ReadChar()) != 0 && i < samples) {
					//Print current progress
					if (i % frac == 0) Console.WriteLine(baud + ", " + (i/(float)samples*100.0f) + "%");

					//Calculate next nuber in the series
					curr = (curr % 9) + 1;

					//If the next number doesn't match the calculated number, then print an error message
					if (curr != (int)chr) {
						Console.WriteLine(baud + ", E: " + (int)chr);
						curr = (uint)chr;
					}

					i++;
				}
				//Stop the stopwatch and write the baud rate and time to the CSV-file
				sw.Stop();

				Console.Write(baud + "," + ((sw.ElapsedMilliseconds / 1000.0) / samples) + "\n");
				File.AppendAllText("Data.csv", baud + "," + ((sw.ElapsedMilliseconds / 1000.0) / samples) + "\n");
			}

			port.Close();

			Console.ReadLine();
		}

		//Check if Arduino and computer have the same baud rate and make sure we don't recieve data from the Arduino at the wrong baudrate
		private static void Handshake(int baudI, uint baud) {
			//Recieve bytes from Arduino, if it doesn't equal our baud rate, then try to read the next one
			while (port.ReadByte() != baudI);

			//Remove everyting from in buffer, so it isn't wrong data and setup the new baudrate
			port.DiscardInBuffer();
			port.Close();
			port.BaudRate = (int)baud;
			port.Open();

			//Write the current baud rate back to the Arduino, to signal it to continue
			port.Write(new Byte[] { (byte)baudI }, 0, 1);
		}
	}
}
