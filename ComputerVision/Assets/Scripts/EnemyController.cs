using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;

    public LayerMask whatStopsMovement;

    public GameObject player;
    public Transform playerMovePoint;

    // 1 - dreapta, 2 - stanga, 3 - sus, 4 - jos, altceva - sta pe loc -> schimbat
    // 0 - hold, 1 - up, 2 - down, 3 - left, 4 - right
    public int[] movement;

    private int contor = 0;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        //GAME OVER
        if (Vector3.Distance(transform.position, player.transform.position) <= .05)
        {
            player.SetActive(false);
            Time.timeScale = 0f;
        }

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                /*int move = Random.Range(1, 5);
                if(contor < movement.Length)
                {
                    move = movement[contor];
                }*/

                int move = -1;

                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                Vector3 playerPosition = playerObject.transform.position;

                Vector3 enemyPosition = transform.position;

                move = DecisionTree(playerPosition, enemyPosition);

                switch (move)
                {
                    case 0: // hold
                        // the player dies or the enemy dies
                        break;
                    case 1: // up
                        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 1f, 0f), .2f, whatStopsMovement))
                        {
                            movePoint.position += new Vector3(0f, 1f, 0f);
                        }
                        break;
                    case 2: // down
                        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, -1f, 0f), .2f, whatStopsMovement))
                        {
                            movePoint.position += new Vector3(0f, -1f, 0f);
                        }
                        break;
                    case 3: // left
                        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(-1f, 0f, 0f), .2f, whatStopsMovement))
                        {
                            movePoint.position += new Vector3(-1f, 0f, 0f);
                        }
                        break;
                    case 4: // right
                        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(1f, 0f, 0f), .2f, whatStopsMovement))
                        {
                            movePoint.position += new Vector3(1f, 0f, 0f);
                        }
                        break;
                    default: // error
                        break;
                }

                //contor++;
            }
        }
    }

    int DecisionTree(Vector3 playerPosition, Vector3 enemyPosition)
    {
        int move = -1;

        float x_dif = enemyPosition.x - playerPosition.x;
        float y_dif = enemyPosition.y - playerPosition.y;

        if (y_dif <= -0.5)
            move = 1;
        else if (y_dif <= 0.5)
        {
            if (x_dif <= -0.5)
                move = 4;
            else if (x_dif <= 0.5)
                move = 0;
            else
                move = 3;
        }
        else
            move = 2;

        return move;
    }
}
