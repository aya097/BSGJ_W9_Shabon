using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VContainer;
using Shabon.Score;
using Shabon.Bubble;
using Shabon.Clap;
using Shabon.Breath;


namespace Shabon.Game
{
    public class ResultButton : MonoBehaviour
    {
        private IDirtValue _dirtValue = null!;
        private IScoreValue _scoreValue = null!;
        private IBubbleCombo _bubbleCombo = null!;
        private ClapModel _clapModel = null!;
        private BreathModel _breathModel = null!;

        [Inject]
        public void Initialize(
            IDirtValue dirtValue,
            IScoreValue scoreValue,
            IBubbleCombo bubbleCombo,
            ClapModel clapModel,
            BreathModel breathModel
        )
        {
            _dirtValue = dirtValue;
            _scoreValue = scoreValue;
            _bubbleCombo = bubbleCombo;
            _clapModel = clapModel;
            _breathModel = breathModel;
        }

        void Start()
        {
            Button btn = this.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(osu);
            }
            else
            {
                Debug.LogWarning("Button component not found on " + gameObject.name);
            }
        }

        void osu()
        {
            int dirtValueCountSum = _dirtValue.IncreaseCount; // 増加した回数
            ResultData.SaveResults(
                _dirtValue.DirtNum,                       // FinalDirt
                _bubbleCombo.ComboNum,                    // FinalCombo
                _clapModel.ClapCount,                     // FinalClapCount
                dirtValueCountSum,                        // DirtValueCountSum（増加回数）
                _breathModel.TotalBreathTime,             // FinalBreathTime
                _breathModel.TotalBreathStrength,         // Calorie計算用
                0                                         // BossBattleTime
            );
            SceneManager.LoadScene("ResultScene");
        }
    }
}
