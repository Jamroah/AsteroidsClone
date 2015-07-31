using UnityEngine;
using System.Collections;

public class AsteroidBig : Asteroid
{
    public override void Explode()
    {
        base.Explode();

        Debug.Log("Big Asteroid Exploded");
    }
}