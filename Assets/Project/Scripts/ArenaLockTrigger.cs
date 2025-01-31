using UnityEngine;
using Unity.Behavior;

public class ArenaLockTrigger : MonoBehaviour
{
    [SerializeField] GameObject wall;
    [SerializeField] GameObject boss;
    [SerializeField] AudioSource bossSoundtrack;
    [SerializeField] GameObject bossHP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wall.GetComponent<BoxCollider>().enabled = false;
        boss.GetComponent<BehaviorGraphAgent>().enabled = false;
        bossHP.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        wall.GetComponent<BoxCollider>().enabled = true;
        boss.GetComponent<BehaviorGraphAgent>().enabled = true;
        bossHP.SetActive(true);
        bossSoundtrack.Play();
    }
}
