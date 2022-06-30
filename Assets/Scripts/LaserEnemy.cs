using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : MonoBehaviour {

    private int _laserSpeed = -10;
    private Player player;
    public Transform[] laserArray;


    // Start is called before the first frame update
    void Start() {

        if (GameObject.Find("Player") != null) { 
        player = GameObject.Find("Player").GetComponent<Player>();
        }

        laserArray = new Transform[2];
        laserArray[0] = this.gameObject.transform.GetChild(0);
        laserArray[1] = this.gameObject.transform.GetChild(1);  

    }

    // Update is called once per frame
    void Update() {

        //transform.Translate(Vector3.up * Time.deltaTime * _laserSpeed);
        transform.Translate(new Vector3(0, _laserSpeed, 0) * Time.deltaTime);

        if (transform.position.y <= -8) {

            Destroy(this.gameObject);
            Destroy(laserArray[0]);
            Destroy(laserArray[1]);

        }

        if(transform.position.y < -7) {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.tag == "Player")  {
            player.Damage();
            Destroy(this.gameObject);
            Destroy(laserArray[0]);
            Destroy(laserArray[1]);
        }

    }
}
