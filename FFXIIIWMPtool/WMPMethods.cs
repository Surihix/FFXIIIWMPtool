using System;

namespace FFXIIIWMPtool
{
    internal class WMPMethods
    {
        public static void ErrorExit(string errorMsg)
        {
            Console.WriteLine(errorMsg);
            Console.ReadLine();
            Environment.Exit(1);
        }

        public static void ProcessVariables(string inMovieItemsDbFileName, WMPVariables wmpVars)
        {
            if (inMovieItemsDbFileName.Contains(".win32."))
            {
                wmpVars.PlatformCode = ".win32";
                wmpVars.FmvFileExtension = ".bik";
            }
            if (inMovieItemsDbFileName.Contains(".ps3."))
            {
                wmpVars.PlatformCode = ".ps3";
                wmpVars.FmvFileExtension = ".pam";
            }
            if (inMovieItemsDbFileName.Contains(".x360."))
            {
                wmpVars.PlatformCode = ".x360";
                wmpVars.PlatformCode = ".bik";
            }

            if (inMovieItemsDbFileName.Contains("us."))
            {
                wmpVars.VoSuffix = "_us";
            }
            else
            {
                wmpVars.VoSuffix = "";
            }
        }
    }
}