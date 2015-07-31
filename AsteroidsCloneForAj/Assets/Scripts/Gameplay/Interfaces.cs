using UnityEngine;
using System.Collections;

public interface IExplodable
{
    void Explode();
}

// Weirdly named I guess but works for the boss and player so whatevs
public interface IDamageable<T>
{
    T MaxHealth {get; set;}
    T CurrentHealth { get; set; }
    void Damage(T amount);
}
