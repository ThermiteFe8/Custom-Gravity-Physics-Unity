using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded;
    private void OnTriggerEnter(Collider other)
    {
        isGrounded = true;
    }

    private void OnTriggerStay(Collider other)
    {
        isGrounded = true;
        //This is technically redundant, but it helps out with strange edge cases that break everything
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }
}
