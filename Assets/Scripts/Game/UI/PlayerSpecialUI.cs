using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSpecialUI : MonoBehaviour
{
    public Image specialMeterBar;
    public bool isCoolDownActive = false;
    public float maxLevel;
    public float curLevel;
    public float chargeRate;
    // Start is called before the first frame update
    void Start()
    {
        specialMeterBar = GameObject.FindWithTag("Special").GetComponent<Image>();
    }

    public void DepleteMeter()
    {
        curLevel = 0;
        specialMeterBar.fillAmount = curLevel/maxLevel;
    }

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
