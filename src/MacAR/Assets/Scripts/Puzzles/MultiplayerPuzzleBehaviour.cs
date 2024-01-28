using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MultiplayerPuzzleBehaviour : ClickableObjectBase
{
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject sphere;

    //Executed On Click
    public override void OnClick(Color newColor)
    {
        var cubeRenderer = cube.GetComponent<Renderer>();
        var sphereRenderer = sphere.GetComponent<Renderer>();
        
        if(cubeRenderer.material.GetColor("_Color") == newColor)
        {
            newColor = Color.white;
        }

        // Update Local First
        cubeRenderer.material.SetColor("_Color", newColor);
        sphereRenderer.material.SetColor("_Color", newColor);

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
        var sphereRenderer = sphere.GetComponent<Renderer>();

        // Call SetColor using the shader property name "_Color" and setting the color to red
        cubeRenderer.material.SetColor("_Color", newColor);
        sphereRenderer.material.SetColor("_Color", newColor);
    }
}
