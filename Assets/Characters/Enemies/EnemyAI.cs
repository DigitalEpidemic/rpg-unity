using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour {

        [SerializeField] float chaseRadius = 6f;

        PlayerMovement player;

        bool isAttacking = false; // TODO Richer state
        float currentWeaponRange;

        void Start() {
            player = FindObjectOfType<PlayerMovement>();
        }

        void Update() {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>(); // Possible performance issue
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }

        void OnDrawGizmos() {
            // Draw attack sphere
            Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            // Draw chase sphere
            Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }

    } // EnemyAI
}
