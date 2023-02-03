using UnityEngine;

namespace Runtime.Data
{
    [CreateAssetMenu(menuName = "Data/" + nameof(GameData), fileName = nameof(GameData), order = 0)]
    public class GameData : ScriptableObject
    {
        [field: SerializeField] public int MinCardsOnScene { get; private set; } = 3;
        [field: SerializeField] public int MaxCardsOnScene { get; private set; } = 7;

        [field: SerializeField] public float TimeToRememberForOneCard { get; private set; } = 1;
        [field: SerializeField] public float DistanceBetweenCards { get; private set; } = 1.5f;
    }
}