#region Using Statements
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using E2DFramework.Entities;

using Proeve.Entities;
#endregion

namespace Proeve.Resources
{
    class MultiplayerConnection
    {
        #region Events

        public delegate void RecieveConnectionEvent();
        public delegate void RecieveArmyEvent();

        public RecieveConnectionEvent RecieveConnection;
        public RecieveArmyEvent RecieveArmy;

        #endregion

        private enum State
        {
            None,
            Join,
            Army,
            Ready,
            Game
        }

        private State currentState, previousState;

        public bool connected;
        public readonly bool isHosting;
        public readonly string writeFile, readFile;

        private BinaryWriter writer;
        private BinaryReader reader;

        private byte writeValue, readValue;

        public MultiplayerConnection()
        {
            string player1File = "data1.txt";
            string player2File = "data2.txt";

            writeValue = 0;
            readValue = 0;

            if (!File.Exists(player1File))
            {
                // Player 1
                this.isHosting = true;

                writeFile = player2File;
                readFile = player1File;

                CreateFiles();
            }
            else
            {
                // Player 2
                this.isHosting = false;

                writeFile = player1File;
                readFile = player2File;
            }

            this.currentState = State.Join;
            this.previousState = State.None;

            this.connected = false;
        }

        public void Update(GameTime gameTime)
        {
            switch(currentState)
            {
                case State.Join:
                    if (previousState != currentState)
                        if (isHosting)
                            WriteConnection();
                    previousState = currentState;

                    ReadConnection();

                    if (currentState != State.Join) {
                        if (!isHosting)
                            WriteConnection();

                        RecieveConnection();
                    }
                    break;

                case State.Army:
                    if (previousState != currentState)
                        if (isHosting)
                            WriteArmy();
                    previousState = currentState;

                    ReadArmy();

                    if (currentState != State.Army) { 
                        if (!isHosting)
                            WriteArmy();

                        RecieveArmy();
                    }
                    break;
            }
        }

        #region Connection

        private void ReadConnection()
        {
            BeginRead();
            if (NewReadData)
            {
                connected = reader.ReadBoolean();
                currentState = State.Army;
            }
            EndRead();
        }

        private void WriteConnection()
        {
            BeginWrite();
            writer.Write(true);
            EndWrite();
        }

        #endregion

        #region Army

        private void ReadArmy()
        {
            BeginRead();

            Console.WriteLine("readValue before {0}", readValue);

            if (NewReadData)
            {
                Console.WriteLine("readValue after {0}", readValue);

                Armies.opponentArmy = new List<Character>();

                for (int i = 0; i < Armies.ARMY_AMOUNT; i++)
                {
                    Character character = new Character();

                    character.Position = new Vector2(reader.ReadInt32(), reader.ReadInt32());

                    character.hp = reader.ReadInt32();
                    character.move = reader.ReadInt32();

                    character.special = (Character.Special)reader.ReadInt32();
                    character.weapon = (Character.Weapon)reader.ReadInt32();
                    character.rank = (Character.Rank)reader.ReadInt32();

                    Armies.opponentArmy.Add(character);
                }

                currentState = State.None;
            }

            EndRead();
        }

        private void WriteArmy()
        {
            BeginWrite();

            for (int i = 0; i < Armies.ARMY_AMOUNT; i++)
            {
                Character character = Armies.army[i];

                writer.Write((int)character.Position.X);
                writer.Write((int)character.Position.Y);

                writer.Write(character.hp);
                writer.Write(character.move);

                writer.Write((int)character.special);
                writer.Write((int)character.weapon);
                writer.Write((int)character.rank);
            }

            EndWrite();
        }

        #endregion

        #region Read and Write Methods

        private void BeginRead()
        {
            bool succes = false;
            while(!succes)
            {
                try
                {
                    FileStream stream = new FileStream(readFile, FileMode.Open);
                    reader = new BinaryReader(stream);

                    succes = true;
                }
                catch { }
            }
        }

        private void EndRead()
        {
            reader.Close();
        }

        private void BeginWrite()
        {
            bool succes = false;
            while (!succes)
            {
                try
                {
                    FileStream stream = new FileStream(writeFile, FileMode.Create);
                    writer = new BinaryWriter(stream);

                    if (writeValue < byte.MaxValue)
                        writeValue++;
                    else
                        writeValue = 0;

                    writer.Write(writeValue);

                    succes = true;
                }
                catch { }
            }
        }

        private void EndWrite()
        {
            writer.Close();
        }

        private bool NewReadData
        {
            get
            {
                byte newReadValue = reader.ReadByte();

                if (newReadValue != readValue)
                {
                    readValue = newReadValue;
                    return true;
                }

                return false;
            }
        }

        private void CreateFiles()
        {
            bool succes = false;
            while (!succes)
            {
                try
                {
                    FileStream stream = new FileStream(writeFile, FileMode.Create);
                    writer = new BinaryWriter(stream);
                    writer.Close();

                    stream = new FileStream(readFile, FileMode.Create);
                    writer = new BinaryWriter(stream);
                    writer.Write(readValue);
                    writer.Close();

                    succes = true;
                }
                catch { }
            }
        }

        #endregion
    }
}
