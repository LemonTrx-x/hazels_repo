using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigames : MonoBehaviour
{
    public static Minigames Instance { get; private set; }

    [Header("Player")]
    public GameObject player;
    
    [Header("Player UI")]
    public GameObject playerPanel;
    public GameObject hotBar;
    public GameObject toDoList0;
    public GameObject toDoList1;
    public GameObject toDoList2;

    [Header("Camera")]
    public GameObject cameraPlayer;
    public GameObject newCameraParent;
    public GameObject oldCameraParent;
    public float camSpeed = 5f;

    private bool isMovingToNew = false;
    private bool isMovingToOld = false;

    [Header("Chest")]
    public GameObject objSpawn;
    public int spawnForce = 50;
    [HideInInspector] public List<Slot> chestSlots = new List<Slot>();
    public List<RecipeData> allRecipes;
    public int sameItems = 0;
    private List<ItemData> itemsToCheck = new List<ItemData>();

    [Header("Minigames")]
    public GameObject progressSliderObj;
    [HideInInspector] public Slider progressSlider;
    public int progress = 0;
    public int maxProgress;
    public int currentMinigame = 0;
    public bool potionMade = false;

    void Awake()
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
        progressSliderObj.SetActive(false);
        progressSlider = progressSliderObj.GetComponent<Slider>();
        
        progressSlider.value = 0;
        progressSlider.maxValue = 10;
    }

    void Update()
    {
        MoveCamera();
        CheckProgress();
        HandlePlayerUI();
        HandlePlayerInMinigame();
    }

    public void MoveCamera()
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
    }
    public void HandlePlayerUI()
    {
        if (GameManager.Instance.inMinigame || GameManager.Instance.inPC || GameManager.Instance.inMenu)
        {
            playerPanel.SetActive(false);
            toDoList0.SetActive(false);
            toDoList1.SetActive(false);
            toDoList2.SetActive(false);

            if (!GameManager.Instance.inMenu) hotBar.SetActive(false);
        }
        
        else
        {
            playerPanel.SetActive(true);
            hotBar.SetActive(true);
            toDoList0.SetActive(true);
            toDoList1.SetActive(true);
            toDoList2.SetActive(true);
        }
    }

    public void HandlePlayerInMinigame()
    {
        if (GameManager.Instance.inMinigame)
        {
            player.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }

        else
        {
            player.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }

        List<ShowHandObj> handList = new List<ShowHandObj>();
        handList.AddRange(hotBar.GetComponentsInChildren<ShowHandObj>());

        for (int i = 0; i < handList.Count; i++)
        {
            if (handList[i].handObj != null)
            {
                if (GameManager.Instance.inMinigame)
                {
                    handList[i].handObj.GetComponentInChildren<MeshRenderer>().enabled = false;
                }

                else
                {
                    handList[i].handObj.GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }
        }
    }

    public void StartMiniGame()
    {
        oldCameraParent.transform.rotation = cameraPlayer.transform.rotation;
        potionMade = false;

        isMovingToNew = true;
        isMovingToOld = false;

        if (!GameManager.Instance.inPC)
        {
            progressSliderObj.SetActive(true);
            GameManager.Instance.inMinigame = true;
        }

        ToDoList.Instance.stepGuide.SetActive(false);
    }

    public void EndMinigame()
    {
        if (GameManager.Instance.inMinigame)
        {
            isMovingToNew = false;
            isMovingToOld = true;

            progressSliderObj.SetActive(false);

            CookMaterials();
            GameManager.Instance.inMinigame = false;

            ToDoList.Instance.stepGuide.SetActive(true);
        }
    }

    public void CheckProgress()
    {
        if (GameManager.Instance.inMinigame)
        {
            progressSlider.value = progress;

            if (progress >= maxProgress)
            {
                potionMade = true;
                EndMinigame();
                progress = 0;
            }
        }
    }

    public void CookMaterials()
    {
        for (int i = 0; i < chestSlots.Count; i++)
        {
            if (chestSlots[i].itemData == null) return;

            if (chestSlots[i].itemData.minigame != currentMinigame) return;

            if (chestSlots[i].itemData.isFinished == false)
            {
                if (chestSlots[i].itemData.nextStage != null)
                {
                    GameObject newStage;

                    newStage = Instantiate(chestSlots[i].itemData.nextStage.prefab, objSpawn.transform.position, objSpawn.transform.rotation);
                    newStage.GetComponent<Rigidbody>().AddForce(objSpawn.transform.up * spawnForce);

                    AudioManager.Instance.PlaySFX(7); //PUFF

                    chestSlots[i].ClearItem();
                }
            }

            else
            {
                CookPotions();
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

        bool recipeFound = false;

        //Check all the recipes
        for (int i = 0; i < allRecipes.Count; i++)
        {
            //Temporal copy to evaluate this recipe without destroying the original
            List<ItemData> tempItemsToCheck = new List<ItemData>(itemsToCheck);

            //Verufy if the quantity of ingridients matches
            if (tempItemsToCheck.Count == allRecipes[i].requiredMaterials.Count)
            {
                int matches = 0;

                foreach (ItemData item in allRecipes[i].requiredMaterials)
                {
                    if (tempItemsToCheck.Contains(item))
                    {
                        matches++;
                        tempItemsToCheck.Remove(item); //Remove from temporal copy
                    }
                }

                //If all ingredients match the recipe
                if (matches == allRecipes[i].requiredMaterials.Count)
                {
                    //Create potion
                    GameObject newPotion = Instantiate(allRecipes[i].potion.prefab, objSpawn.transform.position, objSpawn.transform.rotation);
                    newPotion.GetComponent<Rigidbody>().AddForce(objSpawn.transform.up * spawnForce);

                    AudioManager.Instance.PlaySFX(7); //PUFF

                    recipeFound = true;
                    break; //End search because we found the correct recipe
                }
            }
        }

        //If no recipe matches, items go to store
        if (!recipeFound && itemsToCheck.Count > 0)
        {
            AddItemToShop(itemsToCheck);
        }
    }

    public void AddItemToShop(List<ItemData> itemsToCheck)
    {
        foreach (ItemData item in itemsToCheck)
        {
            item.shopStock++;
        }

        itemsToCheck.Clear();
    }
}
