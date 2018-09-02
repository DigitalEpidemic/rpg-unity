﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbility {

        SelfHealConfig config = null;
        Player player = null;
        AudioSource audioSource = null;

        void Start() {
            player = GetComponent<Player>();
            audioSource = GetComponent<AudioSource>();
        }

        public void SetConfig(SelfHealConfig configToSet) {
            this.config = configToSet;
        }

        public void Use(AbilityUseParams useParams) {
            player.Heal(config.GetExtraHealth());
            PlayParticleEffect();
            audioSource.clip = config.GetAudioClip(); // TODO Move audio to parent class
            audioSource.Play();
        }

        private void PlayParticleEffect() {
            var particlePrefab = config.GetParticlePrefab();
            var prefab = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
            prefab.transform.parent = transform; // Attaching to player
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            float totalDuration = myParticleSystem.main.duration + myParticleSystem.main.startLifetime.constant; // Fixes duration bug
            Destroy(prefab, totalDuration);
        }

    } // SelfHealBehaviour
}