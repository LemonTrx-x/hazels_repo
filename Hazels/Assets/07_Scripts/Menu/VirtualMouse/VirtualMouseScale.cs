using UnityEngine;

public class VirtualMouseScale : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRectTransform;
    
    void Update()
    {
        transform.localScale = new Vector2(Screen.width / 2, Screen.height / 2);
    }
}
