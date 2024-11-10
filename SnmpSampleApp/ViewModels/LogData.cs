using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnmpSampleApp
{
    internal class LogData : Common.ViewModel.PropertyNotify
    {
        public int MaxLogLines { get; set; } = 500;
        public List<string> Log { get; set; } = new List<string>();

        public string LogText
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var logEntry in Log)
                {
                    sb.AppendLine(logEntry);
                }
                return sb.ToString();
            }
        }

        public void Clear()
        {
            Log.Clear();
            RaisePropertyChanged(nameof(LogText));
        }

        public void AddLogEntry(string message)
        {
            if (Log.Count >= MaxLogLines)
            {
                Log.RemoveAt(0);
            }
            Log.Add(message);
            RaisePropertyChanged(nameof(LogText));
        }
    }
}
