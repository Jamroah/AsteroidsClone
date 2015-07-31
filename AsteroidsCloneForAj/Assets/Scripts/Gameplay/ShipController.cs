using UnityEngine;
using System.Collections;

public class ShipController : ScreenwrapObject
{
    public GameObject bullet;
    public GameObject gun;

    public float maxSpeed;
    public float acceleration;
    [Range(5, 10)]
    public float rotSpeed;
    public bool canControl;

    public ParticleSystem deathParticles;

    private float velocity;

    private Rigidbody2D m_rigidbody2D;
    private Collider2D m_collider2D;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_anim;

    private static GameObjectPool m_bulletPool;

    public override void Start()
    {
        base.Start();

        if (bullet != null && m_bulletPool == null)
            m_bulletPool = new GameObjectPool(bullet, 5, false);
    }

    public void Initialise()
    {
        GameManager.PlayerShip = this;

        canControl = true;

        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_collider2D = GetComponent<Collider2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_anim = GetComponent<Animator>();
        Stopping();
    }

    public void Fire()
    {
        // Activate a bullet immediately (the "true" part) and hold a reference to it
        GameObject bullet = m_bulletPool.Get(true);

        if (bullet == null)
            return;

        // Spawn it at the gun's position.
        bullet.transform.position = gun.transform.position;

        // Simply get the direction vector between the center of the ship and the gun. 
        // Luckily it's pointing in the direction we want.
        // If there were multiple guns you'd have to define the center of each gun to get the right direction vectors.
        bullet.GetComponent<Bullet>().Fire(gun.transform.position - m_transform.position, gameObject);
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

    public override void Explode()
    {
        GameManager.TakeDamage(1);
        canControl = false;
        deathParticles.Play();
        m_rigidbody2D.velocity = Vector2.zero;
        m_spriteRenderer.enabled = false;
        m_collider2D.enabled = false;
    }

    //void Reset()
    //{
    //    canControl = true;
    //    m_transform.position = Vector2.zero;
    //    m_transform.rotation = Quaternion.identity;
    //    Stopping();
    //    StartCoroutine(Invincibility());
    //}

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

    public void ResetToCenter()
    {
        m_transform.position = Vector2.zero;
        m_transform.rotation = Quaternion.identity;
    }
}
