using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Movement variables
    [Header("Movement parameters")]
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeedMultiplier = 1.75f;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    float horizontalMove;
    float verticalMove;
    Vector2 playerDirection;
    Vector2 lastPlayerDirection;

    //Attacking variables
    [Header("Attack elements")]
    [SerializeField] GameObject attackingHitBox;
    [SerializeField] float attackingTime = 1;
    public float attackDamage = 10;
    bool isAttacking = false;
    public ThrowableItem currentItem;
    [SerializeField] Transform throwPos;
    [SerializeField] float throwForce = 3;
    bool isCarrying;

    //Input variables
    [Header("Input settings")]
    [SerializeField] InputActionAsset actionAsset;
    InputActionMap actionMap;
    InputAction horizontalMoveAction;
    InputAction verticalMoveAction;
    InputAction sprintAction;
    InputAction attackAction;
    InputAction GrabAction;
    Rigidbody2D rigidbody2;

    void Awake()
    {
        actionMap = actionAsset.FindActionMap("Player");
        horizontalMoveAction = actionMap.FindAction("HorizontalMove");
        verticalMoveAction = actionMap.FindAction("VerticalMove");
        sprintAction = actionMap.FindAction("Sprint");
        attackAction = actionMap.FindAction("Attack");
        GrabAction = actionMap.FindAction("Grab");

        verticalMoveAction.performed += GetVerticalMove;
        verticalMoveAction.canceled += GetVerticalMove;

        horizontalMoveAction.performed += GetHorizontalMove;
        horizontalMoveAction.canceled += GetHorizontalMove;

        sprintAction.performed += IncreseSpeed;
        sprintAction.canceled += DecreaseSpeed;

        attackAction.performed += StartAttack;
        GrabAction.performed += Grab;
    }

    private void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        horizontalMoveAction.Enable();
        verticalMoveAction.Enable();
        sprintAction.Enable();
        attackAction.Enable();
        GrabAction.Enable();
    }

    private void OnDisable()
    {
        horizontalMoveAction.Disable();
        verticalMoveAction.Disable();
        sprintAction.Disable();
        attackAction.Disable();
        GrabAction.Disable();
    }

    //Read values from horizontal and vertical input axis.
    void GetHorizontalMove(InputAction.CallbackContext ctx)
    {
        horizontalMove = ctx.ReadValue<float>();

    }
    void GetVerticalMove(InputAction.CallbackContext ctx)
    {
        verticalMove = ctx.ReadValue<float>();
    }

    void IncreseSpeed(InputAction.CallbackContext ctx)
    {
        speed *= sprintSpeedMultiplier;
    }
    void DecreaseSpeed(InputAction.CallbackContext ctx)
    {
        speed /= sprintSpeedMultiplier;
    }

    void StartAttack(InputAction.CallbackContext ctx)
    {
        if (isAttacking)
        {
            return;
        }

        StartCoroutine(Attack());
    }

    //Attack sequence
    IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        isAttacking = true;
        attackingHitBox.SetActive(true);
        yield return new WaitForSeconds(attackingTime);
        //attackingHitBox.SetActive(false);
        isAttacking = false;
    }

    private void Update()
    {
        if (playerDirection.magnitude != 0)
        {
            lastPlayerDirection = playerDirection;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    //Moves the player
    void Move()
    {
        playerDirection = new Vector2(verticalMove, horizontalMove);
        CalculateSpriteDirection();
        // rigidbody2.AddForce(speed * Time.deltaTime * direction.normalized);
        // transform.Translate(speed * Time.deltaTime * direction.normalized);
        rigidbody2.velocity = speed * playerDirection.normalized;
        // Debug.Log("Velocidad: " + rigidbody2.velocity.magnitude);
    }

    void CalculateSpriteDirection()
    {
        if (playerDirection.magnitude != 0)
        {
            animator.SetBool("Run", true);
        }
        else
            animator.SetBool("Run", false);

        if (playerDirection.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            return;
        }

        if (playerDirection.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void Grab(InputAction.CallbackContext ctx)
    {
        if (currentItem == null) return;

        if (isCarrying)
        {
            currentItem.DetachParent();
            CalculateSpriteDirection();
            currentItem.Throw(CalculatePlayerDirection());
            isCarrying = false;
            return;
        }

        currentItem.rb.isKinematic = true;
        currentItem.transform.position = throwPos.position;
        currentItem.transform.SetParent(throwPos);
        isCarrying = true;
    }

    Vector2 CalculatePlayerDirection()
    {
        Vector2 ThrowDirection;
        if (playerDirection.magnitude != 0)
        {
            ThrowDirection = playerDirection * throwForce;
        }
        else
        {
            ThrowDirection = lastPlayerDirection.normalized * throwForce;
        }
        return ThrowDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ThrowableItem") && !isCarrying)
        {
            currentItem = collision.gameObject.GetComponent<ThrowableItem>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ThrowableItem") && !isCarrying)
        {
            currentItem = null;
        }
    }
}
