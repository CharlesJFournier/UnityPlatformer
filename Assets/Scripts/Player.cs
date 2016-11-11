using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;
    float gravity;
    float jumpVelocity;
    float velocityXSmoothing;
    Controller2D controller;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        //jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        jumpVelocity = 20;
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }

    void Update()
    {
        bool wallSliding = false;
        int wallDirX = (controller.collisions.left) ? -1 : 1;
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -3)
            {
                velocity.y = -3;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            velocity.x *= 4;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallSliding)
            {
                velocity.y = jumpVelocity-3;
                velocity.x = -wallDirX * 10;
            }
            if (controller.collisions.below)
            {
                velocity.y = jumpVelocity;
            }
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
