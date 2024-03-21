using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour
{
    public Button skipButton;

	void Start () {
		Button btn = skipButton.GetComponent<Button>();
		btn.onClick.AddListener(SkipButtonClick);
	}

	void SkipButtonClick(){
		Debug.Log ("You have clicked the button!");
	}
}
