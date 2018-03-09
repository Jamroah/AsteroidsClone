// Copyright (C) 2015 Ben Beagley //

using UnityEngine;
using System.Collections;

public class Asteroid : ScreenwrapObject, IDamageable
{
    public Sprite[] sprites;

    protected Rigidbody2D m_rigidbody2D;
    protected int m_currentHealth;

    public virtual int MaxHealth
    {
        get { return 1; }
        set { }
    }

    public virtual int CurrentHealth
    {
        get { return m_currentHealth; }
        set { m_currentHealth = value; }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        m_rigidbody2D = GetComponent<Rigidbody2D>(); 
        m_rigidbody2D.AddTorque(Random.Range(-100, 100));
        CurrentHealth = MaxHealth;

        if(sprites.Length > 0)
            GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }

    public override void Update()
    {
        base.Update();
    }

    public virtual void Die()
    {
        // Here will be a particle system;
        AudioManager.PlaySFX("Enemy Explosion", 0.5f, 0.75f);
        gameObject.SetActive(false);
    }

    public virtual void TakeDamage(int value, GameObject culprit)
    {
        Die();
    }

    public virtual void SetTrajectory(Vector2 fromPosition)
    {

    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Hitable")
        {
            col.gameObject.GetComponent<IDamageable>().TakeDamage(1, gameObject);
            //Die();
        }
    }

    protected Vector2 GetRandomScreenPosition()
    {
        return MainCamera.ViewportToWorldPoint(new Vector2(Random.Range(0, 100f) / 100f, Random.Range(0, 100f) / 100f));
    }
}