using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODManager : SingletonMonobehaviour<FMODManager>
{
    [field: Header("Ambience")]

    [field: SerializeField] public EventReference Amnbience { get; private set; }

    [field: Header("Music")]

    [field: SerializeField] public EventReference Music { get; private set; }

    [field: Header("Player")]

    [field: SerializeField] public EventReference PlayerStepSound { get; private set; }
    [field: SerializeField] public EventReference PlayerStepSoundOnWater { get; private set; }

    [field: Header("Enemy")]


    [field: SerializeField] public EventReference DefIdle { get; private set; }
    [field: SerializeField] public EventReference DefStepSound { get; private set; }
    [field: SerializeField] public EventReference DefStepSoundOnWater { get; private set; }

    [field: SerializeField] public EventReference BlindIdle { get; private set; }
    [field: SerializeField] public EventReference BlindStepSound { get; private set; }
    [field: SerializeField] public EventReference BlindStepSoundOnWater { get; private set; }

    [field: Header("UI")]

    [field: SerializeField] public EventReference ButtonPress { get; private set; }

    [field: SerializeField] public EventReference PlaySound { get; private set; }
    [field: SerializeField] public EventReference GameOverSound { get; private set; }

    protected override void Awake()
    {
        base.Awake();

    }
}