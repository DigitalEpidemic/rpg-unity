using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters {
    public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility {

        AreaEffectConfig config;

        public void SetConfig(AreaEffectConfig configToSet) {
            this.config = configToSet;
        }

        void Start() {
            print("AreaEffectBehaviour attached to: " + gameObject.name);
        }

        public void Use(AbilityUseParams useParams) {
            DealRadialDamage(useParams);
            PlayParticleEffect();
        }

        private void PlayParticleEffect() {
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            // TODO Decide if particle system attaches to player
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }

        private void DealRadialDamage(AbilityUseParams useParams) {
            print("AreaEffectBehaviour used by: " + gameObject.name);

            // Static sphere cast for target
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, config.GetRadius(), Vector3.up, config.GetRadius());

            foreach (RaycastHit hit in hits) {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();

                if (damageable != null) {
                    float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget(); // TODO Is this okay?
                    damageable.AdjustHealth(damageToDeal);
                }
            }
        }
    } // AreaEffectBehaviour
}
