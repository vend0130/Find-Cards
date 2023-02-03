using System;
using UnityEngine;

namespace Runtime.Data
{
    [Serializable]
    public class CardData
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public CardInfo CardInfo { get; private set; }
    }
}