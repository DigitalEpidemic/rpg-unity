﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float chaseRadius = 6f;

    [SerializeField] float attackRadius = 4f;
    [SerializeField] float damagePerShot = 9f;
    [SerializeField] float secondsBetweenShots = 0.5f;

    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSocket;

    bool isAttacking = false;

    float currentHealthPoints = 100f;

    AICharacterControl aiCharacterControl = null;
    GameObject player = null;


    public float healthAsPercentage {
        get {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    public void TakeDamage(float damage) {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }

    void Start() {
        aiCharacterControl = GetComponent<AICharacterControl>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= attackRadius && !isAttacking) {
            isAttacking = true;
            InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShots); // TODO Switch to coroutines
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

    void SpawnProjectile() {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(damagePerShot);

        Vector3 unitVectorToPlayer = (player.transform.position - projectileSocket.transform.position).normalized;
        float projectileSpeed = projectileComponent.projectileSpeed;
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
