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

    //1 - dreapta, 2 - stanga, 3 - sus, 4 - jos, altceva - sta pe loc
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
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(player.transform.position, playerMovePoint.position) <= .05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                int move = Random.Range(1, 5);
                if(contor < movement.Length)
                {
                    move = movement[contor];
                }

                switch (move)
                {
                    case 1:
                        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(1f, 0f, 0f), .2f, whatStopsMovement))
                        {
                            movePoint.position += new Vector3(1f, 0f, 0f);
                        }
                        break;
                    case 2:
                        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(-1f, 0f, 0f), .2f, whatStopsMovement))
                        {
                            movePoint.position += new Vector3(-1f, 0f, 0f);
                        }
                        break;
                    case 3:
                        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 1f, 0f), .2f, whatStopsMovement))
                        {
                            movePoint.position += new Vector3(0f, 1f, 0f);
                        }
                        break;
                    case 4:
                        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, -1f, 0f), .2f, whatStopsMovement))
                        {
                            movePoint.position += new Vector3(0f, -1f, 0f);
                        }
                        break;
                    default:
                        break;
                }
                contor++;
            }
        }
    }
}
