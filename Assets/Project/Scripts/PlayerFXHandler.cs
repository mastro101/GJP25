using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFXHandler : MonoBehaviour
{
    PlayerMovement playerMovement;

    [Header("VFX")]
    [SerializeField] ParticleSystem DashTrails;
    [SerializeField] ParticleSystem DashSmoke;
    [SerializeField] ParticleSystem AttackSmoke;
    [SerializeField] ParticleSystem AttackSparkles;

    [Header("SFX")]
    [SerializeField] AudioClip jumpStart;
    [SerializeField] AudioClip dashStart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.onDashStarted += PlayDashFX;
        playerMovement.onJumpStarted += PlayJumpStartedFX;
        playerMovement.onAttackStarted += PlayAttackStartedFX;
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

    void PlayAttackStartedFX()
    {
        AttackSmoke.Play();
        AttackSparkles.Play();
        playerMovement.GetComponent<AudioSource>().PlayOneShot(dashStart);
    }

    void OnDisable()
    {
        playerMovement.onDashStarted -= PlayDashFX;
        playerMovement.onJumpStarted -= PlayJumpStartedFX;
    }
}
