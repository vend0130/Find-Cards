using Runtime.Data;
using Runtime.Enums;
using Runtime.View;
using UnityEngine;

namespace Runtime.Model
{
    public class GameStateModel
    {
        public CardView ActiveCard { get; private set; }
        public GameState GameState { get; private set; }
        public CardInfo TargetCard { get; private set; }
        public int TotalCardsOnScene { get; private set; }
        public int Iteration { get; private set; } = 1;
        public int Score { get; private set; } = 0;

        private readonly GameData _gameData;
        private readonly CardsData _cardsData;

        public GameStateModel(GameData gameData, CardsData cardsData)
        {
            _gameData = gameData;
            _cardsData = cardsData;
        }

        public void NextIteration() =>
            Iteration++;

        public void IncreaseScore() =>
            Score++;

        public void ReduceScore()
        {
            Score--;

            if (Score < 0)
                Score = 0;
        }

        public void ChangeActiveCard(CardView activeCard) =>
            ActiveCard = activeCard;

        public void ChangeGameState(GameState gameState)
        {
            if (GameState == GameState.EndGame && gameState != GameState.Remember)
                return;

            GameState = gameState;
        }

        public void ChangeCardInfo(CardInfo targetCard) =>
            TargetCard = targetCard;

        public void ChangeTotalCardsOnScene() =>
            TotalCardsOnScene = Random.Range(_gameData.MinCardsOnScene, _gameData.MaxCardsOnScene + 1);
    }
}