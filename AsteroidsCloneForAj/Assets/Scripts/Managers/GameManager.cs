using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    public GameObject ship;
    public GameObject asteroidBig;
    public GameObject asteroidSmall;
    public GameObject gameOverPanel;

    public static ShipController PlayerShip;
    public static GameObjectPool BigAsteroidPool;
    public static GameObjectPool SmallAsteroidPool;

    private static int m_maxLives = 3;
    private static int m_currentLives;
    private static int m_score = 0;

    //private static Coroutine m_spawnAsteroids;

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
                HighScore = Score;
                ModalPanel.GameOver(ResetBoard);
                Instance.StopAllCoroutines();
                //Instance.StopCoroutine(Instance.SpawnAsteroids());
            }
            else if (value < m_currentLives)
            {
                Instance.StartCoroutine(Instance.SpawnShip(true, 2));
            }

            m_currentLives = value;
        }
    }

    public static int Score
    {
        get { return m_score; }
        set
        {
            m_score = value;
            Messenger<int>.Broadcast("Update Score UI", m_score);
        }
    }

    public static int HighScore
    {
        get
        {
            if (PlayerPrefs.HasKey("HighScore"))
                return PlayerPrefs.GetInt("HighScore");

            return 0;
        }
        set
        {
            if (!PlayerPrefs.HasKey("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", value);
                return;
            }

            if (value > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", value);
            }
        }
    }

    void Start()
    {
        BigAsteroidPool = new GameObjectPool(asteroidBig, 10, true, transform);
        SmallAsteroidPool = new GameObjectPool(asteroidSmall, 20, true, transform);
        //GameStart();
        InputManager.PushInputContext(INPUT_CONTEXT.GAME);
        ModalPanel.Menu(GameStart);
    }

    void Update()
    {
        if (InputManager.GetButtonDown("Pause", INPUT_CONTEXT.GAME))
            ModalPanel.PauseGame();
        else if (InputManager.GetButtonDown("Pause", INPUT_CONTEXT.PAUSE))
            ModalPanel.UnPauseGame();
    }

    public static void GameStart()
    {
        
        Instance.gameOverPanel.SetActive(false);
        CurrentHealth = MaxHealth;
        Instance.StartCoroutine(Instance.SpawnShip(false, 0));
        Instance.StartCoroutine(Instance.SpawnAsteroids());
        BigAsteroidPool.DisableAll();
        SmallAsteroidPool.DisableAll();
        Score = 0;
        Messenger.Broadcast("Update UI");    
    }

    public static void ResetBoard()
    {
        Instance.StopAllCoroutines();
        DestroyImmediate(PlayerShip.gameObject);
        PlayerShip = null;
        ModalPanel.Menu(GameStart);
    }

    public IEnumerator SpawnAsteroids()
    {
        bool bossTime = false;
        while (!bossTime)
        {
            BigAsteroidPool.Get(true).GetComponent<Asteroid>().SetTrajectory(GetRandomSpawnPoint());
            yield return new WaitForSeconds(Score > 2500 ? (Score > 5000 ? 0.5f : 1) : 2);

            if (Score > 10000)
                bossTime = true;
        }
    }

    public IEnumerator SpawnShip(bool invincible, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (PlayerShip == null)
            PlayerShip = Instantiate(Instance.ship).GetComponent<ShipController>();

        PlayerShip.Initialise();

        if (invincible)
            Instance.StartCoroutine(PlayerShip.Invincibility());
    }

    public static void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        Messenger.Broadcast("Update UI");
    }

    // Could probably be better. 
    // Alternatively could have used a particle system but I think that would have overcomplicated things not being able to use GameObjects as particles.
    static Vector2 GetRandomSpawnPoint()
    {
        float chance = Random.Range(0, 101);

        Vector2 coords = new Vector2(Random.Range(25, 75f) / 100f, Random.Range(25, 75f) / 100f);

        // An improvement would be to detect which edge the random coord is closest to and move the difference so you don't get asteroids spawning too far out.
        // This'll do for for now. It works and other things take priority.
        if (chance < 25)
            coords.x -= 1;
        else if( chance < 50)
            coords.x += 1;
        else if (chance < 75)
            coords.y += 1;
        else if (chance < 100)
            coords.y -= 1;

        Debug.Log(coords);
        return Camera.main.ViewportToWorldPoint(coords);
    }
}