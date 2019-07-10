using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour, IEdible {

	protected float animationCounter = 0;
	protected float RADIUS;

	protected bool alive = true;
	public bool Alive { get { return this.alive; } }
	
	protected Body body;
	public float size { get { return this.body.size; } }
	public float speed { get { return this.body.speed; } }
	public float attack { get { return this.body.attack; } }

	protected Movement movement;
	protected bool isMoving = false;

	protected int offspring = 0;

	public virtual void HandleMoveLogic() {

		if (!this.alive)
			return;

		// Decide how to move
		this.movement = this.body.Brain.ChooseDirection (this.transform.position, this.body.size);

		// Make movement
		if (movement.moveType == "target")
			this.body.MoveToTarget (ref movement);
		else
			this.body.SearchForTarget (ref movement);
		
		// Check energy level
		if (!movement.successfulMove) {
			this.GetEaten ();
		} else {
			EventManager.Broadcast (EVENT.AddMovingAnimal, new MovingAnimalArgs (this));
			isMoving = true;
		}

		this.animationCounter = 0;
	}
	public void HandleMoveAnimation() {
		if (!this.alive)
			return;
		
		CheckOutOfBounds ();

		float remainingDistance = (this.movement.target3 - this.transform.position).magnitude;

		// Done Animating movement
		if (Mathf.Abs (remainingDistance) <= 0.1) {
			EventManager.Broadcast (EVENT.RemoveMovingAnimal, new MovingAnimalArgs (this));
			this.isMoving = false;
		}

		// transform.position = Vector3.MoveTowards(transform.position, this.movement.target3, (remainingDistance/ANIMATION_TIME));
		// transform.position = Vector3.Lerp(transform.position, this.movement.target3, Time.deltaTime / 0.25f);

		this.animationCounter += Time.deltaTime / Constants.ANIMATION_TIME;
		transform.position = Vector3.Lerp(this.movement.startingPosition3, this.movement.target3, this.animationCounter);
	}
	private void CheckOutOfBounds() {
		if (this.transform.position.x + this.RADIUS > 4 * Constants.worldScale)
			this.movement.MakeXNeg (this.transform.position);
		else if (this.transform.position.x - this.RADIUS < -4 * Constants.worldScale)
			this.movement.MakeXPos (this.transform.position);
		if (this.transform.position.z + this.RADIUS > 4 * Constants.worldScale)
			this.movement.MakeYNeg (this.transform.position);
		else if (this.transform.position.z - this.RADIUS < -4 * Constants.worldScale)
			this.movement.MakeYPos (this.transform.position);
	}

	public float GetEaten() {
		if (this.alive) {
			EventManager.Broadcast (EVENT.RemoveMovingAnimal, new MovingAnimalArgs (this));
			this.alive = false;
			Destroy (this.gameObject);

			return this.body.energy / 2;
		}
		return 0;
	}

	public void EatNearbyFood() {
		Collider[] cols = Physics.OverlapSphere (this.transform.position, this.RADIUS);

		foreach (Collider col in cols) {
			string tag = col.tag;
			string energyStatus;

			switch (tag) {

			case "food":
				var script = col.gameObject.GetComponent<IEdible> ();
				float foodCount = script.GetEaten ();

				energyStatus = this.body.EatFood (foodCount);

				if (energyStatus == "full") {
					this.Reproduce ();
				}
				break;
			}
		}

		Collider[] cols2 = Physics.OverlapSphere (this.transform.position, this.body.Brain.vision + this.RADIUS);

		foreach (Collider col in cols2) {
			string tag = col.tag;
			string energyStatus;

			switch (tag) {

			case "animal":
				var script = (Animal) col.gameObject.GetComponent<IEdible> ();

				if (this.size >= 2 * script.size && this.speed > script.speed && this.attack > script.attack) {
					float energy = script.GetEaten ();
				
					energyStatus = this.body.EatAnimal (energy);

					if (energyStatus == "full") {
						this.Reproduce ();
					}
				}
				break;
			}
		}
	}


	public void Init() {
		body = new Body (Constants.MIN_SPEED, Constants.MAX_SPEED, Constants.MIN_SIZE, Constants.MAX_SIZE, Constants.MIN_ATTACK, Constants.MAX_ATTACK, Constants.MIN_VISION, Constants.MAX_VISION, 0);

		InitColor ();

		// Change size
		this.transform.localScale = new Vector3(this.body.size, this.body.size, this.body.size);

		Vector3 boxCollider = this.GetComponent<BoxCollider> ().size;
		this.RADIUS = Mathf.Sqrt(Mathf.Pow(boxCollider.x / 2, 2) + Mathf.Pow(boxCollider.y / 2, 2) + Mathf.Pow(boxCollider.z / 2, 2));
	}

	public void Init(float minSpeed, float maxSpeed, float minSize, float maxSize, float minAttack, float maxAttack, float minVision, float maxVision, Dictionary<Vector2, float> directionBias, int wealthBonus) {
		body = new Body (minSpeed, maxSpeed, minSize, maxSize, minAttack, maxAttack, minVision, maxVision, directionBias, wealthBonus);

		InitColor ();

		// Change size
		this.transform.localScale = new Vector3(this.body.size, this.body.size, this.body.size);

		Vector3 boxCollider = this.GetComponent<BoxCollider> ().size;
		this.RADIUS = Mathf.Sqrt(Mathf.Pow(boxCollider.x / 2, 2) + Mathf.Pow(boxCollider.y / 2, 2) + Mathf.Pow(boxCollider.z / 2, 2));
	}
	private void InitColor() {
		if (this.body.speed > Constants.currentMaxSpeed)
			Constants.currentMaxSpeed = this.body.speed;

		if (this.body.Brain.vision > Constants.currentMaxVision)
			Constants.currentMaxVision = this.body.Brain.vision;

		// Change color
		Color bodyColor = Color.Lerp(Color.green, Color.red, body.speed / Constants.currentMaxSpeed);
		bodyColor.a = this.body.Brain.vision / Constants.currentMaxVision;

		Material newMaterial = new Material (Shader.Find ("Standard"));
		newMaterial.color = bodyColor;
		this.GetComponent<MeshRenderer> ().material = newMaterial;
	}

	private void Reproduce() {
		this.offspring++;

		float x = this.transform.position.x;
		float z = this.transform.position.z;
		float unit = Constants.worldScale / 2;
		float clamp = 4 * Constants.worldScale;
		Vector3 newPosition = Random.value >= 0.5f ?
			new Vector3 (
				Constants.GetClampedRandomFloat(x - unit, x - unit / 2, clamp),
				2,
				Constants.GetClampedRandomFloat(z - unit, z - unit / 2, clamp)
			)
			:
			new Vector3 (
				Constants.GetClampedRandomFloat(x + unit / 2, x + unit, clamp),
				2,
				Constants.GetClampedRandomFloat(z + unit / 2, z + unit, clamp)
			);

		float minSpeed = this.body.speed - 4 < 0 ? 1 : this.body.speed - 4;
		float maxSpeed = this.body.speed + 5;
		float minSize = this.body.size -2 <= 0 ? 1 : this.body.size - 2;
		float maxSize = this.body.size + 3;
		float minAttack = this.body.attack - 4 < 0 ? 1 : this.body.attack - 4;
		float maxAttack = this.body.attack + 5;
		float minVision = this.body.Brain.vision - 14 <= 0 ? 1 : this.body.Brain.vision - 14;
		float maxVision = this.body.Brain.vision + 15;

		Dictionary<Vector2, float> directionBias = this.body.Brain.DirectionBias;

		ReproduceArgs newAnimal = new ReproduceArgs (newPosition, minSpeed, maxSpeed, minSize, maxSize, minAttack, maxAttack, minVision, maxVision, directionBias, this.offspring + 1);
		EventManager.Broadcast (EVENT.Reproduce, newAnimal);
	}


	private void InitPosition() {
		float x = Random.Range (-10, 10);
		float y = Random.Range (-10, 10);

		this.transform.position = new Vector3 (x, 2, y);
	}
}
