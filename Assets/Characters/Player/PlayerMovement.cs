using UnityEngine;
using RPG.CameraUI; // For mouse events

namespace RPG.Characters {
    public class PlayerMovement : MonoBehaviour {
        
        [Range(0.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticles;
        
        Enemy enemy;
        Character character;
        SpecialAbilities abilities;
        CameraRaycaster cameraRaycaster;
        WeaponSystem weaponSystem;

        
        
        void Start() {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();

            RegisterForMouseEvents();
            
        }

        void RegisterForMouseEvents() {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        void Update() {
            ScanForAbilityKeyDown();
        }

        void ScanForAbilityKeyDown() {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++) {
                if (Input.GetKeyDown(keyIndex.ToString())) {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination) {
            if (Input.GetMouseButton(0)) {
                character.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemyToSet) {
            this.enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject)) {
                weaponSystem.AttackTarget(enemy.gameObject);
            } else if (Input.GetMouseButtonDown(1)) {
                abilities.AttemptSpecialAbility(0);
            }
        }
        
        private bool IsTargetInRange(GameObject target) {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }

    } // PlayerMovement
}
