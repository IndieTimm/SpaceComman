using UnityEngine;
using GameUtilities.Math;

public class BubleDisplacement : MonoBehaviour
{
    public Vector3 Displacement
    {
        get => _displacement;
        set => _displacement = value;
    }

    public Vector3 LocalScale
    {
        get => _localScale;
        set => _localScale = value;
    }

    [SerializeField] private Vector3PIDController _pid = null;
    [SerializeField] private Vector3 _displacement;

    private Vector3 _localScale;

    private void Awake()
    {
        LocalScale = transform.localScale;
    }

    private void Update()
    {
        Displacement += _pid.PID(-Displacement) * Time.deltaTime;

        transform.localScale = Vector3.Lerp(transform.localScale, LocalScale + Displacement, 5F * Time.deltaTime);
    }
}
