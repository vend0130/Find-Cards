using Runtime.View;
using UnityEngine;

namespace Runtime.Data
{
    [CreateAssetMenu(menuName = "Data/" + nameof(CardsData), fileName = nameof(CardsData), order = 1)]
    public class CardsData : ScriptableObject
    {
        [field: SerializeField] public CardView BasePrefab { get; private set; }
        [field: SerializeField] public CardData[] Cards { get; private set; }
    }
}