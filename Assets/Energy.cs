using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Characters {
    public class Energy : MonoBehaviour {

        [SerializeField] RawImage energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float pointsPerClick = 10f;

        float currentEnergyPoints;

        CameraRaycaster cameraRaycaster;

        void Start() {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();

            currentEnergyPoints = maxEnergyPoints;
            cameraRaycaster.notifyRightClickObservers += ProcessRightClick;
        }

        void ProcessRightClick(RaycastHit raycastHit, int layerHit) {
            float newEnergyPoints = currentEnergyPoints - pointsPerClick;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);

            UpdateEnergyBar();
        }

        private void UpdateEnergyBar() {
            float xValue = -(EnergyAsPercent() / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        float EnergyAsPercent() {
            return currentEnergyPoints / maxEnergyPoints;
        }

    } // Energy
}
