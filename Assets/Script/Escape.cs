using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{

    //When user press the escape keyboard button all the UI button=enable
    void Update()
    {
        AddObstacle A = gameObject.GetComponent<AddObstacle>();
        SpawnFood f = gameObject.GetComponent<SpawnFood>();
        AddPath p = gameObject.GetComponent<AddPath>();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            p.Allowcl();
            A.AllowClick();
            f.AllowClick();
        }
    }
}
