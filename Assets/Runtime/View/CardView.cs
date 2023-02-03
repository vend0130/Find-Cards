using System;
using DG.Tweening;
using Runtime.Data;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.View
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CardView : ClickHandler
    {
        [SerializeField] private BoxCollider2D _collider;

        [SerializeField] private SpriteRenderer _frontSide;
        [SerializeField] private GameObject _backSide;

        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private Vector3 _activeSideScale = Vector3.one;
        [SerializeField] private Vector3 _inactiveSideScale = new Vector3(0, 1, 1);
        [SerializeField] private float _duration = .2f;

        public event Action<CardView> EndTurnsOverHandle;
        public CardInfo CardInfo { get; private set; }
        public bool Found { get; private set; }

        private SideType _side;
        private Sequence _sequence;
        private GameObject _activeSide;
        private GameObject _inactiveSide;

        public void Init() =>
            InitCardView(this);

        public void ChangeSide(SideType sideType)
        {
            Found = false;
            _side = sideType;

            switch (_side)
            {
                case SideType.FrontSide:
                    ResetCard(_frontSide.gameObject, _backSide);
                    break;
                case SideType.BackSide:
                    ResetCard(_backSide, _frontSide.gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ChangeCardInfo(Sprite sprite, CardInfo cardInfo)
        {
            _frontSide.sprite = sprite;
            CardInfo = cardInfo;
        }

        public void TurnOver(SideType sideType, bool doubleTurn = false)
        {
            if (_side == sideType)
                return;

            _side = doubleTurn
                ? _side
                : _side == SideType.BackSide
                    ? SideType.FrontSide
                    : SideType.BackSide;

            PlayTurnOver(doubleTurn);
        }

        public void ChangeFound() =>
            Found = true;

        private void PlayTurnOver(bool doubleTurn = false)
        {
            InitSequence();

            _collider.enabled = false;

            _sequence.Append(_activeSide.transform.DOScaleX(_inactiveSideScale.x, _duration));
            _sequence.AppendCallback(() => _activeSide.SetActive(false));
            _sequence.AppendCallback(() => _inactiveSide.SetActive(true));
            _sequence.Append(_inactiveSide.transform.DOScaleX(_activeSideScale.x, _duration));

            _sequence.OnComplete(() => EndTurnOver(doubleTurn));

            _sequence.Play();
        }

        private void EndTurnOver(bool doubleTurn)
        {
            (_activeSide, _inactiveSide) = (_inactiveSide, _activeSide);
            if (doubleTurn)
            {
                PlayTurnOver();
                return;
            }

            _collider.enabled = true;
            EndTurnsOverHandle?.Invoke(this);
        }

        private void InitSequence()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.SetEase(_ease);
        }

        private void ResetCard(GameObject activeSide, GameObject inactiveSide)
        {
            _activeSide = activeSide;
            ChangeSide(_activeSide, true, _activeSideScale);

            _inactiveSide = inactiveSide;
            ChangeSide(_inactiveSide, false, _inactiveSideScale);
        }

        private void ChangeSide(GameObject side, bool value, Vector3 scale)
        {
            side.SetActive(value);
            side.transform.localScale = scale;
        }
    }
}