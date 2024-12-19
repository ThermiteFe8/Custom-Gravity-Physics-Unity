using System.Dynamic;
using Unity.Cinemachine;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

//Internal structure used for sending camera data from the trigger to the player or camera controller
public class CamPackageStruct : Object
{
    
    public CinemachineCamera thisCam;
    public int camPriority;
    public Vector3 forwardVect; //Overrides player's forward movement
    public Vector3 leftVect; //Overrides player's left movement
    public bool flipUp; //Flips player's forward/backward movement
    public bool flipLeft; //Flips player's left/right movement


    public CamPackageStruct(CinemachineCamera positionStandIn, int camPriorityIn, Vector3 forwardVectIn, Vector3 leftVectIn, bool flipUpIn, bool flipLeftIn)
    {
        this.thisCam = positionStandIn;
        this.camPriority = camPriorityIn;
        this.forwardVect = forwardVectIn;
        this.leftVect = leftVectIn;
        this.flipUp = flipUpIn;
        this.flipLeft = flipLeftIn;
    }
}
