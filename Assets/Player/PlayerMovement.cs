using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField] float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

    bool isInDirectMode = false; // TODO Consider making static

    private void Start () {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster> ();
        m_Character = GetComponent<ThirdPersonCharacter> ();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate () {
        // TODO Add to menu
        if (Input.GetKeyDown(KeyCode.G)) {
            isInDirectMode = !isInDirectMode; // Toggle mode
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
        Vector3 m_CamForward = Vector3.Scale (Camera.main.transform.forward, new Vector3 (1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

        m_Character.Move (m_Move, false, false);
    }

    private void ProcessMouseMovement () {
        if (Input.GetMouseButton (0)) {

            switch (cameraRaycaster.layerHit) {
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
            m_Character.Move (playerToClickPoint, false, false);

        } else {
            m_Character.Move (Vector3.zero, false, false);
        }
    }
}

