using System.IO;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.Util;

namespace AgentRalph.ExploreSimianResults
{
    public class Block : IBlock
    {
        public DocumentRange? Range { get; private set; }
        private readonly int endLineNumber;
        private readonly IProjectFile projectFile;
        private readonly int startLineNumber;
        private readonly Set set;
        private TextRange textRange;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startLineNumber">Indexed from 1</param>
        /// <param name="endLineNumber">Indexed from 1</param>
        /// <param name="projectFile"></param>
        /// <param name="set"></param>
        /// <param name="range"></param>
        public Block(int startLineNumber, int endLineNumber, IProjectFile projectFile, Set set, DocumentRange? range)
        {
            Range = range;
            this.set = set;
            this.projectFile = projectFile;
            this.startLineNumber = startLineNumber;
            this.endLineNumber = endLineNumber;
        }

        public int StartLineNumber
        {
            get { return startLineNumber; }
        }

        public int EndLineNumber
        {
            get { return endLineNumber; }
        }

        public IProjectFile ProjectFile
        {
            get { return projectFile; }
        }

        public TextRange TextRange
        {
            get
            {
                

                using (TextReader tr = new StreamReader(ProjectFile.CreateReadStream()))
                {
                    int position = 0;
                    int char_count = 0;
                    string line;
                    while (position++ < StartLineNumber && (line = tr.ReadLine()) != null)
                    {
                        char_count += line.Length + 1;
                    }
                    textRange = new TextRange(char_count);
                }

                return textRange;
            }
        }

        public Set Set
        {
            get { return set; }
        }

        public string Text
        {
            get
            {
                string text = "";
                using (TextReader tr = new StreamReader(ProjectFile.CreateReadStream()))
                {
                    string line;
                    int cur_line_num = 0;
                    while((line = tr.ReadLine()) != null)
                    {
                        cur_line_num++;
                        if(cur_line_num > EndLineNumber)
                        {
                            return text;
                        }
                        
                        if(cur_line_num >= StartLineNumber)
                        {
                            text += line;
                        }
                    }
                }
                return text;
            }
        }

        public override string ToString()
        {
            return MakeBlockTitle();
        }

        private string MakeBlockTitle()
        {
            return "(" + StartLineNumber + "-" + EndLineNumber + ")\t" + ProjectFile.GetPresentableProjectPath();
        }

        public int DistanceTo(IBlock block)
        {
            if (block.StartLineNumber > StartLineNumber)
                return block.StartLineNumber - EndLineNumber;
            return StartLineNumber - block.EndLineNumber;
        }

        public bool SharesSourceFile(IBlock block)
        {
            return block.ProjectFile == this.projectFile;
//            return SourceFile == block.SourceFile;
        }
    }
}