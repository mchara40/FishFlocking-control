using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(MeshCollider))]
public class AddObstacle : MonoBehaviour
{

    public GameObject obstacle;
    private Vector3 screenPoint;
    private Vector3 offset;
    public Button button;
    bool allow = true;

    //When user press the AddObstacle button put on the field the obstacle and enable all other button
    public void Update()
    {

        AddPath p = gameObject.GetComponent<AddPath>();
        
        SpawnFood f = gameObject.GetComponent<SpawnFood>();
        button.onClick.AddListener(Cenable);
        if (allow)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Instantiate(obstacle, new Vector3(cursorPos.x, 0, cursorPos.z), Quaternion.identity);
            }
        }
        button.onClick.AddListener(p.Allowcl);
        button.onClick.AddListener(f.AllowClick);

    }


    public void AllowClick()
    {
        allow = false;
    }
    public void Cenable()
    {
        allow = true;
    }
}
