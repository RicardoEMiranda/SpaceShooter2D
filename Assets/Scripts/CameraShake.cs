using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private float timer = 0;
    public float shakeDuration = 2f;
    public float magnitude = 1f;
    public Vector3 originalPosition;

 
    // Start is called before the first frame update
    void Start() {
        originalPosition = this.transform.position;

    }

    public IEnumerator ShakeCamera() {

        while (timer < shakeDuration) {
            timer += Time.deltaTime;
            //Debug.Log("Shake");
            float x = Random.Range(-.5f, .5f);
            float y = Random.Range(-.5f, .5f);
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(x, y, originalPosition.z), .1f) * magnitude;
            yield return null;
        }

        timer = 0;
        this.transform.position = originalPosition;
       
    }
}
