using Kdx.Contracts.Interfaces;

namespace KdxDesigner.Services.Difinitions
{
    /// <summary>
    /// 定義情報のデータ操作を行うサービス実装
    /// </summary>
    internal class DifinitionsService : IDifinitionsService
    {
        private readonly IAccessRepository _repository;

        public DifinitionsService(IAccessRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public List<Models.Difinitions> GetDifinitions(string category)
        {
            // IAccessRepositoryのGetDifinitionsメソッドを使用（categoryパラメータを渡す）
            var dtoDifinitions = _repository.GetDifinitions(category ?? string.Empty);
            
            // DTOからModelsへ変換
            return dtoDifinitions.Select(d => new Models.Difinitions
            {
                // プロパティをマッピング（Difinitionsモデルの定義に基づいて調整が必要）
                ID = d.ID,
                Category = d.Category,
                // 他のプロパティも必要に応じてマッピング
            }).ToList();
        }
    }
}