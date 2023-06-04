using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkeletonGroupController {

	public GameObject origin;
	public int originRadius = 20;
	public List<GameObject> agents;

	int totalActiveAgents;

	public SkeletonGroupController(GameObject origin) {
		this.agents = new List<GameObject>();
		this.origin = origin;
		this.totalActiveAgents = 0;
	}

	public void CreateAgent(float directionFacing, GameObject skeletonPrefab) {
		totalActiveAgents++;
		GameObject skeleton = (GameObject) MonoBehaviour.Instantiate (
				skeletonPrefab, 
				origin.gameObject.transform.position, 
            	Quaternion.Euler (new Vector3 (0f, directionFacing, 0f))
        );

		SkeletonController controller = skeleton.GetComponent<SkeletonController> ();
		controller.SetGroupController (this);	
		agents.Add (skeleton);
	}

	public void DestroyAgent(SkeletonController agent) {
		totalActiveAgents--;
		agents.Remove (agent.gameObject);
	}

	public bool IsActive() {
		return totalActiveAgents > 0;
	}
}












