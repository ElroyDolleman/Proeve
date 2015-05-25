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

            characters[(int)Character.Rank.Leader].Add(new Character(ArtAssets.MedievalMarshalChip, AnimationAssets.MedievalMarshal, 5, 2, Character.Rank.Leader));
            characters[(int)Character.Rank.General].Add(new Character(ArtAssets.MedievalGeneralChip, AnimationAssets.MedievalGeneral, 4, 2, Character.Rank.General));
            characters[(int)Character.Rank.Captain].Add(new Character(ArtAssets.MedievalMajoorChip, AnimationAssets.MedievalMajoor, 3, 2, Character.Rank.Captain));
            characters[(int)Character.Rank.Soldier].Add(new Character(ArtAssets.MedievalCaptainChip, AnimationAssets.MedievalCaptain, 2, 2, Character.Rank.Soldier));
            characters[(int)Character.Rank.Special].Add(new Character(ArtAssets.MedievalMinerChip, AnimationAssets.MedievalMiner, 1, 2, Character.Rank.Special, Character.Army.Normal, Character.Special.Miner));
            characters[(int)Character.Rank.Bomb].Add(new Character(ArtAssets.MedievalBombChip, AnimationAssets.MedievalBomb, 1, 1, Character.Rank.Bomb, Character.Army.Normal, Character.Special.Bomb));
        }
    }
}
