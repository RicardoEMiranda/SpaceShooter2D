using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour {

    [SerializeField] private SpriteRenderer shield;
    [SerializeField] private GameObject littleExplosion;
    [SerializeField] private SpriteRenderer healthBar;
    [SerializeField] private Sprite healthBar75;
    [SerializeField] private Sprite healthBar50;
    [SerializeField] private Sprite healthBar25;
    [SerializeField] private SpriteRenderer bossSprite;
    [SerializeField] private CircleCollider2D collider;
    [SerializeField] private GameObject bigExplosion;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private GameObject homingStun;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bossThemeClip;
    [SerializeField] private CameraShake cameraShake;

    private float cruiseSpeed = 1.5f;
    private float attackSpeed = 4f;
    private bool shieldEnabled = true;
    private int shieldHitCount = 0;

    private Vector3 waypoint1 = new Vector3(-7, 2.45f, 0);
    private Vector3 waypoint2 = new Vector3(7, 2.45f, 0);
    private bool attackCycleStart;
    private bool goLeft;
    private bool goRight;
    private bool goCenter;
    private float pauseDelay = 3f;
    private float goRightTime;
    private int attackSequence = 0;
    private bool canFire;
    private float fireTime = 0;

    private int health = 5;


    // Start is called before the first frame update
    void Start() {

        shield = transform.GetChild(2).GetComponent<SpriteRenderer>();
        littleExplosion = Resources.Load<GameObject>("Explosion");
        bigExplosion = Resources.Load<GameObject>("BigExplosion");
        healthBar = transform.GetChild(3).GetComponent<SpriteRenderer>();
        healthBar75 = Resources.Load<Sprite>("BossHealthBar-75");
        healthBar50 = Resources.Load<Sprite>("BossHealthBar-50");
        healthBar25 = Resources.Load<Sprite>("BossHealthBar-25");
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        homingStun = Resources.Load<GameObject>("HomingStun");

        bossSprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<CircleCollider2D>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        audioSource = GameObject.Find("AudioManager").GetComponentInChildren<AudioSource>();
        bossThemeClip = Resources.Load<AudioClip>("audio_BossTheme");
        cameraShake = GameObject.Find("CameraContainer").GetComponent<CameraShake>();

        audioSource.clip = bossThemeClip;
        audioSource.volume = .5f;
        audioSource.PlayOneShot(bossThemeClip);

    }

    // Update is called once per frame
    void Update()  {

        MoveIntoPosition();
        AggressiveAttack();
        UpdateHealthBar();

    }

    private void MoveIntoPosition() {
        if (transform.position.y >= 2.45) {
            transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * cruiseSpeed);
        } 

    }

    private void AggressiveAttack() {
        if(shieldEnabled == false && attackCycleStart)  {

            if(attackSequence == 1)  {
                pauseDelay = 1f;
                attackSpeed = 7f;
            }
            if (attackSequence >= 2)  {
                pauseDelay = .5f;
                attackSpeed = 10f;
            }

            if(goLeft)  {
                if(transform.position.x > -7)  {
                    transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * attackSpeed);
                }
                else  {
                    StartCoroutine(PauseAttackLeft());
                    StartCoroutine(Fire());
                }
            }

            if(goRight)  {
                if(transform.position.x < 7)  {
                    transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * attackSpeed);

                    if(Time.time > fireTime)  {
                        StartCoroutine(FireWhileMoving());
                        fireTime = Time.time + .5f;
                    }
                    
                } else {
                    StartCoroutine(PauseAttackRight());
                    StartCoroutine(Fire());
                }
            }

            if(goCenter)   {
                if (transform.position.x > 0)  {
                    transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * attackSpeed);
                }
                else  {
                    StartCoroutine(PauseAttackCenter());
                    StartCoroutine(Fire());
                }
            }
        }
    }

    IEnumerator Fire() {
        Instantiate(homingStun, transform.localPosition, Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        Instantiate(homingStun, transform.localPosition, Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        Instantiate(homingStun, transform.localPosition, Quaternion.identity);
    }

    IEnumerator FireWhileMoving() {
        Instantiate(homingStun, transform.localPosition, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(homingStun, transform.localPosition, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(homingStun, transform.localPosition, Quaternion.identity);
    }


    IEnumerator PauseAttackLeft()  {
        goLeft = false;
        yield return new WaitForSeconds(pauseDelay);
        goRight = true;
    }

    IEnumerator PauseAttackRight() {
        goRight = false;
        yield return new WaitForSeconds(pauseDelay);
        goCenter = true;
    }

    IEnumerator PauseAttackCenter()  {
        goCenter = false;
        yield return new WaitForSeconds(pauseDelay);
        goLeft = true;
        attackSequence += 1;
        //Debug.Log(attackSequence);
    }

    private void UpdateHealthBar() {
        if(health == 3) {
            healthBar.sprite = healthBar75;
        }
        if (health == 2) {
            healthBar.sprite = healthBar50;
        }
        if (health == 1) {
            healthBar.sprite = healthBar25;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)   {

        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Laser") || other.gameObject.tag == "Missile" && shieldEnabled)  {

            shieldHitCount += 1;
            if (shieldHitCount == 1)
            {
                Instantiate(littleExplosion, transform.localPosition + new Vector3(.51f, -1.44f, 0), Quaternion.identity);
                shield.color = new Color(.72f, .1f, 1f);
            }
            if (shieldHitCount == 2)
            {
                Instantiate(littleExplosion, transform.localPosition + new Vector3(-.51f, -1.44f, 0), Quaternion.identity);
                shield.color = new Color(.44f, .4f, 1f);
            }
            if (shieldHitCount == 3)
            {
                Instantiate(littleExplosion, transform.localPosition + new Vector3(0, -1.5f, 0), Quaternion.identity);
                shield.color = new Color(.07f, 0f, 1f);
            }
            if (shieldHitCount == 4)
            {
                Instantiate(littleExplosion, transform.localPosition + new Vector3(0, -1.5f, 0), Quaternion.identity);
                shield.enabled = false;
                shieldEnabled = false;
                attackCycleStart = true;
                goLeft = true;
            }

        }

        if(!shieldEnabled) {
            if(other.gameObject.tag == "Player" || other.gameObject.tag == "Laser" || other.gameObject.tag == "Missile") {
                health += -1;
                //Debug.Log(health);
                Instantiate(littleExplosion, transform.localPosition + new Vector3(0, -1.44f, 0), Quaternion.identity);

                if(health < 4)  {
                    StartCoroutine(cameraShake.ShakeCamera());
                }

                if(health <= 0)  {
                    bossSprite.enabled = false;
                    collider.enabled = false;
                    Instantiate(bigExplosion, transform.localPosition, Quaternion.identity);
                    healthBar.enabled = false;
                    Destroy(this.gameObject);
                    StartCoroutine(cameraShake.ShakeCamera());
                    uiManager.gameOver = true;
                    uiManager.GameOver();
                    spawnManager.StopSpawning();

                }
            }
            
        }

  
    }
}
