using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Body depends on Brain, needs access to vision to calculate starting energy
// 
public class Body {

	protected int age = 0;

	protected Brain brain;
	public Brain Brain { get { return this.brain; } }

	public float singleMoveEnergy,
					energyToReproduce,
					energy,
					energyFromFood,
					size,
					speed,
					attack;

	public bool HasEnergy {
		get { return this.energy > 0; }
	}

	public Body(float minSpeed, float maxSpeed, float minSize, float maxSize, float minAttack, float maxAttack, float minVision, float maxVision, Dictionary<Vector2, float> directionBias, int wealthBonus) {
		this.brain = new Brain (minVision, maxVision, directionBias);

		this.speed = Constants.GetRandomFloat(minSpeed, maxSpeed);
		this.size = Constants.GetRandomFloat(minSize, maxSize);
		this.attack = Constants.GetRandomFloat (minAttack, maxAttack);

		InitEnergy (wealthBonus);
	}
	public Body(float minSpeed, float maxSpeed, float minSize, float maxSize, float minAttack, float maxAttack, float minVision, float maxVision, int wealthBonus) {
		this.brain = new Brain (minVision, maxVision);

		this.speed = Constants.GetRandomFloat(minSpeed, maxSpeed);
		this.size = Constants.GetRandomFloat(minSize, maxSize);
		this.attack = Constants.GetRandomFloat (minAttack, maxAttack);

		InitEnergy (wealthBonus);
	}

	// Return true if success, false if out of energy
	public bool UseEnergy(float distance) {
		this.age++;

		float energyCosumption = this.CalcEnergyUsage (distance);
		
		this.energy -= energyCosumption;

		return this.energy > 0;
	}

	public void SearchForTarget(ref Movement movement) {
		// Update position
		Vector2 displacement = movement.direction * speed;
		movement.target = movement.startingPosition + displacement;

		// Use energy
		movement.successfulMove = UseEnergy (speed) ? true : false;
	}
	public void MoveToTarget(ref Movement movement) {
		Vector2 singleMoveDisplacement = movement.direction * speed;

		float singleMoveDistance = singleMoveDisplacement.magnitude;
		float actualDistance;
		// No need to move so far
		// Made it to target
		if (singleMoveDistance > movement.targetDistance) {
			
			actualDistance = movement.targetDistance;
			// Do nothing to movement.target; 

		} else {
			
			// Can't reach
			actualDistance = singleMoveDistance;
			movement.target = movement.startingPosition + singleMoveDisplacement;
			movement.targetDistance = actualDistance;

		}

		movement.successfulMove = UseEnergy (actualDistance) ? true : false;
	}

	public string EatFood(float foodCount) {
		// Increase energy
		this.energy += (foodCount * this.energyFromFood);

		// If double initial energy, use energy to reproduce
		string energyStatus = this.energy >= this.energyToReproduce ? "full" : "hungry";
		if (energyStatus == "full")
			this.energy -= (this.energyToReproduce / 2);
		
		return energyStatus;
	}

	public string EatAnimal(float energy) {
		// Increase energy
		this.energy += energy;

		// If double initial energy, use energy to reproduce
		string energyStatus = this.energy >= this.energyToReproduce ? "full" : "hungry";
		if (energyStatus == "full")
			this.energy -= (this.energyToReproduce / 2);

		return energyStatus;
	}

	// Overwrite in inheritted classes to introduce new Body types, ie Mammal Body, Bird Body
	private float CalcEnergyUsage(float distance) {
		// size^3 * distance^2 * vision
		return ((1 + this.age / 10) * Mathf.Pow(this.size, 3)) + (size * Mathf.Pow(distance, 2)) + Mathf.Pow(this.attack, 2) + (this.brain.vision * distance);
	}
	private void InitEnergy(int wealthBonus) {
		this.singleMoveEnergy = this.CalcEnergyUsage (this.speed);
		this.energyToReproduce = 20 * this.singleMoveEnergy;

		this.InitStartingEnergy (wealthBonus);
		this.InitEnergyFromFood ();
	}
	private void InitStartingEnergy(int wealthBonus) {
		int MIN_MOVES = 1 + 1 * wealthBonus;
		int MAX_MOVES = 3 + 1 * wealthBonus;

		float MIN_ENERGY = MIN_MOVES * this.singleMoveEnergy;
		float MAX_ENERGY = MAX_MOVES * this.singleMoveEnergy;

		this.energy = Constants.GetRandomFloat(MIN_ENERGY, MAX_ENERGY);
	}
	private void InitEnergyFromFood() {
		int MIN_MOVES = 1;
		int MAX_MOVES = 3;

		float MIN_ENERGY = MIN_MOVES * this.singleMoveEnergy;
		float MAX_ENERGY = MAX_MOVES * this.singleMoveEnergy;

		this.energyFromFood = Constants.GetRandomFloat(MIN_ENERGY, MAX_ENERGY);
	}
}
