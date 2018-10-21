using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructible
{
    void DoDamage(int amount, Vector2 pos, Vector2 dir);

    void Destruct();
}
