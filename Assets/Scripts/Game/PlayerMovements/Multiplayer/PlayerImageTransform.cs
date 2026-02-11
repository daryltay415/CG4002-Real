using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.ARFoundation;
using Unity.Netcode.Components;

public class PlayerImageTransform : NetworkBehaviour
{
    private ARTrackedImageManager _imageManager;
    private bool _isImageVisible;
    private Vector3 _arPosition;
    private Quaternion _arRotation;
    private Transform _arTrans;
    public float zoffset = 0.8f;
    public float yoffset = -0.8f;
    private Vector3 camEuler;

    [SerializeField] private float _smoothSpeed = 15f;

    public override void OnNetworkSpawn()
    {
        // We only override the OTHER player
        if (IsOwner) return;

        _imageManager = FindFirstObjectByType<ARTrackedImageManager>();
        _imageManager.trackedImagesChanged += OnImagesChanged;
    }

    private void OnImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var img in args.updated)
        {
            if (img.referenceImage.name == "Client" && IsHost)
            {
                _isImageVisible = img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking;
                _arPosition = img.transform.position;
                _arRotation = img.transform.rotation;
                _arTrans = img.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (IsOwner || !_isImageVisible) return;

        // LATE UPDATE is key: it runs AFTER the NetworkTransform has moved the object.
        // We now "snap" it back to the AR position.
        //transform.position = Vector3.Lerp(transform.position, _arPosition, Time.deltaTime * _smoothSpeed);
        //transform.rotation = Quaternion.Slerp(transform.rotation, _arRotation, Time.deltaTime * _smoothSpeed);
        Vector3 targetPos = _arPosition + _arTrans.forward * zoffset + _arTrans.up * yoffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);
            
        camEuler = _arRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, camEuler.y, 0);
    
    }
}
