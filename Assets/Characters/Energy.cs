﻿using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters {
    public class Energy : MonoBehaviour {

        [SerializeField] RawImage energyBar = null;
        [SerializeField] float maxEnergyPoints = 100f;

        float currentEnergyPoints;

        CameraUI.CameraRaycaster cameraRaycaster;

        void Start() {
            currentEnergyPoints = maxEnergyPoints;
        }

        public bool IsEnergyAvailable(float amount) {
            return amount <= currentEnergyPoints;
        }

        public void ConsumeEnergy(float amount) {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar() {
            // TODO Remove magic numbers
            float xValue = -(EnergyAsPercent() / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        float EnergyAsPercent() {
            return currentEnergyPoints / maxEnergyPoints;
        }

    } // Energy
}
