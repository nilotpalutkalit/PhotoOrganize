using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PhotoGroup
{
    public class AttributeParser
    {
        public AttributeParser()
        {
        }

        public Dictionary<string, string> GetAttribute(string fileName)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "mdls",
                Arguments = "\"" + string.Format(fileName) + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process process = new Process()
            {
                StartInfo = startInfo,
            };
            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            Regex regex = new Regex(@"([_k]+.*):?=(.*)");
            var match = regex.Matches(result);
            foreach (Match eachMatch in match)
            {
                attributes.Add(eachMatch.Groups[1].ToString().Trim(), eachMatch.Groups[2].ToString().Trim('\"',' '));
            }

            return attributes;
        }
    }
}
