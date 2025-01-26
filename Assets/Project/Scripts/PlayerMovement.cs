using Cinemachine;
using Eastermaster;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [SerializeInterface(typeof(IStaminable))] GameObject playerData;
    [SerializeField] float speed = 10;
    [SerializeField] float jumpForce = 7.5f;
    [SerializeField] float jumpStaminaCost = 2f;
    [SerializeField] float dashForce = 500;
    [SerializeField] float dashStaminaCost = 2f;
    [SerializeField] float dashCooldown = 2;
    [SerializeField] CinemachineFreeLook freeCamera;
    [SerializeField] CinemachineVirtualCamera lockOnCamera;
    //[SerializeField] float cameraSpeed;
    public System.Action onDashStarted;
    public System.Action onJumpStarted;

    Rigidbody rb;
    IStaminable staminaData;
    Camera cam;
    ViewTrigger viewTriggerForLockOn;
    InputAction moveAction;
    InputAction lookAction;
    InputAction lockAction;
    InputAction jumpAction;
    InputAction dashAction;
    float dashTimer;
    Vector2 inputDirection;
    Vector2 lookDirection;
    bool lockOn;
    IDetectable lockOnDetectable;

    //float cameraSpeedMouse => cameraSpeed / 2;
    Vector3 center => transform.position;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        staminaData = playerData.GetComponent<IStaminable>();
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
        dashTimer = 0;
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
    }

    private void DashAction_performed(InputAction.CallbackContext obj)
    {
        if (dashTimer <= 0 && inputDirection != Vector2.zero && staminaData.Stamina > 1)
        {
            rb.AddForce(cam.transform.rotation * (new Vector3(inputDirection.x, 0, inputDirection.y).normalized * dashForce), ForceMode.Impulse);
            onDashStarted?.Invoke();
            dashTimer = dashCooldown;
            staminaData.Consume(dashStaminaCost);
        }

    }

    const float deadzoneJump = 0.01f;
    private void JumpAction_performed(InputAction.CallbackContext obj)
    {
        if (rb.linearVelocity.y > -deadzoneJump && rb.linearVelocity.y < deadzoneJump && staminaData.Stamina > 1)
        {
            rb.linearVelocity += Vector3.up * jumpForce;
            onJumpStarted?.Invoke();
            staminaData.Consume(jumpStaminaCost);
        }
    }

    private void LockAction_performed(InputAction.CallbackContext obj)
    {
        if (lockOn == true)
        {
            lockOn = false;
            lockOnDetectable = null;
            freeCamera.gameObject.SetActive(true);
            lockOnCamera.gameObject.SetActive(false);
        }
        else
        {
            IDetectable[] detectables = viewTriggerForLockOn.GetDetectedObjects();
            if (detectables.Length > 0)
            {
                lockOn = true;
                lockOnDetectable = viewTriggerForLockOn.GetDetectedObjects()[0];
                lockOnCamera.LookAt = viewTriggerForLockOn.GetDetectedObjects()[0].transform;
                lockOnCamera.gameObject.SetActive(true);
                freeCamera.gameObject.SetActive(false);
            }
                
        }

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

    private void Update()
    {
        inputDirection = moveAction.ReadValue<Vector2>();
        if (dashTimer > 0)
            dashTimer -= Time.deltaTime;
        //if (lockOn && lockOnDetectable != null)
        //{
        //    lookDirection = lockOnDetectable.transform.position;
        //    //Vector3.Lerp(cameraPivot.transform.position + cameraPivot.transform.forward, lockOnDetectable.transform.position, .2f)
        //    //cameraPivot.LookAt(Vector3.Lerp((cameraPivot.transform.position + cameraPivot.transform.forward) * lookDirection.magnitude, new Vector3(lookDirection.x, 0, lookDirection.y), 0.2f ) * -1);
        //}
        //else
        //{
        //    lookDirection = lookAction.ReadValue<Vector2>();
        //    //cameraPivot.Rotate(new Vector3(0, lookDirection.x, 0) * (mouse ? cameraSpeedMouse : cameraSpeed) * Time.deltaTime, Space.Self);

        //}

    }


    private void FixedUpdate()
    {
        Vector3 translation = cam.transform.rotation * (new Vector3(inputDirection.x, 0, inputDirection.y) * speed * Time.fixedDeltaTime);
        translation = new Vector3(translation.x, 0, translation.z);
        if (lockOn && lockOnDetectable != null)
        {
            transform.LookAt(new Vector3(lockOnDetectable.transform.position.x, 0, lockOnDetectable.transform.position.z));
        }
        else
        {
            transform.LookAt(Vector3.Lerp(center + transform.forward, center + translation, .75f));
        }
        //transform.Translate(translation, Space.World);
        rb.MovePosition(center + translation);
    }
}