using UnityEngine;

public class LevitationController : MonoBehaviour
{
    [SerializeField] private Vector2 levitationFrequency = new Vector2(0.5F, 1F);
    [SerializeField] private Vector2 levitationScale = new Vector2(0.02F, 0.05F);

    private Vector3 defaultLocalPosition;

    private void Awake()
    {
        defaultLocalPosition = transform.localPosition;
    }

    private void FixedUpdate()
    {
        Vector2 levitation = new Vector2(
            Mathf.Sin(levitationFrequency.x * Time.time),
            Mathf.Cos(levitationFrequency.y * Time.time));

        Vector3 levitationOffset = Vector2.Scale(levitationScale, levitation);

        transform.localPosition = defaultLocalPosition + levitationOffset;
    }
}
