using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVFXHandler : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] ParticleSystem DashTrails;
    [SerializeField] ParticleSystem DashSmoke;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.onDashStarted += PlayDashVFX;
        playerMovement.onJumpStarted += PlayJumpStartedVFX;
    }

    void PlayDashVFX()
    {
        DashTrails.Play();
        DashSmoke.Play();
    }

    void PlayJumpStartedVFX()
    {
        DashSmoke.Play();
    }

    void OnDisable()
    {
        playerMovement.onDashStarted -= PlayDashVFX;
        playerMovement.onJumpStarted -= PlayJumpStartedVFX;
    }
}
