using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour {

    private Vector3 _pos;


    // Start is called before the first frame update
    void Start() {
       
        _pos = new Vector3(0, transform.position.y, 0);
}

    // Update is called once per frame
    void Update() {

        ConstrainPlayer();

    }

    private void ConstrainPlayer() {
        //Applies scene boundary constraints to the player 

        //obtain current position and allocate to a position variable
        //We want the current position so we can compare it to our 
        //known boundary constraints
        _pos = new Vector3(transform.position.x, transform.position.y, 0);


        //Apply upper boundary limit on the screen
        //if player position reaches y >= 0 set y = 0 
        if (_pos.y >= 0) {
            transform.position = new Vector3(transform.position.x, 0, 0);
        } else if (_pos.y <= -4.9) {
            transform.position = new Vector3(transform.position.x, -4.9f, 0);
        } //Apply lower boundary limit on the screen
          //if player reaches y<= -4.9 set y = -4.9

        //Apply lateral boundary limits & wrapping on the screen
        //if x >= 10 set x = -10
        //if x <= -10 set x = 10
        if (_pos.x >= 10) {
            transform.position = new Vector3(-10.0f, _pos.y, 0);
        }
        else if (_pos.x <= -10) {
            transform.position = new Vector3(10.0f, _pos.y, 0);
        }

    }

}
