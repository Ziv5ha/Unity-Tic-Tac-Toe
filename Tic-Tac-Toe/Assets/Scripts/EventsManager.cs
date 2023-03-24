using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// The classes themselves can have events. I would like to hear what is the reasoning behind separating this into a static EventsManager class.
public static class EventsManager
{
    public static Action<int> OnPlayTurn;
    public static Action OnPlayAgain;
}
