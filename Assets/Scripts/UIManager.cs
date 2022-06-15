using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    [SerializeField] private GameObject _scoreTextObj;
    [SerializeField] private Text _scoreTextComp;
    private int score = 0;

    [SerializeField] private Sprite[] _healthSpriteArray = new Sprite[4];
    [SerializeField] private GameObject _uiPlayerLives;
    private Image playerLivesImage;
    [SerializeField] private GameObject _uiGameOverText;
    private Text gameOverText;
    private GameObject _uiRestartText;
    private Text restartText;
    private Player _player;

    private float r, g, b, a;


    // Start is called before the first frame update
    void Start() {
        _scoreTextObj = GameObject.Find("Score_text");
        _scoreTextComp = _scoreTextObj.GetComponent<Text>();
        _scoreTextComp.text = "Score: " + score;

        _uiPlayerLives = GameObject.Find("UI_PlayerLives");
        playerLivesImage = _uiPlayerLives.GetComponent<Image>();
        _healthSpriteArray[3] = Resources.Load<Sprite>("sprite_health3");
        _healthSpriteArray[2] = Resources.Load<Sprite>("sprite_health2");
        _healthSpriteArray[1] = Resources.Load<Sprite>("sprite_health1");
        _healthSpriteArray[0] = Resources.Load<Sprite>("sprite_health0");
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiGameOverText = GameObject.Find("TextGameOver");
        gameOverText = _uiGameOverText.GetComponent<Text>();
        _uiGameOverText.SetActive(false);
        _uiRestartText = GameObject.Find("TextRestart");
        restartText = _uiRestartText.GetComponent<Text>();
        _uiRestartText.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        _scoreTextComp.text = "Score: " + score.ToString();

        if(_player._lives == 0 && Input.GetKey("r"))  {
            SceneManager.LoadScene("Scene_Level1");
        } 

        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    public void UpdatePlayerScore() {
        score += 10;
    }

    public void UpdateLivesUI(int lives) {

        playerLivesImage.sprite = _healthSpriteArray[lives];
    }

    public void GameOver() {
           
        _uiGameOverText.SetActive(true);
        gameOverText.color = new Color(1, 0, 0, 1);

        _uiRestartText.SetActive(true);
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine() {

        while (true) {
            yield return new WaitForSeconds(.65f);
            //_uiGameOverText.SetActive(false);
            gameOverText.color = new Color(1, 0, 0, 1);

            yield return new WaitForSeconds(.65f);
            //_uiGameOverText.SetActive(false);
            gameOverText.color = new Color(1, 1, 1, 1);

            yield return new WaitForSeconds(.65f);
            //_uiGameOverText.SetActive(true);
            gameOverText.color = new Color(0, 0, 1, 1);

            yield return new WaitForSeconds(.65f);
            gameOverText.color = new Color(0, 0, 0, 0);

  
        }
    }



    
}
