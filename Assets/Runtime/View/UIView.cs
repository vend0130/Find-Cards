using Runtime.Data;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Runtime.View
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _findText;
        [SerializeField] private TMP_Text _iterationText;
        [SerializeField] private TMP_Text _scoreText;

        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        private void Start()
        {
            _restartButton.onClick.AddListener(Restart);
            _exitButton.onClick.AddListener(Exit);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(Restart);
            _exitButton.onClick.RemoveListener(Exit);
        }

        public void ChangeFindText(CardInfo cardInfo) =>
            _findText.text = $"Find The {cardInfo.CardName}";

        public void ResetFindText() =>
            _findText.text = "";

        public void UpdateTextIteration(int value) =>
            _iterationText.text = $"Iteration: {value}";

        public void UpdateTextScore(int value) =>
            _scoreText.text = $"Score: {value}";

        private void Restart() =>
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        private void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}