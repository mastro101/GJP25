using Cinemachine;
using Eastermaster;
using Eastermaster.Helper;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [SerializeInterface(typeof(IStaminable))] GameObject playerData;
    [SerializeField] float speed = 5;
    [SerializeField] float jumpForce = 7.5f;
    [SerializeField] float jumpStaminaCost = 2;
    [SerializeField] float dashForce = 15;
    [SerializeField] float dashDuration = .75f;
    [SerializeField] float dashStaminaCost = 2;
    [SerializeField] float dashCooldown = 2;
    [SerializeField] float attackRange = 1;
    [SerializeField] int attackDamage = 1;
    [SerializeField] float attackDashForce = 15;
    [SerializeField] float attackDuration = .75f;
    [SerializeField] float attackStaminaCost = 2;
    [SerializeField] float attackCooldown = 2;
    [SerializeField] Vector3 attackOffset;
    [SerializeField] CinemachineFreeLook freeCamera;
    [SerializeField] CinemachineVirtualCamera lockOnCamera;
    [SerializeField] PlayerStateMachine stateMachine;

    //[SerializeField] float cameraSpeed;
    public System.Action onDashStarted;
    public System.Action onJumpStarted;
    public System.Action onAttackStarted;

    Rigidbody rb;
    IStaminable staminaData;
    IDamageable damageData;

    //view
    Camera cam;
    ViewTrigger viewTriggerForLockOn;
    IDetectable currentDetectable;
    bool lockOn;
    //

    //input event
    InputAction moveAction;
    InputAction lookAction;
    InputAction lockAction;
    InputAction jumpAction;
    InputAction dashAction;
    InputAction attackAction;
    //

    bool canMove;
    bool canDash;
    bool canAttack;

    bool _onFloor = false;
    bool onFloor
    {
        get => _onFloor;
        set
        {
            _onFloor = value;
            stateMachine.SetBool("OnFloor", _onFloor);
        }
    }
    float dashCooldownTimer;
    float dashDurationTimer;
    float attackCooldownTimer;
    float attackDurationTimer;

    Vector2 inputDirection;
    Vector3 center => transform.position;
    Vector3 inputDirectionFromCameraView => new Quaternion(0, cam.transform.rotation.y, 0, cam.transform.rotation.w) * new Vector3(inputDirection.x, 0, inputDirection.y).normalized;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        staminaData = playerData.GetComponent<IStaminable>();
        damageData = playerData.GetComponent<IDamageable>();
        cam = Camera.main;
        viewTriggerForLockOn = cam.GetComponent<ViewTrigger>();
        moveAction = InputSystem.actions.FindAction("Player/Move");
        lookAction = InputSystem.actions.FindAction("Player/Look");
        lockAction = InputSystem.actions.FindAction("Player/Lock");
        lockAction.performed += LockAction_performed;
        jumpAction = InputSystem.actions.FindAction("Player/Jump");
        jumpAction.performed += JumpAction_performed;
        dashAction = InputSystem.actions.FindAction("Player/Dash");
        dashAction.performed += DashAction_performed;
        attackAction = InputSystem.actions.FindAction("Player/Attack");
        attackAction.performed += AttackAction_performed;

        dashCooldownTimer = 0;
        dashDurationTimer = 0;
        attackCooldownTimer = 0;
        attackDurationTimer = 0;
        lockOn = false;
    }

    private void OnEnable()
    {
        InputUser.onChange += InputUser_onChange;
    }

    private void OnDisable()
    {
        InputUser.onChange -= InputUser_onChange;
        lockAction.performed -= LockAction_performed;
        jumpAction.performed -= JumpAction_performed;
        dashAction.performed -= DashAction_performed;
        attackAction.performed -= AttackAction_performed;
    }

    Vector3 dashDirection = Vector3.zero;
    private void DashAction_performed(InputAction.CallbackContext obj)
    {
        if (dashCooldownTimer <= 0 && inputDirection != Vector2.zero && staminaData.Stamina > 1)
        {
            dashCooldownTimer = dashCooldown;
            dashDurationTimer = dashDuration;
            dashDirection = inputDirectionFromCameraView;
            staminaData.Consume(dashStaminaCost);
            onDashStarted?.Invoke();
            stateMachine.SetTrigger("Dash");
        }
    }

    public List<IDamageable> attackedDamageable = new List<IDamageable>();
    private void AttackAction_performed(InputAction.CallbackContext obj)
    {
        if (attackCooldownTimer <= 0 && staminaData.Stamina > 1)
        {
            attackedDamageable = new List<IDamageable>();
            if (inputDirection == Vector2.zero)
                inputDirection = Vector2.up;

            attackCooldownTimer = attackCooldown;
            attackDurationTimer = attackDuration;
            dashDirection = inputDirectionFromCameraView;
            staminaData.Consume(attackStaminaCost);
            onAttackStarted?.Invoke();
            stateMachine.SetTrigger("Attack");
        }
    }

    public void CanDash(bool b)
    {
        canDash = b;
    }

    public float GetDashDurationTimer()
    {
        return dashDurationTimer;
    }
    
    public float GetAttackDurationTimer()
    {
        return attackDurationTimer;
    }

    void DashHandler(float delta)
    {
        if (!canDash)
            return;

        rb.MovePosition(center + (dashDirection * dashForce * delta));
        //rb.AddForce(inputDirectionFromCameraView * dashForce, ForceMode.Impulse);
    }

    const float deadzoneJump = 0.01f;
    private void JumpAction_performed(InputAction.CallbackContext obj)
    {
        if (onFloor)
            Jump();
    }

    public void Jump()
    {
        if (rb.linearVelocity.y > -deadzoneJump && rb.linearVelocity.y < deadzoneJump && staminaData.Stamina > 1)
        {
            rb.linearVelocity += (Vector3.up * jumpForce) + (inputDirectionFromCameraView * speed);
            onJumpStarted?.Invoke();
            staminaData.Consume(jumpStaminaCost);
        }
    }

    private void LockAction_performed(InputAction.CallbackContext obj)
    {
        LockOnToggle();
    }

    void LockOnToggle()
    {
        if (lockOn)
        {
            StopLockOn();
        }
        else
        {
            LockOnDetectable();
        }
    }

    void LockOnToggle(bool b)
    {
        if (b)
        {
            LockOnDetectable();
        }
        else
        {
            StopLockOn();
        }
    }

    void LockOnDetectable()
    {
        if (lockOn)
            return;

        IDetectable[] detectables = viewTriggerForLockOn.GetDetectedObjects();
        if (detectables.Length > 0)
        {
            lockOn = true;
            currentDetectable = viewTriggerForLockOn.GetDetectedObjects()[0];
            lockOnCamera.LookAt = this.transform;
            lockOnCamera.gameObject.SetActive(true);
            freeCamera.gameObject.SetActive(false);
            currentDetectable.OnDetectableChange += LockOnToggle;
        }
    }

    void StopLockOn()
    {
        if (!lockOn)
            return;

        lockOn = false;
        if (freeCamera) freeCamera.gameObject.SetActive(true);
        if (lockOnCamera) lockOnCamera.gameObject.SetActive(false);
        currentDetectable.OnDetectableChange -= LockOnToggle;
        currentDetectable = null;
    }

    //bool mouse = true;
    private void InputUser_onChange(InputUser arg1, InputUserChange arg2, InputDevice arg3)
    {
        //switch (arg2)
        //{
        //    case InputUserChange.DevicePaired:
        //        Debug.Log(arg3.displayName);
        //        Debug.Log(arg3.name);
        //        if (arg3.name == "Mouse" || arg3.name == "Keyboard")
        //        {
        //            mouse = true;
        //        }
        //        else
        //        {
        //            mouse = false;
        //        }
        //        break;
        //}
    }

    public void CanMove(bool b)
    {
        canMove = b;
    }

    void MoveHandler(float delta)
    {
        if (!canMove)
            return;
        if (!onFloor)
            return;

        Vector3 translation = inputDirectionFromCameraView * speed * delta;
        translation = new Vector3(translation.x, 0, translation.z);
        if (lockOn && currentDetectable != null)
        {
            transform.LookAt(new Vector3(currentDetectable.transform.position.x, 0, currentDetectable.transform.position.z));
        }
        else
        {
            transform.LookAt(Vector3.Lerp(center + transform.forward, center + translation, .75f));
        }
        //transform.Translate(translation, Space.World);
        rb.MovePosition(center + translation);
    }

    void CheckFloor()
    {
        Collider[] colliders = Physics.OverlapSphere(center, .2f);

        foreach (Collider collider in colliders)
        {
            if (collider.GetComponentInNearParents<PlayerMovement>() == this)
                continue;

            onFloor = true;
            return;
            
        }
        onFloor = false;
    }

    public bool AttackHandler(float delta)
    {
        if (!canAttack)
            return false;

        rb.MovePosition(center + (dashDirection * attackDashForce * delta));

        Collider[] colliders = Physics.OverlapSphere(transform.position + attackOffset, attackRange);

        foreach (Collider collider in colliders)
        {
            IDamageable damagable = collider.GetComponentInNearParents<IDamageable>();
            if (damagable == null)
                continue;
            if (damagable == damageData)
                continue;
            damagable.Damage(attackDamage);
            stateMachine.SetTrigger("Next");
            return true;
        }
        return false;
    }

    public void CanAttack(bool b)
    {
        canAttack = b;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + attackOffset, attackRange);
    }
#endif

    private void Update()
    {
        inputDirection = moveAction.ReadValue<Vector2>();
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
        if (dashDurationTimer > 0)
            dashDurationTimer -= Time.deltaTime;
        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;
        if (attackDurationTimer > 0)
            attackDurationTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        CheckFloor();
        MoveHandler(Time.fixedDeltaTime);
        DashHandler(Time.fixedDeltaTime);
        AttackHandler(Time.fixedDeltaTime);
    }

}