using UnityEngine;
using System.Collections;

public class Asteroid : ScreenwrapObject
{
    private Rigidbody2D m_rigidbody2D;

    public override void Start()
    {
        base.Start();
        m_rigidbody2D = GetComponent<Rigidbody2D>();

        m_rigidbody2D.AddForce((GetRandomScreenPosition() - new Vector2(m_transform.position.x, m_transform.position.y)) * Random.Range(5, 15));
        m_rigidbody2D.AddTorque(Random.Range(-100, 100));
    }

    public override void Update()
    {
        base.Update();
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Hitable")
        {
            col.gameObject.GetComponent<IExplodable>().Explode();
        }
    }

    Vector2 GetRandomScreenPosition()
    {
        return m_camera.ViewportToWorldPoint(new Vector2(Random.Range(0, 100f) / 100f, Random.Range(0, 100f) / 100f));
    }
}