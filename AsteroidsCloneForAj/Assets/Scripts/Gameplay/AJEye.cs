using UnityEngine;
using System.Collections;

public class AJEye : MonoBehaviour, IDamageable
{
    public SpriteRenderer renderer;

    private int m_currentHealth;
    private bool m_disabled;
    private AJController m_controller;
    private Collider2D m_collider;

    private float redness = 0;

    public int MaxHealth
    {
        get { return 50; }
        set { }
    }

    public int CurrentHealth
    {
        get { return m_currentHealth; }
        set
        {
            m_currentHealth = value;

            if (m_currentHealth <= 0)
                Die();
        }
    }

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        m_controller = transform.parent.GetComponent<AJController>();
        m_collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_disabled)
            return;

        if(GameManager.PlayerShip != null)
        {
            transform.LookAt2DLerp(GameManager.PlayerShip.transform.position, 0.1f);
        }

        if (redness <= 1)
            redness += Time.deltaTime * 2;

        renderer.color = Color.Lerp(Color.red, Color.white, redness);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Hitable")
        {
            col.gameObject.GetComponent<IDamageable>().TakeDamage(1, gameObject);
        }
    }

    public void Initialise()
    {
        renderer.enabled = true;
        m_disabled = false;
        CurrentHealth = MaxHealth;
        m_collider.enabled = false;
    }

    public void TakeDamage(int value, GameObject culprit)
    {
        if (m_disabled)
            return;

        if (culprit != null && culprit == GameManager.PlayerShip.gameObject)
        {
            CurrentHealth -= value;
            GameManager.Score += 200;
            redness = 0;
            //Messenger.Broadcast("Update Boss UI");
            ModalPanel.Instance.bossHealth.value = m_controller.CurrentHealth;
        }
    }

    public void Die()
    {
        // We don't destroy or disable the game object because we still need the health property.
        AudioManager.PlaySFX("Enemy Explosion", 0.4f, 0.6f);
        m_disabled = true;
        renderer.enabled = false;
        m_collider.enabled = false;
        PauseAttacking();
        m_controller.NextPhase();
    }

    public void BeginAttacking()
    {
        if (m_disabled)
            return;

        m_collider.enabled = true;
        Invoke("Fire", Random.Range(1.5f, 2.5f));
    }

    public void PauseAttacking()
    {
        m_collider.enabled = false;
        CancelInvoke("Fire");
    }

    public void Fire()
    {
        GameManager.EnemyBulletPool.Get(true).GetComponent<Bullet>().Fire(gameObject, transform.position, transform.rotation.eulerAngles.z + 90, 30);
        Invoke("Fire", m_controller.Phase == AJController.PHASE.ONE ? Random.Range(1.5f, 2.5f) : Random.Range(0.5f, 1.5f));
    }
}
