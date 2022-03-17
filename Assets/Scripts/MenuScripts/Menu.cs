// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/MenuScripts/Menu.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Menu : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Menu()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Menu"",
    ""maps"": [
        {
            ""name"": ""User"",
            ""id"": ""04c6bb26-d29f-4c9a-9472-833bc0f33baf"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""26dadaa3-79e1-463d-a4b5-889592547d7e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Close menu"",
                    ""type"": ""Button"",
                    ""id"": ""c92f2957-e867-4551-9ffc-377f6ff3a7d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""88dbef95-2f58-4531-a371-0733276448b4"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab3b3d24-a02f-4856-9a2e-53d4058ee69d"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5fd25ec5-f682-41b1-8255-f39910df855e"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Close menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ac5bad3-886d-4062-98c4-3afcaeb13b04"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Close menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // User
        m_User = asset.FindActionMap("User", throwIfNotFound: true);
        m_User_Select = m_User.FindAction("Select", throwIfNotFound: true);
        m_User_Closemenu = m_User.FindAction("Close menu", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // User
    private readonly InputActionMap m_User;
    private IUserActions m_UserActionsCallbackInterface;
    private readonly InputAction m_User_Select;
    private readonly InputAction m_User_Closemenu;
    public struct UserActions
    {
        private @Menu m_Wrapper;
        public UserActions(@Menu wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_User_Select;
        public InputAction @Closemenu => m_Wrapper.m_User_Closemenu;
        public InputActionMap Get() { return m_Wrapper.m_User; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UserActions set) { return set.Get(); }
        public void SetCallbacks(IUserActions instance)
        {
            if (m_Wrapper.m_UserActionsCallbackInterface != null)
            {
                @Select.started -= m_Wrapper.m_UserActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_UserActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_UserActionsCallbackInterface.OnSelect;
                @Closemenu.started -= m_Wrapper.m_UserActionsCallbackInterface.OnClosemenu;
                @Closemenu.performed -= m_Wrapper.m_UserActionsCallbackInterface.OnClosemenu;
                @Closemenu.canceled -= m_Wrapper.m_UserActionsCallbackInterface.OnClosemenu;
            }
            m_Wrapper.m_UserActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Closemenu.started += instance.OnClosemenu;
                @Closemenu.performed += instance.OnClosemenu;
                @Closemenu.canceled += instance.OnClosemenu;
            }
        }
    }
    public UserActions @User => new UserActions(this);
    public interface IUserActions
    {
        void OnSelect(InputAction.CallbackContext context);
        void OnClosemenu(InputAction.CallbackContext context);
    }
}
