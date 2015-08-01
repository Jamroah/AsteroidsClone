using UnityEngine;
using System.Collections;

public class AsteroidSmall : Asteroid
{
    public override void SetTrajectory(Vector2 fromPosition)
    {
        m_transform.position = fromPosition;
        m_rigidbody2D.AddForce((GetRandomScreenPosition() - new Vector2(m_transform.position.x, m_transform.position.y)) * Random.Range(13, 17));
    }

    public override void Explode()
    {
        base.Explode();
        //Messenger<uint>.Broadcast("Update Score", 50, MessengerMode.DONT_REQUIRE_LISTENER);
        GameManager.Score += 50;
        gameObject.SetActive(false);    
    }
}
