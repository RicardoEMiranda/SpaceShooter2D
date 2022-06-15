using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    [SerializeField] private float speed = -3f;
    private Player _player;

    //IDs for Powerups
    //0 - TrippleShot
    //1 - Speed
    //2 - Shield
    [SerializeField] private int powerupID;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioPowerUp;
    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start() {
 
        audioSource = GetComponent<AudioSource>();
        audioPowerUp = Resources.Load<AudioClip>("audio_PowerUp");
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {

        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if(Mathf.Abs(transform.position.y) >= 8) {
            Destroy(this.gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            audioSource.PlayOneShot(audioPowerUp);
            renderer.enabled = false;

            if (player != null) {

                switch (powerupID) {
                    case 0:
                        player.TrippleShotActivate();
                        break;
                    
                    case 1:
                        player.SpeedBoostActivate();
                        break;

                    case 2:
                        player.SheildBoostActivate();
                        break;

                    default:
                        break;
                }

            }
            Destroy(this.gameObject, 2.0f);
        }

    }
    
}
