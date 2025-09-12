using Kdx.Contracts.DTOs;
using Kdx.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kdx.Infrastructure.Services
{
    /// <summary>
    /// OperationIOテーブルのデータ操作を行うサービスクラス
    /// </summary>
    public class OperationIOService : IOperationIOService
    {
        private readonly IAccessRepository _repository;

        public OperationIOService(IAccessRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// 指定されたOperationに関連付けられたIOのリストを取得
        /// </summary>
        public List<OperationIO> GetOperationIOs(int operationId)
        {
            try
            {
                return _repository.GetOperationIOs(operationId);
            }
            catch (Exception ex) when (ex.Message.Contains("OperationIO") && ex.Message.Contains("見つかりませんでした"))
            {
                // テーブルが存在しない場合は空のリストを返す
                return new List<OperationIO>();
            }
        }

        /// <summary>
        /// 指定されたIOに関連付けられたOperationのリストを取得
        /// </summary>
        public List<OperationIO> GetIOOperations(string ioAddress, int plcId)
        {
            try
            {
                return _repository.GetIOOperations(ioAddress, plcId);
            }
            catch (Exception ex) when (ex.Message.Contains("OperationIO") && ex.Message.Contains("見つかりませんでした"))
            {
                return new List<OperationIO>();
            }
        }

        /// <summary>
        /// OperationとIOの関連付けを追加
        /// </summary>
        public void AddAssociation(int operationId, string ioAddress, int plcId, string ioUsage, string? comment = null)
        {
            // 既存の関連付けをチェック
            var existing = _repository.GetOperationIOs(operationId)
                .FirstOrDefault(o => o.IOAddress == ioAddress && o.PlcId == plcId);

            if (existing != null)
            {
                throw new InvalidOperationException("この関連付けは既に存在します。");
            }

            _repository.AddOperationIOAssociation(operationId, ioAddress, plcId, ioUsage, comment);
        }

        /// <summary>
        /// OperationとIOの関連付けを削除
        /// </summary>
        public void RemoveAssociation(int operationId, string ioAddress, int plcId)
        {
            _repository.RemoveOperationIOAssociation(operationId, ioAddress, plcId);
        }

        /// <summary>
        /// 指定されたPLCのすべての関連付けを取得
        /// </summary>
        public List<OperationIO> GetAllAssociations(int plcId)
        {
            try
            {
                return _repository.GetAllOperationIOAssociations(plcId);
            }
            catch (Exception ex) when (ex.Message.Contains("OperationIO") && ex.Message.Contains("見つかりませんでした"))
            {
                return new List<OperationIO>();
            }
        }
    }
}