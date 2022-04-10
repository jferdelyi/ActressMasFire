using ActressMasWrapper;
using FireMAS;
using UnityEngine;

public class CellBehaviour : AgentBehaviour {
    // Exposed variables ash
    public Color ashColor;
    // Exposed variables empty
    public Color emptyColor;

    // Exposed variables forest
    public Color forestColor;
    public int forestInitLife = 1000;
    public int forestIncreasingValue = 3;
    public int forestDecreasingValue = -1;

    // Exposed variables plain
    public Color plainColor;
    public int plaintInitLife = 5000;
    public int plainIncreasingValue = 2;
    public int plaintDecreasingValue = -2;

    // Exposed variables swamp
    public Color swampColor;
    public int swampInitLife = 10000;
    public int swampIncreasingValue = 1;
    public int swampDecreasingValue = -3;

    public void Init(FireMASEnvironment pEnvironment, int pX, int pY, CellType pCellType) {
        // Load data
        if (pCellType == CellType.Forest) {
            GetComponent<SpriteRenderer>().color = forestColor;
            Agent = new ForestCell(this, pEnvironment, pX, pY, forestInitLife, forestIncreasingValue, forestDecreasingValue);

        } else if (pCellType == CellType.Plain) {
            GetComponent<SpriteRenderer>().color = plainColor;
            Agent = new PlainCell(this, pEnvironment, pX, pY, plaintInitLife, plainIncreasingValue, plaintDecreasingValue);

        } else if (pCellType == CellType.Swamp) {
            GetComponent<SpriteRenderer>().color = swampColor;
            Agent = new SwampCell(this, pEnvironment, pX, pY, swampInitLife, swampIncreasingValue, swampDecreasingValue);

        }

        // Init location
        transform.position = new Vector3(pX * GetComponent<SpriteRenderer>().bounds.size.x, pY * GetComponent<SpriteRenderer>().bounds.size.y, 0);
        transform.parent = pEnvironment.Transform;
    }

    // Update is called once per frame
    void Update() {
        if (((Cell)Agent).IsAsh) {
            // Set to ash color
            GetComponent<SpriteRenderer>().color = ashColor;
        }
    }
}
