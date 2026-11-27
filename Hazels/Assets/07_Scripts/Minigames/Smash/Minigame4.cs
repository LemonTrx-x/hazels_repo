using UnityEngine;
using UnityEngine.InputSystem;

public class Minigame4 : MonoBehaviour
{
    public InteractMinigame interactMinigame;

    [Header("Minigame")]
    public int divideAmount = 50;
    public int difficultDivideAmount = 20;
    
    [Header("Hand")]
    public GameObject hand;
    public GameObject handFBX;

    public Transform handInitialPos;
    private Quaternion handInitialRot;

    public Transform handNewPos;
    private Quaternion handNewRot = Quaternion.Euler(270, 0, 90);

    public bool goToNew = false;
    public bool goToOld = false;

    public float moveTime = 1f;
    
    void Start()
    {
        handInitialPos.position = hand.transform.position;
        handInitialRot = handFBX.transform.localRotation;
    }

    void Update()
    {
        if (Minigames.Instance.currentMinigame == interactMinigame.newCurrentMinigame && hand != null)
        {
            ChangeHandPos();

            if (GameManager.Instance.inMinigame)
            {
                GetComponentInParent<Collider>().enabled = false;

                LowProgress();
            }

            else
            {
                GetComponentInParent<Collider>().enabled = true;
            }
        }
    }

    public void ChangeHandPos()
    {
        if (goToNew)
        {
            hand.transform.position = Vector3.Lerp(hand.transform.position, handNewPos.position, moveTime * Time.deltaTime);
            handFBX.transform.rotation = Quaternion.Lerp(handFBX.transform.rotation, handNewRot, moveTime * Time.deltaTime);

            if (Vector3.Distance(hand.transform.position, handNewPos.transform.position) < 0.001f)
            {
                hand.transform.position = handNewPos.transform.position;
                handFBX.transform.localRotation = handNewRot;
                goToNew = false;
            }
        }

        if (goToOld)
        {
            hand.transform.position = Vector3.Lerp(hand.transform.position, handInitialPos.position, moveTime * Time.deltaTime);
            handFBX.transform.localRotation = Quaternion.Lerp(handFBX.transform.localRotation, handInitialRot, moveTime * Time.deltaTime);

            if (Vector3.Distance(hand.transform.position, handInitialPos.position) < 0.001f)
            {
                hand.transform.position = handInitialPos.position;
                handFBX.transform.localRotation = handInitialRot;
                goToOld = false;
            }
        }
    }

    public void SmashMaterials(InputAction.CallbackContext callbackContext)
    {
        if (goToNew || goToOld) return;
        
        if (callbackContext.performed &&
            GameManager.Instance.inMinigame &&
            Minigames.Instance.currentMinigame == interactMinigame.newCurrentMinigame)
        {
            Minigames.Instance.progress += interactMinigame.newProgressAmount;
            Debug.Log("Smashed");
        }
    }

    public void LowProgress()
    {
        if (Minigames.Instance.progress > 0)
        {
            if (Minigames.Instance.progress >= Minigames.Instance.maxProgress / 2)
            {
                Minigames.Instance.progress -= interactMinigame.newProgressAmount / difficultDivideAmount;
            }

            else
            {
                Minigames.Instance.progress -= interactMinigame.newProgressAmount / divideAmount;
            }
        }
    }
}
