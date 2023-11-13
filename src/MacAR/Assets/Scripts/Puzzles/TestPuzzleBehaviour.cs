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
        var cubeRenderer = cube.GetComponent<Renderer>();
        Color newColor = Color.red;
        
        if(cubeRenderer.material.GetColor("_Color") == newColor)
        {
            newColor = Color.white;
        }

        // Update Local First
        cubeRenderer.material.SetColor("_Color", newColor);

        // Update Other Clients on Network
        UpdateColourServerRpc(newColor);
    }

    //Send new colour to server
    [ServerRpc(RequireOwnership = false)]
    public void UpdateColourServerRpc(Color newColor)
    {
        // Original client gets updated twice, can optimize
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
}
