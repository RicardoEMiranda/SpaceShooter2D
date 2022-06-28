using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWEnemy : MonoBehaviour {

    [SerializeField] private GameObject plasma;
    [SerializeField] private GameObject plasmaLeft;
    [SerializeField] private GameObject plasmaRight;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioFire;
    [SerializeField] private Player player;
    private float speed = 2f;
    private bool canAttack = true;
    private float delay = 3f;


    // Start is called before the first frame update
    void Start() {

        plasma = Resources.Load<GameObject>("ProtonPlasma");
        plasmaLeft = Resources.Load<GameObject>("ProtonPlasmaLeft");
        plasmaRight = Resources.Load<GameObject>("ProtonPlasmaRight");
        audioSource = gameObject.GetComponent<AudioSource>();
        audioFire = Resources.Load<AudioClip>("audio_BlasterCharging");

        if (GameObject.Find("Player").GetComponent<Player>() != null) {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
        
    }

    // Update is called once per frame
    void Update()  {

        MoveEnemy();
        
    }

    private void MoveEnemy()  {

        transform.Translate(new Vector3(0, 1, 0) * speed * Time.deltaTime);

        if (transform.position.y <= 2.7 && canAttack) {

            StartCoroutine(PrepareAttack());
            StartCoroutine(Fire());
            StartCoroutine(Pause());
 
        }

        if(transform.position.y <= -8)  {
            Destroy(gameObject);
        }
    }

    IEnumerator PrepareAttack()  {
        speed = 0;
        canAttack = false;
        yield return new WaitForSeconds(5.5f);
        speed = 2f;

    }

    IEnumerator Fire()  {
        audioSource.PlayOneShot(audioFire);
        yield return new WaitForSeconds(1.5f);
        Instantiate(plasma, transform.position + new Vector3 (0, -1.08f, 0), Quaternion.identity);
        Instantiate(plasmaRight, transform.position + new Vector3(.78f, -.36f, 0), Quaternion.identity);
        Instantiate(plasmaLeft, transform.position + new Vector3(-.78f, -.36f, 0), Quaternion.identity);
    }

    IEnumerator Fire2()
    {
        audioSource.PlayOneShot(audioFire);
        yield return new WaitForSeconds(1.5f);
        Instantiate(plasma, transform.position + new Vector3(0, -1.08f, 0), Quaternion.identity);
        Instantiate(plasmaRight, transform.position + new Vector3(.4f, 0f, 0), Quaternion.identity);
        Instantiate(plasmaLeft, transform.position + new Vector3(-.4f, 0f, 0), Quaternion.identity);
    }

    IEnumerator Pause() {
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Fire());
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Fire2());
       
    }

    private void OnTriggerEnter2D(Collider2D collider)  {
        if(collider.tag == "Player") {
            player.Damage();
        }
    }



}
