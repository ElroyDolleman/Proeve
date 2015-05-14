using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.Entities;
using Proeve.Resources;

namespace Proeve.States
{
    class FightState : State
    {
        Character character;
        Character enemyCharacter;
        bool myAttackTurn;
        int lastDamage;

        float timer;

        public FightState()
        {

        }

        public override void Initialize()
        {
            
        }

        public void SetUnits(Character attacker, Character defender)
        {
            this.myAttackTurn = Globals.multiplayerConnection.myTurn;

            if (myAttackTurn) {
                this.character = attacker;
                this.enemyCharacter = defender;
                Attack();
            }
            else {
                this.character = defender;
                this.enemyCharacter = attacker;
                Defend();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!character.IsDead && !enemyCharacter.IsDead)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timer > 2000)
                {
                    if (myAttackTurn)
                        Attack();
                    else
                        Defend();

                    myAttackTurn = !myAttackTurn;
                    timer = 0;
                }
            }
            else
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timer > 2000)
                    StateManager.RemoveState();
            }
        }

        private void Attack()
        {
            lastDamage = -WeaponDamage(character.weapon, enemyCharacter.weapon);

            if (character.special == Character.Special.Bomb)
                lastDamage = -enemyCharacter.hp;

            enemyCharacter.hp += lastDamage;
        }

        private void Defend()
        {
            lastDamage = -WeaponDamage(enemyCharacter.weapon, character.weapon);

            if (enemyCharacter.special == Character.Special.Bomb)
                lastDamage = -character.hp;

            character.hp += lastDamage;
        }

        private int WeaponDamage(Character.Weapon attackWeapon, Character.Weapon defendWeapon)
        {
            if (character.special == Character.Special.None && enemyCharacter.special == Character.Special.None)
            {
                switch (attackWeapon)
                {
                    default:
                    case Character.Weapon.Axe:
                        if (defendWeapon == Character.Weapon.Shield)
                            return 2;
                        else
                            return 1;
                    case Character.Weapon.Shield:
                        if (defendWeapon == Character.Weapon.Sword)
                            return 2;
                        else
                            return 1;
                    case Character.Weapon.Sword:
                        if (defendWeapon == Character.Weapon.Axe)
                            return 2;
                        else
                            return 1;
                }
            }
            else
            {
                if (myAttackTurn)
                {
                    if (enemyCharacter.special == Character.Special.Bomb)
                    {
                        if (character.special == Character.Special.Minor)
                            return enemyCharacter.hp;
                        else
                            return 0;
                    }

                    if (character.special == Character.Special.Spy)
                        if (enemyCharacter.rank == Character.Rank.Marshal)
                            return enemyCharacter.hp;

                    return 1;
                }
                else
                {
                    if (character.special == Character.Special.Bomb)
                    {
                        if (enemyCharacter.special == Character.Special.Minor)
                            return character.hp;
                        else
                            return 0;
                    }

                    if (enemyCharacter.special == Character.Special.Spy)
                        if (character.rank == Character.Rank.Marshal)
                            return character.hp;

                    return 1;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            StateManager.GetState(2).Draw(spriteBatch);

            spriteBatch.DrawRectangle(Main.WindowRectangle, Color.Black * .5f);

            spriteBatch.DrawDebugText("Rank: " + character.rank, 100, 16, Color.White);
            spriteBatch.DrawDebugText("Weapon: " + character.weapon, 100, 32, Color.White);
            spriteBatch.DrawDebugText("Level: " + character.Level, 100, 48, Color.White);
            spriteBatch.DrawDebugText("Steps: " + character.move, 100, 64, Color.White);
            spriteBatch.DrawDebugText("HP: " + character.hp, 100, 80, Color.White);

            spriteBatch.DrawDebugText("Rank: " + enemyCharacter.rank, 500, 16, Color.White);
            spriteBatch.DrawDebugText("Weapon: " + enemyCharacter.weapon, 500, 32, Color.White);
            spriteBatch.DrawDebugText("Level: " + enemyCharacter.Level, 500, 48, Color.White);
            spriteBatch.DrawDebugText("Steps: " + enemyCharacter.move, 500, 64, Color.White);
            spriteBatch.DrawDebugText("HP: " + enemyCharacter.hp, 500, 80, Color.White);

            if (character.IsDead)
                spriteBatch.DrawDebugText("Lost", 100, 120, Color.Pink);
            else if (enemyCharacter.IsDead)
                spriteBatch.DrawDebugText("Win", 100, 120, Color.Pink);
            else if (myAttackTurn)
                spriteBatch.DrawDebugText("Damage: " + lastDamage, 100, 120, Color.Pink);
            else
                spriteBatch.DrawDebugText("Damage: " + lastDamage, 500, 120, Color.Pink);
        }
    }
}
