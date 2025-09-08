using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KdxDesigner.Models;
using KdxDesigner.Services.MnemonicDevice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Kdx.Contracts.DTOs;
using Kdx.Contracts.Enums;

namespace KdxDesigner.ViewModels
{
    /// <summary>
    /// メモリデバイスリスト表示用ViewModel
    /// </summary>
    public partial class MemoryDeviceListViewModel : ObservableObject
    {
        private readonly IMnemonicDeviceMemoryStore _memoryStore;
        private readonly int? _plcId;
        private readonly int? _cycleId;

        [ObservableProperty] private ObservableCollection<MnemonicDevice> mnemonicDevices = new();
        [ObservableProperty] private ObservableCollection<MnemonicTimerDevice> timerDevices = new();
        [ObservableProperty] private ObservableCollection<MnemonicSpeedDevice> speedDevices = new();
        
        [ObservableProperty] private int selectedTabIndex = 0;
        [ObservableProperty] private string statusMessage = string.Empty;
        
        // フィルタリング用
        [ObservableProperty] private string filterText = string.Empty;
        [ObservableProperty] private int? selectedMnemonicType;
        
        // 統計情報
        [ObservableProperty] private int totalMnemonicDeviceCount;
        [ObservableProperty] private int totalTimerDeviceCount;
        [ObservableProperty] private int totalSpeedDeviceCount;
        [ObservableProperty] private int processCount;
        [ObservableProperty] private int detailCount;
        [ObservableProperty] private int operationCount;
        [ObservableProperty] private int cylinderCount;

        public ObservableCollection<MnemonicTypeItem> MnemonicTypes { get; } = new()
        {
            new MnemonicTypeItem { Id = null, Name = "すべて" },
            new MnemonicTypeItem { Id = (int)MnemonicType.Process, Name = "工程" },
            new MnemonicTypeItem { Id = (int)MnemonicType.ProcessDetail, Name = "工程詳細" },
            new MnemonicTypeItem { Id = (int)MnemonicType.Operation, Name = "操作" },
            new MnemonicTypeItem { Id = (int)MnemonicType.CY, Name = "シリンダ" }
        };

        public MemoryDeviceListViewModel(IMnemonicDeviceMemoryStore memoryStore, int? plcId = null, int? cycleId = null)
        {
            _memoryStore = memoryStore ?? throw new ArgumentNullException(nameof(memoryStore));
            _plcId = plcId;
            _cycleId = cycleId;
            
            LoadData();
        }

        partial void OnFilterTextChanged(string value)
        {
            ApplyFilter();
        }

        partial void OnSelectedMnemonicTypeChanged(int? value)
        {
            ApplyFilter();
        }

        private void LoadData()
        {
            try
            {
                // MnemonicDevice データの読み込み
                if (_plcId.HasValue)
                {
                    var devices = _memoryStore.GetMnemonicDevices(_plcId.Value);
                    MnemonicDevices = new ObservableCollection<MnemonicDevice>(devices);
                    
                    // 統計情報の更新
                    TotalMnemonicDeviceCount = devices.Count;
                    ProcessCount = devices.Count(d => d.MnemonicId == (int)MnemonicType.Process);
                    DetailCount = devices.Count(d => d.MnemonicId == (int)MnemonicType.ProcessDetail);
                    OperationCount = devices.Count(d => d.MnemonicId == (int)MnemonicType.Operation);
                    CylinderCount = devices.Count(d => d.MnemonicId == (int)MnemonicType.CY);
                }
                
                // TimerDevice データの読み込み
                if (_plcId.HasValue && _cycleId.HasValue)
                {
                    var timers = _memoryStore.GetTimerDevices(_plcId.Value, _cycleId.Value);
                    TimerDevices = new ObservableCollection<MnemonicTimerDevice>(timers);
                    TotalTimerDeviceCount = timers.Count;
                }
                
                // SpeedDevice データの読み込み
                if (_plcId.HasValue)
                {
                    var speeds = _memoryStore.GetSpeedDevices(_plcId.Value);
                    SpeedDevices = new ObservableCollection<MnemonicSpeedDevice>(speeds);
                    TotalSpeedDeviceCount = speeds.Count;
                }
                
                StatusMessage = $"データ読み込み完了: {DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"エラー: {ex.Message}";
                MessageBox.Show($"データの読み込みに失敗しました。\n{ex.Message}", "エラー", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilter()
        {
            // フィルタリング実装
            // 本実装では、CollectionViewSourceを使用してフィルタリングすることを推奨
            // ここでは簡易的な実装を示す
            if (_plcId.HasValue)
            {
                var devices = _memoryStore.GetMnemonicDevices(_plcId.Value);
                
                // Mnemonicタイプでフィルタ
                if (SelectedMnemonicType.HasValue)
                {
                    devices = devices.Where(d => d.MnemonicId == SelectedMnemonicType.Value).ToList();
                }
                
                // テキストフィルタ
                if (!string.IsNullOrWhiteSpace(FilterText))
                {
                    var filterLower = FilterText.ToLower();
                    devices = devices.Where(d => 
                        d.DeviceLabel?.ToLower().Contains(filterLower) == true ||
                        d.StartNum.ToString().Contains(FilterText) ||
                        d.Comment1?.ToLower().Contains(filterLower) == true ||
                        d.Comment2?.ToLower().Contains(filterLower) == true
                    ).ToList();
                }
                
                MnemonicDevices = new ObservableCollection<MnemonicDevice>(devices);
            }
        }

        [RelayCommand]
        private void Refresh()
        {
            LoadData();
        }

        [RelayCommand]
        private void ClearFilter()
        {
            FilterText = string.Empty;
            SelectedMnemonicType = null;
            LoadData();
        }

        [RelayCommand]
        private void ExportToCsv()
        {
            try
            {
                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    DefaultExt = ".csv",
                    FileName = $"MemoryDevices_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (dialog.ShowDialog() == true)
                {
                    using (var writer = new System.IO.StreamWriter(dialog.FileName, false, System.Text.Encoding.UTF8))
                    {
                        // ヘッダー行
                        writer.WriteLine("ID,MnemonicId,RecordId,DeviceLabel,StartNum,OutCoilCount,PlcId,Comment1,Comment2");
                        
                        // データ行
                        foreach (var device in MnemonicDevices)
                        {
                            writer.WriteLine($"{device.ID},{device.MnemonicId},{device.RecordId}," +
                                $"{device.DeviceLabel},{device.StartNum},{device.OutCoilCount}," +
                                $"{device.PlcId}," +
                                $"\"{device.Comment1}\",\"{device.Comment2}\"");
                        }
                    }
                    
                    MessageBox.Show($"CSVファイルを出力しました。\n{dialog.FileName}", "完了", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CSV出力に失敗しました。\n{ex.Message}", "エラー", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Close(Window window)
        {
            window?.Close();
        }
    }

    public class MnemonicTypeItem
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}