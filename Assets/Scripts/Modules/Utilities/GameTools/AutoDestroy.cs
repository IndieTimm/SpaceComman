using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float timer = 0.5F;

    private void Start()
    {
        Destroy(gameObject, timer);
    }
}