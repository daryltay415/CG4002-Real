using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.ARFoundation;
using Unity.Netcode.Components;

public class PlayerImageTransform : NetworkBehaviour
{
    private ARTrackedImageManager _imageManager;
    
    // Set these in the prefab's Inspector
    [SerializeField] private string hostImageName = "HostMarker";
    [SerializeField] private string clientImageName = "ClientMarker";

    public override void OnNetworkSpawn()
    {
        // I don't need to track MYSELF via image, only the opponent
        if (IsOwner) return;

        _imageManager = FindFirstObjectByType<ARTrackedImageManager>();
        if (_imageManager != null)
        {
            _imageManager.trackedImagesChanged += OnImagesChanged;
        }
    }

    private void OnImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        Debug.Log($"Images changed! Added: {args.added.Count}, Updated: {args.updated.Count}");
        // Determine which image we are looking for based on who we are
        // If I am Host (ID 0), I look for the Client image to find my opponent
        string targetImage = NetworkManager.Singleton.LocalClientId == 0 ? clientImageName : hostImageName;

        foreach (var img in args.updated)
        {
            if (img.referenceImage.name == targetImage)
            {
                if (img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
                {
                    // Snap the prefab to the physical image
                    transform.position = img.transform.position;
                    transform.rotation = img.transform.rotation;
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (_imageManager != null)
            _imageManager.trackedImagesChanged -= OnImagesChanged;
    }
}
