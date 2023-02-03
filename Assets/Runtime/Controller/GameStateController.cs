using Runtime.Enums;
using Runtime.Factory;
using Runtime.Interface;
using Runtime.Model;

namespace Runtime.Controller
{
    public class GameStateController : IDisposable
    {
        private readonly CardsController _cardsController;
        private readonly GameStateModel _gameStateModel;
        private readonly CardFactory _cardFactory;
        private readonly Timer _timer;

        public GameStateController(CardsController cardsController, GameStateModel gameStateModel,
            CardFactory cardFactory, Timer timer)
        {
            _cardsController = cardsController;
            _gameStateModel = gameStateModel;
            _cardFactory = cardFactory;
            _timer = timer;
        }

        public void Init()
        {
            _timer.EndTimerHandle += EndTimer;
            _cardsController.EndGameHandle += StartGame;
            _gameStateModel.ChangeTotalCardsOnScene();
        }

        public void Dispose()
        {
            _timer.EndTimerHandle -= EndTimer;
            _cardsController.EndGameHandle -= StartGame;
        }
        
        public void StartGame()
        {
            _gameStateModel.ChangeGameState(GameState.Remember);
            _cardFactory.SpawnCards(_gameStateModel.TotalCardsOnScene);
            _cardsController.AllCardsTurnOver(SideType.FrontSide);
            _timer.StartTimer(SideType.BackSide, _gameStateModel.TotalCardsOnScene);
        }

        private void EndTimer(SideType sideType)
        {
            _gameStateModel.ChangeGameState(GameState.TurnOverCard);
            _cardsController.AllCardsTurnOver(sideType);

            if (sideType == SideType.BackSide)
            {
                _cardsController.UpdateTargetCard();
            }
        }
    }
}