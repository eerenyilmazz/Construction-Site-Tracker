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
    public partial class frmAlis : Form
    {
        public frmAlis()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=ERTUGRUL-OZCAN;Initial Catalog=Santiye_Takip;Integrated Security=True");

        DataSet daset = new DataSet();

        private void StokListele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select *from Sepet",baglanti);
            adtr.Fill(daset,"Sepet");
            dataGridView1.DataSource = daset.Tables["Sepet"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            baglanti.Close();

        }

        private void btnStokEksilt_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into StokEksilt(SantiyeAdi,FirmaAdi,ProjeMaliyeti,Telefon,Adres,Email,MalzemeNo,MalzemeAd,Miktar,Alisfiyatı,ToplamFiyat,Tarih) values(@SantiyeAdi,@FirmaAdi,@ProjeMaliyeti,@Telefon,@Adres,@Email,@MalzemeNo,@MalzemeAd,@Miktar,@Alisfiyatı,@ToplamFiyat,@Tarih)", baglanti);
                komut.Parameters.AddWithValue("@SantiyeAdi", txtSantiyeAdi.Text);
                komut.Parameters.AddWithValue("@FirmaAdi", txtFirmaAdi.Text);
                komut.Parameters.AddWithValue("@ProjeMaliyeti", txtProjeMaliyeti.Text);
                komut.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@Adres", txtAdres.Text);
                komut.Parameters.AddWithValue("@Email", txtEmail.Text);
                komut.Parameters.AddWithValue("@MalzemeNo", dataGridView1.Rows[i].Cells["MalzemeNo"].Value.ToString());
                komut.Parameters.AddWithValue("@MalzemeAd", dataGridView1.Rows[i].Cells["MalzemeAd"].Value.ToString());
                komut.Parameters.AddWithValue("@Miktar", int.Parse(dataGridView1.Rows[i].Cells["Miktar"].Value.ToString()));
                komut.Parameters.AddWithValue("@AlisFiyatı", double.Parse(dataGridView1.Rows[i].Cells["AlisFiyatı"].Value.ToString()));
                komut.Parameters.AddWithValue("@ToplamFiyat", double.Parse(dataGridView1.Rows[i].Cells["ToplamFiyat"].Value.ToString()));
                komut.Parameters.AddWithValue("@Tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                SqlCommand komut2 = new SqlCommand("update Malzeme set Miktar=Miktar-'" + int.Parse(dataGridView1.Rows[i].Cells["Miktar"].Value.ToString()) + "' where MalzemeNo= '" + dataGridView1.Rows[i].Cells["MalzemeNo"].Value.ToString() + "'",baglanti);
                komut2.ExecuteNonQuery();
                baglanti.Close();
           
               
            }
            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("delete from Sepet", baglanti);
            komut3.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["Sepet"].Clear();
            StokListele();
            Hesapla();

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmSantiyeEkle ekle = new frmSantiyeEkle();
            ekle.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmMalzemeEkle ekle = new frmMalzemeEkle();
            ekle.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmSantiyeLisetele listele = new frmSantiyeLisetele();
            listele.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmKategori kategori = new frmKategori();
            kategori.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            frmMarka marka = new frmMarka();
            marka.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmMalzemeListele listele = new frmMalzemeListele();
            listele.ShowDialog();
        }

        private void Hesapla()
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select sum(ToplamFiyat) from Sepet",baglanti);
                lblGenelToplam.Text = komut.ExecuteScalar() + " TL";
                baglanti.Close();
            }
            catch (Exception)
            {

                ;
            }
        }

        private void frmAlis_Load(object sender, EventArgs e)
        {
            StokListele();
        }

        private void txtSantiyeAdi_TextChanged(object sender, EventArgs e)
        {
            if (txtSantiyeAdi.Text=="")
            {
                txtFirmaAdi.Text = "";
                txtProjeMaliyeti.Text = "";
                txtTelefon.Text = "";
                txtAdres.Text = "";
                txtEmail.Text = "";
            }

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from Santiye where SantiyeAdi like '"+txtSantiyeAdi.Text+"'",baglanti);
            SqlDataReader read = komut.ExecuteReader();

            while (read.Read())
            {
                txtFirmaAdi.Text = read["FirmaAdi"].ToString();
                txtProjeMaliyeti.Text = read["ProjeMaliyeti"].ToString();
                txtTelefon.Text = read["Telefon"].ToString();
                txtAdres.Text = read["Adres"].ToString();
                txtEmail.Text = read["Email"].ToString();
            }
            baglanti.Close();
        }

        private void txtMalzemeNo_TextChanged(object sender, EventArgs e)
        {
            Temizle();

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from Malzeme where MalzemeNo like '" + txtMalzemeNo.Text + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();

            while (read.Read())
            {
                txtMalzemeAd.Text = read["MalzemeAd"].ToString();
                txtAlisFiyati.Text = read["AlisFiyatı"].ToString();
            }
            baglanti.Close();
        }

        private void Temizle()
        {
            if (txtMalzemeNo.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMiktar)
                        {
                            item.Text = "";
                        }
                    }

                }
            }
        }

        private void txtMiktar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtAlisFiyati.Text)).ToString();
            }
            catch (Exception)
            {

                ;
            }
        }

        bool durum;
        private void MalzemeNoKontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from sepet",baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtMalzemeNo.Text==read["MalzemeNo"].ToString())
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            MalzemeNoKontrol();
            if (durum==true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into Sepet(SantiyeAdi,FirmaAdi,ProjeMaliyeti,Telefon,Adres,Email,MalzemeNo,MalzemeAd,Miktar,Alisfiyatı,ToplamFiyat,Tarih) values(@SantiyeAdi,@FirmaAdi,@ProjeMaliyeti,@Telefon,@Adres,@Email,@MalzemeNo,@MalzemeAd,@Miktar,@Alisfiyatı,@ToplamFiyat,@Tarih)", baglanti);
                komut.Parameters.AddWithValue("@SantiyeAdi", txtSantiyeAdi.Text);
                komut.Parameters.AddWithValue("@FirmaAdi", txtFirmaAdi.Text);
                komut.Parameters.AddWithValue("@ProjeMaliyeti", txtProjeMaliyeti.Text);
                komut.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@Adres", txtAdres.Text);
                komut.Parameters.AddWithValue("@Email", txtEmail.Text);
                komut.Parameters.AddWithValue("@MalzemeNo", txtMalzemeNo.Text);
                komut.Parameters.AddWithValue("@MalzemeAd", txtMalzemeAd.Text);
                komut.Parameters.AddWithValue("@Miktar", int.Parse(txtMiktar.Text));
                komut.Parameters.AddWithValue("@AlisFiyatı", double.Parse(txtAlisFiyati.Text));
                komut.Parameters.AddWithValue("@ToplamFiyat", double.Parse(txtToplamFiyat.Text));
                komut.Parameters.AddWithValue("@Tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            else
            {
                baglanti.Open();
                SqlCommand komut2 = new SqlCommand("update sepet set Miktar=Miktar+'"+ int.Parse(txtMiktar.Text)+ "' where MalzemeNo='" + txtMalzemeNo.Text + "' ", baglanti);
                komut2.ExecuteNonQuery();

                SqlCommand komut3 = new SqlCommand("update sepet set ToplamFiyat=Miktar*AlisFiyatı where MalzemeNo='"+txtMalzemeNo.Text+"'", baglanti);
                komut3.ExecuteNonQuery();

                baglanti.Close();

            }
            
            txtMiktar.Text = "1";
            daset.Tables["Sepet"].Clear();
            StokListele();
            Hesapla();

            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtMiktar)
                    {
                        item.Text = "";
                    }
                }

            }

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from Sepet where MalzemeNo= '"+dataGridView1.CurrentRow.Cells["MalzemeNo"].Value.ToString() +"'",baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Malzeme Stoktan Silindi");
            daset.Tables["Sepet"].Clear();
            StokListele();
            Hesapla();

        }

        private void btnAlisIptal_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from Sepet", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Malzeme Stoktan Silindi");
            daset.Tables["Sepet"].Clear();
            StokListele();
            Hesapla();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmStokListele listele = new frmStokListele();
            listele.ShowDialog();
        }
    }
}
