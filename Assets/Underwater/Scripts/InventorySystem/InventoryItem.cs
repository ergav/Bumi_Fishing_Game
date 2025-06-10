using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemSO itemSO;

    [SerializeField] Image iconImage;

    void Update()
    {
        iconImage.sprite = itemSO.icon;
    }
}
