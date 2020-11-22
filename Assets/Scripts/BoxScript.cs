using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    private float min_x = -2.2f, max_x = 2.2f;

    private bool canMove;
    private float move_speed = 2f;
    private Rigidbody2D myBody;
    private bool gameOver;
    private bool ignoreCollision;
    private bool ignoreTrigger;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        myBody.gravityScale = 0f;
    }

    private void Start()
    {
        canMove = true;

        if(Random.Range(0, 2) > 0)
        {
            move_speed *= -1f;

        }
        GameplayController.instance.currentBox = this;
    }
    private void Update()
    {
        MoveBox();
    }
    void MoveBox()
    {
        if(canMove)
        {
            Vector3 temp = transform.position;


            temp.x += move_speed * Time.deltaTime;

            if(temp.x > max_x)
            {
                move_speed *= -1f;
            } else if (temp.x < min_x)
            {
                move_speed *= -1f;
            }

            transform.position = temp;

        }
    }

    public void DropBox()
    {
        canMove = false;
        myBody.gravityScale = Random.Range(2, 4);
    }

    void Landed()
    {
        if (gameOver)
            return;

        ignoreCollision = true;
        ignoreTrigger = true;

        GameplayController.instance.SpawnNewBox();
        GameplayController.instance.MoveCamera();
    }

    void RestartGame()
    {
        GameplayController.instance.RestartGame();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ignoreCollision)
            return;

        if(collision.gameObject.tag == "Box" || collision.gameObject.tag == "Platform" )
        {
            Invoke("Landed", 1f);
            ignoreCollision = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ignoreTrigger)
            return;

        if(collision.tag == "GameOver")
        {
            CancelInvoke("Landed");
            gameOver = true;
            ignoreTrigger = true;

            Invoke("RestartGame", 2f);
        }
    }
}
