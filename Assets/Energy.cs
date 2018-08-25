using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters {
    public class Energy : MonoBehaviour {

        [SerializeField] RawImage energyBar;
        [SerializeField] float maxEnergyPoints = 100f;

        float currentEnergyPoints;

        void Start() {
            currentEnergyPoints = maxEnergyPoints;
        }

        // Update is called once per frame
        void Update() {

        }

    } // Energy
}
