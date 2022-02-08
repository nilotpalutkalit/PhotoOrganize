using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

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
                    var extension = Path.GetExtension(fileName).TrimStart('.');  
                    if (attributes.ContainsKey("kMDItemAcquisitionModel") && attributes.ContainsKey("kMDItemAcquisitionMake"))
                    {
                        var folderName = Path.Combine(Path.GetDirectoryName(fileName), attributes["kMDItemAcquisitionMake"] + " " + attributes["kMDItemAcquisitionModel"]);
                        if(!Directory.Exists(folderName))
                        {
                            Directory.CreateDirectory(folderName);
                        }
                        
                        var destinationFileName = Path.Combine(folderName, Path.GetFileName(fileName));
                        File.Move(fileName, destinationFileName);
                    }
                    else if(!extension.Equals("jpeg") && !extension.Equals("png") &&
                        !extension.Equals("heic") && !extension.Equals("DS_Store"))
                    {

                        var folderName = Path.Combine(Path.GetDirectoryName(fileName), Path.GetExtension(fileName).TrimStart('.'));
                        if (!Directory.Exists(folderName))
                        {
                            Directory.CreateDirectory(folderName);
                        }

                        var destinationFileName = Path.Combine(folderName, Path.GetFileName(fileName));
                        File.Move(fileName, destinationFileName);

                    }
                    else if(attributes.ContainsKey("kMDItemPixelHeight") && attributes.ContainsKey("kMDItemPixelWidth"))
                    {
                        var width = int.Parse(attributes["kMDItemPixelWidth"]);
                        var height = int.Parse(attributes["kMDItemPixelHeight"]);
                        var dimensionMax = (width > height) ? width : height;
                        if(dimensionMax<720)
                        {
                            continue;
                        }
                        dimensionMax = dimensionMax - (dimensionMax % 100);
                        var folderName = Path.Combine(Path.GetDirectoryName(fileName), dimensionMax.ToString());
                        if (!Directory.Exists(folderName))
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
