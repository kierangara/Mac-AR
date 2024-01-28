using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class ClickableObjectBase : NetworkBehaviour
{
    public abstract void OnClick(Color newColor);
}
