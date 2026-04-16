using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages the tutorial guide for the player in tutorial mode
/// </summary>
public class TutorialUI : MonoBehaviour
{
    public GameObject[] guides;
    public GameObject exitUI;
    private GameObject player;
    private PlayerStateMachineMultiplayer psmmpComponent;
    public AudioSource audioSource;
    private const float DURATION_OF_DISPLAY = 1f;
    public CanvasGroup canvasGroup;
    private void OnEnable() {
        player = GameObject.FindWithTag("Player");
        psmmpComponent = player.GetComponent<PlayerStateMachineMultiplayer>();
        canvasGroup.alpha = 0;
        foreach(var guide in guides)
        {
            guide.SetActive(false);
        }
        StartCoroutine(TogglePanel());
    }

    // Call this method to fade the panel in or out
    IEnumerator TogglePanel()
    {
        foreach(var guide in guides)
        {
            guide.SetActive(true);
            yield return StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1, 3f));
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            bool correctGesture = false;
            while(!correctGesture)
            {
                if(guide.tag == "Walk" && psmmpComponent._camIsMoving)
                {
                    correctGesture = true;
                    break;
                }
                else if(guide.tag == "Guard" && psmmpComponent._isGuardingPressed)
                {
                    correctGesture = true;
                    break;
                }
                else if(guide.tag == psmmpComponent.atktype.ToString() && (psmmpComponent._stillAttacking == 1))
                {
                    correctGesture = true;
                    break;
                }
                yield return null;
            }
            //Play correct sound
            audioSource.Play();
            yield return StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0, 3f));
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            guide.SetActive(false);
        }
        exitUI.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1, 3f));
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        
        
    }

    // Coroutine to fade the canvasgroup
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;
        float currentAlpha = startAlpha;

        while (Time.time <= endTime)
        {
            currentAlpha = Mathf.Lerp(startAlpha, endAlpha, (Time.time - startTime) / duration);
            cg.alpha = currentAlpha;
            yield return null;
        }

        cg.alpha = endAlpha; 
    }

    string ConvertAtkTypeToString(int atktype)
    {
        switch (atktype) {
            case 1: 
                return "leftJab";
            case 2: 
                return "rightJab";
            case 3:
                return "shoot";
            case 4:
                return "leftHook";
            case 5:
                return "rightHook";
            case 6:
                return "leftUpper";
            case 7:
                return "rightUpper";
            default:
                return "Null";
        }
    }
}
