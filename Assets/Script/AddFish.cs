

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AddFish : MonoBehaviour
{
    public GameObject MyFlock;
    public GameObject MyFlock2;
    public GameObject MyFlock3;
    public GameObject MyFlock4;
    
    //Add 50 fish in every flock 
    public void Generate()
    {
        
        MyFlock.GetComponent<Flock>().AddnewUnits(50);
        MyFlock2.GetComponent<Flock>().AddnewUnits(50);
        MyFlock3.GetComponent<Flock>().AddnewUnits(50);
        MyFlock4.GetComponent<Flock>().AddnewUnits(50);
       

    }
}
