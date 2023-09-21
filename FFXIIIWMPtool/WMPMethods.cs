namespace FFXIIIWMPtool
{
    internal partial class WMP
    {
        static void FMVextension(string inMovieItemsDbFileName, WMP wmpVars)
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
        }

        static void VOSuffix(string inMovieItemsDbFileName, WMP wmpVars)
        {
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