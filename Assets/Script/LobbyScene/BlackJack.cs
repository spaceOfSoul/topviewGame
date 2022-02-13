using UnityEngine;
public class BlackJack
{
        public const string PLAYER_LIVES = "PlayerLives";
        public const string PLAYER_READY = "IsPlayerReady";
        public const string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";

    public static Color GetColor(int colorChoice)
    {
        switch (colorChoice)
        {
            case 0: return Color.blue;
            case 1: return Color.green;
            case 2: return Color.red;
            case 3: return Color.yellow;
            case 4: return Color.cyan;
            case 5: return Color.magenta;
            case 6: return Color.white;
            case 7: return Color.grey;
        }

        return Color.black;
    }
}