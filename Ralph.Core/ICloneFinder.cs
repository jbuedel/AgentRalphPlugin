using AgentRalph.CloneCandidateDetection;

namespace AgentRalph
{
    public interface ICloneFinder
    {
        void FileUpdate(string fileId, string codeText);
        ScanResult GetCloneReplacements(string fileId);
    }
}