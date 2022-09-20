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
    public partial class frmMalzemeEkle : Form
    {
        public frmMalzemeEkle()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=ERTUGRUL-OZCAN;Initial Catalog=Santiye_Takip;Integrated Security=True");


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

        private void frmMalzemeEkle_Load(object sender, EventArgs e)
        {
            Kategori_Getir();
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

        private void btnYeniEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into Malzeme(MalzemeNo,Kategori,Marka,MalzemeAd,Miktar,AlisFiyatı,ToplamFiyat,Tarih) values(@MalzemeNo,@Kategori,@Marka,@MalzemeAd,@Miktar,@AlisFiyatı,@ToplamFiyat,@Tarih)", baglanti);
            komut.Parameters.AddWithValue("@MalzemeNo",txtMalzemeNo.Text);
            komut.Parameters.AddWithValue("@Kategori", comboKategori.Text);
            komut.Parameters.AddWithValue("@Marka", comboMarka.Text);
            komut.Parameters.AddWithValue("@MalzemeAd", txtMalzemeAd.Text);
            komut.Parameters.AddWithValue("@Miktar", int.Parse(txtMiktar.Text));
            komut.Parameters.AddWithValue("@AlisFiyatı", double.Parse(txtAlışFiyatı.Text));
            komut.Parameters.AddWithValue("@ToplamFiyat", double.Parse(txtToplamFiyat.Text));
            komut.Parameters.AddWithValue("@Tarih", DateTime.Now.ToString());

            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Malzeme Eklendi");
            comboMarka.Items.Clear();

            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";

                }
                if (item is ComboBox)
                {
                    item.Text = "";
                }
            }

        }

        private void MalzemeNotxt_TextChanged(object sender, EventArgs e)
        {
            if (MalzemeNotxt.Text=="")
            {
                lblMiktar.Text = "";
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        item.Text = "";
                    }
                }
            }

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from Malzeme where MalzemeNo like '" + MalzemeNotxt.Text + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                Kategoritxt.Text = read["Kategori"].ToString();
                Markatxt.Text = read["Marka"].ToString();
                MalzemeAdtxt.Text = read["MalzemeAd"].ToString();
                lblMiktar.Text = read["Miktar"].ToString();
                AlışFiyatıtxt.Text = read["AlisFiyatı"].ToString();
                ToplamFiyattxt.Text = read["ToplamFiyat"].ToString();

            }
            baglanti.Close();
        }

        private void btnVarOlanaEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update Malzeme set Miktar=Miktar+'" + int.Parse(Miktartxt.Text) + "' where MalzemeNo= '" + MalzemeNotxt.Text + "'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();

            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
            MessageBox.Show("Var olan malzemeye ekleme yapıldı");
        }
    }
}
