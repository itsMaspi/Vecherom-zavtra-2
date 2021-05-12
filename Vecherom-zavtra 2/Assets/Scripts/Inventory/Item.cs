using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
public class Item
{
	public enum ItemTypes { Weapon, Consumable, Quest }
    public List<BaseStat> Stats { get; set; }
	public string ObjectSlug { get; set; }
	public string Description { get; set; }
	[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
	public ItemTypes ItemType { get; set; }
	public string ActionName { get; set; }
	public string ItemName { get; set; }
	public bool ItemModifier { get; set; }

	public Item(List<BaseStat> Stats, string ObjectSlug)
	{
		this.Stats = Stats;
		this.ObjectSlug = ObjectSlug;
	}

	[Newtonsoft.Json.JsonConstructor]
	public Item(List<BaseStat> Stats, string ObjectSlug, string Description, ItemTypes ItemType, string ActionName, string ItemName, bool ItemModifier)
	{
		this.Stats = Stats;
		this.ObjectSlug = ObjectSlug;
		this.Description = Description;
		this.ItemType = ItemType;
		this.ActionName = ActionName;
		this.ItemName = ItemName;
		this.ItemModifier = ItemModifier;
	}
}
