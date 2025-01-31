using UnityEngine;

public class KatanaSFXHandler : MonoBehaviour
{
    AudioSource katanaSource;

    [SerializeField] AudioClip katanaStep;
    [SerializeField] AudioClip katanaSlash;
    [SerializeField] AudioClip katanaGotHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        katanaSource = this.GetComponent<AudioSource>();
    }

    public void PlayStepSound()
    {
        katanaSource.PlayOneShot(katanaStep);
    }

    public void PlaySlashSound()
    {
        katanaSource.PlayOneShot(katanaSlash);
    }

    public void PlayGotHitSound()
    {
        katanaSource.PlayOneShot(katanaGotHit);
    }
}
