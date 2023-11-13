using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TestPuzzleBehaviour : NetworkBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private CommonData commonData;
    [SerializeField] private Color lastKnownColor = Color.white;

    //Executed On Click
    public void OnClick()
    {
        Debug.Log("Click0");
        UpdateColourServerRpc(Color.red);
        Debug.Log("Click");
    }

    //Send new colour to server
    [ServerRpc(RequireOwnership = false)]
    public void UpdateColourServerRpc(Color newColor)
    {
        //May need to specifically loop through and send to only playerlist
        UpdateColourClientRpc(newColor);
    }

    //Send new colour to client
    [ClientRpc]
    public void UpdateColourClientRpc(Color newColor)
    {
        var cubeRenderer = cube.GetComponent<Renderer>();

        // Call SetColor using the shader property name "_Color" and setting the color to red
        cubeRenderer.material.SetColor("_Color", newColor);
    }

    // public override void OnNetworkSpawn()
    // {

    // }

    // public override void OnNetworkDespawn()
    // {

    // }

    // void Update()
    // {
    //     // var cubeRenderer = cube.GetComponent<Renderer>();

    //     // if(cubeRenderer.material.GetColor("_Color") != lastKnownColor)
    //     // {
    //     //     for (int i = 0; i < commonData.players.Count; i++)
    //     //     {
    //     //         commonData.players[i].CubeColor = cubeRenderer.material.GetColor("_Color");
    //     //     }
    //     // }

    //     // if(commonData.players[0].CubeColor != lastKnownColor)
    //     // {
    //     //     cubeRenderer.material.SetColor("_Color", commonData.players[0].CubeColor);
    //     //     lastKnownColor = commonData.players[0].CubeColor;
    //     // }
    // }
}
