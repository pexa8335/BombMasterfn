using SuperTank;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperTank
{
    public partial class Lobby : Form
    {
        private CancellationTokenSource _cts;
        public string HostName { get; set; }
        public bool IsHost { get; set; } = false;
        public string RoomId { get; }

        public Lobby()
        {
            InitializeComponent();
            namePlayer1.AutoEllipsis = true;
            namePlayer2.AutoEllipsis = true;
            namePlayer3.AutoEllipsis = true;
            namePlayer4.AutoEllipsis = true;
            this.Load += Lobby_Load;

            // Display HostName and set IsHost
            if (!string.IsNullOrEmpty(HostName))
            {
                if (SocketClient.localPlayer != null && HostName == SocketClient.localPlayer.Name)
                {
                    IsHost = true;
                    lb_roomID.Text = "MÃ PHÒNG: " + RoomId + " (Bạn là chủ phòng)";
                }
                else
                {
                    lb_roomID.Text = "MÃ PHÒNG: " + RoomId + $" (Chủ phòng: {HostName})";
                }
            }
        }

        private async void Lobby_Load(object sender, EventArgs e)
        {
            _cts = new CancellationTokenSource();
            await RunContinuouslyAsync(_cts.Token);
        }

        private async Task RunContinuouslyAsync(CancellationToken token)
        {
            SocketClient.SendData($"SEND_LOBBY;{SocketClient.joinedRoom}");
            await Task.Delay(100, token);

            while (!token.IsCancellationRequested)
            {
                InitLobby();

                if (SocketClient.isStartGame)
                {
                    // Synchronize player list from `joinedLobby` to `SocketClient.players` once game starts
                    if (SocketClient.players.Count == 0)
                    {
                        foreach (var lobbyPlayer in SocketClient.joinedLobby.Players)
                        {
                            //phần này là phần tạo constructor khởi tạo cho playertank giá trị ban đầu, đéo hiểu, k làm dc

                            SocketClient.players.Add(new SocketClient.PlayerTank
                            {
                                Name = lobbyPlayer.Name,
                                Position = new PointF(0, 0) // Set initial spawn point or position
                            });
                        }
                        Debug.WriteLine("All players added to SocketClient.players for MainGame start.");
                    }

                    // Cancel lobby task and start game -> show form game
                    _cts.Cancel();
                    frmGameMulti multi = new frmGameMulti();
                    this.Hide();
                    multi.Show();
                }

                try
                {
                    await Task.Delay(500, token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        private void InitLobby()
        {
            try
            {
                lb_Total.Text = "Total: " + SocketClient.joinedLobby.PlayersName.Count.ToString();
                lb_roomID.Text = "Room ID: " + SocketClient.joinedLobby.RoomId;
                if (SocketClient.joinedLobby != null && SocketClient.joinedLobby.Host != null && 
                    SocketClient.localPlayer != null && SocketClient.localPlayer.Name == SocketClient.joinedLobby.Host.Name)
                {
                    btn_Start.Enabled = true;
                    btn_Start.Visible = true;
                }
                else
                {
                    btn_Start.Enabled = false;
                    btn_Start.Visible = false;
                }

                string[] playersName = new string[4];
                lb_Total.Text = "Total: " + SocketClient.joinedLobby.PlayersName.Count.ToString();
                lb_roomID.Text = "Room ID: " + SocketClient.joinedLobby.RoomId;
                int countPlayer = SocketClient.joinedLobby.PlayersName.Count;

                for (int i = 0; i < countPlayer; i++)
                {
                    switch (i)
                    {
                        case 0:
                            namePlayer1.Text = SocketClient.joinedLobby.PlayersName[i];
                            //ptb_player1.Image = Properties.Resources.ares;
                            lbReady1.Visible = true;
                            if (SocketClient.CheckIsReady(namePlayer1.Text))
                            {
                                lbReady1.Text = "Ready";
                                lbReady1.ForeColor = Color.Lime;
                            }
                            break;

                        case 1:
                            namePlayer2.Text = SocketClient.joinedLobby.PlayersName[i];
                            // ptb_player2.Image = Properties.Resources.knight;
                            lbReady2.Visible = true;
                            if (SocketClient.CheckIsReady(namePlayer2.Text))
                            {
                                lbReady2.Text = "Ready";
                                lbReady2.ForeColor = Color.Lime;
                            }
                            break;

                        case 2:
                            namePlayer3.Text = SocketClient.joinedLobby.PlayersName[i];
                            //ptb_player3.Image = Properties.Resources.serial_killer;
                            lbReady3.Visible = true;
                            if (SocketClient.CheckIsReady(namePlayer3.Text))
                            {
                                lbReady3.Text = "Ready";
                                lbReady3.ForeColor = Color.Lime;
                            }
                            break;

                        case 3:
                            namePlayer4.Text = SocketClient.joinedLobby.PlayersName[i];
                            //ptb_player4.Image = Properties.Resources.player1;
                            lbReady4.Visible = true;
                            if (SocketClient.CheckIsReady(namePlayer4.Text))
                            {
                                lbReady4.Text = "Ready";
                                lbReady4.ForeColor = Color.Lime;
                            }
                            break;
                    }
                }
                for (int i = countPlayer; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            namePlayer1.Text = "Player1";
                            //ptb_player1.Image = Properties.Resources.anonymous;
                            lbReady1.Visible = false;
                            lbReady1.Text = "Not ready";
                            lbReady1.ForeColor = Color.Red;
                            break;

                        case 1:
                            namePlayer2.Text = "Player2";
                            //ptb_player2.Image = Properties.Resources.anonymous;
                            lbReady2.Visible = false;
                            lbReady2.Text = "Not ready";
                            lbReady2.ForeColor = Color.Red;
                            break;

                        case 2:
                            namePlayer3.Text = "Player3";
                            //ptb_player3.Image = Properties.Resources.anonymous;
                            lbReady3.Visible = false;
                            lbReady3.Text = "Not ready";
                            lbReady3.ForeColor = Color.Red;
                            break;

                        case 3:
                            namePlayer4.Text = "Player4";
                            // ptb_player4.Image = Properties.Resources.anonymous;
                            lbReady4.Visible = false;
                            lbReady4.Text = "Not ready";
                            lbReady4.ForeColor = Color.Red;
                            break;
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ptb_player1_Click(object sender, EventArgs e)
        {

        }

        private void btn_Ready_Click(object sender, EventArgs e)
        {
            SocketClient.SendData("READY");
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (SocketClient.CheckIsReadyForAll() )
            {
                SocketClient.SendData("START");
            }
            else
            {
                MessageBox.Show("Các người chơi khác vẫn chưa sẵn sàng!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
