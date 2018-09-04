using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(HealthSystem))]
    public class EnemyAI : MonoBehaviour {

        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2f;

        PlayerControl player;
        Character character;

        float currentWeaponRange;
        float distanceToPlayer;
        int nextWaypointIndex;

        enum State { idle, patrolling, attacking, chasing }
        State state = State.idle;

        void Start() {
            player = FindObjectOfType<PlayerControl>();
            character = GetComponent<Character>();
        }

        void Update() {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>(); // No performance issue
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

            if (distanceToPlayer > chaseRadius && state != State.patrolling) {
                StopAllCoroutines();
                StartCoroutine(Patrol());
            }

            if (distanceToPlayer <= chaseRadius && state != State.chasing) {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }

            if (distanceToPlayer <= currentWeaponRange && state != State.attacking) {
                StopAllCoroutines();
                state = State.attacking;
            }
        }

        IEnumerator Patrol() {
            state = State.patrolling;

            while (patrolPath != null) {
                Vector3 nextWayPointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWayPointPos);
                CycleWaypointWhenClose(nextWayPointPos);
                yield return new WaitForSeconds(0.5f); // TODO Parameterize
            }
        }

        void CycleWaypointWhenClose(Vector3 nextWayPointPos) {
            if (Vector3.Distance(transform.position, nextWayPointPos) <= waypointTolerance) {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
            }
        }

        IEnumerator ChasePlayer() {
            state = State.chasing;
            while (distanceToPlayer >= currentWeaponRange) {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
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
