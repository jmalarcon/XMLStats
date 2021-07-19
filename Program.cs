using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace XMLStats
{
    class Program
    {
        //Private vars to keep simple file and folder counts
        private static int numFolders = 0;
        private static int numFiles = 0;
        private static int numElements = 0;
        private static int numAttrs = 0;
        private static int numTextNodes = 0;
        private static int numCData = 0;
        private static int numEntityRefs = 0;
        private static int numEntities = 0;
        private static int numProcInstrs = 0;
        private static int numComments = 0;
        private static int numWords = 0;

        static void Main(string[] args)
        {
            if (args.Length == 0 || (args[0] == "-?") || (args[0] == "/?"))
            {
                ShowHelp();
                return;
            }

            string path = Path.GetFullPath(args[0]);
            if (!Directory.Exists(path))
            {
                Console.WriteLine("Folder \"{0}\" does not exist. Exiting...", path);
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();
            Processfolder(path);
            ConsoleWriteInLine("");
            sw.Stop();
            Console.WriteLine(@$"Stats for the XML files:
Elapsed time: {sw.Elapsed}
Folders: {numFolders}
Files: {numFiles}
Elements: {numElements}
Text Nodes: {numTextNodes}
CDATA Nodes: {numCData}
Words: {numWords}
Comments: {numComments}
Attributes: {numAttrs}
Entities: {numEntities}
Entity References: {numEntityRefs}
Processing Instructions: {numProcInstrs}");
        }

        /// <summary>
        /// Shows help for the current implementing command
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine("Cycles through all the .xml files in a folder and subfolders and shows the numbers for the all the files including the number of words in nodes (excluding comments).");
            Console.WriteLine("Usage: XMLStats folder");
            Console.WriteLine("Ej: XMLStats C:\\MyFolder");
            Console.WriteLine("(c) José M. Alarcón [www.campusMVP.es]");
        }

        /// <summary>
        /// Process folder
        /// </summary>
        /// <param name="path">Path of the current folder</param>
        private static void Processfolder(string path)
        {
            numFolders++;
            
            //Cycle through files in the current folder
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.GetFiles("*.xml").ToArray<FileInfo>();
            foreach (FileInfo file in files)
            {
                try
                {
                    ProcessFile(file.FullName); //This is the main processing
                    numFiles++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading file {0}: {1}", file.FullName, ex.Message);
                    Console.WriteLine("");
                }
            }

            //Cycle through the subfolders
            DirectoryInfo[] folders = di.GetDirectories();

            foreach (DirectoryInfo folder in folders)
            {
                Processfolder(folder.FullName);
            }
        }

        /// <summary>
        /// Implements the main functionality
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private static void ProcessFile(string fullName)
        {
            ConsoleWriteInLine($"Processing file {fullName}");

            XmlTextReader xmlTextReader = new XmlTextReader(fullName);
            while (xmlTextReader.Read())
            {
                switch (xmlTextReader.NodeType)
                {
                    case XmlNodeType.Element:
                        numElements++;
                        break;
                    case XmlNodeType.Attribute:
                        numAttrs++;
                        break;
                    case XmlNodeType.Text:
                        numWords += CountNumOfWords(xmlTextReader.Value);
                        numTextNodes++;
                        break;
                    case XmlNodeType.CDATA:
                        numWords += CountNumOfWords(xmlTextReader.Value);
                        numCData++;
                        break;
                    case XmlNodeType.EntityReference:
                        numEntityRefs++;
                        break;
                    case XmlNodeType.Entity:
                        numEntities++;
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        numProcInstrs++;
                        break;
                    case XmlNodeType.Comment:
                        //numWords += CountNumOfWords(xmlTextReader.Value);
                        numComments++;
                        break;
                    case XmlNodeType.DocumentFragment:
                        break;
                    default:
                        break;
                }
            }
        }

        private static int CountNumOfWords(string text)
        {
            return text.Trim().Split(' ').Length;
        }

        private static int maxConsoleLineLen = 0;
        private static void ConsoleWriteInLine(string msg)
        {
            //This is not shown if the console is redirect to a different devide (i.e., a file)
            if (Console.IsOutputRedirected) return;

            //Clean current console line
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new String(' ', maxConsoleLineLen));
            if (msg.Length > maxConsoleLineLen) maxConsoleLineLen = msg.Length;

            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(msg);
        }
    }

}
