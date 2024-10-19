using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Movement variables
    [Header("Movement parameters")]
    [SerializeField] float speed = 500f;
    [SerializeField] float sprintSpeedMultiplier = 1.75f;
    float horizontalMove;
    float verticalMove;

    //Attacking variables
    [Header("Attack elements")]
  //  [SerializeField] GameObject attackingHitBox;
    [SerializeField] float attackingTime = 1;
    public float attackDamage = 10;
    bool isAttacking = false;

    //Input variables
    [Header("Input settings")]
    [SerializeField] InputActionAsset actionAsset;
    InputActionMap actionMap;
    InputAction horizontalMoveAction;
    InputAction verticalMoveAction;
    InputAction sprintAction;
    InputAction attackAction;
    Rigidbody2D rigidbody2;

    void Awake()
    {
        actionMap = actionAsset.FindActionMap("Player");
        horizontalMoveAction = actionMap.FindAction("HorizontalMove");
        verticalMoveAction = actionMap.FindAction("VerticalMove");
        sprintAction = actionMap.FindAction("Sprint");
        attackAction = actionMap.FindAction("Attack");

        verticalMoveAction.performed += GetVerticalMove;
        verticalMoveAction.canceled += GetVerticalMove;

        horizontalMoveAction.performed += GetHorizontalMove;
        horizontalMoveAction.canceled += GetHorizontalMove;

        sprintAction.performed += IncreseSpeed;
        sprintAction.canceled += DecreaseSpeed;

        attackAction.performed += StartAttack;
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
    }

    private void OnDisable()
    {
        horizontalMoveAction.Disable();
        verticalMoveAction.Disable();
        sprintAction.Disable();
        attackAction.Disable();
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
        /*isAttacking = true;
        attackingHitBox.SetActive(true);*/
        yield return new WaitForSeconds(attackingTime);
       /* attackingHitBox.SetActive(false);
        isAttacking = false;*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    //Moves the player
    void Move()
    {
        Vector3 direction = new Vector2(verticalMove, horizontalMove);
        rigidbody2.velocity = speed * Time.deltaTime * direction.normalized;
        Debug.Log("Velocidad: "+ rigidbody2.velocity.magnitude);
    }
}
