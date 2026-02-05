using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetakeImage : MonoBehaviour
{
    [SerializeField] private Button RetakeImageButton;


    private void Start()
    {
        RetakeImageButton.onClick.AddListener(QuitGameToRetakeImage);
    }

    void QuitGameToRetakeImage()
    {
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);
    }
}