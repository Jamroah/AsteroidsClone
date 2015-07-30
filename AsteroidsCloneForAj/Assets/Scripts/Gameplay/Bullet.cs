using UnityEngine;
using System.Collections;

public class Bullet : ScreenwrapObject
{
    private Vector2 m_direction = new Vector2();

    // Use this for initialization
    public override void OnEnable()
    {
        base.OnEnable();
        Invoke("Death", 1);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        m_transform.Translate(m_direction * 0.75f);
    }

    public void SetDirection(Vector2 direction)
    {
        m_direction = direction;
    }

    private void Death()
    {
        CancelInvoke("Death");
        gameObject.SetActive(false);
    }
}
