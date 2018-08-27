﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Consider re-wiring
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters {
    public class Enemy : MonoBehaviour, IDamageable {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float chaseRadius = 6f;

        [SerializeField] float attackRadius = 4f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float secondsBetweenShots = 0.5f;

        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0f, 1f, 0f);

        bool isAttacking = false;

        float currentHealthPoints;

        AICharacterControl aiCharacterControl = null;
        GameObject player = null;


        public float healthAsPercentage {
            get {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        public void TakeDamage(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0) {
                Destroy(gameObject);
            }
        }

        void Start() {
            aiCharacterControl = GetComponent<AICharacterControl>();
            player = GameObject.FindGameObjectWithTag("Player");
            currentHealthPoints = maxHealthPoints;
        }

        void Update() {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= attackRadius && !isAttacking) {
                isAttacking = true;
                InvokeRepeating("FireProjectile", 0f, secondsBetweenShots); // TODO Switch to coroutines
            }

            if (distanceToPlayer > attackRadius) {
                isAttacking = false;
                CancelInvoke();
            }

            if (distanceToPlayer <= chaseRadius) {
                aiCharacterControl.SetTarget(player.transform);
            } else {
                aiCharacterControl.SetTarget(transform);
            }
        }

        // TODO Separate out character firing logic
        void FireProjectile() {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        }

        void OnDrawGizmos() {
            // Draw attack sphere
            Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            // Draw chase sphere
            Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }


    } // Enemy
}
