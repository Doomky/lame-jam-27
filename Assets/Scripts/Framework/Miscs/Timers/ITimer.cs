using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimer
{
    bool IsRunning();
    bool IsTriggered();
    void Trigger();
    void Reset();
    float TimeLeftToTrigger();
}
