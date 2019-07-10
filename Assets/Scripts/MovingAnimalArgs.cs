using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnimalArgs : EventArgs {

	public Animal script;

	public MovingAnimalArgs(Animal script) {
		this.script = script;
	}
}
