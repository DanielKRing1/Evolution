using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {

	public static int worldScale = 25;
	public static int startingAnimalCount = 10;
	public static int maxFoodCount = 100;
	public static int movesUntilUpdateFood = 35;

	public static float ANIMATION_TIME = 0.05f;

	public static float MIN_SPEED = 5;
	public static float MAX_SPEED = 25;
	public static float currentMaxSpeed = Constants.MAX_SPEED;
	public static float MIN_SIZE = 1;
	public static float MAX_SIZE = 10;
	public static float MIN_ATTACK = 1;
	public static float MAX_ATTACK = 10;
	public static float MIN_VISION = 0;
	public static float MAX_VISION = 80;
	public static float currentMaxVision = Constants.MAX_VISION;

	public static float GetRandomFloat(float min, float max) {
		return Random.Range (100 * min, 100 * max) / 100;
	}

	public static float GetClampedRandomFloat(float min, float max, float clamp) {
		float rand = Random.Range (100 * min, 100 * max) / 100;
		return Mathf.Clamp (rand, -1 * clamp, clamp);
	}
}
