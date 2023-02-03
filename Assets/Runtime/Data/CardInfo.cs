using System;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Data
{
    [Serializable]
    public struct CardInfo
    {
        [SerializeField] private TypeCard _typeCard;
        [SerializeField] private Suits _suits;

        public string CardName => _typeCard + (_suits == Suits.None ? "" : $" Of {_suits}");
        public string CardNameWithoutSpace => CardName.Replace(" ", "");

        public CardInfo(TypeCard typeCard, Suits suits)
        {
            _typeCard = typeCard;
            _suits = suits;
        }
    }
}