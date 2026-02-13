using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ManagerTest : MonoBehaviour
{
    void Start()
    {
        var manager = GetComponent<ARTrackedImageManager>();
        if (manager != null)
        {
            Debug.Log("MANAGER IS HERE");
            manager.trackedImagesChanged += (args) => Debug.Log("MANAGER IS WORKING!");
        }
        else
        {
            Debug.LogError("No ARTrackedImageManager on this object!");
        }
    }

    void Update()
    {
        var manager = GetComponent<ARTrackedImageManager>();

        // 1. Safety check: Does the component exist?
        if (manager == null) 
        {
            if (Time.frameCount % 60 == 0) Debug.LogError("ARTrackedImageManager component is MISSING on this GameObject!");
            return;
        }
    
        // 2. Check the Subsystem Heartbeat
        // This tells you if the AR engine is actually scanning for your 'Host' and 'Client' markers.
        if (manager.subsystem != null && manager.subsystem.running) 
        {
            if (Time.frameCount % 60 == 0) 
            {
                Debug.Log("<color=cyan><b>AR Status:</b> ACTIVE. Scanning for images...</color>");
            }
        }
        else 
        {
            if (Time.frameCount % 60 == 0) 
            {
                string reason = (manager.subsystem == null) ? "Subsystem is NULL" : "Subsystem is NOT running";
                Debug.LogWarning($"<color=red><b>AR Status:</b> {reason}. Check ARSession status and Library assignment.</color>");
            }
        }
    }
}