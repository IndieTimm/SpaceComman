using UnityEngine;

public class GradientSetter : MonoBehaviour
{
    public enum Mode
    {
        Normal,
        ParticleSystemTrail
    }

    public Mode mode = Mode.Normal;
    public Gradient gradient;
    public string propertyName = "_Gradient";
    public int resolution = 64;

    [ContextMenu("Set gradient")]
    private void Awake()
    {
        switch (mode)
        {
            case Mode.Normal: GetComponent<Renderer>().sharedMaterial.SetTexture(propertyName, GetColorRamp()); break;
            case Mode.ParticleSystemTrail: GetComponent<ParticleSystemRenderer>().trailMaterial.SetTexture(propertyName, GetColorRamp()); break;
        }
    }

    private Texture2D GetColorRamp()
    {
        Texture2D texture = new Texture2D(resolution, 1);

        for (int i = 0; i < resolution; i++)
        {
            float time = i / (resolution - 1.0F);

            texture.SetPixel(i, 0, gradient.Evaluate(time));
        }

        texture.Apply();

        return texture;
    }
}