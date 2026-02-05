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
            //transform.position = mainCam.transform.position + mainCam.transform.forward * zoffset + mainCam.transform.up * yoffset;
            Vector3 targetPos = mainCam.transform.position + mainCam.transform.forward * zoffset + mainCam.transform.up * yoffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);
            
            camEuler = mainCam.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, camEuler.y, 0);
        }
    }
}
