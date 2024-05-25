using System;
using System.IO;
using System.Linq;

namespace FFXIIIWMPtool
{
    internal class Core
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && args.Contains("-h") || args.Contains("-?"))
            {
                Console.WriteLine("For Unpacking: FFXIIIWMPtool.exe -u \"movie_items db file\" \"WMP file\"");
                Console.WriteLine("For Repacking: FFXIIIWMPtool.exe -r \"movie_items db file\" \"unpacked WMP folder\"");
                Console.ReadLine();
                Environment.Exit(0);
            }

            if (args.Length < 2)
            {
                Console.WriteLine("Warning: Enough arguments not specified. for more info, please launch this tool with -h or -? switches.");
                Console.ReadLine();
                Environment.Exit(0);
            }

            var actionEnumString = args[0].Replace("-", "");

            var convertedAction = new ActionEnums();
            if (Enum.TryParse(actionEnumString, false, out ActionEnums resultEnum))
            {
                convertedAction = resultEnum;
            }

            var inMovieItemsDbFile = args[1];
            var inWMPFileOrFolder = args[2];

            if (!File.Exists(inMovieItemsDbFile))
            {
                WMPMethods.ErrorExit("Error: Specified movie items db file does not exist");
            }

            Console.WriteLine("");

            try
            {
                switch (convertedAction)
                {
                    case ActionEnums.u:
                        if (!File.Exists(inWMPFileOrFolder))
                        {
                            WMPMethods.ErrorExit("Error: Specified WMP file does not exist");
                        }

                        WMP.UnpackWMP(inMovieItemsDbFile, inWMPFileOrFolder);
                        break;

                    case ActionEnums.r:
                        if (!Directory.Exists(inWMPFileOrFolder))
                        {
                            WMPMethods.ErrorExit("Error: Specified unpacked WMP directory to repack does not exist");
                        }

                        WMP.RepackWMP(inMovieItemsDbFile, inWMPFileOrFolder);
                        break;

                    default:
                        WMPMethods.ErrorExit("Error: Proper tool action is not specified.\nMust be: '-u' or '-r'");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                Console.ReadLine();
                Environment.Exit(2);
            }
        }

        enum ActionEnums
        {
            u,
            r
        }
    }
}