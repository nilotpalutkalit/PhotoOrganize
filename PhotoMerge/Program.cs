using System;
using System.IO;
using System.Collections.Generic;

namespace PhotoMerge
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            try
            {
                // Set a variable to the My Documents path.
                string fromPath = "/Volumes/ExFat/ExportedImages/";
                string toPath = "/Volumes/ExFat/MacOSXPictures_07022022/";
                string MergePath = "/Volumes/ExFat/Merged/";

                HashSet<string> toSet = new HashSet<string>(Directory.EnumerateFiles(fromPath));
                HashSet<string> fromSet = new HashSet<string>(Directory.EnumerateFiles(toPath));
                toSet = MergeSet(ref fromSet, ref toSet);
                if (!Directory.Exists(MergePath))
                {
                    Directory.CreateDirectory(MergePath);
                }

                foreach (var fileName in toSet)
                {
                    var pathName = Path.Combine(toPath, fileName);
                    if (File.Exists(pathName))
                    {

                        if (!File.Exists(Path.Combine(MergePath, fileName)))
                        {
                            File.Copy(pathName, Path.Combine(MergePath, fileName));
                        }

                        continue;
                    }

                    Console.WriteLine("Does not exists : " + pathName + " Exists : " + Path.Combine(fromPath, fileName) + " " + File.Exists(Path.Combine(fromPath, fileName)));
                    if (File.Exists(Path.Combine(fromPath, fileName)))
                    {
                        if (!File.Exists(Path.Combine(MergePath, fileName)))
                        {
                            File.Copy(Path.Combine(fromPath, fileName), Path.Combine(MergePath, fileName));
                        }
                    }


                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (PathTooLongException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static HashSet<string> MergeSet(ref HashSet<string> from, ref HashSet<string> to)
        {
            HashSet<string> fileNames = new HashSet<string>();
            foreach (var str in to)
            {
                fileNames.Add(Path.GetFileName(str));
            }
            foreach (var str in from)
            {
                if (fileNames.Contains(Path.GetFileName(str)))
                {
                    Console.WriteLine(Path.GetFileName(str));
                }
                fileNames.Add(Path.GetFileName(str));
            }

            return fileNames;
        }
    }
}
