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

        // The PV of the cell
        protected int mLife;

        // The PV of the cell
        protected int mLifeMax;

        // The PV of the cell
        protected int mIncreasingValue;

        // The PV of the cell
        protected int mDecreasingValue;

        // The fire (can be null)
        public Fire Fire { get; set; }

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
        public Cell(AgentBehaviour pBehaviour, EnvironmentWrapper pEnvironment, int pX, int pY, int pInitLife, int pIncreasingValue, int pDecreasingValue)
            : base(pBehaviour, pEnvironment) {
            X = pX;
            Y = pY;
            mLife = pInitLife;
            mLifeMax = pInitLife;
            mIncreasingValue = pIncreasingValue;
            mDecreasingValue = pDecreasingValue;
        }

        // Init the cell
        public void FillNeighbour() {
            int lStartX = X - 1;
            int lStopX = X + 1;
            int lStartY = Y - 1;
            int lStopY = Y + 1;

            // Setup neighbour
            for (int lX = Math.Max(0, lStartX); lX <= Math.Min(((FireMASEnvironment)Environment).GridSize - 1, lStopX); lX++) {
                for (int lY = Math.Max(0, lStartY); lY <= Math.Min(((FireMASEnvironment)Environment).GridSize - 1, lStopY); lY++) {
                    if (((FireMASEnvironment)Environment).GetFromGrid(lX, lY) != this) {
                        Neighbour.Add(((FireMASEnvironment)Environment).GetFromGrid(lX, lY));
                    }
                }
            }
        }

        // Start the fire
        public void Ignite() {
            Debug.Assert(Fire == null);
            if (!((FireMASEnvironment)Environment).AddData(new AgentData(Name, this))) {
                AgentData lAgentData = (AgentData)((FireMASEnvironment)Environment).GetData(Name);
                lAgentData.Intensity++;
            }
        }

        // Intensify the fire
        public void Intensify() {
            Debug.Assert(Fire != null);
            Fire.Intensify(((FireMASEnvironment)Environment).PropagationIntensity / 2);
        }

        // Propagate the fire
        public void PropagateFire() {
            foreach (Cell lCell in Neighbour) {
                if (!lCell.IsAsh) {
                    if (lCell.Fire == null) {
                        lCell.Ignite();
                    } else if (!lCell.Fire.IsOut) {
                        lCell.Intensify();
                    }
                }
            }
        }

        // Each step
        public override void Update() {
            // Decrease life if there is fire
            if (IsBurning) {
                mLife -= Fire.Intensity;
            }

            // If the life is 0 then turn it to ash
            if (mLife <= 0) {
                mLife = 0;
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

        // The type of the cell must redifined for each cell type
        public abstract CellType GetCellType();
    }
}
