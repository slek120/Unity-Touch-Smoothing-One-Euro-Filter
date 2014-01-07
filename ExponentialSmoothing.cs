using UnityEngine;
using System.Collections;

public class ExponentialSmoothing : MonoBehaviour
{
	//Smoothing factor from 0 to 1. 1 for no smoothing. 0 for so smooth it doesn't even move.
	public float alpha;
	
	private Vector2 currentPosition;
	private Vector2 previousFilteredPosition;
	
	void Start ()
	{
		currentPosition = Camera.main.WorldToScreenPoint (transform.position);
		previousFilteredPosition = Camera.main.WorldToScreenPoint (transform.position);
	}
	
	void Update ()
	{
		if (Input.touchCount > 0) {
			UnityEngine.Touch touch = Input.GetTouch (0);
			currentPosition = touch.position;
		}

		alpha = Mathf.Clamp (alpha, 0, 1);
		Vector2 filteredPosition = alpha * currentPosition + (1 - alpha) * previousFilteredPosition;
		previousFilteredPosition = filteredPosition;

		transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (filteredPosition.x, filteredPosition.y, 8));
	}
}
