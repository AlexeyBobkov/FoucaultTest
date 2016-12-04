int req = 5; //mic REQ line goes to pin 5 through q1 (arduino high pulls request line low)
int dat = 2; //mic Data line goes to pin 2
int clk = 3; //mic Clock line goes to pin 3

void setup()
{
    Serial.begin(115200);
    pinMode(req, OUTPUT);
    pinMode(clk, INPUT_PULLUP);
    pinMode(dat, INPUT_PULLUP);
    digitalWrite(req,LOW); // set request at LOW
}

static int poll(byte *data)
{
    int i, j, k, num = 1;

    // get data from mic
    unsigned long tmo = millis() + 150;

    for(i = 0; i < 13; i++ )
    {
        k = 0;
        for (j = 0; j < 4; j++)
        {
            while( digitalRead(clk) == LOW)     // hold until clock is high
            {
                if(millis() > tmo)
                    return num;
            }
            ++num;
            tmo = millis() + 150;
            while( digitalRead(clk) == HIGH)    // hold until clock is low
            {
                if(millis() > tmo)
                    return num;
            }
            ++num;
            tmo = millis() + 150;

            bitWrite(k, j, (digitalRead(dat) & 0x1)); // read data bits, and reverse order )
        }

        // extract data
        data[i] = k;
    }
    return 0;
}

void loop()
{
    if (Serial.available() <= 0 || Serial.read() != 'm')
        return;

    byte mydata[14];
    digitalWrite(req, HIGH); // generate set request

    int num = poll(mydata);
    Serial.write(byte(num));
    for(int i = 4; i < 13; ++i)
        Serial.write(mydata[i]);

    digitalWrite(req, LOW); // set request at LOW
}
