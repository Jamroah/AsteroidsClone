using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    public GameObject ship;
    public GameObject gameOverPanel;

    public static ShipController PlayerShip;

    private static int m_maxLives = 3;
    private static int m_currentLives;

    private static bool m_gameOver;

    public static int MaxHealth
    {
        get { return m_maxLives; }
        private set { }
    }

    public static int CurrentHealth
    {
        get { return m_currentLives; }
        private set
        {          
            if (value <= 0)
            {
                GameOver();
            }
            else if (value < m_currentLives)
            {
                GameManager.Instance.StartCoroutine(GameManager.Instance.SpawnShip(true, 2));
            }

            m_currentLives = value;
        }
    }

    void Start()
    {
        GameStart();
    }

    void Update()
    {
        if(m_gameOver)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                DestroyImmediate(PlayerShip.gameObject);
                PlayerShip = null;
                GameStart();
            }
        }
    }

    public static void GameStart()
    {
        m_gameOver = false;
        GameManager.Instance.gameOverPanel.SetActive(false);
        CurrentHealth = MaxHealth;
        GameManager.Instance.StartCoroutine(GameManager.Instance.SpawnShip(false, 0));
        Messenger.Broadcast("Update UI");
    }

    public static void GameOver()
    {
        m_gameOver = true;
        GameManager.Instance.gameOverPanel.SetActive(true);
    }

    public IEnumerator SpawnShip(bool invincible, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (PlayerShip == null)
            PlayerShip = Instantiate(GameManager.Instance.ship).GetComponent<ShipController>();

        PlayerShip.Initialise();

        if (invincible)
            GameManager.Instance.StartCoroutine(PlayerShip.Invincibility());
    }

    public static void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        Messenger.Broadcast("Update UI");
    }
}