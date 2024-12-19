using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ForceTransparentRay : MonoBehaviour
{
    [TextArea]
    public string notes = "Put this on the camera - gravity can lead to walls unexpectedly blocking the view of the camera. To prevent jarring movement, you can just turn it transparent. Note that the objects need the MakeTransparent script attached if you want them to turn transparent.";

    [SerializeField] SimplePlayerController player;
    [SerializeField] LayerMask layersToAvoid;
    
    //List of objects that the raycast hits
    List<MakeTransparent> currentlyTrans = new List<MakeTransparent>();
    List<MakeTransparent> previouslyTrans = new List<MakeTransparent>();


    // Update is called once per frame
    void Update()
    {
        //Save the stuff from the previous frame onto the previouslyTrans list and clear the current list
        previouslyTrans.Clear();
        for (int i = 0; i < currentlyTrans.Count; i++)
        {
            previouslyTrans.Add(currentlyTrans[i]);
        }
        currentlyTrans.Clear();
        
        //Cast a ray that goes between the camera and player and save the stuff it hits
        RaycastHit[] stuffHit = Physics.RaycastAll(transform.position, player.camToPlayer * -1, player.camToPlayer.magnitude);
        if(stuffHit != null)
        {
            for(int i = 0; i < stuffHit.Length; i++)
            {
                MakeTransparent potentialTrans = stuffHit[i].collider.gameObject.GetComponent<MakeTransparent>();
                if(potentialTrans != null)
                {
                    currentlyTrans.Add(potentialTrans);
                }
            }
        }

        forceTrans();
        unTrans();
    }

    public void forceTrans()
    {
        //Make anything hit by the ray transparent
        if(currentlyTrans.Count > 0)
        {
            for(int i = 0; i < currentlyTrans.Count; i++)
            {
                currentlyTrans[i].makeTransparent();
            }
        }
    }

    public void unTrans()
    {
        //If something was part of the previous frame, but not this one, turn it opaque again
        if(previouslyTrans.Count > 0)
        {
            for (int i = 0; i < previouslyTrans.Count; i++)
            {
                if (currentlyTrans.Contains(previouslyTrans[i]) == false)
                {
                    previouslyTrans[i].makeOpaque();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, player.camToPlayer * -1);
    }
}
