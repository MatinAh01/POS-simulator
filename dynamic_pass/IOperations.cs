using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dynamic_pass
{
    public interface IOperations
    {
        void DisplayAllCards();
        Task CreateDynamicPass();
        void CreateNewCreditCard();
        void DeleteCard();
        void EditCard();
    }
}