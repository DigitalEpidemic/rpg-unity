using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    [ExecuteInEditMode]
    public class WeaponPickupPoint : MonoBehaviour {

        [SerializeField] Weapon weaponConfig;
        [SerializeField] AudioClip pickUpSFX;

        AudioSource audioSource;

        // Use this for initialization
        void Start() {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update() {
            if (!Application.isPlaying) {
                DestroyChildren();
                InstantiateWeapon();
            }
        }

        void DestroyChildren() {
            foreach(Transform child in transform) {
                DestroyImmediate(child.gameObject);
            }
        }

        void InstantiateWeapon() {
            var weapon = weaponConfig.GetWeaponPrefab();
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, gameObject.transform);
        }

        void OnTriggerEnter() {
            FindObjectOfType<PlayerMovement>().PutWeaponInHand(weaponConfig); // Possibly slows down performance
            audioSource.PlayOneShot(pickUpSFX);
        }
    } // WeaponPickupPoint
}