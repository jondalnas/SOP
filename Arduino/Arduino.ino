#include <SoftwareSerial.h>

void setup() {
  Serial.begin(9600);
}

//uint32_t bauds[] = {/*300, 600, 1200, 2400, 4800, */9600, 14400, 19200, 28800, 38400, 57600, 76800, 115200, 230400, 250000, 500000};
String bytesArray[] = {"a", "aa", "aaaa"};
void loop() {
  while (!Serial.available());
    
  //for (uint8_t baudI = 0; baudI < 11; baudI++) {
  //  uint32_t baud = bauds[baudI];
  uint32_t baud = 9600;
  Serial.flush();
  Serial.begin(baud);
  
  for (uint8_t bytes = 0; bytes < 3; bytes++) {
    for (uint32_t i = 0; i < baud; i++) {
      Serial.print(bytesArray[bytes]);
    }
    Serial.print(";");
    //while (!Serial.available());
    //Serial.read();
  }
}
