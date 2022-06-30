using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    [SerializeField] private float speed = -3f;
    private float collectionSpeed = 2f;
    private Player _player;

    //IDs for Powerups
    //0 - TrippleShot
    //1 - Speed
    //2 - Shield
    //3 - Ammo
    //4 - Repair
    //5 - Missile
    //6 - Stun
    [SerializeField] private int powerupID;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioPowerUp;
    [SerializeField] private AudioClip audioExplosion;
    private SpriteRenderer renderer;
    [SerializeField] private GameObject plasma;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Player player;
    [SerializeField] private GameObject goPlayer;
    private Vector3 navigationVector;

    // Start is called before the first frame update
    void Start() {
 
        audioSource = GetComponent<AudioSource>();
        audioPowerUp = Resources.Load<AudioClip>("audio_PowerUp");
        audioExplosion = Resources.Load<AudioClip>("audio_explosion");
        renderer = GetComponent<SpriteRenderer>();
        explosion = Resources.Load<GameObject>("Explosion");
        player = GameObject.Find("Player").GetComponent<Player>();
        goPlayer = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update() {

        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if(Mathf.Abs(transform.position.y) >= 8) {
            Destroy(this.gameObject);
        }

        FindPlayer();
        
    }

    private void FindPlayer()  {
        if (player.collect)  {
            navigationVector = goPlayer.transform.position - transform.position;
            transform.Translate(navigationVector * Time.deltaTime * .5f, Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "Player") {
            Player player = other.GetComponent<Player>();

            if(powerupID != 6)  {
                audioSource.PlayOneShot(audioPowerUp);
            } 

            if (renderer != null) {
                renderer.enabled = false;
            }

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

                    case 3:
                        player.AmmoBoostActivate();
                        break;

                    case 4:
                        player.RepairBoostActivate();
                        break;

                    case 5:
                        player.MissileBoostActivate();
                        break;

                    case 6:
                        this.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
                        player.StunActivate();
                        break;

                    default:
                        break;
                }

            }
            Destroy(this.gameObject, 2.0f);
        }

        if(other.tag == "LaserEnemy") {
            audioSource.PlayOneShot(audioExplosion);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            explosion.transform.localScale = new Vector3(.25f, .25f, .25f);
            Instantiate(explosion, transform.position, Quaternion.identity);
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(this.gameObject, 1f);
        }

    }
    
}
