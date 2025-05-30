using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VContainer;
using Shabon.Score;
using Shabon.Bubble;

namespace Shabon.Game
{
    public class ResultButton : MonoBehaviour
    {
        private IDirtValue _dirtValue = null!;
        private IScoreValue _scoreValue = null!;
        private IBubbleCombo _bubbleCombo = null!;

        [Inject]
        public void Initialize(IDirtValue dirtValue, IScoreValue scoreValue, IBubbleCombo bubbleCombo)
        {
            _dirtValue = dirtValue;
            _scoreValue = scoreValue;
            _bubbleCombo = bubbleCombo;
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
            ResultData.SaveResults(_dirtValue.DirtNum, _scoreValue.ScoreNum, _bubbleCombo.ComboNum);
            SceneManager.LoadScene("ResultScene");
        }
    }
}
