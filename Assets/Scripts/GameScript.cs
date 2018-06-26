using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

    public GameObject bird;

    public GameObject pipesPrefab;
    public float moveSpeed = 1.0f;
    public float defaultMoveSpeed = 1.0f;
    public float moveSpeedInc = 0.02f;
    public float spawnRate = 3.0f;

    public float width = 10.0f;
    public float min = -5.0f;
    public float max = 5.0f;

    public float spawnRight = 10.0f;
    public float despawnLeft = -10.0f;

    private List<GameObject> pipesList = new List<GameObject> ();
    private float lastTimeSpawn;

    private int score;

    public Text scoreUI;


    public enum GameMode {
        waiting,
        running,
        gameOver
    };

    public GameMode gameMode = GameMode.running;

	void Start () {
        Restart ();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameMode == GameMode.running) {
            if (Time.time > lastTimeSpawn + spawnRate) {
                lastTimeSpawn += spawnRate;
                SpawnPipe ();
            }
        }

	}

    void FixedUpdate() {
        if (gameMode == GameMode.running) {
            List <GameObject> pipesToDestroy = new List<GameObject> ();
            foreach (GameObject pipes in pipesList) {
                Vector2 pos = pipes.transform.position;
                pipes.transform.position = pos + (Vector2.left * moveSpeed * Time.deltaTime);

                if (pos.x < despawnLeft) {
                    pipesToDestroy.Add (pipes);
                }
            }
            foreach (GameObject pipes in pipesToDestroy) {
                Destroy (pipes);
                pipesList.Remove (pipes);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player") {
            GameOver ();
        }
    }

    public void GameOver() {
        gameMode = GameMode.gameOver;
    }

    public void Score() {
        score++;
        scoreUI.text = "" + score;
        moveSpeed += moveSpeedInc;
    }

    public void StartGame() {
        gameMode = GameMode.running;
        lastTimeSpawn = Time.time;
    }

    public void Restart() {
        foreach(GameObject pipes in pipesList)
        {
            Destroy(pipes);
        }
        pipesList.Clear ();
        gameMode = GameMode.waiting;
        bird.transform.position = new Vector2 (0, 0);
        score = 0;
        scoreUI.text = "0";
        moveSpeed = defaultMoveSpeed;
    }

    void SpawnPipe() {
        Vector2 pos = new Vector3 (spawnRight, Random.Range (min, max), 100.0f);
        GameObject pipes = Instantiate (pipesPrefab, pos, transform.rotation) as GameObject;
        pipesList.Add (pipes);
    }
}
