using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVFXHandler : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] ParticleSystem DashVFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.onDashStarted += PlayDashVFX;
    }

    void PlayDashVFX()
    {
        DashVFX.
    }

    void OnDisable()
    {
        playerMovement.onDashStarted -= PlayDashVFX;
    }
}
