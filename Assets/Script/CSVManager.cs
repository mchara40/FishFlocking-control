using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditor;

//This Class control all the funtions for the CSV file(report.csv)
public class CSVManager : MonoBehaviour
{
    private static string reportDirectoryName = "Report";
    private static string reportFileName = "report.csv";
    private static string recordreportFileName = "record_report.csv";
    private static string reportSeparator = ",";
    private static string[] reportHeaders = new string[6] {
        "Flock ID",
        "Fish ID",
        "X",
        "Y",
        "Z",
        "Time(sec)"
    };
    private static string[] reportDrawHeaders = new string[3] {
       
        "X",
        "Y",
        "Z",
       
    };



    //String to Vector3
    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));
        return result;
    }

    #region Interactions
    //load x,y,z from csv to draw path
    public LineRenderer[] LoadFromReport(LineRenderer FL)
    {
        
        VerifyDirectory();
        VerifyFile();
        int j = 0;
        int p = 0;
        bool allow = true;
        int k = 0;
        int l = 0;
        int w = 0;
        int line = 0;
        int stop = 0;
        Vector3[] final = new Vector3[3];
       
        string path = EditorUtility.OpenFilePanel("Overwrite with csv", "", "csv");
        StreamReader sr = new StreamReader(path);
        bool endofFile = false;
        
        int c = 0;

        c = check(path);
        
        LineRenderer[] lis = new LineRenderer[c];

        for (int i = 0; i < c; i++)
        {
            lis[i] = Instantiate(FL, Vector3.zero, Quaternion.identity);
        }
        while (!endofFile)
        {

            string data_string = sr.ReadLine();
            line++;
            if (data_string == "0,0,0")
            {
                allow = true;
                while (allow)
                {
                    data_string = sr.ReadLine();
                    line++;
                    if (data_string == "0,0,0" || data_string == null)
                    {
                        k = 0;
                        stop++;
                        if (stop == c)
                        {
                            endofFile = false;
                            break;
                        }
                        sr = new StreamReader(path);
                        for (int i = 0; i < line - 1; i++)
                        {
                            data_string = sr.ReadLine();
                        }
                        line--;
                        allow = false;
                        break;
                    }
                    var data_values = data_string.Split(',');
                    for (int i = 0; i < 3; i++)
                    {
                        if (data_values[i] != "X" && data_values[i] != "Y" && data_values[i] != "Z")
                        {
                            final[i] = StringToVector3(data_string);
                            lis[l].SetPosition(k, final[i]);
                        }
                    }
                    lis[l].positionCount++;
                    k++;

                }
                lis[l].positionCount = lis[l].positionCount - 2;
                l++;
            }
            else
            {
                if (data_string == null)
                {
                    endofFile = true;
                    break;
                }
                var data_values = data_string.Split(',');
                for (int i = 0; i < 3; i++)
                {
                    if (data_values[i] != "X" && data_values[i] != "Y" && data_values[i] != "Z")
                    {
                        final[i] = StringToVector3(data_string);
                        FL.SetPosition(j, final[i]);
                    }
                }
                FL.positionCount++;
                j++;

                for (int i = 0; i < 3; i++)
                {
                    if (data_values[i] == "X" || data_values[i] == "Y" || data_values[i] == "Z")
                    {
                        FL.positionCount = 2;
                        j = 0;

                    }
                }
            }
        }
        FL.positionCount = FL.positionCount - 2;
        return lis;
    }

    //retyrn the number of line 
    public static int check(string path)
    {

        StreamReader sr = new StreamReader(path);
        bool endofFile = false;
        int c = 0;
        while (!endofFile)
        {
            string data_string = sr.ReadLine();
            if (data_string == null)
            {

                endofFile = true;
                break;
            }
            if (data_string == "0,0,0")
            {
                c++;
            }
        }
        return c;
    }

    //write x,y,z of all points in csv file
    public static void AppendToReport(string[] strings)
    {
        VerifyDirectory();
        VerifyFile();

        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < strings.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += strings[i];
            }

            sw.WriteLine(finalString);
        }
    }

    //create csv file with title(Headers)
    public static void CreateReport(string path)
    {
        VerifyDirectory();
        using (StreamWriter sw = File.CreateText(path))
        {
            string finalString = "";
            for (int i = 0; i < reportHeaders.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += reportHeaders[i];
            }

            sw.WriteLine(finalString);
        }
    }

    ////create csv file with title(Headers)
    public static void CreateDrawReport(string path)
    {
        VerifyDirectory();
        using (StreamWriter sw = File.CreateText(path))
        {
            string finalString = "";
            for (int i = 0; i < reportDrawHeaders.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += reportDrawHeaders[i];
            }

            sw.WriteLine(finalString);
        }
    }


    #endregion

    #region Operations
    //check if exist directory.if !exist then create it
    static void VerifyDirectory()
    {
        string dir = GetDirectoryPath();
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    //check if exist file.if !exist then create it
    static void VerifyFile()
    {
        string file = GetFilePath();
        if (!File.Exists(file))
        {
            CreateDrawReport(file);
        }
    }


    #endregion

    #region Queries
    //get path of directory 
    static string GetDirectoryPath()
    {
        return Application.dataPath + "/" + reportDirectoryName;
    }

    //get path of file
    static string GetFilePath()
    {
        return GetDirectoryPath() + "/" + reportFileName;
    }

    //get path of file
    public static string GetFilePath2()
    {
        return GetDirectoryPath() + "/" + recordreportFileName;
    }

    //get time
    static string GetTimeStamp()
    {
        return System.DateTime.Now.ToString();
    }
    #endregion
}
