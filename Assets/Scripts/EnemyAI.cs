using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform player;

    public float speed = 2f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public float checkRad = 10f;
    public bool blinded;
    public GameObject blindparticles;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;
    private bool canAttack = true;
    private bool shouldAttack;

    public Transform jumpCheck;
    public SpriteRenderer[] listSpr;

    public float cooldown = 2f;
    public void Blind()
    {
        rb.mass = 3f;
        blinded = true;
        blindparticles.SetActive(true);
        CancelInvoke(nameof(UnBlind));
        Invoke(nameof(UnBlind), 4f);
    }
    public void UnBlind()
    {
        rb.mass = 1f;
        blinded = false;
        blindparticles.SetActive(false);
    }
    void Start()
    {
        blindparticles.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 3f;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the Player has the correct tag.");
            enabled = false;
            return;
        }
        InvokeRepeating(nameof(InvokeJump), 5f, 5f);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(jumpCheck.position, 0.5f, groundLayer);
    }

    void Update()
    {
        if (player == null) return; // Avoid null reference issues

        if (ShouldAct() && !blinded)
        {
            isGrounded = IsGrounded();
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, playerLayer);

            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

                RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);
                RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, groundLayer);
                RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, groundLayer);

                if (groundInFront.collider && gapAhead.collider)
                {
                    shouldJump = true;
                }
                else if (isPlayerAbove && platformAbove.collider)
                {
                    shouldJump = true;
                }
            }
            shouldAttack = Physics2D.OverlapCircle(transform.position, 0.5f, playerLayer);
        }
    }

    void InvokeJump()
    {
        if (!blinded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
            Vector2 dirToPlayer = player.transform.position - transform.position;
            if (Mathf.Abs(dirToPlayer.x) > 0.5f)
            {
                if (dirToPlayer.x > 0f && transform.rotation != Quaternion.Euler(0, 180, 0))
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (dirToPlayer.x < 0f && transform.rotation != Quaternion.Euler(0, 0, 0))
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }

            if (isGrounded && shouldJump)
            {
                shouldJump = false;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            if (shouldAttack && canAttack)
            {
                Player playerScript = player.GetComponent<Player>();
                if (playerScript != null)
                {
                    playerScript.Hit();
                    GetComponent<soundManager>().PlaySound(0, 0.9f, 1.1f, false);
                    canAttack = false;
                    StartCoroutine(AttackCooldown());
                }
            }
        }
    }

    public IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
