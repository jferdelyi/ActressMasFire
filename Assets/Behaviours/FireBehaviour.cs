using ActressMasWrapper;
using FireMAS;
using UnityEngine;

public class FireBehaviour : GraphicalAgent {

	// Use this for initialization
	public void Init(Cell pCell) {
		Agent = new Fire(pCell, this);
		transform.position = new Vector3(pCell.Transform.position.x, pCell.Transform.position.y, -0.1f);
		transform.parent = pCell.Transform;
		transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
	}

	// Use this for initialization
	public void Init(Cell pCell, Fire pAgent) {
		Agent = pAgent;
		transform.position = new Vector3(pCell.Transform.position.x, pCell.Transform.position.y, -0.1f);
		transform.parent = pCell.Transform;
		transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
	}

	// Update is called once per frame
	void Update() {
		// Check if the agent still exists
		if (Agent == null || Agent.Environment[Agent.Name] == null) {
			Kill();
		} else if (!Agent.IsOnDestroy && Agent.IsInit) {
			float lIntensityPercent = ((Fire)Agent).Intensity / (float)((Fire)Agent).MaxIntensity;
			transform.localScale = new Vector3(lIntensityPercent, lIntensityPercent, lIntensityPercent);
		}
	}
}
