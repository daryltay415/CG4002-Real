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
        if (IsOwner){
            //Vector3 targetPos = mainCam.transform.position + mainCam.transform.forward * zoffset + mainCam.transform.up * yoffset;
            //transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);
            //
            //camEuler = mainCam.transform.rotation.eulerAngles;
            //transform.rotation = Quaternion.Euler(0, camEuler.y, 0);

            // 1. Safety check: Are we parented to the Shared Origin?
        if (transform.parent == null) return;

        // 2. Convert World Camera Position to LOCAL Position relative to Batman
        // This is the "Magic Sauce" that makes everyone agree on location
        Vector3 cameraInSharedSpace = transform.parent.InverseTransformPoint(mainCam.transform.position);
        Vector3 forwardInSharedSpace = transform.parent.InverseTransformDirection(mainCam.transform.forward);
        Vector3 upInSharedSpace = transform.parent.InverseTransformDirection(mainCam.transform.up);

        // 3. Calculate target position in shared space
        Vector3 targetLocalPos = cameraInSharedSpace + (forwardInSharedSpace * zoffset) + (upInSharedSpace * yoffset);

        // 4. Smoothly move the transform's LOCAL position
        //transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPos, Time.deltaTime * 10f);
        //
        //// 5. Sync rotation relative to the shared space
        //Quaternion targetLocalRotation = Quaternion.LookRotation(new Vector3(forwardInSharedSpace.x, 0, forwardInSharedSpace.z));
        //transform.localRotation = Quaternion.Slerp(transform.localRotation, targetLocalRotation, Time.deltaTime * 10f);
        transform.localPosition = targetLocalPos;
        
        // Sync rotation relative to shared space
        if (forwardInSharedSpace.x != 0 || forwardInSharedSpace.z != 0)
        {
            transform.localRotation = Quaternion.LookRotation(new Vector3(forwardInSharedSpace.x, 0, forwardInSharedSpace.z));
        }
        }
    }
}
