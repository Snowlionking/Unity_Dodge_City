using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackEnemy : Enemy {


    private Transform player;

    private void Start() {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameObject.transform.position = new Vector3(Random.Range(-40, 40), Random.Range(-20, 20), 0);
    }

    private void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

}
