using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFish : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject MyFlock;
    public GameObject MyFlock2;
    public GameObject MyFlock3;
    public GameObject MyFlock4;


    public void Generate()
    {
        MyFlock.GetComponent<Flock>().DestroyFish(50);
        MyFlock2.GetComponent<Flock>().DestroyFish(50);
        MyFlock3.GetComponent<Flock>().DestroyFish(50);
        MyFlock4.GetComponent<Flock>().DestroyFish(50);


    }

}

