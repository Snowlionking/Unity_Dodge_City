using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int damage;
    public float speed;

    public new Rigidbody rigidbody;

    private Vector3 velocity;

    private Transform position;

    private readonly int playerLayer = 9;

    void Start() {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        gameObject.transform.position = new Vector3(Random.Range(-40, 40), Random.Range(-20, 20), 0);
        rigidbody.velocity = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
    }

    void FixedUpdate() {
        rigidbody.velocity = rigidbody.velocity.normalized * speed;
        velocity = rigidbody.velocity;
    }

    public void OnCollisionEnter(Collision collision) {
        rigidbody.velocity = Vector3.Reflect(velocity, collision.contacts[0].normal);
        if (collision.gameObject.layer == playerLayer) {
            GameManager.Instance.EnemyDestroyed(gameObject);
            Destroy(gameObject);
            GameManager.Instance.Health -= damage;
        }
    }
}
