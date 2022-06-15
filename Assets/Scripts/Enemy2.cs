using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour {

    private float multiplier;
    private float _speedX;
    private float _speedY;
    private float switcher;
    private float direction;
    private float dummySpeed;

    // Start is called before the first frame update
    void Start() {

        _speedX = DetermineComponents();
        _speedY = DetermineComponents();
    }

    // Update is called once per frame
    void Update() {

        transform.Translate(new Vector3(_speedX, _speedY, 0) * Time.deltaTime);
        DestroySelf();
    }

    void DestroySelf() {
        
        if(Mathf.Abs(gameObject.transform.position.y) >= 40) {
            Destroy(this.gameObject);
        }
      
    }

    private float DetermineComponents() {
        
        //Determine X Component
        multiplier = Random.Range(-5, 5);
        direction = Mathf.Sign(Random.Range(-1f, 1f));

        if (Mathf.Abs(multiplier) <= 2)  {
            multiplier *= 20;
        }

        dummySpeed = multiplier * direction;

        if(Mathf.Abs(dummySpeed) < 1.0) {
            dummySpeed = 5;
        }

        return dummySpeed;

    }
}
