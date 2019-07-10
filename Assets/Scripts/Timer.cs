using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

	public delegate void EventHandler();
	private event EventHandler moveAnimals;
	private event EventHandler updateFood;

	private float previousEventTime;
	private bool active;
	private float interval;
	private int iterations = 0;

	public void Init(EventHandler moveAnimals, EventHandler updateFood, float interval) {
		this.interval = interval;
		this.moveAnimals = moveAnimals;
		this.updateFood = updateFood;
	}

	void FixedUpdate () {
		if (this != null && this.active && Time.time - this.previousEventTime >= this.interval) {
			this.previousEventTime = Time.time;
			this.moveAnimals ();
			iterations++;
		}

		if (this.iterations % Constants.movesUntilUpdateFood == 0) {
			this.updateFood ();
		}
	}
	
	public void Start () {
		this.previousEventTime = Time.time;
		this.active = true;
	}

	public void Pause () {
		this.active = false;
	}

}
