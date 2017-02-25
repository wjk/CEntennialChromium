using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using BuildChromium.Logging;
using BuildChromium.Utilities;

namespace BuildChromium
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.AddLogger(new ConsoleLogger());

            try
            {
                bool build64Bit = false;
                string patchSetPath = null, sandboxDirectory = null;
                IReadOnlyList<string> patchSetsToApply = Array.Empty<string>();
                IReadOnlyList<string> logFiles = Array.Empty<string>();

                // Note: System.CommandLine currently requires that I provide each parameter that is
                // common to all subcommands repeatedly, once for each command. Yes, it's stupid.
                ArgumentSyntax.Parse(args, syntax =>
                {
                    syntax.ApplicationName = "BuildChromium";

                    syntax.DefineCommand("prepare");
                    syntax.DefineOptionList("logfile", ref logFiles, "Write status messages to this file");
                    syntax.DefineParameter("sandbox-directory", ref sandboxDirectory, "The directory to clone Chromium into");

                    syntax.DefineCommand("patch");
                    syntax.DefineOptionList("logfile", ref logFiles, "Write status messages to this file");
                    syntax.DefineOption("patchset-dir", ref patchSetPath, "Directory containing the patchset files");
                    syntax.DefineOptionList("p|apply-patchset", ref patchSetsToApply, "Apply these patchsets");
                    syntax.DefineParameter("sandbox-directory", ref sandboxDirectory, "The directory to clone Chromium into");

                    syntax.DefineCommand("build");
                    syntax.DefineOptionList("logfile", ref logFiles, "Write status messages to this file");
                    syntax.DefineOption("patchset-dir", ref patchSetPath, "Directory containing the patchset files");
                    syntax.DefineOptionList("p|apply-patchset", ref patchSetsToApply, "Apply these patchsets");
                    syntax.DefineOption("x64", ref build64Bit, "Build a 64-bit binary");
                    syntax.DefineParameter("sandbox-directory", ref sandboxDirectory, "The directory to clone Chromium into");
                });

                foreach (string path in logFiles)
                {
                    if (!Directory.Exists(Path.GetDirectoryName(path)))
                    {
                        Log.Write(LogLevel.Warning, "Cannot log to file {0}: Path does not exist. Ignoring this file.", Path.GetFileName(path));
                        continue;
                    }

                    if (Directory.Exists(path))
                    {
                        Log.Write(LogLevel.Warning, "Cannot log to file {0}: Is a directory. Ignoring this file.", path);
                        continue;
                    }

                    Log.AddLogger(new FileLogger(new FileInfo(path)));
                }

                throw new NotImplementedException();
            }
            catch (BuilderException ex)
            {
                Log.Write(LogLevel.FatalError, ex.Message);
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Log.Write(LogLevel.FatalError, "Unhandled {0} was thrown: {1}", ex.GetType().Name, ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
