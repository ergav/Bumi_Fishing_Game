using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryParent;
    [SerializeField] private GameObject _slotPrefab;

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
            Slot slot = Instantiate(_slotPrefab, _inventoryParent.transform).GetComponent<Slot>();

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
        _inventoryParent.SetActive(isInventoryOpened);

    }

    public void ToggleInventory()
    {
        isInventoryOpened = !isInventoryOpened;
    }
}
