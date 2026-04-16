using Unity.Netcode;
using UnityEngine;
using Unity.XR.CoreUtils;
/// <summary>
/// This class positions the player's sprite infront of the camera during AR gameplay
/// </summary>
public class FollowCamera : NetworkBehaviour
{
    private Camera mainCam;
    public float xoffset = -0.2f;
    public float zoffset = 0.8f;
    public float yoffset = -0.8f;
    public float yRotationOffset = 23f;
    private Vector3 camEuler;
    private XROrigin xrOrigin;

    public override void OnNetworkSpawn()
    {   
    if (IsOwner)
        {
            mainCam = Camera.main;
        }
    }

    void Start()
    {
        xrOrigin = FindObjectOfType<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {

        if (IsOwner)
        {
            Transform sharedOrigin = transform.parent;

            // 1. Get Camera position relative to the Shared Origin
            Vector3 localCamPos = sharedOrigin.InverseTransformPoint(mainCam.transform.position);

            // 2. Get Camera forward direction relative to the Shared Origin's rotation
            // Translates the phone's "forward" into the Shared Origin's coordinate system.
            Vector3 localCamForward = sharedOrigin.InverseTransformDirection(mainCam.transform.forward);

            // 3. Calculate the offset using the SHARED LOCAL forward
            // everyone sees the sprite moved in that same shared direction.
            Vector3 localForwardFlat = new Vector3(localCamForward.x, 0, localCamForward.z).normalized;
            Vector3 targetLocalPos = new Vector3(
                localCamPos.x + (localForwardFlat.x * zoffset), 
                yoffset, // This forces the sprite to the "floor"
                localCamPos.z + (localForwardFlat.z * zoffset)
                );
            // 4. Apply to transform
            transform.localPosition = targetLocalPos;
            
            // 5. Sync Rotation
            if (localForwardFlat != Vector3.zero)
            {
                transform.localRotation = Quaternion.LookRotation(localForwardFlat, Vector3.up);
            }

        }
    }
}
