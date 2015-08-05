using UnityEngine;
using System.Collections;

public class Bullet : ScreenwrapObject
{
    private GameObject m_owner;
    private Rigidbody2D m_rigidbody2D;

    public override void OnEnable()
    {
        base.OnEnable();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        Invoke("DisableBullet", 1f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == m_owner)
            return;

        if (col.tag == "Hitable" || col.tag == "Asteroid")
        {
            DisableBullet();
            col.gameObject.GetComponent<IDamageable>().TakeDamage(1, m_owner);
        }
    }

    /// <summary>
    /// Fires a bullet with a defined direction.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="degrees"></param>
    public void Fire(GameObject owner, Vector2 origin, float degrees, float force)
    {
        m_owner = owner;
        m_transform.position = origin;
        m_rigidbody2D.AddForce(MathV2D.OnEdgeOfCircle(degrees, force), ForceMode2D.Impulse);
        AudioManager.PlaySFX("Bullet", 0.8f, 1.1f);
    }

    private void DisableBullet()
    {
        CancelInvoke("DisableBullet");
        gameObject.SetActive(false);
    }
}
