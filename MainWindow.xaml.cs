using Microsoft.Data.Sqlite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TourGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ConnectionString = "Data Source = tour.sql";
        private SqliteConnection connection;
        List<Versenyzo> dataList = [];


        public MainWindow()
        {
            InitializeComponent();
            connection = new SqliteConnection(ConnectionString);
            connection.Open();

            ReadFromDatabase();
        }

        public void ReadFromDatabase()
        {
            string queryText = """"
                    SELECT versenyzo.id, nev, csapatNev, nemzetiseg
                    FROM csapat INNER JOIN versenyzo ON csapat.id=versenyzo.csapatId
                """";

            SqliteCommand command = new SqliteCommand(queryText, connection);
            SqliteDataReader reader = command.ExecuteReader();

            dataList = [];

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string nev = reader.GetString(1);
                string csapatNev = reader.GetString(2);
                string nemzetiseg = reader.GetString(3);

                dataList.Add(new Versenyzo(id, nev, csapatNev, nemzetiseg));
            }
            reader.Close();
            VersenyzokDtg.ItemsSource = dataList;
            VersenyzokDtg.SelectedIndex = 0;
        }

        private void VersenyzokDtg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Versenyzo kivalasztott = (Versenyzo)VersenyzokDtg.SelectedItem;
            int id = kivalasztott.Id;

            string query = """
                    SELECT ido FROM eredmeny WHERE versenyzoId = @id AND szakasz = 5
                """;
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string ido = reader.GetString(0);
                eredmenyLb.Content = $"5. szakasz eredménye: {ido}";
            }
            reader.Close();
        }

        private void versenyzoSzamaBtn_Click(object sender, RoutedEventArgs e)
        {
            var csapatCsoport = dataList.GroupBy(x => x.CsapatNev).Select(y => new {Csapat = y.Key, Fo = y.Count() }).ToList();

            foreach (var item in csapatCsoport)
            {
                versenyzoSzamaListB.Items.Add($"{item.Csapat}: {item.Fo} Fő");
            }
        }

        private void magyarVersenyzoSzamaBtn_Click(object sender, RoutedEventArgs e)
        {
            var magyar = dataList.Count(x => x.Nemzetiseg == "HUN");

            MessageBox.Show($"magyar versenyzők száma: {magyar}");
        }
    }
}