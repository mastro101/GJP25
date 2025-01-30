using UnityEngine;

public class PlayerAnimEventFXHandler : MonoBehaviour
{
    [SerializeField] AudioSource playerSource;

    [SerializeField] AudioClip playerStepSound;

    public void PlayPlayerStepSound()
    {
        playerSource.PlayOneShot(playerStepSound);
    }
}
