using UnityEngine;
using DG.Tweening;

public class PlayerController : Utility
{
    [SerializeField] private Transform state, neck, mainCamera, leftHand, rightHand;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float animSpeed = 0.2f;
    [SerializeField] private float crouchSpeedMultiper = 0.5f;
    [SerializeField] private float dashSpeedMultiper = 2.0f;
    [SerializeField] private float jumpForce = 3.0f;
    [SerializeField] private float actionDistance = 2.0f;

    private Vector3 mouseRotate;
    private Vector3 moveDir;
    private float cameraSensitivity = 0.5f;
    private bool isCrouch = false, isDash = false, isLeftHandUp = false, isRightHandUp = false;
    private float handUpInterval = -1f;

    private CharacterController controller;
    private Animator animator;

    private void Start ()
    {
        controller = GetComponent<CharacterController> ();
        animator = GetComponent<Animator> ();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update ()
    {
        GameState gameState = GameManager.GetPlayerMoveAble ();

        if (gameState == GameState.RESULT) { return; }

        LookCamera ();

        if(gameState == GameState.READY) { return; }

        Move ();
        Dash ();
        Attack ();
        CheckRay ();

        if (controller.isGrounded)
        {
            Crouch ();
            Jump ();
        }
        else
        {
            isCrouch = false;
        }

        if(isRightHandUp || isLeftHandUp)
        {
            if (handUpInterval < 0f)
            {
                HandRelease ();
            }
        }

        if (handUpInterval > 0f)
        {
            handUpInterval -= Time.deltaTime;
        }

        state.DOLocalMoveY (isCrouch ? -0.6f : 0f, animSpeed);
        Animation ();
    }

    private void Move ()
    {
        float h = Input.GetAxis ("Horizontal");
        float v = Input.GetAxis ("Vertical");
        float speed = moveSpeed * (isCrouch ? crouchSpeedMultiper : 1.0f) * (isDash ? dashSpeedMultiper : 1.0f);
        moveDir = Vector3.Normalize (transform.forward * v + transform.right * h) * speed + Vector3.up * moveDir.y;
        moveDir.y -= 9.8f * Time.deltaTime;
        controller.Move (moveDir * Time.deltaTime);
    }

    private void LookCamera ()
    {
        //MobileInputのslideDeltaに感度をかけて代入
        float mX = Input.GetAxis("Mouse X") * cameraSensitivity;
        float mY = Input.GetAxis ("Mouse Y") * cameraSensitivity;
        //xは-80〜80度の範囲で回転、yは無限に回転
        mouseRotate = new Vector3 (Mathf.Clamp (mouseRotate.x - mY, -80, 80), mouseRotate.y + mX, 0f);
        //y軸はこのオブジェクト、x軸は首オブジェクトを反映
        transform.eulerAngles = new Vector3 (0, mouseRotate.y, 0);
        neck.localEulerAngles = new Vector3 (mouseRotate.x, 0, 0);
    }

    private void Crouch ()
    {
        if (Input.GetButtonDown ("Crouch"))
        {
            isCrouch = !isCrouch;
        }
    }

    private void Dash ()
    {
        isDash = Input.GetButton ("Dash");
    }

    private void Jump ()
    {
        if (Input.GetButtonDown ("Jump"))
        {
            moveDir.y = jumpForce;
        }
    }

    private void Attack ()
    {
        if (Input.GetButtonDown ("Attack"))
        {
            animator.SetTrigger ("Attack");
        }
    }

    private void CheckRay ()
    {
        Ray ray = new Ray (mainCamera.position, mainCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast (ray, out hit, actionDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag ("Interact"))
            {
                if (Input.GetButtonDown ("Action"))
                {
                    hitObject.GetComponent<Animator> ().SetTrigger ("Action");
                    hitObject.GetComponent<InteractSound> ().Play ();
                }
            }

            Rigidbody target; 
            if ((target = FindRigidBodyParent (hitObject)) != null)
            {
                if (Input.GetButtonDown ("LeftHand"))
                {
                    if (!isLeftHandUp)
                    {
                        target.constraints = RigidbodyConstraints.FreezeAll;
                        HandUp (true, target.transform);
                    }
                }
                else if (Input.GetButtonDown ("RightHand"))
                {
                    if (!isRightHandUp)
                    {
                        target.constraints = RigidbodyConstraints.FreezeAll;
                        HandUp (false, target.transform);
                    }
                }
            }
        }
    }

    private void HandUp (bool left, Transform target)
    {
        if (target.CompareTag ("SmartPhone"))
        {
            gameManager.FindSmartPhone ();
        }
        if (left)
        {
            isLeftHandUp = true;
            target.transform.parent = leftHand;
        }
        else
        {
            isRightHandUp = true;
            target.transform.parent = rightHand;
        }
        target.transform.localPosition = Vector3.zero;
        Transform[] targetChildren = target.GetComponentsInChildren<Transform> ();
        foreach (Transform t in targetChildren)
        {
            t.gameObject.layer = 2;
        }
        handUpInterval = 0.5f;
    }

    private void HandRelease ()
    {
        if (isLeftHandUp)
        {
            if (Input.GetButtonDown ("LeftHand"))
            {
                Transform child = leftHand.GetChild (0);
                Transform[] targetChildren = child.GetComponentsInChildren<Transform> ();
                foreach (Transform t in targetChildren)
                {
                    t.gameObject.layer = 0;
                }
                child.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
                child.parent = null;
                isLeftHandUp = false;
            }
        }

        if (isRightHandUp)
        {
            if (Input.GetButtonDown ("RightHand"))
            {
                Transform child = rightHand.GetChild (0);
                Transform[] targetChildren = child.GetComponentsInChildren<Transform> ();
                foreach (Transform t in targetChildren)
                {
                    t.gameObject.layer = 0;
                }
                child.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
                child.parent = null;
                isRightHandUp = false;
            }
        }
    }

    private Rigidbody FindRigidBodyParent (GameObject child)
    {
        Rigidbody rigidParent = child.GetComponentInParent<Rigidbody> ();
        if(rigidParent == null)
        {
            return null;
        }
        else
        {
            return rigidParent;
        }
    }

    private void Animation ()
    {
        animator.SetFloat ("Velocity", controller.isGrounded ? controller.velocity.magnitude : 0f);
    }
}
