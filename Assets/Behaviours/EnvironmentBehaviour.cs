using UnityEngine;
using FireMAS;
using ActressMasWrapper;

// The graphic environment behaviour
public class EnvironmentBehaviour : GraphicalEnvironment {
	// Exposed variables cell
	public GameObject cellPrefab;
	// Exposed variables fire
	public GameObject firePrefab;

	public int simulationPerSeconds = 60;
	// Exposed variables forest count
	public int forestCount = 1500;
	// Exposed variables swamp count
	public int swampCount = 500;
	// Exposed variables starting fire count
	public int fireCount = 1;
	// Exposed variables size of the environment
	public int gridSize = 50; // 50 * 50 = 2500 -> default is plain

	// The clamp for propagation
	public int propagationIntensity = 2;
	// Max fire intensity
	public int maxIntensity = 10;

	// Parallel or sequential ?
	public bool parallel = false;

	// Origine
	private int mOrigineX;
	private int mOrigineY;

	/*************************************************************************/

	// Start is called before the first frame update
	void Start() {
		Environment = new FireMASEnvironment(simulationPerSeconds, this, parallel);
		Cell[,] lWorld = new Cell[gridSize, gridSize];
		mOrigineX = (int)transform.position.x;
		mOrigineY = (int)transform.position.y;

		int lCellsCount = gridSize * gridSize;
		int[] lRandVect = Environment.RandomArray(lCellsCount);

		// Create agents Forest
		for (int lI = 0; lI < forestCount; lI++) {
			int lX = lRandVect[lI] / gridSize;
			int lY = lRandVect[lI] % gridSize;
			InitCell(lWorld, lX, lY, CellType.Forest);
		}

		// Create agents Swamp
		for (int lI = forestCount; lI < forestCount + swampCount; lI++) {
			int lX = lRandVect[lI] / gridSize;
			int lY = lRandVect[lI] % gridSize;
			InitCell(lWorld, lX, lY, CellType.Swamp);
		}

		// Set agent Plain
		for (int lI = forestCount + swampCount; lI < gridSize * gridSize; lI++) {
			int lX = lRandVect[lI] / gridSize;
			int lY = lRandVect[lI] % gridSize;
			InitCell(lWorld, lX, lY, CellType.Plain);
		}

		// New permutation
		lRandVect = Environment.RandomArray(lCellsCount);

		// Create agents Fire
		for (int lI = 0; lI < fireCount; lI++) {
			int lX = lRandVect[lI] / gridSize;
			int lY = lRandVect[lI] % gridSize;
			InitFire(lWorld[lX, lY]);
		}

		// Set shared values
		Environment.SetMemory("maxIntensity", maxIntensity);
		Environment.SetMemory("propagationIntensity", propagationIntensity);

		FillNeighbour(lWorld);
	}

	/*************************************************************************/

	// Fill the neighbourg of each cell
	private void FillNeighbour(Cell[,] pWorld) {
		for (int lX = 0; lX < gridSize; lX++) {
			for (int lY = 0; lY < gridSize; lY++) {
				pWorld[lX, lY].FillNeighbour(pWorld, gridSize);
			}
		}
	}

	// Create new cell
	private void InitCell(Cell[,] pWorld, int pX, int pY, CellType pCellType) {
		// Instanciate and init cell
		GameObject lCell = Instantiate(cellPrefab);
		CellBehaviour lCellBehaviour = lCell.GetComponent<CellBehaviour>();
		lCellBehaviour.Init((FireMASEnvironment)Environment, mOrigineX, mOrigineY, pX, pY, pCellType);
		pWorld[pX, pY] = (Cell)lCellBehaviour.Agent;
		Environment.Add(lCellBehaviour.Agent, Tools.CreateName());
	}

	// Create new fire
	private void InitFire(Cell pCell) {
		GameObject lFire = Instantiate(firePrefab);
		FireBehaviour lFireBehaviour = lFire.GetComponent<FireBehaviour>();
		lFireBehaviour.Init(pCell);
		Environment.Add(lFireBehaviour.Agent, Tools.CreateName());
	}

	// Create new fire 
	private void CreateFire(AgentData pData) {
		GameObject lFire = Instantiate(firePrefab);
		FireBehaviour lFireBehaviour = lFire.GetComponent<FireBehaviour>();
		lFireBehaviour.Init(pData.Cell, pData.Fire);
	}

	/*************************************************************************/

	// Check new data
	protected override void CheckNewData() {
		// Create new fire
		foreach (var lFireData in mAgentDataWrapper) {
			CreateFire((AgentData)lFireData.Value);
		}
		mAgentDataWrapper.Clear();
	}
}
