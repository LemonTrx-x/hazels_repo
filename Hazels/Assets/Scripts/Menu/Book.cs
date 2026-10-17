using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    /*
    public Transform entryContainer;
    public Transform entryTemplate;

    public List<Transform> bookEntryTransformList;

    public TextMeshProUGUI potionID;
    public TextMeshProUGUI potionName;
    public Sprite potionSprite; 

    void Awake()
    {
        entryContainer = transform.Find("BookPanel");
        entryContainer = entryContainer.Find("RecipeTemplateBtn");

        entryTemplate.gameObject.SetActive(false);

        StartBook();
    }

    public void StartBook()
    {
        Debug.Log("BOOK");

        string jsonString = PlayerPrefs.GetString("recipeTable");
        Recipes recipes = JsonUtility.FromJson<Recipes>(jsonString);

        if (recipes == null || recipes.recipeEntryList == null)
        {
            recipes = new Recipes { recipeEntryList = new List<RecipeEntry>() };
        }

        for (int i = 0; i < recipes.recipeEntryList.Count; i++)
        {
            for (int j = i + 1; j < recipes.recipeEntryList.Count; j++)
            {
                if (recipes.recipeEntryList[j].id > recipes.recipeEntryList[i].id)
                {
                    RecipeEntry tmp = recipes.recipeEntryList[i];
                    recipes.recipeEntryList[i] = recipes.recipeEntryList[j];
                    recipes.recipeEntryList[j] = tmp;
                }
            }
        }

        bookEntryTransformList = new List<Transform>();
        foreach (RecipeEntry highscoreEntry in recipes.recipeEntryList)
        {
            CreateRecipeEntryTransform(highscoreEntry, entryContainer, bookEntryTransformList);
        }
    }

    private void CreateRecipeEntryTransform(RecipeEntry recipeEntry, Transform container, List<Transform> transformList)
    {
        if (transformList.Count <= 9)
        {
            float templateHeight = 60f;
            Transform entryTransform = Instantiate(entryTemplate, container);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
            entryTransform.gameObject.SetActive(true);

            int rank = transformList.Count + 1;

            entryTransform.Find("PosText").GetComponent<TextMeshProUGUI>().text = potionID;

            int score = recipeEntry.id;
            entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();

            string name = recipeEntry.name;
            entryTransform.Find("NameText").GetComponent<TextMeshProUGUI>().text = name;

            entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);

            transformList.Add(entryTransform);
        }
    }

    private class Recipes
    {
        public List<RecipeEntry> recipeEntryList = new List<RecipeEntry>();
    }

    [System.Serializable]
    private class RecipeEntry
    {
        public int id;
        public string name;
    }
    */

    //Still needs work. A lot.
}
