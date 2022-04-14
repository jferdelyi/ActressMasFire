using System;
using System.Collections.Generic;
using ActressMas;
using ActressMasWrapper;
using UnityEngine;

namespace FireMAS {

	// Cell state
	public enum CellType { Empty, Forest, Plain, Swamp, Unknown };

	// Cell object
	public abstract class Cell : AgentWrapper {

		// Lock
		protected object mLocker = new object();

		// The PV of the cell
		protected int mLife;

		// The PV of the cell
		protected int mLifeMax;

		// The PV of the cell
		protected int mIncreasingValue;

		// The PV of the cell
		protected int mDecreasingValue;

		// The fire (can be null)
		private Fire mFire = null;
		public Fire Fire {
			get {
				lock (mLocker) {
					return mFire;
				}
			}
			set {
				lock (mLocker) {
					mFire = value;
				}
			}
		}

		// Neighbour of the fire (8)
		public List<Agent> Neighbour { get; set; } = new List<Agent>();

		// If true, there is nothing to burn...
		public bool IsAsh { get { return mLife <= 0; } }

		// If true, this cell is already burning
		public bool IsBurning { get { return Fire != null; } }

		// X
		public int X { get; protected set; }

		// Y
		public int Y { get; protected set; }

		// Constructor
		public Cell(GraphicalAgent pBehaviour, int pX, int pY, int pInitLife, int pIncreasingValue, int pDecreasingValue)
			: base(pBehaviour) {
			X = pX;
			Y = pY;
			mLife = pInitLife;
			mLifeMax = pInitLife;
			mIncreasingValue = pIncreasingValue;
			mDecreasingValue = pDecreasingValue;
		}

		// Init the cell
		public void FillNeighbour(Cell[,] pGrid, int pGridSize) {
			int lStartX = X - 1;
			int lStopX = X + 1;
			int lStartY = Y - 1;
			int lStopY = Y + 1;

			// Setup neighbour

			for (int lX = Math.Max(0, lStartX); lX <= Math.Min(pGridSize - 1, lStopX); lX++) {
				for (int lY = Math.Max(0, lStartY); lY <= Math.Min(pGridSize - 1, lStopY); lY++) {
					if (pGrid[lX, lY] != this) {
						Neighbour.Add(pGrid[lX, lY]);
					}
				}
			}
		}

		// Start the fire
		public void Ignite() {
			// Add and check if the data already exists
			if (!((FireMASEnvironment)Environment).AddGraphicalData(Name, new AgentData(this))) {

				// If already exists
				AgentData lAgentData = (AgentData)((FireMASEnvironment)Environment).GetGraphicalData(Name);

				// Check if the agent still exists
				if (Environment[lAgentData.Fire.Name] == null) {

					// If not, then delete the graphical data and re ignite
					((FireMASEnvironment)Environment).EraseData(Name);
					Ignite();
				} else {

					// If true, then intensify
					lAgentData.Fire.Intensify(1);
				}
			} else {

				// If is not already exists then create a new fire agent
				CreateFire();
			}
		}

		// Internal create fire
		protected void CreateFire() {
			Fire lFire = new(this);
			AgentData lAgentData = (AgentData)((FireMASEnvironment)Environment).GetGraphicalData(Name);
			Environment.Add(lFire, Tools.CreateName());
			lAgentData.Fire = lFire;
		}

		// Intensify the fire
		public void Intensify() {
			Debug.Assert(Fire != null);
			Fire.Intensify(1);
		}

		// Propagate the fire
		public void PropagateFire() {
			foreach (Cell lCell in Neighbour) {
				if (!lCell.IsAsh) {
					if (lCell.Fire == null) {
						lCell.Ignite();
					} else {
						lCell.Intensify();
					}
				}
			}
		}

		// Burning override
		public int GetBurningValue() {
			// If there is nothing else to burn
			if (mLife <= 0) {
				return 0;
			}

			// If mid life, the fire start to decreasing
			if (mLife > (mLifeMax / 2)) {
				return mIncreasingValue;
			} else {
				return mDecreasingValue;
			}
		}


		// Decision-Action of the agent
		public override void Action() {
			// Decrease life if there is fire
			if (IsBurning) {
				mLife -= Fire.Intensity;
			}

			// If the life is 0 then turn it to ash
			if (mLife <= 0) {
				mLife = 0;
			}
		}

		public override void Init() { }

		public override bool PerceptionFilter(Dictionary<string, string> pObserved) => false;

		public override void Perception(List<ObservableAgent> pObservableAgents) { }

		public override void Reaction(Message pMessage) { }

		public override void Teardown() { }

		// The type of the cell must redifined for each cell type
		public abstract CellType GetCellType();
	}
}
