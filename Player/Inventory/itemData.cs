using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int itemID;
    public Sprite itemIcon;
}
