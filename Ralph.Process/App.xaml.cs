using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Windows;
using AgentRalph;
using AgentRalph.CloneCandidateDetection;

namespace Ralph.Process
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var channel = new IpcChannel("AgentRalphIpcChannel");
            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(typeof(CloneFinderService), "CloneFinder", WellKnownObjectMode.Singleton);
        }
    }

    public class CloneFinderService : MarshalByRefObject, ICloneFinder
    {
        readonly Dictionary<string,CloneFileInstance> Files = new Dictionary<string, CloneFileInstance>();
        
        public void FileUpdate(string fileId, string codeText)
        {
            if (!Files.ContainsKey(fileId))
            {
                Files.Add(fileId, new CloneFileInstance());
            }

            var file = Files[fileId];
            if (file.CodeText != codeText)
            {
                file.UpdateCodeText(codeText);
            }
        }

        public ScanResult GetCloneReplacements(string fileId)
        {
            if (Files.ContainsKey(fileId))
            {
                var file = Files[fileId];

                if (file.Completed)
                {
                    return file.GetResult();
                }
            }
            return null;
        }
    }

    internal class CloneFileInstance
    {
        public string CodeText;
        private ScanResult scanResult;
        private CloneFinderJob Job;

        public bool Completed
        {
            get { return scanResult != null; }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCodeText(string codeText)
        {
            if (Job != null)
            {
                Job.Terminate();
                scanResult = null; // ditch any previous results.

                // Abandon to the garbage collector.
                Job = null;
            }

            CodeText = codeText;

            Job = new CloneFinderJob(codeText);
            Job.JobDone += OnJobDone;

            Job.Start();
        }

        private void OnJobDone(object sender, ScanResult args)
        {
            Console.WriteLine("Job done.  Found " + args.Clones.Count + " cloens.");

            scanResult = args;
        }

        public ScanResult GetResult()
        {
            return scanResult;
        }
    }

    internal class CloneFinderJob
    {
        internal delegate void JobDoneEvent(object sender, ScanResult args);

        private readonly string CodeText;
        private MethodsOnASingleClassCloneFinder CloneFinder;
        public event JobDoneEvent JobDone;

        private void InvokeJobDone(ScanResult args)
        {
            JobDoneEvent @event = JobDone;
            if (@event != null) @event(this, args);
        }

        public CloneFinderJob(string codeText)
        {
            CodeText = codeText;
        }

        private delegate ScanResult DoWorkDelegate(string codeText);
        private ScanResult FindThemClones(string codeText)
        {
            CloneFinder = new MethodsOnASingleClassCloneFinder(new OscillatingExtractMethodExpansionFactory());
            CloneFinder.AddRefactoring(new LiteralToParameterExpansion());

            return CloneFinder.GetCloneReplacements(codeText);
        }
        public void Start()
        {
            Console.WriteLine("Started.");
            DoWorkDelegate d = FindThemClones;
            d.BeginInvoke(CodeText, WhenDoneCallback, d);
        }

        private void WhenDoneCallback(IAsyncResult ar)
        {
            DoWorkDelegate d = (DoWorkDelegate) ar.AsyncState;
            ScanResult scanResult = d.EndInvoke(ar);

            InvokeJobDone(scanResult);
        }
        
        public void Terminate()
        {
           Console.WriteLine("Terminated!");
        }
    }
}
