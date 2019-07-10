using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public bool auto;
	public bool oscillate;
	public int cycleLength = 25;
	public int maxFoodCount = Constants.maxFoodCount;
	public int foodChange = -1;

	public GameObject timer;
	public GameObject world;
	public GameObject animal;
	public GameObject food;

	public int foodCount;

	public int movesMade = 0;

	private HashSet<Animal> movingAnimals = new HashSet<Animal>();
	private HashSet<Animal> stillMovingAnimals = new HashSet<Animal>();

	// Use this for initialization
	void Start () {
		AddEventHandlers ();

		InitWorld ();

		if (this.auto) {
			var newTimer = Instantiate (timer, Vector3.zero, Quaternion.identity);
			var script = newTimer.GetComponent<Timer> ();
			script.Init (this.MoveAnimals, this.ReplenishFood, Constants.ANIMATION_TIME + 0.02f);
			script.Start ();
		} 
	}
	
	void FixedUpdate () {

		if (!this.auto && Input.GetKeyDown (KeyCode.Space)) {
			MoveAnimals ();
		} 

		MakeAnimalsEatInOrder ();
	}

	// This is where Animals are added to movingAnimals
	private void MoveAnimals() {
		this.movesMade++;

		if (this.oscillate) {
			if (this.movesMade % this.cycleLength == 0) {
				this.foodChange *= -1;
			}
			this.maxFoodCount += this.foodChange;
		}

		
		GameObject[] animals = GameObject.FindGameObjectsWithTag ("animal");
		Debug.Log (animals.Length);
		foreach (GameObject animal in animals) {
			animal.GetComponent<Animal> ().HandleMoveLogic ();
		}

		this.movingAnimals = new HashSet<Animal>(this.stillMovingAnimals);
	}
	private void MakeAnimalsEatInOrder() {

		foreach (Animal animal in this.movingAnimals) {
			if(animal != null) {
				animal.HandleMoveAnimation ();
				animal.EatNearbyFood ();
			}
		}

		this.movingAnimals = new HashSet<Animal> (this.stillMovingAnimals);
	}

	private void InitWorld() {
		world.transform.localScale = new Vector3 (Constants.worldScale, 1, Constants.worldScale);
		Instantiate (world, new Vector3 (0, 0, 0), Quaternion.identity);

		InitAnimals ();
		InitFood ();
	}
	private void InitAnimals() {
		for (int i = 0; i < Constants.startingAnimalCount; i++) {
			float x = Random.Range (-4 * Constants.worldScale, 4 * Constants.worldScale);
			float y = Random.Range (-4 * Constants.worldScale, 4 * Constants.worldScale);

			var animalGO = Instantiate (animal, new Vector3 (x, 2, y), Quaternion.identity);
			animalGO.GetComponent<Animal>().Init ();
		}
	}
	private void Reproduce(EventArgs args) {
		ReproduceArgs newAnimal = (ReproduceArgs)args;

		var animalGO = Instantiate (animal, newAnimal.startingPosition, Quaternion.identity);
		animalGO.GetComponent<Animal>().Init (newAnimal.minSpeed, newAnimal.maxSpeed, newAnimal.minSize, newAnimal.maxSize, newAnimal.minAttack, newAnimal.maxAttack, newAnimal.minVision, newAnimal.maxVision, newAnimal.directionBias, newAnimal.wealthBonus);
	}
	private void AddAnimal(Vector3 position, Quaternion rotation) {

	}

	private void InitFood() {
		this.foodCount = Constants.maxFoodCount;
		AddFood (this.foodCount);
	}
	private void EatFood(EventArgs args) {
		var foodData = (ReplenishFoodArgs)args;
		this.foodCount -= foodData.foodCount;
	}
	private void ReplenishFood() {
		int missingFood = this.maxFoodCount - this.foodCount;

		this.foodCount += missingFood;
		this.AddFood (missingFood);
	}
	private void AddFood(int foodToAdd) {
		for (int i = 0; i < foodToAdd; i++) {
			float x = Random.Range (-4 * Constants.worldScale, 4 * Constants.worldScale);
			float y = Random.Range (-4 * Constants.worldScale, 4 * Constants.worldScale);

			Instantiate (food, new Vector3 (x, 2, y), Quaternion.identity);
		}
	}
		
	private void AddMovingAnimal(EventArgs args) {
		MovingAnimalArgs animalData = (MovingAnimalArgs)args;

		this.stillMovingAnimals.Add (animalData.script);
	}
	private void RemoveMovingAnimal(EventArgs args) {
		MovingAnimalArgs animalData = (MovingAnimalArgs)args;

		this.stillMovingAnimals.Remove(animalData.script);

		// if (this.movesMade % 5 == 0 && this.stillMovingAnimals.Count == 0) {
			// this.ReplenishFood ();
		// }
	}

	private void AddEventHandlers() {
		EventManager.AddHandler (EVENT.EatFood, EatFood);
		EventManager.AddHandler (EVENT.AddMovingAnimal, AddMovingAnimal);
		EventManager.AddHandler (EVENT.RemoveMovingAnimal, RemoveMovingAnimal);
		EventManager.AddHandler (EVENT.Reproduce, Reproduce);
	}
}
