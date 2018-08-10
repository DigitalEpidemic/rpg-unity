using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
public class Enemy : MonoBehaviour {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attackRadius = 4f;

    float currentHealthPoints = 100f;

    AICharacterControl aiCharacterControl = null;
    GameObject player = null;


    public float healthAsPercentage {
        get {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    void Start () {
        aiCharacterControl = GetComponent<AICharacterControl> ();
        player = GameObject.FindGameObjectWithTag ("Player");
    }

    void Update () {
        float distanceToPlayer = Vector3.Distance (player.transform.position, transform.position);

        if (distanceToPlayer <= attackRadius) {
            aiCharacterControl.SetTarget (player.transform);
        } else {
            aiCharacterControl.SetTarget (transform);
        }
    }

} // Enemy
