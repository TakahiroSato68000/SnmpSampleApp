using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SnmpSampleApp
{
    internal class MainWindowViewModel : Common.ViewModel.BaseNotifyListViewModel
    {
        internal MainModel MainModel { get => MainModel.Instance; }
        internal ConfigData ConfigData { get => MainModel.ConfigData; }
        internal LogData LogData { get => MainModel.Instance.LogData; }
        public string LogText { get { return MainModel.Instance.LogData.LogText; } }

        public ObservableCollection<string> IpAddressList
        {
            get => GetProperty<ObservableCollection<string>>(
                initialValue: new ObservableCollection<string>(ConfigData.IpAddressList)
                );
            set => RaisePropertyChangedIfSet(value);
        }

        public string IpAddress
        {
            get => GetProperty<string>(initialValue: ConfigData.IpAddressList[0]);
            set => RaisePropertyChangedIfSet(value);
        }

        public ObservableCollection<string> Commands
        {
            get => GetProperty<ObservableCollection<string>>(
                initialValue: new ObservableCollection<string>(ConfigData.CommandList)
                );
            set => RaisePropertyChangedIfSet(value);
        }

        public string SelectedCommand
        {
            get => GetProperty<string>(initialValue: ConfigData.CommandList[0]);
            set => RaisePropertyChangedIfSet(value);
        }

        public ICommand GetSnmpDataCommand { get; }
        public ICommand GetSnmpWalk { get; }

        public MainWindowViewModel()
        {
            GetSnmpDataCommand = new RelayCommand(ExecuteGetSnmpData);
            GetSnmpWalk = new RelayCommand(ExecutetWalkSnmp);
        }

        private async void ExecuteGetSnmpData(object? parameter)
        {
            try
            {
                //string data = string.Empty;
                await Task.Run(() =>
                {
                    string community = "public"; // コミュニティ名を指定
                    MainModel.GetSnmpData(IpAddress, community, SelectedCommand);
                    RaisePropertyChanged(nameof(LogText));
                });
            }
            catch (System.Exception ex)
            {
                LogData.AddLogEntry(string.Format("Exception: '{0}'", ex.Message));
                RaisePropertyChanged(nameof(LogText));
                MessageBox.Show(ex.Message);
            }
        }
        private async void ExecutetWalkSnmp(object? parameter)
        {
            try
            {
                await Task.Run(() =>
                {
                    string community = "public"; // コミュニティ名を指定
                    MainModel.GetSnmpWalk(IpAddress, community, SelectedCommand);
                    RaisePropertyChanged(nameof(LogText));
                });
            }
            catch (System.Exception ex)
            {
                LogData.AddLogEntry(string.Format("Exception: '{0}'", ex.Message));
                RaisePropertyChanged(nameof(LogText));
                MessageBox.Show(ex.Message);
            }
        }
    }
}