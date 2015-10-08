using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelsSideScroll : MonoBehaviour {

	public RectTransform container;
	public Button rightButton;
	public Button leftButton;
	public float sideJumpDist;
	public float speed;

//	float offset;

	// Use this for initialization
	void Start () {
		rightButton.onClick.AddListener (OnRightButtonClicked);
		leftButton.onClick.AddListener (OnLeftButtonClicked);
//		offset = sideJumpDist;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnRightButtonClicked () {
		container.anchoredPosition = new Vector2 (container.anchoredPosition.x - sideJumpDist, container.anchoredPosition.y);
	}
	public void OnLeftButtonClicked () {
		container.anchoredPosition = new Vector2 (container.anchoredPosition.x + sideJumpDist, container.anchoredPosition.y);
	}
}
