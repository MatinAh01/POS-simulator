using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pos_simulator
{
    public interface IOperations
    {
        void NewTransaction();
        void DisplayAllTransaction();
    }
}