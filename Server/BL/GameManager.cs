using Common.Interfaces;
using Common.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.BL
{
    public class GameManager
    {
        //Singleton
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }

        //Fields
        Random rand;

        //Properties
        Dictionary<string, IBoardState> _gameState;

        //Ctor
        private GameManager()
        {
            rand = new Random();
            _gameState = new Dictionary<string, IBoardState>();
        }

        //Methods
        internal Cube RollCubes()
        {
            return new Cube() { Cube1 = rand.Next(1, 7), Cube2 = rand.Next(1, 7) };
        }

        internal void InitBoard(string sender, string reciver)
        {
            string key = sender;
            BoardState newBoard = new BoardState(sender, reciver);
            _gameState.Add(key, newBoard);
        }

        internal IBoardState GetBoardState(string key)
        {
            return _gameState[key];
        }

        internal string GetKey(string sender, string reciver)
        {
            return _gameState.ContainsKey(sender) ? sender : reciver;
        }
    }
}