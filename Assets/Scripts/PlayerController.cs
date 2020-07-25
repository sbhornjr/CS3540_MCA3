using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpHeight = 10f;
    public float gravity = 9.81f;
    public float airControl = 10f;

    CharacterController controller;
    Vector3 input, moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.isGameOver)
        {
            float moveH = Input.GetAxis("Horizontal");
            float moveV = Input.GetAxis("Vertical");

            input = (transform.right * moveH + transform.forward * moveV).normalized;

            if (controller.isGrounded)
            {
                moveDirection = input;

                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                }
                else
                {
                    moveDirection.y = 0.0f;
                }
            }
            else
            {
                input.y = moveDirection.y;
                moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
            }

            moveDirection.y -= gravity * Time.deltaTime;

            controller.Move(moveDirection * Time.deltaTime * speed);
        }
    }
}
