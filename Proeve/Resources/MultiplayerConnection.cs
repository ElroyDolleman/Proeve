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

        public delegate void RecieveConnectionEvent(bool isHosting);
        public delegate void RecieveArmyEvent(List<Character> army);

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

            if (!File.Exists(player1File))
            {
                // Player 1
                this.isHosting = true;

                writeFile = player2File;
                readFile = player1File;

                //CreateFiles();
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

        }

        #region Connection



        #endregion

        #region Army

        private void ReadArmy()
        {
            BeginRead();

            

            EndRead();
        }

        private void WriteArmy()
        {

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
