using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPath : MonoBehaviour
{
    public Camera cam = null;
    public LineRenderer lineRenderer = null;
    public Button button;
    private Vector3 mousePos;
    private Vector3 Pos;
    private Vector3 previousPos;
    public List<Vector3> linePositions = new List<Vector3>();

    public float minimumDistance = 0.05f;
    private float distance = 0;
    bool allowclick = true;
   
    public Button Follow;
   
    
    public LineRenderer linePrefab;
    public LineRenderer currentLine;

    //When user press the AddObstacle button put on the field the obstacle and enable all other button
    void Update()
    {
        AddObstacle A = gameObject.GetComponent<AddObstacle>();
        SpawnFood f = gameObject.GetComponent<SpawnFood>();
        button.onClick.AddListener(Cenable);
        if (allowclick)
        {

            if (Input.GetMouseButtonDown(0))
            {
                
                currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
                linePositions.Clear();
              
                
                mousePos = Input.mousePosition;
                mousePos.z = 10;
                //  mousePos.y = 0;
                Pos = cam.ScreenToWorldPoint(mousePos);
                previousPos = Pos;
                linePositions.Add(Pos);

            }
            else
            if (Input.GetMouseButton(0))
            {
                mousePos = Input.mousePosition;
                mousePos.z = 10;
                // mousePos.y = 0;
                Pos = cam.ScreenToWorldPoint(mousePos);
                distance = Vector3.Distance(Pos, previousPos);
                if (distance >= minimumDistance)
                {
                    previousPos = Pos;
                    linePositions.Add(Pos);
                    lineRenderer.positionCount = linePositions.Count;
                    lineRenderer.SetPositions(linePositions.ToArray());
                    
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                
                //CSVManager.Takelist(Pos);
            }
            
        }

        button.onClick.AddListener(A.AllowClick);
        button.onClick.AddListener(f.AllowClick);

    }

    public void Cenable()
    {
        allowclick = true;
    }
    public void Allowcl()
    {
        allowclick = false;
    }
   
}
