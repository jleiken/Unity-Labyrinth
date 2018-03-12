using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameStatusController : MonoBehaviour {

	// The text that displays if we win or lose
	public GameObject statusTextObject;
	// The light - undims before a game
	public GameObject sun;

	private TextMesh statusText;
	private Light light;
	private float losingDepth = -15.0f;
	private Rigidbody rb;

	// Contain program state
	public static bool isStopped = true;
	public static bool hasEnded = false;
	private bool isLighting, wasLighting;
	private float lStartTime, lEndTime;

	void Start () {
		statusText = statusTextObject.GetComponent<TextMesh> ();
		statusText.text = "";
		light = sun.GetComponent<Light> ();
		light.intensity = 0.1f;
		rb = GetComponent<Rigidbody>();
		rb.isKinematic = true;
		isLighting = false;
		wasLighting = false;
	}
	
	void Update () {
		if (Input.GetAxis ("Cancel") > 0 || transform.position.y < losingDepth) {
			Loser ();
		}
		if (isStopped && !hasEnded && !isLighting && !wasLighting) {
			isLighting = true; 
			lStartTime = Time.time;
			lEndTime = lStartTime + 3;
		} else if (isLighting) {
			light.intensity = Mathf.InverseLerp (lStartTime, lEndTime, Time.time);
			if (light.intensity >= 1.0f) {
				isLighting = false;
				wasLighting = true;
			}
		} else if (wasLighting) {
			wasLighting = false;
			rb.isKinematic = false;
			isStopped = false;
		} 
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Winning Pad")) {
			Winner ();
		} else if (other.gameObject.CompareTag ("Hole") && !isStopped) {
			transform.position = new Vector3 (0.0f, -5.0f, 0.0f);
			rb.isKinematic = true;
			Loser ();
		}
	}

	void Winner() {
		statusText.text = "You win!";
		isStopped = true;
		hasEnded = true;
	}

	void Loser() {
		statusText.text = "You lose :(";
		isStopped = true;
		hasEnded = true;
	}
}
