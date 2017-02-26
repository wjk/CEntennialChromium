using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BuildChromium.Logging;
using BuildChromium.Utilities;

namespace BuildChromium
{
    public abstract class Builder
    {
        // This contains a list of directories to prepend to the PATH of any executed commands.
        public List<string> PathOverrides { get; } = new List<string>();
        public List<string> TargetsToBuild { get; } = new List<string> { "chrome" };

        public DirectoryInfo BuildRoot { get; set; } = null;
        public DirectoryInfo ResourcesRoot { get; set; } = null;

        public bool ForceRedownload { get; set; } = false;
        public bool RunSourceCleaner { get; set; } = false;
        public bool RunDomainSubstitution { get; set; } = true;
        public string NinjaCommand { get; set; } = "ninja";
        public string OutputDirectory { get; set; } = Path.Combine("out", "Default");

        private readonly SourceVersion SourceVersion;
        private DirectoryInfo BuildSandbox, DownloadsDirectory, PathOverridesDirectory;

        public Builder(string chromiumVersion, string releaseRevision)
        {
            SourceVersion = new SourceVersion();
            SourceVersion.ChromiumVersion = chromiumVersion;
            SourceVersion.ReleaseRevision = releaseRevision;
        }

        public Builder(FileInfo versionXmlFile)
        {
            SourceVersion = SourceVersion.ParseXml(versionXmlFile);
        }

        public void CreateDirectories()
        {
            if (!BuildRoot.Exists) BuildRoot.Create();

            BuildSandbox = BuildRoot.CreateSubdirectory("sandbox");
            DownloadsDirectory = BuildRoot.CreateSubdirectory("Downloads");
            PathOverridesDirectory = BuildRoot.CreateSubdirectory("PathOverrides");

            PathOverrides.Add(PathOverridesDirectory.FullName);
        }

        protected virtual HashSet<DirectoryInfo> ResourcePaths
        {
            get
            {
                if (ResourcesRoot == null) throw new InvalidOperationException($"{nameof(ResourcesRoot)} must be set");
                HashSet<DirectoryInfo> paths = new HashSet<DirectoryInfo>();
                paths.Add(ResourcesRoot.SafeCreateSubdirectory(Path.Combine("resources", "common")));
                return paths;
            }
        }
        
        protected Process RunSubprocess(ProcessStartInfo startInfo)
        {
            if (!startInfo.Environment.ContainsKey("PATH"))
                startInfo.Environment.Add("PATH", Environment.GetEnvironmentVariable("PATH"));

            startInfo.Environment["PATH"] = string.Join(Path.PathSeparator.ToString(), PathOverrides) + Path.PathSeparator + startInfo.Environment["PATH"];
            return Process.Start(startInfo);
        }

        protected virtual void WritePathOverride(string filename, IReadOnlyList<string> newCommandLine)
        {
            if (newCommandLine[0] == filename) throw new InvalidOperationException($"PATH override command {filename} can recursively execute itself");

            if (File.Exists(Path.Combine(PathOverridesDirectory.FullName, filename))) Log.Write(LogLevel.Warning, "Overwriting existing PATH override {0}", filename);
            FileInfo pathOverride = PathOverridesDirectory.CreateFile(filename);

            string[] lines = new[]
            {
                "#!/bin/bash",
                "exec " + string.Join(" ", newCommandLine.Select(x => "\"" + x + "\"")) + " \"$@\""
            };
            File.WriteAllLines(pathOverride.FullName, lines);
            if (!BuildUtility.Chmod(pathOverride, 0755)) throw new BuilderException($"Could not chmod {pathOverride.FullName}");
        }
    }
}
