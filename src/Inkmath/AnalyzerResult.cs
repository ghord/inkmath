using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inkmath
{
    public class AnalyzerResult
    {
        private AnalyzerResult()
        {

        }

        public static AnalyzerResult FromExpression(string expression, OutputFormat format)
        {
            return new AnalyzerResult
            {
                OutputFormat = format,
                Success = true,
                Expression = expression
            };
        }

        public static AnalyzerResult FromError(string error)
        {
            return new AnalyzerResult
            {
                Success = false,
                Error = error
            };
        }

        public bool Success { get; private set; }
        public OutputFormat OutputFormat { get; private set; }
        public string Expression { get; private set; }
        public string Error { get; private set; }
        public bool IsCancelled { get; private set; }
        public static AnalyzerResult Cancelled { get; } = new AnalyzerResult { Success = false, IsCancelled = true };

    }
}
