#define SOFTWARE 0

#if SOFTWARE
  #include <SoftwareSerial.h>
  
  SoftwareSerial serial(0, 1);
#endif

void setup() {
  //Setup initial communication
  #if SOFTWARE
    serial.begin(9600);
  #else
    Serial.begin(9600);
  #endif
}

uint32_t bauds[] = {9600, 14400, 19200, 38400, 57600, 76800, 115200, 230400, 250000, 500000, 1000000, 2000000, 4000000, 8000000, 16000000};
void loop() {
  //Loop through all 12 baud rates and test them
  for (uint8_t baudI = 0; baudI < 12; baudI++) {
    uint32_t baud = bauds[baudI];

    //Make sure that the program is listening at the right frequency
    handshake(baudI, baud);

    //Test each baud rate for n*4 times, where n is the baud rate,
    //so it doesn't take too little time to finish a baud rate test,
    //for the C# program to be able to take time
    for (uint32_t i = 0; i < baud; i++) {
      //Write 1-9 in binary
      #if SOFTWARE
        serial.write((byte) (i % 9 + 1));
      #else
        Serial.write((byte) (i % 9 + 1));
      #endif
      //Delay the program for a set amount of time, to simulate another process running
      //delay(1);
    }

    //Send a stop byte, so the program C# program knows to make the next readings
    #if SOFTWARE
      serial.write((byte) 0);
    #else
      Serial.write((byte) 0);
    #endif
  }
  
  while(1);
}

//Handshake makes sure that the Arduino and C# program is in sync and that they don't send data at the wrong baud rate
void handshake(uint8_t baudI, uint32_t baud) {
  //Write the current baud rate index, so the C# program knows where we are in the list
  //Then delete any old messages recieved from the C# program and begin a new connection at the new baud rate
  //Wait for the C# program to respond with the current baud rate index at the agreed baud rate
  #if SOFTWARE
    serial.write((byte*)&baudI, 1);
    
    serial.flush();
    serial.begin(baud);
    
    while (serial.read() != baudI);
  #else
    Serial.write((byte*)&baudI, 1);
    
    Serial.flush();
    Serial.begin(baud);
    
    while (Serial.read() != baudI);
  #endif
}

