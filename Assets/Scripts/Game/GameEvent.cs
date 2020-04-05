using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent 
{
    public const int GAME_SYNC_TRANS = 0;
    public const int GAME_PLAYER_SPAWN=1;
    public const int GAME_PLAYER_ADD = 2;
    public const int GAME_PLAYER_EXIT = 3;
    public const int GAME_UPLOAD_TRANS = 4;
    public const int GAME_SYNC_HP = 5;
    public const int GAME_SYNC_HG = 6;
    public const int GAME_SYNC_KILL = 7;
    public const int GAME_SYNC_INFO = 8;
    public const int GAME_REDUCE_HP = 9;
    public const int GAME_AUGMENT_HP = 10;
    public const int GAME_REDUCE_HG = 11;
    public const int GAME_AUGMENT_HG = 12;
    public const int GAME_PLAYER_DEATH=13;
    public const int GAME_CREAT_PROPS = 14;
    public const int GAME_REMOVE_PROPS = 15;
    public const int GAME_REMOVE_PROPS_SEND = 16;
    public const int GAME_DOSKILL = 17;
    public const int GAME_STOPSKILL = 18;
   // public const int GAME_DOATTACK = 18;
}
