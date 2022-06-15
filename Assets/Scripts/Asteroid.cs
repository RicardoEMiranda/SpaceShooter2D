using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    private float rotationRate;
    private float xSpeed;
    private float ySpeed;
    private Animator animator;
    private CircleCollider2D collider;
    private SpawnManager spawnManager;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioExplosion;

    // Start is called before the first frame update
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Asteroid");
        rotationRate = Random.Range(0f, 180f);
        xSpeed = Random.Range(-2, 2);
        ySpeed = Random.Range(-2, -5);
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        audioSource = GetComponent<AudioSource>();
        audioExplosion = Resources.Load<AudioClip>("audio_explosion");

    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(0f, 0f, rotationRate * Time.deltaTime, Space.World);
        transform.Translate(new Vector3(0f, 0f, 0f) * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.tag == "Laser") {
            Destroy(other.gameObject);

            animator.SetInteger("AsteroidState", 1);
            collider.enabled = false;
            spawnManager.ContinueSpawning();

            //can set ySpeed and xSpeed to slow down or to zero
            //if decide to with xSpeed = 0f ySpeed = 0f
            //for the explosion's translation speed
            audioSource.PlayOneShot(audioExplosion);
            Destroy(this.gameObject, 2.5f);
        }

        if(other.tag == "Player")  {
            animator.SetInteger("AsteroidState", 1);
            collider.enabled = false;
            audioSource.PlayOneShot(audioExplosion);
            Destroy(this.gameObject, 2.5f);
        }

    }

}
