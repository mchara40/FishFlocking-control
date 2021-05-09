using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System.Text;
using System.IO;
using DataStructures.ViliWonka.KDTree;

public class Flock : MonoBehaviour
{
    [Header("Spawn Setup")]
    [SerializeField] private FlockUnit flockUnitPrefab;
    [SerializeField] public int flockSize;
    [SerializeField] public Vector3 spawnBounds;

    [Header("Speed Setup")]
    [Range(0, 10)]
    [SerializeField] private float _minSpeed;
    public float minSpeed { get { return _minSpeed; } }
    [Range(0, 10)]
    [SerializeField] private float _maxSpeed;
    public float maxSpeed { get { return _maxSpeed; } }

    [Header("Detection Distances")]
    [Range(0, 10)]
    [SerializeField] public float _cohesionDistance;
    public float cohesionDistance { get { return _cohesionDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _avoidanceDistance;
    public float avoidanceDistance { get { return _avoidanceDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _aligementDistance;
    public float aligementDistance { get { return _aligementDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _obstacleDistance;
    public float obstacleDistance { get { return _obstacleDistance; } }

    [Range(0, 100)]
    [SerializeField] private float _boundsDistance;
    public float boundsDistance { get { return _boundsDistance; } }

    [Range(0, 20)]
    [SerializeField] private float _FoodDistance;
    public float FoodDistance { get { return _FoodDistance; } }


    [Range(0, 100)]
    [SerializeField] private float _PathDistance;
    public float PathDistance { get { return _PathDistance; } }



    [Header("Behaviour Weights")]
    [Range(0, 10)]
    [SerializeField] private float _cohesionWeight;
    public float cohesionWeight { get { return _cohesionWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _avoidanceWeight;
    public float avoidanceWeight { get { return _avoidanceWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _aligementWeight;
    public float aligementWeight { get { return _aligementWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _boundsWeight;
    public float boundsWeight { get { return _boundsWeight; } }

    [Range(0, 100)]
    [SerializeField] private float _obstacleWeight;
    public float obstacleWeight { get { return _obstacleWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _FoodWeight;
    public float FoodWeight { get { return _FoodWeight; } }

    [Range(0, 600)]
    [SerializeField] private float _PathWeight;
    public float PathWeight { get { return _PathWeight; } }

    public FlockUnit[] allUnits { get; set; }


    public LineRenderer list;
    public Slider Coh_Dis_mSlider;
    public Slider Avo_Dis_mSlider;
    public Slider Ali_Dis_mSlider;
    public Slider Obs_Dis_mSlider;
    public Slider Bound_Dis_mSlider;
    public Slider Food_Dis_mSlider;
    public Slider Path_Dis_mSlider;
    public Slider Coh_Weig_mSlider;
    public Slider Avo_Weig_mSlider;
    public Slider Ali_Weig_mSlider;
    public Slider Obs_Weig_mSlider;
    public Slider Bound_Weig_mSlider;
    public Slider Food_Weig_mSlider;
    public Slider Path_Weig_mSlider;


    bool stoprecord = false;
    public Vector3[] positionsfish;
    KDTree tree;
    int counterframe = 0;

    //start move fish and create kd-tree
    private void Start()
    {
        CSVManager.CreateReport(CSVManager.GetFilePath2());
        GenerateUnits();
        positionsfish = new Vector3[allUnits.Length];

        for (int i = 0; i < allUnits.Length; i++)
        {
            positionsfish[i] = allUnits[i].transform.position;
        }
        int maxPointsPerLeafNode = 32;
        tree = new KDTree(positionsfish, maxPointsPerLeafNode);


    }

    //update the positions of fish and rebuild tree
    void UpdateTree()
    {



        for (int i = 0; i < allUnits.Length; i++)
        {
            tree.Points[i] = allUnits[i].transform.position;
        }
        tree.Rebuild();

        // yield return new WaitForSeconds(1f);



    }

    //start record positions
    public void Setrecordtrue()
    {
        foreach (FlockUnit all in allUnits)
        {
            all.GetComponent<Records>().record = true;
        }
    }

    void Update()
    {

        for (int i = 0; i < allUnits.Length; i++)
        {

            if (allUnits[i].GetComponent<MeshRenderer>() != null)
            {
                allUnits[i].MoveUnit(list, tree);


            }

        }
        if (counterframe % 50 == 0)
        {
            UpdateTree();
        }
        counterframe++;

        if (stoprecord == true)
        {
           
            int k = 0;

            float time = 0;
            Debug.Log("<color=red>End Record.Save positions to csv file!!</color>");
            var csv = new StringBuilder();

            foreach (FlockUnit o in allUnits)
            {

                string flname = gameObject.name;
                char flid = flname.Last();
                foreach (Vector3 t in o.GetComponent<Records>().Positions)
                {


                    var newLine = flid + ", " + k + ", " + t.x.ToString() + ", " + t.y.ToString() + ", " + t.z.ToString() + ", " + time.ToString();
                    csv.AppendLine(newLine);
                    time += 0.1f;
                }
                k++;
                time = 0;
            }


            File.AppendAllText(CSVManager.GetFilePath2(), csv.ToString());
            stoprecord = false;


        }

        _cohesionDistance = Coh_Dis_mSlider.value;
        _avoidanceDistance = Avo_Dis_mSlider.value;
        _aligementDistance = Ali_Dis_mSlider.value;
        _obstacleDistance = Obs_Dis_mSlider.value;
        _boundsDistance = Bound_Dis_mSlider.value;
        _FoodDistance = Food_Dis_mSlider.value;
        _PathDistance = Path_Dis_mSlider.value;
        _cohesionWeight = Coh_Weig_mSlider.value;
        _avoidanceWeight = Avo_Weig_mSlider.value;
        _aligementWeight = Ali_Weig_mSlider.value;
        _boundsWeight = Bound_Weig_mSlider.value;
        _obstacleWeight = Obs_Weig_mSlider.value;
        _FoodWeight = Food_Weig_mSlider.value;
        _PathWeight = Path_Weig_mSlider.value;


    }

    private void GenerateUnits()
    {
        allUnits = new FlockUnit[flockSize];
        for (int i = 0; i < flockSize; i++)
        {
            var randomVector = UnityEngine.Random.insideUnitSphere;
            randomVector = new Vector3(randomVector.x * spawnBounds.x, randomVector.y * spawnBounds.y, randomVector.z * spawnBounds.z);
            var spawnPosition = transform.position + randomVector;
            var rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            allUnits[i] = Instantiate(flockUnitPrefab, spawnPosition, rotation);
            allUnits[i].AssignFlock(this);
            allUnits[i].InitializeSpeed(UnityEngine.Random.Range(minSpeed, maxSpeed));

        }

    }

    public void AddnewUnits(int howmany)
    {

        FlockUnit[] allUnits2 = new FlockUnit[allUnits.Length + howmany];




        for (int i = allUnits.Length; i < allUnits2.Length; i++)
        {
            var randomVector = UnityEngine.Random.insideUnitSphere;
            randomVector = new Vector3(randomVector.x * spawnBounds.x, randomVector.y * spawnBounds.y, randomVector.z * spawnBounds.z);
            var spawnPosition = transform.position + randomVector;
            var rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            allUnits2[i] = Instantiate(flockUnitPrefab, spawnPosition, rotation);
            allUnits2[i].AssignFlock(this);
            allUnits2[i].InitializeSpeed(UnityEngine.Random.Range(minSpeed, maxSpeed));
        }

        for (int i = 0; i < allUnits.Length; i++)
            allUnits2[i] = allUnits[i];
        allUnits = allUnits2;
    }

    public void DestroyFish(int howmany)
    {

        for (int i = 0; i < howmany; i++)
        {
            var n = UnityEngine.Random.Range(0, allUnits.Length);
            while (allUnits[n].GetComponent<MeshRenderer>() == null)
            {
                n = UnityEngine.Random.Range(0, allUnits.Length);
            }
            Destroy(allUnits[n].GetComponent<MeshRenderer>());
        }
    }
    public void stop()
    {
        stoprecord = true;
    }


}

