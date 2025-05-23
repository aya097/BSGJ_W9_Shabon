using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Shabon.Bubble;

namespace Shabon.Clap
{
    public class ClapModel
    {
        private readonly IBubbleHandler _bubbleHandler; // バブル操作を管理するハンドラー
        private readonly ClapGetterViewMono _clapGetter; // ClapGetterViewMono を参照

        public ClapModel(IBubbleHandler bubbleHandler, ClapGetterViewMono clapGetter)
        {
            // コンストラクタでバブルハンドラーと ClapGetterViewMono を受け取る
            _bubbleHandler = bubbleHandler;
            _clapGetter = clapGetter;
        }

        public void ExecuteClap(float strength)
        {
            // ClapGetterMono から範囲内の Bubble を取得
            IEnumerable<IBubbleMono> bubbles = _clapGetter.GetBubbleMonos();


            foreach (var bubble in bubbles)
            {
                bubble.InvokeOnClap(new OnClapArg(strength));
            }
        }
    }
}