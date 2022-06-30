using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingStun : MonoBehaviour {

    [SerializeField] private GameObject player;
    [SerializeField] private Player playerScript;
    //[SerializeField] private AudioSource audioSource;
    //[SerializeField] private AudioClip clip;

    private Vector3 navigationVector;
    private float navigationVectorX;
    private float navigationVectorY;
    private float xSpeed;
    private float ySpeed;
    public bool isBoss = true;


    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();
        navigationVector = player.transform.position - transform.position;
        navigationVectorX = navigationVector.x;
        navigationVectorY = navigationVector.y;

        xSpeed = navigationVectorX;
        ySpeed = navigationVectorY;

    }

    // Update is called once per frame
    void Update() {

        transform.Translate(new Vector3(xSpeed, ySpeed, 0) * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.gameObject.tag == "Player")  {
            playerScript.StunActivate();
        }

    }
}
