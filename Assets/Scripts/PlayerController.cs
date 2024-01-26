using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 movementDirection;
    public Vector2 movementSpeedRange = new Vector2(.5f, 2);
    public float accelerationSpeed = 0.5f;
    public float currentHorizontalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed+(movementDirection.x*Time.deltaTime), movementSpeedRange.x, movementSpeedRange.y);

        transform.position += new Vector3(currentHorizontalSpeed, (movementDirection.y * currentHorizontalSpeed));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>()*accelerationSpeed;
    }
}
