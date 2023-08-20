using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODManager : SingletonMonobehaviour<FMODManager>
{
    [field: Header("Ambience")]

    [field: SerializeField] public EventReference Ambience { get; private set; }
    [field: SerializeField] public EventReference Rain { get; private set; }

    [field: Header("Music")]

    [field: SerializeField] public EventReference Music { get; private set; }

    [field: Header("Enverenment")]
    [field: SerializeField] public EventReference RandomSFX { get; private set; }
    [field: SerializeField] public EventReference Thunder { get; private set; }
    [field: SerializeField] public EventReference Exit { get; private set; }


    [field: Header("Player")]

    [field: SerializeField] public EventReference PlayerStepSound { get; private set; }
    [field: SerializeField] public EventReference PlayerStepSoundOnWater { get; private set; }
    [field: SerializeField] public EventReference DieSound { get; private set; }

    [field: Header("Enemy")]


    [field: SerializeField] public EventReference DefIdle { get; private set; }
    [field: SerializeField] public EventReference DefJump { get; private set; }

    [field: SerializeField] public EventReference BlindIdle { get; private set; }
    [field: SerializeField] public EventReference BlindJump { get; private set; }

    [field: Header("UI")]

    [field: SerializeField] public EventReference ButtonPress { get; private set; }

    protected override void Awake()
    {
        base.Awake();

    }
}