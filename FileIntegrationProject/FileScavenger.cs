using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileIntegrationProject
{
    class FileScavenger
    {
        /// <summary>
        /// Adds a file to a list of files in order to create
        /// </summary>
        /// <param name="file">File used as reference for new file</param>
        /// <param name="files">Files to which the new file is added</param>
        /// <returns>A file to add lines to after being read from another file</returns>
        public FileStream CreateNewFileInListOfFiles(string file, List<string> files)
        {
            var x = (int)Double.Parse(Path.GetFileName(file).Split(' ')[0]);

            var name = Path.GetFileName(file).Split(' ').ToList();
            string fileName = "";
            for (int b = 1; b < name.Count; b++)
            {
                fileName += name[b] + " ";
            }

            var newFile = File.Create($"{Path.GetDirectoryName(file)}/{x + " " + fileName}");
            newFile.Close();
            files.Add(newFile.Name);
            return newFile;
        }

        /// <summary>
        ///  Main method that is a series of loops that traverses folders and combines files
        /// </summary>
        /// <param name="files">Files to be combined</param>
        /// <param name="folders">Folders to traverse and combine files within</param>
        /// <param name="regex">Checks for floating point numbers in the beginning of file names</param>
        public void FileTraverser(List<string> files, List<string> folders, Regex regex)
        {

            files.OrderBy(x => x);
            //First go through all the files in the folder and combine them
            for (int k = 0; k < files.Count; k++)
            {
                if (Path.GetExtension(files[k]) == ".md")
                {
                    if (regex.IsMatch(Path.GetFileName(files[k])))
                    {
                        //For first file, check if the file after it is the same module, if it is
                        //create a new file and add the lines from the first file to it
                        if (k == 0)
                        {
                            if (files[k].Split('.')[0] == files[k + 1].Split('.')[0])
                            {
                                var cnf = new FileScavenger();
                                FileStream newFile = cnf.CreateNewFileInListOfFiles(files[k], files);
                                File.AppendAllLines(newFile.Name, File.ReadAllLines(files[k]));
                                File.AppendAllText(newFile.Name, Environment.NewLine);
                            }
                        }
                        //If the modules match, add the lines to the file that was created and placed at the end of the List
                        else if (files[k - 1].Split('.')[0] == files[k].Split('.')[0])
                        {
                            File.AppendAllLines(files[files.Count - 1], File.ReadAllLines(files[k]));
                            File.AppendAllText(files[files.Count - 1], Environment.NewLine);
                        }
                        //On the boundary of two different modules, create a new module for the next set of module files
                        else
                        {
                            var cnf = new FileScavenger();
                            FileStream newFile = cnf.CreateNewFileInListOfFiles(files[k], files);
                            File.AppendAllLines(newFile.Name, File.ReadAllLines(files[k]));
                            File.AppendAllText(newFile.Name, Environment.NewLine);
                        }
                    }
                }
            }
            //Traverses other folders and repeats the steps above until there are no folders
            foreach (string folder in folders)
            {
                FileTraverser(Directory.GetFiles(folder).ToList(), Directory.GetDirectories(folder).ToList(), regex);
            }
        }
    }
}
