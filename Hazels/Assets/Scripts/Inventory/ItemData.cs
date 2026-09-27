using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public int id = 0;
    public string itemName = "";
    public Sprite icon;
    public int maxStock = 1;
}
