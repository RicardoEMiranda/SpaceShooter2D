using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    // Start is called before the first frame update
    void Start()  {
        audioSource = GetComponent<AudioSource>();
        clip = Resources.Load<AudioClip>("audio_explosion");
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update() {
        Destroy(this.gameObject, 2.8f);
    }
}
