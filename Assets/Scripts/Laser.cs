using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    private int _laserSpeed = 10;
    private Player _player;
    private float _xVel=0;

    // Start is called before the first frame update
    void Start() {

        _player = GameObject.Find("Player").GetComponent<Player>();
        //_xVel = _player.xVel/2.5f;

    }

    // Update is called once per frame
    void Update() {

        //transform.Translate(Vector3.up * Time.deltaTime * _laserSpeed);
        transform.Translate(new Vector3(_xVel, _laserSpeed, 0) * Time.deltaTime);
       
        if (transform.position.y >= 12) {

            //check if this laser has a parent (TrippleShotParent)
            //if it does, destroy the parent object
            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "Enemy") {
            Destroy(this.gameObject);
        }

    }
}
