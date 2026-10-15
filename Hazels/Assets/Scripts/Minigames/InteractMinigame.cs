using UnityEngine;

public class InteractMinigame : InteractiveObj
{
    public GameObject newParent;
    public GameObject chestUI;
    public GameObject newCookedObjSpawn;
    public int newCurrentMinigame;

    public override void Interact()
    {
        Minigames.Instance.newCameraParent = newParent;
        Minigames.Instance.objSpawn = newCookedObjSpawn;
        Minigames.Instance.currentMinigame = newCurrentMinigame;

        if (chestUI != null)
        {
            //Add the chestUI slots to chestSlots
            Minigames.Instance.chestSlots.AddRange(chestUI.GetComponentsInChildren<Slot>());

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
