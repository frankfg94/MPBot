using Bot_Test.MP.Scripts.Discord;
using BT.MP.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BT
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
         AdminPanelOperations op;

        public AdminPanel()
        {
            Init();
        }



        [STAThread]
        void Init()
        {
            op = new AdminPanelOperations();
            InitializeComponent();
        }

        private void ArmBomb_Button_Click(object sender, RoutedEventArgs e)
        {
            op.ArmSampleBomb();
            disarmBomb_Button.IsEnabled = true;
            rearmBomb_Button.IsEnabled = false;
            armBomb_Button.IsEnabled = false;
        }

        private void Lobby_Button_Click(object sender, RoutedEventArgs e)
        {
            op.StartLobby();
        }

        private void StopAudio_Button_Click(object sender, RoutedEventArgs e)
        {
            op.StopAudio();
        }

        private void InitCom_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Indiquez la commande : !init dans le chat pour Synchroniser le tout");
        }

        private void DEye_Button_Click(object sender, RoutedEventArgs e)
        {
             new DeadEyeDiscord().Test();
        }

        private void DesarmBomb_Button_Click(object sender, RoutedEventArgs e)
        {
            op.DisarmSampleBomb();
            disarmBomb_Button.IsEnabled = false;
            rearmBomb_Button.IsEnabled = true;
            armBomb_Button.IsEnabled = true;
        }

        private void RearmBomb_Button_Click(object sender, RoutedEventArgs e)
        {
            op.RearmSampleBomb();
            rearmBomb_Button.IsEnabled = false;
            disarmBomb_Button.IsEnabled = false;
        }
    }
}
