using UnityEngine;

public class PlayerAnimEventFXHandler : MonoBehaviour
{
    [SerializeField] AudioSource playerSource;

    [SerializeField] AudioClip playerStepSound;
    [SerializeField] AudioClip playerGotHitSound;

    public void PlayPlayerStepSound()
    {
        playerSource.PlayOneShot(playerStepSound);
    }

    public void PlayPlayerGotHitSound()
    {
        playerSource.PlayOneShot(playerGotHitSound);
    }
}
