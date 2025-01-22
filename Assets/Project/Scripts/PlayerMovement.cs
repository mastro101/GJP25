using Eastermaster;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.Jobs;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float cameraSpeed;
    [SerializeField] ViewTrigger viewTriggerForLockOn;

    Camera cam;
    Transform cameraPivot;
    InputAction moveAction;
    InputAction lookAction;
    InputAction lockAction;
    Vector2 inputDirection;
    Vector2 lookDirection;
    bool lockOn;
    IDetectable lockOnDetectable;

    float cameraSpeedMouse => cameraSpeed / 2;
    Vector3 center => transform.position;

    private void Awake()
    {
        cam = Camera.main;
        cameraPivot = cam.transform.parent;
        moveAction = InputSystem.actions.FindAction("Player/Move");
        lookAction = InputSystem.actions.FindAction("Player/Look");
        lockAction = InputSystem.actions.FindAction("Player/Lock");
        lockAction.performed += LockAction_performed;
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
    }

    private void LockAction_performed(InputAction.CallbackContext obj)
    {
        if (lockOn == true)
        {
            lockOn = false;
            lockOnDetectable = null;
        }
        else
        {
            lockOn = true;
            IDetectable[] detectables = viewTriggerForLockOn.GetDetectedObjects();
            if (detectables.Length > 0)
                lockOnDetectable = viewTriggerForLockOn.GetDetectedObjects()[0];
        }

    }

    bool mouse = true;
    private void InputUser_onChange(InputUser arg1, InputUserChange arg2, InputDevice arg3)
    {
        switch (arg2)
        {
            case InputUserChange.DevicePaired:
                Debug.Log(arg3.displayName);
                Debug.Log(arg3.name);
                if (arg3.name == "Mouse" || arg3.name == "Keyboard")
                {
                    mouse = true;
                }
                else
                {
                    mouse = false;
                }
                break;
        }
    }

    private void Update()
    {
        inputDirection = moveAction.ReadValue<Vector2>();

        if (lockOn && lockOnDetectable != null)
        {
            lookDirection = lockOnDetectable.transform.position;
            cameraPivot.LookAt(Vector3.Lerp(cameraPivot.transform.position + cameraPivot.transform.forward, lockOnDetectable.transform.position, .50f));
        }
        else
        {
            lookDirection = lookAction.ReadValue<Vector2>();
            cameraPivot.Rotate(new Vector3(0, lookDirection.x, 0) * (mouse ? cameraSpeedMouse : cameraSpeed) * Time.deltaTime, Space.Self);

        }

    }


    private void FixedUpdate()
    {
        Vector3 translation = cam.transform.rotation * (new Vector3(inputDirection.x, 0, inputDirection.y) * speed * Time.fixedDeltaTime);
        transform.Translate(translation, Space.World);
        if (lockOn && lockOnDetectable != null)
            transform.LookAt(lockOnDetectable.transform.position);
        else
            transform.LookAt(Vector3.Lerp(center + transform.forward, center + translation, .75f));
    }
}
