using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Proeve.Entities;

namespace Proeve.Resources
{
    class Armies
    {
        public enum CharacterRanks
        {
            Marshal,
            General,
            Majoor,
            Captain,
            Special,
            Bomb
        }

        public enum Army
        {
            Normal,
            Tiki
        }

        public static List<Character> army;
        public static List<Character> opponentArmy;

        public static List<List<Character>> characters;

        public static Character GetCharacter(CharacterRanks rank, Army army = Army.Normal)
        {
            return characters[(int)rank][(int)army].Clone();
        }

        public static void SetUpCharacters()
        {
            characters = new List<List<Character>>();

            for (int i = 0; i < 6; i++)
                characters.Add(new List<Character>());

            characters[(int)CharacterRanks.Marshal].Add(new Character(ArtAssets.MarshalChip, 5, 2));
            characters[(int)CharacterRanks.General].Add(new Character(ArtAssets.GeneralChip, 4, 2));
            characters[(int)CharacterRanks.Majoor].Add(new Character(ArtAssets.MajoorChip, 3, 2));
            characters[(int)CharacterRanks.Captain].Add(new Character(ArtAssets.CaptainChip, 2, 2));
            characters[(int)CharacterRanks.Special].Add(new Character(ArtAssets.MinorChip, 1, 2, Character.Special.Minor));
            characters[(int)CharacterRanks.Bomb].Add(new Character(ArtAssets.BombChip, 0, 0, Character.Special.Bomb));
        }
    }
}
