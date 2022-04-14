using ActressMasWrapper;
using UnityEngine;

namespace FireMAS {

	// World environment implementation of EnvironmentMas
	public class FireMASEnvironment : EnvironmentWrapper {

		// Constructor create shared world
		public FireMASEnvironment(int pSimulationPerSeconds, GraphicalEnvironment pGraphicalEnvironment, bool pParallel)
			: base(pSimulationPerSeconds, pGraphicalEnvironment, pParallel : pParallel) {
		}

		// Simulation finished
		public override void SimulationFinished() {
			Debug.Log("Simulation finished");
			int lAshCount = 0;
			int lForestCount = 0;
			int lPlainCount = 0;
			int lSwampCount = 0;
			int lFireCount = 0;
			int lGlobalCount = 0;

			foreach (var lAgent in Agents) {
				if (lAgent is Cell cell) {
					if (cell.IsAsh) {
						lAshCount++;
					} else if (lAgent is ForestCell) {
						lForestCount++;
					} else if (lAgent is PlainCell) {
						lPlainCount++;
					} else if (lAgent is SwampCell) {
						lSwampCount++;
					}
				} else if (lAgent is Fire) {
					lFireCount++;
				}
				lGlobalCount++;
			}
			Debug.Log("Ash count: " + lAshCount);
			Debug.Log("Forest count: " + lForestCount);
			Debug.Log("Plain count: " + lPlainCount);
			Debug.Log("Swamp count: " + lSwampCount);
			Debug.Log("Fire count: " + lFireCount);
			Debug.Log("Global count: " + lGlobalCount);
		}

		// Turn finished
		public override void TurnFinished(int pTurn) {

		}
	}
}
