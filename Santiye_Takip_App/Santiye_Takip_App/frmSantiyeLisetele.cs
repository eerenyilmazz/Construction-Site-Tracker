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

namespace Santiye_Takip_App
{
    public partial class frmSantiyeLisetele : Form
    {
        public frmSantiyeLisetele()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=ERTUGRUL-OZCAN;Initial Catalog=Santiye_Takip;Integrated Security=True");
        DataSet daset = new DataSet();

        private void frmSantiyeLisetele_Load(object sender, EventArgs e)
        {
            Kayıt_Göster();
        }

        private void Kayıt_Göster()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select *from Santiye", baglanti);
            adtr.Fill(daset, "Santiye");
            dataGridView1.DataSource = daset.Tables["Santiye"];
            baglanti.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSantiyeAdi.Text = dataGridView1.CurrentRow.Cells["SantiyeAdi"].Value.ToString();
            txtFirmaAdi.Text = dataGridView1.CurrentRow.Cells["FirmaAdi"].Value.ToString();
            txtProjeMaliyeti.Text = dataGridView1.CurrentRow.Cells["ProjeMaliyeti"].Value.ToString();
            txtTelefon.Text = dataGridView1.CurrentRow.Cells["Telefon"].Value.ToString();
            txtAdres.Text = dataGridView1.CurrentRow.Cells["Adres"].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells["Email"].Value.ToString();

        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update Santiye set FirmaAdi=@FirmaAdi,ProjeMaliyeti=@ProjeMaliyeti,Telefon=@Telefon,Adres=@Adres,Email=@Email where SantiyeAdi=@SantiyeAdi", baglanti);
            komut.Parameters.AddWithValue("@SantiyeAdi", txtSantiyeAdi.Text);
            komut.Parameters.AddWithValue("@FirmaAdi", txtFirmaAdi.Text);
            komut.Parameters.AddWithValue("@ProjeMaliyeti", txtProjeMaliyeti.Text);
            komut.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
            komut.Parameters.AddWithValue("@Adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@Email", txtEmail.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["Santiye"].Clear();
            Kayıt_Göster();
            MessageBox.Show("Şantiye Kaydı Güncellendi");
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from Santiye where SantiyeAdi'" +dataGridView1.CurrentRow.Cells["SantiyeAdi"].Value.ToString()+"'",baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["Santiye"].Clear();
            Kayıt_Göster();
            MessageBox.Show("Kayıt Silindi");

        }

        private void txtSantiyeAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select *from Santiye where SantiyeAdi like '%"+txtSantiyeAra.Text+"%'",baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void txtSantiyeAdi_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
