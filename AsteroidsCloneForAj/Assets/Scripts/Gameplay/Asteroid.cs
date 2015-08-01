using UnityEngine;
using System.Collections;

public class Asteroid : ScreenwrapObject
{
    public Sprite[] sprites;

    protected Rigidbody2D m_rigidbody2D;

    public virtual void Start()
    {

    }

    public override void OnEnable()
    {
        base.OnEnable();
        m_rigidbody2D = GetComponent<Rigidbody2D>(); 
        m_rigidbody2D.AddTorque(Random.Range(-100, 100));

        if(sprites.Length > 0)
            GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }

    public override void Update()
    {
        base.Update();
    }

    public virtual void SetTrajectory(Vector2 fromPosition)
    {

    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Hitable")
        {
            col.gameObject.GetComponent<IExplodable>().Explode();
        }
    }

    protected Vector2 GetRandomScreenPosition()
    {
        return MainCamera.ViewportToWorldPoint(new Vector2(Random.Range(0, 100f) / 100f, Random.Range(0, 100f) / 100f));
    }
}