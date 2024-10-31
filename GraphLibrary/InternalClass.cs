using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLibrary
{
    public class InternalClass
    {
        public static Action? Wait;
        public static void WaitNext()
        {
            Wait?.Invoke();
        }

        public static Action<string>? PrintAfter;
        public static void Print(string str)
        {
            PrintAfter?.Invoke(str);
        }
    }
}
