using System;
using System.IO;

namespace FFXIIIWMPtool
{
    internal partial class WMP
    {
        public static void UnpackWMP(string inMovieItemsDbFile, string inWMPfile)
        {
            var inWMPfileDir = Path.GetDirectoryName(inWMPfile);

            var inMovieItemsDbFileName = Path.GetFileName(inMovieItemsDbFile);
            var inWMPfileName = Path.GetFileName(inWMPfile);

            var wmpId = Path.GetFileNameWithoutExtension(inWMPfileName).Replace(".win32", "").Replace(".ps3", "").
                Replace("x360", "").Replace("_us", "");
            var wmpOutFolder = Path.GetFileNameWithoutExtension(inWMPfileName).Replace(".win32", "").Replace(".ps3", "").
                Replace("x360", "");

            var wmpVars = new WMPVariables();
            WMPMethods.ProcessVariables(inMovieItemsDbFileName, wmpVars);


            using (var dbStream = new FileStream(inMovieItemsDbFile, FileMode.Open, FileAccess.Read))
            {
                using (var dbReader = new BinaryReader(dbStream))
                {
                    dbReader.BaseStream.Position = 4;
                    var totalFMVs = dbReader.ReadBytesUInt32(true) - 4;

                    dbReader.BaseStream.Position = 32;
                    var wmpIdsPos = dbReader.ReadBytesUInt32(true);


                    uint startVal = 144;
                    var fmvSizeValBuffer = new byte[4];
                    int unpackCount = 0;

                    for (int f = 0; f < totalFMVs; f++)
                    {
                        dbReader.BaseStream.Position = startVal;
                        var fmvName = dbReader.ReadStringTillNull();

                        dbReader.BaseStream.Position = startVal + 16;
                        var fmvInfoPos = dbReader.ReadBytesUInt32(true);

                        dbReader.BaseStream.Position = fmvInfoPos;
                        var fmvWMPidPos = dbReader.ReadBytesUInt32(true);

                        dbReader.BaseStream.Position = wmpIdsPos + fmvWMPidPos;
                        var fmvWMPid = dbReader.ReadStringTillNull();

                        dbReader.BaseStream.Position = fmvInfoPos + 4;
                        var fmvSize = dbReader.ReadBytesUInt32(true);
                        var fmvStart = dbReader.ReadBytesUInt64(true);

                        if (fmvWMPid == wmpId)
                        {
                            var fmvExtractDir = Path.Combine(inWMPfileDir, wmpOutFolder);
                            if (!Directory.Exists(fmvExtractDir))
                            {
                                Directory.CreateDirectory(fmvExtractDir);
                            }

                            var fmvFile = Path.Combine(fmvExtractDir, fmvName + wmpVars.VoSuffix + wmpVars.FmvFileExtension);

                            Console.WriteLine("Unpacking file " + Path.GetFileName(fmvFile) + "....");

                            using (var wmpStream = new FileStream(inWMPfile, FileMode.Open, FileAccess.Read))
                            {
                                if (File.Exists(fmvFile))
                                {
                                    File.Delete(fmvFile);
                                }

                                using (var fmvOutStream = new FileStream(fmvFile, FileMode.OpenOrCreate, FileAccess.Write))
                                {
                                    if (wmpVars.FmvFileExtension == ".bik")
                                    {
                                        wmpStream.Seek((long)fmvStart + 4, SeekOrigin.Begin);
                                        wmpStream.Read(fmvSizeValBuffer, 0, 4);
                                        fmvSize = BitConverter.ToUInt32(fmvSizeValBuffer, 0) + 8;
                                    }

                                    wmpStream.Seek((long)fmvStart, SeekOrigin.Begin);
                                    wmpStream.CopyStreamTo(fmvOutStream, fmvSize, false);
                                }

                                unpackCount++;
                            }

                            Console.WriteLine("Unpacked file " + fmvFile);
                            Console.WriteLine("");
                        }

                        startVal += 32;
                    }

                    Console.WriteLine("");

                    if (unpackCount == 0)
                    {
                        Console.WriteLine("Warning: No files were unpacked from " + "\"" + Path.GetFileName(inWMPfile) + "\"");
                        Console.WriteLine("");
                    }
                }
            }

            Console.WriteLine("Finished unpacking file(s) from " + "\"" + Path.GetFileName(inWMPfile) + "\"");
        }
    }
}