﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters {
    public class SpecialAbilities : MonoBehaviour {

        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 1f;
        // TODO Add outOfEnergy sound

        float currentEnergyPoints;
        AudioSource audioSource;

        float EnergyAsPercent() {
            return currentEnergyPoints / maxEnergyPoints;
        }

        void Start() {
            audioSource = GetComponent<AudioSource>();

            currentEnergyPoints = maxEnergyPoints;
            AttachInitialAbilities();
            UpdateEnergyBar();
        }

        void Update() {
            if (currentEnergyPoints < maxEnergyPoints) {
                AddEnergyPoints();
                UpdateEnergyBar();
            }
        }

        public int GetNumberOfAbilities() {
            return abilities.Length;
        }

        public void ConsumeEnergy(float amount) {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        public void AttemptSpecialAbility(int abilityIndex) {
            var energyComponent = GetComponent<SpecialAbilities>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();

            if (energyCost <= currentEnergyPoints) {
                ConsumeEnergy(energyCost);
                print("Using special ability " + abilityIndex); // TODO Make work
            } else {
                // TODO Play outOfEnergy sound
            }
        }

        void AttachInitialAbilities() {
            for (int abilitiesIndex = 0; abilitiesIndex < abilities.Length; abilitiesIndex++) {
                abilities[abilitiesIndex].AttachAbilityTo(gameObject);
            }
        }

        void AddEnergyPoints() {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }

        private void UpdateEnergyBar() {
            energyBar.fillAmount = EnergyAsPercent();
        }
        
    } // Energy
}
