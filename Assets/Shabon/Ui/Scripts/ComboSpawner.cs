#nullable enable

using Unity.Mathematics;
using UnityEngine;
using VContainer;

namespace Shabon.Ui
{
    /// <summary>
    /// ComboViewを生成するクラス
    /// </summary>
    public class ComboSpawner
    {
        private readonly IComboViewParam _comboViewParam;   // コンボの本体

        [Inject]
        public ComboSpawner(IComboViewParam comboViewParam)
        {
            _comboViewParam = comboViewParam;
        }

        // コンボ
        public void Spawn(int comboNum)
        {
            var combo = GameObject.Instantiate(_comboViewParam.ComboPrefab, Vector3.zero, quaternion.identity);
            GameObject.Destroy(combo);
        }
    }
}