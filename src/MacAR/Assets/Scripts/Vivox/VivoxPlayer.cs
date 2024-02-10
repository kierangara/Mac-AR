using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Vivox;
using VivoxUnity;
using UnityEngine.Android;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class VivoxPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public VivoxVoiceManager _vvm;

    IChannelSession _chan;
    private int PermissionAskedCount;
    private string VoiceChannelName = "TestChannel";
    public string lobbyer;


    



   
    

    void Start()
    {
        _vvm = VivoxVoiceManager.Instance;
        _vvm.OnUserLoggedInEvent += OnUserLoggedIn;
        _vvm.OnUserLoggedOutEvent += OnUserLoggedOut;
        
    }

    public void SignIntoVivox(string playerName)
    {
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
    bool IsAndroid12AndUp()
    {
        // android12VersionCode is hardcoded because it might not be available in all versions of Android SDK
        const int android12VersionCode = 31;
        AndroidJavaClass buildVersionClass = new AndroidJavaClass("android.os.Build$VERSION");
        int buildSdkVersion = buildVersionClass.GetStatic<int>("SDK_INT");

        return buildSdkVersion >= android12VersionCode;
    }

    string GetBluetoothConnectPermissionCode()
    {
        if (IsAndroid12AndUp())
        {
            // UnityEngine.Android.Permission does not contain the BLUETOOTH_CONNECT permission, fetch it from Android
            AndroidJavaClass manifestPermissionClass = new AndroidJavaClass("android.Manifest$permission");
            string permissionCode = manifestPermissionClass.GetStatic<string>("BLUETOOTH_CONNECT");

            return permissionCode;
        }
        return "";
    }
#endif

        bool IsMicPermissionGranted()
        {
            bool isGranted = Permission.HasUserAuthorizedPermission(Permission.Microphone);
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (IsAndroid12AndUp())
        {
            // On Android 12 and up, we also need to ask for the BLUETOOTH_CONNECT permission for all features to work
            isGranted &= Permission.HasUserAuthorizedPermission(GetBluetoothConnectPermissionCode());
        }
#endif
            return isGranted;
        }

        void AskForPermissions()
        {
            string permissionCode = Permission.Microphone;

#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (PermissionAskedCount == 1 && IsAndroid12AndUp())
        {
            permissionCode = GetBluetoothConnectPermissionCode();
        }
#endif
            PermissionAskedCount++;
            Permission.RequestUserPermission(permissionCode);
        }

        bool IsPermissionsDenied()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        // On Android 12 and up, we also need to ask for the BLUETOOTH_CONNECT permission
        if (IsAndroid12AndUp())
        {
            return PermissionAskedCount == 2;
        }
#endif
            return PermissionAskedCount == 1;
        }
        //Actual code runs from here
        if (IsMicPermissionGranted())
        {
            _vvm.Login(_vvm.PlayerName);
        }
        else
        {
            if (IsPermissionsDenied())
            {
                PermissionAskedCount = 0;
                _vvm.Login(_vvm.PlayerName);
            }
            else
            {
                AskForPermissions();
                _vvm.Login(_vvm.PlayerName);      //NEED TO FIX !
            }
        }
    }

    public void setLobby(Lobby lobby)
    {
        lobbyer = lobby.Data["JoinCode"].Value;
    }

    public void setJoinCode(string lobbyCode)
    {
        this.lobbyer = lobbyCode;
    }


    void OnUserLoggedIn ()
    {
        if (_vvm.LoginState == VivoxUnity.LoginState.LoggedIn)
        {
            Debug.Log("Successfully connected to Vivox");
            Debug.Log("Joining voice channel: " + lobbyer);
            
            _vvm.JoinChannel(lobbyer, ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.TextAndAudio);

        }
        else
        {
            Debug.Log("Cannot sign into Vivox, check your credentials and token settings");
        }
    }

    void OnUserLoggedOut ()
    {
        Debug.Log("Disconnecting from voice channel " + VoiceChannelName);
        _vvm.DisconnectAllChannels();
        Debug.Log("Disconnecting from Vivox");
        _vvm.Logout();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
