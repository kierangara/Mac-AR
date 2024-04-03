using UnityEngine;
public class ButtonClick : MonoBehaviour
{
    public ClickableObjectBase networkObject;

    void OnMouseDown()
    {
        var buttonRender = gameObject.GetComponent<Renderer>();
        networkObject.OnClick(buttonRender.material.GetColor("_Color"));
    }
}
