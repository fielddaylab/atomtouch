using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelProvider : MonoBehaviour
{

	//prefabs for panel provider to make static
	public GameObject mainMenuPrefab;

	//panels for other scripts to access
	public static GameObject mainMenu;

	public void Awake ()
	{
		mainMenu = mainMenuPrefab;
	}
	
}
