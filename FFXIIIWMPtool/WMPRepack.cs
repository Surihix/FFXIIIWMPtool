using BinaryReaderEx;
using BinaryWriterEx;
using StreamExtension;
using System;
using System.IO;

namespace FFXIIIWMPtool
{
    internal partial class WMP
    {
        public static void RepackWMP(string inMovieItemsDbFile, string inWMPfolder)
        {
            Console.WriteLine("");

            var inMovieItemsDbFileName = Path.GetFileName(inMovieItemsDbFile);
            var inWMPfolderName = Path.GetFileName(inWMPfolder);
            var wmpId = inWMPfolderName.Replace("_us", "");

            var wmpVars = new WMP();
            FMVextension(inMovieItemsDbFileName, wmpVars);
            VOSuffix(inMovieItemsDbFileName, wmpVars);

            var inWMPfolderDir = Path.GetDirectoryName(inWMPfolder);
            var newWMPfile = Path.Combine(inWMPfolderDir, wmpId + wmpVars.VoSuffix + wmpVars.PlatformCode + ".wmp");

            if (File.Exists(newWMPfile))
            {
                File.Delete(newWMPfile);
            }


            using (var dbStream = new FileStream(inMovieItemsDbFile, FileMode.Open, FileAccess.ReadWrite))
            {
                using (var dbReader = new BinaryReader(dbStream))
                {
                    using (var dbWriter = new BinaryWriter(dbStream))
                    {
                        dbReader.BaseStream.Position = 4;
                        var totalFMVs = dbReader.ReadBytesUInt32(true) - 4;

                        dbReader.BaseStream.Position = 32;
                        var wmpIdsPos = dbReader.ReadBytesUInt32(true);


                        uint startVal = 144;
                        int repackCount = 0;

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

                            if (fmvWMPid.Equals(wmpId))
                            {
                                var fmvFile = Path.Combine(inWMPfolderDir, inWMPfolderName, fmvName + wmpVars.VoSuffix + wmpVars.FmvFileExtension);

                                if (File.Exists(fmvFile))
                                {
                                    var newFMVsize = (uint)new FileInfo(fmvFile).Length;

                                    if (!File.Exists(newWMPfile))
                                    {
                                        using (var newWMP = new StreamWriter(newWMPfile))
                                        {
                                            newWMP.Write("WMP\0");
                                            newWMP.Write("Ver:0.01");
                                            newWMP.Write("\0\0\0\0");
                                        }
                                    }

                                    var newFMVstart = (ulong)new FileInfo(newWMPfile).Length;

                                    Console.WriteLine("Repacking " + fmvFile + " to " + Path.GetFileName(newWMPfile));

                                    using (var newWMPStream = new FileStream(newWMPfile, FileMode.Append, FileAccess.Write))
                                    {
                                        using (var fmvStream = new FileStream(fmvFile, FileMode.Open, FileAccess.Read))
                                        {
                                            fmvStream.ExCopyTo(newWMPStream, 0, newFMVsize);
                                        }
                                    }

                                    dbWriter.BaseStream.Position = fmvInfoPos + 4;
                                    dbWriter.WriteBytesUInt32(newFMVsize, true);

                                    dbWriter.BaseStream.Position = fmvInfoPos + 8;
                                    dbWriter.WriteBytesUInt64(newFMVstart, true);

                                    repackCount++;

                                    Console.WriteLine("Repacked data to " + Path.GetFileName(newWMPfile));
                                    Console.WriteLine("");
                                }
                            }

                            startVal += 32;
                        }

                        Console.WriteLine("");

                        if (repackCount.Equals(0))
                        {
                            Console.WriteLine("Warning: No file(s) were repacked");
                            Console.WriteLine("");
                            Console.WriteLine("");
                        }
                    }
                }
            }

            Console.WriteLine("Finished repacking file(s) into " + Path.GetFileName(newWMPfile));
            Console.ReadLine();
        }
    }
}