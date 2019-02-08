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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Public class for adding information to datagrid
        public class Business
        {
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }
        }
        // Strings to recognize if text has actually been changed.
        private String PrevText;
        private String PrevCityText;
        // Initialize Window upon opening.
        // Sets event handlers to appropriate events, and initializes boxes in window.
        public MainWindow()
        {
            InitializeComponent();
            initStates();
            initCities();
            initGrid();
            States.DropDownClosed += States_SelectionChangeCommitted;
            Cities.DropDownClosed += Cities_SelectionChangeComitted;
            Cities.SelectionChanged += Cities_Changed;
            PrevText = States.Text;
            PrevCityText = Cities.Text;
        }
        // String with information on SQL server (local in this case), remember to add password back in
        // Which was removed for posting online.
        private string buildConnString()
        {
            return "Host=localhost; Username=postgres; Password=; Database=Milestone1";
        }
        // Init function, finds states in SQL database and adds them to 
        // Top combo box
        public void initStates()
        {
            States.Items.Add("[Select State]");
            States.Text = "[Select State]";
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT state FROM \"BusinessData\" ORDER BY state;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            States.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }
       // Initialize cities function, just adds [select city] to cities combo box.
       public void initCities()
        {
            Cities.Items.Add("[Select City]");
            Cities.Text = "[Select City]";
        }
        // Add cities function, when a state is selected this fills the 
        // city combo box with cities in that state available in the database.
        private void addCities()
        {
            if (PrevText != States.Text)
            {
                Cities.Items.Clear();
                Cities.Items.Add("[Select City]");
                Cities.Text = "[Select City]";
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT DISTINCT city FROM \"BusinessData\" WHERE state=" + "'" + States.Text + "'" + " ORDER BY city;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Cities.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                    conn.Close();
                    PrevText = States.Text;
                }
            }
        }
        // Actual event that triggers addCities function from above.
        private void States_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            addCities();
        }
        // When City has been selected this calls the SQL database and gets
        // business names for that city and state, then it fills the data grid with that info.
        private void fillGrid()
        {
            if (PrevCityText != Cities.Text)
            {
                BusinessGrid.Items.Clear();
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name From \"BusinessData\" WHERE city='" + Cities.Text.ToString() + "'" + " AND state=" + "'" + States.Text.ToString() + "'" + " ORDER BY name;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BusinessGrid.Items.Add(new Business() { name = reader.GetString(0), state = States.Text, city = Cities.Text });
                            }
                        }
                    }
                    conn.Close();
                    PrevCityText = Cities.Text;
                }
            }
        }
        // Event handler for when a city is selected that calls fillGrid function from above.
        private void Cities_SelectionChangeComitted(object sender, System.EventArgs e)
        {
            fillGrid();
        }
        // When a the selected value of cities combo box is changed for any
        // reason, this checks if it's been set back to it's standard setting
        // and clears the combo box if that has occurred.
        private void Cities_Changed(object sender, System.EventArgs e)
        {
            if (Cities.Text == "[Select City]")
                BusinessGrid.Items.Clear();
        }
        // Initialize the data grid, give headers to columns.
        public void initGrid()
        {
            DataGridTextColumn BusinessName = new DataGridTextColumn();
            BusinessName.Header = "Business Name";
            BusinessName.Binding = new Binding("name");
            BusinessName.Width = 300;
            BusinessGrid.Columns.Add(BusinessName);
            DataGridTextColumn StateName = new DataGridTextColumn();
            StateName.Header = "State";
            StateName.Binding = new Binding("state");
            StateName.Width = 85;
            BusinessGrid.Columns.Add(StateName);
            DataGridTextColumn CityName = new DataGridTextColumn();
            CityName.Header = "City";
            CityName.Binding = new Binding("city");
            CityName.Width = 85;
            BusinessGrid.Columns.Add(CityName);
        }
    }
}
