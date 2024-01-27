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
    public AnimationCurve jumpCurve;
    public AnimationCurve deathScaleCurve;

    public BoxCollider2D playableArea;
    public Rigidbody2D rootBone;
    public List<Rigidbody2D> rigidbody2Ds;

    public ParticleSystem dust; 

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
            currentHorizontalSpeed=Mathf.Clamp(currentHorizontalSpeed-deathDrag*Time.deltaTime, 0, currentHorizontalSpeed);
            var t = 1-(currentHorizontalSpeed/deathStartVelocity);
            transform.position = new Vector3(transform.position.x, deathStartY + deathScaleCurve.Evaluate(t), transform.position.z);
            transform.localScale = Vector3.one + Vector3.one * t;
            // rootBone.angularDrag=0;//deathDrag*100;
            // rootBone.angularDrag=360*(1-currentHorizontalSpeed);                
                foreach (var rb in rigidbody2Ds)
                {
                    if (currentHorizontalSpeed == 0)
                    {
                        rb.isKinematic=true;
                        rb.velocity=Vector2.zero;
                        rb.angularVelocity=0;
                    }
                    else
                    {
                    rb.angularDrag=(1-currentHorizontalSpeed);
                    rb.drag = 1-currentHorizontalSpeed;
                    }
                }
            return;
        }

        dust.Play();
        currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed + (movementDirection.x * Time.deltaTime), movementSpeedRange.x, movementSpeedRange.y);
        var desiredPosition = transform.position + new Vector3(0, movementDirection.y * constantVerticalSpeed * Time.deltaTime);
        if (isInAir)
        {
            var t = Time.time-jumpStartTime;
            var v = jumpCurve.Evaluate(t);
            transform.position = new Vector3(0, jumpStartY+v);
            if (t > jumpCurve.keys[jumpCurve.length-1].time)
            {
                isInAir=false;
                transform.position = Vector2.up*jumpStartY;
            }                
        }
        else
        {
            if (playableArea.OverlapPoint(desiredPosition))
            {
                transform.position = desiredPosition;
                accumulatedDistance += currentHorizontalSpeed;
            }
        }
    }

    public void Die(bool ragdoll, GameObject killer)
    {
        if (isDead) return;

        isDead = true;
        deathStartVelocity=currentHorizontalSpeed;
        deathStartY = transform.position.y;
        // currentHorizontalSpeed=0;
        GetComponent<BoxCollider2D>().enabled=false;
        if (ragdoll)
        {
            ToggleRagdoll(true);
            // foreach (var rb in rblist)
            // {
            //     // var dist = (killer.transform.position - rb.transform.position).sqrMagnitude;
            //     // rb.AddTorque(dist);
            //     rb.AddForce(currentHorizontalSpeed * Vector2.right * Random.Range(0.5f,1));
            // }
            rootBone.AddTorque(-360*currentHorizontalSpeed, ForceMode2D.Impulse);
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
            isInAir=true;
            jumpStartTime=Time.time;
            jumpStartY=transform.position.y;
        }
    }

    List<Rigidbody2D> GetAllRbs()
    {
        List<Rigidbody2D> rbs = new List<Rigidbody2D>();
        Queue<Transform> children = new Queue<Transform>();
        children.Enqueue(transform);
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

    public void ToggleRagdoll(bool ragdoll)
    {
        foreach (var rb in rigidbody2Ds)
        {
            rb.isKinematic = !ragdoll;
        }
    }
}
