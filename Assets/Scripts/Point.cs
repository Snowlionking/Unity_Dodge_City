using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {

    public int points = 10;
    private int playerLayer = 9;

    void Start() {
        gameObject.transform.position = new Vector3(Random.Range(-40, 40), Random.Range(-20, 20), 0);
    }

    void Update() {
    }

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.layer == playerLayer) {
            Destroy(gameObject);
            GameManager.Instance.Score += points;
        }
    }
}
