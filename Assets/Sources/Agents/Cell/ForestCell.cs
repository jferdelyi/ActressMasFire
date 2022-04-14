using ActressMasWrapper;

namespace FireMAS {

	// Forest cell
	public class ForestCell : Cell {

		// Constructor
		public ForestCell(GraphicalAgent pBehaviour, int pX, int pY, int pInitLife, int pIncreasingValue, int pDecreasingValue)
			: base(pBehaviour, pX, pY, pInitLife, pIncreasingValue, pDecreasingValue) {
		}

		// Cell type override
		public override CellType GetCellType() {
			return CellType.Forest;
		}
	}
}
