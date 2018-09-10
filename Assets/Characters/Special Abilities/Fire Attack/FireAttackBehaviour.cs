using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public class FireAttackBehaviour : AbilityBehaviour {

        public override void Use(GameObject target) {
            PlayAbilitySound();
            PlayAbilityAnimation();
            DealDamage(target);
            PlayFireBlastEffect(); // TODO Move to parent class
        }

        private void DealDamage(GameObject target) {
            float damageToDeal = (config as FireAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
        }

    } // FireAttackBehaviour
}
