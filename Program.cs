using System;
using System.IO;
using System.Collections.Generic;
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

        struct ValueOccurence
        {
            public string value;
            public int numoccurence;
            public ValueOccurence(string inputValue, int inputOccurence)
            {
                value = inputValue;
                numoccurence = inputOccurence;
            }

            public ValueOccurence(string inputValue)
            {
                value = inputValue;
                numoccurence = 1;
            }
            /*
            public void Increase()
            {
                numoccurence++;
            }*/

            public override string ToString()
            {
                return "[" + value + "-timesoccured" + numoccurence + "]";
            }
        }

        struct Coloumn
        {
            private static string nominal = "nominal";
            private static string numeric = "numeric";
            public string title;
            public List<string> instances;
            public List<ValueOccurence> ValueEntries;//for nominal 
            public ValueOccurence majorityValue;//for nominal 
            public List<int> MissingValueIndexes;

            public string type;

            public Coloumn(string titleinput)
            {
                MissingValueIndexes = new List<int>();
                majorityValue = new ValueOccurence("NONE", -1);
                ValueEntries = new List<ValueOccurence>();
                instances = new List<string>();
                title = titleinput;
                string result;

                switch (title)
                {
                    case "\"is_attack\"":
                    case "\"P101\"":
                    case "\"P203\"":
                    case "\"P102\"":
                    case "\"P205\"":
                    case "\"P206\"":
                    case "\"P301\"":
                    case "\"P302\"":
                    case "\"P401\"":
                    case "\"P402\"":
                    case "\"P403\"":
                    case "\"P404\"":
                    case "\"P501\"":
                    case "\"P502\"":
                    case "\"P601\"":
                    case "\"P602\"":
                    case "\"P603\"":
                    case "\"P201\"":
                    case "\"P202\"":
                    case "\"P204\"":
                    case "\"MV301\"":
                    case "\"MV302\"":
                    case "\"MV304\"":
                    case "\"UV401\"":
                    case "\"MV101\"":
                    case "\"MV201\"":
                    case "\"MV303\"":
                        result = nominal;
                        break;
                    default:
                        result = numeric;
                        break;
                }
                type = result;
            }

            public void AddValue(string value)
            {
                if (type == nominal)
                {
                    bool found = false;
                    for (int i = 0; i < ValueEntries.Count; i++)
                    {
                        string test = ValueEntries[i].value;
                        if (ValueEntries[i].value == value)
                        {
                            ValueOccurence newVO = new ValueOccurence(ValueEntries[i].value, ValueEntries[i].numoccurence + 1);
                            ValueEntries[i] = newVO;
                            //Console.WriteLine("INCREASED: "+ValueEntries[i].value+":"+ValueEntries[i].numoccurence);
                            found = true;
                            break;
                        }

                    }
                    if (value != "")
                    {
                        if (found == false)
                            ValueEntries.Add(new ValueOccurence(value));

                    }
                    else
                    {
                        MissingValueIndexes.Add(instances.Count);
                    }
                }
                instances.Add(value);

            }
            public void UpdateMajorityValue()
            {
                
                foreach (var item in ValueEntries)
                {
                    if (item.numoccurence > this.majorityValue.numoccurence)
                    {
                        //Console.WriteLine("LAST MV: " + this.majorityValue.value);
                        this.majorityValue = item;
                        //Console.WriteLine("CURRENT MV: " + this.majorityValue.value);
                    }
                }
            }
            public override string ToString()
            {
                string output = "--" + title + "--\n";
                foreach (var row in instances)
                {
                    output += "row\n";
                }
                output += "--" + title + "--\n";
                return output;
            }

        }

        static List<FixValue> FixList = new List<FixValue>();

        static void Main(string[] args)
        {
            //Can be made into seperate function where input is read through text
            FixList.Add(new FixValue("N", "0"));
            FixList.Add(new FixValue("Y", "1"));
            FixList.Add(new FixValue("NA", ""));
            FixList.Add(new FixValue("\"N\"", "0"));
            FixList.Add(new FixValue("\"Y\"", "1"));

            List<Coloumn> table = new List<Coloumn>();
            List<List<string>> Lines = new List<List<string>>();
            string fread = "5402_dataset.csv";
            string fwrite = "5402_datasetfix.csv";
            Lines = ReadFile(fread);
            Lines = ProprocessLines(Lines);

            table = CreateTable(Lines);
            table = FixNominals(table);

            Lines = ConvertBackIntoLines(table);
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
                //Console.WriteLine(OutputLine(inputLine));
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

        private static List<Coloumn> CreateTable(List<List<string>> linestoProcess)
        {

            List<Coloumn> table = new List<Coloumn>();

            //create columns
            foreach (string title in linestoProcess[0])
            {
                table.Add(new Coloumn(title));
            }
            for (int i = 1; i < linestoProcess.Count; i++)
            {
                int index = 0;
                foreach (var item in linestoProcess[i])
                {
                    table[index].AddValue(item);
                    index++;
                }
            }
            return table;

        }

        private static List<Coloumn> FixNominals(List<Coloumn> tabletoProcess)
        {

            for (int i = 0; i < tabletoProcess.Count; i++)
            {
                if (tabletoProcess[i].type == "nominal")
                {
                    //figure out majority number// already done with nominal automitically
                    //process

                    Coloumn coloumn = tabletoProcess[i];
                    /*
                    Console.WriteLine(coloumn.title+": "+ coloumn.MissingValueIndexes.Count);
                    
                    foreach (var item in coloumn.ValueEntries)
                    {
                        Console.WriteLine(tabletoProcess[i].type);
                        Console.WriteLine(item);
                        
                    }*/
                   // Console.WriteLine("BEFORE");
                    //Console.WriteLine(coloumn.MissingValueIndexes.Count + ":" + coloumn.majorityValue.value);

                    coloumn.UpdateMajorityValue();
                    //Console.WriteLine("AFTER");
                    //Console.WriteLine(coloumn.MissingValueIndexes.Count + ":" + coloumn.majorityValue.value);

                    for (int j = 0; j < coloumn.MissingValueIndexes.Count; j++)
                    {

                        coloumn.instances[coloumn.MissingValueIndexes[j]] = coloumn.majorityValue.value;
                    }
                    tabletoProcess[i] = coloumn;
                }
            }
            return tabletoProcess;

        }

        private static List<List<string>> ConvertBackIntoLines(List<Coloumn> coloumnstoConvert)
        {

            List<List<string>> linestoReturn = new List<List<string>>();
            List<string> linetoAdd = new List<string>();

            //create columns
            foreach (Coloumn column in coloumnstoConvert)
            {
                linetoAdd.Add(column.title);
            }
            linestoReturn.Add(linetoAdd);
            int numRows = coloumnstoConvert[0].instances.Count;
            for (int i = 1; i < numRows; i++)
            {
                linetoAdd = new List<string>();
                foreach (Coloumn column in coloumnstoConvert)
                {
                    linetoAdd.Add(column.instances[i]);
                }
                linestoReturn.Add(linetoAdd);
            }

            return linestoReturn;

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
                    if (stringtomodify == FixList[j].error)
                    {
                        linetoModify[i] = FixList[j].fix;
                        break;
                    }
                }
            }
            return linetoModify;

        }
    }
}
