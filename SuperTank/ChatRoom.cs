using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperTank
{
    public partial class ChatRoom : Form
    {
        public ChatRoom()
        {
            InitializeComponent();
            SocketClient.OnReceiveMessage += UpdateMessage;
        }
        private void UpdateMessage(string message)
        {
            if (showMessage.InvokeRequired)
            {
                showMessage.Invoke(new Action(() => UpdateMessage(message)));
            }
            else
            {
                showMessage.Items.Add(message.ToString());
            }
        }
        private void sendButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(messageBox.Text)) return;

            string message = String.Format($"[{SocketClient.localPlayer.Name}]: {messageBox.Text}");
            messageBox.Clear();
            showMessage.Items.Add(message);

            SocketClient.SendData($"MESSAGE;{message}");
        }

    }
}
