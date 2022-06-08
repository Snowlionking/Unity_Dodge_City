using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject pointPrefab;
    public Text scoreText;
    public Text levelText;
    public Text healthText;
    public Text highscoreText;

    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelGameOver;
    public GameObject panelLevelCompleted;
    public GameObject panelOverall;
    public GameObject panelPause;

    public GameObject[] enemiesInLevels;
    public GameObject[] levels;
    public int[] scoresToCompleteLevel;

    public bool isInputEnabled = true;
    public static GameManager Instance { get; private set; }



    public enum State { MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, GAMEOVER, PAUSE }

    private State state;
    private GameObject currentLevel;
    private GameObject currentPoint;
    private GameObject player;
    private List<GameObject> enemies = new List<GameObject>();
    private bool isSwitchingState;
    private bool isLaunching;

    private int score;


    public int Score {
        get { return score; }
        set {
            score = value;
            scoreText.text = "SCORE: " + score;
        }
    }

    private int level;

    public int Level {
        get { return level; }
        set {
            level = value;
            levelText.text = "LEVEL: " + (level + 1);
        }
    }

    private int health;

    public int Health {
        get { return health; }
        set {
            health = value;
            healthText.text = "HEALTH: " + health;
        }
    }

    private int highscore;

    public int Highscore {
        get { return highscore; }
        set {
            highscore = value;
            highscoreText.text = "HIGHSCORE: " + highscore;
        }
    }

    public void EnemyDestroyed(GameObject enemy) {
        enemies.Remove(enemy);
    }

    public void PlayClicked() {
        SwitchState(State.INIT);
    }
    public void ResumeClicked() {
        SwitchState(State.PLAY);
    }
    public void QuitClicked() {
        Application.Quit();
    }

    void Start() {
        Instance = this;
        panelOverall.SetActive(true);
        SwitchState(State.MENU);

    }

    private void Launch() {
        currentPoint = Instantiate(pointPrefab);
        enemies.Add(Instantiate(enemiesInLevels[level]));
        isLaunching = false;
    }

    public void SwitchState(State newState, float delay = 0) {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay) {
        isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        state = newState;
        BeginState(state);
        isSwitchingState = false;
    }

    void BeginState(State newState) {
        switch (newState) {
            case State.MENU:
                Score = 0;
                Level = 0;
                Health = 100;
                Highscore = PlayerPrefs.GetInt("highscore");
                panelMenu.SetActive(true);
                break;
            case State.INIT:
                panelPlay.SetActive(true);
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                Destroy(currentLevel);
                Destroy(player);
                Destroy(currentPoint);
                foreach (GameObject enemy in enemies) {
                    Destroy(enemy);
                }
                enemies.Clear();
                panelLevelCompleted.SetActive(true);
                Level++;
                SwitchState(State.LOADLEVEL, 2);
                break;
            case State.LOADLEVEL:
                if (Level >= levels.Length) {
                    SwitchState(State.GAMEOVER);
                } else {
                    currentLevel = Instantiate(levels[level]);
                    player = Instantiate(playerPrefab);
                    isLaunching = true;
                    Invoke("Launch", 1f);
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                Destroy(player);
                Destroy(currentPoint);
                foreach (GameObject enemy in enemies) {
                    Destroy(enemy);
                }
                if (Score > PlayerPrefs.GetInt("highscore")) {
                    PlayerPrefs.SetInt("highscore", Score);
                }
                panelGameOver.SetActive(true);
                break;
            case State.PAUSE:
                isInputEnabled = false;
                panelPause.SetActive(true);
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                foreach (GameObject enemy in enemies) {
                    enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                break;
        }
    }

    void Update() {
        switch (state) {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    SwitchState(State.PAUSE);
                }

                if (currentPoint == null && score < scoresToCompleteLevel[level] && !isLaunching) {
                    enemies.Add(Instantiate(enemiesInLevels[level]));
                    currentPoint = Instantiate(pointPrefab);
                }

                if (score >= scoresToCompleteLevel[level] && !isSwitchingState) {
                    SwitchState(State.LEVELCOMPLETED);
                }

                if (health <= 0) {
                    SwitchState(State.GAMEOVER);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                if (Input.anyKeyDown) {
                    SwitchState(State.MENU);
                }
                break;
            case State.PAUSE:
                break;
        }
    }

    void EndState() {
        switch (state) {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                panelPlay.SetActive(false);
                break;
            case State.LEVELCOMPLETED:
                panelLevelCompleted.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelGameOver.SetActive(false);
                break;
            case State.PAUSE:
                isInputEnabled = true;
                panelPause.SetActive(false);
                foreach (GameObject enemy in enemies) {
                    enemy.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
                }
                break;
        }
    }
}
