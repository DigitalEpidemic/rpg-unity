using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons {
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject {

        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;

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
