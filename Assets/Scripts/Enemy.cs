using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private BoxCollider2D boxCollider;
    [SerializeField] private float _ySpeed = 3.5f;
    public UIManager _UIManager;

    private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioExplosion;
    [SerializeField] private AudioClip audioLaser;
    [SerializeField] private GameObject enemyLaser;
    private float fireDelay;
    private float fireTime;
    private bool canFire = true;
    private float firePosition;

    // Start is called before the first frame update
    void Start() {
        _UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();  
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();    

        audioSource = GetComponent<AudioSource>();
        audioExplosion = Resources.Load<AudioClip>("audio_explosion");
        audioLaser = Resources.Load<AudioClip>("audio_laser");

        enemyLaser = Resources.Load<GameObject>("LaserEnemy");
        fireTime = 0;
        fireDelay = 3f;
        firePosition = Random.Range(2f, 4.5f);

        if (audioExplosion == null)
        {
            Debug.LogError("Audio Clip is NULL.");
        }

        if (audioSource == null)
        {
            Debug.LogError("Audio Source is NULL.");
        }
    }

    // Update is called once per frame
    void Update() {

        //move down (-x direction) at 4 units/second
        transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * _ySpeed);

        FireLaser();

        //if at bottom of screen, respawn at top
        //at a new random x position
        //with new random x direction movement
        if (transform.position.y < -7) {
            //     float _startXpos = Random.Range(-8f, 8f);
            //    transform.position = new Vector3(_startXpos, 7, 0);
            Destroy(this.gameObject);
        }

    }

    private void FireLaser() {

        if(Time.time > fireTime)  {
            canFire = true;
        }

        if (Time.time > fireTime && (transform.position.y <= firePosition) && canFire) {
            Instantiate(enemyLaser, transform.position, Quaternion.identity);
            audioSource.PlayOneShot(audioLaser);
            canFire = false;
            fireDelay = Random.Range(2f, 7f);
            fireTime = Time.time + fireDelay;
        }

    }



    private void OnTriggerEnter2D(Collider2D other) {
        
        //if the collider is the laser, destroy the laser FIRST so this
        //Enemy script can continue running and THEN, destroy self (enemy)

        //if collider is the player, then destry self (Enemy) and call
        //player's damage method
        if (other.tag == "Laser") {

            Destroy(other);

            //before destroying enemy object (self), add 10 points
            //to player score in the UIManager
            if (_UIManager != null) {
                _UIManager.UpdatePlayerScore();
            } else if (_UIManager == null)  {
                Debug.LogError("No UI Manager Object Found.");
            }

            //transition to enemy exploding animation 
            //after a delay
            animator.SetInteger("EnemyState", 1);
            _ySpeed = _ySpeed / 2;
            boxCollider.enabled = false;

            audioSource.PlayOneShot(audioExplosion);
            Destroy(this.gameObject, 2.8f);
            
        } else if (other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();
            
            if (player != null) {
                player.Damage();

                //call UIManager.UpdateLivesUI() and update 
                //UI for number of Lives
                _UIManager.UpdateLivesUI(player._lives);
            }

            //transition to enemy exploding animation 
            //after a delay
            animator.SetInteger("EnemyState", 1);
            _ySpeed = _ySpeed / 2;
            boxCollider.enabled=false;

            audioSource.PlayOneShot(audioExplosion);
            Destroy(this.gameObject, 2.8f);
        }

    }
}
