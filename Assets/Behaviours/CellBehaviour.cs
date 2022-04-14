using ActressMasWrapper;
using FireMAS;
using UnityEngine;

public class CellBehaviour : GraphicalAgent {
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

	// Handler
	protected SpriteRenderer mSpriteRenderer;

	public void Init(FireMASEnvironment pEnvironment, int pOffsetX, int pOffsetY, int pX, int pY, CellType pCellType) {
		mSpriteRenderer = GetComponent<SpriteRenderer>();

		// Load data
		if (pCellType == CellType.Forest) {
			mSpriteRenderer.color = forestColor;
			Agent = new ForestCell(this, pX, pY, forestInitLife, forestIncreasingValue, forestDecreasingValue);

		} else if (pCellType == CellType.Plain) {
			mSpriteRenderer.color = plainColor;
			Agent = new PlainCell(this, pX, pY, plaintInitLife, plainIncreasingValue, plaintDecreasingValue);

		} else if (pCellType == CellType.Swamp) {
			mSpriteRenderer.color = swampColor;
			Agent = new SwampCell(this, pX, pY, swampInitLife, swampIncreasingValue, swampDecreasingValue);

		}

		// Init location
		transform.position = new Vector3(pOffsetX + (pX * mSpriteRenderer.bounds.size.x), pOffsetY + (pY * mSpriteRenderer.bounds.size.y), 0);
		transform.parent = pEnvironment.Graphical.transform;
	}

	// Update is called once per frame
	void Update() {
		if (Agent != null && ((Cell)Agent).IsAsh) {
			// Set to ash color
			mSpriteRenderer.color = ashColor;
		}
	}
}
