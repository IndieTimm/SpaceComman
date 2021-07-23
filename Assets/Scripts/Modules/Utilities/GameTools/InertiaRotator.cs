using UnityEngine;

public class InertiaRotator : MonoBehaviour
{
    public Space RotationSpace = Space.Self;

    [Space]
    public Vector3 RotationAxis = Vector3.forward;
    public float MinimalAngle = -1.0F;
    public float MaximalAngle = 1.0F;

    [Space]
    public float minimalInertia = -1.0F;
    public float maximalInertia = 1.0F;
    public float inertiaScale = 1.0F;

    private Vector3 inertia;
    private Vector3 position;
    private Vector3 defaultRotation;

    private void Awake()
    {
        defaultRotation = (RotationSpace == Space.Self)
            ? transform.localEulerAngles
            : transform.eulerAngles;
    }

    private void Update()
    {
        Vector3 delta = transform.position - position;

        inertia += delta * inertiaScale * Time.deltaTime;
        inertia.x = Mathf.Clamp(inertia.x, minimalInertia, maximalInertia);
        inertia.y = Mathf.Clamp(inertia.y, minimalInertia, maximalInertia);
        inertia.z = Mathf.Clamp(inertia.z, minimalInertia, maximalInertia);

        Vector3 angle = inertia.y * RotationAxis;

        angle.x = Mathf.Clamp(angle.x, MinimalAngle, MaximalAngle);
        angle.y = Mathf.Clamp(angle.y, MinimalAngle, MaximalAngle);
        angle.z = Mathf.Clamp(angle.z, MinimalAngle, MaximalAngle);

        if (RotationSpace == Space.Self)
        {
            transform.localEulerAngles = defaultRotation + angle;
        }
        else
        {
            transform.eulerAngles = defaultRotation + angle;
        }

        position = transform.position;
    }
}
