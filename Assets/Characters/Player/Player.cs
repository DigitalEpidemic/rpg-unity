using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

// TODO Consider re-wiring
using RPG.CameraUI;
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters {
    public class Player : MonoBehaviour, IDamageable {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon weaponInUse = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;
        [Range(0.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticles = null;

        // Temporarily serialized for dubbing
        [SerializeField] AbilityConfig[] abilities;

        const string ATTACK_TRIGGER = "Attack";
        const string DEATH_TRIGGER = "Death";
        
        Enemy enemy = null;

        AudioSource audioSource = null;
        Animator animator = null;
        float currentHealthPoints = 0f;
        CameraRaycaster cameraRaycaster = null;
        float lastHitTime = 0f;

        public float healthAsPercentage {
            get {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        void Start() {
            audioSource = GetComponent<AudioSource>();

            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupRuntimeAnimator();
            AttachInitialAbilities();
        }

        private void AttachInitialAbilities() {
            for (int abilitiesIndex = 0; abilitiesIndex < abilities.Length; abilitiesIndex++) {
                abilities[abilitiesIndex].AttachComponentTo(gameObject);
            }
        }

        void Update() {
            if (healthAsPercentage > Mathf.Epsilon) {
                ScanForAbilityKeyDown();
            }
        }

        private void ScanForAbilityKeyDown() {
            for (int keyIndex = 1; keyIndex < abilities.Length; keyIndex++) {
                if (Input.GetKeyDown(keyIndex.ToString())) {
                    AttemptSpecialAbility(keyIndex);
                }
            }
        }

        public void TakeDamage(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            audioSource.clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.Play();

            if (currentHealthPoints <= 0) {
                StartCoroutine(KillPlayer());
            }
        }

        public void Heal(float points) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0f, maxHealthPoints);
        }

        IEnumerator KillPlayer() {
            animator.SetTrigger(DEATH_TRIGGER);

            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            yield return new WaitForSecondsRealtime(audioSource.clip.length);

            SceneManager.LoadScene(0);
        }

        private void SetCurrentMaxHealth() {
            currentHealthPoints = maxHealthPoints;
        }

        private void SetupRuntimeAnimator() {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); // TODO Remove constant
        }

        private void PutWeaponInHand() {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
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
                AttemptSpecialAbility(0);
            }
        }

        private void AttemptSpecialAbility(int abilityIndex) {
            var energyComponent = GetComponent<Energy>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();

            if (energyComponent.IsEnergyAvailable(energyCost)) { // TODO Read from config
                energyComponent.ConsumeEnergy(energyCost);
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                abilities[abilityIndex].Use(abilityParams);
            }
        }

        private void AttackTarget() {
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits()) {
                animator.SetTrigger(ATTACK_TRIGGER);
                enemy.TakeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage() {
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
            float damageBeforeCritical = baseDamage + weaponInUse.GetAdditionalDamage();

            if (isCriticalHit) {
                criticalHitParticles.Play();
                return damageBeforeCritical * criticalHitMultiplier;
            } else {
                return damageBeforeCritical;
            }
        }

        private bool IsTargetInRange(GameObject target) {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }

    } // Player
}
