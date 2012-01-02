using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AgentRalph.Options;
using JetBrains.Util;

namespace AgentRalph
{
    internal class SimianUtil
    {
        public static string RunSimianToXml(IEnumerable<FileSystemPath> files_to_analyze, SimianOptions simian_opts)
        {
            string output_file_name = Path.GetTempFileName();
            string config_file_name = Path.GetTempFileName();
            try
            {
                string simian_args = string.Format("\"-formatter=xml:{0}\" \"-config={1}\"", output_file_name, config_file_name);

                Debug.WriteLine("simian config file name is :" + config_file_name);

                // Put all command line params into a config file else the command line would be too long.
                using (TextWriter w = new StreamWriter(File.OpenWrite(config_file_name)))
                {
                    // Write out all the options.
                    foreach (SimianBooleanOption option in simian_opts.CommandLineOptions)
                    {
                        if(option.Value)
                        {
                            w.WriteLine("-" + option.Name);
                        }
                    }

                    // And put each file name in
                    foreach (FileSystemPath file in files_to_analyze)
                    {
                        w.WriteLine(file.FullPath);
                    }
                }

                ProcessStartInfo info =
                    new ProcessStartInfo(simian_opts.PathToSimian, simian_args) {UseShellExecute = false, RedirectStandardError = true, RedirectStandardOutput = true, CreateNoWindow = true};

                Process simian_process = Process.Start(info);
                while (simian_process.WaitForExit(1000) == false)
                {
                }

                string std_out = simian_process.StandardOutput.ReadToEnd();
                string std_err = simian_process.StandardError.ReadToEnd();
                Debug.WriteLine(std_out);
                Debug.WriteLine(std_err);
            }
            finally
            {
                if (File.Exists(config_file_name))
                    File.Delete(config_file_name);
            }
            return output_file_name;
        }
    }
}