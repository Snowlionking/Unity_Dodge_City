using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed = 30f;

    private new Rigidbody rigidbody;

    private float horizontalInput;

    private float verticalInput;

    private int money;

    private PowerUp[] powerUps;
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate() {
        if (GameManager.Instance.isInputEnabled) {
            rigidbody.velocity = new Vector3(horizontalInput * speed, verticalInput * speed, 0);
        }
    }
}
