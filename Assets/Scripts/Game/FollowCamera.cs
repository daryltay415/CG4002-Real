using Unity.Netcode;
using UnityEngine;

public class FollowCamera : NetworkBehaviour
{
    private Camera mainCam;
    public float zoffset = 0.8f;
    public float yoffset = -0.8f;
    private Vector3 camEuler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnNetworkSpawn()
    {   
    if (IsOwner)
        {
            mainCam = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (IsOwner){
            // 1. Safety check: Are we parented to the Shared Origin?
            if (transform.parent == null) return;

        // 2. Convert World Camera Position to LOCAL Position relative to Batman
        // This is the "Magic Sauce" that makes everyone agree on location
            Vector3 cameraInSharedSpace = transform.parent.InverseTransformPoint(mainCam.transform.position);
            Vector3 forwardInSharedSpace = transform.parent.InverseTransformDirection(mainCam.transform.forward);
            Vector3 upInSharedSpace = transform.parent.InverseTransformDirection(mainCam.transform.up);

        // 3. Calculate target position in shared space
            Vector3 targetLocalPos = cameraInSharedSpace + (forwardInSharedSpace * zoffset) + (upInSharedSpace * yoffset);

        //// 5. Sync rotation relative to the shared space
            transform.localPosition = targetLocalPos;
        
        // Sync rotation relative to shared space
            if (forwardInSharedSpace.x != 0 || forwardInSharedSpace.z != 0)
            {
                transform.localRotation = Quaternion.LookRotation(new Vector3(forwardInSharedSpace.x, 0, forwardInSharedSpace.z));
            }
        }
        */
        

        
        if (IsOwner)
        {
            Transform sharedOrigin = transform.parent;

            // 1. Get Camera position relative to the Shared Origin
            Vector3 localCamPos = sharedOrigin.InverseTransformPoint(mainCam.transform.position);

            // 2. Get Camera forward direction relative to the Shared Origin's rotation
            // This is the critical step: we translate the phone's "forward" 
            // into the Shared Origin's coordinate system.
            Vector3 localCamForward = sharedOrigin.InverseTransformDirection(mainCam.transform.forward);

            // 3. Flatten the direction (so the sprite doesn't tilt into the ground)
            

            // 4. Calculate the offset using the SHARED LOCAL forward
            // If localForwardFlat is (1,0,0) in the shared space, 
            // everyone sees the sprite moved in that same shared direction.
            Vector3 localForwardFlat = new Vector3(localCamForward.x, 0, localCamForward.z).normalized;
            //Vector3 targetLocalPos = localCamPos + (localForwardFlat * zoffset) + (Vector3.up * yoffset);
            Vector3 upInSharedSpace = sharedOrigin.InverseTransformDirection(mainCam.transform.up);
            Vector3 targetLocalPos = localCamPos + (localCamForward * zoffset) + (upInSharedSpace * yoffset);
            // 5. Apply to transform
            transform.localPosition = targetLocalPos;
            
            // 6. Sync Rotation
            if (localForwardFlat != Vector3.zero)
            {
                transform.localRotation = Quaternion.LookRotation(localForwardFlat, Vector3.up);
            }

        }
    }
}
