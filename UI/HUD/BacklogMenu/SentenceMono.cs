using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.HUD.BacklogMenu
{
    public class SentenceMono : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI author;
        [SerializeField] private TextMeshProUGUI text;
        public string Author => author.text;
        public string Text => text.text;
        public event Action<SentenceMono> OnReturn;

        [Inject]
        private void Construct()
        {
            
        }
        
        public void SetText(string newAuthor, string newText)
        {
            author.text = newAuthor;
            text.text = newText;
        }

        public void Reset()
        {
            author.text = "";
            text.text = "";
        }
    }
}