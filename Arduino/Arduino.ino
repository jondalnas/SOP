void setup() {
  //Serial.begin(9600);
  Serial.begin(2000000);
}

void loop() {
  if (Serial.available()) {
    while (Serial.available()) {
      Serial.print((char) Serial.read());
      delay(2);
    }
    Serial.println();
  }
}
