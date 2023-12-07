using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPlayer1 : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator Animator;
    public GameObject Player;
    Rigidbody2D PlayerRGBD;
    public CircleCollider2D MovingCollider;
    public string InputName;

    public Image HealthBar;

    public GameObject PunchPosition;
    public GameObject KickPosition;

    public AudioSource AttackingAudio;
    public AudioSource HitAudio;

    bool PunchDebounce = false;
    bool KickDebounce = false;
    bool HitDebounce = false;

    Vector3 EnemyPosition;

    void Start()
    {
        PlayerRGBD = GetComponent<Rigidbody2D>();
        MovingCollider = Player.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !PunchDebounce)
        {
            AttackingAudio.Play();
            Animator.SetTrigger("Punch");
            GetComponent<PlayerMovement>().Attacking = true;
            PunchDebounce = true;
            DoAttackPunch();
        }

        if (Input.GetKeyDown(KeyCode.C) && !KickDebounce)
        {
            AttackingAudio.Play();
            Animator.SetTrigger("Kick");
            GetComponent<PlayerMovement>().Attacking = true;
            KickDebounce = true;
            DoAttackKick();
        }
    }

    private void OnTriggerEnter2D(Collider2D HitObject)
    {
        if (HitObject.gameObject != gameObject && HitObject.gameObject.GetComponent<Rigidbody2D>() && !HitDebounce)
        {
            Debug.Log(1);
            HitDebounce = true;
            EnemyPosition = HitObject.transform.position;
            if (HitObject.GetComponent<AttackPlayer2>().InputName == "Punch")
            {
                gameObject.GetComponent<PlayerMovement>().Stunned = true;
                RegisterAttack(150f, HitObject.gameObject, 1);
            }
            else
            {
                gameObject.GetComponent<PlayerMovement>().Stunned = true;
                RegisterAttack(250f, HitObject.gameObject, 3);
            }
        }
    }

    void RegisterAttack(float Force, GameObject HitObject, int Damage)
    {
        HealthBar.GetComponent<HealthBar>().current -= Damage;
        HitAudio.Play();
        PlayerRGBD.AddForce((EnemyPosition - transform.position).normalized * -Force, ForceMode2D.Force);
        StartCoroutine(WaitTimeAfterHit());
    }
    void DoAttackPunch()
    {
        InputName = "Punch";
        StartCoroutine(WaitTimePunch());
    }

    void DoAttackKick()
    {
        InputName = "Kick";
        StartCoroutine(WaitTimeKick());
    }

    IEnumerator WaitTimeAfterHit()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<PlayerMovement>().Stunned = false;
        HitDebounce = false;
    }

    IEnumerator WaitTimePunch()
    {
        yield return new WaitForSeconds(0.10f);
        MovingCollider.offset = (Vector2) (PunchPosition.transform.position - transform.position) * transform.localScale;
        MovingCollider.enabled = true;
        yield return new WaitForSeconds(0.05f);
        MovingCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        GetComponent<PlayerMovement>().Attacking = false;
        PunchDebounce = false;
    }

    IEnumerator WaitTimeKick()
    {
        yield return new WaitForSeconds(0.15f);
        MovingCollider.offset = (Vector2) (KickPosition.transform.position - transform.position) * transform.localScale;
        MovingCollider.enabled = true;
        yield return new WaitForSeconds(0.08f);
        MovingCollider.enabled = false;
        yield return new WaitForSeconds(0.4f);
        GetComponent<PlayerMovement>().Attacking = false;
        KickDebounce = false;
    }
}
