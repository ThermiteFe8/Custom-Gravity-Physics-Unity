using UnityEngine;

public class ParallelGrav : MonoBehaviour
{
    [TextArea]
    public string notes = "Sets the player gravity to the direction of a single vector. Useful for planes, cubes, or wall-running sequences!";

    [SerializeField] Vector3 gravVector;
    [SerializeField] int priority = 0;
    private void OnTriggerStay(Collider other)
    {
        CustomGravityAffected gravityReceiver = other.gameObject.GetComponent<CustomGravityAffected>();
        if (gravityReceiver != null)
        {
            GravPackageStruct gravPackageToSend = new GravPackageStruct(gravVector, priority);
            gravityReceiver.AddGravStruct(gravPackageToSend);
        }
    }
}
