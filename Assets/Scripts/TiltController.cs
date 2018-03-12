using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TiltController : MonoBehaviour {
	// So I can vary the amount by which the ground shifts
	public float rotationSpeed;
	// Regulates how far the ground is allowed to turn
	public float allowedAngle;
	// The number of walls to place
	public int numWalls;
	// The number of holes to place
	public int numHoles;
	// The marble object that's going to roll around
	public GameObject marble;
	// The wall prefab so I can make a maze
	public GameObject wallPrefab;
	// The hole prefab so I can make holes
	public GameObject holePrefab;
	// The winning pad prefad so I can set the place the player can win
	public GameObject winPrefab;

	private float worldSize = 14.5f;
	public static bool needNewGame;
	private ArrayList generatedComponents;

	void Start () {
		generatedComponents = new ArrayList ();
		NewGame ();
	}

	// Update is called once per frame
	void Update () {
		if (needNewGame) {
			NewGame ();
			needNewGame = false;
		}
		Vector3 angles = transform.eulerAngles;
		#if UNITY_IOS
		float moveZ = -Input.acceleration.x * rotationSpeed + angles.z;
		float moveX = -Input.acceleration.z * rotationSpeed + angles.x;
		#elif UNITY_ANDROID
		float moveZ = -Input.acceleration.x * rotationSpeed + angles.z;
		float moveX = -Input.acceleration.z * rotationSpeed + angles.x;
		#else
		float moveZ = -Input.GetAxis ("Horizontal") * rotationSpeed + angles.z;
		float moveX = Input.GetAxis ("Vertical") * rotationSpeed + angles.x;
		#endif
		if (GameStatusController.isStopped) {
			moveX = 0;
			moveZ = 0;
		}

		// If any of the new tilted ground angles are too big, put it back
		if (moveX > allowedAngle && moveX < allowedAngle * 4) {
			moveX = allowedAngle;
		} else if (moveX > (360 - allowedAngle * 4) && moveX < (360 - allowedAngle)) {
			moveX = 360 - allowedAngle;
		}
		if (moveZ > allowedAngle && moveZ < allowedAngle * 4) {
			moveZ = allowedAngle;
		}
		if (moveZ > (360 - allowedAngle * 4) && moveZ < (360 - allowedAngle)) {
			moveZ = 360 - allowedAngle;
		}

		// Actually apply the transform
		transform.eulerAngles = new Vector3 (moveX, 0.0f, moveZ);
		// Set the marble to not transform so it doesn't wobble
		marble.transform.localScale = new Vector3 (0.25f, 0.75f, 0.25f);
		marble.transform.eulerAngles = Vector3.zero;
	}

	void NewGame() {
		foreach (Object toss in generatedComponents) {
			Destroy (toss);
		}
		generatedComponents = new ArrayList ();
		float x = Random.Range (-worldSize, worldSize);
		float z = Random.Range (-worldSize, worldSize);
		for (int i = 0; i < numWalls; i++) {
			generatedComponents.Add(Instantiate (wallPrefab, new Vector3 (x, 0.75f, z), Quaternion.identity, transform)); 
			x = Random.Range (-worldSize, worldSize);
			z = Random.Range (-worldSize, worldSize);
		}
		for (int i = 0; i < numHoles; i++) {
			generatedComponents.Add(Instantiate (holePrefab, new Vector3 (x, 0.001f, z), Quaternion.identity, transform)); 
			x = Random.Range (-worldSize, worldSize);
			z = Random.Range (-worldSize, worldSize);
		}
		generatedComponents.Add(Instantiate (winPrefab, new Vector3 (x, 0.001f, z), Quaternion.identity, transform)); 
		x = Random.Range (-worldSize, worldSize);
		z = Random.Range (-worldSize, worldSize);
		marble.transform.SetPositionAndRotation(new Vector3(x, 5.0f, z), Quaternion.identity);
	}
}
