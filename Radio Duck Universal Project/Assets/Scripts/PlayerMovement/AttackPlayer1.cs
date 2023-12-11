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

    public Image Healthbar;

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
        Healthbar = GameObject.Find("HealthSystem").transform.GetChild(0).gameObject.GetComponent<Image>();
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
            GetComponent<PlayerMovement1>().Attacking = true;
            PunchDebounce = true;
            DoAttackPunch();
        }

        if (Input.GetKeyDown(KeyCode.C) && !KickDebounce)
        {
            AttackingAudio.Play();
            Animator.SetTrigger("Kick");
            GetComponent<PlayerMovement1>().Attacking = true;
            KickDebounce = true;
            DoAttackKick();
        }
    }

    private void OnTriggerEnter2D(Collider2D HitObject)
    {
        if (HitObject.gameObject != gameObject && HitObject.gameObject.GetComponent<Rigidbody2D>() && !HitDebounce)
        {
            Debug.Log("Player1 was attacked!");
            HitDebounce = true;
            EnemyPosition = HitObject.transform.position;
            if (HitObject.GetComponent<AttackPlayer2>().InputName == "Punch")
            {
                HitObject.GetComponent<PlayerMovement2>().Stunned = true;
                RegisterAttack(300f, HitObject.gameObject);
            }
            else
            {
                HitObject.GetComponent<PlayerMovement2>().Stunned = true;
                RegisterAttack(350f, HitObject.gameObject);
            }
        }
    }

    void RegisterAttack(float Force, GameObject HitObject)
    {
        HitAudio.Play();
        PlayerRGBD.AddForce(Vector3.Normalize(EnemyPosition - transform.position) * -Force, ForceMode2D.Force);
        StartCoroutine(WaitTimeAfterHit(HitObject));
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

    IEnumerator WaitTimeAfterHit(GameObject ObjHit)
    {
        yield return new WaitForSeconds(0.5f);
        ObjHit.GetComponent<PlayerMovement2>().Stunned = false;
        HitDebounce = false;
    }

    IEnumerator WaitTimePunch()
    {
        yield return new WaitForSeconds(0.10f);
        Debug.Log(transform.position.x / 3);
        Debug.Log(transform.position.x);
        MovingCollider.offset = new Vector2(1.6f, 0);
        MovingCollider.enabled = true;
        yield return new WaitForSeconds(0.05f);
        MovingCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        GetComponent<PlayerMovement1>().Attacking = false;
        PunchDebounce = false;
    }

    IEnumerator WaitTimeKick()
    {
        yield return new WaitForSeconds(0.15f);
        MovingCollider.offset = new Vector2(1.2f, -0.3f);
        MovingCollider.enabled = true;
        yield return new WaitForSeconds(0.08f);
        MovingCollider.enabled = false;
        yield return new WaitForSeconds(0.4f);
        GetComponent<PlayerMovement1>().Attacking = false;
        KickDebounce = false;
    }
}
