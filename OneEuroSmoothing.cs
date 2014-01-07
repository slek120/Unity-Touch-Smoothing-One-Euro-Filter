using UnityEngine;
using System.Collections;

public class OneEuroSmoothing : MonoBehaviour
{
	//Higher values reduce jitter (fcmin or minimum cutoff frequency)
	//Keep above 0. Start at 1. Increase until no jitter.
	public float jitterReduction;
	//Higher values reduce lag (beta or slope of velocity for cutoff frequency)
	//Keep above 0. Start at 0. Increase until no lag.
	public float lagReduction;
	
	private Vector2 currentPosition;
	private Vector2 currentVelocity;
	private Vector2 filteredPosition;
	private Vector2 filteredVelocity;
	
	void Start ()
	{
		currentPosition = Camera.main.WorldToScreenPoint (transform.position);
		currentVelocity = Vector2.zero;
		filteredPosition = Camera.main.WorldToScreenPoint (transform.position);
		filteredVelocity = Vector2.zero;
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
		
		OneEuroFilter (currentPosition, currentVelocity, Time.deltaTime);
		
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (filteredPosition.x, filteredPosition.y, 8));
	}
	
	void OneEuroFilter (Vector2 currentPosition, Vector2 currentVelocity, float dt)
	{
		//Get a smooth velocity using exponential smoothing
		filteredVelocity = Filter (currentVelocity, filteredVelocity, Alpha (Vector2.one, dt));
		//Use velocity to get smoothing factor for position
		Vector2 cutoffFrequency;
		cutoffFrequency.x = jitterReduction + lagReduction * Mathf.Abs (filteredVelocity.x);
		cutoffFrequency.y = jitterReduction + lagReduction * Mathf.Abs (filteredVelocity.y);
		//Get a smooth position using exponential smoothing with smoothing factor from velocity
		filteredPosition = Filter (currentPosition, filteredPosition, Alpha (cutoffFrequency, dt));
	}
	
	Vector2 Alpha (Vector2 cutoff, float dt)
	{
		float tauX = 1 / (2 * Mathf.PI * cutoff.x);
		float tauY = 1 / (2 * Mathf.PI * cutoff.y);
		float alphaX = 1 / (1 + tauX / dt);
		float alphaY = 1 / (1 + tauY / dt);
		alphaX = Mathf.Clamp (alphaX, 0, 1);
		alphaY = Mathf.Clamp (alphaY, 0, 1);
		return new Vector2 (alphaX, alphaY);
	}
	
	Vector2 Filter (Vector2 current, Vector2 previous, Vector2 alpha)
	{
		float x = alpha.x * current.x + (1 - alpha.x) * previous.x;
		float y = alpha.y * current.y + (1 - alpha.y) * previous.y;
		return new Vector2 (x, y);
	}
}
