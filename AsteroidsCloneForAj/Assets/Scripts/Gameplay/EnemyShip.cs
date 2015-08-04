using UnityEngine;
using System.Collections;

public class EnemyShip : ScreenwrapObject, IDamageable
{
    private Vector2 m_direction;
    RaycastHit2D hit;

    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

    public override void OnEnable()
    {
        base.OnEnable();
        Invoke("Die", 10);
        Invoke("Fire", 1);
    }

    public void OnDisable()
    {
        CancelInvoke();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        // Very, very simple "AI" that allows the enemy ship to loosely avoid asteroids.
        // The ships do a similar thing in the original.
        Debug.DrawLine(m_transform.position + new Vector3(0, -1.5f, 0), m_transform.position + new Vector3(0, 1.5f, 0), Color.yellow);

        hit = Physics2D.Linecast(m_transform.position + new Vector3(0, -2f, 0), m_transform.position + new Vector3(0, 2f, 0), 1 << LayerMask.NameToLayer("Asteroids"));

        m_direction.y = 0;

        if (hit)
        {
            //Debug.Log("Ship is avoiding a " + hit.collider.name + " on layer " + LayerMask.NameToLayer("Asteroids"));
            if (hit.point.y > m_transform.position.y)
                m_direction.y = -1f;
            if (hit.point.y < m_transform.position.y)
                m_direction.y = 1f;
        }

        m_transform.Translate(m_direction * 0.075f); 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Hitable" || col.tag == "Asteroid")
        {           
            col.gameObject.GetComponent<IDamageable>().TakeDamage(1, gameObject);
            //Debug.Log("Enemy hit buy something other than a bullet");
            //Die();
        }
    }

    public void TakeDamage(int value, GameObject culprit)
    {
        if(culprit == GameManager.PlayerShip.gameObject)
            GameManager.Score += 200;

        if (culprit == gameObject)
            return;

        Die();
    }

    public void Die()
    {
        CancelInvoke();
        gameObject.SetActive(false);
    }

    public void SetSpawnPoint(Vector2 point)
    {
        m_transform.position = point;
        m_direction = new Vector2(point.x > 0.5f ? -1 : 1, 0);
    }

    void Fire()
    {
        GameObject bullet = GameManager.EnemyBulletPool.Get(true);

        if (bullet == null)
            return;

        bullet.transform.position = m_transform.position;
        bullet.GetComponent<Bullet>().Fire(gameObject, m_transform.position, Random.Range(0f, 360f), 7);
        Invoke("Fire", Random.Range(0.5f, 1.5f));
    }
}
