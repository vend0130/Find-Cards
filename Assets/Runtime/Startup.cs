using System.Collections.Generic;
using Runtime.Controller;
using Runtime.Data;
using Runtime.Factory;
using Runtime.Model;
using Runtime.View;
using UnityEngine;
using IDisposable = Runtime.Interface.IDisposable;

namespace Runtime
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private GameData _gameData;
        [SerializeField] private CardsData _cardsData;
        [SerializeField] private Timer _timer;
        [SerializeField] private UIView _uiView;

        private readonly List<IDisposable> _disposes = new();

        private void Awake()
        {
            CardFactory cardFactory = new CardFactory(_gameData, _cardsData);
            GameStateModel gameStateModel = new GameStateModel(_gameData, _cardsData);
            CardsController cardsController = new CardsController(gameStateModel, cardFactory, _uiView);
            GameStateController gameStateController =
                new GameStateController(cardsController, gameStateModel, cardFactory, _timer);

            _disposes.AddRange(new IDisposable[] { cardsController, gameStateController });

            _timer.Init(_gameData.TimeToRememberForOneCard);
            cardFactory.CreateCards();
            cardsController.Init();
            gameStateController.Init();
            gameStateController.StartGame();
        }

        private void OnDestroy()
        {
            foreach (var dispose in _disposes)
            {
                dispose.Dispose();
            }
        }
    }
}