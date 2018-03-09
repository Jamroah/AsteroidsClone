// Copyright (C) 2015 Ben Beagley //

using UnityEngine;
using System.Collections;

public interface IDamageable
{
    int MaxHealth { get; set; }
    int CurrentHealth { get; set; }
    void TakeDamage(int value, GameObject culprit);
    void Die();
}
