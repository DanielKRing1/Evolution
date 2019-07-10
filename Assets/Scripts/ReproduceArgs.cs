using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproduceArgs : EventArgs {

	public Vector3 startingPosition;
	public float minSpeed;
	public float maxSpeed;
	public float minSize;
	public float maxSize;
	public float minVision;
	public float maxVision;
	public float minAttack;
	public float maxAttack;
	public Dictionary<Vector2, float> directionBias;
	public int wealthBonus;

	public ReproduceArgs(Vector3 startingPosition, float minSpeed, float maxSpeed, float minSize, float maxSize, float minAttack, float maxAttack, float minVision, float maxVision, Dictionary<Vector2, float> directionBias, int wealthBonus) {
		this.startingPosition = startingPosition;
		this.minSpeed = minSpeed;
		this.maxSpeed = maxSpeed;
		this.minSize = minSize;
		this.maxSize = maxSize;
		this.minVision = minVision;
		this.maxVision = maxVision;
		this.minAttack = minAttack;
		this.maxAttack = maxAttack;
		this.directionBias = directionBias;
		this.wealthBonus = wealthBonus;
	}

}
