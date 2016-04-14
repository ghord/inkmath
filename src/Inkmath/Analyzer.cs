using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace Inkmath
{
    public class Analyzer
    {
        private AnalyzerConfiguration configuration_;

        public Analyzer(AnalyzerConfiguration configuration)
        {
            configuration_ = configuration;
            configuration.ThrowIfInvalid();
        }

        public Task<AnalyzerResult> AnalyzeAsync(CancellationToken token)
        {
            return Task.Run(() => Analyze(token));
        }

        private List<Stroke> strokes_ = new List<Stroke>();
        public void AddStrokes(IEnumerable<Stroke> strokes)
        {
            strokes_.AddRange(strokes);
        }

        private void WriteScgi(StreamWriter writer)
        {
            writer.WriteLine("SCG_INK\n");
            writer.WriteLine(strokes_.Count);
            foreach (var stroke in strokes_)
            {
                writer.WriteLine(stroke.StylusPoints.Count);

                foreach (var point in stroke.StylusPoints)
                {
                    writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} {1}", point.X, point.Y));
                }
            }
            writer.Close();
        }

        public AnalyzerResult Analyze(CancellationToken token)
        {
            var psi = new ProcessStartInfo(configuration_.SeshatPath);
            psi.CreateNoWindow = true;
            psi.Arguments = "-c " + Path.GetFileName(configuration_.SeshatConfig);
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.WorkingDirectory = Path.GetDirectoryName(configuration_.SeshatConfig);
            var process = Process.Start(psi);
            //process.BeginOutputReadLine();

            using (var writer = process.StandardInput)
            {
                WriteScgi(writer);
                //write csgi to standard input
                using (var file = File.OpenWrite("test.scgink"))
                using (var test = new StreamWriter(file))
                {
                    WriteScgi(test);
                }
            }

            while (!process.WaitForExit(100))
            {
                if (token.IsCancellationRequested)
                {
                    process.Kill();
                }
            }

            if (token.IsCancellationRequested)
            {
                return AnalyzerResult.Cancelled;
            }
            else
            {
                var code = process.ExitCode;

                if (code != 0)
                {
                    return AnalyzerResult.FromError(process.StandardError.ReadToEnd());
                }
                else
                {
                    return AnalyzerResult.FromExpression(process.StandardOutput.ReadToEnd(), OutputFormat.LaTeX);
                }
            }
        }
    }
}
