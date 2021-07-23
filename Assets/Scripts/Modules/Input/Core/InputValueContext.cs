using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public abstract class InputValueContextBase
{
    public abstract void SetValue(InputAction.CallbackContext context);
    public abstract void RegisterValueChangingCallback(Action callback, GameSystemType systemType);
}

public abstract class InputValueContext<T> : InputValueContextBase where T : struct
{
    public T Value { get; private set; }

    private Dictionary<GameSystemType, List<Action>> callbacks = new Dictionary<GameSystemType, List<Action>>();

    public override void SetValue(InputAction.CallbackContext context)
    {
        // Source: Input player component
        if (!(context.performed || (context.canceled)))
            return;

        T tempValue = ValueProcessor(context.ReadValue<T>(), context);

        bool valueChanged = !Value.Equals(tempValue);

        Value = tempValue;

        if (valueChanged)
        {
            foreach (var callbackItem in callbacks)
            {
                if (GameInputFocus.IsActive(callbackItem.Key))
                {
                    callbackItem.Value.ForEach(x => x?.Invoke());
                }
            }
        }
    }

    public override void RegisterValueChangingCallback(Action callback, GameSystemType systemType)
    {
        if (!callbacks.ContainsKey(systemType))
        {
            callbacks.Add(systemType, new List<Action>());
        }

        callbacks[systemType].Add(callback);
    }

    public virtual T ValueProcessor(T value, InputAction.CallbackContext context)
    {
        return value;
    }
}