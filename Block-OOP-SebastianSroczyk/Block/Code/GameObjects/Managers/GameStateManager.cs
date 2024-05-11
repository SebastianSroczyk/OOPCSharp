using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Block.Code.GameObjects.Managers
{
    public class GameStateManager
    {
        private ObjectManager _objectManager;
        private InventoryManager _inventoryManager;

        private int NumberOfEnemies = 100;
        private int NumberOfItems = 100;
        private int currentNumberOfMonsters = 0;
        private int currentRound = 1;

        public int CurrentNumberOfMonsters { get { return currentNumberOfMonsters; } set { currentNumberOfMonsters = value; } }
        public int CurrentRound { get { return currentRound; } private set { currentRound = value; } }


        public void PlayGame()
        {    
            CurrentNumberOfMonsters = 0;
            CurrentRound = 0;

            _objectManager = new ObjectManager();
            _inventoryManager = new InventoryManager();


        }

        




    }
}
