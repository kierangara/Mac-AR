using System;
using TMPro;
using UnityEngine;

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
