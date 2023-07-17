using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //public TerrainManager terrainManager;

    private enum Direction
    {
        Up,
        Right,
        Left
    }

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private PlayerInput playerInput;
    private new BoxCollider2D collider;

    [Header("得分")]
    public int stepPoint;
    private int pointResult;

    [Header("跳跃")]
    public float jumpDistance;
    private float moveDistance;
    private bool buttonHeld;
    private bool isJump;
    private bool canJump;

    private Vector2 destination;
    private Vector2 touchPosition;
    private Direction dir;

    private bool isDead;

    [Header("方向指示")]
    public SpriteRenderer signRenderer;
    public Sprite upSign;
    public Sprite leftSign;
    public Sprite rightSign;

    //判断碰撞检测返回的物体
    private RaycastHit2D[] result = new RaycastHit2D[3];

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        collider = GetComponent<BoxCollider2D>();
        signRenderer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isDead)
        {
            DisableInput();
            return;
        }
        if (canJump)
        {
            TriggerJump();
        }
    }

    private void FixedUpdate()
    {
        if(isJump)
            rb.position = Vector2.Lerp(transform.position, destination, 0.134f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water") && !isJump)
        {
            Physics2D.RaycastNonAlloc(transform.position + Vector3.up * 0.2f, Vector2.zero, result);

            bool inWater = true;

            foreach (var hit in result)
            {
                if (hit.collider == null) continue;
                if (hit.collider.CompareTag("Wood"))
                {
                    //跟随木板移动
                    //Debug.Log("On the wood");
                    transform.parent = hit.collider.transform;
                    inWater = false;
                }
            }

            //没有木板游戏结束
            if (inWater)
            {
                Debug.Log("GAME OVER");
                isDead = true;
            }
        }
        if (other.CompareTag("Border") || other.CompareTag("Car"))
        {
            Debug.Log("GAME OVER");
            isDead = true;
        }

        if (!isJump && other.CompareTag("Obstacle"))
        {
            Debug.Log("GAME OVER");
            isDead = true;
        }

        if (isDead)
        {
            //广播通知游戏结束
            EventHandler.CallGameOverEvent();
            collider.enabled = false;
        }
    }

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Wood"))
    //    {
    //        transform.parent = null;
    //    }
    //}

    #region INPUT 输入回调函数
    public void Jump(InputAction.CallbackContext context)
    {
        //执行跳跃，跳跃的距离，记录分数，播放跳跃的音效
        if(context.performed && !isJump)
        {
            //执行跳跃
            moveDistance = jumpDistance;
            //Debug.Log("JUMP!" + " " + moveDistance);
            canJump = true;
            AudioManager.instance.SetJumpClip(0);

            if (dir == Direction.Up)
            {
                pointResult += stepPoint;
            }
        }
    }

    public void LongJump(InputAction.CallbackContext context)
    {
        if (context.performed && !isJump)
        {
            moveDistance = jumpDistance * 2;
            buttonHeld = true;
            AudioManager.instance.SetJumpClip(1);

            signRenderer.gameObject.SetActive(true);
        }

        if (context.canceled && buttonHeld && !isJump)
        {
            //执行跳跃
            //Debug.Log("LONG JUMP!" + " " + moveDistance);
            buttonHeld = false;
            canJump = true;
            signRenderer.gameObject.SetActive(false);

            if (dir == Direction.Up)
            {
                pointResult += stepPoint * 2;
            }
        }
    }

    public void GetTouchPosition(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log(context.ReadValue<Vector2>());
            touchPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            //Debug.Log(touchPosition);
            var offset = ((Vector3)touchPosition - transform.position).normalized;

            if (Mathf.Abs(offset.x) <= 0.7f)
            {
                dir = Direction.Up;
                signRenderer.sprite = upSign;
            }
            else if (offset.x < 0)
            {
                dir = Direction.Left;
                if (transform.localScale.x == -1)
                {
                    signRenderer.sprite = rightSign;
                }
                else
                {
                    signRenderer.sprite = leftSign;
                }
            }
            else if (offset.x > 0)
            {
                dir = Direction.Right;
                if (transform.localScale.x == -1)
                {
                    signRenderer.sprite = leftSign;
                }
                else
                {
                    signRenderer.sprite = rightSign;
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// 触发执行跳跃动画
    /// </summary>
    private void TriggerJump()
    {
        canJump = false;
        //获得移动方向，播放动画
        switch (dir)
        {
            case Direction.Up:
                //触发切换左右方向动画
                anim.SetBool("isSide", false);
                destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
                transform.localScale = Vector3.one;
                //sr.flipX = true;
                break;
            case Direction.Left:
                anim.SetBool("isSide", true);
                destination = new Vector2(transform.position.x - moveDistance, transform.position.y);
                transform.localScale = Vector3.one;
                break;
            case Direction.Right:
                anim.SetBool("isSide", true);
                destination = new Vector2(transform.position.x + moveDistance, transform.position.y);
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }
        anim.SetTrigger("Jump");
    }

    #region Animation Event
    public void JumpAnimationEvent()
    {
        //播放跳跃音效
        AudioManager.instance.PlayJumpFx();
        //改变状态
        isJump = true;
        //修改排序图层
        sr.sortingLayerName = "Front";
        //修改parent
        transform.parent = null;
    }
    public void FinishJumpAnimationEvent()
    {
        isJump = false;
        //修改排序图层
        sr.sortingLayerName = "Middle";

        if(dir == Direction.Up && !isDead)
        {
            //TODO: 得分、触发地图生成检测
            //terrainManager.CheckPosition();

            EventHandler.CallGetPointEvent(pointResult);

            //Debug.Log("总得分" + pointResult); 
        }
    }
    #endregion

    private void DisableInput()
    {
        playerInput.enabled = false;
    }
}
