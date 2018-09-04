﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters {
    public class AreaEffectBehaviour : AbilityBehaviour {

        public override void Use(GameObject target) {
            PlayAbilitySound();
            DealRadialDamage();
            PlayParticleEffect();
        }

        private void DealRadialDamage() {
            // Static sphere cast for target
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, (config as AreaEffectConfig).GetRadius(), Vector3.up, (config as AreaEffectConfig).GetRadius());

            foreach (RaycastHit hit in hits) {
                var damageable = hit.collider.gameObject.GetComponent<HealthSystem>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerMovement>();

                if (damageable != null && !hitPlayer) {
                    float damageToDeal = (config as AreaEffectConfig).GetDamageToEachTarget();
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }

    } // AreaEffectBehaviour
}
