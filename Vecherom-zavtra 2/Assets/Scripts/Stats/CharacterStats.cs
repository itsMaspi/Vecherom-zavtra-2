using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    public List<BaseStat> stats = new List<BaseStat>();

    public CharacterStats(int damage, int attackSpeed, int critic, int range, int speed, int spread)
	{
        stats = new List<BaseStat>() { 
            new BaseStat(BaseStat.BaseStatType.Damage, damage, "Damage"),
            new BaseStat(BaseStat.BaseStatType.AttackSpeed, attackSpeed, "Fire rate"),
            new BaseStat(BaseStat.BaseStatType.Critic, critic, "Critic chance"),
            new BaseStat(BaseStat.BaseStatType.Range, range, "Range"),
            new BaseStat(BaseStat.BaseStatType.Speed, speed, "Speed"),
            new BaseStat(BaseStat.BaseStatType.Spread, spread, "Spread")
        };
	}

    public BaseStat GetStat(BaseStat.BaseStatType stat)
	{
        return this.stats.Find(x => x.StatType == stat);
	}

    public void AddStatBonus(List<BaseStat> statBonuses)
	{
		foreach (BaseStat statBonus in statBonuses)
		{
            GetStat(statBonus.StatType).AddStatBonus(new StatBonus(statBonus.BaseValue));
		}
	}

    public void RemoveStatBonus(List<BaseStat> statBonuses)
    {
        foreach (BaseStat statBonus in statBonuses)
        {
            GetStat(statBonus.StatType).RemoveStatBonus(new StatBonus(statBonus.BaseValue));
        }
    }
}
