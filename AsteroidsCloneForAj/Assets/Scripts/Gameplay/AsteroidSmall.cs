using UnityEngine;
using System.Collections;

public class AsteroidSmall : Asteroid
{
    public override void SetTrajectory(Vector2 fromPosition)
    {
        m_transform.position = fromPosition;
        m_rigidbody2D.AddForce((GetRandomScreenPosition() - new Vector2(m_transform.position.x, m_transform.position.y)) * Random.Range(13, 17));
    }

    public override void TakeDamage(int value, GameObject culprit)
    {
        if (culprit == GameManager.PlayerShip.gameObject)
            GameManager.Score += 50;

        Die();
    }
}
