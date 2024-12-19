using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// Note: This class is designed to be used with the `vault-floating-dropdown` jslib pluggin. 
/// This script will remove the dropdown elment from the DOM after the target scene has been unloaded.
/// </summary>
public class VaultDropdownToggle : MonoBehaviour {
    [SerializeField]
    private MainMenuPanel menu;

    [DllImport("__Internal")]
    private static extern void DisableVaultButton();

    private void OnEnable() {
        menu.levelsButton.onClick.AddListener(OnMenuExit);
		menu.creditsButton.onClick.AddListener(OnMenuExit);
		menu.freePlayButton.onClick.AddListener(OnMenuExit);
    }

    private void OnMenuExit() {
#if UNITY_WEBGL && !UNITY_EDITOR
        DisableVaultButton();
#endif
    }
}