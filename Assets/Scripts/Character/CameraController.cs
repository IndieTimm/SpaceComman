using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum Axis
    {
        All = 3,
        Horizontal = 2,
        Vertical = 1
    }

    public Axis activeAxis;
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    private Vector2 rotation;

    private AxisInputContext lookContext;

    private void Awake()
    {
        rotation = transform.localRotation.eulerAngles;
        lookContext = GameInputManager.Instance.GetContext<OrbitContext>();
    }

    private void Update()
    {
        rotation.y += (IsActiveAxis(Axis.Horizontal) ? lookContext.Value.x : 0) * mouseSensitivity * Time.deltaTime;
        rotation.x -= (IsActiveAxis(Axis.Vertical) ? lookContext.Value.y : 0) * mouseSensitivity * Time.deltaTime;

        rotation.x = Mathf.Clamp(rotation.x, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(rotation);
    }

    private bool IsActiveAxis(Axis axis)
    {
        return ((int)activeAxis & (int)axis) == (int)axis;
    }
}
