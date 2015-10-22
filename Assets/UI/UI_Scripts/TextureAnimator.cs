using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextureAnimator : MonoBehaviour {

	public Texture2D[] frames;
	public int framesPerSecond;
	public RawImage image;

	private float index;

	void Start () {
		for (int i = 0; i < frames.Length; i++) {
			frames[i] = Resources.Load ("MainMenuAnimation/mm00" + (i + 111)) as Texture2D;
		}
	}

	void Update () {
		index = Time.time * framesPerSecond;
		index = index % frames.Length;
		image.texture = frames [(int)index];
	}

}
