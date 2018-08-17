using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D targetCursor = null;
    [SerializeField] Texture2D unknownCursor = null;

    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

    // TODO Solve conflict between serialize and const
    [SerializeField] const int walkableLayerNumber = 9;
    [SerializeField] const int enemyLayerNumber = 10;

    CameraRaycaster cameraRaycaster;

    // Use this for initialization
    void Start() {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged; // Registering
    }

    // Called after all Update functions have been called
    void OnLayerChanged(int newLayer) {
        switch (newLayer) {
            case walkableLayerNumber:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case enemyLayerNumber:
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                return;
        }

    }

    // TODO Consider de-registering OnLayerChanged on exit

} // CursorAffordance
