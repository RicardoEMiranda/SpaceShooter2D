using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidBody;
    [SerializeField] private float _ySpeed = 3.5f;
    public UIManager _UIManager;

    private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioExplosion;
    [SerializeField] private AudioClip audioLaser;
    [SerializeField] private GameObject enemyLaser;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject[] lasers;
    private int shieldRoll;
    private bool shieldActive;


    private int kamikazeRoll;
    [SerializeField] private bool kamikazeEnemy;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private Vector3 navigationVector;
    [SerializeField] private GameObject[] powerupArray;
    [SerializeField] private int noOfPowerups;

    private float fireDelay;
    private float fireTime;
    private bool canFire = true;
    private float firePosition;
    private float randomSpeed;
    private float xSpeed;
    private float shootPowerupTimer = 0;
    private bool canShootPowerup = true;
    public bool canEvade = true;
    public bool inRange;
    public float evasionFactor = 1;
    public float evasionSpeed = 0;
    public float maxEvasionSpeed = 2.5f;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start() {
        _UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();  
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();    
        rigidBody = GetComponent<Rigidbody2D>();
        explosion = Resources.Load<GameObject>("Explosion");
        shield = transform.GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        if(GameObject.FindGameObjectsWithTag("Powerup") != null) {
            powerupArray = GameObject.FindGameObjectsWithTag("Powerup");
            noOfPowerups = powerupArray.Length;
        }


        audioSource = GetComponent<AudioSource>();
        audioExplosion = Resources.Load<AudioClip>("audio_explosion");
        audioLaser = Resources.Load<AudioClip>("audio_laser");

        enemyLaser = Resources.Load<GameObject>("LaserEnemy");
        fireTime = 0;
        fireDelay = 3f;
        firePosition = Random.Range(2f, 4.5f);
        randomSpeed = Random.Range(-1f, 1f);

        if (audioExplosion == null)
        {
            Debug.LogError("Audio Clip is NULL.");
        }

        if (audioSource == null)
        {
            Debug.LogError("Audio Source is NULL.");
        }

        shieldRoll = Random.Range(0, 3);
        if(shieldRoll == 1) {
            shield.SetActive(true);
            shieldActive = true;
        }

        kamikazeRoll = Random.Range(0, 4);
        if(kamikazeRoll == 2) {
            kamikazeEnemy = true;
        }

        playerPosition = player.transform.position;
        navigationVector = playerPosition - this.transform.position;

    }

    // Update is called once per frame
    void Update() {

        //move down (-x direction) at 4 units/second
        int roll = Random.Range(0, 6);
        if(roll == 0)  {
            xSpeed = randomSpeed;
        } else {
            xSpeed = 0;
        }

        TrackPowerups();
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

    void LateUpdate()  {
        TrackPlayer();
        MoveEnemy();
    }

    private void TrackPlayer ()  {

        if(canEvade)  {
            if(player != null)  {
                float delta = player.transform.position.x - transform.position.x;
                float evasionSpeed = Mathf.Pow(2.718f, -(delta * delta)) * 5;

                if(delta >= 0)  {
                    evasionSpeed = - Mathf.Pow(2.718f, -(delta * delta)) * 5; ;
                }

                transform.Translate(new Vector3(evasionSpeed, -_ySpeed, 0) * Time.deltaTime);

                //Debug.Log(evasionSpeed);
            }
        }
  
    }


    private void TrackPowerups()  {
       
        for (int i = 0; i < noOfPowerups; i++)  {
            if (powerupArray[i] != null)  {
                float xPositionPowerup = powerupArray[i].GetComponent<Transform>().position.x;
                float xDelta = Mathf.Abs(xPositionPowerup - transform.position.x);
                if (xDelta <= .5f && canShootPowerup)  {
                    //Debug.Log("Within Aim Field");
                    ShootPowerup();
                }
            }
        }
    }

    private void ShootPowerup() {
        Instantiate(enemyLaser, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(audioLaser);
        StartCoroutine(PausePowerupShooting());

    }

    IEnumerator PausePowerupShooting()  {
        canShootPowerup = false;
        yield return new WaitForSeconds(1);
        canShootPowerup = true;
    }

    private void MoveEnemy() {

        if (!kamikazeEnemy && !canEvade)  {
            transform.Translate(new Vector3(xSpeed, -_ySpeed, 0) * Time.deltaTime );
        }

        if(kamikazeEnemy && transform.position.y > 3.5) {
 
            transform.Translate(new Vector3(xSpeed, -1, 0) * Time.deltaTime * _ySpeed);
        }

        if (kamikazeEnemy && transform.position.y <= 3.5) {
            playerPosition = player.transform.position;
            navigationVector = playerPosition - this.transform.position;
            if (navigationVector.magnitude > .1)  {
                //transform.Translate(new Vector3(navigationVector.x * 2 , -1 * _ySpeed, 0) * Time.deltaTime );
                transform.Translate(new Vector3(navigationVector.x, -_ySpeed, 0) * Time.deltaTime, Space.World);
                if(navigationVector.x < .5)   {
                    transform.Translate(new Vector3(navigationVector.x * 2.5f, -_ySpeed, 0) * Time.deltaTime, Space.World);
                }
            }
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
        if ((other.tag == "Laser" || other.tag == "Missile") && !shieldActive) {
            Destroy(other.gameObject);

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

            //audioSource.PlayOneShot(audioExplosion);
            Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            
        } else if (other.tag == "Player" && !shieldActive) {
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

            //audioSource.PlayOneShot(audioExplosion);
            Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        if((other.tag == "Player" || other.tag == "Laser" || other.tag == "Missile") && shieldActive)  {
            shield.SetActive(false);
            shieldActive = false;
            audioSource.PlayOneShot(audioExplosion);
        }

    }
}
