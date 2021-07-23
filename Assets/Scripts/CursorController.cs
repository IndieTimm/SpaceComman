using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D texture;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        //Cursor.SetCursor(texture, Vector2.zero, CursorMode.ForceSoftware);
        //Cursor.visible = false;
    }
}
