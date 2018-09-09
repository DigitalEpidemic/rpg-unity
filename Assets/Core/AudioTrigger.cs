using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Audio trigger with optional captions
public class AudioTrigger : MonoBehaviour {

    [SerializeField] AudioClip clip;
    [SerializeField] float playerDistanceThreshold = 5f;
    [SerializeField] bool isOneTimeOnly = true;

    AudioTrigger[] audioTriggers;

    Text captionUI;
    [TextArea] [SerializeField] string captionText;
    [SerializeField] float captionLengthSeconds = 5f;

    bool hasPlayed = false;
    AudioSource audioSource;
    GameObject player; // Will only trigger on distance to player

    void Start() {
        if (gameObject.GetComponent<AudioSource>()) {
            audioSource = GetComponent<AudioSource>();
        } else {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioTriggers = FindObjectsOfType(typeof(AudioTrigger)) as AudioTrigger[];

        audioSource.playOnAwake = false;
        audioSource.clip = clip;

        player = GameObject.FindWithTag("Player");
        captionUI = GameObject.FindGameObjectWithTag("CaptionUI").GetComponent<Text>();
    }

    void Update() {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= playerDistanceThreshold) {
            RequestPlayAudioClip();
        }
    }

    void RequestPlayAudioClip() {
        if (isOneTimeOnly && hasPlayed) {
            return;
        } else if (audioSource.isPlaying == false) {
            foreach (AudioTrigger trigger in audioTriggers) {
                trigger.StopAllCoroutines();
            }
            captionUI.gameObject.SetActive(true);
            StartCoroutine(SetupCaptionText());
            audioSource.Play();
            hasPlayed = true;
        }

    }

    IEnumerator SetupCaptionText() {
        if (captionUI != null) {
            captionUI.text = captionText;
            yield return new WaitForSecondsRealtime(captionLengthSeconds);
            captionUI.gameObject.SetActive(false);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(0, 255f, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, playerDistanceThreshold);
    }

} // AudioTrigger
