using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItem : MonoBehaviour
{
    public Item item;

	[HideInInspector] public TMPro.TextMeshProUGUI itemText;
	[HideInInspector] public Image itemImage;

	void Awake()
	{
		itemText = transform.Find("Item_Name").GetComponent<TMPro.TextMeshProUGUI>();
		itemImage = transform.Find("Item_Icon").GetComponent<Image>();
	}

    public void SetItem(Item item)
	{
		this.item = item;
		SetupItemValues();
	}

	void SetupItemValues()
	{
		itemText.text = item.ItemName;
		itemImage.sprite = Resources.Load<Sprite>($"UI/Icons/Items/{item.ObjectSlug}");
	}

	public void OnSelectItemButton()
	{
		InventoryController.Instance.SetItemDetails(item, GetComponent<Button>());
	}
}
