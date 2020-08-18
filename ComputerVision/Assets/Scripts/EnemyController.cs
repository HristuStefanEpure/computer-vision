using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 0 - hold, 1 - up, 2 - down, 3 - left, 4 - right

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;

    public LayerMask whatStopsMovement;

    public GameObject player;
    // public Transform playerMovePoint;

    public Vector3 enemyPosition;
    public Vector3 enemyMovePointPosition;
    public Color spriteRendererColor;

    private SpriteRenderer spriteRenderer;
    public Sprite face;
    public Sprite back;
    public Sprite side;

    // Acest timer determina modul de jos. > 0 - normal, <= 0 - power
    private static float timer = 0;

    public int direction = -1;

    public const int HOLD = 0;
    public const int UP = 1;
    public const int DOWN = 2;
    public const int LEFT = 3;
    public const int RIGHT = 4;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        enemyPosition = transform.position;
        enemyMovePointPosition = movePoint.position;
        spriteRendererColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().gameState == 1)
            return;

        Vector3 scale = transform.localScale;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = spriteRendererColor;
        }

        //GAME OVER
        if (Vector3.Distance(transform.position, player.transform.position) <= .5)
        {
            if (timer <= 0) //daca e modul normal
            {
                //player.SetActive(false);
                Time.timeScale = 0f;
                player.GetComponent<PlayerController>().gameState = 2;
            }
            else //daca e modul putere
            {
                player.GetComponent<PlayerController>().score += 100;
                //gameObject.SetActive(false);
                transform.position = enemyPosition;
                movePoint.position = enemyMovePointPosition;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            //if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                int moveTree = -1;
                List<int> moves = new List<int>();
                int move = -1;
                bool moveDone = false;
                int i = 0;

                moveTree = DecisionTree(player.transform.position, transform.position);
                moves = BuildMoves(moveTree);

                while (!moveDone)
                {
                    move = moves[i];

                    switch (move)
                    {
                        case HOLD: // hold
                            // the player dies or the enemy dies
                            break;
                        case UP: // up
                            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 1f, 0f), .2f, whatStopsMovement))
                            {
                                movePoint.position += new Vector3(0f, 1f, 0f);
                                direction = UP;
                                moveDone = true;
                                spriteRenderer.sprite = back;
                            }
                            break;
                        case DOWN: // down
                            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, -1f, 0f), .2f, whatStopsMovement))
                            {
                                movePoint.position += new Vector3(0f, -1f, 0f);
                                direction = DOWN;
                                moveDone = true;
                                spriteRenderer.sprite = face;
                            }
                            break;
                        case LEFT: // left
                            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(-1f, 0f, 0f), .2f, whatStopsMovement))
                            {
                                movePoint.position += new Vector3(-1f, 0f, 0f);
                                direction = LEFT;
                                moveDone = true;
                                scale.x = 4;
                                spriteRenderer.sprite = side;
                            }
                            break;
                        case RIGHT: // right
                            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(1f, 0f, 0f), .2f, whatStopsMovement))
                            {
                                movePoint.position += new Vector3(1f, 0f, 0f);
                                direction = RIGHT;
                                moveDone = true;
                                scale.x = -4;
                                spriteRenderer.sprite = side;
                            }
                            break;
                        default: // error
                            break;
                    }

                    i++;
                }
            }
        }

        transform.localScale = scale;
    }

    //functia care schimba modul de joc
    public static void ModeChange()
    {
        timer = 4; // secunde modul power
    }

    /*int DecisionTree(Vector3 playerPosition, Vector3 enemyPosition)
    {
        int move = -1;

        float x_dif = enemyPosition.x - playerPosition.x;
        float y_dif = enemyPosition.y - playerPosition.y;

        if (y_dif <= -0.5)
            move = UP;
        else if (y_dif <= 0.5)
        {
            if (x_dif <= -0.5)
                move = RIGHT;
            else if (x_dif <= 0.5)
                move = HOLD;
            else
                move = LEFT;
        }
        else
            move = DOWN;

        return move;
    }*/

    int DecisionTree(Vector3 playerPosition, Vector3 enemyPosition)
    {
        int move = -1;

        float x_dif;
        float y_dif;

        if (timer <= 0)
        {
            x_dif = enemyPosition.x - playerPosition.x;
            y_dif = enemyPosition.y - playerPosition.y;
        }
        else
        {
            x_dif = playerPosition.x - enemyPosition.x;
            y_dif = playerPosition.y - enemyPosition.y;
        }

        if (y_dif <= -0.5)
        {
            if (x_dif <= -0.5)
                move = 14;
            else if (x_dif > -0.5)
            {
                if (x_dif <= 0.5)
                    move = 1;
                else if (x_dif > 0.5)
                    move = 13;
            }
        }
        else if (y_dif > -0.5)
        {
            if (y_dif <= 0.5)
            {
                if (x_dif <= -0.5)
                    move = 4;
                else if (x_dif > -0.5)
                {
                    if (x_dif <= 0.5)
                        move = 0;
                    else if (x_dif > 0.5)
                        move = 3;
                }
            }
            else if (y_dif > 0.5)
            {
                if (x_dif <= -0.5)
                    move = 24;
                else if (x_dif > -0.5)
                {
                    if (x_dif <= 0.5)
                        move = 2;
                    else if (x_dif > 0.5)
                        move = 23;
                }
            }
        }

        return move;
    }

    List<int> BuildMoves(int move)
    {
        List<int> moves = new List<int>();
        int antiDirection = -1;
        int i = 0;

        int first = move / 10;
        int second = move % 10;

        if (first != 0)
            moves.Add(first);

        moves.Add(second);

        for (i = UP; i <= RIGHT; i++)
        {
            if (!moves.Contains(i))
                moves.Add(i);
        }

        antiDirection = AntiDirection(direction);
        moves.Remove(antiDirection);

        return moves;
    }

    int AntiDirection(int direction)
    {
        int antiDirection = -1;

        switch (direction)
        {
            case UP:
                antiDirection = DOWN;
                break;
            case DOWN:
                antiDirection = UP;
                break;
            case LEFT:
                antiDirection = RIGHT;
                break;
            case RIGHT:
                antiDirection = LEFT;
                break;
            default:
                antiDirection = -1;
                break;
        }

        return antiDirection;
    }
}
