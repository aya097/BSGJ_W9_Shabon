using System.Collections.Generic;
using System.Linq;
using R3;
using Shabon.Dirt;
using Shabon.Param;
using Shabon.Score;
using UnityEngine;
using VContainer;

public class DirtEffectCollectorMono : MonoBehaviour
{
    [SerializeField] private List<DirtEffectViewMono> firstDirtEffects = new();     // 一段階目の汚れエフェクト
    [SerializeField] private List<DirtEffectViewMono> secondDirtEffects = new();    // 二段階目の汚れエフェクト
    [SerializeField] private List<DirtEffectViewMono> thirdDirtEffects = new();     // 三段階目の汚れエフェクト
    [SerializeField] private DirtEffectViewMono finalDirtEffect = null!;    // 最後に表示するエフェクト


    [Inject]
    private void Initialize(IDirtValue dirtValue, IGameRuleParam gameRuleParam)
    {
        Observable.EveryValueChanged(dirtValue, d => d.DirtNum)
            .Subscribe(dirtNum =>
            {
                // 汚れの段階が変わったらエフェクトを更新
                UpdateDirtEffects(dirtValue);

                // 汚れが最大になったら
                if (dirtNum == gameRuleParam.MaxDirtValue)
                {
                    finalDirtEffect.Enable();
                }
            })
            .AddTo(this);
    }



    private void UpdateDirtEffects(IDirtValue dirtValue)
    {
        int dirtNum = dirtValue.DirtNum - 2;    // 最初の2段階は無視 
        // 現在Activeな汚れエフェクトの数を取得
        int currentDirt = firstDirtEffects.Where(dirt => dirt.IsActive).Count();
        currentDirt += secondDirtEffects.Where(dirt => dirt.IsActive).Count();
        currentDirt += thirdDirtEffects.Where(dirt => dirt.IsActive).Count();


        // エフェクトの方が少ない場合追加
        if (currentDirt < dirtNum)
        {
            int diff = dirtNum - currentDirt;
            for (int i = 0; i < diff; i++)
            {
                IncreaseDirtEffects();
            }
        }
        // エフェクトの方が多い場合削除
        else if (currentDirt > dirtNum)
        {
            int diff = currentDirt - dirtNum;
            for (int i = 0; i < diff; i++)
            {
                DecreaseDirtEffects();
            }
        }
    }
    // 汚れを一つ減らす
    private void DecreaseDirtEffects()
    {
        // 3段階目の汚れエフェクトをランダムで減らす
        if (RandomDecrease(thirdDirtEffects)) return;
        // 2段階目の汚れエフェクトをランダムで減らす
        if (RandomDecrease(secondDirtEffects)) return;
        // 1段階目の汚れエフェクトをランダムで減らす
        if (RandomDecrease(firstDirtEffects)) return;
    }
    // ランダムで汚れエフェクトを一つ減らす、もし減らせなければfalse
    private bool RandomDecrease(List<DirtEffectViewMono> dirtEffects)
    {
        // アクティブな汚れエフェクトがある場合、ランダムで一つ減らす
        var activeDirtEffects = dirtEffects.Where(dirt => dirt.IsActive);
        if (activeDirtEffects.Count() > 0)
        {
            int randomIndex = Random.Range(0, activeDirtEffects.Count());  // ランダムなものを削除
            activeDirtEffects.ElementAt(randomIndex).Disable();
            return true;
        }
        return false;
    }

    // 汚れを一つ増やす
    private void IncreaseDirtEffects()
    {
        // 1段階目の汚れエフェクトをランダムでアクティブにする
        if (RandomIncrease(firstDirtEffects)) return;
        // 2段階目の汚れエフェクトをランダムでアクティブにする
        if (RandomIncrease(secondDirtEffects)) return;
        // 3段階目の汚れエフェクトをランダムでアクティブにする
        if (RandomIncrease(thirdDirtEffects)) return;

    }

    // ランダムで汚れエフェクトを一つ増やす、もし増やせなければfalse
    private bool RandomIncrease(List<DirtEffectViewMono> dirtEffects)
    {
        // 非アクティブな汚れエフェクトがある場合、ランダムで一つ増やす
        var inactiveDirtEffects = dirtEffects.Where(dirt => !dirt.IsActive);
        if (inactiveDirtEffects.Count() > 0)
        {
            int randomIndex = Random.Range(0, inactiveDirtEffects.Count());  // ランダムなものをアクティブにする
            inactiveDirtEffects.ElementAt(randomIndex).Enable();
            return true;
        }
        return false;
    }
}
