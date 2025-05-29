using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Shabon.Game
{
    public class ResultButton : MonoBehaviour
    {
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
            SceneManager.LoadScene("ResultScene");
        }
    }

}
