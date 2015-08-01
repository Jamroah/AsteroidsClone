using UnityEngine;
using System.Collections;

public class AsteroidBig : Asteroid
{
    public override void SetTrajectory(Vector2 fromPosition)
    {
        m_transform.position = fromPosition;
        m_rigidbody2D.AddForce((GetRandomScreenPosition() - new Vector2(m_transform.position.x, m_transform.position.y)) * Random.Range(5, 7));
    }

    public override void Explode()
    {
        base.Explode();

        GameManager.SmallAsteroidPool.Get(true).GetComponent<Asteroid>().SetTrajectory(m_transform.position);
        GameManager.SmallAsteroidPool.Get(true).GetComponent<Asteroid>().SetTrajectory(m_transform.position);

        //Messenger<uint>.Broadcast("Update Score", 20, MessengerMode.DONT_REQUIRE_LISTENER);
        GameManager.Score += 20;
        gameObject.SetActive(false);
    }
}