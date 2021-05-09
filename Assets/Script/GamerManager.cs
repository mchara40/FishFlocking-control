using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;


public class GamerManager : MonoBehaviour
{
    GamerManager instance;
    public LineRenderer list;
    // public List<LineRenderer> lis = new List<LineRenderer>();
    //public static List<Vector3> all;
    public static LineRenderer[] all;
    List<Vector3> FL = new List<Vector3>();
    
    public Camera cam = null;
    static List<int> temp = new List<int>();
    int temp2 = 0;

    bool allow = false;
   

    public Button button;
    public Button button2;
   
    public GameObject GO1;
    public GameObject GO2;
    public GameObject GO3;
    public GameObject GO4;
    public GameObject valuetext1;
    public GameObject valuetext2;
    public GameObject valuetext3;
    public GameObject valuetext4;
  


    void Awake()
    {
        instance = this;

    }
    
    void Update()
    {
       
        float screenPosX = cam.transform.position.x;
        float screenPosZ = cam.transform.position.z;
        
        if (screenPosX < -85 && screenPosZ<45)
        {
            GO1.active = false;
            GO2.active = true;
            GO3.active = false;
            GO4.active = false;
            valuetext1.active = false;
            valuetext2.active = true;
            valuetext3.active = false;
            valuetext4.active = false;
        }
        else if(screenPosX>-85 && screenPosZ < 45)
        {

            GO1.active = true;
            GO2.active = false;
            GO3.active = false;
            GO4.active = false;
            valuetext1.active = true;
            valuetext2.active = false;
            valuetext3.active = false;
            valuetext4.active = false;

        }else if (screenPosX > -150 && screenPosZ > 45)
        {
            GO1.active = false;
            GO2.active = false;
            GO3.active = true;
            GO4.active = false;
            valuetext1.active = false;
            valuetext2.active = false;
            valuetext3.active = true;
            valuetext4.active = false;

        }
        else if(screenPosX<-150 && screenPosZ > 45)
        {
            GO1.active = false;
            GO2.active = false;
            GO3.active = false;
            GO4.active = true;
            valuetext1.active = false;
            valuetext2.active = false;
            valuetext3.active = false;
            valuetext4.active = true;
        }
    
        if (Input.GetMouseButton(2))
        {
            cam.transform.position += cam.transform.right * Input.GetAxis("Mouse X") * 2.5f;
            cam.transform.position += cam.transform.up * Input.GetAxis("Mouse Y") * 2.5f;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            cam.orthographicSize += 1;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            cam.orthographicSize -= 1;
        }

        if (list.positionCount != temp2 && list.positionCount > 2)
        {
            temp2 = list.positionCount;
            Vector3[] newPos = new Vector3[list.positionCount];
            list.GetPositions(newPos);
            FL.AddRange(newPos);
            FL = FL.Distinct().ToList();
            temp.Add(temp2);

        }
        button.onClick.AddListener(allowbutton);
       

        if (allow)
        {

            Generate(FL);
            allow = false;
        }
        

    }

    public static void Generate(List<Vector3> FL)
    {
        //CSVManager csv = gameObject.GetComponent<CSVManager>();
        int c = 0;

        temp.RemoveAt(0);
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i] == 3)
            {
                c = c + 3;
                FL.Insert(i + c, Vector3.zero);
            }
        }

        for (int i = 0; i < FL.Count; i++)
        {

            CSVManager.AppendToReport(GetReportLine(FL, i));
            //csv.AppendToReport(GetReportLine(FL, i));
        }
        Debug.Log("<color=magenta>Report updated succesfully!!</color>");

    }

    public void LoadData()
    {

        CSVManager csv = gameObject.GetComponent<CSVManager>();

        all = csv.LoadFromReport(list);


    }
   
    static string[] GetReportLine(List<Vector3> newPos, int temp)
    {
        string[] returnable = new string[3];

        returnable[0] = newPos[temp].x.ToString();
        returnable[1] = newPos[temp].y.ToString();
        returnable[2] = newPos[temp].z.ToString();


        return returnable;
    }

    public void allowbutton()
    {
        allow = true;
    }
   


}
