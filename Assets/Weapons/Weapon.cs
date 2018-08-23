﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons {
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject {

        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float maxAttackRange = 2f;

        public float GetMinTimeBetweenHits() {
            // TODO Consider taking animation time into account
            return minTimeBetweenHits;
        }

        public float GetMaxAttackRange() {
            return maxAttackRange;
        }

        public GameObject GetWeaponPrefab() {
            return weaponPrefab;
        }

        public AnimationClip GetAttackAnimClip() {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        // Removes bugs/crashes from asset packs
        private void RemoveAnimationEvents() {
            attackAnimation.events = new AnimationEvent[0];
        }

    } // Weapon
}
