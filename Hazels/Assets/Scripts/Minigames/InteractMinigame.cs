using UnityEngine;

public class InteractMinigame : InteractiveObj
{
    public GameObject newParent;

    public override void Interact()
    {
        Minigames.Instance.newCameraParent = newParent;
        Minigames.Instance.StartMiniGame();
    }
}
