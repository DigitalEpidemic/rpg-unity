using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Fire Attack"))]
    public class FireAttackConfig : AbilityConfig {

        [Header("Fire Attack Specific")]
        [SerializeField] float extraDamage = 10f;

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo) {
            return objectToAttachTo.AddComponent<FireAttackBehaviour>();
        }

        public float GetExtraDamage() {
            return extraDamage;
        }

    } // FireAttackConfig
}
