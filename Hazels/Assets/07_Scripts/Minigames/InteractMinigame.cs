using UnityEngine;

public class InteractMinigame : InteractiveObj
{
    public GameObject newParent;
    public GameObject chestUI;
    public GameObject newCookedObjSpawn;
    public int newCurrentMinigame;
    public int newProgressAmount;
    public int newMaxProgress;

    public override void Interact()
    {
        Minigames.Instance.newCameraParent = newParent;
        Minigames.Instance.objSpawn = newCookedObjSpawn;
        Minigames.Instance.currentMinigame = newCurrentMinigame;
        Minigames.Instance.maxProgress = newMaxProgress;
        Minigames.Instance.progressSlider.maxValue = newMaxProgress;

        if (chestUI != null)
        {
            //Add the chestUI slots to chestSlots
            Minigames.Instance.chestSlots.AddRange(chestUI.GetComponentsInChildren<Slot>());

            //Checks if all the items can be cooked in the minigame
            foreach (Slot slot in Minigames.Instance.chestSlots)
            {
                if (slot.itemData != null)
                {
                    if (slot.itemData.minigame != Minigames.Instance.currentMinigame)
                    {
                        return;
                    }

                    else
                    {
                        Minigames.Instance.StartMiniGame();
                    }
                }
            }
        }
    }
}
