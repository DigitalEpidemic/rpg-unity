using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float damageCaused;
    public float projectileSpeed; // Note other classes can set
    
    void OnTriggerEnter(Collider collider) {
        Component damageableComponent = collider.gameObject.GetComponent(typeof(IDamageable));

        if (damageableComponent) {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
    }
}
