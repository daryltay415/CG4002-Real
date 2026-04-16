using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Toggles the color of the button
/// </summary>
public class UIButtonToggle : MonoBehaviour
{
    public static UIButtonToggle Instance;
    private Button cheatButton;
    public bool isCheatModeOn = false;
    private void Awake()
    {   
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
        
    }
    void Start()
    {
        cheatButton = gameObject.GetComponent<Button>();
        cheatButton.onClick.AddListener(onCheatButtonClicked);
    }

    // It toggles the button green or white when the button is clicked
    private void onCheatButtonClicked()
    {
        if (isCheatModeOn)
        {
            cheatButton.GetComponent<Image>().color = Color.white;
            isCheatModeOn = false;
        }
        else
        {
            cheatButton.GetComponent<Image>().color = Color.green;
            isCheatModeOn = true;
        }
    }


}
