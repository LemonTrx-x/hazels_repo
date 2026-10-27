using UnityEngine;

public class InteractPC : InteractiveObj
{
    public GameObject newParent;
    public int newCurrentMinigame = 10;

    public override void Interact()
    {
        GameManager.Instance.inPC = true;
        Minigames.Instance.newCameraParent = newParent;
        Minigames.Instance.currentMinigame = newCurrentMinigame;
        Minigames.Instance.StartMiniGame();
    }
}
