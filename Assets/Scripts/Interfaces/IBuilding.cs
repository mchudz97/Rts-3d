using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    interface IBuilding
    {
        bool IsSurfaceAvailable();
        void Build(Vector3 origin);
        void TakeDamageFromUnit(GameObject unit);
        void TakeDamageFromTower(GameObject building);

    }
}
