using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Vector2 movementDirection;
    public Vector2 movementSpeedRange = new Vector2(.5f, 2);
    public float accelerationSpeed = 0.5f;
    public float currentHorizontalSpeed;
    public float constantVerticalSpeed = 0.5f;

    public float deathDrag = 0.1f;

    public float accumulatedDistance = 0;
    public bool isInAir = false;
    public bool isDead = false;
    public bool isRagdoll = false;
    public AnimationCurve jumpCurve;
    public AnimationCurve deathScaleCurve;

    public BoxCollider2D playableArea;
    public Rigidbody2D rootBone;
    public List<Rigidbody2D> rigidbody2Ds;

    public int health = 1;

    public Animator animator;
    public ParticleSystem dust;

    bool deathScreenShowed = false;

    public List<(float distance, DynamicallyLoadedLevelElement element)> encounteredElements = new List<(float distance, DynamicallyLoadedLevelElement element)>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rigidbody2Ds = GetAllRbs();
        ToggleRagdoll(false);
    }

    float jumpStartTime;
    float jumpStartY;
    float deathStartVelocity;
    float deathStartY;
    void Update()
    {
        if (isDead)
        {
            if (isRagdoll)
            {
                currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed - deathDrag * Time.deltaTime, 0, currentHorizontalSpeed);
                var t = 1 - (currentHorizontalSpeed / deathStartVelocity);
                if (currentHorizontalSpeed != 0)
                {
                    rootBone.MovePosition(new Vector2(0, deathStartY + deathScaleCurve.Evaluate(t)));
                }
                foreach (var rb in rigidbody2Ds)
                {
                    if (rb != rootBone)
                        rb.gravityScale = deathScaleCurve.Evaluate(t); // currentHorizontalSpeed == 0 ? 0 : 1;
                }
                rootBone.angularDrag = 360 * (1 - currentHorizontalSpeed);
                if (currentHorizontalSpeed < .3f)
                {
                    foreach (var rb in rigidbody2Ds)
                    {
                        rb.drag = 10;
                        rb.angularDrag = 10;
                    }
                }

            }
            else
            {
                currentHorizontalSpeed = 0;
            }
            if (!deathScreenShowed && currentHorizontalSpeed == 0)
            {
                deathScreenShowed = true;
                
                StartCoroutine(GameOverCoroutine());
            }
            return;
        }

        dust.Play();
        currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed + (movementDirection.x * Time.deltaTime), movementSpeedRange.x, movementSpeedRange.y);
        var desiredPosition = transform.position + new Vector3(0, movementDirection.y * constantVerticalSpeed * Time.deltaTime);
        animator.SetBool(AnimatorConstants.inAir, isInAir);
        if (isInAir)
        {
            var t = Time.time - jumpStartTime;
            var v = jumpCurve.Evaluate(t);
            transform.position = new Vector3(0, jumpStartY + v);
            if (t > jumpCurve.keys[jumpCurve.length - 1].time)
            {
                isInAir = false;
                transform.position = Vector2.up * jumpStartY;
            }
        }
        else
        {
            animator.SetFloat(AnimatorConstants.runSpeed, currentHorizontalSpeed);
            if (playableArea.OverlapPoint(desiredPosition))
            {
                transform.position = desiredPosition;
                accumulatedDistance += currentHorizontalSpeed;
            }
        }
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(2);
        GameUI.instance.ShowGameOverPanel();
    }

    public void Die(bool ragdoll, GameObject killer)
    {
        if (isDead) return;

        isDead = true;
        deathStartVelocity = currentHorizontalSpeed;
        deathStartY = rootBone.position.y;
        GetComponent<BoxCollider2D>().enabled = false;
        if (ragdoll)
        {
            ToggleRagdoll(true);
            rootBone.AddTorque(-360 * currentHorizontalSpeed * 10, ForceMode2D.Impulse);
            rootBone.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>() * accelerationSpeed;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        var value = context.ReadValueAsButton();
        if (value && !isInAir)
        {
            isInAir = true;
            jumpStartTime = Time.time;
            jumpStartY = transform.position.y;
        }
    }

    List<Rigidbody2D> GetAllRbs()
    {
        List<Rigidbody2D> rbs = new List<Rigidbody2D>();
        Queue<Transform> children = new Queue<Transform>();
        children.Enqueue(rootBone.transform);
        while (children.Count > 0)
        {
            var c = children.Dequeue();
            var rb = c.GetComponent<Rigidbody2D>();
            if (rb != null) rbs.Add(rb);
            foreach (Transform child in c)
            {
                children.Enqueue(child);
            }
        }
        return rbs;
    }

    [ContextMenu("ToggleRagdoll")]
    public void ToggleRagdoll2()
    {
        ToggleRagdoll(isRagdoll);
    }

    public void ToggleRagdoll(bool ragdoll)
    {
        foreach (var rb in rigidbody2Ds)
        {
            rb.isKinematic = !ragdoll;
        }
        isRagdoll = ragdoll;
        animator.enabled = !isRagdoll;
    }
}
