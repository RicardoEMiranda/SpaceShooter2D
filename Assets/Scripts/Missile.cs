using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private Vector3 navigationVector;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float theta;
    [SerializeField] private Quaternion rotAngle;
    private float rotSpeed = 10f;

    private AudioSource audioSource;
    private AudioClip clipRocket;


    // Start is called before the first frame update
    void Start()  {


        if (GameObject.FindGameObjectWithTag("Enemy") != null)  {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyTransform = enemy.transform;
        } 
       
        if(GameObject.FindGameObjectWithTag("Enemy") == null){
            navigationVector = Vector3.up;
        }

        audioSource = GetComponent<AudioSource>();
        clipRocket = Resources.Load<AudioClip>("audio_Rocket");
        audioSource.PlayOneShot(clipRocket);
    }

    // Update is called once per frame
    void LateUpdate() {


        if (enemy != null) {
            enemyTransform = enemy.transform;
            navigationVector = (enemyTransform.position - transform.position);
            theta = Mathf.Atan2(navigationVector.y, navigationVector.x) * Mathf.Rad2Deg - 90;
            rotAngle = Quaternion.AngleAxis(theta, new Vector3(0, 0, 1));
            
            if (navigationVector.magnitude > .1) {
                transform.Translate(navigationVector.normalized * Time.deltaTime * speed, Space.World);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotAngle, Time.deltaTime * rotSpeed);
            }

        }
 
        else {
            transform.Translate(navigationVector.normalized * Time.deltaTime * speed, Space.World);
        }


    }


}
