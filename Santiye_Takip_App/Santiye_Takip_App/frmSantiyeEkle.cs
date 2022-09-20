using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Santiye_Takip_App
{
    public partial class frmSantiyeEkle : Form
    {
        public frmSantiyeEkle()
        {
            InitializeComponent();
        }


        SqlConnection baglanti = new SqlConnection("Data Source=ERTUGRUL-OZCAN;Initial Catalog=Santiye_Takip;Integrated Security=True");

        

        private void frmSantiyeEkle_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into Santiye(SantiyeAdi,FirmaAdi,ProjeMaliyeti,Telefon,Adres,Email) values(@SantiyeAdi,@FirmaAdi,@ProjeMaliyeti,@Telefon,@Adres,@Email)", baglanti);
            komut.Parameters.AddWithValue("@SantiyeAdi",txtSantiyeAdi.Text);
            komut.Parameters.AddWithValue("@FirmaAdi", txtFirmaAdi.Text);
            komut.Parameters.AddWithValue("@ProjeMaliyeti", txtProjeMaliyeti.Text);
            komut.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
            komut.Parameters.AddWithValue("@Adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@Email", txtEmail.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Şantiye Kaydı Eklendi");
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
