// Copyright (C) 2015 Ben Beagley //

using UnityEngine;
using System.Collections;

public class AsteroidBig : Asteroid
{
    public override void SetTrajectory(Vector2 fromPosition)
    {
        m_transform.position = fromPosition;
        m_rigidbody2D.AddForce((GetRandomScreenPosition() - new Vector2(m_transform.position.x, m_transform.position.y)) * Random.Range(5, 7));
    }

    public override void Die()
    {
        GameManager.SmallAsteroidPool.Get(true).GetComponent<Asteroid>().SetTrajectory(m_transform.position);
        GameManager.SmallAsteroidPool.Get(true).GetComponent<Asteroid>().SetTrajectory(m_transform.position);

        base.Die();
    }

    public override void TakeDamage(int value, GameObject culprit)
    {
        if(culprit != null && culprit == GameManager.PlayerShip.gameObject)
            GameManager.Score += 20;

        Die();
    }
}