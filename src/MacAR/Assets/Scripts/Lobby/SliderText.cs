using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
