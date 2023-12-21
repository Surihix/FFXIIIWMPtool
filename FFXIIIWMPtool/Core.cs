using System;
using System.IO;

namespace FFXIIIWMPtool
{
    internal class Core
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                CmnMethods.ErrorExit("Error: Enough arguments not specified\n" +
                    "\nFor Unpacking: FFXIIIWMPtool.exe -u \"movie_items db file\" \"WMP file\" " +
                    "\nFor Repacking: FFXIIIWMPtool.exe -r \"movie_items db file\" \"unpacked WMP folder\"");
            }

            var actionEnumString = args[0].Replace("-", "");

            var convertedAction = new CmnMethods.ActionEnums();
            if (Enum.TryParse(actionEnumString, false, out CmnMethods.ActionEnums resultEnum))
            {
                convertedAction = resultEnum;
            }

            var inMovieItemsDbFile = args[1];
            var inWMPFileOrFolder = args[2];

            if (!File.Exists(inMovieItemsDbFile))
            {
                CmnMethods.ErrorExit("Error: Specified movie items db file does not exist");
            }

            try
            {
                switch (convertedAction)
                {
                    case CmnMethods.ActionEnums.u:
                        if (!File.Exists(inWMPFileOrFolder))
                        {
                            CmnMethods.ErrorExit("Error: Specified WMP file does not exist");
                        }

                        WMP.UnpackWMP(inMovieItemsDbFile, inWMPFileOrFolder);
                        break;

                    case CmnMethods.ActionEnums.r:
                        if (!Directory.Exists(inWMPFileOrFolder))
                        {
                            CmnMethods.ErrorExit("Error: Specified unpacked WMP directory to repack does not exist");
                        }

                        WMP.RepackWMP(inMovieItemsDbFile, inWMPFileOrFolder);
                        break;

                    default:
                        CmnMethods.ErrorExit("Error: Proper tool action is not specified.\nMust be: '-u' or '-r'");
                        break;
                }
            }
            catch (Exception ex)
            {
                CmnMethods.ErrorExit("Error: " + ex);
            }
        }
    }
}