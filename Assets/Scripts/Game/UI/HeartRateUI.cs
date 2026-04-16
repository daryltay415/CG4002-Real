using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// This class updates the heart rate UI value and color
/// </summary>
public class HeartRateUI : MonoBehaviour
{
    public TMP_Text pulseElement;
    private Animator animator;
    private int redHeartHash;
    private int orangeHeartHash;
    private int greenHeartHash;
    public int heartRate = 123;
    
    public enum HeartRate
    {
        LOW = 0,
        MED = 1,
        HIGH = 2,
    }
    private HeartRate currentHeartRate = HeartRate.LOW;
    private HeartRate prevHeartRate = HeartRate.LOW;
    // Update is called once per frame
    void Start()
    {
        animator = GetComponent<Animator>();
        redHeartHash = Animator.StringToHash("HighRate");
        orangeHeartHash = Animator.StringToHash("MedRate");
        greenHeartHash = Animator.StringToHash("LowRate");
        MsgVisualiser.Instance.OnBPMDetected += UpdateBPM;
    }

    // Updates the BPM everytime a input is obtained from the gloves
    void UpdateBPM(int BPM)
    {
        Debug.Log("BPM: " + BPM);
        heartRate = BPM;
    }

    // Updates the text in the heart and the color of the heart UI
    void Update()
    {
        pulseElement.text = heartRate.ToString();
        if(heartRate < 80)
        {
            currentHeartRate = HeartRate.LOW;
        }
        else if(heartRate >= 80 && heartRate < 100)
        {
            currentHeartRate = HeartRate.MED;
        }
        else if(heartRate >= 100)
        {
            currentHeartRate = HeartRate.HIGH;
        }
        
        if(prevHeartRate != currentHeartRate)
        {
            prevHeartRate = currentHeartRate;
            switch (currentHeartRate)
            {
                case HeartRate.LOW:
                    setLow();
                    break;
                case HeartRate.MED:
                    setMed();
                    break;
                case HeartRate.HIGH:
                    setHigh();
                    break;
                default:
                    setLow();
                    break;
            }   
            
        }
    }

    void setLow()
    {
        animator.SetBool(greenHeartHash, true);
        animator.SetBool(orangeHeartHash, false);
        animator.SetBool(redHeartHash, false);
    }

    void setMed()
    {
        animator.SetBool(greenHeartHash, false);
        animator.SetBool(orangeHeartHash, true);
        animator.SetBool(redHeartHash, false);
    }

    void setHigh()
    {
        animator.SetBool(greenHeartHash, false);
        animator.SetBool(orangeHeartHash, false);
        animator.SetBool(redHeartHash, true);
    }

    private void OnDestroy() {
        MsgVisualiser.Instance.OnBPMDetected -= UpdateBPM;
    }
}
