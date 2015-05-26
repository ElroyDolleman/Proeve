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

            for (int i = 0; i < 8*2; i++)
                characters.Add(new List<Character>());

            characters[(int)Character.Rank.Leader].Add(new Character(ArtAssets.MedievalLeaderChip, AnimationAssets.MedievalLeader, 5, 2, Character.Rank.Leader));
            characters[(int)Character.Rank.Leader].Add(new Character(ArtAssets.TikiLeaderChip, AnimationAssets.TikiLeader, 6, 1, Character.Rank.Leader));

            characters[(int)Character.Rank.General].Add(new Character(ArtAssets.MedievalGeneralChip, AnimationAssets.MedievalGeneral, 4, 2, Character.Rank.General));
            characters[(int)Character.Rank.General].Add(new Character(ArtAssets.TikiGeneralChip, AnimationAssets.TikiGeneral, 3, 3, Character.Rank.General));

            characters[(int)Character.Rank.Captain].Add(new Character(ArtAssets.MedievalCaptainChip, AnimationAssets.MedievalCaptain, 3, 2, Character.Rank.Captain));
            characters[(int)Character.Rank.Captain].Add(new Character(ArtAssets.TikiCaptainChip, AnimationAssets.TikiCaptain, 4, 1, Character.Rank.Captain));

            characters[(int)Character.Rank.Soldier].Add(new Character(ArtAssets.MedievalSoldierChip, AnimationAssets.MedievalSoldier, 2, 2, Character.Rank.Soldier));
            characters[(int)Character.Rank.Soldier].Add(new Character(ArtAssets.TikiSoldierChip, AnimationAssets.TikiSoldier, 3, 1, Character.Rank.Soldier));

            characters[(int)Character.Rank.Spy].Add(new Character(ArtAssets.MedievalSpyChip, AnimationAssets.MedievalSpy, 1, 2, Character.Rank.Spy, Character.Army.Normal, Character.Special.Spy));
            characters[(int)Character.Rank.Spy].Add(new Character(ArtAssets.TikiSpyChip, AnimationAssets.TikiSpy, 1, 2, Character.Rank.Spy, Character.Army.Normal, Character.Special.Spy));

            characters[(int)Character.Rank.Miner].Add(new Character(ArtAssets.MedievalMinerChip, AnimationAssets.MedievalMiner, 1, 2, Character.Rank.Miner, Character.Army.Normal, Character.Special.Miner));
            characters[(int)Character.Rank.Miner].Add(new Character(ArtAssets.TikiMinerChip, AnimationAssets.TikiMiner, 2, 1, Character.Rank.Miner, Character.Army.Normal, Character.Special.Miner));

            characters[(int)Character.Rank.Healer].Add(new Character(ArtAssets.MedievalHealerChip, AnimationAssets.MedievalHealer, 1, 2, Character.Rank.Healer, Character.Army.Normal, Character.Special.Healer));
            characters[(int)Character.Rank.Healer].Add(new Character(ArtAssets.TikiHealerChip, AnimationAssets.TikiHealer, 2, 1, Character.Rank.Healer, Character.Army.Normal, Character.Special.Healer));

            characters[(int)Character.Rank.Bomb].Add(new Character(ArtAssets.MedievalBombChip, AnimationAssets.MedievalBomb, 1, 1, Character.Rank.Bomb, Character.Army.Normal, Character.Special.Bomb));
            characters[(int)Character.Rank.Bomb].Add(new Character(ArtAssets.TikiBombChip, AnimationAssets.TikiBomb, 1, 1, Character.Rank.Bomb, Character.Army.Normal, Character.Special.Bomb));
        }
    }
}
