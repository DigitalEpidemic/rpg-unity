using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

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
            print("AreaEffectBehaviour used by: " + gameObject.name);

            // Static sphere cast for target
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, config.GetRadius(), Vector3.up, config.GetRadius());

            foreach (RaycastHit hit in hits) {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();

                if (damageable != null) {
                    float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget(); // TODO Is this okay?
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }

    } // AreaEffectBehaviour
}
