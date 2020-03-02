void setup() {
  Serial.begin(9600);
}

void loop() {
  if (Serial.available()) {
    Serial.println();
    
    while (Serial.available()) {
      Serial.print((char) Serial.read());
    }
  }
}
