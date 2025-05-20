using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPoint : MonoBehaviour
{
    public Collider point;
    public GameObject particle;

    void OnTriggerEnter(Collider other)
    {

        CharacterManager.Instance.Player.controller.canOnPoint = true;
        particle.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        CharacterManager.Instance.Player.controller.canOnPoint = false;
        CharacterManager.Instance.Player.controller.getOn = false;
        ResetGetOn(CharacterManager.Instance.Player.gameObject.GetComponent<Rigidbody>());
        particle.SetActive(false);
    }

    public void OnPointAction()
    {
        Transform posPlayer = CharacterManager.Instance.Player.gameObject.transform;

        Vector3 newPos = new Vector3(point.bounds.min.x, point.bounds.max.y, point.bounds.min.z);

        posPlayer.position = newPos;

        Rigidbody rigidPlayer = CharacterManager.Instance.Player.gameObject.GetComponent<Rigidbody>();
        SetGetOn(rigidPlayer);
    }

    void SetGetOn(Rigidbody rigidPlayer) // x,z,y 축 고정
    {
        rigidPlayer.constraints = RigidbodyConstraints.FreezePosition;
        rigidPlayer.constraints = RigidbodyConstraints.FreezeRotation |
                          RigidbodyConstraints.FreezePositionX |
                          RigidbodyConstraints.FreezePositionZ;
        rigidPlayer.useGravity = false;

        CharacterManager.Instance.Player.controller.jumpCount = CharacterManager.Instance.Player.controller.plusJumpCount;
    }

    public void ResetGetOn(Rigidbody rigidPlayer)
    {
        rigidPlayer.constraints = RigidbodyConstraints.FreezeRotation;
        rigidPlayer.useGravity = true;
    }

}
