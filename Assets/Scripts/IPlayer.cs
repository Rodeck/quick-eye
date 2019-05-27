using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public interface IPlayer
    {
        void ObjectPicked(Pickable picked);

        void FaultyObjectPicked(Pickable picked);

        void ReturnItem(Pickable picked);

        string GetPoints();
    }
}
