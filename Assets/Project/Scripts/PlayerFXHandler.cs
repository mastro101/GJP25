using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFXHandler : MonoBehaviour
{
    PlayerMovement playerMovement;

    [Header("VFX")]
    [SerializeField] ParticleSystem DashTrails;
    [SerializeField] ParticleSystem DashSmoke;

    [Header("SFX")]
    [SerializeField] AudioClip jumpStart;
    [SerializeField] AudioClip dashStart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.onDashStarted += PlayDashFX;
        playerMovement.onJumpStarted += PlayJumpStartedFX;
    }

    void PlayDashFX()
    {
        DashTrails.Play();
        DashSmoke.Play();
        playerMovement.GetComponent<AudioSource>().PlayOneShot(dashStart);
    }

    void PlayJumpStartedFX()
    {
        DashSmoke.Play();
        playerMovement.GetComponent<AudioSource>().PlayOneShot(jumpStart);
    }

    void OnDisable()
    {
        playerMovement.onDashStarted -= PlayDashFX;
        playerMovement.onJumpStarted -= PlayJumpStartedFX;
    }

    public void PlayPlayerStepSound()
    {

    }
}
