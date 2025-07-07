#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shabon.Ui
{
    [CreateAssetMenu(fileName = "ComboViewParam", menuName = "Scriptable Objects/ComboViewParam")]
    public class ComboViewParam : ScriptableObject, IComboViewParam
    {
        [Header("コンボのプレハブ")]
        [SerializeField] ComboViewMono comboPrefab = null!;

        [Header("評価に対応したコンボ数")]
        [SerializeField] private List<ComboEvaluationPair> comboEvaluationPairs = null!;

        // ゲッター
        public ComboViewMono ComboPrefab
        {
            get { return comboPrefab; }
        }
        public IEnumerable<ComboEvaluationPair> ComboEvaluationPairs
        {
            get { return comboEvaluationPairs; }
        }
    }

    public interface IComboViewParam
    {
        ComboViewMono ComboPrefab { get; }
        IEnumerable<ComboEvaluationPair> ComboEvaluationPairs { get; }
    }

    /// <summary>
    /// コンボ評価の種類
    /// </summary>
    public enum ComboEvaluation
    {
        Good,
        Great,
        Excellent
    }

    /// <summary>
    /// コンボ評価と対応するコンボ数のペアを管理するクラス
    /// </summary>
    [Serializable]
    public class ComboEvaluationPair
    {
        [SerializeField] ComboEvaluation comboEvaluation;
        [SerializeField] int comboNum;

        // Getter
        public ComboEvaluation ComboEvaluation
        {
            get { return comboEvaluation; }
        }
        public int ComboNum
        {
            get { return comboNum; }
        }
    }
}