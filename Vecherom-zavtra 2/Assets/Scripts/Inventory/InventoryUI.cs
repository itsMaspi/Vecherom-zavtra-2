using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public void OnToggleInventory(InputValue value)
	{
        if (PauseManager.pauseState == PauseState.Paused) return;
        if (value.isPressed)
        {
            menuIsActive = !menuIsActive;
            inventoryPanel.gameObject.SetActive(menuIsActive);
        }
	}

    public void ItemAdded(Item item)
	{
        Debug.Log($"InventoryUI.cs: ItemAdded({item.ObjectSlug})");
        InventoryUIItem emptyItem = Instantiate(itemContainer);
        emptyItem.SetItem(item);
        emptyItem.transform.SetParent(scrollViewContent);
	}
}
