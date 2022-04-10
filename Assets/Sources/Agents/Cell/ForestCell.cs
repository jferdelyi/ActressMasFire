using ActressMasWrapper;

namespace FireMAS {

    // Forest cell
    public class ForestCell : Cell {

        // Constructor
        public ForestCell(AgentBehaviour pBehaviour, FireMASEnvironment pEnvironment, int pX, int pY, int pInitLife, int pIncreasingValue, int pDecreasingValue)
            : base(pBehaviour, pEnvironment, pX, pY, pInitLife, pIncreasingValue, pDecreasingValue) {
        }

        // Cell type override
        public override CellType GetCellType() {
            return CellType.Forest;
        }
    }
}
