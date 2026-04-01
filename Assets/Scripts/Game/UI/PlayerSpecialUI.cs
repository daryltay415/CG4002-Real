using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Manages the special bar UI in the game to allow it to recharge or deplete
/// </summary>

public class PlayerSpecialUI : MonoBehaviour
{
    public Image specialMeterBar;
    private UIManager uimanager;
    public bool isCoolDownActive = false;
    public float maxLevel; // max level the special meter can reach
    public float curLevel; // current level of the special meter
    public float chargeRate; 

    void Start()
    {
        uimanager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        specialMeterBar = uimanager.specialMeterBar;
        if (UIButtonToggle.Instance.isCheatModeOn)
        {
            chargeRate = 50f;
        }
        else
        {
            chargeRate = 20f;
        }
    }

    // Depletes the meter when the player shoots a projectile
    public void DepleteMeter()
    {
        curLevel = 0;
        specialMeterBar.fillAmount = curLevel/maxLevel;
    }

    // Recharges the sepcial meter via a coroutine
    public IEnumerator Recharge()
    {
        isCoolDownActive = true;
        yield return new WaitForSeconds(1f);
        while(curLevel < maxLevel)
        {
            curLevel += chargeRate / 10f;
            if(curLevel > maxLevel)
            {
                curLevel = maxLevel;
            }
            specialMeterBar.fillAmount = curLevel/maxLevel;
            yield return new WaitForSeconds(.1f);
            
        }
        isCoolDownActive = false;
    }
}
