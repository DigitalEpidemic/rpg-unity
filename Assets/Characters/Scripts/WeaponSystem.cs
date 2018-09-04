using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters {
    public class WeaponSystem : MonoBehaviour {

        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig;

        Animator animator;
        Character character;

        GameObject target;
        GameObject weaponObject;
        float lastHitTime;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        // Use this for initialization
        void Start() {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();

            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();
        }

        // Update is called once per frame
        void Update() {
            // TODO Check continuously if character should be attacking
        }

        public void PutWeaponInHand(WeaponConfig weaponToUse) {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject); // Empty hands
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

        public void AttackTarget(GameObject targetToAttack) {
            target = targetToAttack;
            StartCoroutine(AttackTargetRepeatedly());
        }

        IEnumerator AttackTargetRepeatedly() {
            bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;

            while (attackerStillAlive && targetStillAlive) {
                float weaponHitRate = currentWeaponConfig.GetMinTimeBetweenHits();
                float timeToWait = weaponHitRate * character.GetAnimSpeedMultiplier();

                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;

                if (isTimeToHitAgain) {
                    AttackTargetOnce();
                    lastHitTime = Time.time;
                }

                yield return new WaitForSeconds(timeToWait);
            }
        }

        private void AttackTargetOnce() {
            transform.LookAt(target.transform);
            animator.SetTrigger(ATTACK_TRIGGER);
            float damageDelay = 1.0f; // TODO Get from weapon
            SetAttackAnimation();
            StartCoroutine(DamageAfterDelay(damageDelay));
        }

        IEnumerator DamageAfterDelay(float delay) {
            yield return new WaitForSecondsRealtime(delay);
            target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
        }

        public WeaponConfig GetCurrentWeapon() {
            return currentWeaponConfig;
        }

        private void SetAttackAnimation() {
            if (!character.GetOverrideController()) {
                Debug.Break();
                Debug.LogAssertion("Please provide: " + gameObject + " with an Animator Override Controller!");
            }

            var animatorOverrideController = character.GetOverrideController();

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
        
        // TODO Use coroutines for move and attack
        private void AttackTarget() {
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits()) {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage() {
            return baseDamage + currentWeaponConfig.GetAdditionalDamage();
        }

    } // WeaponSystem
}
