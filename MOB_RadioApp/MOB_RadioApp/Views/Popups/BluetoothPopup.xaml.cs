using MOB_RadioApp.Models;
using MOB_RadioApp.ViewModels;
using MvvmCross;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOB_RadioApp.Popups
{
    /// <summary>
    /// popup for filtering on genre and language
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothPopup : PopupPage
    {


        #region Fields
        
        private readonly IBluetoothLE _ble;
        IAdapter _adapter;
        private ObservableCollection<IDevice> _devices;
        private IDevice _nativeDevice;
        private IDevice _selectedDevice;
        private IList<IService> _services;
        private IService _service;
        #endregion Fields
        #region Constructors
        public BluetoothPopup()
        {
            InitializeComponent();
             BindingContext = this;
            _ble = CrossBluetoothLE.Current;

            _adapter = CrossBluetoothLE.Current.Adapter;
            _devices = new ObservableCollection<IDevice>();
        }


        #endregion Constructors

        #region Properties
        public ObservableCollection<IDevice> Devices
        {
            get { return _devices; }
            set { SetValue(ref _devices, value); }
        }
        public IDevice NativeDevice
        {
            get { return _nativeDevice; }
            set => SetValue(ref _nativeDevice, value);
        }
        public IDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set => SetValue(ref _selectedDevice, value);
        }
        #endregion
        #region Commands

        #endregion Commands

        #region Private Methods
        protected void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return;
            backingField = value;
            OnPropertyChanged(propertyName);
        }
        #region Bluetooth
        private async void Button_Clicked(object sender, EventArgs e)
        {
            _devices.Clear();
            _adapter.ScanTimeout = 20000;
            _adapter.ScanMode = ScanMode.Balanced;
            _adapter.DeviceDiscovered += (s, a) =>
            {
                _devices.Add(a.Device);
            };
            if (!_ble.Adapter.IsScanning)
                await _adapter.StartScanningForDevicesAsync();
        }
       
        private async Task StatusAsync()
        {
            var state = _ble.State;
            await App.Current.MainPage.DisplayAlert("Notice", state.ToString(), "ok");
        }
        private async Task ConnectAsync()
        {
            try
            {
                if (_selectedDevice != null)
                {
                    await _adapter.ConnectToDeviceAsync(_selectedDevice);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("error", "no device selected", "ok");
                }
            }

            catch (DeviceConnectionException ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message.ToString(), "ok");
            }
        }

        private async Task ConnectKnownDeviceAsync()
        {

            try
            {
                await _adapter.ConnectToKnownDeviceAsync(new Guid("guid"));
            }
            catch (DeviceConnectionException ex)
            {
                await App.Current.MainPage.DisplayAlert("Error,", ex.Message.ToString(), "ok");
            }
        }
        private async Task GetServicesAsync()
        {
            _services = (IList<IService>)await _selectedDevice.GetServicesAsync();
            _service = await _selectedDevice.GetServiceAsync(_selectedDevice.Id);
        }
        IList<ICharacteristic> _characteristics;
        ICharacteristic _characteristic;
        private async Task GetCharacteristicsAsync()
        {
            var characteristics = await _service.GetCharacteristicsAsync();
            Guid idGuid = Guid.Parse("guid");
            _characteristic = await _service.GetCharacteristicAsync(idGuid);
        }
        IDescriptor _descriptor;
        IList<IDescriptor> _descriptors;
        private async Task GetDescriptorsAsync()
        {
            _descriptors = (IList<IDescriptor>)await _characteristic.GetDescriptorsAsync();
            _descriptor = await _characteristic.GetDescriptorAsync(Guid.Parse("guid"));
        }
        private async Task ReadWriteDescriptorsAsync()
        {
            var bytes = await _descriptor.ReadAsync();
            await _descriptor.WriteAsync(bytes);
        }
        private async Task ReadWriteCharacteristicAsync()
        {
            var bytes = await _characteristic.ReadAsync();
            await _characteristic.WriteAsync(bytes);
        }
        private async Task UpdateAsync()
        {
            _characteristic.ValueUpdated += (o, args) =>
            {
                var bytes = args.Characteristic.Value;
            };
            await _characteristic.StartUpdatesAsync();
        }


    }


    #endregion
    #endregion Private Methods

}
