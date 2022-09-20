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
    public partial class frmMalzemeListele : Form
    {
        public frmMalzemeListele()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=ERTUGRUL-OZCAN;Initial Catalog=Santiye_Takip;Integrated Security=True");
        DataSet daset = new DataSet();

        private void Kategori_Getir()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from KategoriBilgileri", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboKategori.Items.Add(read["Kategori"].ToString());
            }
            baglanti.Close();
        }

        private void frmMalzemeListele_Load(object sender, EventArgs e)
        {
            Malzeme_Listele();
            Kategori_Getir();
        }

        private void Malzeme_Listele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select *from Malzeme", baglanti);
            adtr.Fill(daset, "Malzeme");
            dataGridView1.DataSource = daset.Tables["Malzeme"];
            baglanti.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MalzemeNotxt.Text = dataGridView1.CurrentRow.Cells["MalzemeNo"].Value.ToString();
            Kategoritxt.Text = dataGridView1.CurrentRow.Cells["Kategori"].Value.ToString();
            Markatxt.Text = dataGridView1.CurrentRow.Cells["Marka"].Value.ToString();
            MalzemeAdtxt.Text = dataGridView1.CurrentRow.Cells["MalzemeAd"].Value.ToString();
            Miktartxt.Text = dataGridView1.CurrentRow.Cells["Miktar"].Value.ToString();
            AlışFiyatıtxt.Text = dataGridView1.CurrentRow.Cells["AlisFiyatı"].Value.ToString();
            ToplamFiyattxt.Text = dataGridView1.CurrentRow.Cells["ToplamFiyat"].Value.ToString();
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update Malzeme set MalzemeAd=@MalzemeAd,Miktar=@Miktar,AlisFiyatı=@AlisFiyatı,ToplamFiyat=@ToplamFiyat where MalzemeNo=@MalzemeNo",baglanti);
            komut.Parameters.AddWithValue("@MalzemeNo",MalzemeNotxt.Text);
            komut.Parameters.AddWithValue("@MalzemeAd", MalzemeAdtxt.Text);
            komut.Parameters.AddWithValue("@Miktar", int.Parse(Miktartxt.Text));
            komut.Parameters.AddWithValue("@AlisFiyatı", double.Parse(AlışFiyatıtxt.Text));
            komut.Parameters.AddWithValue("@ToplamFiyat", double.Parse(ToplamFiyattxt.Text));
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Güncelleme Yapıldı!");

            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void btnMarkaGuncelle_Click(object sender, EventArgs e)
        {
            if (MalzemeNotxt.Text !="")
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("update Malzeme set Kategori=@Kategori,Marka=@Marka where MalzemeNo=@MalzemeNo", baglanti);
                komut.Parameters.AddWithValue("@MalzemeNo", MalzemeNotxt.Text);
                komut.Parameters.AddWithValue("@Kategori", comboKategori.Text);
                komut.Parameters.AddWithValue("@Marka", comboMarka.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();
                daset.Tables["Malzeme"].Clear();
                Malzeme_Listele();
                MessageBox.Show("Güncelleme Yapıldı!");
                daset.Tables["Malzeme"].Clear();
                Malzeme_Listele();
            }
            else
            {
                MessageBox.Show("Malzeme No Yazılı Değil!");
            }

            foreach (Control item in this.Controls)
            {
                if (item is ComboBox)
                {
                    item.Text = "";
                }
            }
        }

        private void comboKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMarka.Items.Clear();
            comboMarka.Text = "";
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from MarkaBilgileri where kategori='" + comboKategori.SelectedItem + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboMarka.Items.Add(read["Marka"].ToString());
            }
            baglanti.Close();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from Malzeme where MalzemeNo='" + dataGridView1.CurrentRow.Cells["MalzemeNo"].Value.ToString() + "'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["Malzeme"].Clear();
            Malzeme_Listele();
            MessageBox.Show("Kayıt Silindi");
        }

        private void txtMalzemeNoAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select *from Malzeme where MalzemeNo like '%" + txtMalzemeNoAra.Text + "%'", baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
    }
}
