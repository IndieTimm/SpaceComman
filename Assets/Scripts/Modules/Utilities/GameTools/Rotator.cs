using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Space space = Space.Self;
    public float length = 1.0F;
    public Vector3 axis;

    private void Update()
    {
        transform.Rotate(axis * length * Time.deltaTime, space);
    }
}
