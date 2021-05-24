using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Mirror;
using System.Linq;

public class ItemDatabase : NetworkBehaviour
{
    private List<Item> Items { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        BuildDatabase();
    }

    private void BuildDatabase()
    {
        Items = JsonConvert.DeserializeObject<List<Item>>(Resources.Load<TextAsset>("JSON/Items").ToString());
        //Debug.Log(Items[2].ItemName + "'s " + Items[2].Stats[0].StatName + " is " + Items[2].Stats[0].GetCalculatedStatValue());
    }

    public Item GetItem(string itemSlug)
    {
        Debug.Log($"Item slug: {itemSlug}");
        var item = Items.Where(x => x.ObjectSlug == itemSlug).FirstOrDefault();
        /*foreach(Item item in Items)
        {
            if (item.ObjectSlug == itemSlug)
            {
                return item;
            }
        }*/
        if (item == null)
        {
            Debug.LogWarning("Couldn't find the item " + itemSlug);
        }
        return item;
    }
}
