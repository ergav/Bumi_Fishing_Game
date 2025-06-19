using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryParent;
    [SerializeField] private GameObject slotPrefab;

    public GameObject iconPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;


    public bool isInventoryOpened;

    public static InventoryManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        isInventoryOpened = false;

        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryParent.transform).GetComponent<Slot>();

            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
    }

    private void Update()
    {
        inventoryParent.SetActive(isInventoryOpened);

    }

    public void ToggleInventory()
    {
        isInventoryOpened = !isInventoryOpened;
    }
}
