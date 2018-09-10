using System.Collections;
using UnityEngine;
using RPG.CameraUI; // For mouse events

namespace RPG.Characters {
    public class PlayerControl : MonoBehaviour {
        Character character;
        SpecialAbilities abilities;
        WeaponSystem weaponSystem;

        GameObject updatedTarget;
        public bool isAttacking;

        void Start() {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();

            RegisterForMouseEvents();
        }

        void RegisterForMouseEvents() {
            var cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        void Update() {
            ScanForAbilityKeyDown();

            if (isAttacking && updatedTarget && updatedTarget.transform.hasChanged) {
                character.SetDestination(updatedTarget.transform.position);
            }
        }

        void ScanForAbilityKeyDown() {
            for (int keyIndex = 0; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++) {
                if (Input.GetKeyDown(keyIndex.ToString()) && !abilities.GetRequiresTarget(keyIndex)) {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination) {
            if (Input.GetMouseButton(0)) {
                weaponSystem.StopAttacking();
                character.SetDestination(destination);
            }
        }

        bool IsTargetInRange(GameObject target) {
            updatedTarget = target;
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }

        void OnMouseOverEnemy(EnemyAI enemy) {

            if (Input.GetMouseButtonDown(0) && IsTargetInRange(enemy.gameObject)) {
                weaponSystem.AttackTarget(enemy.gameObject);
            } else if (Input.GetMouseButtonDown(0) && !IsTargetInRange(enemy.gameObject)) {
                StartCoroutine(MoveAndAttack(enemy));
            }

            for (int keyIndex = 0; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++) {
                if (abilities.GetRequiresTarget(keyIndex)) {
                    if (Input.GetKeyDown(keyIndex.ToString()) && IsTargetInRange(enemy.gameObject)) { // Alphanumeric keys
                        abilities.AttemptSpecialAbility(keyIndex, enemy.gameObject);
                    } else if (Input.GetKeyDown(keyIndex.ToString()) && !IsTargetInRange(enemy.gameObject)) {
                        StartCoroutine(MoveAndSpecialAttack(enemy.gameObject));

                        
                    } else if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy.gameObject)) { // Right Click
                        abilities.AttemptSpecialAbility(0, enemy.gameObject);
                    } else if (Input.GetMouseButtonDown(1) && !IsTargetInRange(enemy.gameObject)) {
                        StartCoroutine(MoveAndPowerAttack(enemy));
                    }
                }
            }
        }

        IEnumerator MoveToTarget(GameObject target) {
            updatedTarget = target;
            character.SetDestination(target.transform.position);
            while (!IsTargetInRange(target)) {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        IEnumerator MoveAndAttack(EnemyAI enemy) {
            updatedTarget = enemy.gameObject;
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            weaponSystem.AttackTarget(enemy.gameObject);
        }

        IEnumerator MoveAndPowerAttack(EnemyAI enemy) {
            updatedTarget = enemy.gameObject;
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            abilities.AttemptSpecialAbility(0, enemy.gameObject);
        }

        IEnumerator MoveAndSpecialAttack(GameObject enemy) {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++) {
                if (Input.GetKeyDown(keyIndex.ToString())) {
                    print("Pressed key: " + keyIndex);
                    updatedTarget = enemy;
                    yield return StartCoroutine(MoveToTarget(enemy));
                    abilities.AttemptSpecialAbility(keyIndex, enemy);
                }
            }
        }

    } // PlayerMovement
}
