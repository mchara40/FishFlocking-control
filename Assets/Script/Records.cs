using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Records : MonoBehaviour
{
    public List<Vector3> Positions = new List<Vector3>();
    Vector3 Pos;
    public bool record = false;
    bool wantsave = false;
    float seconds = 0.1f;
    
    int k = 0;
   

    // Update is called once per frame
    void Update()
    {
        Pos = gameObject.transform.position;

        if (record == true && wantsave == false)
        {
            StartCoroutine(Record());
            record = false;
        }
        if (record == false && wantsave == true)
        {
            StopCoroutine(Record());
            wantsave = false;
           
           
        }

    }
    IEnumerator Record()
    {
        Debug.Log("<color=green>Start Record!!</color>");
        while (wantsave == false)
        {
            
            Positions.Add(Pos);
            
           yield return new WaitForSeconds(seconds);
            k++;
           
        }
    }
    public void allowRecord()
    {
        record = true;
    }
    public void dontallow()
    {

        record = false;

    }
    public void allowSave()
    {
        wantsave = true;
    }
    public void DontSave()
    {
        wantsave = false;
    }
}
