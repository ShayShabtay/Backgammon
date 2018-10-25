using Common.Interfaces;
using Common.Models;
using Server.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class BoardState : IBoardState
    {
        //Managers
        UserManager _userManager = UserManager.Instance;

        //Fields
        internal readonly string connectionIdBlack;
        internal readonly string connectionIdWhite;
        internal readonly string BlackPlayer;
        internal readonly string WhitePlayer;

        //Properties
        public Dictionary<int, int> BlackLocation { get; set; }
        public Dictionary<int, int> WhiteLocation { get; set; }
        public Cube Cube { get; set; }
        public string CurrentPlayer { get; set; }
        public int BarredBlackCheckers { get; set; }
        public int BarredWhiteCheckers { get; set; }
        public bool IsDouble { get; set; }

        //Ctor
        public BoardState(string whitePlayer, string blackPlayer)
        {
            BlackPlayer = blackPlayer;
            CurrentPlayer = WhitePlayer = whitePlayer;
            connectionIdBlack = _userManager.GetConnectionID(blackPlayer);
            connectionIdWhite = _userManager.GetConnectionID(whitePlayer);

            BlackLocation = new Dictionary<int, int>()
            {
                {5,5},
                {7,3},
                {12,5},
                {23,2},
            };

            WhiteLocation = new Dictionary<int, int>()
            {
                {0,2},
                {11,5},
                {16,3},
                {18,5},
            };

        }

    }
    
}