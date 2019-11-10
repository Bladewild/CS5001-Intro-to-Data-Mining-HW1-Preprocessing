using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace HW1
{
    class Program
    {

        struct FixValue
        {
            public string error;
            public string fix;
            public FixValue(string inputError, string inputFix)
            {
                error = inputError;
                fix = inputFix;
            }

            public override string ToString()
            {
                return "[" + error + "," + fix + "]";
            }
        }
        
        static List<FixValue> FixList = new List<FixValue>();
        
        static void Main(string[] args)
        {
            //Can be made into seperate function where input is read through text
            FixList.Add( new FixValue("N","0"));
            FixList.Add( new FixValue("Y","1"));
            FixList.Add( new FixValue("NA",""));
            FixList.Add( new FixValue("\"N\"","0"));
            FixList.Add( new FixValue("\"Y\"","1"));

            List<List<string>> Lines = new List<List<string>>();
            string fread = "5402_dataset.csv";
            string fwrite = "5402_datasetfix.csv";
            Lines = ReadFile(fread);
            Lines = ProprocessLines(Lines);
            WriteFile(fwrite, Lines);


            //output
            /*
            StreamWriter file = new System.IO.StreamWriter("test.csv");
            
            foreach (var item in Lines)
            {
                Console.Write(item);       
            }
            file.Close();
            */
        }


        #region I/0
        private static List<List<string>> ReadFile(string filetoRead)
        {
            // Taking a new input stream i.e.  
            StreamReader sr = new StreamReader(filetoRead);
            List<List<string>> linecontainer = new List<List<string>>();


            // This is use to specify from where  
            // to start reading input stream 
            sr.BaseStream.Seek(0, SeekOrigin.Begin);

            string[] input;


            while (sr.EndOfStream == false)
            {
                input = sr.ReadLine().Split(',');
                List<string> inputLine = new List<string>();
                inputLine.AddRange(input);
                linecontainer.Add(inputLine);
                Console.WriteLine(OutputLine(inputLine));
                //Console.WriteLine("-------------------------------------------");
            }
            /*
            while (sr.EndOfStream == false)
            {
            }*/
            // to close the stream 
            sr.Close();
            return linecontainer;
        }

        private static void WriteFile(string filetoWrite, List<List<string>> LinestoWrite)
        {
            StreamWriter file = new System.IO.StreamWriter(filetoWrite);

            foreach (var item in LinestoWrite)
            {
                file.WriteLine(OutputLine(item));
            }
            file.Close();
        }

        private static string OutputLine(List<string> line)
        {
            string s = "";
            for (int i = 0; i < line.Count; i++)
            {
                s += line[i];

                if (i <= line.Count - 2)
                    s += ",";
            }
            return s;

        }
        #endregion

        private static List<List<string>> ProprocessLines(List<List<string>> linestoProcess)
        {

            for (int i = 0; i < linestoProcess.Count; i++)
            {
                linestoProcess[i] = ProprocessLine(linestoProcess[i]);
            }
            return linestoProcess;

        }
        private static List<string> ProprocessLine(List<string> linetoProcess)
        {
            List<string> linetoModify = linetoProcess;
            for (int i = 0; i < linetoProcess.Count; i++)
            {
                string stringtomodify = linetoModify[i];
                for (int j = 0; j < FixList.Count; j++)
                {
                    if(stringtomodify == FixList[j].error)
                    {
                        linetoModify[i]=FixList[j].fix;                           
                        break;
                    }                 
                }
            }
            return linetoModify;

        }
    }
}
