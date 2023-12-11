using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPlayer2 : MonoBehaviour
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
    bool BlockDebounce = false;
    bool HitDebounce = false;

    Vector3 EnemyPosition;

    void Start()
    {
        HealthBar = GameObject.Find("HealthSystem").transform.GetChild(1).gameObject.GetComponent<Image>();
        PlayerRGBD = GetComponent<Rigidbody2D>();
        MovingCollider = Player.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift) && !PunchDebounce)
        {
            AttackingAudio.Play();
            Animator.SetTrigger("Punch");
            GetComponent<PlayerMovement2>().Attacking = true;
            PunchDebounce = true;
            DoAttackPunch();
        }

        if (Input.GetKeyDown(KeyCode.Slash) && !BlockDebounce)
        {
            BlockDebounce = true;
            DoAttackBlock();
        }
    }

    private void OnTriggerEnter2D(Collider2D HitObject)
    {
        if (HitObject.gameObject != gameObject && HitObject.gameObject.GetComponent<Rigidbody2D>() && !HitDebounce)
        {
            Debug.Log("Player2 was attacked!");
            HitDebounce = true;
            EnemyPosition = HitObject.transform.position;
            if (HitObject.GetComponent<AttackPlayer1>().InputName == "Punch" && !BlockDebounce)
            {
                HitObject.GetComponent<PlayerMovement1>().Stunned = true;
                RegisterAttack(300f, HitObject.gameObject, 1);
            }
            else if (!BlockDebounce)
            {
                HitObject.GetComponent<PlayerMovement1>().Stunned = true;
                RegisterAttack(150f, HitObject.gameObject, 3);
            }
        }
    }

    void RegisterAttack(float Force, GameObject HitObject, int Damage)
    {
        HealthBar.GetComponent<HealthBar>().current -= Damage;
        HitAudio.Play();
        PlayerRGBD.AddForce(Vector3.Normalize(EnemyPosition - transform.position) * -Force, ForceMode2D.Force);
        StartCoroutine(WaitTimeAfterHit(HitObject));
    }
    void DoAttackPunch()
    {
        InputName = "Punch";
        StartCoroutine(WaitTimePunch());
    }

    void DoAttackBlock()
    {
        StartCoroutine(WaitTimeBlock());
    }

    IEnumerator WaitTimeAfterHit(GameObject ObjHit)
    {
        yield return new WaitForSeconds(0.5f);
        ObjHit.GetComponent<PlayerMovement1>().Stunned = false;
        HitDebounce = false;
    }

    IEnumerator WaitTimePunch()
    {
        yield return new WaitForSeconds(0.10f);
        MovingCollider.offset = new Vector2(1.6f, 0);
        MovingCollider.enabled = true;
        yield return new WaitForSeconds(0.05f);
        MovingCollider.enabled = false;
        yield return new WaitForSeconds(0.1f);
        GetComponent<PlayerMovement2>().Attacking = false;
        PunchDebounce = false;
    }

    IEnumerator WaitTimeBlock()
    {
        PunchDebounce = true;
        GetComponent<PlayerMovement2>().Blocking = true;
        yield return new WaitForSeconds(0.35f);
        while (Input.GetKeyDown(KeyCode.Slash))
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.35f);
        GetComponent<PlayerMovement2>().Blocking = false;
        PunchDebounce = false;
        BlockDebounce = false;
    }
}
