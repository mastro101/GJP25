using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Eastermaster
{
    public class ViewTrigger : MonoBehaviour
    {
        [SerializeField][HideInInspector] float radius = 1f;
        [SerializeField][HideInInspector] float viewAngle = 45f;

        Vector3 differenceVector = Vector2.zero;
        float dist = 0;
        float angleViewToObj;

        public IDetectable[] GetDetectedObjects()
        {
            List<IDetectable> res = new List<IDetectable>();
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            foreach (var hitCollider in hitColliders)
            {
                IDetectable detectable = hitCollider.GetComponent<IDetectable>();

                if (detectable == null) continue;

                if (CheckAngle(hitCollider.transform) && CheckNoObstacle(hitCollider.transform))
                    res.Add(detectable);
            }
            return res.ToArray();
        }

        public bool CheckDistance(Transform t)
        {
            differenceVector = t.position - transform.position;
            dist = differenceVector.magnitude;
            return dist <= radius;
        }

        public bool CheckAngle(Transform t)
        {
            differenceVector = t.position - transform.position;
            angleViewToObj = Vector3.Angle(transform.forward, differenceVector);

            return angleViewToObj <= viewAngle / 2f;
        }

        public bool CheckNoObstacle(Transform t)
        {
            RaycastHit hit;
            Vector3 tLocalPos = t.position - transform.position;
            if (Physics.Raycast(transform.position, tLocalPos, out hit, tLocalPos.magnitude))
            {
                if (hit.transform == t || hit.transform.IsChildOf(t))
                    return true;
                else
                    return false;
            }

            return true;
        }
    }
}