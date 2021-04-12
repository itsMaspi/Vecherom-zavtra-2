using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public List<BaseStat> stats = new List<BaseStat>();

    // Start is called before the first frame update
    void Start()
    {
        // this may go to the inspector, so we can add custom stats for each entity
        stats.Add(new BaseStat(50, "Health", "The health points"));
        stats.Add(new BaseStat(15, "Attack", "The attack power"));
        stats.Add(new BaseStat(10, "Defense", "The resistance against attacks"));
        stats[0].AddStatBonus(new StatBonus(3));
        stats[2].AddStatBonus(new StatBonus(-5));

        stats.ForEach(x => Debug.Log($"{x.StatName}: {x.GetFullValue()}"));
    }

    public void AddStatBonus(List<BaseStat> statBonuses)
	{
		foreach (BaseStat statBonus in statBonuses)
		{
            stats.Find(x => x.StatName == statBonus.StatName).AddStatBonus(new StatBonus(statBonus.BaseValue));
		}
	}

    public void RemoveStatBonus(List<BaseStat> statBonuses)
    {
        foreach (BaseStat statBonus in statBonuses)
        {
            stats.Find(x => x.StatName.Equals(statBonus.StatName)).RemoveStatBonus(new StatBonus(statBonus.BaseValue));
        }
    }
}
