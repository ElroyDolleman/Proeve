using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Proeve.Entities;

namespace Proeve.Resources
{
    class Armies
    {
        public const int ARMY_AMOUNT = 10;

        public static List<Character> army;
        public static List<Character> opponentArmy;

        public static List<List<Character>> characters;

        public static Character GetCharacter(Character.Rank rank, Character.Army army = Character.Army.Normal)
        {
            return characters[(int)rank][(int)army].Clone();
        }

        public static void SetUpCharacters()
        {
            characters = new List<List<Character>>();

            for (int i = 0; i < 6; i++)
                characters.Add(new List<Character>());

            characters[(int)Character.Rank.Marshal].Add(new Character(ArtAssets.MarshalChip, 5, 2, Character.Rank.Marshal));
            characters[(int)Character.Rank.General].Add(new Character(ArtAssets.GeneralChip, 4, 2, Character.Rank.General));
            characters[(int)Character.Rank.Majoor].Add(new Character(ArtAssets.MajoorChip, 3, 2, Character.Rank.Majoor));
            characters[(int)Character.Rank.Captain].Add(new Character(ArtAssets.CaptainChip, 2, 2, Character.Rank.Captain));
            characters[(int)Character.Rank.Special].Add(new Character(ArtAssets.MinorChip, 1, 2, Character.Rank.Special, Character.Army.Normal, Character.Special.Minor));
            characters[(int)Character.Rank.Bomb].Add(new Character(ArtAssets.BombChip, 0, 0, Character.Rank.Bomb, Character.Army.Normal, Character.Special.Bomb));
        }
    }
}
