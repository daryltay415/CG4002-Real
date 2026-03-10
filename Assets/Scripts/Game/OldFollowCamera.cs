using Unity.Netcode;
using UnityEngine;

public class OldFollowCamera : NetworkBehaviour
{
    private Camera mainCam;
    public float xoffset = -0.2f;
    public float zoffset = 0.8f;
    public float yoffset = -0.8f;
    public float yRotationOffset = 0.1f;

    public override void OnNetworkSpawn()
    {   
        if (IsOwner)
        {
            mainCam = Camera.main;
            
            // OPTIONAL: If you want to be tidy, you can disable the 
            // NetworkTransform component here if it's attached, 
            // since we are doing local-only tracking now.
            //if(TryGetComponent<NetworkTransform>(out var nt)) nt.enabled = false;
        }
    }

    void Update()
    {
        // Only move the prefab that belongs to ME on MY screen.
        if (IsOwner)
        {
            if (mainCam == null) return;

            // Simple follow logic: Move to camera position + offsets
            Vector3 targetPos = mainCam.transform.position +
                                mainCam.transform.right * xoffset +  
                                mainCam.transform.forward * zoffset + 
                                mainCam.transform.up * yoffset;

            transform.position = targetPos;
            
            // Rotate to match camera face, but keep it upright (y-axis only)
            Vector3 camForward = mainCam.transform.forward;
            if (camForward.x != 0 || camForward.z != 0)
            {
                
                Quaternion baseRotation = Quaternion.LookRotation(new Vector3(camForward.x, 0, camForward.z));
                Quaternion offset = Quaternion.Euler(0, yRotationOffset, 0);
                // Combine them (order matters: base * offset)
                transform.rotation = baseRotation * offset;
            }
        }
    }
}