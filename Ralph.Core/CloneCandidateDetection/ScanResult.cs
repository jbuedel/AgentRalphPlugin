using System;
using System.Collections.Generic;

namespace AgentRalph.CloneCandidateDetection
{
    [Serializable]
    public class ScanResult
    {
        private readonly QuickFixInfo[] cloneInstances;

        public ScanResult(QuickFixInfo[] cloneInstances)
        {
            this.cloneInstances = cloneInstances;
        }

        public ScanResult() : this(new QuickFixInfo[0]) { }

        public IList<QuickFixInfo> Clones
        {
            get { return cloneInstances; }
        }
    }
}