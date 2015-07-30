using UnityEngine;
using System.Collections;

public class ShipController : ScreenwrapObject
{
    public GameObject bullet;
    public GameObject gun;

    public float maxSpeed;
    public float acceleration;
    //public float linearDrag;
    [Range(5, 10)]
    public float rotSpeed;

    private float velocity;

    private Rigidbody2D m_rigidbody2D;
    private GameObjectPool m_bulletPool;
    private Animator m_anim;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();

        if(bullet != null)
            m_bulletPool = new GameObjectPool(bullet, 5, false);
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
        bullet.GetComponent<Bullet>().SetDirection(gun.transform.position - m_transform.position);
    }

    public void Accelerate()
    {
        //m_transform.Translate((gun.transform.position - m_transform.position));
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
}
