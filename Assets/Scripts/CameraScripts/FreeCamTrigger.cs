using System.ComponentModel;
using Unity.Cinemachine;
using UnityEngine;

public class FreeCamTrigger : MonoBehaviour
{
    [TextArea]
    public string notes = "Camera trigger - place this onto trigger boxes whenever you want the camera to change. Make sure to attach the correct camera too!";

    [Header("Important stuff")]
    [SerializeField] CinemachineCamera cam;
    [SerializeField] int priority = 0;

    [Header("Player Overrides (keep them as default for no overrides)")]
    [SerializeField] Vector3 forwardVect = Vector3.zero;
    [SerializeField] Vector3 leftVect = Vector3.zero;
    [SerializeField] bool flipUp = false;
    [SerializeField] bool flipLeft = false;

    private void Start()
    {
        if (cam == null)
            Debug.LogError("No camera attached to FreeCamTrigger!");
    }
    private void OnTriggerStay(Collider other)
    {
        CameraController camReceiver = other.gameObject.GetComponent<CameraController>();
        if (camReceiver != null )
        {
            //Send the CamPackageStruct to the player and camera
            CamPackageStruct newPackage = new CamPackageStruct(cam, priority, forwardVect, leftVect, flipUp, flipLeft);
            camReceiver.AddCam(newPackage);
        }
    }
}
