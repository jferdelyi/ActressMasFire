using UnityEngine;
using FireMAS;
using ActressMasWrapper;

// The graphic environment behaviour
public class EnvironmentBehaviour : ActressMasWrapper.EnvironmentBehaviour {
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
    // Each step of PropagationIntensity the fire propagate
    public int propagationIntensity = 2;
    // Max fire intensity
    public int maxIntensity = 10;

    /*************************************************************************/

    // Start is called before the first frame update
    void Start() {
        Environment = new FireMASEnvironment(simulationPerSeconds, this, gridSize, propagationIntensity, maxIntensity);

        // Inner init
        Init();
    }

    // Each ticks
    void Update() {
        // Inner update
        RunTurn();
    }

    /*************************************************************************/

    // Create new cell
    private void CreateCell(int lX, int lY, CellType pCellType) {
        // Instanciate and init cell
        GameObject lCell = Instantiate(cellPrefab);
        CellBehaviour lCellBehaviour = lCell.GetComponent<CellBehaviour>();
        lCellBehaviour.Init(((FireMASEnvironment)Environment), lX, lY, pCellType);
        ((FireMASEnvironment)Environment).AddToGrid((Cell)lCellBehaviour.Agent, lX, lY);
        Environment.Add(lCellBehaviour.Agent, Tools.CreateName());
    }

    // Create new fire
    private void CreateFire(Cell mCell) {
        if (mCell.Fire != null) {
            mCell.Fire.Intensify(propagationIntensity / 2);
        } else {
            // Instanciate and init fire
            GameObject lFire = Instantiate(firePrefab);
            FireBehaviour lFireBehaviour = lFire.GetComponent<FireBehaviour>();
            lFireBehaviour.Init(((FireMASEnvironment)Environment), mCell);
            Environment.Add(lFireBehaviour.Agent, Tools.CreateName());
        }
    }

    /*************************************************************************/

    // Init the environment
    protected override void InitEnvironment() {
        int lCellsCount = gridSize * gridSize;
        int[] lRandVect = Tools.RandomPermutation(lCellsCount);

        // Create agents Forest
        for (int lI = 0; lI < forestCount; lI++) {
            int lX = lRandVect[lI] / gridSize;
            int lY = lRandVect[lI] % gridSize;
            CreateCell(lX, lY, CellType.Forest);
        }

        // Create agents Swamp
        for (int lI = forestCount; lI < forestCount + swampCount; lI++) {
            int lX = lRandVect[lI] / gridSize;
            int lY = lRandVect[lI] % gridSize;
            CreateCell(lX, lY, CellType.Swamp);
        }

        // Set agent Plain
        for (int lI = forestCount + swampCount; lI < gridSize * gridSize; lI++) {
            int lX = lRandVect[lI] / gridSize;
            int lY = lRandVect[lI] % gridSize;
            CreateCell(lX, lY, CellType.Plain);
        }

        // New permutation
        lRandVect = Tools.RandomPermutation(lCellsCount);

        // Create agents Fire
        for (int lI = 0; lI < fireCount; lI++) {
            int lX = lRandVect[lI] / gridSize;
            int lY = lRandVect[lI] % gridSize;
            CreateFire(((FireMASEnvironment)Environment).GetFromGrid(lX, lY));
        }

        ((FireMASEnvironment)Environment).FillNeighbour();
    }

    // Check new data
    protected override void CheckNewData() {
        // Create new fire
        foreach (var lFireData in mAgentDataWrapper) {
            CreateFire(((AgentData)lFireData.Value).Cell);
        }
        mAgentDataWrapper.Clear();
    }
}
