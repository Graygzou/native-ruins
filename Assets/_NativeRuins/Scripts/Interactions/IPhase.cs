using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhase {

    void SetupCutScene();

    void TriggerActions();

    void Interrupt();

}
