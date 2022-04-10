using ActressMasWrapper;

namespace FireMAS {

    // Data to create
    public class AgentData : AgentDataWrapper {

        // The cell where the fire will be created
        public Cell Cell { get; protected set; }

        // Default intensity
        public int Intensity { get; set; }

        // Constructor
        public AgentData(string pName, Cell pCell) {
            Name = pName;
            Cell = pCell;
            Intensity = 1;
        }
    }
}
