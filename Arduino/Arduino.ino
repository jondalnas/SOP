#include <SoftwareSerial.h>

void setup() {
  Serial.begin(9600);
}

uint32_t bauds[] = {9600, 14400, 19200, 38400, 57600, 76800, 115200, 230400, 250000, 500000, 1000000, 2000000};
void loop() {
    
  for (uint8_t baudI = 0; baudI < 12; baudI++) {
    uint32_t baud = bauds[baudI];
    
    handshake(baudI, baud);
    
    for (uint32_t i = 0; i < baud<<2; i++) {
      Serial.print((char) (i % 9 + 1));
    }
    Serial.print((char) 0);
  }
  
  while(1);
}

void handshake(uint8_t baudI, uint32_t baud) {
  Serial.write((byte*)&baudI, 1);
    
  Serial.flush();
  Serial.begin(baud);
  
  while (Serial.read() != baudI);
}

