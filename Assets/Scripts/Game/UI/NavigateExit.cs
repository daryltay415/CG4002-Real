using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigateExit : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    private bool buttonSelected = false;

    //private void OnEnable() {
    //    MsgVisualiser.Instance.OnExitNavDetected += OnExitNavDetectedInstance;
    //}

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnExitNavDetectedInstance("SELECT");
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            OnExitNavDetectedInstance("EXIT");
        }
    }

    void OnExitNavDetectedInstance(string nav)
    {
        switch (nav)
        {
            case "EXIT":
                ToggleButton();
                break;
            case "SELECT":
                if (buttonSelected)
                {
                    quitButton.onClick.Invoke();
                }
                break;
            default:
                break;
        }
    }
    
    private void ToggleButton()
    {
        if (buttonSelected)
        {
            quitButton.GetComponent<Image>().color = Color.white;
            buttonSelected = false;
        }
        else
        {
            quitButton.GetComponent<Image>().color = Color.green;
            buttonSelected = true;
        }
    }

    //private void OnDisable() {
    //    MsgVisualiser.Instance.OnExitNavDetected -= OnExitNavDetectedInstance;
    //}
}
