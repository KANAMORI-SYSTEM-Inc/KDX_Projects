using System;
using System.Collections.Generic;
using System.Linq;
using Kdx.Contracts.DTOs;
using Kdx.Contracts.Enums;
using KdxDesigner.Models;
using KdxDesigner.Services.Access;
using KdxDesigner.Services.MnemonicDevice;
using KdxDesigner.ViewModels;
using Timer = Kdx.Contracts.DTOs.Timer;

namespace KdxDesigner.Services.MemonicTimerDevice
{
    /// <summary>
    /// MnemonicTimerDeviceServiceのメモリストアアダプター
    /// 既存のインターフェースを維持しながら、メモリストアを使用する
    /// </summary>
    public class MnemonicTimerDeviceMemoryAdapter : IMnemonicTimerDeviceService
    {
        private readonly IMnemonicDeviceMemoryStore _memoryStore;
        private readonly MnemonicTimerDeviceService _dbService;
        private bool _useMemoryStoreOnly = true;
        
        public MnemonicTimerDeviceMemoryAdapter(
            IAccessRepository repository,
            MainViewModel mainViewModel,
            IMnemonicDeviceMemoryStore memoryStore = null)
        {
            _memoryStore = memoryStore ?? new MnemonicDeviceMemoryStore();
            _dbService = new MnemonicTimerDeviceService(repository, mainViewModel);
        }
        
        /// <summary>
        /// メモリストアのみを使用するかどうかを設定
        /// </summary>
        public void SetMemoryOnlyMode(bool useMemoryOnly)
        {
            _useMemoryStoreOnly = useMemoryOnly;
        }
        
        /// <summary>
        /// PlcIdとCycleIdに基づいてMnemonicTimerDeviceを取得
        /// </summary>
        public List<MnemonicTimerDevice> GetMnemonicTimerDevice(int plcId, int cycleId)
        {
            // メモリストアから取得
            var devices = _memoryStore.GetTimerDevices(plcId, cycleId);
            
            // メモリストアにデータがない場合、データベースから取得
            if (!devices.Any() && !_useMemoryStoreOnly)
            {
                devices = _dbService.GetMnemonicTimerDevice(plcId, cycleId);
                
                // データベースから取得したデータをメモリストアにキャッシュ
                foreach (var device in devices)
                {
                    _memoryStore.AddOrUpdateTimerDevice(device, plcId, cycleId);
                }
            }
            
            return devices;
        }
        
        /// <summary>
        /// すべてのMnemonicTimerDeviceレコードを削除
        /// </summary>
        public void DeleteAllMnemonicTimerDevice()
        {
            // メモリストアをクリア（全体）
            // 注: 現在のメモリストアは個別のPlcId/CycleIdごとのクリアのみサポート
            // 必要に応じて拡張が必要
            
            // データベースもクリア（メモリオンリーモードでない場合）
            if (!_useMemoryStoreOnly)
            {
                _dbService.DeleteAllMnemonicTimerDevice();
            }
        }
        
        /// <summary>
        /// ProcessDetailのタイマーデバイスを保存
        /// </summary>
        public void SaveWithDetail(List<Timer> timers, List<ProcessDetail> details, int startNum, int plcId, ref int count)
        {
            var devices = new List<MnemonicTimerDevice>();
            
            foreach (var detail in details)
            {
                // ProcessDetailに関連するタイマーを検索
                // MnemonicIdとCycleIdで関連付け
                var timer = timers.FirstOrDefault(t => 
                    t.MnemonicId == (int)MnemonicType.ProcessDetail && 
                    t.CycleId == detail.CycleId);
                if (timer == null) continue;
                
                var device = new MnemonicTimerDevice
                {
                    MnemonicId = (int)MnemonicType.ProcessDetail,
                    RecordId = detail.Id,
                    TimerId = timer.ID,
                    TimerCategoryId = timer.TimerCategoryId,
                    ProcessTimerDevice = $"T{startNum + count}",
                    TimerDevice = $"ZR{timer.TimerNum}",
                    PlcId = plcId,
                    CycleId = timer.CycleId,
                    Comment1 = detail.DetailName,
                    Comment2 = timer.Example?.ToString(),
                    Comment3 = timer.TimerName
                };
                
                devices.Add(device);
                
                // メモリストアに保存
                _memoryStore.AddOrUpdateTimerDevice(device, plcId, timer.CycleId ?? 1);
                
                count++;
            }
            
            // データベースにも保存（メモリオンリーモードでない場合）
            if (!_useMemoryStoreOnly)
            {
                _dbService.SaveWithDetail(timers, details, startNum, plcId, ref count);
            }
        }
        
        /// <summary>
        /// Operationのタイマーデバイスを保存
        /// </summary>
        public void SaveWithOperation(List<Timer> timers, List<Operation> operations, int startNum, int plcId, ref int count)
        {
            var devices = new List<MnemonicTimerDevice>();
            
            foreach (var operation in operations)
            {
                // Operationに関連するタイマーを検索
                // MnemonicIdとCycleIdで関連付け
                var timer = timers.FirstOrDefault(t => 
                    t.MnemonicId == (int)MnemonicType.Operation && 
                    t.CycleId == operation.CycleId);
                if (timer == null) continue;
                
                var device = new MnemonicTimerDevice
                {
                    MnemonicId = (int)MnemonicType.Operation,
                    RecordId = operation.Id,
                    TimerId = timer.ID,
                    TimerCategoryId = timer.TimerCategoryId,
                    ProcessTimerDevice = $"T{startNum + count}",
                    TimerDevice = $"ZR{timer.TimerNum}",
                    PlcId = plcId,
                    CycleId = timer.CycleId,
                    Comment1 = operation.OperationName,
                    Comment2 = timer.Example?.ToString(),
                    Comment3 = timer.TimerName
                };
                
                devices.Add(device);
                
                // メモリストアに保存
                _memoryStore.AddOrUpdateTimerDevice(device, plcId, timer.CycleId ?? 1);
                
                count++;
            }
            
            // データベースにも保存（メモリオンリーモードでない場合）
            if (!_useMemoryStoreOnly)
            {
                _dbService.SaveWithOperation(timers, operations, startNum, plcId, ref count);
            }
        }
        
        /// <summary>
        /// CY（シリンダー）のタイマーデバイスを保存
        /// </summary>
        public void SaveWithCY(List<Timer> timers, List<CY> cylinders, int startNum, int plcId, ref int count)
        {
            var devices = new List<MnemonicTimerDevice>();
            
            foreach (var cylinder in cylinders)
            {
                // CYに関連するタイマーを検索
                // MnemonicIdで関連付け（CYはCycleIdを持たない可能性がある）
                var relevantTimers = timers.Where(t => t.MnemonicId == (int)MnemonicType.CY).ToList();
                
                foreach (var timer in relevantTimers)
                {
                
                    var device = new MnemonicTimerDevice
                    {
                        MnemonicId = (int)MnemonicType.CY,
                        RecordId = cylinder.Id,
                        TimerId = timer.ID,
                        TimerCategoryId = timer.TimerCategoryId,
                        ProcessTimerDevice = $"T{startNum + count}",
                        TimerDevice = $"ZR{timer.TimerNum}",
                        PlcId = plcId,
                        CycleId = timer.CycleId,
                        Comment1 = cylinder.CYNum,
                        Comment2 = timer.Example?.ToString(),
                        Comment3 = timer.TimerName
                    };
                    
                    devices.Add(device);
                    
                    // メモリストアに保存
                    _memoryStore.AddOrUpdateTimerDevice(device, plcId, timer.CycleId ?? 1);
                    
                    count++;
                }
            }
            
            // データベースにも保存（メモリオンリーモードでない場合）
            if (!_useMemoryStoreOnly)
            {
                _dbService.SaveWithCY(timers, cylinders, startNum, plcId, ref count);
            }
        }
        
        // その他のインターフェースメソッドの実装
        public List<MnemonicTimerDevice> GetMnemonicTimerDeviceByCycle(int plcId, int cycleId, int mnemonicId)
        {
            var devices = _memoryStore.GetTimerDevices(plcId, cycleId);
            return devices.Where(d => d.MnemonicId == mnemonicId).ToList();
        }
        
        public List<MnemonicTimerDevice> GetMnemonicTimerDeviceByMnemonic(int plcId, int mnemonicId)
        {
            // 全サイクルのデータを取得する必要がある
            // 現在のメモリストアの制限により、データベースから取得
            if (!_useMemoryStoreOnly)
            {
                return _dbService.GetMnemonicTimerDeviceByMnemonic(plcId, mnemonicId);
            }
            return new List<MnemonicTimerDevice>();
        }
        
        public List<MnemonicTimerDevice> GetMnemonicTimerDeviceByTimerId(int plcId, int timerId)
        {
            // タイマーIDでの検索
            // 現在のメモリストアの制限により、データベースから取得
            if (!_useMemoryStoreOnly)
            {
                return _dbService.GetMnemonicTimerDeviceByTimerId(plcId, timerId);
            }
            return new List<MnemonicTimerDevice>();
        }
    }
}