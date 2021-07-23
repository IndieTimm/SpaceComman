using System;
using UnityEngine;

public class LineTrigger : Trigger
{
    public int raysNumber = 1;
    public float lineLength = 1;
    public float rayLength = 1.0F;

    public override bool Test(out TriggerContext context, Func<TriggerContext, bool> match = null)
    {
        for (int i = 0; i < raysNumber; i++)
        {
            var origin = transform.position;

            if (raysNumber > 1)
            {
                origin += transform.right * lineLength * (i / (raysNumber - 1.0F) - 0.5F);
            }

            var direction = transform.forward;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, rayLength))
            {
                context = new TriggerContext(hit, this);

                if (match != null && !match.Invoke(context))
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
        for (int i = 0; i < raysNumber; i++)
        {
            var origin = transform.position;

            if (raysNumber > 1)
            {
                origin += transform.right * lineLength * (i / (raysNumber - 1.0F) - 0.5F);
            }

            var direction = transform.forward;

            Gizmos.DrawLine(origin, origin + direction * rayLength);
        }
    }
}
