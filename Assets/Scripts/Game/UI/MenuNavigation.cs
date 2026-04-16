using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// This class manages the menu navigation in the menu screen
/// </summary>
public class MenuNavigation : MonoBehaviour
{
    public Button[] leftMenuButtons;    
    private Button[] currentMenuButton;
    public GameObject[] leftMenuImages;
    private GameObject[] currentMenuImage;
    private int index=1;
    

    void Start()
    {
        MsgVisualiser.Instance.OnNavDetected += OnNavigationDetectedInstance;
        currentMenuButton = leftMenuButtons;
        currentMenuImage = leftMenuImages;
        UpdateHighlightButtons();
    }

    // Highlights the current button chosen
    void UpdateHighlightButtons()
    {
        if (currentMenuButton[index].gameObject.activeInHierarchy)
        {
            foreach (var img in leftMenuImages)
            {
                img.SetActive(false);
            }
            currentMenuImage[index].SetActive(true);
        }
        
    }

    // Starts the function associated to that button chosen
    void UpdateSelectedButtons()
    {
        currentMenuButton[index].onClick.Invoke();
    }

    // Updates the buttons highlighted when the navigation input is received from the gloves
    void OnNavigationDetectedInstance(string nav)
    {
        switch (nav)
        {
            case "UP":
                if (index-1 >=0)
                {
                    index--;
                }
                break;
            case "DOWN":
                if (index + 1 < currentMenuImage.Length )
                {
                    index++;
                }
                break;
            case "SELECT":
                UpdateSelectedButtons();
                break;
            default :
                break;
        }
        UpdateHighlightButtons();
    }

    private void OnDestroy() {
        MsgVisualiser.Instance.OnNavDetected -= OnNavigationDetectedInstance;
    }
}
