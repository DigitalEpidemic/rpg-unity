using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField] float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

    bool isInDirectMode = false;

    private void Start () {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster> ();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter> ();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate () {
        // TODO Add to menu
        if (Input.GetKeyDown(KeyCode.G)) {
            isInDirectMode = !isInDirectMode; // Toggle mode
            currentClickTarget = transform.position; // Clear the click target
        }

        if (isInDirectMode) {
            ProcessDirectMovement();
        } else {
            ProcessMouseMovement ();
        }
    }

    private void ProcessDirectMovement () {
        // Read inputs
        float h = Input.GetAxis ("Horizontal"); // CrossPlatformInputManager for mobile
        float v = Input.GetAxis ("Vertical");

        // Calculate camera relative direction to move:
        Vector3 cameraForward = Vector3.Scale (Camera.main.transform.forward, new Vector3 (1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move (movement, false, false);
    }

    private void ProcessMouseMovement () {
        if (Input.GetMouseButton (0)) {

            switch (cameraRaycaster.currentLayerHit) {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;  // So not set in default case
                    break;
                case Layer.Enemy:
                    print ("Not moving to enemy.");
                    break;
                default:
                    print ("Unexpected layer found!");
                    return;
            }

        }

        Vector3 playerToClickPoint = currentClickTarget - transform.position;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius) {
            thirdPersonCharacter.Move (playerToClickPoint, false, false);

        } else {
            thirdPersonCharacter.Move (Vector3.zero, false, false);
        }
    }
}

