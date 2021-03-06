﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters {
    public class SpecialAbilities : MonoBehaviour {

        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 1f;
        [SerializeField] AudioClip outOfEnergy;

        float currentEnergyPoints;
        AudioSource audioSource;
        PlayerControl playerControl;

        float EnergyAsPercent() {
            return currentEnergyPoints / maxEnergyPoints;
        }

        void Start() {
            audioSource = GetComponent<AudioSource>();
            playerControl = GetComponent<PlayerControl>();

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

        public bool GetRequiresTarget(int abilityIndex) {
            return abilities[abilityIndex].GetRequiresTarget();
        }

        public void ConsumeEnergy(float amount) {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null) {
            var energyCost = abilities[abilityIndex].GetEnergyCost();

            if (energyCost <= currentEnergyPoints) {
                ConsumeEnergy(energyCost);
                playerControl.isAttacking = true;
                abilities[abilityIndex].Use(target);
            } else {
                audioSource.PlayOneShot(outOfEnergy);
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
            if (energyBar) {
                energyBar.fillAmount = EnergyAsPercent();
            }
        }

    } // Energy
}
