using UnityEngine;

public class SphereGrav : MonoBehaviour
{
    [TextArea]
    public string notes = "Pulls the player towards a central point. Useful for spherical plants or the ends of capsule-shaped ones. Note that centerPosition is relative to to this transform's position. In other words <0, 0, 0> would use the transform as the center.";

    [SerializeField] Vector3 centerPosition = Vector3.zero;
    [SerializeField] float gravIntensity = 30;
    [SerializeField] int priority = 0;
    private void OnTriggerStay(Collider other)
    {
        CustomGravityAffected gravityReceiver = other.gameObject.GetComponent<CustomGravityAffected>();
        if (gravityReceiver != null)
        {
            //Calculate the Vector pointing from the player (or other gravity affected object) to the center
            Vector3 pointAtCenter = (other.transform.position - (centerPosition + transform.position)).normalized * gravIntensity * -1;
            //Send gravity data to the player
            GravPackageStruct gravPackageToSend = new GravPackageStruct(pointAtCenter, priority);
            gravityReceiver.AddGravStruct(gravPackageToSend);
        }
    }
}
