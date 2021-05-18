using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIDetails : MonoBehaviour
{
    Item item;
    Button selectedItemButton, itemInteractButton;
    TMPro.TextMeshProUGUI itemNameText, itemDescriptionText, itemInteractButtonText, statText;

    void Start()
	{
        itemNameText = transform.Find("Item_Name").GetComponent<TMPro.TextMeshProUGUI>();
        itemDescriptionText = transform.Find("Item_Description").GetComponent<TMPro.TextMeshProUGUI>();
        itemInteractButton = transform.Find("Interact_Button").GetComponent<Button>();
        itemInteractButtonText = itemInteractButton.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        statText = transform.Find("Stats_List").GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    public void SetItem(Item item, Button selectedButton) 
    {
        gameObject.SetActive(true);
        statText.text = "";
        if (item.Stats != null)
		{
			foreach (BaseStat stat in item.Stats)
			{
                statText.text += $"{stat.StatName}: {stat.BaseValue}\n";
			}
		}
        itemInteractButton.onClick.RemoveAllListeners();
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
            //transform.parent.parent.parent.parent.GetComponent<InventoryController>().ConsumeItem(item);
            transform.GetComponentInParent<InventoryController>().ConsumeItem(item);
            Destroy(selectedItemButton.gameObject);
        }
        else if (item.ItemType == Item.ItemTypes.Weapon)
		{
            transform.GetComponentInParent<InventoryController>().EquipItem(item);
            Destroy(selectedItemButton.gameObject);
        }
        item = null;
        gameObject.SetActive(false);
	}
}
