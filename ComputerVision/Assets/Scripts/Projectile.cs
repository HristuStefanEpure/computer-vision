using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        
    }
}
