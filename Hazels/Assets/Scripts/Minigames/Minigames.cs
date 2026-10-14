using System.Collections.Generic;
using UnityEngine;

public class Minigames : MonoBehaviour
{
    public static Minigames Instance { get; private set; }

    public GameObject pointer;
    public GameObject hotBar;

    public int progress = 0;

    [Header("Camera")]
    public GameObject cameraPlayer;
    public GameObject newCameraParent;
    public GameObject oldCameraParent;
    public float camSpeed = 5f;

    private bool isMovingToNew = false;
    private bool isMovingToOld = false;

    [Header("Chest")]
    public GameObject chestUI;
    public GameObject objSpawn;
    public int spawnForce = 50;
    [HideInInspector] public List<Slot> chestSlots = new List<Slot>();
    public List<RecipeData> allRecipes;
    public int sameItems = 0;
    private List<ItemData> itemsToCheck = new List<ItemData>();

    [Header("Minigames")]
    public int currentMinigame = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        //Moving camera to new position
        if (isMovingToNew)
        {
            cameraPlayer.transform.position = Vector3.Lerp(cameraPlayer.transform.position, newCameraParent.transform.position, camSpeed * Time.deltaTime);
            cameraPlayer.transform.rotation = Quaternion.Lerp(cameraPlayer.transform.rotation, newCameraParent.transform.rotation, camSpeed * Time.deltaTime);

            //If camera is too close, we stop the movement to save recources
            if (Vector3.Distance(cameraPlayer.transform.position, newCameraParent.transform.position) < 0.001f)
            {
                cameraPlayer.transform.position = newCameraParent.transform.position;
                cameraPlayer.transform.rotation = newCameraParent.transform.rotation;
                isMovingToNew = false;
            }
        }

        //Moving camera to original position
        if (isMovingToOld)
        {
            cameraPlayer.transform.position = Vector3.Lerp(cameraPlayer.transform.position, oldCameraParent.transform.position, camSpeed * Time.deltaTime);
            cameraPlayer.transform.rotation = Quaternion.Lerp(cameraPlayer.transform.rotation, oldCameraParent.transform.rotation, camSpeed * Time.deltaTime);

            if (Vector3.Distance(cameraPlayer.transform.position, oldCameraParent.transform.position) < 0.001f)
            {
                cameraPlayer.transform.position = oldCameraParent.transform.position;
                cameraPlayer.transform.rotation = oldCameraParent.transform.rotation;
                isMovingToOld = false;
            }
        }

        CheckProgress();
    }

    public void StartMiniGame()
    {
        oldCameraParent.transform.rotation = cameraPlayer.transform.rotation;

        isMovingToNew = true;
        isMovingToOld = false;

        Cursor.lockState = CursorLockMode.None;

        pointer.SetActive(false);
        hotBar.SetActive(false);

        GameManager.Instance.inMinigame = true;
    }

    public void EndMinigame()
    {
        if (GameManager.Instance.inMinigame)
        {
            isMovingToNew = false;
            isMovingToOld = true;

            Cursor.lockState = CursorLockMode.Locked;

            pointer.SetActive(true);
            hotBar.SetActive(true);

            CookMaterials();
            GameManager.Instance.inMinigame = false;
        }
    }

    public void CheckProgress()
    {
        if (progress >= 10)
        {
            EndMinigame();
            progress = 0;
        }
    }

    public void CookMaterials()
    {
        for (int i = 0; i < chestSlots.Count; i++)
        {
            if (chestSlots[i].itemData != null)
            {
                if (chestSlots[i].itemData.minigame == currentMinigame)
                {
                    if (chestSlots[i].itemData.isFinished == false)
                    {
                        if (chestSlots[i].itemData.nextStage != null)
                        {
                            GameObject newStage;

                            newStage = Instantiate(chestSlots[i].itemData.nextStage.prefab, objSpawn.transform.position, objSpawn.transform.rotation);
                            newStage.GetComponent<Rigidbody>().AddForce(objSpawn.transform.up * spawnForce);

                            chestSlots[i].ClearItem();
                        }
                    }

                    else
                    {
                        CookPotions();
                    }
                }
            }
        }
    }

    //Method for checking whether a given list matches this pattern (regardless of the order)
    public void CookPotions()
    {
        itemsToCheck.Clear();

        //Add items in chest to itemsToCheck list
        for (int i = 0; i < chestSlots.Count; i++)
        {
            if (chestSlots[i].itemData != null)
            {
                itemsToCheck.Add(chestSlots[i].itemData);
                chestSlots[i].ClearItem();
            }
        }

        //Check if items in itemsToCheck mathces with requiredMaterials in recipes
        for (int i = 0; i < allRecipes.Count; i++)
        {
            foreach (ItemData item in allRecipes[i].requiredMaterials)
            {
                if (itemsToCheck.Count != allRecipes[i].requiredMaterials.Count)
                {
                    Debug.Log(itemsToCheck.Count);
                    Debug.Log(allRecipes[i].requiredMaterials.Count);
                    continue;
                }

                if (itemsToCheck.Contains(item))
                {
                    sameItems++;
                    itemsToCheck.Remove(item);
                }
            }

            if (sameItems == allRecipes[i].requiredMaterials.Count)
            {
                GameObject newPotion;

                newPotion = Instantiate(allRecipes[i].potion, objSpawn.transform.position, objSpawn.transform.rotation);
                newPotion.GetComponent<Rigidbody>().AddForce(objSpawn.transform.up * spawnForce);
            }
        }

        //If we have not used items, add them again to chest
        if (itemsToCheck.Count > 0)
        {
            AddItemToShop(itemsToCheck);
        }
    }

    public void AddItemToShop(List<ItemData> itemsToCheck)
    {
        for (int i = 0; i < itemsToCheck.Count; i++)
        {
            itemsToCheck[i].shopStock++;
            itemsToCheck.Remove(itemsToCheck[i]);
        }
    }
}
