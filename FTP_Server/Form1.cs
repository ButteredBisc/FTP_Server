using System;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Drawing;
using System.Windows.Forms;

namespace FTP_Server
{
    public partial class Form1 : Form
    {
        Int32 port1 = 5050;
        string filepath;

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {           
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = folderBrowserDialog1.SelectedPath;                
            }
        }
        private void ReceiveFile(String FilePath)
        {
            lblDest.Text = "Uploading File......";

            //Activate server and listen on port for an IP
            TcpListener listener = new TcpListener(IPAddress.Any, port1);
            listener.Start();
            Socket client = listener.AcceptSocket();
                
            //After connection open file to be written, allocate A BUFFER 64Kb, Receive the data
            //and write to the file
            if(client.Connected) lblConnection.Text = "Connected";
            Stream fileStream = File.OpenWrite(FilePath);
            int chunkSize = 64 *1024;
            int dataRead = 0;
            Byte[] recBuffer = new Byte[chunkSize];
            while ((dataRead = client.Receive(recBuffer, recBuffer.Length, SocketFlags.None)) > 0)
            {
                byte[] actual = new byte[dataRead];
                Buffer.BlockCopy(recBuffer, 0, actual, 0, dataRead);
                fileStream.Write(actual, 0, actual.Length);
            }
            fileStream.Close();
            if (!client.Connected) lblConnection.Text = "Disconnected";
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if ((txtFileName.Text == "") || (txtFile.Text == "")) {
                MessageBox.Show("Enter Filepath and Filename!");
            }
            else
            {
                filepath = txtFile.Text + "\\" + txtFileName.Text;
                ReceiveFile(filepath);
                lblDest.Text = "File Received......";
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnPhoto_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtPhoto.Text = openFileDialog1.FileName;
                picBox.Image = new Bitmap(openFileDialog1.FileName);
                picBox.SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }

        private void lblConnection_Click(object sender, EventArgs e)
        {

        }
    }
}
