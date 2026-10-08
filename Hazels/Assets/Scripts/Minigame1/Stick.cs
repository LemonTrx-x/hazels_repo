using UnityEngine;
using UnityEngine.EventSystems;

public class Stick : MonoBehaviour ,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform stickParent;
    public bool dragActive;

    public Minigame1 minigame1;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragActive = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragActive == true)
        {
            this.transform.position = new Vector3(eventData.pointerCurrentRaycast.worldPosition.x, this.transform.position.y, eventData.pointerCurrentRaycast.worldPosition.z);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragActive = false;
    }
}
