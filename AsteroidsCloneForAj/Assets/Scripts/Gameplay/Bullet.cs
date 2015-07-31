using UnityEngine;
using System.Collections;

public class Bullet : ScreenwrapObject
{
    private GameObject m_owner;

    private Vector2 m_direction = new Vector2();

    // Use this for initialization
    public override void OnEnable()
    {
        base.OnEnable();
        Invoke("DisableBullet", 1);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        m_transform.Translate(m_direction * 0.75f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == m_owner)
            return;

        if(col.tag == "Hitable")
        {
            DisableBullet();
            col.gameObject.GetComponent<IExplodable>().Explode();
        }
    }

    public void Fire(Vector2 direction, GameObject owner)
    {
        m_direction = direction;
        m_owner = owner;
    }

    private void DisableBullet()
    {
        CancelInvoke("DisableBullet");
        gameObject.SetActive(false);
    }
}
