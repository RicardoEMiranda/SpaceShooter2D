using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ThrusterBar : MonoBehaviour {

    [SerializeField] private Player player;
    [SerializeField] private float thrustTimer;
    [SerializeField] private float thrustCoolDownTimer;
    private Slider slider;
    [SerializeField] private float tankCharge;
    [SerializeField] private bool canUseThruster;
    private float maxThrustTime;
    [SerializeField] private float sliderValue;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update() {

        thrustTimer = player.thrustTimer;
        thrustCoolDownTimer = player.thrustCoolDownTimer;
        maxThrustTime = player.maxThrustTime;

        //tankCharge = thrustTimer
        canUseThruster = player.canUseThruster;

        if (canUseThruster && thrustTimer == 0) {
            if (thrustTimer == 0) { 
                tankCharge = maxThrustTime;
            }
        }
        if(canUseThruster && thrustTimer > 0) {
            tankCharge = maxThrustTime - thrustTimer;
        }

        if(!canUseThruster) {
            tankCharge = 0;
        }

        slider.value = tankCharge / maxThrustTime;
        sliderValue = slider.value;

    }
}
