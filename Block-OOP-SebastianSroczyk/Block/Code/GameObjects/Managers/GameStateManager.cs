using Block.Code.GameObjects.Enemy;
using Block.Code.Screens;
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
        /// <summary>
        /// This is the Game State Manager, it is responsable for the whole game, in here anything from enablaling objects to controlling the UI elements controlled.
        /// </summary>

        

        // Classes
        private InventoryManager _inventoryManager;
        private MonsterGen _monsterGen;
        private MyWorld _world;

        //General Game Settings
        private int currentNumberOfMonsters = 0;
        private int currentRound = 1;
        private bool playing = true; 

        public int CurrentNumberOfMonsters { get { return currentNumberOfMonsters; } set { currentNumberOfMonsters = value; } }
        public int CurrentRound { get { return currentRound; } private set { currentRound = value; } }

        // Object Pool - Relative Variables and Properties 
        private int NumberOfEnemies = 100;
        private int NumberOfItems = 100;
        
        private List<GameObject> monstersList = new List<GameObject>();
        private List<Vector2> monsterOriginPos = new List<Vector2>();

        public List<GameObject> MonstersList { get {  return monstersList; } }

        private void Initialize()
        {
            CurrentNumberOfMonsters = 0;
            CurrentRound = 0;
            _inventoryManager = new InventoryManager();
            playing = true;
        }

        public void PlayGame()
        {
            Initialize();
            

        }
        private void EndGame()
        {

        }
        
        private Vector2 PlaceObjects(Vector2 p1, Vector2 p2)
        {
            Random r = new Random();

            Vector2 tempPos = new Vector2(r.Next((int)Settings.GameResolution.X - 30), r.Next((int)Settings.GameResolution.Y - 30));

            while ((tempPos.X > p1.X && tempPos.X < p2.X) && (tempPos.Y > p1.Y && tempPos.Y < p2.Y))
            {
                tempPos = new Vector2(r.Next((int)Settings.GameResolution.X - 30), r.Next((int)Settings.GameResolution.Y - 30));
            }

            return tempPos;
        }

        public Tuple<GameObject,Vector2> GenerateObjects()
        {
            _monsterGen = new MonsterGen();

            Vector2 BoxTopLeft = new Vector2(300, 300);
            Vector2 BoxBottomRight = new Vector2(900, 900);

            while (CurrentNumberOfMonsters != CurrentRound)
            {
                Vector2 pos = PlaceObjects(BoxTopLeft, BoxBottomRight);
                monstersList.Add(_monsterGen.GenerateMonster(pos));
                monsterOriginPos.Add(pos);
                CurrentNumberOfMonsters++;

                // there is an index out of range error here...
                return Tuple.Create(monstersList[CurrentNumberOfMonsters], monsterOriginPos[CurrentNumberOfMonsters]);
            }

            
            // Disables all objects in Object Pool
            foreach (GameObject obj in monstersList)
            {
                obj.SetActive(false);
                obj.SetVisible(false);
            }
            

            return null;
            
        }


    }
}
