using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Transform player;

    public float speed = 2f;
    public float defSpeed = 2f;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public float checkRad = 10f;
    public bool blinded = false;
    public GameObject blindparticles;
    public float health;

    private Rigidbody2D rb;
    private bool canAttack = true;
    private bool shouldAttack;

    public Animator anim;

    public GameObject HitCol;

    public float cooldown = 1f;
    public void Blind()
    {
        health -= Time.deltaTime;
        //blindparticles.SetActive(true);
        CancelInvoke(nameof(UnBlind));
        Invoke(nameof(UnBlind), 1f);
    }
    public void UnBlind()
    {
        //blindparticles.SetActive(false);
    }
    void Start()
    {
        speed = defSpeed;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the Player has the correct tag.");
            enabled = false;
            return;
        }
        StartCoroutine(SpeedUp());
    }
    public IEnumerator SpeedUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 10f));
            speed = defSpeed * 5f;
            anim.speed = 5f;
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
            speed = defSpeed;
            anim.speed = 1f;
        }

    }
    void Update()
    {
        if (player == null) return; // Avoid null reference issues

        if (ShouldAct() && !blinded)
        {
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
            shouldAttack = Physics2D.OverlapCircle(transform.position, 5f, playerLayer);
        }
    }


    bool ShouldAct()
    {
        return Physics2D.OverlapCircle(transform.position, checkRad, playerLayer);
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        if (ShouldAct() && !blinded)
        {
            if (Mathf.Abs(rb.linearVelocity.x) > 0f)
            {
                if (rb.linearVelocity.x > 0f && transform.rotation != Quaternion.Euler(0, 180, 0))
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (rb.linearVelocity.x < 0f && transform.rotation != Quaternion.Euler(0, 0, 0))
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }


            if (shouldAttack && canAttack)
            {
                Player playerScript = player.GetComponent<Player>();
                if (playerScript != null)
                {
                    anim.SetTrigger("Hitt");
                    canAttack = false;
                    StartCoroutine(AttackCooldown());
                }
            }
        }
    }

    public IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        anim.ResetTrigger("Hitt");
        if (Physics2D.OverlapCircle(HitCol.transform.position, 3f, playerLayer))
        {
            player.GetComponent<Player>().Hit();
        }
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
