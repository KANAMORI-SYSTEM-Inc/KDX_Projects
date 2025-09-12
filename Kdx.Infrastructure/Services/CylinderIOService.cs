using Kdx.Contracts.DTOs;
using Kdx.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kdx.Infrastructure.Services
{
    /// <summary>
    /// CylinderIOテーブルのデータ操作を行うサービスクラス
    /// </summary>
    public class CylinderIOService : ICylinderIOService
    {
        private readonly IAccessRepository _repository;

        public CylinderIOService(IAccessRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// 指定されたCYに関連付けられたIOのリストを取得
        /// </summary>
        public List<CylinderIO> GetCylinderIOs(int cylinderId, int plcId)
        {
            try
            {
                return _repository.GetCylinderIOs(cylinderId, plcId);
            }
            catch (Exception ex) when (ex.Message.Contains("CylinderIO") && ex.Message.Contains("見つかりませんでした"))
            {
                // テーブルが存在しない場合は空のリストを返す
                return new List<CylinderIO>();
            }
        }

        /// <summary>
        /// 指定されたIOに関連付けられたCYのリストを取得
        /// </summary>
        public List<CylinderIO> GetIOCylinders(string ioAddress, int plcId)
        {
            return _repository.GetIOCylinders(ioAddress, plcId);
        }

        /// <summary>
        /// CYとIOの関連付けを追加
        /// </summary>
        public void AddAssociation(int cylinderId, string ioAddress, int plcId, string ioType, string? comment = null)
        {
            // 既存の関連付けをチェック
            var existing = _repository.GetCylinderIOs(cylinderId, plcId)
                .FirstOrDefault(c => c.IOAddress == ioAddress);

            if (existing != null)
            {
                throw new InvalidOperationException("この関連付けは既に存在します。");
            }

            _repository.AddCylinderIOAssociation(cylinderId, ioAddress, plcId, ioType, comment);
        }

        /// <summary>
        /// CYとIOの関連付けを削除
        /// </summary>
        public void RemoveAssociation(int cylinderId, string ioAddress, int plcId)
        {
            _repository.RemoveCylinderIOAssociation(cylinderId, ioAddress, plcId);
        }

        /// <summary>
        /// 指定されたPLCのすべての関連付けを取得
        /// </summary>
        public List<CylinderIO> GetAllAssociations(int plcId)
        {
            return _repository.GetAllCylinderIOAssociations(plcId);
        }
    }
}