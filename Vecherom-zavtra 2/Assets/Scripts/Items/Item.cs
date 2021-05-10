using System.Collections;
using System.Collections.Generic;

public class Item
{
    public List<BaseStat> Stats { get; set; }
	public string ObjectSlug { get; set; }
	public string Description { get; set; }
	public string ActionName { get; set; }
	public string ItemName { get; set; }
	public bool ItemModifier { get; set; }

	public Item(List<BaseStat> Stats, string ObjectSlug)
	{
		this.Stats = Stats;
		this.ObjectSlug = ObjectSlug;
	}

	[Newtonsoft.Json.JsonConstructor]
	public Item(List<BaseStat> Stats, string ObjectSlug, string Description, string ActionName, string ItemName, bool ItemModifier)
	{
		this.Stats = Stats;
		this.ObjectSlug = ObjectSlug;
		this.Description = Description;
		this.ActionName = ActionName;
		this.ItemName = ItemName;
		this.ItemModifier = ItemModifier;
	}
}
