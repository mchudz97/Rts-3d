using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IUnit
    {

        void Attack(GameObject target);
        void MoveTo(Vector3 place);
        void TakeDamageFromTower(GameObject from);
        void TakeDamageFromUnit(GameObject from);

    }
}
