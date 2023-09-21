using System;

namespace FFXIIIWMPtool
{
    internal static class CmnMethods
    {
        public static void ErrorExit(string errorMsg)
        {
            Console.WriteLine(errorMsg);
            Console.ReadLine();
            Environment.Exit(0);
        }

        public enum ActionEnums
        {
            u,
            r
        }
    }
}