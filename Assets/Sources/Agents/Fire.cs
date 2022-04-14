using System.Collections.Generic;
using ActressMas;
using ActressMasWrapper;

namespace FireMAS {

	// The agent class Fire
	public class Fire : AgentWrapper {

		// The fire intensity
		public int Intensity { get; protected set; } = 1;

		// If true then the fire is already propagate
		public int MaxIntensity { get; protected set; }

		// The location of the fire
		protected Cell mLocation;

		// If true then the fire is already propagate
		protected bool mPropagated = false;

		// Propagation intensity where the fire increase
		protected int mPropagationIntensity;

		// Constructor
		public Fire(Cell pLocation, GraphicalAgent pBehaviour = null)
			: base(pBehaviour) {
			mLocation = pLocation;
			mLocation.Fire = this;
		}

		// Fire intensify
		public void Intensify(int pValue) {
			Intensity += pValue;
			if (Intensity > MaxIntensity) {
				Intensity = MaxIntensity;
			} else if (Intensity <= 0) {
				Teardown();
			}
		}

		// Decision-Action logic of agent
		public override void Action() {
			if (mLocation.IsAsh) {
				Teardown();
			} else {
				int lBurningValue = mLocation.GetBurningValue();
				Intensify(lBurningValue);

				// If the fire increase and reach a propagation step
				if (!mPropagated && (Intensity > 0) && lBurningValue > 0 && (Intensity >= mPropagationIntensity)) {
					mLocation.PropagateFire();
					mPropagated = true;
				}
			}
		}

		// Teardown
		public override void Teardown() {
			Intensity = 0;
			mLocation.Fire = null;
			Kill();
		}

		// Init the agent
		public override void Init() {
			MaxIntensity = Environment.GetMemory("maxIntensity");
			mPropagationIntensity = Environment.GetMemory("propagationIntensity");
		}

		public override bool PerceptionFilter(Dictionary<string, string> pObserved) => false;

		public override void Perception(List<ObservableAgent> pObservableAgents) { }

		public override void Reaction(Message pMessage) { }
	}
}
