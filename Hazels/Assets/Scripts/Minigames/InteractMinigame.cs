using UnityEngine;

public class InteractMinigame : InteractiveObj
{
    public GameObject newParent;
    public GameObject newChestUI;
    public GameObject newCookedObjSpawn;

    public override void Interact()
    {
        Minigames.Instance.newCameraParent = newParent;
        Minigames.Instance.chestUI = newChestUI;
        Minigames.Instance.objSpawn = newCookedObjSpawn;

        if (Minigames.Instance.chestUI != null)
        {
            Minigames.Instance.chestSlots.AddRange(Minigames.Instance.chestUI.GetComponentsInChildren<Slot>());
        }

        Minigames.Instance.StartMiniGame();
    }
}
