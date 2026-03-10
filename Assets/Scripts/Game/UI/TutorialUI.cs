using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public GameObject[] guides;
    public GameObject exitUI;
    private const float DURATION_OF_DISPLAY = 1f;
    public CanvasGroup canvasGroup;
    private void OnEnable() {
        //jabGuide.SetActive(false);
        //walkGuide.SetActive(true);
        //uppercutGuide.SetActive(false);
        //hookGuide.SetActive(false);
        //blockGuide.SetActive(false);
        //specialGuide.SetActive(false);
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
            yield return new WaitForSeconds(DURATION_OF_DISPLAY);
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
}
