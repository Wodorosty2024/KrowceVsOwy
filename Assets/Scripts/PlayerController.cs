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

    public float accumulatedDistance = 0;
    public bool isInAir = false;
    public bool isDead = false;
    public AnimationCurve jumpCurve;

    public BoxCollider2D playableArea;

    public ParticleSystem dust; 

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    float jumpStartTime;
    float jumpStartY;
    void Update()
    {
        if (isDead) return;

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

    public void Die()
    {
        isDead = true;
        currentHorizontalSpeed=0;
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
}
