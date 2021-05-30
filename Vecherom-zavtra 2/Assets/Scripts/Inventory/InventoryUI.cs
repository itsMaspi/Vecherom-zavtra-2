using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public RectTransform inventoryPanel;
    public RectTransform scrollViewContent;
    InventoryUIItem itemContainer { get; set; }
    bool menuIsActive { get; set; }
    Item currentSelectedItem { get; set; }


    void Awake()
    {
        itemContainer = Resources.Load<InventoryUIItem>("UI/Item_Container");
        UIEventHandler.OnItemAddedToInventory += ItemAdded;
    }
    // Start is called before the first frame update
    void Start()
    {
        inventoryPanel.gameObject.SetActive(false);
    }

    public void OnToggleInventory()
	{
        if (PauseManager.pauseState == PauseState.Paused) return;
        menuIsActive = !menuIsActive;
        inventoryPanel.gameObject.SetActive(menuIsActive);
	}

    public void ItemAdded(Item item)
	{
        InventoryUIItem emptyItem = Instantiate(itemContainer);
        emptyItem.SetItem(item);
        emptyItem.transform.SetParent(scrollViewContent);
	}
}
