using UnityEngine;
using System.Collections;

// Things admittedly get a little messy here. Communication between the eyes and the controller is a little spaghetti-like.
// It works, though. Production-quality code isn't written in a week-long gamejam.
public class AJController : MonoBehaviour
{
    public enum PHASE
    {
        ONE,
        TWO,
    }

    public AJEye leftEye;
    public AJEye rightEye;
    public Sprite defaultEye;
    public Sprite attackEye;
    public Transform bulletEmitter;

    private Animator m_anim;
    private PHASE m_phase = PHASE.ONE;

    public PHASE Phase
    {
        get { return m_phase; }
        private set { m_phase = value; }
    }

    public int MaxHealth
    {
        get { return leftEye.MaxHealth + rightEye.MaxHealth; }
        private set { }
    }

    public int CurrentHealth
    {
        get { return leftEye.CurrentHealth + rightEye.CurrentHealth; }
        private set { }
    }

    void OnEnable()
    {
        Messenger.AddListener("Boss Time", BeginBattle);
        Messenger.AddListener("Reset", Reset);
        m_anim = GetComponent<Animator>();
    }

    void OnDisable()
    {
        Messenger.RemoveListener("Reset", Reset);
        Messenger.RemoveListener("Boss Time", BeginBattle);
    }

    void Reset()
    {
        m_anim.SetTrigger("Reset");
        transform.position = new Vector2(20, 0);
        leftEye.PauseAttacking();
        rightEye.PauseAttacking();      
        StopAllCoroutines();
        ModalPanel.Instance.bossPanel.SetActive(false);
    }

    void BeginBattle()
    {
        AudioManager.PlayBGM("Boss", TRANSITION.INSTANT);
        leftEye.CurrentHealth = leftEye.MaxHealth;
        rightEye.CurrentHealth = rightEye.MaxHealth;
        m_phase = PHASE.ONE;
        leftEye.Initialise();
        rightEye.Initialise();
        ModalPanel.Instance.bossPanel.SetActive(true);
        ModalPanel.Instance.bossHealth.value = MaxHealth;
        StartCoroutine("MoveToCenter");
    }

    IEnumerator MoveToCenter()
    {
        while (transform.position != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, 0.02f);
            yield return null;
        }

        leftEye.BeginAttacking();
        rightEye.BeginAttacking();
        ModalPanel.DisableBossMessage();
        Debug.Log("AJ has reached the center");
    }

    public void BeginMouthAttack()
    {
        leftEye.PauseAttacking();
        rightEye.PauseAttacking();
        StartCoroutine("MouthAttack");
    }

    public void EndMouthAttack()
    {
        leftEye.BeginAttacking();
        rightEye.BeginAttacking();
        StopCoroutine("MouthAttack");
    }

    IEnumerator MouthAttack()
    {
        while(m_anim.GetCurrentAnimatorStateInfo(0).fullPathHash != Animator.StringToHash("Base Layer.AJ Mouth Attack"))
        {
            yield return null;
        }

        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            GameManager.EnemyBulletPool.Get(true).GetComponent<Bullet>().Fire(gameObject, bulletEmitter.position, Random.Range(0f, 360f), 15);
        }
    }

    public void NextPhase()
    {
        switch (Phase)
        {
            case PHASE.ONE:
                m_anim.SetTrigger("Mouth Attack");
                Phase = PHASE.TWO;
                break;
            case PHASE.TWO:
                StartCoroutine("DeathSequence");
                break;
            default:
                break;
        }
    }

    IEnumerator DeathSequence()
    {
        m_anim.SetTrigger("Dead");
        yield return new WaitForSeconds(2);
        ModalPanel.Message("YOU WIN, CONGRATULATIONS!", "Press START to begin a new adventure", "Start", GameManager.ResetBoard, false);
    }
}