using UnityEngine;
using UnityEngine.UI;
public class ToggleIngame : MonoBehaviour
{
    public Toggle VoiceToggle;
    //public static readonly VivoxUnity.Client mainClient = new VivoxUnity.Client();
    
// Start is called before the first frame update
    void Start()
    {
        VoiceToggle.onValueChanged.AddListener(delegate
            { VivoxToggle(VoiceToggle, PlayerList.mainClient); });

        if (PlayerList.muted == true)
        {
            PlayerList.mainClient.AudioInputDevices.Muted = true;
            VoiceToggle.isOn = true;
        } else if (PlayerList.muted == false)
        {
            PlayerList.mainClient.AudioInputDevices.Muted = false;
            VoiceToggle.isOn = false;
        }
    }

    void VivoxToggle(Toggle voiceToggle, VivoxUnity.Client client)
    {
        Debug.Log("Voice " + voiceToggle.isOn);
        if (voiceToggle.isOn)
        {
            client.AudioInputDevices.Muted = true;
        }
        else
        {
            client.AudioInputDevices.Muted = false;
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
