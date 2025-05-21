using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    public float moveSpeed;
    public float locationX;
    public float locationY;
    public float locationZ;
    bool onMove;
    Vector3 baseLocation;
    Transform player;

    void Awake()
    {
        baseLocation = transform.position;
    }

    void Update()
    {
        if (onMove)
        {
            ObjectMove();
        }
        else if (gameObject.transform.position != baseLocation && !onMove)
        {
            ReturnMove();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // player = collision.gameObject.transform;
            collision.transform.SetParent(transform);
            onMove = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            onMove = false;
        }
    }

    void ObjectMove()
    {
        Vector3 location = new Vector3(locationX, locationY, locationZ);
        transform.position = Vector3.MoveTowards(transform.position, location, moveSpeed * Time.deltaTime);

        // player.position = Vector3.MoveTowards(player.position, location, moveSpeed * Time.deltaTime);
    }

    void ReturnMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, baseLocation, moveSpeed * Time.deltaTime);
    }
}
