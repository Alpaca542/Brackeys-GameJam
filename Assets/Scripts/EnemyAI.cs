using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    public float speed = 2f;
    public float jumpForce = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool IsGrounded;
    private bool shouldJump;
    private bool canAttack = true;
    private bool shouldAttack;

    public Transform JumpCheck1;
    public Transform JumpCheck2;
    public Transform JumpCheck3;

    public float cooldown;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private bool Grounded()
    {
        if (Physics2D.Raycast(JumpCheck1.position, -transform.up, 0.1f, groundLayer) || Physics2D.Raycast(JumpCheck2.position, -transform.up, 0.1f, groundLayer) || Physics2D.Raycast(JumpCheck3.position, -transform.up, 0.1f, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Update()
    {
        IsGrounded = Grounded();
        float direction = Mathf.Sin(player.position.x - transform.position.x);
        bool isPlayerAbove = Physics2D.Raycast(transform.position, transform.up, 3f, 1 << player.gameObject.layer);

        if (IsGrounded)
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);
            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, groundLayer);
            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, groundLayer);

            if (!groundInFront.collider && !gapAhead.collider)
            {
                shouldJump = true;
            }
            else if (isPlayerAbove && platformAbove.collider)
            {
                shouldJump = true;
            }
        }
        shouldAttack = Physics2D.OverlapCircle(transform.position, 0.5f, 1 << player.gameObject.layer);
    }
    private void FixedUpdate()
    {
        if (IsGrounded && shouldJump)
        {
            shouldJump = false;
            Vector2 direction = (player.position - transform.position).normalized;

            Vector2 JumpDirection = direction * jumpForce;

            rb.AddForce(new Vector2(JumpDirection.x, jumpForce), ForceMode2D.Impulse);
        }

        if (shouldAttack && canAttack)
        {
            player.gameObject.GetComponent<Player>().Hit();
            canAttack = false;
            StartCoroutine(attackCrtn());
        }
    }

    public IEnumerator attackCrtn()
    {
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}
