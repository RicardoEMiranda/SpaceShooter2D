using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingCharge : MonoBehaviour {

    [SerializeField] private GameObject player;
    [SerializeField] private Player playerScript;
    private Vector3 navigationVector;
    private float xSpeed = 6f;
    private float ySpeed;

    // Start is called before the first frame update
    void Start()  {

        if(GameObject.Find("Player") != null) {
            player = GameObject.Find("Player");
            playerScript = player.GetComponent<Player>();
            navigationVector = player.transform.position - transform.position;
            ySpeed = navigationVector.normalized.y * 5f;
            xSpeed = navigationVector.normalized.x * 5f;
            
        }
      
        
    }

    // Update is called once per frame
    void Update()  {

        //navigationVector = player.transform.position - transform.position;
        //float xVector = navigationVector.normalized.x;
        transform.Translate(new Vector3(xSpeed, ySpeed, 0) * Time.deltaTime, Space.World);
        
    }

    private void OnTriggerEnter2D(Collider2D collider)  {
        if(collider.tag == "Player")  {
            playerScript.Damage();
            Destroy(this.gameObject);
        }
    }
}
