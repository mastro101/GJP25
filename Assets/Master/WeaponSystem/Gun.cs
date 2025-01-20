using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] Bullet bullet;
        [Space]
        [SerializeField] GunTypology gunTypology;
        [Space]
        [SerializeField] float fireRatio = .5f;
        [SerializeField] float semiAutomaticRatio = .5f;

        PoolManager<Bullet> bullets;

        float fireTimer = 0;
        float semiAutomaticTimer = 0;

        private void Start()
        {
            bullets = new PoolManager<Bullet>(bullet, 10);
        }

        private void Update()
        {
            switch (gunTypology)
            {
                case GunTypology.Manual:
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Shoot();
                    }
                    break;
                case GunTypology.Automatic:
                    if (Input.GetKey(KeyCode.Space))
                    {
                        Shoot();
                    }
                    break;
                case GunTypology.SemiAutomatic:
                    if (semiAutomaticTimer < semiAutomaticRatio)
                    {
                        if (Input.GetKey(KeyCode.Space))
                        {
                            semiAutomaticTimer += Time.deltaTime;
                            Shoot();
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        semiAutomaticTimer = 0;
                    }
                    break;
                default:
                    break;
            }

            if (fireTimer > 0)
                fireTimer -= Time.deltaTime;
        }

        void Shoot()
        {
            if (fireTimer <= 0)
            {
                fireTimer = fireRatio;
                bullets.Get(transform.position, transform.forward);
            }
        }
    }

    public enum GunTypology
    {
        Manual,
        Automatic,
        SemiAutomatic
    }
}