using ActressMasWrapper;

namespace FireMAS {

    // Forest cell
    public class PlainCell : Cell {

        // Constructor
        public PlainCell(AgentBehaviour pBehaviour, FireMASEnvironment pEnvironment, int pX, int pY, int pInitLife, int pIncreasingValue, int pDecreasingValue)
            : base(pBehaviour, pEnvironment, pX, pY, pInitLife, pIncreasingValue, pDecreasingValue) {
        }

        // Cell type override
        public override CellType GetCellType() {
            return CellType.Plain;
        }
    }
}
