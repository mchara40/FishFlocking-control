
using UnityEditor;
using UnityEngine;

public class tools : MonoBehaviour
{

    static void DEV_AppendToReport()
    {
       // CSVManager csv = gameObject.GetComponent<CSVManager>();
       CSVManager.AppendToReport(
            new string[3]
            {
                "2",
                "2",
                "2"
            }
         );
        // EditorApplication.Beep();
        Debug.Log("<color=green>Report updated succesfully!!</color>");
    }


    public  void ResetToReport()
    {
        string path = EditorUtility.OpenFilePanel("Overwrite with csv", "", "csv");
        //CSVManager csv = gameObject.GetComponent<CSVManager>();
        CSVManager.CreateReport(path);
        // EditorApplication.Beep();
        Debug.Log("<color=orange>The Report has been reset...</color>");
    }


    public void ResetToDrawReport()
    {
        string path = EditorUtility.OpenFilePanel("Overwrite with csv", "", "csv");
        //CSVManager csv = gameObject.GetComponent<CSVManager>();
        CSVManager.CreateDrawReport(path);
        // EditorApplication.Beep();
        Debug.Log("<color=orange>The Report has been reset...</color>");
    }


}
