#include <SoftwareSerial.h>

void setup() {
}

uint32_t bauds[] = {300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 57600, 76800, 115200, 230400, 250000, 500000};
void loop() {
  for (uint8_t baudI = 0; baudI < 16; baudI++) {
    uint32_t baud = bauds[baudI];
    Serial.flush();
    Serial.begin(baud);
    for (uint32_t i = 0; i < baud; i++) {
      while(!Serial.available());
      Serial.print((char) Serial.read());
    }
  }
}
