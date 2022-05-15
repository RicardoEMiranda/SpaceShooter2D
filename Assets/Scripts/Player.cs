using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float _horizontalInput;
    [SerializeField] private float _verticalInput;
    [SerializeField] private float _speed = 8.0f;
    private Vector3 _pos;

    // Start is called before the first frame update
    void Start() {

        //Set player initial starting position
        transform.position = new Vector3(0, -4, 0);
        _pos = new Vector3(0, transform.position.y, 0);
        
    }

    // Update is called once per frame
    void Update() {

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        MovePlayer();
        ConstrainPlayer();

    }

    private void MovePlayer() {

        transform.Translate(new Vector3(1, 0, 0) * _horizontalInput * Time.deltaTime * _speed);
        transform.Translate(new Vector3(0, 1, 0) * _verticalInput * Time.deltaTime * _speed);

    }

    private void ConstrainPlayer() {
        //Applies scene boundary constraints to the player 

        //obtain current position and allocate to a position variable
        _pos = new Vector3(transform.position.x, transform.position.y, 0);

        //Apply upper boundary limit on the screen
        //if player reaches y >= 0 set y = 0 
        if (_pos.y >= 0) {
            transform.position = new Vector3(_pos.x, 0, 0);
        }

        //Apply lower boundary limit on the screen
        //if player reaches y<= -4.9 set y = -4.9
        if (_pos.y <= -4.9) {
            transform.position = new Vector3(_pos.x, -4.9f, 0);
        }

        //Apply lateral boundary limits on the screen
        //if x >= 10 set x = -10
        //if x <= -10 set x = 10
        if (_pos.x >= 10) {
            transform.position = new Vector3(-10.0f, _pos.y, 0);
        }

        if (_pos.x <= -10) {
            transform.position = new Vector3(10.0f, _pos.y, 0);
        }

    }
}
