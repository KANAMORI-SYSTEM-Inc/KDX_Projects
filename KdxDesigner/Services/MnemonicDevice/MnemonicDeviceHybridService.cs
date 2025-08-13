using System;
using System.Collections.Generic;
using System.Linq;
using Kdx.Contracts.DTOs;
using Kdx.Contracts.Enums;
using KdxDesigner.Models;
using KdxDesigner.Services.Access;
using KdxDesigner.Services.Memory;

namespace KdxDesigner.Services.MnemonicDevice
{
    /// <summary>
    /// MnemonicDeviceのハイブリッドサービス
    /// メモリストアを優先的に使用し、必要に応じてデータベースにも保存する
    /// </summary>
    public class MnemonicDeviceHybridService : IMnemonicDeviceService
    {
        private readonly IMnemonicDeviceMemoryStore _memoryStore;
        private readonly MnemonicDeviceService _dbService;
        private readonly IMemoryService _memoryService;
        
        // メモリストアのみを使用するかどうかのフラグ
        private bool _useMemoryStoreOnly = true;
        
        public MnemonicDeviceHybridService(
            IAccessRepository repository,
            IMemoryService memoryService,
            IMnemonicDeviceMemoryStore memoryStore = null)
        {
            _memoryStore = memoryStore ?? new MnemonicDeviceMemoryStore();
            _dbService = new MnemonicDeviceService(repository);
            _memoryService = memoryService;
        }
        
        /// <summary>
        /// メモリストアのみを使用するかどうかを設定
        /// </summary>
        public void SetMemoryOnlyMode(bool useMemoryOnly)
        {
            _useMemoryStoreOnly = useMemoryOnly;
        }
        
        /// <summary>
        /// PlcIdに基づいてニーモニックデバイスのリストを取得
        /// </summary>
        public List<Models.MnemonicDevice> GetMnemonicDevice(int plcId)
        {
            // まずメモリストアから取得を試みる
            var devices = _memoryStore.GetMnemonicDevices(plcId);
            
            // メモリストアにデータがない場合、データベースから取得
            if (!devices.Any() && !_useMemoryStoreOnly)
            {
                devices = _dbService.GetMnemonicDevice(plcId);
                
                // データベースから取得したデータをメモリストアにキャッシュ
                if (devices.Any())
                {
                    _memoryStore.BulkAddMnemonicDevices(devices, plcId);
                }
            }
            
            return devices;
        }
        
        /// <summary>
        /// PlcIdとMnemonicIdに基づいてニーモニックデバイスのリストを取得
        /// </summary>
        public List<Models.MnemonicDevice> GetMnemonicDeviceByMnemonic(int plcId, int mnemonicId)
        {
            // メモリストアから取得
            var devices = _memoryStore.GetMnemonicDevicesByMnemonic(plcId, mnemonicId);
            
            // メモリストアにデータがない場合、データベースから取得
            if (!devices.Any() && !_useMemoryStoreOnly)
            {
                devices = _dbService.GetMnemonicDeviceByMnemonic(plcId, mnemonicId);
                
                // データベースから取得したデータをメモリストアに追加
                foreach (var device in devices)
                {
                    _memoryStore.AddOrUpdateMnemonicDevice(device, plcId);
                }
            }
            
            return devices;
        }
        
        /// <summary>
        /// すべてのニーモニックデバイスを削除
        /// </summary>
        public void DeleteAllMnemonicDevices()
        {
            // メモリストアをクリア
            _memoryStore.ClearAll();
            
            // データベースもクリア（メモリオンリーモードでない場合）
            if (!_useMemoryStoreOnly)
            {
                _dbService.DeleteAllMnemonicDevices();
            }
        }
        
        /// <summary>
        /// 特定のニーモニックデバイスを削除
        /// </summary>
        public void DeleteMnemonicDevice(int mnemonicId, int recordId)
        {
            // TODO: メモリストアから特定のデバイスを削除する実装
            // 現在は未実装のため、データベースサービスに委譲
            if (!_useMemoryStoreOnly)
            {
                _dbService.DeleteMnemonicDevice(mnemonicId, recordId);
            }
        }
        
        /// <summary>
        /// Process用のニーモニックデバイスを保存
        /// </summary>
        public void SaveMnemonicDeviceProcess(List<Process> processes, int startNum, int plcId)
        {
            var devices = new List<Models.MnemonicDevice>();
            var memories = new List<Kdx.Contracts.DTOs.Memory>();
            
            foreach (var process in processes)
            {
                var device = new Models.MnemonicDevice
                {
                    MnemonicId = (int)MnemonicType.Process,
                    RecordId = process.Id,
                    DeviceLabel = "L",
                    StartNum = startNum,
                    OutCoilCount = 10,
                    PlcId = plcId,
                    Comment1 = process.ProcessName,
                    Comment2 = process.ProcessCategoryId?.ToString() ?? ""
                };
                
                devices.Add(device);
                
                // メモリデータも生成
                for (int i = 0; i < device.OutCoilCount; i++)
                {
                    var memory = GenerateMemoryForDevice(device, i);
                    memories.Add(memory);
                }
                
                startNum += 100; // 次のプロセス用にオフセット
            }
            
            // メモリストアに保存
            _memoryStore.BulkAddMnemonicDevices(devices, plcId);
            _memoryStore.CacheGeneratedMemories(memories, plcId);
            
            // データベースにも保存（メモリオンリーモードでない場合）
            if (!_useMemoryStoreOnly)
            {
                _dbService.SaveMnemonicDeviceProcess(processes, startNum, plcId);
            }
        }
        
        /// <summary>
        /// ProcessDetail用のニーモニックデバイスを保存
        /// </summary>
        public void SaveMnemonicDeviceProcessDetail(List<ProcessDetail> details, int startNum, int plcId)
        {
            var devices = new List<Models.MnemonicDevice>();
            var memories = new List<Kdx.Contracts.DTOs.Memory>();
            
            foreach (var detail in details)
            {
                var device = new Models.MnemonicDevice
                {
                    MnemonicId = (int)MnemonicType.ProcessDetail,
                    RecordId = detail.Id,
                    DeviceLabel = "L",
                    StartNum = startNum,
                    OutCoilCount = 10,
                    PlcId = plcId,
                    Comment1 = detail.DetailName,
                    Comment2 = detail.ProcessId.ToString()
                };
                
                devices.Add(device);
                
                // メモリデータも生成
                for (int i = 0; i < device.OutCoilCount; i++)
                {
                    var memory = GenerateMemoryForDevice(device, i);
                    memories.Add(memory);
                }
                
                startNum += 100;
            }
            
            // メモリストアに保存
            _memoryStore.BulkAddMnemonicDevices(devices, plcId);
            
            // 既存のキャッシュに追加
            var existingMemories = _memoryStore.GetCachedMemories(plcId);
            existingMemories.AddRange(memories);
            _memoryStore.CacheGeneratedMemories(existingMemories, plcId);
        }
        
        /// <summary>
        /// Operation用のニーモニックデバイスを保存
        /// </summary>
        public void SaveMnemonicDeviceOperation(List<Operation> operations, int startNum, int plcId)
        {
            var devices = new List<Models.MnemonicDevice>();
            var memories = new List<Kdx.Contracts.DTOs.Memory>();
            
            foreach (var operation in operations)
            {
                var device = new Models.MnemonicDevice
                {
                    MnemonicId = (int)MnemonicType.Operation,
                    RecordId = operation.Id,
                    DeviceLabel = "M",
                    StartNum = startNum,
                    OutCoilCount = 10,
                    PlcId = plcId,
                    Comment1 = operation.OperationName,
                    Comment2 = operation.GoBack?.ToString() ?? ""
                };
                
                devices.Add(device);
                
                // メモリデータも生成
                for (int i = 0; i < device.OutCoilCount; i++)
                {
                    var memory = GenerateMemoryForDevice(device, i);
                    memories.Add(memory);
                }
                
                startNum += 100;
            }
            
            // メモリストアに保存
            _memoryStore.BulkAddMnemonicDevices(devices, plcId);
            
            // 既存のキャッシュに追加
            var existingMemories = _memoryStore.GetCachedMemories(plcId);
            existingMemories.AddRange(memories);
            _memoryStore.CacheGeneratedMemories(existingMemories, plcId);
            
            // データベースにも保存（メモリオンリーモードでない場合）
            if (!_useMemoryStoreOnly)
            {
                _dbService.SaveMnemonicDeviceOperation(operations, startNum, plcId);
            }
        }
        
        /// <summary>
        /// CY用のニーモニックデバイスを保存
        /// </summary>
        public void SaveMnemonicDeviceCY(List<Cylinder> cylinders, int startNum, int plcId)
        {
            var devices = new List<Models.MnemonicDevice>();
            var memories = new List<Kdx.Contracts.DTOs.Memory>();
            
            foreach (var cylinder in cylinders)
            {
                var device = new Models.MnemonicDevice
                {
                    MnemonicId = (int)MnemonicType.CY,
                    RecordId = cylinder.Id,
                    DeviceLabel = "M",
                    StartNum = startNum,
                    OutCoilCount = 10,
                    PlcId = plcId,
                    Comment1 = cylinder.CYNum,
                    Comment2 = ""
                };
                
                devices.Add(device);
                
                // メモリデータも生成
                for (int i = 0; i < device.OutCoilCount; i++)
                {
                    var memory = GenerateMemoryForDevice(device, i);
                    memories.Add(memory);
                }
                
                startNum += 100;
            }
            
            // メモリストアに保存
            _memoryStore.BulkAddMnemonicDevices(devices, plcId);
            
            // 既存のキャッシュに追加
            var existingMemories = _memoryStore.GetCachedMemories(plcId);
            existingMemories.AddRange(memories);
            _memoryStore.CacheGeneratedMemories(existingMemories, plcId);
            
            // データベースにも保存（メモリオンリーモードでない場合）
            if (!_useMemoryStoreOnly)
            {
                _dbService.SaveMnemonicDeviceCY(cylinders, startNum, plcId);
            }
        }
        
        /// <summary>
        /// メモリストアから直接メモリデータを取得
        /// データベースアクセスを避けてパフォーマンスを向上
        /// </summary>
        public List<Kdx.Contracts.DTOs.Memory> GetGeneratedMemories(int plcId)
        {
            return _memoryStore.GetCachedMemories(plcId);
        }
        
        /// <summary>
        /// デバイス情報からメモリデータを生成
        /// </summary>
        private Kdx.Contracts.DTOs.Memory GenerateMemoryForDevice(Models.MnemonicDevice device, int outcoilIndex)
        {
            var deviceNum = device.StartNum + outcoilIndex;
            var deviceString = device.DeviceLabel + deviceNum.ToString();
            
            var categoryString = device.MnemonicId switch
            {
                1 => "工程",
                2 => "工程詳細",
                3 => "操作",
                4 => "出力",
                _ => "なし"
            };
            
            return new Kdx.Contracts.DTOs.Memory
            {
                PlcId = device.PlcId,
                Device = deviceString,
                DeviceNumber = deviceNum,
                DeviceNumber1 = deviceString,
                Category = categoryString,
                Row_1 = device.Comment1,
                Row_2 = device.Comment2,
                Row_3 = $"Outcoil {outcoilIndex}",
                Row_4 = "",
                MnemonicId = device.MnemonicId,
                RecordId = device.RecordId,
                OutcoilNumber = outcoilIndex
            };
        }
        
        /// <summary>
        /// メモリストアの統計情報を取得
        /// </summary>
        public MnemonicDeviceStatistics GetStatistics(int plcId)
        {
            return _memoryStore.GetStatistics(plcId);
        }
        
        /// <summary>
        /// メモリストアのデータをデータベースに永続化
        /// </summary>
        public void PersistToDatabase(int plcId)
        {
            if (_useMemoryStoreOnly)
            {
                // メモリストアのデータをデータベースに保存
                var devices = _memoryStore.GetMnemonicDevices(plcId);
                
                // MnemonicIdごとにグループ化して保存
                var processDevices = devices.Where(d => d.MnemonicId == (int)MnemonicType.Process).ToList();
                var detailDevices = devices.Where(d => d.MnemonicId == (int)MnemonicType.ProcessDetail).ToList();
                var operationDevices = devices.Where(d => d.MnemonicId == (int)MnemonicType.Operation).ToList();
                var cyDevices = devices.Where(d => d.MnemonicId == (int)MnemonicType.CY).ToList();
                
                // TODO: 各タイプごとにデータベースに保存する処理を実装
                // 現在はメモリストアのみで動作するため、必要に応じて実装
            }
        }
    }
    
}
