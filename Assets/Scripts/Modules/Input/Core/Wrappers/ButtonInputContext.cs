using System;

public class ButtonInputContext : InputValueContext<float>
{
    //TODO: Add threshould constant.
    public bool IsHold => Value >= 0.5F;
    public bool IsRelease => Value < 0.5F;

    public void RegisterButtonPressCallback(Action callback, GameSystemType type)
    {
        RegisterValueChangingCallback(() =>
        {
            if (IsHold)
            {
                callback?.Invoke();
            }
        },
        type);
    }
}
