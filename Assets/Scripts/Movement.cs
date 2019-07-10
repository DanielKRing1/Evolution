using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement {

	public Vector2 startingPosition;
	public Vector3 startingPosition3 {
		get { return new Vector3 (this.startingPosition.x, 2, this.startingPosition.y); }
	}
	public Vector2 target;
	public Vector3 target3 {
		get { return new Vector3 (this.target.x, 2, this.target.y); }
	}
	public float targetDistance;
	public Vector2 direction;
	public string moveType;

	public bool successfulMove;

	public Movement() {
		this.target = Vector2.positiveInfinity;
		this.direction = Vector2.positiveInfinity;
	}

	public void setTarget(Vector2 startingPosition, Vector2 target) {
		this.startingPosition = startingPosition;
		this.direction = (target - startingPosition).normalized;
		this.moveType = "target";

		this.target = target;
		this.targetDistance = (target - startingPosition).magnitude;
	}

	public void setSearch(Vector2 startingPosition, Vector2 direction) {
		this.startingPosition = startingPosition;
		this.direction = direction;
		this.moveType = "search";

		// Set target
	}

	public void MakeXPos(Vector3 curPos3) {
		if (this.direction.x < 0)
			FlipTargetX (curPos3);
	}
	public void MakeXNeg(Vector3 curPos3) {
		if (this.direction.x > 0)
			this.FlipTargetX (curPos3);
	}
	private void FlipTargetX(Vector3 curPos3) {
		this.direction = new Vector2 (-1 * this.direction.x, this.direction.y);

		Vector2 curPos = new Vector2 (curPos3.x, curPos3.z);
		Vector2 remainingDisplacement = this.target - curPos;

		this.startingPosition = curPos;
		this.target = curPos + new Vector2 (-1 * remainingDisplacement.x, remainingDisplacement.y);
	}

	public void MakeYPos(Vector3 curPos3) {
		if (this.direction.y < 0)
			this.FlipTargetY (curPos3);
	}
	public void MakeYNeg(Vector3 curPos3) {
		if (this.direction.y > 0)
			this.FlipTargetY (curPos3);
	}
	private void FlipTargetY(Vector3 curPos3) {
		this.direction = new Vector2 (this.direction.x, -1 * this.direction.y);

		Vector2 curPos = new Vector2 (curPos3.x, curPos3.z);
		Vector2 remainingDisplacement = this.target - curPos;

		this.startingPosition = curPos;
		this.target = curPos + new Vector2(remainingDisplacement.x, -1 * remainingDisplacement.y);
	}

}
