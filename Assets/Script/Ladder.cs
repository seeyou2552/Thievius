using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Ladder : MonoBehaviour
{
    public Collider way;
    public GameObject particle;

    void OnTriggerEnter(Collider other)
    {
        CharacterManager.Instance.Player.controller.canLadder = true;
        particle.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        CharacterManager.Instance.Player.controller.canLadder = false;
        CharacterManager.Instance.Player.controller.laddering = false;
        ResetClimbing(CharacterManager.Instance.Player.gameObject.GetComponent<Rigidbody>());
        particle.SetActive(false);
    }

    public void LadderAction()
    {
        Transform posPlayer = CharacterManager.Instance.Player.gameObject.transform;

        Vector3 newPos = new Vector3(way.bounds.min.x, posPlayer.position.y + 0.1f, way.bounds.min.z);

        posPlayer.position = newPos;

        Rigidbody rigidPlayer = CharacterManager.Instance.Player.gameObject.GetComponent<Rigidbody>();
        SetClimbing(rigidPlayer);
        LookTarget(posPlayer);
    }

    public void LookTarget(Transform player) // 탈때 물체 바라보기
    {
        Vector3 direction = way.transform.position - player.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            player.rotation = Quaternion.LookRotation(direction);
        }
    }

    void SetClimbing(Rigidbody rigidPlayer) // x,z 축 고정
    {
        rigidPlayer.constraints = RigidbodyConstraints.FreezePosition;
        rigidPlayer.constraints = RigidbodyConstraints.FreezeRotation |
                          RigidbodyConstraints.FreezePositionX |
                          RigidbodyConstraints.FreezePositionZ;
        rigidPlayer.useGravity = false;

        CharacterManager.Instance.Player.controller.jumpCount = CharacterManager.Instance.Player.controller.plusJumpCount;
    }

    public void ResetClimbing(Rigidbody rigidPlayer)
    {
        rigidPlayer.constraints = RigidbodyConstraints.FreezeRotation;
        rigidPlayer.useGravity = true;
    }


}
