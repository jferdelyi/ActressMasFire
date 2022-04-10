using ActressMasWrapper;
using FireMAS;
using UnityEngine;

public class FireBehaviour : AgentBehaviour {

    // Use this for initialization
    public void Init(FireMASEnvironment pEnvironment, Cell pCell) {
        transform.position = new Vector3(pCell.Transform.position.x, pCell.Transform.position.y, -0.1f);
        transform.parent = pCell.Transform;
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        Agent = new Fire(this, pCell, pEnvironment);
    }

    // Update is called once per frame
    void Update() {

        float lIntencityPercent = ((Fire)Agent).Intensity / (float)((Fire)Agent).MaxIntensity;
        transform.localScale = new Vector3(lIntencityPercent, lIntencityPercent, lIntencityPercent);

        if (((Fire)Agent).IsOut) {
            Destroy(gameObject);
            Agent.Environment.Remove(Agent);
        }
    }
}
