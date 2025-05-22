using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;
    public float jumpPower;
    public int jumpCount;
    public int plusJumpCount;
    public bool dashing = false;
    public float dashStamina;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;


    [Header("Look")]
    public Transform cameraContainer;
    public Transform camera;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    public Vector2 mouseDelta;
    public bool canLook = true;
    public bool thirdView;

    [Header("Ladder Action")]
    public bool canLadder;
    public bool laddering;
    public Ladder ladder;

    [Header("OnPoint Action")]
    public bool canOnPoint;
    public bool getOn;
    public OnPoint onPoint;

    [Header("Animation")]
    public Animator animator;

    public Action inventory;
    private Rigidbody _rigid;
    private PlayerStat stat;

    public void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        stat = GetComponent<PlayerStat>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();
    }
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
            animator.SetBool("Idle", false);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);

        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
        else if (context.phase == InputActionPhase.Started && jumpCount >= 1)
        {
            _rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            jumpCount--;
            if (laddering)
            {
                ladder.ResetClimbing(gameObject.GetComponent<Rigidbody>());
                laddering = false;
            }
            else if (getOn)
            {
                onPoint.ResetGetOn(gameObject.GetComponent<Rigidbody>());
                getOn = false;
            }
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            dashing = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            dashing = false;
            animator.SetBool("Run", false);
        }

    }

    public void OnAction(InputAction.CallbackContext context) // 특정 상황에서 G키 입력시 특수 상호작용
    {
        if (context.phase == InputActionPhase.Started && canLadder && !laddering)
        {
            laddering = true;
            ladder.LadderAction();
        }
        else if (context.phase == InputActionPhase.Started && canOnPoint && !getOn)
        {
            getOn = true;
            onPoint.OnPointAction();
        }
    }

    public void OnThirdView(InputAction.CallbackContext context) // 마우스 우클릭 시 3인칭 시점 전환
    {
        if (context.phase == InputActionPhase.Started && !thirdView)
        {
            camera.localPosition = new Vector3(0f, 1.5f, -3f);
            thirdView = true;
        }
        else if (context.phase == InputActionPhase.Started && thirdView)
        {
            camera.localPosition = new Vector3(0f, 1.5f, 0f);
            thirdView = false;
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }


    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }



    void Move()
    {
        if (!laddering)
        {
            Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
            if (dashing && stat.CheckStamina())
            {
                dir *= (moveSpeed + stat.plusSpeed + 5);
                stat.UseStamina(dashStamina);
                animator.SetBool("Run", !animator.GetBool("Idle"));

            }
            else dir *= (moveSpeed + stat.plusSpeed);
            dir.y = _rigid.velocity.y;

            if (!stat.CheckStamina())
            {
                animator.SetBool("Run", false);
            }

            if (curMovementInput != Vector2.zero)
            {
                animator.SetBool("Walk", !animator.GetBool("Run"));
            }

            _rigid.velocity = dir;
        }
        else if (laddering)
        {
            Vector3 dir = transform.up * curMovementInput.y;
            dir *= moveSpeed;

            _rigid.velocity = dir;

            if (IsGrounded())
            {
                laddering = false;
                ladder.ResetClimbing(gameObject.GetComponent<Rigidbody>());
            }
        }

    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                jumpCount = plusJumpCount;
                return true;
            }

        }
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (canLadder && other.CompareTag("Ladder"))
        {
            ladder = other.GetComponent<Ladder>();
        }
        else if (canOnPoint && other.CompareTag("OnPoint"))
        {
            onPoint = other.GetComponent<OnPoint>();
        }
    }
}
