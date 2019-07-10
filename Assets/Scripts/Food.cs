using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IEdible {

	// GO not get destroyed immediately
	// Use bool to check if waiting to be destroyed
	public bool active = true;

	public float GetEaten() {
		if(this.active) {
			this.active = false; 

			EventManager.Broadcast (EVENT.EatFood, new ReplenishFoodArgs(1));
			DestroyImmediate (this.gameObject);

			return 1;
		}
		return 0;
	}
}
