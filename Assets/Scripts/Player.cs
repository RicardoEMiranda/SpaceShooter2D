using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float _horizontalInput;
    [SerializeField] private float _verticalInput;
    private bool leftShiftDown;
    private bool leftShiftUp;
    [SerializeField] private float _speed = 8.0f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _trippleShotPrefab;
    [SerializeField] public int _lives = 3;
    [SerializeField] private float _fireDelay = .35f;
    private bool _canFire = true;
    private float _fireTime = 0f;
    private Vector3 _pos;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private bool _trippleShotEnabled;
    [SerializeField] private bool _speedBoostEnabled;
    [SerializeField] private bool _shieldBoostEnabled;
    [SerializeField] private float _boostSpeedFactor = 1.0f;

    //x velocity calculation variables
    [SerializeField] private float xPos;
    [SerializeField] public float xVel;
    private UIManager _uiManager;
    [SerializeField] private GameObject leftEngine, rightEngine;
    [SerializeField] private GameObject explosion;
    private int engineID;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioLaser;
    [SerializeField] private AudioClip audioExplosion;
    [SerializeField] private AudioClip audioINeedAmmo;
    [SerializeField] private AudioClip audioImHit;
    [SerializeField] private AudioClip audioReactorFailure;
    [SerializeField] private HomingStun homingStun;
    private float targetTime = 0f;
    private float imHitTargetTime = 0;

    private int shieldHit = 0;
    private bool ammoOut = false;
    private bool hasMissileBoost;
    public bool canUseThruster = true;
    private bool stopWatchOn;
    public float thrustTimer = 0;
    public float thrustCoolDownTimer = 0;
    public float maxThrustTime = 6f;

    public CameraShake cameraShake;
    private bool reactorStunned;
    private float messageTimer = 0;
    public bool collect;


    [SerializeField] private GameObject missile;

    // Start is called before the first frame update
    void Start() {

        //Set player initial starting position
        transform.position = new Vector3(0, -4, 0);
        _pos = new Vector3(0, transform.position.y, 0);
        _laserPrefab = Resources.Load<GameObject>("Laser");
        _trippleShotPrefab = Resources.Load<GameObject>("TrippleShot");
       
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        leftEngine = GameObject.Find("LeftEngine");
        rightEngine = GameObject.Find("RightEngine");
        explosion = Resources.Load<GameObject>("Explosion");
        leftEngine.SetActive(false);
        rightEngine.SetActive(false);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null) {
            Debug.LogError("Spawn Manager is Null!");
        }

        audioLaser = Resources.Load<AudioClip>("audio_laser");
        if (audioLaser == null)  {
            Debug.LogError("Laser Audio Clip is NULL.");
        }

        audioExplosion = Resources.Load<AudioClip>("audio_explosion");
        if (audioExplosion == null) {
            Debug.LogError("Explosion Audio Clip is NULL.");
        }

        audioSource = GetComponent<AudioSource>();
        //audioSource.clip = audioLaser;
        if (audioSource == null) {
            Debug.LogError("Audio Source is NULL.");
        }


        audioINeedAmmo = Resources.Load<AudioClip>("audio_IneedAmmo");
        audioImHit = Resources.Load<AudioClip>("audio_ImHit");
        audioReactorFailure = Resources.Load<AudioClip>("audio_ReactorFailure");

        missile = Resources.Load<GameObject>("Missile");
        cameraShake = GameObject.Find("CameraContainer").GetComponent<CameraShake>();

    }

    // Update is called once per frame
    void Update() {

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        //leftShiftDown = Input.GetKey(KeyCode.LeftShift);
        leftShiftDown = Input.GetKeyDown(KeyCode.LeftShift);
        leftShiftUp = Input.GetKeyUp(KeyCode.LeftShift);
        collect = Input.GetKey(KeyCode.C);

        if (GameObject.FindGameObjectWithTag("HomingStun") != null)  {
            homingStun = GameObject.FindGameObjectWithTag("HomingStun").GetComponent<HomingStun>();
        }

        ManageThruster();
        MovePlayer();
        ConstrainPlayer();
        FireLazer();
        CheckAmmoCount();
        CheckHitStatus();
        CheckMissileFireStatus();

    }

    private void ManageThruster()  {
        if (leftShiftDown && canUseThruster)  {
            StartCoroutine(ThrusterTimerOn());
            stopWatchOn = true;
        }
        if (leftShiftUp)  {
            StartCoroutine(ThrusterCoolDown());
            stopWatchOn = false;
        }

        if (stopWatchOn && canUseThruster) {
            ThrusterStopWatch();
        }
        if (!stopWatchOn & !canUseThruster)  {
            thrustTimer = 0;
            ThrusterCoolDownStopWatch();
        }
    }

    private void ThrusterStopWatch() {
        thrustTimer += Time.deltaTime;
        //Debug.Log("Thrust Timer: " + thrustTimer);
    }

    private void ThrusterCoolDownStopWatch() {
        thrustCoolDownTimer += Time.deltaTime;
        //Debug.Log("Thrust Cool Down: " + thrustCoolDownTimer);
    }

    /// <summary>//////////////////////////////////////////////////////////////////////////
    /// Methods 
    /// </summary>////////////////////////////////////////////////////////////////////////
    private void MovePlayer() {

        xPos = transform.position.x;
        transform.Translate(new Vector3(1, 0, 0) * _horizontalInput * Time.deltaTime * _speed * _boostSpeedFactor);
        transform.Translate(new Vector3(0, 1, 0) * _verticalInput * Time.deltaTime * _speed * _boostSpeedFactor);
        xVel = (transform.position.x - xPos)/Time.deltaTime;


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
        } else if (_pos.x <= -10) {
            transform.position = new Vector3(10.0f, _pos.y, 0);
        }

    }

    private void FireLazer() {

        if (Time.time > _fireTime && ammoOut==false && !hasMissileBoost) {
            _canFire = true;
        }

        if(ammoOut || reactorStunned) {
            _canFire = false;

            if (reactorStunned && messageTimer <= Time.time) {
                audioSource.PlayOneShot(audioReactorFailure);
                messageTimer = Time.time + 2f;
            }

        }

        if(!reactorStunned) {
            _canFire = true;
        }

        if(Input.GetKeyDown(KeyCode.Space) && _canFire && !_trippleShotEnabled && !hasMissileBoost) {

            _canFire = false;
            _fireTime = Time.time + _fireDelay;
            Instantiate(_laserPrefab, transform.position + new Vector3(0, .36f, 0), Quaternion.identity);
            _uiManager.UpdateAmmoCount();
            audioSource.PlayOneShot(audioLaser);
        }

        if(Input.GetKeyDown(KeyCode.Space) && _canFire && _trippleShotEnabled) {
            _canFire = false;
            _fireTime = Time.time + _fireDelay;
            Instantiate(_trippleShotPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            _uiManager.UpdateAmmoCount();
            audioSource.PlayOneShot(audioLaser);
        }
        


    }

    private void CheckAmmoCount() {
        
        if(_uiManager.ammoCount<=5 && targetTime<=Time.time)  {
            audioSource.PlayOneShot(audioINeedAmmo);
            targetTime = Time.time + 5f;
        }

        if(_uiManager.ammoCount<=0) {
            ammoOut = true;
        }
        if (_uiManager.ammoCount > 0) {
            ammoOut = false;
        }

    }

    private void CheckHitStatus()  {
        if (_lives ==1 && imHitTargetTime <= Time.time) {
            audioSource.PlayOneShot(audioImHit);
            imHitTargetTime = Time.time + 5f;
        }
    }

    public void Damage() {

        //if shieldActive, return (don't take damage) 
        //Also could set _shieldBoostEnabled to false
        //but instead, we'll let it run for 10 seconds according to
        //ShieldBoostPowerDown() routine
        if (_shieldBoostEnabled)  {
            return;
        } else {
            _lives -= 1;
            audioSource.PlayOneShot(audioExplosion);
            _uiManager.UpdateLivesUI(_lives);

            //if lives = 2, enable right or left engine
            if(_lives == 2)  {
                leftEngine.SetActive(true);
            }

            //if lives = 1, enable the other engine
            if (_lives == 1) {
                rightEngine.SetActive(true);

            }

            StartCoroutine(cameraShake.ShakeCamera());
            
        }

        if (_lives<1) {
            _uiManager.GameOver();
            audioSource.PlayOneShot(audioExplosion);
            Instantiate(explosion, transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
            Destroy(this.gameObject, 2f);
            _spawnManager.StopSpawning();
        }
    }

    private void CheckMissileFireStatus() {

        if(hasMissileBoost && Input.GetKeyDown(KeyCode.Space) && !reactorStunned) {
            Instantiate(missile, transform.position, Quaternion.identity); 
        }

    }

    public void TrippleShotActivate() {

        //set _trippleShotEnabled true
        //start powerdown coroutine for tripple shot "StartCoroutine(TrippleShotPowerDownRoutine());"
        _trippleShotEnabled = true;
        StartCoroutine(TrippleShotPowerDownRoutine());

    }

    public void SpeedBoostActivate() {
        _speedBoostEnabled = true;
        _boostSpeedFactor = 2;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void SheildBoostActivate() {
        _shieldBoostEnabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(ShieldBoostPowerDownRoutine());
    }

    public void MissileBoostActivate() {
        hasMissileBoost = true;
        _canFire = false;
        StartCoroutine(FireMissileRoutine());
    }

    public void AmmoBoostActivate() {
        ammoOut = false;
        _uiManager.BoostAmmo();
    }

    public void RepairBoostActivate() {
        
        if(_lives==2 || _lives==1) {
            _lives += 1;
            _uiManager.UpdateLivesUI(_lives);
        }

        if(_lives == 3) {
            leftEngine.SetActive(false);
            rightEngine.SetActive(false);   
        }

        if (_lives == 2)  {
            leftEngine.SetActive(true);
            rightEngine.SetActive(false);
        }

    }

    public void StunActivate() {
        Damage();
        StartCoroutine(StunReactor());
    }

    IEnumerator StunReactor() {
        reactorStunned = true;
        int stunDelay = 10;
        if(homingStun.isBoss) {
            //stunDelay = 5;
        }
   
        yield return new WaitForSeconds(stunDelay);
        reactorStunned = false;
    }

    IEnumerator ThrusterTimerOn() {
        _speed = 16f;
        thrustCoolDownTimer = 0;
        yield return new WaitForSeconds(maxThrustTime);
        _speed = 8f;
        canUseThruster = false;
        thrustTimer = 0;
    }

    IEnumerator ThrusterCoolDown() {
        _speed = 8f;
        canUseThruster = false;
        yield return new WaitForSeconds(10);
        canUseThruster = true;
    }

    IEnumerator FireMissileRoutine() {

        yield return new WaitForSeconds(10);
        hasMissileBoost = false;
        _canFire = true;
    }

    //IEnumerator TrippleShotPowerDownRoutine
    //wait 5 sec
    //set _trippleShotEnabled to false
    IEnumerator TrippleShotPowerDownRoutine() {

        yield return new WaitForSeconds(10);
        _trippleShotEnabled = false;
    }

    IEnumerator SpeedBoostPowerDownRoutine() {
        yield return new WaitForSeconds(10);
        _boostSpeedFactor = 1.0f;
        _speedBoostEnabled= false;
    }

    IEnumerator ShieldBoostPowerDownRoutine() {

        yield return new WaitForSeconds(15);
        _shieldBoostEnabled= false;
        transform.GetChild(0).gameObject.SetActive(false);
        shieldHit = 0;
    }

    private void TerminateShieldEarly() {
        shieldHit = 0;
        _shieldBoostEnabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        StopCoroutine("ShieldBoostPowerDownRoutine");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.gameObject.name == "LaserEnemy") {
            audioSource.PlayOneShot(audioExplosion);
        }

        if((other.gameObject.tag == "Enemy" || other.gameObject.tag == "LaserEnemy") || other.gameObject.tag == "EWEnemy" || other.gameObject.tag == "HomingStun" || other.gameObject.tag == "HomingCharge" && _shieldBoostEnabled) {

            shieldHit += 1;
            if (shieldHit == 1) {
                transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(.72f, .1f, 1f);
            }
            if (shieldHit == 2) {
                transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(.44f, .4f, 1f);
            }
            if (shieldHit == 3) {
                transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(.07f, 0f, 1f);
            }
            if (shieldHit == 4) {
                TerminateShieldEarly();
            }

        }

    }

}
