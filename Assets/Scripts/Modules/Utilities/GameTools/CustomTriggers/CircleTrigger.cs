using System;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    public abstract bool Test(out TriggerContext context, Func<TriggerContext, bool> match = null);
}

public class CircleTrigger : Trigger
{
    public float length = 1.0F;
    public float angleBetweenRays = 1.0F;
    public Vector3 normal = Vector3.up;

    public override bool Test(out TriggerContext context, Func<TriggerContext, bool> match = null)
    {
        var axis = transform.localToWorldMatrix.MultiplyVector(normal);
        var angleBetweenRays = Mathf.Clamp(this.angleBetweenRays, 1F, 360F);

        for (float angle = 0; angle < 360F; angle += angleBetweenRays)
        {
            float alpha = angle * Mathf.Deg2Rad;

            Vector3 direction =
                Mathf.Cos(alpha) * transform.up +
                Mathf.Sin(alpha) * transform.right;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, length) && !hit.collider.isTrigger && !hit.collider.gameObject.isStatic)
            {
                context = new TriggerContext(hit, this);

                if(match != null && !match.Invoke(context))
                {
                    continue;
                }

                return true;
            }
        }

        context = new TriggerContext();

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 axis = transform.localToWorldMatrix.MultiplyVector(normal);

        Gizmos.DrawLine(transform.position, transform.position + axis);

        float angleBetweenRays = Mathf.Clamp(this.angleBetweenRays, 1, 360);

        for (float angle = 0; angle < 360F; angle += angleBetweenRays)
        {
            float alpha = angle * Mathf.Deg2Rad;

            Vector3 direction =
                Mathf.Cos(alpha) * transform.up +
                Mathf.Sin(alpha) * transform.right;

            Vector3 point = transform.position + direction * length;

            Gizmos.color = new Color(alpha / Mathf.PI, 1F - alpha / Mathf.PI, 0);

            Gizmos.DrawLine(transform.position, point);
        }
    }
}