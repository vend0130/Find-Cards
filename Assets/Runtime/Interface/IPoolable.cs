using System.Collections.Generic;
using Runtime.View;

namespace Runtime.Interface
{
    public interface IPoolable
    {
        IEnumerable<CardView> GetAllCards();
        IEnumerable<CardView> GetCardsOnScene();
    }
}