using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.ProjectModel;
using JetBrains.Util;

namespace AgentRalph
{
    public class SimianProjectVisitor : RecursiveProjectVisitor
    {
        private readonly IList<FileSystemPath> Files;
        private readonly List<Regex> SpecsToIgnore = new List<Regex>();
        private readonly List<Regex> SpecsToInclude = new List<Regex>();

        public SimianProjectVisitor(IList<FileSystemPath> files)
        {
            Files = files;
        }

        public override void VisitProjectFile(IProjectFile projectFile)
        {
            // Base class handles the recursion.  As long as you call on it.
            base.VisitProjectFile(projectFile);

            // I really need a way to exclude binary files.  Or only include text files.
            // The GetGeneratedByToolProperty() seems to take care of some of the binary files.
            // From it's name alone you'd think projectFile.IsTextSearchable() would work.  
            // But it returns false for everything.
            if (projectFile.Kind == ProjectItemKind.PHYSICAL_FILE /*&& projectFile.GetGeneratedByToolProperty() == false*/)
            {
                if (IsIncludedFile(projectFile) && !IsIgnoredFile(projectFile))
                {
                    // Add any files we want to analyze
                    // Sometimes they were getting added twice.  Not sure why exactly.
                    if(!Files.Contains(projectFile.Location))
                        Files.Add(projectFile.Location);
                }
            }
        }

        private bool IsIncludedFile(IProjectFile projectFile)
        {
            foreach (Regex spec in SpecsToInclude)
            {
                if(spec.IsMatch(projectFile.Location.Name))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsIgnoredFile(IProjectFile projectFile)
        {
            foreach (Regex spec in SpecsToIgnore)
            {
                if (spec.IsMatch(projectFile.Location.Name))
                {
                    return true;
                }
            }
            return false;
        }

        public void AddIgnoreSpec(string fileSpec)
        {
            fileSpec = MakeWildcardStringToRegex(fileSpec);
            SpecsToIgnore.Add(new Regex(fileSpec));
        }

        private string MakeWildcardStringToRegex(string fileSpec) 
        {
            return '^' + fileSpec.Replace(".", @"\.").Replace("*", ".*").Replace('?', '.') + "$";
        }

        public void AddIncludeSpec(string fileSpec)
        {
            SpecsToInclude.Add(new Regex(MakeWildcardStringToRegex(fileSpec), RegexOptions.IgnoreCase));
        }

        public void AddIncludeSpecs(string[] includeSpecs)
        {
            foreach (string spec in includeSpecs)
            {
                AddIncludeSpec(spec);
            }
        }

        public void AddExcludeSpecs(string[] excludeSpecs)
        {
            foreach (string spec in excludeSpecs)
            {
                AddIgnoreSpec(spec);
            }
        }
    }
}