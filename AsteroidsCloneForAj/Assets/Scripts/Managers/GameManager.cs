using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    public GameObject ship;
    public GameObject asteroidBig;
    public GameObject asteroidSmall;
    public GameObject enemyShip;
    public GameObject enemyBullet;
    //public GameObject gameOverPanel;

    public static ShipController PlayerShip;
    public static GameObjectPool BigAsteroidPool;
    public static GameObjectPool SmallAsteroidPool;
    public static GameObjectPool EnemyShipPool;
    public static GameObjectPool EnemyBulletPool;

    private static int m_maxLives = 3;
    private static int m_currentLives;
    private static int m_score = 0;

    //private static Coroutine m_spawnAsteroids;

    public static int MaxLives
    {
        get { return m_maxLives; }
        private set { }
    }

    public static int CurrentLives
    {
        get { return m_currentLives; }
        set
        {          
            if (value <= 0)
            {               
                ModalPanel.GameOver(ResetBoard);
                Instance.StopAllCoroutines();
            }
            else if (value < m_currentLives)
            {
                Debug.Log("Hull Damage Taken!");
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
        AudioManager.PlayBGM("Menu", TRANSITION.INSTANT);

        BigAsteroidPool = new GameObjectPool(asteroidBig, 10, true, transform);
        SmallAsteroidPool = new GameObjectPool(asteroidSmall, 20, true, transform);
        EnemyShipPool = new GameObjectPool(enemyShip, 1, false, transform);
        EnemyBulletPool = new GameObjectPool(enemyBullet, 10, true, transform);
        InputManager.PushInputContext(INPUT_CONTEXT.GAME);
        ModalPanel.Menu(GameStart);
    }

    void Update()
    {
        if (InputManager.GetButtonDown("Pause", INPUT_CONTEXT.GAME))
            ModalPanel.PauseGame();
        else if (InputManager.GetButtonDown("Pause", INPUT_CONTEXT.PAUSE))
            ModalPanel.UnPauseGame();

        //if (Input.GetKeyDown(KeyCode.KeypadPlus))
            //Score += 10000;
    }

    public static void GameStart()
    {       
        //Instance.gameOverPanel.SetActive(false);
        CurrentLives = MaxLives;
        Instance.StartCoroutine(Instance.SpawnShip(false, 0));
        Instance.StartCoroutine(Instance.SpawnAsteroids());
        BigAsteroidPool.DisableAll();
        SmallAsteroidPool.DisableAll();
        Score = 0;
        AudioManager.PlayBGM("Game", TRANSITION.INSTANT);
        Messenger.Broadcast("Update UI");    
    }

    public static void ResetBoard()
    {
        HighScore = Score;
        Messenger.Broadcast("Reset", MessengerMode.DONT_REQUIRE_LISTENER);
        AudioManager.PlayBGM("Menu", TRANSITION.INSTANT);
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
            BigAsteroidPool.Get(true).GetComponent<Asteroid>().SetTrajectory(MathV2D.GetRandomVectorOutsideCamera(AXIS_BIAS.BOTH));

            // Arbitrary 15% chance of spawning an enemy space muffin. One at a time.
            if (Random.Range(0, 100) >= 85)
            {
                GameObject eship = EnemyShipPool.Get(true);
                if(eship != null)
                    eship.GetComponent<EnemyShip>().SetSpawnPoint(MathV2D.GetRandomVectorOutsideCamera(AXIS_BIAS.HORIZONTAL));
            }

            yield return new WaitForSeconds(Score > 2500 ? (Score > 5000 ? 1f : 1.5f) : 2);

            if (Score >= 10000)
            {
                bossTime = true;
                Messenger.Broadcast("Boss Time", MessengerMode.DONT_REQUIRE_LISTENER);
            }
        }
    }

    public IEnumerator SpawnShip(bool invincible, float delay)
    {
        Debug.Log("Spawning the ship in " + delay + " seconds");
        yield return new WaitForSeconds(delay);

        if (PlayerShip == null)
            PlayerShip = Instantiate(Instance.ship).GetComponent<ShipController>();

        PlayerShip.Initialise();

        if (invincible)
            Instance.StartCoroutine(PlayerShip.Invincibility());
    }

    
}