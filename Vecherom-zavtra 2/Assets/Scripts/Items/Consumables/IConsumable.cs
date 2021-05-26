using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConsumable 
{
    void Consume(GameObject gameObject);
    void Consume(CharacterStats stats);
}
