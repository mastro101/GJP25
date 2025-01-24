using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVFXHandler : MonoBehaviour
{
    InputAction dash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        dash = InputSystem.actions.FindAction("Player/Dash");
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
