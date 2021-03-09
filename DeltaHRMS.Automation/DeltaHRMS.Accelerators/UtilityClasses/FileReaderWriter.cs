using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace DeltaHRMS.Accelerators.UtilityClasses
{
    public class FileReaderWriter
    {
        /// <summary>
        /// Reads all lines in text file
        /// </summary>
        /// <param name="txtFilepath"></param>
        /// <returns></returns>
        public List<string> ReadAllLinesInTextFile(string txtFilepath)
        {
            List<string> lines = null;
            if (string.IsNullOrEmpty(txtFilepath))
            {
                return lines;
            }
            lines = new List<string>();
            string value = string.Empty;
            using (StreamReader sr = new StreamReader(txtFilepath))
            {
                value = sr.ReadLine();
                while (value != null)
                {
                    lines.Add(value);
                    value = sr.ReadLine();
                }
            }
            return lines;
        }

        /// <summary>
        /// Copies File To Current Working Director.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool CopyFileToCurrentWorkingDirector(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("Argument filePath cannot be empty or null");
            }
            bool result = false;
            FileInfo fileInfo = new FileInfo(filePath);
            string currentDirPath = Directory.GetCurrentDirectory() + "\\" + fileInfo.Name;
            if (!File.Exists(filePath))
            {
                throw new Exception(string.Format("File does not exit in {0}", filePath));
            }
            else
            {
                if (!File.Exists(currentDirPath))
                    File.Copy(filePath, currentDirPath);
                else
                {
                    File.Delete(currentDirPath);
                    File.Copy(filePath, currentDirPath);
                }

                if (!File.Exists(currentDirPath))
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Delete File or list of files.
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="fileNameList"></param>
        public void DeleteFiles(string dirPath, string[] fileNameList)
        {
            if (string.IsNullOrEmpty(dirPath) && fileNameList.Length <= 0)
            {
                throw new Exception("Arguments cannot be null or empty");
            }
            foreach (string fileName in fileNameList)
            {
                string filePath = string.Format(dirPath + "\\{0}", fileName);
                FileInfo fileInfo = new FileInfo(filePath);
                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Deletes all files in specified path.
        /// </summary>
        /// <param name="dirPath"></param>
        public void DeleteFiles(string dirPath, string pattern)
        {
            if (string.IsNullOrEmpty(dirPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("dirpath is empty");
                Console.ResetColor();
                return;
            }
            if(!Directory.Exists(dirPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("dirpath do not exist");
                Console.ResetColor();
                return;
            }

            string[] fileNameList = Directory.GetFiles(dirPath, pattern);

            foreach (string fileName in fileNameList)
            {
                try
                {
                    File.Delete(fileName);
                    Console.WriteLine("{0} got deleted", fileName);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Get Image Hash
        /// </summary>
        /// <param name="bmpSource"></param>
        /// <returns></returns>
        public static List<bool> GetImageHash(Bitmap bmpSource)
        {
            List<bool> lResult = new List<bool>();
            //create new image with 16x16 pixel
            Bitmap bmpMin = new Bitmap(bmpSource, new Size(16, 16));
            for (int j = 0; j < bmpMin.Height; j++)
            {
                for (int i = 0; i < bmpMin.Width; i++)
                {
                    //reduce colors to true / false                
                    lResult.Add(bmpMin.GetPixel(i, j).GetBrightness() < 0.5f);
                }
            }
            return lResult;
        }

        /// <summary>
        /// Create A File
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public static bool CreateAFile(string fileName, string fileContent)
        {
            if (string.IsNullOrEmpty(fileName) && string.IsNullOrEmpty(fileContent))
            {
                throw new Exception("Arguments cannot be null or empty");
            }
            bool result = false;
            try
            {
                using (StreamWriter sr = new StreamWriter(fileName))
                {
                    sr.Write(fileContent);
                    sr.Flush();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Read Content From File
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadContentFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new Exception("Arguments cannot be null or empty");
            }
            string content = string.Empty;
            try
            {
                using (StreamReader str = new StreamReader(fileName))
                {
                    content = str.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("In ReadContentFromFile() " + ex.Message);
            }
            return content;
        }
    }
}