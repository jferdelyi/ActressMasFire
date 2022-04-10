using ActressMasWrapper;

namespace FireMAS {

    // World environment implementation of EnvironmentMas
    public class FireMASEnvironment : EnvironmentWrapper {
        // Max intensity
        public int MaxIntensity { get; protected set; }

        // Propagation intensity
        public int PropagationIntensity { get; protected set; }

        // Grid size
        public int GridSize { get; protected set; }

        // The grid
        protected Cell[,] mGrid;

        // Constructor create shared world
        public FireMASEnvironment(int pSimulationPerSeconds, EnvironmentBehaviour pEnvironmentBehaviour, int pGridSize, int pPropagationIntensity, int pMaxIntensity)
            : base(pSimulationPerSeconds, pEnvironmentBehaviour) {
            GridSize = pGridSize;
            PropagationIntensity = pPropagationIntensity;
            MaxIntensity = pMaxIntensity;
            mGrid = new Cell[GridSize, GridSize];
        }

        // Add cell agent to the grid
        public void AddToGrid(Cell pCell, int pX, int pY) {
            mGrid[pX, pY] = pCell;
        }

        // Return cell from the grid
        public Cell GetFromGrid(int pX, int pY) {
            return mGrid[pX, pY];
        }

        // Fill the neighbourg of each cell
        public void FillNeighbour() {
            for (int lX = 0; lX < GridSize; lX++) {
                for (int lY = 0; lY < GridSize; lY++) {
                    mGrid[lX, lY].FillNeighbour();
                }
            }
        }

        // Add data to create
        public bool AddData(AgentData pAgentData) {
            return Behavior.AddData(pAgentData);
        }

        // Get data to create
        public AgentDataWrapper GetData(string pName) {
            return Behavior.GetData(pName);
        }

        // Simulation finished
        public override void SimulationFinished() { }

        // Turn finished
        public override void TurnEnd(int pTurn) { }
    }
}
