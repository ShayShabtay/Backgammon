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

        //Properties
        internal string CurrentTurn { get; set; }

        private GameManager()
        {

        }

        internal void RollCubes()
        {
            throw new NotImplementedException();
        }
    }
}