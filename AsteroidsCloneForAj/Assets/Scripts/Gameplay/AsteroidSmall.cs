using UnityEngine;
using System.Collections;

public class AsteroidSmall : Asteroid
{
    public override void Explode()
    {
        base.Explode();

        Debug.Log("Small Asteroid Exploded");
    }
}
