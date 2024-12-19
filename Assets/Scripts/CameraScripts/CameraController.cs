using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineBrain mainCam;
    [SerializeField] SimplePlayerController playerController;
    List<CamPackageStruct> newCamSettings;

    [HideInInspector]
    public CinemachineCamera currentCam;
    void Start()
    {
        newCamSettings = new List<CamPackageStruct>();
    }

    void LateUpdate()
    {
        if(newCamSettings.Count > 0)
        {
            //There's probably a better number to use - essentially, just smthn low so that *any* existing camera can override it
            int priorityTracker = -100; 
            CamPackageStruct newerCam = null;
            //Iterate through all potential cameras and pick the one with the highest priority
            for(int i = 0; i < newCamSettings.Count; i++)
            {
                if (newCamSettings[i].camPriority > priorityTracker)
                {
                    newerCam = newCamSettings[i];
                    priorityTracker = newCamSettings[i].camPriority;
                }
            }
            UpdateCam(newerCam);
            newCamSettings.Clear();
        }
    }

    //Should only run when you enter the camera's respective trigger box - if you wanna do fancy stuff, call it anytime you want
    public void AddCam(CamPackageStruct inputInfo)
    {
        newCamSettings.Add(inputInfo);
    }

    public void UpdateCam(CamPackageStruct inputInfo)
    {
        if(inputInfo.thisCam != currentCam) //Make sure we aren't repeating tons of work if the camera hasn't changed
        {
            currentCam = inputInfo.thisCam;
            currentCam.Prioritize();
        }

        //Send appropriate data to the player - in this case, it's for control overrides
        playerController.SetCamControls(inputInfo.forwardVect, inputInfo.leftVect, inputInfo.flipUp, inputInfo.flipLeft);
    }
}
