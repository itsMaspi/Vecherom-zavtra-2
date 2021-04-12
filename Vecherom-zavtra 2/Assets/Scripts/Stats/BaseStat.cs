using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseStat
{
	public List<StatBonus> BaseAdditives { get; set; }
	public int BaseValue { get; set; }
	public string StatName { get; set; }
	public string StatDescription { get; set; }
	public int FinalValue { get; set; }

	public BaseStat(int BaseValue, string StatName, string StatDescription)
	{
		this.BaseAdditives = new List<StatBonus>();
		this.BaseValue = BaseValue;
		this.StatName = StatName;
		this.StatDescription = StatDescription;
	}

	public void AddStatBonus(StatBonus statBonus)
	{
		BaseAdditives.Add(statBonus);
	}

	public void RemoveStatBonus(StatBonus statBonus)
	{
		BaseAdditives.Remove(BaseAdditives.Find(x => x.BonusValue == statBonus.BonusValue));
	}

	public int GetCalculatedStatValue()
	{
		FinalValue = BaseValue + BaseAdditives.Sum(x => x.BonusValue);
		return FinalValue;
	}

	public string GetFullValue()
	{
		string value = $"{GetCalculatedStatValue()}";
		if (BaseAdditives.Sum(x => x.BonusValue) == 0)
			return value;
		if (BaseAdditives.Sum(x => x.BonusValue) > 0)
		{
			return value + $" (+{BaseAdditives.Sum(x => x.BonusValue)})";
		}
		return value + $" ({BaseAdditives.Sum(x => x.BonusValue)})";
	}
}
