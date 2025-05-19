using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{
    public float jumpPower;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Foot"))
        {
            Rigidbody rigid;
            rigid = other.transform.parent.gameObject.GetComponent<Rigidbody>();
            rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }
}
