﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructible
{
    void DoDamage(Hearts amount, Vector3 pos, Vector3 dir);

    void Destruct();
}
