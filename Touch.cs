using UnityEngine;
using System.Collections;

public class Touch : MonoBehaviour
{
	//Lower values reduce jitter (fcmin or minimum cutoff frequency)
	//Keep above 0. Start at 1. Increase until no jitter.
	public float jitterReduction;
	//Higher values reduce lag (beta or slope of velocity for cutoff frequency)
	//Keep above 0. Start at 0. Increase until no lag.
	public float lagReduction;
	
	private Vector2 currentPosition;
	private Vector2 currentVelocity;
	private Vector2 previousFilteredPosition;
	private Vector2 previousFilteredVelocity;
	
	void Start ()
	{
		currentPosition = Camera.main.WorldToScreenPoint (transform.position);
		currentVelocity = Vector2.zero;
		previousFilteredPosition = Camera.main.WorldToScreenPoint (transform.position);
		previousFilteredVelocity = Vector2.zero;
	}
	
	void Update ()
	{
		if (Input.touchCount > 0) {
			UnityEngine.Touch touch = Input.GetTouch (0);
			
			if (touch.phase == TouchPhase.Moved) {
				currentPosition = touch.position;
				currentVelocity = touch.deltaPosition / Time.deltaTime;
			} else { //For began, ended, canceled, and stationary
				currentPosition = touch.position;
				currentVelocity = Vector2.zero;	
			}
		}
		
		Vector2 filteredPosition;
		Vector2 filteredVelocity;
		//Get a smooth velocity using exponential smoothing
		filteredVelocity = (1 / (2 * Mathf.PI)) * currentVelocity + (1 - 1 / (2 * Mathf.PI)) * previousFilteredVelocity;
		//Use velocity to get smoothing factor for position
		float cutoffFrequencyX = jitterReduction + lagReduction * Mathf.Abs (filteredVelocity.x);
		float cutoffFrequencyY = jitterReduction + lagReduction * Mathf.Abs (filteredVelocity.y);
		float tauX = 1 / (2 * Mathf.PI * cutoffFrequencyX);
		float tauY = 1 / (2 * Mathf.PI * cutoffFrequencyY);
		float te = Time.deltaTime;
		//Get a smooth position using exponential smoothing with changing smoothing factor
		filteredPosition.x = (currentPosition.x + tauX / te * previousFilteredPosition.x) / (1 + tauX / te);
		filteredPosition.y = (currentPosition.y + tauY / te * previousFilteredPosition.y) / (1 + tauY / te);
		//Set for next frame
		previousFilteredPosition = filteredPosition;
		previousFilteredVelocity = filteredVelocity;
		
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (filteredPosition.x, filteredPosition.y, 8));
	}
}
