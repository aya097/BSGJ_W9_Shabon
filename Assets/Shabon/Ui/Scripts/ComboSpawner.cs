#nullable enable

using Shabon.Param;
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
        private readonly IComboParent _comboParent;

        [Inject]
        public ComboSpawner(IComboViewParam comboViewParam, IComboParent comboParent)
        {
            _comboViewParam = comboViewParam;
            _comboParent = comboParent;
            GetRandomPosition();

        }

        // コンボ
        public void Spawn(int comboNum)
        {
            var pos = GetRandomPosition();
            var combo = GameObject.Instantiate(_comboViewParam.ComboPrefab, pos, Quaternion.identity, _comboParent.ComboParent);
            combo.SetCombo(comboNum);   // コンボ数を設定
            GameObject.Destroy(combo.gameObject, 1f);
        }

        // スポーンする場所をランダムに決定
        private Vector2 GetRandomPosition()
        {
            var area = _comboParent.ComboArea.GetComponent<RectTransform>();
            float minX = Screen.width - area.position.x - area.sizeDelta.x / 2;
            float maxX = Screen.width - area.position.x + area.sizeDelta.x / 2;
            float minY = Screen.height - area.position.y - area.sizeDelta.y / 2;
            float maxY = Screen.height - area.position.y + area.sizeDelta.y / 2;
            float randX = Random.Range(minX, maxX);
            float randY = Random.Range(minY, maxY);

            return new Vector2(randX, randY);
        }
    }
}