// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/MenuScripts/PauseMenu.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PauseMenu : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PauseMenu()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PauseMenu"",
    ""maps"": [
        {
            ""name"": ""isPause"",
            ""id"": ""ea538daf-bf03-4c90-9e7b-887aca0875e0"",
            ""actions"": [
                {
                    ""name"": ""inPauseMenu"",
                    ""type"": ""Button"",
                    ""id"": ""3040abdb-f969-41fc-ba6f-1903fd85a009"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0308f1d5-5825-450a-b71c-7c7f36c312dc"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybaord Scheme"",
                    ""action"": ""inPauseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UIActive"",
            ""id"": ""6b0c6439-040b-47b5-a76e-ed954ea9d8ff"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""1cf34c39-40e3-4690-ad1b-11689d06925c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""93b845da-563e-4f1b-a70b-48e2069adbad"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keybaord Scheme"",
            ""bindingGroup"": ""Keybaord Scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // isPause
        m_isPause = asset.FindActionMap("isPause", throwIfNotFound: true);
        m_isPause_inPauseMenu = m_isPause.FindAction("inPauseMenu", throwIfNotFound: true);
        // UIActive
        m_UIActive = asset.FindActionMap("UIActive", throwIfNotFound: true);
        m_UIActive_Newaction = m_UIActive.FindAction("New action", throwIfNotFound: true);
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

    // isPause
    private readonly InputActionMap m_isPause;
    private IIsPauseActions m_IsPauseActionsCallbackInterface;
    private readonly InputAction m_isPause_inPauseMenu;
    public struct IsPauseActions
    {
        private @PauseMenu m_Wrapper;
        public IsPauseActions(@PauseMenu wrapper) { m_Wrapper = wrapper; }
        public InputAction @inPauseMenu => m_Wrapper.m_isPause_inPauseMenu;
        public InputActionMap Get() { return m_Wrapper.m_isPause; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(IsPauseActions set) { return set.Get(); }
        public void SetCallbacks(IIsPauseActions instance)
        {
            if (m_Wrapper.m_IsPauseActionsCallbackInterface != null)
            {
                @inPauseMenu.started -= m_Wrapper.m_IsPauseActionsCallbackInterface.OnInPauseMenu;
                @inPauseMenu.performed -= m_Wrapper.m_IsPauseActionsCallbackInterface.OnInPauseMenu;
                @inPauseMenu.canceled -= m_Wrapper.m_IsPauseActionsCallbackInterface.OnInPauseMenu;
            }
            m_Wrapper.m_IsPauseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @inPauseMenu.started += instance.OnInPauseMenu;
                @inPauseMenu.performed += instance.OnInPauseMenu;
                @inPauseMenu.canceled += instance.OnInPauseMenu;
            }
        }
    }
    public IsPauseActions @isPause => new IsPauseActions(this);

    // UIActive
    private readonly InputActionMap m_UIActive;
    private IUIActiveActions m_UIActiveActionsCallbackInterface;
    private readonly InputAction m_UIActive_Newaction;
    public struct UIActiveActions
    {
        private @PauseMenu m_Wrapper;
        public UIActiveActions(@PauseMenu wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_UIActive_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_UIActive; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActiveActions set) { return set.Get(); }
        public void SetCallbacks(IUIActiveActions instance)
        {
            if (m_Wrapper.m_UIActiveActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_UIActiveActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_UIActiveActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_UIActiveActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_UIActiveActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public UIActiveActions @UIActive => new UIActiveActions(this);
    private int m_KeybaordSchemeSchemeIndex = -1;
    public InputControlScheme KeybaordSchemeScheme
    {
        get
        {
            if (m_KeybaordSchemeSchemeIndex == -1) m_KeybaordSchemeSchemeIndex = asset.FindControlSchemeIndex("Keybaord Scheme");
            return asset.controlSchemes[m_KeybaordSchemeSchemeIndex];
        }
    }
    public interface IIsPauseActions
    {
        void OnInPauseMenu(InputAction.CallbackContext context);
    }
    public interface IUIActiveActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
