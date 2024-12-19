using System.Dynamic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

//Internal structure used for sending gravity data from the trigger to the player or other gravity affected item
public class GravPackageStruct : Object
{
    public Vector3 gravVect;
    public int gravPriority;

    public GravPackageStruct(Vector3 gravVect, int gravPriority)
    {
        this.gravVect = gravVect;
        this.gravPriority = gravPriority;
    }
}
