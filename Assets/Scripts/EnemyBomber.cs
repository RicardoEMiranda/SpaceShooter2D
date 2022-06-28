using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : MonoBehaviour {

    private float speed = -3f;
    private bool bombingComplete;
    private bool isLeft;
    private bool isRight;
    [SerializeField] private Quaternion startAngle;
    [SerializeField] private Quaternion attackAngle;
    [SerializeField] private GameObject homingCharge;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioFire;

    private bool maneuvering = true;
    private bool attacking;
    private bool escaping;
    private bool canFire;
    private bool paused = false;
    private float fireTime = 10f;

    private Player player;

    // Start is called before the first frame update
    void Start()  {
        if (GameObject.Find("Player") != null) {
            player = GameObject.Find("Player").GetComponent<Player>();
        }

        audioSource = GetComponent<AudioSource>();
        audioFire = Resources.Load<AudioClip>("audio_BlasterCharging");

        homingCharge = Resources.Load<GameObject>("HomingCharge");

        startAngle = transform.rotation;

        if(transform.position.x < 0)  {
            isLeft = true;
            attackAngle = Quaternion.Euler(0, 0, 90);
        } 
        if(transform.position.x > 0) {
            isRight = true;
            attackAngle = Quaternion.Euler(0, 0, -90);
        }
        
    }

    // Update is called once per frame
    void Update()  {

        if (transform.position.y > -3.5)  {
            transform.Translate(new Vector3(0, speed, 0) * Time.deltaTime);
        }
        else  {
            StartCoroutine(EnemyBomberRoutine());
        }

        if(canFire && !paused)  {

            while(Time.time >= fireTime)  {
                audioSource.PlayOneShot(audioFire);
                StartCoroutine(DelayInstantiate());
                fireTime = Time.time + 1.5f;
            }
            if(!canFire)  {
                paused = true;
            }

        }

        if (bombingComplete && escaping)  {

            transform.rotation = startAngle;
            transform.Translate(new Vector3(0, speed, 0) * Time.deltaTime);
        }

        if(transform.position.y < -8.5)  {
            Destroy(this.gameObject);
        }
        
    }

    IEnumerator EnemyBomberRoutine() {
        //Debug.Log("Bomber Routine Entered");


        if(isLeft && maneuvering)  {
            //Quaternion finalAngle = Quaternion.AngleAxis(90, new Vector3(0, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, attackAngle, .01f);
            if(transform.rotation == attackAngle)  {
               canFire = true;
            }
        }
        if(isRight && maneuvering)  {
            Quaternion finalAngle = Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, attackAngle, .01f);
            if (transform.rotation == attackAngle)  {
                canFire = true;
            }
        }
        yield return new WaitForSeconds(1f);
        attacking = true;
        canFire = false;
  
        yield return new WaitForSeconds(5);
        maneuvering = false;

        //attacking = true;
        if (isLeft && attacking) {
            Quaternion finalAngle = Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, startAngle, .01f);
        }
        if (isRight && attacking) {
            Quaternion finalAngle = Quaternion.AngleAxis(90, new Vector3(0, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, startAngle, .01f);
        }
        attacking = false;
        yield return new WaitForSeconds(1);
        attacking = false;
        escaping = true;
        bombingComplete = true;

    }

    IEnumerator DelayInstantiate() {

        yield return new WaitForSeconds(1.5f);
        float xPos = transform.position.x + 1f;
        float yPos = transform.position.y;
        Instantiate(homingCharge, new Vector3(xPos, yPos, 0), Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Player")  {
            player.Damage();
        }
    }
}
