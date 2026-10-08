using UnityEngine;
using UnityEngine.EventSystems;

public class DragStick : MonoBehaviour ,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject stick;
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
        if (GameManager.Instance.inMinigame)
        {
            dragActive = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.inMinigame)
        {
            if (dragActive == true)
            {
                this.transform.position = Input.mousePosition;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.inMinigame)
        {
            dragActive = false;
        }
    }
}
