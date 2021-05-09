using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RemoveAll : MonoBehaviour
{
    public Button button;
    bool allow = false;
    public LineRenderer List;
    private GameObject[] path;
    

    // Update is called once per frame
    void Update()
    {
        
        button.onClick.AddListener(REnable);
        if (allow)
        {
            GameObject[] Obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            GameObject[] Food = GameObject.FindGameObjectsWithTag("Food");
            
            // AddPath p = gameObject.GetComponent<AddPath>();
            for (int i = 0; i < Obstacles.Length; i++)
            {
                Destroy(Obstacles[i]);


            }
            for (int i = 0; i < Food.Length; i++)
            {
                Destroy(Food[i]);


            }
            path = GameObject.FindGameObjectsWithTag("Path");

            for (int i = 0; i < path.Length; i++)
            {
                path[i].GetComponent<LineRenderer>().positionCount = 2;
                path[i].GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                path[i].GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));

            }
            for (int i = 1; i < path.Length; i++)
            {
                Destroy(path[i]);
            }


            allow = false;
        }
    }
    public void REnable()
    {
        allow = true;
    }
    public void DeleteAll()
    {
      
    }
}
