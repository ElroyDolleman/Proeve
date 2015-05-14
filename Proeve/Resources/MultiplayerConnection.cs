#region Using Statements
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using E2DFramework.Entities;

using Proeve.Entities;
using Proeve.Resources.Calculations;
#endregion

namespace Proeve.Resources
{
    class MultiplayerConnection
    {
        #region Events

        public delegate void RecieveConnectionEvent();
        public delegate void RecieveArmyEvent();
        public delegate void RecieveMoveEvent(int characterIndex, Point gridLocation);
        public delegate void RecieveFightEvent(int characterIndexAttacker, int characterIndexDefender);
        public delegate void RecieveEndTurnEvent();
        public delegate void RecieveResponsEvent();

        public RecieveConnectionEvent RecieveConnection;
        public RecieveArmyEvent RecieveArmy;
        public RecieveMoveEvent RecieveMove;
        public RecieveFightEvent RecieveFight;
        public RecieveEndTurnEvent RecieveEndTurn;
        public RecieveResponsEvent RecieveResponse;

        #endregion

        private enum State
        {
            None,
            Join,
            Army,
            GameLogicRecieving,
            WaitingResponse,
            ConnectionLost
        }

        private enum GameLogicData
        {
            Move,
            Fight,
            EndTurn
        }

        private State currentState, previousState;

        public bool IsWaitingForResponse { get { return currentState == State.WaitingResponse; } }
        public bool ConnectionLost { get { return currentState == State.ConnectionLost; } }

        public bool myTurn;
        public bool Connected { get; private set; }
        public readonly bool isHosting;
        public readonly string writeFile, readFile;

        private BinaryWriter writer;
        private BinaryReader reader;

        private byte writeValue, readValue;

        public MultiplayerConnection()
        {
            // The file names.
            string player1File = "data1.txt";
            string player2File = "data2.txt";

            // The values for checking if it reads something new.
            writeValue = 0;
            readValue = 0;

            if (!File.Exists(player1File))
            {
                // Player1 is the host and gets the first turn
                this.isHosting = true;
                this.myTurn = true;

                writeFile = player2File;
                readFile = player1File;

                // The host creates the files
                CreateFiles();
            }
            else
            {
                // Player 2 doesn't host and gets his turn after player1
                this.isHosting = false;
                this.myTurn = false;

                writeFile = player1File;
                readFile = player2File;
            }

            this.currentState = State.Join;
            this.previousState = State.None;

            this.Connected = false;
        }

        public void Update(GameTime gameTime)
        {
            switch (currentState)
            {
                case State.Join:
                    if (previousState != currentState)
                        if (isHosting)
                            WriteConnection();
                    previousState = currentState;

                    ReadConnection();

                    if (currentState != State.Join)
                    {
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

                    if (currentState != State.Army)
                    {
                        if (!isHosting)
                            WriteArmy();

                        RecieveArmy();
                    }
                    break;

                case State.GameLogicRecieving:
                    ReadGameLogicData();
                    break;
                case State.WaitingResponse:
                    ReadResponse();
                    break;
            }
        }

        #region Game Logic Connection

        private void ReadGameLogicData()
        {
            BeginRead();

            if (NewReadData)
            {
                GameLogicData gameLogicData = (GameLogicData)reader.ReadInt32();

                switch (gameLogicData)
                {
                    case GameLogicData.Move:
                        int charIndex = reader.ReadInt32();
                        Point gridLocation = new Point(reader.ReadInt32(), reader.ReadInt32());

                        RecieveMove(charIndex, gridLocation);
                        break;
                    case GameLogicData.Fight:
                        RecieveFight(reader.ReadInt32(), reader.ReadInt32());
                        break;
                    case GameLogicData.EndTurn:
                        myTurn = true;
                        currentState = State.None;

                        RecieveEndTurn();
                        break;
                }

                WriteResponse();
            }

            EndRead();
        }

        #region Sends A Move
        /// <summary>
        /// Sends the move that a specific character did.
        /// </summary>
        /// <param name="characterIndex">The index value of the chatacter that made a move.</param>
        /// <param name="gridLocation">The grid location where the character went.</param>
        #endregion
        public void SendMove(int characterIndex, Point gridLocation)
        {
            if (currentState == State.None)
            {
                BeginWrite();

                writer.Write((int)GameLogicData.Move);

                writer.Write(characterIndex);
                writer.Write(gridLocation.X);
                writer.Write(gridLocation.Y);

                EndWrite();

                currentState = State.WaitingResponse;
            }
            else
                throw new Exception("Wait for the other player response.");
        }

        #region Sends A Fight
        /// <summary>
        /// Sends which two characters are going to battle.
        /// </summary>
        /// <param name="attackerIndex">The index value of this player his character that attacks.</param>
        /// <param name="defenderIndex">The index value of the opponent his character that is going to be attacked.</param>
        #endregion
        public void SendFight(int attackerIndex, int defenderIndex)
        {
            if (currentState == State.None)
            {
                BeginWrite();

                writer.Write((int)GameLogicData.Fight);

                writer.Write(attackerIndex);
                writer.Write(defenderIndex);

                EndWrite();

                currentState = State.WaitingResponse;
            }
            else
                throw new Exception("Wait for the other player response.");
        }

        #region Sending End Of Turn
        /// <summary>
        /// Sending that this player ends his turn.
        /// </summary>
        #endregion
        public void SendEndTurn()
        {
            if (currentState == State.None)
            {
                myTurn = false;

                BeginWrite();
                writer.Write((int)GameLogicData.EndTurn);
                EndWrite();

                currentState = State.WaitingResponse;
            }
            else if (currentState == State.WaitingResponse)
                throw new Exception("Wait for the other player response.");
            else
                throw new Exception("State is still in " + currentState + " State");
        }

        private void ReadResponse()
        {
            BeginRead();

            if (NewReadData)
            {
                if (myTurn)
                    currentState = State.None;
                else
                    currentState = State.GameLogicRecieving;
            }


            EndRead();
        }

        private void WriteResponse()
        {
            BeginWrite();

            writer.Write(true);

            EndWrite();
        }

        #endregion

        #region Connection

        private void ReadConnection()
        {
            BeginRead();
            if (NewReadData)
            {
                Connected = reader.ReadBoolean();
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

            if (NewReadData)
            {
                Armies.opponentArmy = new List<Character>();

                for (int i = 0; i < Armies.ARMY_AMOUNT; i++)
                {
                    Character character = new Character(ArtAssets.EnemyChip);

                    Vector2 newPosition;
                    Point gridPosition = Grid.ToGridLocation((new Vector2(reader.ReadInt32(), reader.ReadInt32())).ToPoint(), Globals.GridLocation, Globals.TileDimensions);

                    newPosition.X = ((Globals.GRID_WIDTH - 1) - gridPosition.X) * Globals.TILE_WIDTH + Globals.GridLocation.X;
                    newPosition.Y = ((Globals.GRID_HEIGHT - 1) - gridPosition.Y) * Globals.TILE_HEIGHT + Globals.GridLocation.Y;
                    character.position = newPosition;

                    character.hp = reader.ReadInt32();
                    character.move = reader.ReadInt32();

                    character.special = (Character.Special)reader.ReadInt32();
                    character.weapon = (Character.Weapon)reader.ReadInt32();
                    character.rank = (Character.Rank)reader.ReadInt32();

                    Armies.opponentArmy.Add(character);
                }

                if (myTurn)
                    currentState = State.None;
                else
                    currentState = State.GameLogicRecieving;
            }

            EndRead();
        }

        private void WriteArmy()
        {
            BeginWrite();

            for (int i = 0; i < Armies.ARMY_AMOUNT; i++)
            {
                Character character = Armies.army[i];

                writer.Write((int)character.position.X);
                writer.Write((int)character.position.Y);

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
            while (!succes)
            {
                try
                {
                    FileStream stream = new FileStream(readFile, FileMode.Open);
                    reader = new BinaryReader(stream);

                    succes = true;
                }
                catch
                {
                    if (!File.Exists(readFile))
                    {
                        currentState = State.ConnectionLost;
                        break;
                    }
                }
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
                catch
                {
                    if (!File.Exists(writeFile))
                    {
                        currentState = State.ConnectionLost;
                        break;
                    }
                }
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
                if (!ConnectionLost)
                {
                    byte newReadValue = reader.ReadByte();

                    if (newReadValue != readValue)
                    {
                        readValue = newReadValue;
                        return true;
                    }
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