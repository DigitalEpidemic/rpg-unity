using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public abstract class SpecialAbilityConfig : ScriptableObject {

        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;

        protected ISpecialAbility behaviour; // Only methods or derived classes can access

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

        public void Use() {
            behaviour.Use();
        }

    } // SpecialAbilityConfig
}
