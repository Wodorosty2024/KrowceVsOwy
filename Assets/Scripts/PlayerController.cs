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

    public float accumulatedDistance=0;

    public BoxCollider2D playableArea;

    // Start is called before the first frame update
    void Start()
    {
        instance=this;
    }

    // Update is called once per frame
    void Update()
    {
        currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed+(movementDirection.x*Time.deltaTime), movementSpeedRange.x, movementSpeedRange.y);

        var desiredPosition = transform.position + new Vector3(0, movementDirection.y*constantVerticalSpeed*Time.deltaTime);
        if (playableArea.OverlapPoint(desiredPosition))
        {
            transform.position=desiredPosition;
            accumulatedDistance+=currentHorizontalSpeed*Time.deltaTime;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>()*accelerationSpeed;
    }
}
