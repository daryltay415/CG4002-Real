using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class allows players to navigate the quit button during gameplay, tutorial and endgame
/// </summary>
public class NavigateExit : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    private bool buttonSelected = false;

    private void OnEnable() {
        MsgVisualiser.Instance.OnExitNavDetected += OnExitNavDetectedInstance;
    }


    // Highlights the quit button depending on the input received from the gloves
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
    
    // Toggles the quit button white or green to show it is selected
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

    private void OnDisable() {
        MsgVisualiser.Instance.OnExitNavDetected -= OnExitNavDetectedInstance;
    }
}
