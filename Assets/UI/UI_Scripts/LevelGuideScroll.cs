using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelGuideScroll : MonoBehaviour {

	public AtomTouchGUI controller;
	public RectTransform panelGuide;

	public float desiredBottom;
	public float desiredTop;
//	public float kern;

//	bool movingDown;
//	bool movingUp;
//	bool moveWasCalled;
<<<<<<< HEAD
	bool up;
=======
	bool isUp;
>>>>>>> 728176898934d524cd65009808470807ec532eb4

//	float currentY;
//	float previousY;
//	float startY;
	


	void Start () {
<<<<<<< HEAD
		up = true;
		desiredTop = panelGuide.anchoredPosition.y;
		SetUpPanel ();
//		movingUp = false;
	//	movingDown = false;
=======
		isUp = true;
		desiredTop = panelGuide.anchoredPosition.y;
		SetUpPanel ();
//		movingUp = false;
//		movingDown = false;
>>>>>>> 728176898934d524cd65009808470807ec532eb4
//		moveWasCalled = false;
//		currentY = panelGuide.anchoredPosition.y;
//		startY = currentY;
//		previousY = currentY;
	}
	/*
	void Update () {
	/*	previousY = currentY;
		currentY = panelGuide.anchoredPosition.y;
		if (currentY - previousY < 0)
			MovingDown ();
		else if (currentY - previousY > 0)
			MovingUp ();
		if (currentY <= desiredBottom + 1 || currentY >= desiredTop - 1 && moveWasCalled) {
			moveWasCalled = false;
			controller.changingTemp = false;
		}*/
	}

	void MovingDown() {
		/*if (!movingDown)
			startY = currentY;
		if (currentY > desiredBottom && startY - currentY > kern)
			panelGuide.anchoredPosition = new Vector2(panelGuide.localPosition.x, desiredBottom);

		movingDown = true;
<<<<<<< HEAD
		movingUp = false;*/
//		moveWasCalled = true;
=======
		movingUp = false;
		moveWasCalled = true;
>>>>>>> 728176898934d524cd65009808470807ec532eb4
		Debug.Log ("moving down");
		controller.changingTemp = true; // hack way to get the screen not to move the same way when using temp slider.
	}

	void MovingUp() {
		/*if (!movingUp)
			startY = currentY;
		if (currentY - startY > kern && currentY < desiredTop)
			panelGuide.anchoredPosition = new Vector2(panelGuide.localPosition.x, desiredTop);
		movingDown = false;
<<<<<<< HEAD
		movingUp = true;*/
//		moveWasCalled = true;
=======
		movingUp = true;
		moveWasCalled = true;
>>>>>>> 728176898934d524cd65009808470807ec532eb4
		controller.changingTemp = true; // hack way to get the screen not to move the same way when using temp slider.
		Debug.Log ("moving up");
	}*/

	public void SetUpPanel() {
		panelGuide.anchoredPosition = new Vector2(panelGuide.anchoredPosition.x, desiredTop);
	}

<<<<<<< HEAD
	public void OnClicked() {
		if (up) {
			panelGuide.anchoredPosition = new Vector2 (panelGuide.anchoredPosition.x, desiredBottom);
			up = false;
		} else {
			panelGuide.anchoredPosition = new Vector2 (panelGuide.anchoredPosition.x, desiredTop);
			up = true;
=======
	public void OnClick() {
		if (isUp) {
			panelGuide.anchoredPosition = new Vector2 (panelGuide.anchoredPosition.x, desiredBottom);
			isUp = false;
		} else {
			panelGuide.anchoredPosition = new Vector2(panelGuide.anchoredPosition.x, desiredTop);
			isUp = true;
>>>>>>> 728176898934d524cd65009808470807ec532eb4
		}
	}
	
}
