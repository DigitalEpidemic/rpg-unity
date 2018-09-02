﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public class SelfHealBehaviour : AbilityBehaviour {

        Player player = null;
        AudioSource audioSource = null;

        void Start() {
            player = GetComponent<Player>();
            audioSource = GetComponent<AudioSource>();
        }

        public override void Use(AbilityUseParams useParams) {
            player.Heal((config as SelfHealConfig).GetExtraHealth());
            PlayParticleEffect();
            audioSource.clip = config.GetAudioClip(); // TODO Move audio to parent class
            audioSource.Play();
        }
    } // SelfHealBehaviour
}