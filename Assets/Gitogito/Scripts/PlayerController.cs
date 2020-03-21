using UnityEngine;
using DG.Tweening;

public class PlayerController : Utility
{
    // 人体のアニメーションに使用するトランスフォーム
    [SerializeField] private Transform state, neck, mainCamera, leftHand, rightHand;
    // プレイヤーのパラメーター 移動速度、アニメーション速度、しゃがみの移動速度倍率、ダッシュの移動速度倍率、ジャンプ力、物を掴んだり、扉を開けるときの最大距離
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float animSpeed = 0.2f;
    [SerializeField] private float crouchSpeedMultiper = 0.5f;
    [SerializeField] private float dashSpeedMultiper = 2.0f;
    [SerializeField] private float jumpForce = 3.0f;
    [SerializeField] private float actionDistance = 2.0f;

    // 視点移動用の回転の変数
    private Vector3 mouseRotate;
    // 進行方向を代入する変数
    private Vector3 moveDir;
    // 視点移動の感度
    private float cameraSensitivity = 0.5f;
    // 状態を保持する変数　しゃがんでいるか、ダッシュしているか、左手に何か持っているか、右手に何か持っているか
    private bool isCrouch = false, isDash = false, isLeftHandUp = false, isRightHandUp = false;
    // 物を持ったり、捨てたりする間隔
    private float handUpInterval = -1f;
    private CharacterController controller;
    private Animator animator;

    private void Start ()
    {
        // コンポーネントを代入
        controller = GetComponent<CharacterController> ();
        animator = GetComponent<Animator> ();
        // カーソルを固定して非表示に
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update ()
    {
        // ゲームの状態を取得
        GameState gameState = GameManager.GetState ();

        // ゲームの状態がリザルト中だったら何も出来ない
        if (gameState == GameState.RESULT) { return; }

        // ゲームの開始前だったらカメラ移動だけできる
        LookCamera ();
        if(gameState == GameState.READY) { return; }
        // ここからは、ゲームプレイ中の処理
        // 移動
        Move ();
        // ダッシュ
        Dash ();
        // 攻撃
        Attack ();
        // 反応する物があるかどうかをチェック
        CheckRay ();

        // 接地していたら、しゃがみとジャンプができる
        if (controller.isGrounded)
        {
            Crouch ();
            Jump ();
        }
        else // 接地していないときはしゃがめない (しゃがんでいたら立つ)
        {
            isCrouch = false;
        }

        // 左手か右手に何か持っているとき
        if(isRightHandUp || isLeftHandUp)
        {
            // 間隔が空いていれば離せる
            if (handUpInterval < 0f)
            {
                HandRelease ();
            }
        }

        // 一定時間とったり話したり出来ない (0.5秒間)
        if (handUpInterval > 0f)
        {
            handUpInterval -= Time.deltaTime;
        }

        // しゃがんでいるときは地面に埋まる
        state.DOLocalMoveY (isCrouch ? -0.6f : 0f, animSpeed);
        // アニメーションしている
        Animation ();
    }

    // 移動の関数
    private void Move ()
    {
        // wasdの入力
        float h = Input.GetAxis ("Horizontal");
        float v = Input.GetAxis ("Vertical");
        // 移動速度の計算　しゃがみとダッシュをしている場合はそれらの倍率をかけている
        float speed = moveSpeed * (isCrouch ? crouchSpeedMultiper : 1.0f) * (isDash ? dashSpeedMultiper : 1.0f);
        // 移動方向を単位ベクトルにし、それに速度をかける + 前のyの速度を代入(こうしないと落ちない)
        moveDir = Vector3.Normalize (transform.forward * v + transform.right * h) * speed + Vector3.up * moveDir.y;
        // 重力加速度
        moveDir.y -= 9.8f * Time.deltaTime;
        // キャラクターコントローラーで移動
        controller.Move (moveDir * Time.deltaTime);
    }

    // 視点移動の関数
    private void LookCamera ()
    {
        // マウスの移動量の取得
        float mX = Input.GetAxis("Mouse X") * cameraSensitivity;
        float mY = Input.GetAxis ("Mouse Y") * cameraSensitivity;
        //y軸はそのまま、x軸は角度制限をつけて回転
        mouseRotate = new Vector3 (Mathf.Clamp (mouseRotate.x - mY, -80, 80), mouseRotate.y + mX, 0f);
        //y軸はこのオブジェクト、x軸は首オブジェクトを反映
        transform.eulerAngles = new Vector3 (0, mouseRotate.y, 0);
        neck.localEulerAngles = new Vector3 (mouseRotate.x, 0, 0);
    }

    // しゃがむ関数
    private void Crouch ()
    {
        // フラグを切り替え
        if (Input.GetButtonDown ("Crouch"))
        {
            isCrouch = !isCrouch;
        }
    }

    // ダッシュ関数
    private void Dash ()
    {
        // ダッシュの入力を代入
        isDash = Input.GetButton ("Dash");
    }

    // ジャンプ関数
    private void Jump ()
    {
        // ジャンプが押されたら、ジャンプ力分上に
        if (Input.GetButtonDown ("Jump"))
        {
            moveDir.y = jumpForce;
        }
    }

    // 攻撃関数
    private void Attack ()
    {
        // 攻撃が押されたら、アニメーションさせる
        if (Input.GetButtonDown ("Attack"))
        {
            animator.SetTrigger ("Attack");
        }
    }

    // インタラクティブなオブジェクトのチェック
    private void CheckRay ()
    {
        // レイ
        Ray ray = new Ray (mainCamera.position, mainCamera.forward);
        RaycastHit hit;

        // レイが範囲内で当たっているとき
        if (Physics.Raycast (ray, out hit, actionDistance))
        {
            // レイがヒットしたオブジェクトを代入しておく 参照しやすいように
            GameObject hitObject = hit.collider.gameObject;
            // インタラクトなオブジェクトの時
            if (hitObject.CompareTag ("Interact"))
            {
                // アクションボタンを押したら
                if (Input.GetButtonDown ("Action"))
                {
                    // そのオブジェクトのアクションが行われる
                    hitObject.GetComponent<Animator> ().SetTrigger ("Action");
                    hitObject.GetComponent<InteractSound> ().Play ();
                }
            }

            // レイが当たったオブジェクトの親にRigidBodyが付いていたら
            Rigidbody target; 
            if ((target = FindRigidBodyParent (hitObject)) != null)
            {
                // 左手に持つ
                if (Input.GetButtonDown ("LeftHand"))
                {
                    if (!isLeftHandUp)
                    {
                        target.constraints = RigidbodyConstraints.FreezeAll;
                        HandUp (true, target.transform);
                    }
                }
                // 右手にもつ
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

    // 持つ関数
    private void HandUp (bool left, Transform target)
    {
        // もし持ったのがスマホだったら
        if (target.CompareTag ("SmartPhone"))
        {
            // マネージャーに知らせる
            gameManager.FindSmartPhone ();
        }
        // 左手の時、左手の子にする
        if (left)
        {
            isLeftHandUp = true;
            target.transform.parent = leftHand;
        }
        // 右手の時、右手の子にする
        else
        {
            isRightHandUp = true;
            target.transform.parent = rightHand;
        }
        // 持ったオブジェクトのローカル座標をゼロにする
        target.transform.localPosition = Vector3.zero;
        // プレイヤーと干渉しないよう、こオブジェクトのレイヤーを変える
        Transform[] targetChildren = target.GetComponentsInChildren<Transform> ();
        foreach (Transform t in targetChildren)
        {
            t.gameObject.layer = 2;
        }
        // 0.5秒間は拾ったり、持ったり出来ない
        handUpInterval = 0.5f;
    }

    // 離す関数
    private void HandRelease ()
    {
        // 左手に何か持っている
        if (isLeftHandUp)
        {
            // 左手の拾うボタンを押した
            if (Input.GetButtonDown ("LeftHand"))
            {
                // 左手に持っているオブジェクトの親を無しにし、レイヤーも戻す
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

    // オブジェクトの親オブジェクトにRigidBodyがあればそれを、なければnullを返す
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

    // アニメーションを変更する関数
    private void Animation ()
    {
        // プレイヤーの速度によってアニメーションが変わる
        animator.SetFloat ("Velocity", controller.isGrounded ? controller.velocity.magnitude : 0f);
    }
}
