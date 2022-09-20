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
    public partial class frmStokListele : Form
    {
        public frmStokListele()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=ERTUGRUL-OZCAN;Initial Catalog=Santiye_Takip;Integrated Security=True");
        DataSet daset = new DataSet();

        private void StokEksilt()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select *from StokEksilt", baglanti);
            adtr.Fill(daset, "StokEksilt");
            dataGridView1.DataSource = daset.Tables["StokEksilt"];
            baglanti.Close();

        }

        private void frmStokListele_Load(object sender, EventArgs e)
        {
            StokEksilt();
        }


    }
}
