using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;

    public int nrCoins;

    public Tilemap coins;
    public Tilemap powers;

    public LayerMask whatStopsMovement;

    public Text text;

    public int score = 0;
    public int scoreCoins = 0;
    private int last_move; // 0-side, 1-up, 2-down

    public int gameState = 0; // 0 - in progress, 1 - won, 2 - lost

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        last_move = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreCoins == nrCoins) // nrCoins - total number of coins
        {
            gameState = 1;
        }

        // AFISARE SCOR
        if (gameState == 0)
        {
            text.text = "SCOR: " + score.ToString();
        }
        else if (gameState == 1)
        {
            text.text = "YOU WON";
            gameObject.SetActive(false);
        }
        else if (gameState == 2)
        {
            text.text = "YOU LOST";
            gameObject.SetActive(false);
        }

        Vector3 scale = transform.localScale;

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            // Contorizare scor
            if (coins.HasTile(coins.WorldToCell(transform.position)))
            {
                score++;
                scoreCoins++;
                //Debug.Log(score);
            }

            coins.SetTile(coins.WorldToCell(transform.position), null);

            // Putere
            if (powers.HasTile(powers.WorldToCell(transform.position)))
            {
                EnemyController.ModeChange();
                EnemyController2.ModeChange();
            }

            powers.SetTile(powers.WorldToCell(transform.position), null);

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {

                last_move = 0;

                if (Input.GetAxisRaw("Horizontal") == -1)
                {
                    scale.x = 4;
                }
                else
                {
                    scale.x = -4;
                }

                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (Input.GetAxisRaw("Vertical") == 1)
                {
                    last_move = 1;
                }
                else
                {
                    last_move = 2;
                }

                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
            }
            else
            {
                GetComponent<Animator>().SetTrigger("idle");
            }

            transform.localScale = scale;
        }
        else
        {
            if (last_move == 0) 
            { 
                GetComponent<Animator>().SetTrigger("move_side");
            }
            else if (last_move == 1)
            {
                GetComponent<Animator>().SetTrigger("move_up");
            }
            else
            {
                GetComponent<Animator>().SetTrigger("move_down");
            }
        }

        
    }
}
