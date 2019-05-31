using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb_body;
    [SerializeField]
    private PhysicsMaterial2D material;
    [SerializeField]
    private float force;
    private bool in_game;



    private void Start()
    {
        Setup();
    }
    //controll ball velocity 
    private void FixedUpdate()
    {
        //print(rb_body.velocity);
        if (Mathf.Abs(rb_body.velocity.x) > 7f)
            rb_body.velocity = new Vector2((rb_body.velocity.x > 0) ? 7f : -7f, rb_body.velocity.y);
        if (Mathf.Abs(rb_body.velocity.y) != 10f && rb_body.velocity != Vector2.zero)
            rb_body.velocity = new Vector2(rb_body.velocity.x, (rb_body.velocity.y > 0) ? 10f : -10f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Brick"))
            GameController.init.BrickHit(collision.gameObject);

        if (collision.transform.CompareTag("Player"))
        {
            float x_value = collision.gameObject.GetComponent<Rigidbody2D>().velocity.x;
            rb_body.velocity += Vector2.right * x_value;
        }
    }

    private void Setup()
    {
        tag = "Ball";
        rb_body = GetComponent<Rigidbody2D>();
        rb_body.isKinematic = true;
        rb_body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;//big speed more detection
        material = rb_body.sharedMaterial;
        material.bounciness = 1f;
    }

    public void StartBall()
    {
        if (in_game) return;
        transform.SetParent(null);
        rb_body.isKinematic = false;
        rb_body.AddForce(new Vector2(Random.Range(-35f, 35f), 50f), ForceMode2D.Force);
        in_game = true;
    }

    public void ResetBall()
    {
        in_game = false;
        rb_body.isKinematic = true;
        rb_body.velocity = Vector2.zero;
        rb_body.angularVelocity = 0f;
    }
}
