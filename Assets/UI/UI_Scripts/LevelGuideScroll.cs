using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelGuideScroll : MonoBehaviour {

	public AtomTouchGUI controller;
	public RectTransform panelGuide;

	public float desiredBottom;
	public float desiredTop;
	public float kern;

	bool movingDown;
	bool movingUp;
	bool moveWasCalled;

	float currentY;
	float previousY;
	float startY;
	

	void Start () {
		desiredTop = panelGuide.anchoredPosition.y;
		SetUpPanel ();
		movingUp = false;
		movingDown = false;
		moveWasCalled = false;
		currentY = panelGuide.anchoredPosition.y;
		startY = currentY;
		previousY = currentY;
	}

	void Update () {
		previousY = currentY;
		currentY = panelGuide.anchoredPosition.y;
		if (currentY - previousY < 0)
			MovingDown ();
		else if (currentY - previousY > 0)
			MovingUp ();
		if (currentY <= desiredBottom + 1 || currentY >= desiredTop - 1 && moveWasCalled) {
			moveWasCalled = false;
			controller.changingTemp = false;
		}
	}

	void MovingDown() {
		/*if (!movingDown)
			startY = currentY;
		if (currentY > desiredBottom && startY - currentY > kern)
			panelGuide.anchoredPosition = new Vector2(panelGuide.localPosition.x, desiredBottom);

		movingDown = true;
		movingUp = false;*/
		moveWasCalled = true;
		Debug.Log ("moving down");
		controller.changingTemp = true; // hack way to get the screen not to move the same way when using temp slider.
	}

	void MovingUp() {
		/*if (!movingUp)
			startY = currentY;
		if (currentY - startY > kern && currentY < desiredTop)
			panelGuide.anchoredPosition = new Vector2(panelGuide.localPosition.x, desiredTop);
		movingDown = false;
		movingUp = true;*/
		moveWasCalled = true;
		controller.changingTemp = true; // hack way to get the screen not to move the same way when using temp slider.
		Debug.Log ("moving up");
	}

	public void SetUpPanel() {
		panelGuide.anchoredPosition = new Vector2(panelGuide.anchoredPosition.x, desiredTop);
	}
	
}
