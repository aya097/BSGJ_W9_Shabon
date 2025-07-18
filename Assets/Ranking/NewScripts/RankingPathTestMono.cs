using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;

namespace Rnaking
{
    public class RankingPathTestMono : MonoBehaviour
    {
        [SerializeField] TMP_Text pathText = null!;

        void Awake()
        {
            string exePath = Process.GetCurrentProcess().MainModule.FileName;
            string appDir = Path.GetFullPath(Path.Combine(exePath, "../../../")); // .appパス
            string parentDir = Path.GetDirectoryName(appDir); // .app の親

            pathText.text = $"アプリパス: {appDir}\n親ディレクトリ: {parentDir}";
        }
    }
}
