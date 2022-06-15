using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()  {
        //int i = 0;
        int a = 0;
        for (int j = a + 1; j < 7; a = a + 1 ) {

            Debug.Log("j is: " + j);
        }
    }

    // Update is called once per frame
    void Update() {
    

    }
}
