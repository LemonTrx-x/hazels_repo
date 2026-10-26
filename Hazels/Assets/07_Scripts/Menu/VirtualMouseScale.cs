using UnityEngine;
using UnityEngine.InputSystem.UI;

public class VirtualMouseScale : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRectTransform;
    
    void Update()
    {
        transform.localScale = Vector3.one * (1f / canvasRectTransform.localScale.x);
    }
}
