//Created by Matthew Collard
//Last Updated: 2024/04/04
using System;
using TMPro;
using UnityEngine;
//Updates the text on sliders to let the user know what value the slider is
public class SliderText : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    // Start is called before the first frame update

    public void SetSliderValue(float sliderValue)
    {
        
        double value=Math.Min(Math.Max(2.0, Mathf.RoundToInt(sliderValue * 10)),10);
        textComponent.text = value.ToString();
    }
}
