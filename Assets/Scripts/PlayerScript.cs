using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    public float jump = 10f; // Increased for better responsiveness
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.3f;
    public float jumpTime;
    public float gravityModifier = 2.5f;
    public float HealSpeed;

    [Header("Debug")]
    private Rigidbody2D rb;
    public Animator anim;
    private bool canJump = false;
    public LayerMask groundlayer;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool justGrounded = true;
    public float jumpTimeCounter;
    public bool startedJumping = false;
    public float damaged;

    [Header("Fields")]
    [SerializeField] private GameObject myBottomParticles;

    [Header("JumpChecks")]
    [SerializeField] private Transform JumpCheck1;
    [SerializeField] private Transform JumpCheck2;
    [SerializeField] private Transform JumpCheck3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        anim.SetBool("AmIWalking", false);
    }

    public IEnumerator Regen()
    {
        while (damaged > 0)
        {
            damaged -= Time.deltaTime * HealSpeed;
            yield return null;
        }
        damaged = 0;
    }

    public void Hit()
    {
        if (damaged <= 0)
        {
            damaged++;
            StartCoroutine(Regen());
        }
        else
        {
            damaged++;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(JumpCheck1.position, Vector2.down, 0.1f, groundlayer) ||
               Physics2D.Raycast(JumpCheck2.position, Vector2.down, 0.1f, groundlayer) ||
               Physics2D.Raycast(JumpCheck3.position, Vector2.down, 0.1f, groundlayer);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
    }

    private void Update()
    {
        canJump = IsGrounded();

        if (canJump)
        {
            if (!justGrounded)
            {
                if (jumpBufferCounter > 0f)
                {
                    Jump();
                    jumpBufferCounter = 0f; // Reset buffer after using it
                }
                justGrounded = true;
                Instantiate(myBottomParticles, JumpCheck2.transform.position, Quaternion.identity);
            }

            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            justGrounded = false;
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump Buffering
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            startedJumping = true;
            Jump();
            jumpBufferCounter = 0f; // Reset after successful jump
            jumpTimeCounter = jumpTime;
        }

        if (jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Hold Space for variable jump height
        if (Input.GetKey(KeyCode.Space) && jumpTimeCounter > 0f && startedJumping)
        {
            jumpTimeCounter -= Time.deltaTime;
            Jump();
        }
        else
        {
            startedJumping = false;
        }
    }

    void FixedUpdate()
    {
        float dirX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(dirX * speed * Time.fixedDeltaTime * 100, rb.linearVelocity.y);

        if (Mathf.Abs(dirX) > 0.2f)
        {
            if (dirX < 0.2f && transform.rotation != Quaternion.Euler(0, 180, 0) && transform.rotation != Quaternion.Euler(0, -180, 0))
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (dirX > 0.2f && transform.rotation != Quaternion.Euler(0, 0, 0))
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        foreach (GameObject gm in GameObject.FindGameObjectsWithTag("GreyScaled"))
        {
            gm.GetComponent<TilemapRenderer>().material.SetVector("_PlayerPosition", transform.position);

        }

        // Apply extra gravity when falling for better feel
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity -= Vector2.up * gravityModifier * Time.fixedDeltaTime;
        }
    }
}
