using General;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby {

    public enum Characters
    {
        Zee,
        Ferde
    }
    
    public class MainCanvas : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField]
        private GameObject player1You,
            player2You,
            player1Text,
            player2Text,
            player1Image,
            player2Image,
            player1CharButton,
            player2CharButton,
            player1LevelButton,
            player2LevelButton,
            readyButton,
            gameManager,
            clientSerializer;

        [SerializeField]
        private Characters defaultPlayer1, defaultPlayer2;

        [SerializeField]
        private int defaultLevel;

        #region Properties

        public Characters Player1SelectedChar { get; private set; }
        public Characters Player2SelectedChar { get; set; }

        public int Player1SelectedLvl { get; private set; }
        public int Player2SelectedLvl { get; set; }

        public int Player1MaxLvl { get; private set; }
        public int Player2MaxLvl { get; set; }

        public int Player { get; private set; }

        public bool Player1Ready { get; private set; }
        public bool Player2Ready { get; set; }

        #endregion
        
        private bool _changedLevel;
        
        private GameManager _gameManagerComponent;
        
        private ClientSerializer _clientSerializer;
        
        private Text _player1TextComponent;
        
        private void Start()
        {
            _player1TextComponent = player1Text.GetComponent<Text>();
            _clientSerializer = clientSerializer.GetComponent<ClientSerializer>();
            _gameManagerComponent = gameManager.GetComponent<GameManager>();
            Player1SelectedLvl = defaultLevel;
            Player2SelectedLvl = defaultLevel;
            
            Player1SelectedChar = defaultPlayer1;
            Player2SelectedChar = defaultPlayer2;

            if (PhotonNetwork.IsMasterClient) return;
            clientSerializer = PhotonNetwork.Instantiate(clientSerializer.name, Vector3.zero, Quaternion.identity);
        }

        private void Update()
        {
            Player = _gameManagerComponent.player;

            switch (Player1SelectedChar)
            {
                case Characters.Zee when Player == 1:
                    _gameManagerComponent.playerChar = 1;
                    break;
                case Characters.Ferde when Player == 1:
                    _gameManagerComponent.playerChar = 2;
                    break;
                default:
                {
                    if (Player2SelectedChar == Characters.Zee && Player == 2)
                    {
                        _gameManagerComponent.playerChar = 1;
                    }
                    else
                    {
                        _gameManagerComponent.playerChar = 2;
                    }

                    break;
                }
            }

            if ((Player1SelectedLvl > Player1MaxLvl) && Player == 1)
            {
                Player1SelectedLvl = 1;
            }

            if ((Player2SelectedLvl > Player2MaxLvl) && Player == 2)
            {
                Player2SelectedLvl = 1;
            }

            //Send player 2 vars
            if (!PhotonNetwork.IsMasterClient)
            {
                _clientSerializer.player2SelectedChar = Player2SelectedChar;
                _clientSerializer.player2SelectedLvl = Player2SelectedLvl;
                _clientSerializer.player2MaxLvl = Player2MaxLvl;
                _clientSerializer.player2Ready = Player2Ready;
            }

            //Removes the irrelevant sign
            if (Player == 1)
            {
                player2You.SetActive(false);
            }
            else
            {
                player1You.SetActive(false);
            }

            //Changes player placeholders to nicknames if not empty or null
            foreach (var networkPlayer in PhotonNetwork.PlayerList)
            { 
                if (!string.IsNullOrEmpty(networkPlayer.NickName))
                {
                    _player1TextComponent.text = networkPlayer.NickName;
                }
            }

            //Ready button visibility
            readyButton.SetActive(Player1SelectedChar != Player2SelectedChar && (Player1SelectedLvl == Player2SelectedLvl) && (PhotonNetwork.PlayerList.GetLength(0) == 2));

            //Updating max level values
            if (Player == 1)
            {
                Player1MaxLvl = _gameManagerComponent.GetPlayerMaxLevel();
            }
            else
            {
                Player2MaxLvl = _gameManagerComponent.GetPlayerMaxLevel();
            }

            //Updating Char Buttons
            player1CharButton.GetComponentInChildren<Text>().text = Player1SelectedChar.ToString();
            player2CharButton.GetComponentInChildren<Text>().text = Player2SelectedChar.ToString();

            //Updating Level Buttons
            player1LevelButton.GetComponentInChildren<Text>().text = "LV" +Player1SelectedLvl;
            player2LevelButton.GetComponentInChildren<Text>().text = "LV" +Player2SelectedLvl;

            //Change levels
            if (!Player1Ready || !Player2Ready || (_changedLevel != false)) return;
            if (!PhotonNetwork.IsMasterClient) return;
            foreach (var levelData in _gameManagerComponent.levels)
            {
                if (levelData.levelNumber != Player1SelectedLvl) continue;
                PhotonNetwork.LoadLevel(levelData.sceneNumber);
                Debug.Log("Loading level " + levelData.levelNumber);
                _changedLevel = true;
            }

        }
        
        //send player 1 vars
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(Player1SelectedChar);
                stream.SendNext(Player1SelectedLvl);
                stream.SendNext(Player1MaxLvl);
                stream.SendNext(Player1Ready);
            }
            else
            {
                Player1SelectedChar = (Characters)stream.ReceiveNext();
                Player1SelectedLvl = (int)stream.ReceiveNext();
                Player1MaxLvl = (int)stream.ReceiveNext();
                Player1Ready = (bool)stream.ReceiveNext();
            }
        }

        public void OnPlayer1CharButton()
        {
            if (Player != 1) return;
            Player1SelectedChar = Player1SelectedChar == Characters.Zee ? Characters.Ferde : Characters.Zee;
        }

        public void OnPlayer2CharButton()
        {
            if (Player != 2) return;
            Player1SelectedChar = Player1SelectedChar == Characters.Ferde ? Characters.Zee : Characters.Ferde;
        }

        public void OnPlayer1LevelButton()
        {
            if (Player == 1)
            {
                Player1SelectedLvl++;
            }
        }

        public void OnPlayer2LevelButton()
        {
            if (Player == 2)
            {
                Player2SelectedLvl++;
            }
        }

        public void OnReadyButton()
        {
            if (readyButton.GetComponent<Image>().color == Color.white)
            {
                readyButton.GetComponent<Image>().color = Color.green;
                if (Player == 1)
                {
                    Player1Ready = true;
                }
                else
                {
                    Player2Ready = true;
                }
            }
            else
            {
                readyButton.GetComponent<Image>().color = Color.white;
                if (Player == 1)
                {
                    Player1Ready = true;
                }
                else
                {
                    Player2Ready = true;
                }
            }
        }
    }
}
