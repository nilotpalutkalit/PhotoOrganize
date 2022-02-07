using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PhotoGroup
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            AttributeParser attributeParser = new AttributeParser();
            Console.WriteLine("Hello World!");
            try
            {
                string PhotoPath = "/Volumes/Fast SSD/MacOSXPictures_All";

                HashSet<string> allPhotos = new HashSet<string>(Directory.EnumerateFiles(PhotoPath));

                foreach (var fileName in allPhotos)
                {
                    if(fileName.StartsWith("."))
                    {
                        continue;
                    }

                    var attributes = attributeParser.GetAttribute(fileName);
                    if(attributes.ContainsKey("kMDItemAcquisitionModel") && attributes.ContainsKey("kMDItemAcquisitionMake"))
                    {
                        var folderName = Path.Combine(Path.GetDirectoryName(fileName), attributes["kMDItemAcquisitionMake"] + " " + attributes["kMDItemAcquisitionModel"]);
                        if(!Directory.Exists(folderName))
                        {
                            Directory.CreateDirectory(folderName);
                        }

                        var destinationFileName = Path.Combine(folderName, Path.GetFileName(fileName));
                        File.Move(fileName, destinationFileName);
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


    }
}
