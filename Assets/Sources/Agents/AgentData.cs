namespace FireMAS {

	// Data to create
	public class AgentData {

		// The cell where the fire will be created
		public Cell Cell { get; set; }

		// The agent
		public Fire Fire { get; set; }

		// Default intensity
		public int Intensity { get; set; }

		// Constructor
		public AgentData(Cell pCell, Fire pFire = null, int pIntensity = 1) {
			Cell = pCell;
			Fire = pFire;
			Intensity = pIntensity;
		}
	}
}
