using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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
    private GameObject[] GreyScaledObj;
    private GameObject[] GreyScaled;
    void Start()
    {
        GreyScaledObj = GameObject.FindGameObjectsWithTag("GreyScaledObj");
        GreyScaled = GameObject.FindGameObjectsWithTag("GreyScaled");
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
        if (damaged > 0.5f)
        {
            Volume postProcessingVolume = GameObject.FindGameObjectWithTag("Volume").GetComponent<Volume>();
            if (postProcessingVolume.profile.TryGet<Vignette>(out var vignette))
            {
                vignette.color.value = new Color32(255, 0, 0, 255);
                DOTween.To(() => vignette.color.value, x => vignette.color.value = x, new Color32(0, 0, 0, 0), 5f);
            }
        }
        if (damaged > 2)
        {
            Destroy(gameObject);
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
                Instantiate(myBottomParticles, JumpCheck2.transform.position, Quaternion.Euler(new Vector3(-90f, 0, 0)));
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
        AnimateMe();
    }
    void AnimateMe()
    {
        float dirX = Input.GetAxis("Horizontal");
        if ((Mathf.Abs(rb.linearVelocity.x) > 0f && Mathf.Abs(dirX) > 0.2f) || (!IsGrounded() && anim.GetBool("AmIWalking")))
        {
            GetComponent<soundManager>().StartPlaying(false);
            anim.SetBool("AmIWalking", true);
            anim.SetBool("Jumping", false);
        }
        else
        {
            GetComponent<soundManager>().StopPlaying(false);
            anim.SetBool("AmIWalking", false);
            if ((Mathf.Abs(rb.linearVelocity.y) > 0.5f || !IsGrounded()) && Mathf.Abs(rb.linearVelocity.x) <= 0f)
            {
                anim.SetBool("Jumping", true);
            }
            else
            {
                anim.SetBool("Jumping", false);
            }
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
        foreach (GameObject gm in GreyScaled)
        {
            gm.GetComponent<TilemapRenderer>().material.SetVector("_PlayerPosition", transform.position);
        }
        foreach (GameObject gm in GreyScaledObj)
        {
            gm.GetComponent<SpriteRenderer>().material.SetVector("_PlayerPosition", transform.position);
        }
        // foreach (GameObject gm in GameObject.FindGameObjectsWithTag("GreyScaledEnem"))
        // {
        //     foreach (SpriteRenderer rnd in gm.GetComponent<EnemyAI>().listSpr)
        //     {
        //         rnd.material.SetVector("_PlayerPosition", transform.position);
        //     }
        // }

        // Apply extra gravity when falling for better feel
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity -= Vector2.up * gravityModifier * Time.fixedDeltaTime;
        }
    }
}
