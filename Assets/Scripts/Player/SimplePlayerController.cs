using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [TextArea]
    public string notes = "Simple player movement script - it's a little crappy, but it has the basics in place, like movement dependent on the camera/gravity and override handlers. Ideally, you should replace the movement code here with stuff that suits your game.";

    [Header("Controls")]
    [SerializeField] KeyCode forward;
    [SerializeField] KeyCode backward;
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode jump;
    [SerializeField] KeyCode run;

    [Header("Physics")]
    [SerializeField] public Transform CameraContainer;
    [SerializeField] float speed = 3f;
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float doubleJumpForce = 7f;
    [SerializeField] Rigidbody rb;
    [SerializeField] CustomGravityAffected playerGrav;
    [SerializeField] GroundCheck groundCheck;

    [Header("RefVectors")]
    public Vector3 forwardVector;
    public Vector3 leftVector;
    public Vector3 normalVect;
    public Vector3 camToPlayer;
    public Vector3 inputDirection; 
    public Vector3 velocity;
    public Vector3 forwardVectOverride;
    public Vector3 leftVectOverride;
    //Helper variables
    bool flipUp = false;
    bool flipLeft = false;
    private void FixedUpdate()
    {
        UpdateForwardDirection();
        
        GetPlayerInput();

        TurnPlayer();

        velocity = rb.linearVelocity;
        //This is just to make the velocity viewable - I can't make it display on the rigidbody component itself for some reason
    }

    private void UpdateForwardDirection()
    {
        Vector3 forwardTemp = Vector3.zero;
        Vector3 leftTemp = Vector3.zero;

        //Calculate the normalVect (points opposite of gravity and is gravitational vertical height between the camear and player
        //Calculate the forwardVect (depends on camera angle unless overriden)
        //Calculate the left vector (uses cross product of forward/normal unless overriden
        camToPlayer = CameraContainer.position - transform.position;
        float angleBetweenNormalAndCam = Vector3.Angle(playerGrav.fallingDirection * -1, camToPlayer) * Mathf.Deg2Rad;
        float normalMag = Mathf.Cos(angleBetweenNormalAndCam) * camToPlayer.magnitude;
        normalVect = playerGrav.fallingDirection.normalized * Mathf.Abs(normalMag) * -1;
        forwardTemp = (camToPlayer - normalVect).normalized * -1;
        leftTemp = Vector3.Cross(forwardTemp, normalVect.normalized).normalized;
        forwardVector = Vector3.Cross(normalVect.normalized, leftTemp).normalized;
        leftVector = leftTemp;
        //Overrides (typically received from camera triggers)
        //Useful for inverted gravity shenanigans

        if (forwardVectOverride != Vector3.zero)
        {
            forwardVector = forwardVectOverride;
        }
 
        if(leftVectOverride != Vector3.zero)
        {
            leftVector = leftVectOverride;
        }
        
        if(flipUp)
        {
            forwardVector = -1 * forwardVector;
        }

        if(flipLeft)
        {
            leftVector = -1 * leftVector;
        }
        //This is kinda stupid but idk how else to do this
    }

    private void TurnPlayer()
    {
        //Turn player in the direction they're moving if the player's pushing buttons
        //If not, turn them in the direction of the forward vector (camera dependent or overriden)
        if (inputDirection.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(inputDirection, normalVect.normalized);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(forwardVector, normalVect.normalized);
        }
    }

    public void SetCamControls(Vector3 forwardVectIn, Vector3 leftVectIn, bool flipUpIn, bool flipLeftIn)
    {
        //Get cam overrides
        forwardVectOverride = forwardVectIn;
        leftVectOverride = leftVectIn;
        flipUp = flipUpIn;
        flipLeft = flipLeftIn;
    }

    private void GetPlayerInput()
    {
        //Simple player input thing - missing a lot, doesn't use the new input system, prone to "vectoring" (diagonal movement's faster)
        //Essentially the movement controls of the player
        //Kinda crappy 'cause you should write your own movement code here - the main purpose of this pack is the gravity thing
        inputDirection = new Vector3(0, 0, 0);
        if(Input.GetKey(forward))
        {
            inputDirection = inputDirection + (forwardVector);
        }
        if(Input.GetKey(left))
        {
            inputDirection = inputDirection + leftVector;
        }
        if(Input.GetKey(right))
        {
            inputDirection = inputDirection + leftVector * -1;
        }
        if(Input.GetKey(backward))
        {
            inputDirection = inputDirection + forwardVector * -1;
        }
        if(Input.GetKey(run))
        {
            inputDirection = inputDirection * runSpeed;
        }
        else
        {
            inputDirection = inputDirection * speed;
        }

        //add the movement force
        rb.AddForce(inputDirection, ForceMode.Force);

        //apply jump force
        if (groundCheck.isGrounded && Input.GetKey(jump))
        {
            rb.AddForce(normalVect.normalized * jumpForce, ForceMode.Impulse);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftVector);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, normalVect);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, forwardVector * 3);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.position - CameraContainer.position);
    }
}
