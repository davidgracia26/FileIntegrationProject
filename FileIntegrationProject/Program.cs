using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileIntegrationProject
{
    class Program
    {
        ///<summary>
        ///  The File Combiner
        ///We need an application that will traverse a folder and take all the files of a specific name and combine them into a single file.
        ///Eg.
        ///HTML Basics
        ///    - 03.0 Structure Elements
        ///    - 03.1 HTML5 Structure
        ///    - 04.0 Content Tags
        ///    - 04.1 Intro
        ///    - 04.2 Heading Tags
        ///    - 04.3 Paragraphs

        ///1) The application needs to append the content from 03.0 & 03.1 into a document called 03 Structure Elements.
        ///2) The name of the first item(#.0) will be the name of the combined document. All point numbered documents will be combined into the whole number value.
        ///3) Only.md files will be combined.
        ///4) The source or root folder needs to be configurable(use a config file).
        ///5) Only files that start with a number and have a period in the number will be combined.
        ///6) The app should traverse all the folders from the root down combining all the files it finds.
        ///</summary>
        
        static void Main(string[] args)
        {
            var root = ConfigurationManager.AppSettings["root"];

            var files = Directory.GetFiles(root).ToList();

            var folders = Directory.GetDirectories(root).ToList();

            //Checks to see if there is any sized number with a decimal at the beginning of a file name
            Regex regex = new Regex(@"^(\d*[0-9])[\.](\d*[0-9])");

            FileScavenger FileScavenger = new FileScavenger { };
            FileScavenger.FileTraverser(files, folders, regex);

            Console.WriteLine("Done.");

            Console.ReadLine();
        }
    }

}
