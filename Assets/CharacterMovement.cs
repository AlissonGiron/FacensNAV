using UnityEngine;
using TouchControlsKit;

public class CharacterMovement : MonoBehaviour
{
    Transform myTransform, cameraTransform;
    CharacterController controller;
    bool jump, prevGrounded, isPorjectileCube;

    // Start is called before the first frame update
    void Awake()
    {
        myTransform = transform;
        cameraTransform = Camera.main.transform;
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        Vector2 move = TCKInput.GetAxis("Joystick");
        PlayerMovement(move.x, move.y);
    }

    // PlayerMovement
    private void PlayerMovement(float horizontal, float vertical)
    {
        Vector3 moveDirection = myTransform.forward * vertical;
        moveDirection += myTransform.right * horizontal;
        moveDirection *= 7f;
        controller.Move(moveDirection * Time.fixedDeltaTime);
    }
}
