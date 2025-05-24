using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    private float horizontal;
    private float speed = 6f;
    private float jumpingPower = 15f;
    private bool isFacingRight = true;
    private bool doubleJump;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(10f, 18f);

    private Vector2 rightParticlePos;
    private Vector2 leftParticlePos;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private ParticleSystem dustParticles;

    private void Start()
    {
        rightParticlePos = dustParticles.transform.localPosition;
        leftParticlePos = new Vector2(-rightParticlePos.x, rightParticlePos.y);
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("magnitude", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("isWallSliding", isWallSliding);

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
                animator.SetTrigger("jump");
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayJumpSound();
                }
                else
                {
                    Debug.LogWarning("AudioManager instance is null when player jumps.");
                }
            }
            else if (isWallSliding)
            {
                isWallJumping = true;
                wallJumpingDirection = isFacingRight ? -1 : 1;
                rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
                wallJumpingCounter = wallJumpingTime;

                if ((isFacingRight && wallJumpingDirection < 0) || (!isFacingRight && wallJumpingDirection > 0))
                    Flip();

                Invoke(nameof(StopWallJumping), wallJumpingDuration);
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayJumpSound();
                }
                else
                {
                    Debug.LogWarning("AudioManager instance is null when player wall jumps.");
                }
            }
            else if (!doubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
                animator.SetTrigger("doubleJump");
                doubleJump = true;
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayJumpSound();
                }
                else
                {
                    Debug.LogWarning("AudioManager instance is null when player double jumps.");
                }
            }
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        WallSlide();

        UpdateParticleDirection();

        if (Mathf.Abs(horizontal) > 0.1f && IsGrounded() && !dustParticles.isPlaying)
        {
            dustParticles.Play();
        }
        else if ((Mathf.Abs(horizontal) < 0.1f || !IsGrounded()) && dustParticles.isPlaying)
        {
            dustParticles.Stop();
        }

        if (!isWallJumping)
            FlipCheck();
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void FlipCheck()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void UpdateParticleDirection()
    {
        if (isFacingRight)
        {
            dustParticles.transform.localPosition = rightParticlePos;
            dustParticles.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            dustParticles.transform.localPosition = leftParticlePos;
            dustParticles.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        dustParticles.transform.position = new Vector3(transform.position.x - 0.5f * Mathf.Sign(transform.localScale.x), transform.position.y - 0.5f, transform.position.z);
    }
}
