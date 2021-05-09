using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpawnFood : MonoBehaviour
{

    public GameObject food;
    public Button button;
    bool allowclick = true;
   

    public void Update()
    {
        AddPath p = gameObject.GetComponent<AddPath>();
        AddObstacle A = gameObject.GetComponent<AddObstacle>();
        button.onClick.AddListener(Cenable);
        if (allowclick)
        {

            if (Input.GetMouseButtonDown(0))
            {
               
                for (int i = 0; i < 10; i++)
                {
                    Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Instantiate(food, new Vector3(cursorPos.x+UnityEngine.Random.Range(-10,10), 0, cursorPos.z+ UnityEngine.Random.Range(-10, 10)), Quaternion.identity);
                    
                }

            }
        }

        button.onClick.AddListener(p.Allowcl);
        button.onClick.AddListener(A.AllowClick);



        //Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 wordPos;
        //    Ray ray = Camera.main.ScreenPointToRay(mousePos);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, 1000f))
        //    {
        //        wordPos = hit.point;
        //    }
        //    else
        //    {
        //        wordPos = Camera.main.ScreenToWorldPoint(mousePos);
        //    }
        //    Instantiate(food, wordPos, Quaternion.identity);
        //    //or for tandom rotarion use Quaternion.LookRotation(Random.insideUnitSphere)
    }
    public void AllowClick()
    {
        allowclick = false;
    }
    public void Cenable()
    {
        allowclick = true;
    }
}

