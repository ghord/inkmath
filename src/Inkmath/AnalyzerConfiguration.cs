using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inkmath
{
    public class AnalyzerConfiguration
    {
        public AnalyzerConfiguration(string seshatPath, string seshatConfig)
        {
            SeshatPath = seshatPath;
            SeshatConfig = seshatConfig;
        }

        public string SeshatPath { get; set; }
        public string SeshatConfig { get; set; }
        public OutputFormat OutputFormat { get; set; } = OutputFormat.LaTeX;

        public void ThrowIfInvalid()
        {
            if (!File.Exists(SeshatPath))
                throw new FileNotFoundException($"cannot find {Path.GetFullPath(SeshatPath)}");

            if (!File.Exists(SeshatConfig))
                throw new FileNotFoundException($"cannot find {Path.GetFullPath(SeshatConfig)}");


        }
    }
}
