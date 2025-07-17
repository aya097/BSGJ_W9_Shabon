#nullable enable
using System.Collections.Generic;
using System.Linq;
using Shabon.Param;
using UnityEngine;
using VContainer;
using LitMotion;
using LitMotion.Extensions;

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
            // 画面のランダムな位置にコンボを表示
            var pos = GetRandomPosition();
            var combo = GameObject.Instantiate(_comboViewParam.ComboPrefab, _comboParent.ComboParent);
            RectTransform rect = combo.GetComponent<RectTransform>();
            rect.anchoredPosition = pos;

            // 出現モーション
            LMotion.Create(Vector3.zero, Vector3.one, 0.5f)
                .WithEase(Ease.OutBack) 
                .BindToLocalScale(combo.transform) 
                .AddTo(combo); 

            // コンボ数に対応した評価を取得
            ComboEvaluationGroup comboEvaluationGroup = GetComboEvaluationGroup(comboNum);
            combo.SetCombo(comboNum, comboEvaluationGroup);   // コンボ数を設定

            // 消滅モーション
            LMotion.Create(Vector3.one, Vector3.zero, 0.4f)
            .WithEase(Ease.InBack) 
            .WithOnComplete(() => GameObject.Destroy(combo.gameObject))
            .WithDelay(1.0f)
            .BindToLocalScale(combo.transform);
        }

        // スポーンする場所をランダムに決定
        private Vector2 GetRandomPosition()
        {
            var area = _comboParent.ComboArea.GetComponent<RectTransform>();
            float minX = area.position.x - area.sizeDelta.x / 2;
            float maxX = area.position.x + area.sizeDelta.x / 2;
            float minY = area.position.y - area.sizeDelta.y / 2;
            float maxY = area.position.y + area.sizeDelta.y / 2;
            float randX = Random.Range(minX, maxX);
            float randY = Random.Range(minY, maxY);

            return new Vector2(randX, randY);
        }
        
        /// <summary>
        /// コンボ数に対応した評価テキスト(ex. Good)を変えずメソッド
        /// </summary>
        /// <param name="comboNum"></param>
        private ComboEvaluationGroup GetComboEvaluationGroup(int comboNum)
        {
            ComboEvaluationGroup comboEvaluationGroup
                = _comboViewParam.ComboEvaluationGroups
                    .Where(cep => cep.ComboNum <= comboNum)
                    .OrderByDescending(cep => cep.ComboNum)
                    .FirstOrDefault();

            return comboEvaluationGroup;
        }
    }
}