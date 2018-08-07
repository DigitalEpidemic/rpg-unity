using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 5f;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;

    bool isInDirectMode = false;

    private void Start () {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster> ();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter> ();
        currentDestination = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate () {
        // TODO Add to menu
        if (Input.GetKeyDown(KeyCode.G)) {
            isInDirectMode = !isInDirectMode; // Toggle mode
            currentDestination = transform.position; // Clear the click target
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

            clickPoint = cameraRaycaster.hit.point;
            switch (cameraRaycaster.currentLayerHit) {
                case Layer.Walkable:
                    currentDestination = ShortDestination (clickPoint, walkMoveStopRadius);
                    break;
                case Layer.Enemy:
                    currentDestination = ShortDestination (clickPoint, attackMoveStopRadius);
                    break;
                default:
                    print ("Unexpected layer found!");
                    return;
            }

        }

        WalkToDestination ();
    }

    private void WalkToDestination () {
        Vector3 playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= 0) {
            thirdPersonCharacter.Move (playerToClickPoint, false, false);

        } else {
            thirdPersonCharacter.Move (Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening) {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    void OnDrawGizmos () {
        // Draw movement line
        Gizmos.color = Color.black;
        Gizmos.DrawLine (transform.position, currentDestination);
        Gizmos.DrawSphere (currentDestination, 0.1f);
        Gizmos.DrawSphere (clickPoint, 0.15f);

        // Draw attack sphere
        Gizmos.color = new Color (255f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere (transform.position, attackMoveStopRadius);
    }

} // PlayerMovement
