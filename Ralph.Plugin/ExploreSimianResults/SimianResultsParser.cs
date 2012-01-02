using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using ICSharpCode.NRefactory;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;
using JetBrains.Util.dataStructures.TypedIntrinsics;

namespace AgentRalph
{
    internal class SimianResultsParser
    {
        private readonly ISolution solution;
        private XmlReader xr;

        public SimianResultsParser(ISolution solution)
        {
            this.solution = solution;
        }

        public IEnumerable<Set> GetSimianResults2(string output_file)
        {
            using (FileStream output_stream = File.OpenRead(output_file))
            {
                // Hopefully these settings will let use parse faster.
                XmlReaderSettings settings = new XmlReaderSettings {IgnoreComments = true, IgnoreProcessingInstructions = true, IgnoreWhitespace = true};

                xr = XmlReader.Create(output_stream, settings);
                while (xr.Read())
                {
                    if (IsSet())
                    {
                        yield return ReadSet();
                    }
                }
            }
        }

        private bool IsSet()
        {
            return xr.Name == "set" && xr.NodeType == XmlNodeType.Element;
        }

        private Set ReadSet()
        {
            int lineCount = Convert.ToInt32(xr.GetAttribute("lineCount"));

            Set set = new Set(lineCount);

            if (xr.ReadToDescendant("block"))
            {
                do
                {
                    string sourceFile = xr.GetAttribute("sourceFile");
                    int startLineNumber = Convert.ToInt32(xr.GetAttribute("startLineNumber"));
                    int endLineNumber = Convert.ToInt32(xr.GetAttribute("endLineNumber"));

                    IList<IProjectItem> projectFiles = solution.FindProjectItemsByLocation(new FileSystemPath(sourceFile)).ToArray();
                    if (projectFiles.Count > 1)
                        throw new ApplicationException("Expected exactly one file corresponding to " + sourceFile);

                    IProjectFile projectFile = (IProjectFile) projectFiles[0];
                    DocumentRange? range = GetDocumentRange2(sourceFile, projectFile, startLineNumber, endLineNumber);
                    set.AddBlock(sourceFile, startLineNumber, endLineNumber, projectFile, range);


                } while (xr.ReadToNextSibling("block"));
            }

            return set;
        }

        public readonly IList<IProjectFile> FilesInError = new List<IProjectFile>();

        private DocumentRange? GetDocumentRange2(string sourceFile, IProjectFile projectFile, int startLineNumber, int endLineNumber)
        {
            try
            {
                return GetDocumentRange(projectFile, startLineNumber, endLineNumber, solution);
            }
            catch (ArgumentOutOfRangeException)
            {
                FilesInError.Add(projectFile);
                return null;
            }
        }

        public static DocumentRange GetDocumentRange(IProjectFile projectFile, int startLineNumber, int endLineNumber, ISolution solution)
        {projectFile.GetDocument().
            if (startLineNumber > endLineNumber)
                throw new ArgumentException("start line must come before end line.");

            DocumentManager documentManager = DocumentManager.GetInstance(solution);
            IDocument document = documentManager.GetProjectFile(projectFile);
            
            int startOffset = document.GetLineStartOffset((Int32<DocLine>) (startLineNumber-1));
            int endOffset = document.GetLineEndOffsetNoLineBreak((Int32<DocLine>) (endLineNumber-1));

            return new DocumentRange(document, new TextRange(startOffset, endOffset));
        }

        public static DocumentRange GetDocumentRange(IPsiSourceFile projectFile, Location startLocation, int nameLength, ISolution solution)
        {
            DocumentManager documentManager = DocumentManager.GetInstance(solution);
            IDocument document = documentManager.GetProjectFile(projectFile);
            int startOffset = document.GetLineStartOffset((Int32<DocLine>) (startLocation.Line - 1)) + startLocation.Column;
            int endOffset = startOffset + nameLength;

            return new DocumentRange(document, new TextRange(startOffset, endOffset));
        }
    }
}