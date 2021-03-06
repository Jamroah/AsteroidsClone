﻿// Copyright (C) 2015 Ben Beagley //

using UnityEngine;
using System.Collections;

public class ShipController : ScreenwrapObject, IDamageable
{
    public GameObject bullet;
    public GameObject gun;

    public float acceleration;
    public float rotSpeed;
    public bool canControl;

    public ParticleSystem deathParticles;

    private Rigidbody2D m_rigidbody2D;
    private Collider2D m_collider2D;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_anim;

    private static GameObjectPool m_bulletPool;

    public int MaxHealth
    {
        get { return GameManager.MaxLives; }
        set { }
    }

    public int CurrentHealth
    {
        get { return GameManager.CurrentLives; }
        set { GameManager.CurrentLives = value; }
    }

    public void Start()
    {
        if (bullet != null && m_bulletPool == null)
            m_bulletPool = new GameObjectPool(bullet, 5, false); // One more bullet on screen allowed than original Asteroids to account for wide screen.
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Hitable" || col.tag == "Asteroid")
        {
            col.gameObject.GetComponent<IDamageable>().TakeDamage(1, null);
        }
    }

    public void Initialise()
    {
        GameManager.PlayerShip = this;

        canControl = true;
        Debug.Log("Ship Initialised");

        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_collider2D = GetComponent<Collider2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_anim = GetComponent<Animator>();
        Stopping();
    }

    public void Fire()
    {
        // Activate a bullet immediately (the "true" part) but hold a reference to it because we need to check if it's null before we go any further.
        // Because we limit the amount of player bullets on screen it's possible for the Get() function to return null.
        GameObject bullet = m_bulletPool.Get(true);

        if (bullet == null)
            return;

        bullet.GetComponent<Bullet>().Fire(gameObject, gun.transform.position, m_transform.rotation.eulerAngles.z + 90, 15);
    }

    public void Accelerate()
    {
        m_rigidbody2D.AddForce(m_transform.up * acceleration);
        m_anim.SetBool("Accelerating", true);
    }

    public void Stopping()
    {
        m_anim.SetBool("Accelerating", false);
    }

    public void Rotate(float direction)
    {
        m_transform.Rotate((Vector3.forward * direction) * rotSpeed);
    }

    public void TakeDamage(int value, GameObject culprit)
    {
        if (culprit == gameObject)
        {
            Debug.Log("Stop hitting yourself");
            return;
        }

        CurrentHealth -= value;
        Die();
        Messenger.Broadcast("Update UI");
    }

    public void Die()
    {
        canControl = false;
        deathParticles.Play();
        m_rigidbody2D.velocity = Vector2.zero;
        m_spriteRenderer.enabled = false;
        m_collider2D.enabled = false;
        AudioManager.PlaySFX("Ship Explosion");
    }

    // Kinda inelegant, but it works so...
    public IEnumerator Invincibility()
    {
        m_collider2D.enabled = false;

        float seconds = 0;

        while (seconds <= 2)
        {
            m_spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.05f);
            m_spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.05f);
            seconds += 0.1f;
        }

        m_spriteRenderer.enabled = true;

        m_collider2D.enabled = true;
    }
}
