using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Camera mainCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mainCam.transform.position;
    }
}
