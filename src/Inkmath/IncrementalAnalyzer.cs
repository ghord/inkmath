using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace Inkmath
{
    public class IncrementalAnalyzer : INotifyPropertyChanged
    {
        CancellationTokenSource pending_;
        AnalyzerConfiguration configuration_;
        List<Stroke> strokes_ = new List<Stroke>();

        public event PropertyChangedEventHandler PropertyChanged;

        public IncrementalAnalyzer(AnalyzerConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            configuration.ThrowIfInvalid();

            configuration_ = configuration;
        }

        public void Clear()
        {
            if (pending_ != null)
            {
                pending_.Cancel();
            }

            strokes_.Clear();
            Expression = null;
        }

        public void AddStroke(Stroke stroke)
        {
            strokes_.Add(stroke);

            StrokesChanged();
        }

        private async void StrokesChanged()
        {
            if (pending_ != null)
                pending_.Cancel();

            var cts = new CancellationTokenSource();
            pending_ = cts;

            var analyzer = new Analyzer(configuration_);
            analyzer.AddStrokes(strokes_);

            var result = await analyzer.AnalyzeAsync(cts.Token);

            if (result.Success)
            {
                Expression = result.Expression.Trim();
            }
        }

        public void RemoveStroke(Stroke stroke)
        {
            strokes_.Remove(stroke);

            StrokesChanged();
        }

        private string expression_;

        public string Expression
        {
            get { return expression_; }
            set
            {
                expression_ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Expression)));
            }
        }
    }
}
