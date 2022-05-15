using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float _horizontalInput;
    [SerializeField] private float _verticalInput;
    [SerializeField] private float _speed = 8.0f;

    // Start is called before the first frame update
    void Start() {

        
    }

    // Update is called once per frame
    void Update() {

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        MovePlayer();

    }

    private void MovePlayer() {

        transform.Translate(new Vector3(1, 0, 0) * _horizontalInput * Time.deltaTime * _speed);
        transform.Translate(new Vector3(0, 1, 0) * _verticalInput * Time.deltaTime * _speed);

    }
}
