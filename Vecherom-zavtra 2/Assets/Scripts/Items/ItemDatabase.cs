using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; set; }
    private List<Item> Items { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }
        BuildDatabase();
    }

    private void BuildDatabase()
    {
        Items = JsonConvert.DeserializeObject<List<Item>>(Resources.Load<TextAsset>("JSON/Items").ToString());
        Debug.Log(Items[2].ItemName + "'s " + Items[2].Stats[0].StatName + " is " + Items[2].Stats[0].GetCalculatedStatValue());
    }

    public Item GetItem(string itemSlug)
    {
        foreach(Item item in Items)
        {
            if (item.ObjectSlug == itemSlug)
            {
                return item;
            }
        }
        Debug.LogWarning("Couldn't find the item " + itemSlug);
        return null;
    }
}
