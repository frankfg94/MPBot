using Bot_Test.Database.DbTools;
using Bot_Test;
using Bot_Test.Database;
using Bot_Test.Database.DbModels;
using Bot_Test.Database.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bot_Test.Database.Views
{
    /// <summary>
    /// Logique d'interaction pour DbManager.xaml
    /// </summary>
    public partial class DbManager : Window
    {
        private readonly DelegateCommand viewCommand;
        private readonly DelegateCommand editCommand;
        private readonly DelegateCommand deleteCommand;
        public DbManager()
        {
            InitializeComponent();
            DataContext = this;
            table_picker.ItemsSource = Tables;
            viewCommand = new DelegateCommand(ViewItem);
            editCommand = new DelegateCommand(EditItem);
            deleteCommand = new DelegateCommand(DeleteItem);
        }

        public List<Tables> Tables { get { return Data.Tables; } }
        public string SelectedTable { get; set; }
        public List<SqlObject> Items { get; set; }


        public DelegateCommand ViewItemCommand { get { return viewCommand; } }
        public DelegateCommand EditItemCommand { get { return editCommand; } }
        public DelegateCommand DeleteItemCommand { get { return deleteCommand; } }

        private void ViewItem(object sender)
        {

        }

        private void EditItem(object sender)
        {

        }

        private void DeleteItem(object sender)
        {

        }


        private void TableSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (table_picker.SelectedItem != null)
            {
                SelectedTable = table_picker.SelectedItem.ToString();
                Items = DbRequester.SelectAll(SelectedTable).ToList();
                string nameColumn = Tables.Find(x => x.Name == SelectedTable).NameColumn;
                for (int i = 0; i < Items.Count; i++)
                    Items[i].NameColumn = nameColumn;
                items_listview.ItemsSource = null;
                items_listview.ItemsSource = Items;
                items_listview.Items.Refresh();
            }
        }


    }
}
