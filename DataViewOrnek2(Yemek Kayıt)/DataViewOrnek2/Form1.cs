using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataViewOrnek2
{
    public partial class Form1 : Form
    {
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        DataView dataView;
        string ConnectionString = "Provider=Microsoft.ACE.Oledb.12.0;Data Source=yemek.accdb";
   

        public Form1()
        {
            InitializeComponent();
            Veri_Cek();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Veri_Ekle(yemekAdi_text.Text,Convert.ToInt32(kalori_text.Text));
        }
        DataTable dt = new DataTable();
        
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        void Veri_Cek()
        {
            using (OleDbConnection baglanti = new OleDbConnection(ConnectionString))
            {
       
                dataView = new DataView();
                ds = new DataSet();
                cmd = new OleDbCommand("SELECT * FROM Yemek", baglanti);
                da = new OleDbDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds, "Add New");
                dataView = new DataView(ds.Tables[0]);
                dataView.Sort = "Id";
                dataGridView1.DataSource = dataView;

            }
        }

        void Veri_Ekle(string yemek_adi, int kalori)
        {
            try
            {
                DataRowView newRow = dataView.AddNew();
                newRow["Yemek_Adi"] = yemek_adi;
                newRow["Kalori"] = kalori;
                newRow.EndEdit();
                dataView.Sort = "Id";
                OleDbConnection baglanti = new OleDbConnection(ConnectionString);
                OleDbCommand com = new OleDbCommand("INSERT INTO Yemek (Yemek_Adi,Kalori) VALUES (@Yemek_Adi,@Kalori)");
                com.Connection = baglanti;
                baglanti.Open();
                com.Parameters.Add("@Yemek_Adi", OleDbType.VarChar).Value = dataView[0][1];
                com.Parameters.Add("@Kalori", OleDbType.Integer).Value = dataView[0][2];
                try
                {
                    com.ExecuteNonQuery();  
                    MessageBox.Show("Yemek Ekleme İşlemi Başarılı!");
                    baglanti.Close();
                    Veri_Cek(); 
                }
                catch (OleDbException ex)
                {
                    MessageBox.Show(ex.Source);
                    baglanti.Close();
                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
