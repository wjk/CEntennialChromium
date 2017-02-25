using System;
using System.Collections.Generic;
using System.CommandLine;
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

                // Note: System.CommandLine currently requires that I provide each parameter that is
                // common to all subcommands repeatedly, once for each command. Yes, it's stupid.
                ArgumentSyntax.Parse(args, syntax =>
                {
                    syntax.ApplicationName = "BuildChromium";

                    syntax.DefineCommand("prepare");
                    syntax.DefineParameter("sandbox-directory", ref sandboxDirectory, "The directory to clone Chromium into");

                    syntax.DefineCommand("patch");
                    syntax.DefineOption("patchset-dir", ref patchSetPath, "Directory containing the patchset files");
                    syntax.DefineOptionList("p|apply-patchset", ref patchSetsToApply, "Apply these patchsets");
                    syntax.DefineParameter("sandbox-directory", ref sandboxDirectory, "The directory to clone Chromium into");

                    syntax.DefineCommand("build");
                    syntax.DefineOption("patchset-dir", ref patchSetPath, "Directory containing the patchset files");
                    syntax.DefineOptionList("p|apply-patchset", ref patchSetsToApply, "Apply these patchsets");
                    syntax.DefineOption("x64", ref build64Bit, "Build a 64-bit binary");
                    syntax.DefineParameter("sandbox-directory", ref sandboxDirectory, "The directory to clone Chromium into");
                });
            }
            catch (Exception ex)
            {
                Log.Write(LogLevel.FatalError, "Unhandled {0} was thrown: {1}", ex.GetType().Name, ex.Message);
                Environment.Exit(1);
            }


            Console.WriteLine("Hello World!");
        }
    }
}
