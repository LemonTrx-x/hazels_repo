using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Image ghostIcon;

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

    public void CloseUI()
    {
        AudioManager.Instance.PlaySFX(1); //BtnClicked

        InventoryManager.Instance.HandleInventoryUI();
    }
}
