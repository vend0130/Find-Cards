using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.View
{
    public class ClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public event Action<CardView> ClickHandle;

        private CardView _cardView;

        protected void InitCardView(CardView cardView) =>
            _cardView = cardView;

        public void OnPointerClick(PointerEventData eventData) =>
            ClickHandle?.Invoke(_cardView);
    }
}