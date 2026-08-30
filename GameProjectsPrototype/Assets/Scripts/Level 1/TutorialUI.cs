using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TutorialUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _currentKnownWords;
        [SerializeField] private TextMeshProUGUI _hints;

        [Header("Current Words")]
        [SerializeField]
        private List<string> _currentWords = new List<string>
        {
            "Mass",
            "Lighten",
            "Grow",
            "Shrink"
        };

        private void Start()
        {
            RefreshWords();
            SetHint("");
        }

        public void RefreshWords()
        {
            _currentKnownWords.text = "Current Words:\n- " + string.Join("\n- ", _currentWords);
        }

        public void SetWords(List<string> words)
        {
            _currentWords = new List<string>(words);
            RefreshWords();
        }

        public void AddWord(string word)
        {
            if (_currentWords.Contains(word))
                return;

            _currentWords.Add(word);
            RefreshWords();
        }

        public void SetHint(string hint)
        {
            _hints.text = hint;
            _hints.gameObject.SetActive(!string.IsNullOrEmpty(hint));
        }
    }
}