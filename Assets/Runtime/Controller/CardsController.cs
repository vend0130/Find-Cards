using System;
using System.Linq;
using Runtime.Enums;
using Runtime.Interface;
using Runtime.Model;
using Runtime.View;
using Random = UnityEngine.Random;

namespace Runtime.Controller
{
    public class CardsController : Interface.IDisposable
    {
        public event Action EndGameHandle;

        private readonly GameStateModel _gameStateModel;
        private readonly IPoolable _cardsPool;
        private readonly UIView _uiView;

        public CardsController(GameStateModel gameStateModel, IPoolable cardsPool, UIView uiView)
        {
            _gameStateModel = gameStateModel;
            _cardsPool = cardsPool;
            _uiView = uiView;
        }

        public void Init()
        {
            foreach (var card in _cardsPool.GetAllCards())
            {
                card.Init();
                card.ClickHandle += ClickOnCard;
            }
            
            _uiView.UpdateTextIteration(_gameStateModel.Iteration);
        }

        public void Dispose()
        {
            foreach (var card in _cardsPool.GetAllCards())
            {
                card.ClickHandle -= ClickOnCard;
            }

            if (IsActiveCardExit())
            {
                _gameStateModel.ActiveCard.EndTurnsOverHandle -= EndTurnsOver;
            }
        }

        public void UpdateTargetCard()
        {
            var cardsArray = _cardsPool.GetCardsOnScene()
                .Take(_gameStateModel.TotalCardsOnScene).Where(card => !card.Found).ToArray();

            if (cardsArray.Length == 0)
            {
                _uiView.ResetFindText();
                _gameStateModel.ChangeGameState(GameState.EndGame);
                AllCardsTurnOver(SideType.BackSide);
                return;
            }

            var randomCard = cardsArray[Random.Range(0, cardsArray.Length)].CardInfo;

            _gameStateModel.ChangeCardInfo(randomCard);

            _uiView.ChangeFindText(randomCard);
        }

        public void AllCardsTurnOver(SideType sideType)
        {
            var cardsArray = _cardsPool.GetCardsOnScene().ToArray();

            for (int i = 0; i < cardsArray.Length; i++)
            {
                if (i == cardsArray.Length - 1)
                {
                    TurnCard(cardsArray[i]);
                }

                cardsArray[i].TurnOver(sideType);
            }
        }

        private void EndTurnsOver(CardView card)
        {
            if (_gameStateModel.GameState == GameState.EndGame)
            {
                _gameStateModel.NextIteration();
                _uiView.UpdateTextIteration(_gameStateModel.Iteration);
                EndGameHandle?.Invoke();
                return;
            }

            if (_gameStateModel.GameState != GameState.TurnOverCard)
                return;

            if (card.Found)
            {
                UpdateTargetCard();
            }

            _gameStateModel.ChangeGameState(GameState.SelectCard);
        }

        private void ClickOnCard(CardView card)
        {
            if (_gameStateModel.GameState != GameState.SelectCard || card.Found)
                return;

            bool clickOnTargetCard = card.CardInfo.Equals(_gameStateModel.TargetCard);

            if (clickOnTargetCard)
            {
                _gameStateModel.IncreaseScore();
                card.ChangeFound();
            }
            else
            {
                _gameStateModel.ReduceScore();
            }

            _uiView.UpdateTextScore(_gameStateModel.Score);

            _gameStateModel.ChangeGameState(GameState.TurnOverCard);
            TurnCard(card);
            _gameStateModel.ActiveCard.TurnOver(SideType.FrontSide, doubleTurn: !clickOnTargetCard);
        }

        private void TurnCard(CardView card)
        {
            if (IsActiveCardExit())
            {
                _gameStateModel.ActiveCard.EndTurnsOverHandle -= EndTurnsOver;
            }

            _gameStateModel.ChangeActiveCard(card);
            _gameStateModel.ActiveCard.EndTurnsOverHandle += EndTurnsOver;
        }

        private bool IsActiveCardExit() =>
            _gameStateModel.ActiveCard != null;
    }
}