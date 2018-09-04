﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

// TODO Consider re-wiring
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters {
    public class Player : MonoBehaviour {

        
        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon currentWeaponConfig;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [Range(0.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticles;

        // Temporarily serialized for debugging
        

        const string ATTACK_TRIGGER = "Attack";
        
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        Enemy enemy;
        
        Animator animator;

        SpecialAbilities abilities;
        
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;

        GameObject weaponObject;

        

        void Start() {
            abilities = GetComponent<SpecialAbilities>();

            RegisterForMouseClick();
            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();
        }

        public void PutWeaponInHand(Weapon weaponToUse) {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject); // Empty hands
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

        void Update() {
            var healthPercentage = GetComponent<HealthSystem>().healthAsPercentage;
            if (healthPercentage > Mathf.Epsilon) {
                ScanForAbilityKeyDown();
            }
        }

        private void ScanForAbilityKeyDown() {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++) {
                if (Input.GetKeyDown(keyIndex.ToString())) {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }

        private void SetAttackAnimation() {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();
        }

        private GameObject RequestDominantHand() {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand script found on Player, please add one!");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts found on Player, please remove one!");
            return dominantHands[0].gameObject;
        }

        private void RegisterForMouseClick() {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverEnemy(Enemy enemyToSet) {
            this.enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject)) {
                AttackTarget();
            } else if (Input.GetMouseButtonDown(1)) {
                abilities.AttemptSpecialAbility(0);
            }
        }
        
        private void AttackTarget() {
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits()) {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage() {
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
            float damageBeforeCritical = baseDamage + currentWeaponConfig.GetAdditionalDamage();

            if (isCriticalHit) {
                criticalHitParticles.Play();
                return damageBeforeCritical * criticalHitMultiplier;
            } else {
                return damageBeforeCritical;
            }
        }

        private bool IsTargetInRange(GameObject target) {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= currentWeaponConfig.GetMaxAttackRange();
        }

    } // Player
}
