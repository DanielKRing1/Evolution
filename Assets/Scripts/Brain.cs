using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain {

	// 8 directions on a compass
	private Vector2[] directions = { Vector2.up, new Vector2(1, 1).normalized, Vector2.right, new Vector2(1, -1).normalized, Vector2.down, new Vector2(-1, -1).normalized, Vector2.left, new Vector2(-1, 1).normalized };
	protected Dictionary<Vector2, float> directionBias = new Dictionary<Vector2, float>();
	public Dictionary<Vector2, float> DirectionBias {
		get { return this.directionBias; }
	}
	public float vision;

	public Brain() {
		InitDirectionBias ();

		this.vision = Constants.GetRandomFloat(Constants.MIN_VISION, Constants.MAX_VISION);
	}

	public Brain(float minVision, float maxVision) {
		InitDirectionBias ();

		this.vision = Constants.GetRandomFloat(minVision, maxVision);
	}

	public Brain(float minVision, float maxVision, Dictionary<Vector2, float> directionBias) {
		this.MutateDirectionBias (ref directionBias);
		this.MutateDirectionBias (ref directionBias);
		this.directionBias = directionBias;

		this.vision = Constants.GetRandomFloat(minVision, maxVision);
	}


	public Movement ChooseDirection(Vector3 position3d, float size) {
		// Find nearby GameObjects
		Vector2 position = new Vector2 (position3d.x, position3d.z);
		Collider[] cols = Physics.OverlapSphere (position3d, vision);

		float closestFoodDistance = Mathf.Infinity;
		Vector2 closestFood = Vector2.positiveInfinity;
		float closestPredatorDistance = Mathf.Infinity;
		Vector2 closestPredator = Vector2.positiveInfinity;

		// Find closest Food Object
		foreach (Collider col in cols) {
			// Get Collider info: Tag, Vector2 position
			string tag = col.tag;
			Vector3 colPos3d = col.transform.position;
			Vector2 colPos = new Vector2 (colPos3d.x, colPos3d.z);

			switch (tag) {

			case "food":
				DetermineIfCloser (position, colPos, ref closestFood, ref closestFoodDistance);
				break;

			case "animal":
				// Can eat
				if (size > 2 * col.gameObject.GetComponent<Animal> ().size) {
					DetermineIfCloser(position, colPos, ref closestFood, ref closestFoodDistance);
				}
				break;

			}
		}

		Movement movement = new Movement ();

		// Found Food
		if (!closestFood.Equals (Vector2.positiveInfinity)) {
			movement.setTarget (position, closestFood);
		} else {
			movement.setSearch (position, this.UseDirectionBias ());
		}

		return movement;
	}
	private void DetermineIfCloser(Vector2 currentPosition, Vector2 currentFood, ref Vector2 closestFood, ref float closestFoodDistance) {
		if (closestFood == Vector2.positiveInfinity) {
			closestFood = currentFood;
			closestFoodDistance = (currentPosition - currentFood).magnitude;
		} else if ((currentPosition - currentFood).magnitude < closestFoodDistance) {
			closestFood = currentFood;
			closestFoodDistance = (currentPosition - currentFood).magnitude;
		}
	}

	private Vector2 UseDirectionBias() {
		float rand = Random.Range (0, 100);
		Vector2 dir = Vector2.up;

		for(int i = 0; i < directions.Length; i++) {
			dir = directions [i];

			float probability = directionBias [dir];
			if (rand <= probability)
				return dir;
			rand -= probability;
		}

		return dir;
	}



	private void InitDirectionBias() {

		IList<Vector2> randomDirections = Shuffle<Vector2> (this.directions);
		float remainder = 100;

		for(int i = 0; i < directions.Length; i++) {
			Vector2 dir = directions [i];

			float bias = Random.Range (0, remainder / 2.5f);
			remainder -= bias;

			if (i == directions.Length - 1)
				directionBias.Add (dir, remainder);
			else
				directionBias.Add (dir, bias);
		}
	}
	private void MutateDirectionBias(ref Dictionary<Vector2, float> directionBias) {
		int a = Random.Range (0, this.directions.Length);
		int b = Random.Range (0, this.directions.Length);
		while (a == b)
			b = Random.Range (0, this.directions.Length);

		Vector2 dirA = this.directions [a];
		Vector2 dirB = this.directions [b];

		float temp = 0.25f * directionBias [dirA];
		directionBias [dirA] *= 0.75f;
		directionBias [dirB] += temp;
	}

	private IList<T> Shuffle<T>(IList<T> list) {
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = Random.Range(0, n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}

		return list;
	}

}
