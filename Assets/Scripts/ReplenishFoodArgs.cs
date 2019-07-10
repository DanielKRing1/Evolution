using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplenishFoodArgs : EventArgs {

	public int foodCount;

	public ReplenishFoodArgs(int foodCount) {
		this.foodCount = foodCount;
	}
}
