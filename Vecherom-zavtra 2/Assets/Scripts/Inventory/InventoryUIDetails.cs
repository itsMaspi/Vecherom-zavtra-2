using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIDetails : MonoBehaviour
{
    Item item;
    Button selectedItemButton, itemInteractButton;
    TMPro.TextMeshProUGUI itemNameText, itemDescriptionText, itemInteractButtonText;

    void Start()
	{
        itemNameText = transform.Find("Item_Name").GetComponent<TMPro.TextMeshProUGUI>();
        itemDescriptionText = transform.Find("Item_Description").GetComponent<TMPro.TextMeshProUGUI>();
        itemInteractButton = transform.Find("Interact_Button").GetComponent<Button>();
        itemInteractButtonText = itemInteractButton.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void SetItem(Item item, Button selectedButton) 
    {
        this.item = item;
        selectedItemButton = selectedButton;
        itemNameText.text = item.ItemName;
        itemDescriptionText.text = item.Description;
        itemInteractButtonText.text = item.ActionName;
        itemInteractButton.onClick.AddListener(OnItemInteract);
    }

    public void OnItemInteract()
	{
        if (item.ItemType == Item.ItemTypes.Consumable)
		{
            InventoryController.Instance.ConsumeItem(item);
            Destroy(selectedItemButton.gameObject);
        }
        else if (item.ItemType == Item.ItemTypes.Weapon)
		{
            InventoryController.Instance.EquipItem(item);
            Destroy(selectedItemButton.gameObject);
        }
	}
}
