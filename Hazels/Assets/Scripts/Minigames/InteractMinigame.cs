using UnityEngine;

public class InteractMinigame : InteractiveObj
{
    public GameObject newParent;
    public GameObject newChestUI;
    public GameObject newCookedObjSpawn;
    public int newCurrentMinigame;

    public override void Interact()
    {
        Minigames.Instance.newCameraParent = newParent;
        Minigames.Instance.chestUI = newChestUI;
        Minigames.Instance.objSpawn = newCookedObjSpawn;
        Minigames.Instance.currentMinigame = newCurrentMinigame;

        if (Minigames.Instance.chestUI != null)
        {
            Minigames.Instance.chestSlots.AddRange(Minigames.Instance.chestUI.GetComponentsInChildren<Slot>());

            for (int i = 0; i < Minigames.Instance.chestSlots.Count; i++)
            {
                if (Minigames.Instance.chestSlots[i].itemData != null)
                {
                    Minigames.Instance.StartMiniGame();
                }

                else
                {
                    Debug.Log("Put items in the chest to start the minigame.");
                }
            }
        }
    }
}
