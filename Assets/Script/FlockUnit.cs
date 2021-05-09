using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DataStructures.ViliWonka.KDTree;
using DataStructures.ViliWonka.Heap;
using DataStructures.ViliWonka.Tests;
using System.Diagnostics;

public class FlockUnit : MonoBehaviour
{
    [SerializeField] private float FOVAngle;
    [SerializeField] private float smoothDamp;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask PathMask;
    [SerializeField] private LayerMask FoodMask;
    [SerializeField] private Vector3[] directionsToCheckWhenAvoidingObstacles;


    private List<int> cohesionNeighbours = new List<int>();
    private List<int> avoidanceNeighbours = new List<int>();
    private List<int> aligementNeighbours = new List<int>();
    //private KdTree<FlockUnit> cohesionNeighbours = new KdTree<FlockUnit>();
    //private KdTree<FlockUnit> avoidanceNeighbours = new KdTree<FlockUnit>();
    //private KdTree<FlockUnit> aligementNeighbours = new KdTree<FlockUnit>();
    //private KdTree<FlockUnit> Neighbours = new KdTree<FlockUnit>();




    GameObject object1;
    List<GameObject> Foods = new List<GameObject>();
    List<Vector3> FL = new List<Vector3>();

    private Flock assignedFlock;

    //LineRenderer all = new LineRenderer();

    private AddFish fs;


    private Transform _t;
    private Material _m;
    private Vector3 currentVelocity;
    private Vector3 currentObstacleAvoidanceVector;
    private Vector3 currentFoodVector;
    private float speed;
    private List<GameObject> objects1;

    public Transform myTransform { get; set; }
    public int Health { get; private set; }
    int counterframe1 = 0;
    int counterframe2 = 0;
    int counterframe3 = 0;
    bool firstframe1 = true;
    bool firstframe2 = true;
    bool firstframe3 = true;
    Vector3 PathVector;

    public static Stopwatch m_stopwatch = new Stopwatch();
    private void Awake()
    {
        myTransform = transform;

        _m = GetComponent<Renderer>().material;
        counterframe1 = UnityEngine.Random.Range(0, 100);
        counterframe2 = UnityEngine.Random.Range(0, 100);
        counterframe3 = UnityEngine.Random.Range(0, 100);
       
    }
    
    public void AssignFlock(Flock flock)
    {
        assignedFlock = flock;
    }





    public void InitializeSpeed(float speed)
    {
        this.speed = speed;
    }

    void Update()
    {

        counterframe1++;
        counterframe2++;
        counterframe3++;
        
        if (GamerManager.all != null)
        {
            
            for (int i = 0; i < GamerManager.all.Length; i++)
            {
                CalculatePathVector2(GamerManager.all[i]);
            }
          
        }
        Quaternion pos = myTransform.rotation;
        _m.color = new Color(pos.w, pos.y, pos.x, pos.z);

    }


    public void MoveUnit(LineRenderer list, KDTree tree)
    {
        
        if (firstframe1 || counterframe1 % 50 == 0)
        {
           
            var startTime = DateTime.Now;
            cohesionNeighbours = FindNeighbours(tree, assignedFlock.cohesionDistance);
            
            firstframe1 = false;
            var elapsed = (DateTime.Now - startTime).Milliseconds;
           
           
            UnityEngine.Debug.Log("Time : " + elapsed);
        }
        if (firstframe2 || counterframe2 % 50 == 0)
        {
           
            aligementNeighbours = FindNeighbours(tree, assignedFlock.aligementDistance);
           
            firstframe2 = false;
        }
        if (firstframe3 || counterframe3 % 50 == 0)
        {
            
            avoidanceNeighbours = FindNeighbours(tree, assignedFlock.avoidanceDistance);
            firstframe3 = false;
        }
        
        // FindNeighbours();
        CalculateSpeed();
        var cohesionVector = CalculateCohesionVector(tree) * assignedFlock.cohesionWeight;
        var avoidanceVector = CalculateAvoidanceVector(tree) * assignedFlock.avoidanceWeight;
        var aligementVector = CalculateAligementVector(tree) * assignedFlock.aligementWeight;
        var boundsVector = CalculateBoundsVector() * assignedFlock.boundsWeight;
        var obstacleVector = CalculateObstacleVector() * assignedFlock.obstacleWeight;
        var FoodVector = CalculateFoodVector() * assignedFlock.FoodWeight;
        if (counterframe1 % 8 == 0)
        {
            PathVector = CalculatePathVector(list) * assignedFlock.PathWeight;
        }
        var moveVector = cohesionVector + avoidanceVector + aligementVector + boundsVector + obstacleVector + FoodVector + PathVector;
        moveVector = Vector3.SmoothDamp(myTransform.forward, moveVector, ref currentVelocity, smoothDamp);
        moveVector = moveVector.normalized * speed;
        if (moveVector == Vector3.zero)
            moveVector = transform.forward;

        myTransform.forward = moveVector;
        myTransform.position += moveVector * Time.deltaTime;
       



    }


    
    private List<int> FindNeighbours(KDTree tree, float radius)
    {
       
         var resultIndices = new List<int>();
       
         KDQuery query = new KDQuery();
         query.Radius(tree, transform.position, radius, resultIndices);
        //  Debug.Log(resultIndices.Count);
        if (resultIndices.Count > 40)
        {
            var resultIndices2 = new List<int>();
            resultIndices2 = resultIndices.GetRange(0,40);
            return resultIndices2;
        }
         return resultIndices;



    }

    //private void FindNeighbours()
    //{
    //    cohesionNeighbours.Clear();
    //    avoidanceNeighbours.Clear();
    //    aligementNeighbours.Clear();
    //    var allUnits = assignedFlock.allUnits;
    //    if (first)
    //    {
    //        foreach (FlockUnit fish in allUnits)
    //        {
    //            Neighbours.Add(fish);
    //        }
    //        first = false;
    //    }

    //    //cohesionNeighbours.UpdatePositions();
    //    //avoidanceNeighbours.UpdatePositions();
    //    //aligementNeighbours.UpdatePositions();

    //    //Neighbours.UpdatePositions();
    //    for (int i = 0; i < allUnits.Length; i++)
    //    {
    //        var currentUnit = allUnits[i];
    //        if (currentUnit != this)
    //        {
    //            float currentNeighbourDistanceSqr = Vector3.SqrMagnitude(currentUnit.myTransform.position - myTransform.position);
    //            if (currentNeighbourDistanceSqr <= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance)
    //            {
    //                // UnityEngine.Debug.DrawLine(myTransform.position, currentUnit.transform.position, UnityEngine.Color.black);
    //                cohesionNeighbours.Add(currentUnit);
    //            }
    //            if (currentNeighbourDistanceSqr <= assignedFlock.avoidanceDistance * assignedFlock.avoidanceDistance)
    //            {
    //                avoidanceNeighbours.Add(currentUnit);
    //            }
    //            if (currentNeighbourDistanceSqr <= assignedFlock.aligementDistance * assignedFlock.aligementDistance)
    //            {
    //                aligementNeighbours.Add(currentUnit);
    //            }
    //        }
    //    }

    //    //foreach (FlockUnit fish in allUnits)
    //    //{
    //    //    FlockUnit nearestObj = all.FindClosest(fish.transform.position);
    //    //    UnityEngine.Debug.DrawLine(fish.transform.position, nearestObj.transform.position, UnityEngine.Color.black);

    //    //}

    //}

    private void CalculateSpeed()
    {
        
        if (cohesionNeighbours.Count == 0)
            return;
        speed = 0;
        for (int i = 0; i < cohesionNeighbours.Count; i++)
        {
            speed += assignedFlock.allUnits[cohesionNeighbours[i]].speed;
           // speed += cohesionNeighbours[i].speed;
        }

        speed /= cohesionNeighbours.Count;
        speed = Mathf.Clamp(speed, assignedFlock.minSpeed, assignedFlock.maxSpeed);
    }

    private Vector3 CalculateCohesionVector(KDTree tree)
    {
        var cohesionVector = Vector3.zero;
        if (cohesionNeighbours.Count == 0)
            return Vector3.zero;
        int neighboursInFOV = 0;
        for (int i = 0; i < cohesionNeighbours.Count; i++)
        {
            if (IsInFOV(tree.Points[cohesionNeighbours[i]]))
            {
                neighboursInFOV++;
                cohesionVector += tree.Points[cohesionNeighbours[i]];
                //cohesionVector += cohesionNeighbours[i].myTransform.position;
            }
        }

        cohesionVector /= neighboursInFOV;
        cohesionVector -= myTransform.position;
        cohesionVector = cohesionVector.normalized;
        return cohesionVector;
    }

    private Vector3 CalculateAligementVector(KDTree tree)
    {
        var aligementVector = myTransform.forward;
        if (aligementNeighbours.Count == 0)
            return myTransform.forward;
        int neighboursInFOV = 0;
        for (int i = 0; i < aligementNeighbours.Count; i++)
        {
            if (IsInFOV(tree.Points[aligementNeighbours[i]]))
            {
                neighboursInFOV++;
                aligementVector += assignedFlock.allUnits[aligementNeighbours[i]].myTransform.forward;
                //aligementVector += aligementNeighbours[i].myTransform.forward;
            }
        }

        aligementVector /= neighboursInFOV;
        aligementVector = aligementVector.normalized;
        return aligementVector;
    }


    private Vector3 CalculateAvoidanceVector(KDTree tree)
    {

        var avoidanceVector = Vector3.zero;
        if (aligementNeighbours.Count == 0)
            return Vector3.zero;
        int neighboursInFOV = 0;
        for (int i = 0; i < avoidanceNeighbours.Count; i++)
        {
            if (IsInFOV(tree.Points[avoidanceNeighbours[i]]))
            {
                neighboursInFOV++;
                avoidanceVector += (myTransform.position - tree.Points[avoidanceNeighbours[i]]);
               // avoidanceVector += (myTransform.position - avoidanceNeighbours[i].myTransform.position);
            }
        }

        avoidanceVector /= neighboursInFOV;
        avoidanceVector = avoidanceVector.normalized;
        return avoidanceVector;
    }

    private Vector3 CalculateBoundsVector()
    {
        var offsetToCenter = assignedFlock.transform.position - myTransform.position;
        bool isNearCenter = (offsetToCenter.magnitude >= assignedFlock.boundsDistance * 0.9f);
        return isNearCenter ? offsetToCenter.normalized : Vector3.zero;
    }


    private Vector3 CalculateObstacleVector()
    {
        var obstacleVector = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(myTransform.position, myTransform.forward, out hit, assignedFlock.obstacleDistance, obstacleMask))
        {

            obstacleVector = FindBestDirectionToAvoidObstacle();

        }
        else
        {
            currentObstacleAvoidanceVector = Vector3.zero;
        }
        return obstacleVector;
    }


    private Vector3 CalculateFoodVector()
    {
        float rotationspeed = 5.0f;
        var pos = transform.position;
        Foods = GameObject.FindGameObjectsWithTag("Food").ToList();
        Vector3 direction = Vector3.zero;

        for (int i = 0; i < Foods.Count; i++)
        {
            float dist = Vector3.Distance(Foods[i].transform.position, pos);

            if (dist < assignedFlock.FoodDistance)
            {

                object1 = Foods[i];

            }
        }
        if (object1 != null)
        {
            direction = object1.transform.position - myTransform.position;
            if (direction != object1.transform.position)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationspeed * Time.deltaTime);
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
                transform.position = Vector3.MoveTowards(transform.position, object1.transform.position, 0.35f);
                if (transform.position == object1.transform.position)
                {

                    Attack();

                }
            }
        }
        return direction;
    }


    private void Attack()
    {
        Destroy(object1);
    }

    private Vector3 FindBestDirectionToAvoidObstacle()
    {
        if (currentObstacleAvoidanceVector != Vector3.zero)
        {
            RaycastHit hit;
            if (!Physics.Raycast(myTransform.position, myTransform.forward, out hit, assignedFlock.obstacleDistance, obstacleMask))
            {
                return currentObstacleAvoidanceVector;


            }
        }
        float maxDistance = int.MinValue;
        var selectedDirection = Vector3.zero;
        for (int i = 0; i < directionsToCheckWhenAvoidingObstacles.Length; i++)
        {

            RaycastHit hit;
            var currentDirection = myTransform.TransformDirection(directionsToCheckWhenAvoidingObstacles[i].normalized);

            if (Physics.Raycast(myTransform.position, currentDirection, out hit, assignedFlock.obstacleDistance, obstacleMask))
            {

                float currentDistance = (hit.point - myTransform.position).sqrMagnitude;
                if (currentDistance > maxDistance)
                {
                    maxDistance = currentDistance;
                    selectedDirection = currentDirection;

                }
            }
            else
            {
                selectedDirection = currentDirection;
                currentObstacleAvoidanceVector = currentDirection.normalized;
                return selectedDirection.normalized;

            }
        }
        return selectedDirection.normalized;

    }

    private bool IsInFOV(Vector3 position)
    {
        return Vector3.Angle(myTransform.forward, position - myTransform.position) <= FOVAngle;
    }

    public Vector3 CalculatePathVector(LineRenderer list)
    {

        Vector3 direction = Vector3.zero;

        Vector3[] newPos = new Vector3[list.positionCount];

        if (list.positionCount > 2)
        {
           
            list.GetPositions(newPos);
            
                FL.AddRange(newPos);
            
            FL = FL.Distinct().ToList();
           
        }
        
           
        
        var thesi = 0;

        float dis = int.MaxValue;
        for (int i = 0; i < FL.Count; i++)
        {
            if (Vector3.Distance(myTransform.position, FL[i]) + UnityEngine.Random.Range(0f, dis * 0.01f) < dis)
            {

                dis = Vector3.Distance(myTransform.position, FL[i]);

                thesi = i;
            }

        }
        if (dis <= assignedFlock.PathDistance)
        {
            if (list.positionCount > 2)
            {

                direction = FL[thesi] - myTransform.position;
                for (int i = 1; i <= 2; i++)
                {

                    if (thesi + i < FL.Count)
                    {

                        direction += FL[thesi + i] - FL[thesi];
                        thesi = thesi + i;
                    }

                }
            }
        }
        if (list.positionCount == 2)
        {
            if (list.GetPosition(1) == Vector3.zero && list.GetPosition(0) == Vector3.zero)
            {
                FL.Clear();
            }
        }

        return direction.normalized;

    }

    public void CalculatePathVector2(LineRenderer list)
    {
        if (list != null)
        {
            Vector3[] newPos = new Vector3[list.positionCount];
            if (list.GetPosition(1) != Vector3.zero && list.GetPosition(0) != Vector3.zero)
            {
                list.GetPositions(newPos);



                FL.AddRange(newPos);
                    
                
            }

        }
    }


   
}
