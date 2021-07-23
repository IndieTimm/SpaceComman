using UnityEngine;

public struct TriggerContext
{
    public Vector3 point;
    public Vector3 direction;
    public Vector3 normal;
    public GameObject gameObject;

    public TriggerContext(Vector3 point, GameObject gameObject)
    {
        this.point = point;
        this.gameObject = gameObject;

        direction = Vector3.zero;
        normal = Vector3.zero;
    }

    public TriggerContext(RaycastHit hit, Trigger trigger)
    {
        point = hit.point;
        gameObject = hit.transform.gameObject;
        direction = (hit.point - trigger.transform.position).normalized;
        normal = hit.normal;
    }
}
