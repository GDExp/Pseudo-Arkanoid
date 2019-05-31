using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Ball game_ball;
    private Rigidbody2D rb_body;
    private float y_start_position;
    [SerializeField]
    private float x_limit;
    [SerializeField]
    private float force;
    [SerializeField]
    private sbyte side;



    private void Start()
    {
        SetupBoard();
    }

    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
            side = (sbyte)((Input.GetAxisRaw("Horizontal") > 0) ? 1 : -1);
        else
            side = 0;
        if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space))
            game_ball.StartBall();
        Movement();
    }
    
    private void SetupBoard()
    {
        rb_body = GetComponent<Rigidbody2D>();
        game_ball = FindObjectOfType<Ball>();
        game_ball.transform.SetParent(transform.GetChild(0));
        game_ball.transform.localPosition = Vector2.zero;

        y_start_position = transform.localPosition.y;
        transform.localPosition = new Vector2(0f, y_start_position);
    }

    private void Movement()
    {
        rb_body.velocity = Vector2.zero;
        if (side == 0) return;
        rb_body.velocity = Vector2.right * side * force;
    }

    public void ResetBoard()
    {
        transform.localPosition = new Vector2(0f, y_start_position);
        game_ball.ResetBall();
        game_ball.transform.SetParent(transform.GetChild(0));
        game_ball.transform.localPosition = Vector2.zero;
    }
}
