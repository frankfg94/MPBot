using Bot_Test;
using Bot_Test.MP.Scripts;
using Bot_Test.MP.Scripts.Discord;
using BT.MP;
using BT.MP.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
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
        public static List<Window> windows = new List<Window>();
        public ObservableCollection<string> audiofilesNamesList { get; set; }
        private readonly DelegateCommand playCommand;
        public static ulong guildID;
        public static ulong channelID;
        public WeaponRange CurrentRange { get; set; } 

        public AdminPanel()
        {
            Init();
            playCommand = new DelegateCommand(PlayAudio);
            Loaded += AdminPanel_Loaded;
            windows.Add(this);
            range_Slider.Value = 2;
        }

        private void AdminPanel_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
            weapon_Combobox.ItemsSource = Weapon.GetDefaultWeapons();
            weapon_Combobox.SelectedIndex = 0;
            InitAudio();
            SyncData();
            channelID_tBox.TextChanged += (s, e) => SyncData();
        }

        private void SyncData()
        {
            if (ulong.TryParse(channelID_tBox.Text, out ulong cId))
            {
                //guildID = gId;
                channelID = cId;
            }
        }

        [STAThread]
        void Init()
        {
            op = new AdminPanelOperations();
            InitializeComponent();

        }


        private void InitAudio()
        {
            new AudioExplorer(ref audioList_Lview, audioPath_tblock, true).RefreshList();
        }

        #region Buttons

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
            new DeadEyeDiscord().TargetWithPanelData(this,null);
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
            disarmBomb_Button.IsEnabled = true;
        }

        #endregion Buttons

        #region Commands

        public DelegateCommand PlayAudioCommand { get { return playCommand; } }

        private async void PlayAudio(object sender)
        {
            string fileNameAbsolute = Program.resourceFolderPath + "\\" + sender.ToString();
            await BT.Program.communicator.PlayAudio(fileNameAbsolute, false);
        }

        #endregion Commands
        // TODO : not working, replace with regex or find the cause
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
           if(!double.TryParse(e.Text,out double d))
            {
                e.Handled = false;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.CurrentRange = (WeaponRange)e.NewValue;
            distance_Tblock.Text = $"Distance : {CurrentRange.ToString().Replace("_", " ")}";
        }
    }
}
