using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextureAnimator : MonoBehaviour {

	public Texture2D[] frames;
	public int framesPerSecond;
	public RawImage image;

	private int loadCount;

	private float index;

	void Start () {
		loadCount = 0;
	}

	void Update () {
	if (loadCount < frames.Length) {
			frames [loadCount] = Resources.Load ("MainMenuAnimation/mm00" + (loadCount + 111)) as Texture2D;
			loadCount++;
		} else {
			index = Time.time * framesPerSecond;
			index = index % frames.Length;
			image.texture = frames [(int)index];
		}
		//THis second way to do it looks more elegant, but results in missing frames
		//index = Time.time * framesPerSecond;
		//index = index % frames.Length;
		//image.texture = frames [(int)index];
	}

}
