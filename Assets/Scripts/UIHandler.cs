using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

	// The light - dims after a game
	public GameObject sun;

	private TextMesh statusText;
	private Light light;
	private bool isLighting, wasLighting;
	private float lStartTime, lEndTime;

	void Start () {
		light = sun.GetComponent<Light> ();
		statusText = this.gameObject.GetComponent<TextMesh> ();
	}
	
	void Update () {
		transform.position = new Vector3(-(GetWidth () / 4), 0, 0);
		if (GameStatusController.hasEnded && !isLighting && !wasLighting) {
			lStartTime = Time.time;
			lEndTime = lStartTime + 3;
			isLighting = true;
		}
		if (isLighting) {
			light.intensity = 1 - Mathf.InverseLerp (lStartTime, lEndTime, Time.time);
			if (light.intensity <= 0.1f) {
				isLighting = false;
				wasLighting = true;
				#if UNITY_IOS
				statusText.text = "Press the button on your headset to play again!";
				#elif UNITY_ANDROID
				statusText.text = "Press the button on your headset to play again!";
				#else
				statusText.text = "Press space to play again!";
				#endif
			}
		}
		if (wasLighting) {
			#if UNITY_IOS
			if (Input.touchCount > 0) {
				RestartGame();
			}
			#elif UNITY_ANDROID
			if (Input.touchCount > 0) {
				RestartGame();
			}
			#else
			if (Input.GetKey(KeyCode.Space)) {
				RestartGame();
			}
			#endif
		}
	}

	private void RestartGame() {
		statusText.text = "";
		wasLighting = false;
		GameStatusController.hasEnded = false;
		TiltController.needNewGame = true;
	}

	private float GetWidth() {
		float width = 0;
		foreach (char symbol in statusText.text) {
			CharacterInfo info;
			if (statusText.font.GetCharacterInfo(symbol, out info, statusText.fontSize, statusText.fontStyle)) {
				width += info.advance;
			}
		}
		return width * statusText.characterSize * 0.1f;
	}
}
