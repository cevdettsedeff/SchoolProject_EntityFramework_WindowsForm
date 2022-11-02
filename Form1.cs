using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // ders listesinde entity metodu kullanmadığımız için bu kütüphaneyi eklemek zorundayız.
using System.Reflection;
using System.Collections;

namespace SchoolProject_EntityFramework_WindowsForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void BtnLecturesList_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=CEVDET\SQLEXPRESS;Initial Catalog=DbSchoolStudent;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("Select * From tbllectures",connection);
            SqlDataAdapter dataAdapter= new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            dataGridView1.DataSource = dt;
        }


        DbSchoolStudentEntities db = new DbSchoolStudentEntities();
        private void BtnStudentList_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TBLSTUDENT.ToList();

            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }

        private void BtnGradesList_Click(object sender, EventArgs e)
        {
            var query = from item in db.TBLGRADES
                        select new { item.GRADEID, item.STD, item.EXAM1, item.EXAM2, item.EXAM3, item.AVERAGE, item.STATE };
            dataGridView1.DataSource = query.ToList();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            TBLSTUDENT t = new TBLSTUDENT();
            t.NAME = txtStdName.Text;
            t.SURNAME = txtStdSurname.Text;
            db.TBLSTUDENT.Add(t);
            db.SaveChanges();
            MessageBox.Show("Student Added Successfully");
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtStdId.Text);
            var x = db.TBLSTUDENT.Find(id);
            db.TBLSTUDENT.Remove(x);
            db.SaveChanges();
            MessageBox.Show("Student Deleted Successfully");
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtStdId.Text);
            var x = db.TBLSTUDENT.Find(id);
            x.NAME=txtStdName.Text;
            x.SURNAME=txtStdSurname.Text;
            db.SaveChanges();
            MessageBox.Show("Student Updated Successfully");
        }

        //                                      PROCEDURE KODLARI (SQL)
        // CREATE PROCEDURE GRADELIST
        //AS
        //SELECT GRADEID,NAME + ' '+ SURNAME AS 'NAME SURNAME', LECTURENAME, EXAM1, EXAM2, EXAM3, AVERAGE, STATE
        //FROM TBLGRADES
        //INNER JOIN TBLSTUDENT ON TBLGRADES.STD=TBLSTUDENT.ID
        //INNER JOIN TBLLECTURES ON TBLLECTURES.LECTUREID=TBLGRADES.LECTURE

        private void BtnProcedure_Click(object sender, EventArgs e)
        {
            // Prosedürü ado.net yaklaşımı ile aşağıdaki gibi de kullanabiliriz.

            //SqlConnection connection = new SqlConnection(@"Data Source=CEVDET\SQLEXPRESS;Initial Catalog=DbSchoolStudent;Integrated Security=True");
            //SqlCommand cmd = new SqlCommand("Execute GRADELIST", connection);
            //SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            //dataAdapter.Fill(dt);
            //dataGridView1.DataSource = dt;

            dataGridView1.DataSource=db.GRADELIST();

        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TBLSTUDENT.Where(x=>x.NAME == txtStdName.Text | x.SURNAME == txtStdSurname.Text).ToList();
        }

        private void BtnLinqEntity_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked== true)
            {
                // Ascending - Asc
                List<TBLSTUDENT> list1 = db.TBLSTUDENT.OrderBy(x => x.NAME).ToList();
                dataGridView1.DataSource = list1;
            }

            if(radioButton2.Checked== true)
            {
                // Descending - Dsc
                List<TBLSTUDENT> list2 = db.TBLSTUDENT.OrderByDescending(x => x.NAME).ToList();
                dataGridView1.DataSource = list2;
            }

            if(radioButton3.Checked == true)
            {
                List<TBLSTUDENT> list3 = db.TBLSTUDENT.OrderBy(x=>x.ID).Take(3).ToList();
                dataGridView1.DataSource = list3;
            }

            if(radioButton4.Checked == true)
            {
                List<TBLSTUDENT> list4 = db.TBLSTUDENT.Where(x=>x.ID == 3).ToList();
                dataGridView1.DataSource = list4;

            }

            if(radioButton5.Checked==true)
            {
                var average1 = db.TBLGRADES.Average(p => p.EXAM1);
                MessageBox.Show("The average of Exam 1: " +  average1.ToString());
            }

            if (radioButton6.Checked == true)
            {
                var average2 = db.TBLGRADES.Average(p => p.EXAM2);
                MessageBox.Show("The average of Exam 2: " + average2.ToString());
            }

            if (radioButton7.Checked == true)
            {
                var average3 = db.TBLGRADES.Average(p => p.EXAM3);
                MessageBox.Show("The average of Exam 3: " + average3.ToString());
            }

            if(radioButton8.Checked == true)
            {
                var maxgrade1= db.TBLGRADES.Max(p => p.EXAM1);
                MessageBox.Show("The maximum grade of Exam 1: " + maxgrade1.ToString());
            }

            if(radioButton9.Checked == true)
            {
                int numberstd = db.TBLSTUDENT.Count();
                MessageBox.Show("Number of Students: " + numberstd.ToString());
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
