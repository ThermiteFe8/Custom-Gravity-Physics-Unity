using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravityAffected : MonoBehaviour
{
    [TextArea]
    public string notes = "Attach this to stuff that you want to be affected by gravity. Right now, it accelerates the object in the direction of the gravitational vector, but you can edit this to suit your needs.";
    public Vector3 fallingDirection = Vector3.down;
    Rigidbody rb;
    List<GravPackageStruct> newGravs;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        newGravs = new List<GravPackageStruct>();

    }
    private void Update()
    {
        if(newGravs.Count > 0)
        {
            //I could probably pick a better number - this just guarantees that *any* gravity field can override it and control the object
            int priorityTracker = -100;
            Vector3 newerGrav = Vector3.zero;
            for(int i = 0; i < newGravs.Count; i++)
            {
                if (newGravs[i].gravPriority > priorityTracker)
                {
                    //If you find a gravitational field with greater priority, use it instead
                    newerGrav = newGravs[i].gravVect;
                    priorityTracker = newGravs[i].gravPriority;
                }
            }
            fallingDirection = newerGrav;
        }
        newGravs.Clear();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Accelerate the object in the direction of gravity every physics update
        //You can replace this with whatever you want - as long as it references fallingDirection in some capacity, it should look fine
        rb.AddForce(fallingDirection, ForceMode.Acceleration);
        //transform.up = fallingDirection.normalized * -1;
    }

    public void AddGravStruct(GravPackageStruct gravStruct)
    {
        //Add the gravitational vector to the list
        //This ends up updating ~every frame because stuff like sphere fields and spline fields change their vectors based on position
        newGravs.Add(gravStruct);
    }
}
