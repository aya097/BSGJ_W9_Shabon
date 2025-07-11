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

        [Header("その他パラメータ")]
        [SerializeField] private List<ComboEvaluationGroup> comboEvaluationGroups = null!;

        // Getter
        public ComboViewMono ComboPrefab
        {
            get { return comboPrefab; }
        }
        public IEnumerable<ComboEvaluationGroup> ComboEvaluationGroups
        {
            get { return comboEvaluationGroups; }
        }
    }

    public interface IComboViewParam
    {
        ComboViewMono ComboPrefab { get; }
        IEnumerable<ComboEvaluationGroup> ComboEvaluationGroups { get; }
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
    public class ComboEvaluationGroup
    {   
        [Header("コンボの評価")]
        [SerializeField] ComboEvaluation comboEvaluation;
        
        [Header("コンボの評価テキストの色")]
        [SerializeField] Color textColor;

        [Header("コンボ数")]
        [SerializeField] int comboNum;
        
        // Getter
        public ComboEvaluation ComboEvaluation
        {
            get { return comboEvaluation; }
        }
        public Color TextColor
        {
            get { return textColor; }
        }
        public int ComboNum
        {
            get { return comboNum; }
        }
    }
}