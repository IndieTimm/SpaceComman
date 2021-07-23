using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputDeviceType
{
    Gamepad,
    MouseOrKeyboard
}

public class GameInputManager : StaticBehaviour<GameInputManager>
{
    public InputDeviceType CurrentDevice
    {
        get;
        private set;
    }

    [SerializeField]
    private InputActionAsset inputActions = null;

    private Dictionary<Type, InputValueContextBase> actions;

    public T GetContext<T>() where T : InputValueContextBase
    {
        return (T)actions[typeof(T)];
    }

    public override void Initialization()
    {
        actions = new Dictionary<Type, InputValueContextBase>();

        AddNewAction(new OrbitContext(), "Orbit");
        AddNewAction(new MoveContext(), "Move");
        AddNewAction(new JumpContext(), "Jump");
        AddNewAction(new SprintContext(), "Sprint");
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void AddNewAction(InputValueContextBase actionMarker, params string[] actionNames)
    {
        actions.Add(actionMarker.GetType(), actionMarker);

        foreach (string actionName in actionNames)
        {
            InputAction action = inputActions.FindAction(actionName, true);

            action.actionMap.actionTriggered += (context) =>
            {
                UpdateDeviceType(context.control.device);

                if (context.action == action)
                {
                    actionMarker.SetValue(context);
                }
            };

            if (!action.enabled)
            {
                action.Enable();
            }
        }
    }

    private void UpdateDeviceType(InputDevice device)
    {
        if (device is Gamepad)
        {
            CurrentDevice = InputDeviceType.Gamepad;
        }
        else if (device is Keyboard || device.device is Mouse)
        {
            CurrentDevice = InputDeviceType.MouseOrKeyboard;
        }
    }
}