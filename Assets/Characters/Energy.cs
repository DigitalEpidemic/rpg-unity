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
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverEnemy(Enemy enemy) {
            if (Input.GetMouseButtonDown(1)) {
                UpdateEnergyPoints();
                UpdateEnergyBar();
            }
        }

        private void UpdateEnergyPoints() {
            float newEnergyPoints = currentEnergyPoints - pointsPerClick;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
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
