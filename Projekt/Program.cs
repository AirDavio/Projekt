using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt
{
    public partial class ListaRzeczyDoZrobienia : Form
    {
        SqlConnection connection = new SqlConnection("Server=DESKTOP-A6AIHCM\\SQLEXPRESS;Database=NazwaTwojejBazyDanych;User Id=Projekt;Password=przykladowehaslo;");

        public ListaRzeczyDoZrobienia()
        {
            InitializeComponent();
        }

        DataTable listarzeczy = new DataTable();
        bool isEditing = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            listarzeczy.Columns.Add("Zadanie");
            listarzeczy.Columns.Add("Ważność");
            wykaz.DataSource = listarzeczy;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Zadaniadowykonania_Click(object sender, EventArgs e)
        {

        }

        private void Dodaj_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM Projekt WHERE Zadanie = @Zadanie";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Zadanie", textBox1.Text);
                int existingRecords = (int)checkCommand.ExecuteScalar();

                if (existingRecords > 0)
                {
                    string updateQuery = "UPDATE Projekt SET Ważność = @Ważność WHERE Zadanie = @Zadanie";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@Zadanie", textBox1.Text);
                    updateCommand.Parameters.AddWithValue("@Ważność", textBox2.Text);
                    updateCommand.ExecuteNonQuery();
                }
                else
                {
                    string insertQuery = "INSERT INTO Projekt (Zadanie, Ważność) VALUES (@Zadanie, @Ważność)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Zadanie", textBox1.Text);
                    insertCommand.Parameters.AddWithValue("@Ważność", textBox2.Text);
                    insertCommand.ExecuteNonQuery();
                }

                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Projekt", connection);
                DataTable tabela = new DataTable();
                adapter.Fill(tabela);
                wykaz.DataSource = tabela;

                MessageBox.Show("Dane dodane do bazy danych.");

                textBox1.Text = "";
                textBox2.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania danych do bazy: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }


        private void zmien_Click(object sender, EventArgs e)
        {
            isEditing = true;
            textBox1.Text = listarzeczy.Rows[wykaz.CurrentCell.RowIndex].ItemArray[0].ToString();
            textBox2.Text = listarzeczy.Rows[wykaz.CurrentCell.RowIndex].ItemArray[1].ToString();
        }

        private void Usuń_Click(object sender, EventArgs e)
        {
            try
            {
                listarzeczy.Rows[wykaz.CurrentCell.RowIndex].Delete();
            }
            catch(Exception E)
            {
                Console.WriteLine("Błąd: " + E);
            }
        }

        private void zapis_Click(object sender, EventArgs e)
        {
            if (isEditing)
            {
                listarzeczy.Rows[wykaz.CurrentCell.RowIndex]["Zadanie"] = textBox1.Text;
                listarzeczy.Rows[wykaz.CurrentCell.RowIndex]["Ważność"] = textBox2.Text;
            }
            else
            {
                listarzeczy.Rows.Add(textBox1.Text, textBox2.Text);
            }
            textBox1.Text = "";
            textBox2.Text = "";
            isEditing = false;
        }
    }
}
