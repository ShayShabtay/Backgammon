using Client.BL;
using Client.Models;
using Client.Utils;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Client.ViewModels
{
    class GameViewModel : ViewModelBase
    {
        //Managers
        ClientGameManager _gameManager;

        //Properties
        public ICommand RoleCubeCommand { get; set; }
        private ObservableCollection<Ellipse>[] _cells;
        public ObservableCollection<Ellipse>[] Cells
        {
            get
            {
                return _cells;
            }
            set
            {
                _cells = value;
                OnPropertyChanged();
            }
        }
        private string _imgCube1;
        public string ImgCube1
        {
            get
            {
                return _imgCube1;
            }
            set
            {
                _imgCube1 = value;
                OnPropertyChanged();
            }
        }
        private string _imgCube2;
        public string ImgCube2
        {
            get
            {
                return _imgCube2;
            }
            set
            {
                _imgCube2 = value;
                OnPropertyChanged();
            }
        }
        BoardState boardState;
        public int RotatedBoard { get; set; }

        //Ctor
        public GameViewModel()
        {
            InitializeComponents();
            InitializeBoard();
            // _gameManager.RegisterCubesResultEvent(RollCubesResult);
        }

        //Methods
        private void RollCubes()
        {
            Cube rollResult = _gameManager.RollCubes();
            PaintCubeResult(rollResult);
        }


        //Paint board methods
        private Ellipse Checker(bool isBlack)
        {
            Ellipse Ellipse = new Ellipse
            {
                Height = 20,
                Width = 20,
                Stroke = new SolidColorBrush(Colors.Black)
            };

            if (isBlack)
                Ellipse.Fill = new SolidColorBrush(Colors.Black);
            else
                Ellipse.Fill = new SolidColorBrush(Colors.White);

            return Ellipse;
        }

        private void PaintCubeResult(Cube rollResult)
        {
            ImgCube1 = $"/Assets/{rollResult.Cube1}.png";
            ImgCube1 = $"/Assets/{rollResult.Cube2}.png";
        }


        //Ctor methods
        private void InitializeComponents()
        {
            _gameManager = new ClientGameManager();

            RoleCubeCommand = new RelayCommand(RollCubes);
            Cells = new ObservableCollection<Ellipse>[24];



            boardState = _gameManager.GetBoardState();
            if (boardState.CurrentPlayer != ClientUserManager.CurrentUserName)
            {
                RotatedBoard = 180;
            }

            for (int i = 0; i < 24; i++)
            {
                Cells[i] = new ObservableCollection<Ellipse>();
            }
        }

        private void InitializeBoard()
        {
            ImgCube1 = "/Assets/5.png";
            ImgCube2 = "/Assets/6.png";

            for (int i = 0; i < 24; i++)
            {
                if (boardState.WhiteLocation.ContainsKey(i))
                {
                    for (int j = 0; j < boardState.WhiteLocation[i]; j++)
                    {
                        _cells[i].Add(Checker(false));
                    }
                }
            }

            for (int i = 0; i < 24; i++)
            {
                if (boardState.BlackLocation.ContainsKey(i))
                {
                    for (int j = 0; j < boardState.BlackLocation[i]; j++)
                    {
                        _cells[i].Add(Checker(true));
                    }
                }
            }
        }

        //Events
        private void RollCubesResult(Cube rollCubeResult)
        {
            PaintCubeResult(rollCubeResult);
        }
    }
}

