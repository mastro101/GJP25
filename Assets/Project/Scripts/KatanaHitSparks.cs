using UnityEngine;

public class KatanaHitSparks : MonoBehaviour
{
    [SerializeField] GameObject sparksObject;
    ParticleSystem sparksParticle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sparksParticle = sparksObject.GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        sparksObject.transform.position = other.ClosestPoint(this.transform.position);
        sparksParticle.Play();
    }

}
