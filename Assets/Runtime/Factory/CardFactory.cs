using System.Collections.Generic;
using Runtime.Data;
using Runtime.Enums;
using Runtime.Interface;
using Runtime.View;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Factory
{
    public class CardFactory : IPoolable
    {
        private readonly GameData _gameData;
        private readonly CardsData _cardsData;
        private readonly List<CardView> _allCards = new();
        private readonly List<CardView> _cardsOnScene = new();

        public CardFactory(GameData gameData, CardsData cardsData)
        {
            _gameData = gameData;
            _cardsData = cardsData;
        }

        public void CreateCards()
        {
            for (int i = 0; i < _cardsData.Cards.Length; i++)
            {
                var cardData = _cardsData.BasePrefab;

                var card = Object.Instantiate(cardData).GetComponent<CardView>();
                card.gameObject.SetActive(false);
                card.ChangeSide(SideType.BackSide);
                _allCards.Add(card);
            }
        }

        public void SpawnCards(int total)
        {
            Assert.IsTrue(total <= _allCards.Count, "not correct total card for spawn");

            _cardsOnScene.Clear();
            List<CardData> cardFrontForUse = new List<CardData>(_cardsData.Cards);

            for (int i = 0; i < total; i++)
            {
                var cardData = cardFrontForUse[Random.Range(0, cardFrontForUse.Count)];

                _cardsOnScene.Add(_allCards[i]);
                ChangeSettingsCard(_allCards[i], cardData);
                _allCards[i].transform.localPosition = GetPosition(total, i);

                cardFrontForUse.Remove(cardData);
            }

            for (int i = total; i < _allCards.Count; i++)
            {
                _allCards[i].gameObject.SetActive(false);
            }
        }

        private void ChangeSettingsCard(CardView card, CardData cardData)
        {
            card.ChangeSide(SideType.BackSide);
            card.ChangeCardInfo(cardData.Sprite, cardData.CardInfo);
            card.name = cardData.CardInfo.CardNameWithoutSpace;
            card.gameObject.SetActive(true);
        }

        public IEnumerable<CardView> GetAllCards() =>
            _allCards;

        public IEnumerable<CardView> GetCardsOnScene() =>
            _cardsOnScene;

        private Vector2 GetPosition(int total, int index)
        {
            Vector2 position = Vector2.zero;
            float firstCardPosition = -(total - 1) / 2f * _gameData.DistanceBetweenCards;
            position.x = firstCardPosition + index * _gameData.DistanceBetweenCards;

            return position;
        }
    }
}