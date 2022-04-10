using ActressMasWrapper;

namespace FireMAS {

    // The agent class Fire
    public class Fire : AgentWrapper {

        // The location of the fire
        protected Cell mLocation;

        // The fire intensity
        public int Intensity { get; protected set; } = 1;

        // The fire intensity
        public int MaxIntensity { get; protected set; }

        // True if the fire is out
        public bool IsOut { get { return Intensity <= 0; } }

        // Constructor
        public Fire(AgentBehaviour pBehaviour, Cell pLocation, EnvironmentWrapper pEnvironment)
            : base(pBehaviour, pEnvironment) {
            mLocation = pLocation;
            mLocation.Fire = this;
            MaxIntensity = ((FireMASEnvironment)Environment).MaxIntensity;
        }

        // Fire intensify
        public void Intensify(int pValue) {
            Intensity += pValue;
            if (Intensity > MaxIntensity) {
                Intensity = MaxIntensity;
            } else if (Intensity <= 0) {
                Intensity = 0;
            }
        }

        // Each step
        public override void Update() {
            if (mLocation.IsAsh) {
                Intensity = 0;
            } else {
                int lBurningValue = mLocation.GetBurningValue();
                Intensify(lBurningValue);

                // If the fire increase and reach a propagation step
                if (Intensity > 0 && lBurningValue > 0 && (Intensity % ((FireMASEnvironment)Environment).PropagationIntensity) == 0) {
                    mLocation.PropagateFire();
                }
            }
        }
    }
}
