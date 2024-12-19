using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;


public class SplineGrav : MonoBehaviour
{
    [TextArea]
    public string notes = "Pulls the player towards the point on the spline closest to them! Cool for drawing out custom shapes, cylinders, and Capsules. If you make the gravIntensity negative, it works great for the inside of cylinders and quarter/half pipes.";
    //Internal variables for center/size
    Vector3 m_Center = Vector3.zero;
    float m_Size = 50f;

    [Header("Spline Objects")]
    [SerializeField] SplineContainer splineToUse;
    [SerializeField]
    Transform nearestPointOnSpline;
    [Header("Gravity Fields")]
    [SerializeField] float gravIntensity = 30;
    [SerializeField] int priority = 0;

    void Start()
    {
        if(splineToUse == null)
            splineToUse = GetComponent<SplineContainer>();

        if (nearestPointOnSpline == null)
            Debug.LogError("Nearest Point GameObject is null");
    }

    private void OnTriggerStay(Collider other)
    {
        CustomGravityAffected gravityReceiver = other.gameObject.GetComponent<CustomGravityAffected>();
        if (gravityReceiver != null)
        {
            var nearest = new float4(0, 0, 0, float.PositiveInfinity);
                
            //Calculate nearest point on spline  nearest to player (or other gravity affected object)
            using var native = new NativeSpline(splineToUse.Spline, splineToUse.transform.localToWorldMatrix);
            float d = SplineUtility.GetNearestPoint(native, other.transform.position, out float3 p, out float t);
            if (d < nearest.w)
                nearest = new float4(p, d);

            nearestPointOnSpline.position = nearest.xyz;

            //Calculate vector from player to the closest point and send it over
            Vector3 pointAtNearest = (other.transform.position - nearestPointOnSpline.position).normalized * gravIntensity * -1;
            GravPackageStruct gravPackageToSend = new GravPackageStruct(pointAtNearest, priority);
            gravityReceiver.AddGravStruct(gravPackageToSend);

        }
            
    }
}
