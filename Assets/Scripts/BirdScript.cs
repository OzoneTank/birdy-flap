using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour {

    public float flapStrength = 10.0f;
    Rigidbody2D rb;

    public GameObject game;

	void Start () {
        rb = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {
        GameScript gameScript = game.GetComponent<GameScript> ();

        if (gameScript.gameMode == GameScript.GameMode.running) {
            rb.bodyType = RigidbodyType2D.Dynamic;
        } else {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
        }
        if (Input.GetMouseButtonDown (0) ||
        Input.GetKeyDown (KeyCode.Space)) {
            Flap ();
        }
	}

    void Flap() {
        GameScript gameScript = game.GetComponent<GameScript> ();
        if (gameScript.gameMode == GameScript.GameMode.waiting) {
            gameScript.StartGame ();
            rb.velocity = Vector2.zero;
        }

        if (gameScript.gameMode == GameScript.GameMode.running) {
            Vector2 flapForce = new Vector2 (0, flapStrength);
            rb.AddForce (flapForce);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        game.GetComponent<GameScript> ().GameOver ();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Pipe") {
            game.GetComponent<GameScript> ().Score ();
        }
    }
}
