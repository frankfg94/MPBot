using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BT;

namespace Bot_Test.MP.Scripts
{
    public class AudioExplorer
    {
        private readonly ListView audioList_Lview;
        static List<FileSystemWatcher> watchers = new List<FileSystemWatcher>(); // use static list to keep watcher alive

        public AudioExplorer(ref ListView audioList_Lview, TextBlock resPathTblock, bool watchFolder)
        {
            this.audioList_Lview = audioList_Lview;
            resPathTblock.Dispatcher.Invoke(new Action(()=>{
                resPathTblock.Text = "Chemin: " + Program.resourceFolderPath;
            }));
            if (watchFolder) watch();
        }

        /**
         * Get all the audio files from the resource folder
         */
        public List<string> GetAudioNamesFromResFolder()
        {
            DirectoryInfo d = new DirectoryInfo(Program.resourceFolderPath);
            return d.GetFilesByExtensions(".wav",".mp3").Select(x => x.Name).ToList();
        }

        public void RefreshList()
        {
            if (audioList_Lview != null)
            {
                audioList_Lview.Dispatcher.Invoke(new Action(() => {
                    audioList_Lview.ItemsSource = GetAudioNamesFromResFolder();
                    audioList_Lview.Items.Refresh();
                }));
            }
        }

        private void watch()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(Program.resourceFolderPath);
            watcher.NotifyFilter = NotifyFilters.Attributes |
            NotifyFilters.CreationTime |
            NotifyFilters.FileName |
            NotifyFilters.LastWrite |
            NotifyFilters.Size;
            watcher.Filter = "";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnChanged);
            watcher.Error += OnError;
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            watchers.Add(watcher);
        }
        private static void OnError(object sender, ErrorEventArgs e) =>
            Program.PrintException(e.GetException());

        

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            RefreshList();
        }

    }
}
