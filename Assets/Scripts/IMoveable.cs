using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    bool Add_Force(float mag, Vector3 force);
    Vector3 Update_Agent(float deltaTime);    
}
