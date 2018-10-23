using Client.BL;
using Client.Utils;
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
        private int[,] _blackCheckers;
        private int[,] _whiteCheckers;



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


        //Ctor
        public GameViewModel()
        {
            _gameManager = new ClientGameManager();
            RoleCubeCommand = new RelayCommand(RollCubes);
            Cells = new ObservableCollection<Ellipse>[24];
            for (int i = 0; i < 24; i++)
            {
                Cells[i] = new ObservableCollection<Ellipse>();
            }
            InitializeGame();
        }

        private void RollCubes()
        {
            _gameManager.RollCubes();
        }

        private void InitializeGame()
        {
            _whiteCheckers = new int[,]
           {
                {0,2},
                {11,5},
                {16,3},
                {18,5},
           };
            _blackCheckers = new int[,]
            {
                {5,5},
                {7,3},
                {12,5},
                {23,2},
            };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < _whiteCheckers[i, 1]; j++)
                {
                    _cells[_whiteCheckers[i, 0]].Add(Checker(false));
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < _blackCheckers[i, 1]; j++)
                {
                    _cells[_blackCheckers[i, 0]].Add(Checker(true));
                }
            }

        }

        private Ellipse Checker(bool isBlack)
        {
            Ellipse Ellipse = new Ellipse
            {
                Height = 20,
                Width = 20
            };

            if (isBlack)
                Ellipse.Fill = new SolidColorBrush(Colors.Black);
            else
                Ellipse.Fill = new SolidColorBrush(Colors.White);

            return Ellipse;
        }
    }

}

