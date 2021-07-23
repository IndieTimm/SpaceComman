using System;
using UnityEngine;

public class BoxTrigger : Trigger
{
    public Vector3 halfBoxScale = Vector3.one;
    private TriggerContext context = new TriggerContext();

    public override bool Test(out TriggerContext context, Func<TriggerContext, bool> match = null)
    {
        context = this.context;

        if (Physics.BoxCast(transform.position, halfBoxScale, transform.forward, out RaycastHit hit))
        {
            context.direction = transform.forward;
            context.normal = hit.normal;
            context.point = hit.point;
            context.gameObject = hit.collider.gameObject;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.LookRotation(transform.forward), halfBoxScale * 2);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}