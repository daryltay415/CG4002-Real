using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        OnNavigationDetectedInstance("DOWN");
    //    }
    //    else if (Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        OnNavigationDetectedInstance("UP");
    //    }
    //    else if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        OnNavigationDetectedInstance("RIGHT");
    //    }
    //    else if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        OnNavigationDetectedInstance("LEFT");
    //    } 
    //    else if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        OnNavigationDetectedInstance("SELECT");
    //    }
    //}

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

    void UpdateSelectedButtons()
    {
        currentMenuButton[index].onClick.Invoke();
    }

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
            //case "LEFT":
            //    currentMenuButton = leftMenuButtons;
            //    currentMenuImage = leftMenuImages;
            //    index = 0;
            //    break;
            //case "RIGHT":
            //    currentMenuButton = rightMenuButtons;
            //    currentMenuImage = rightMenuImages;
            //    index = 0;
            //    break;
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
