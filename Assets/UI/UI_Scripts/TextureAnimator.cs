using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextureAnimator : MonoBehaviour {

	public Texture2D[] frames;
	public int framesPerSecond;
	public RawImage image;

	private float index;

	void Update () {
		index = Time.time * framesPerSecond;
		index = index % frames.Length;
		image.texture = frames [(int)index];
	}

}
