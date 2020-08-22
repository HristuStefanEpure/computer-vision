using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int direction;

    public GameObject player;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            this.player = player;
        }

        timer = 8f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }

        switch (direction)
        {
            case 1:
                transform.position += new Vector3(0, 5 * Time.deltaTime, 0);
                break;
            case 3:
                transform.position += new Vector3(-5 * Time.deltaTime, 0, 0);
                break;
            case 4:
                transform.position += new Vector3(5 * Time.deltaTime, 0, 0);
                break;
            default:
                transform.position += new Vector3(0, -5 * Time.deltaTime, 0);
                break;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= .5)
        {
            Time.timeScale = 0f;
            player.GetComponent<PlayerController>().gameState = 2;

        }
    }
}
