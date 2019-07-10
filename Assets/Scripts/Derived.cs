using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Derived : Base{

	public Derived() {

	}

	public override void Test() {
		Debug.Log ("In Derived");
	}
}
