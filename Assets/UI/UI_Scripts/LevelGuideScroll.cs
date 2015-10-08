using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelGuideScroll : MonoBehaviour {

	public AtomTouchGUI controller;
	public RectTransform panelGuide;

	public float desiredBottom;
	public float desiredTop;
	bool up;
	bool isUp;



	void Start () {

		up = true;
		desiredTop = panelGuide.anchoredPosition.y;
		SetUpPanel ();

		isUp = true;
		desiredTop = panelGuide.anchoredPosition.y;
		SetUpPanel ();
	}

	public void SetUpPanel() {
		panelGuide.anchoredPosition = new Vector2(panelGuide.anchoredPosition.x, desiredTop);
	}

	public void OnClicked() {
		if (up) {
			panelGuide.anchoredPosition = new Vector2 (panelGuide.anchoredPosition.x, desiredBottom);
			up = false;
		} else {
			panelGuide.anchoredPosition = new Vector2 (panelGuide.anchoredPosition.x, desiredTop);
			up = true;
		}
	}

	
}
