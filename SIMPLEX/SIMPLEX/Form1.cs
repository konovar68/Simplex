using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIMPLEX
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int s1, s2;
        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Visible = false;
        }
        void datagrid()
        {
            dataGridView1.Columns.Clear();
            s1 = int.Parse(textBox1.Text);
            s2 = int.Parse(textBox2.Text);
            for (int i = 0; i < s2; i++)
            {
                dataGridView1.Columns.Add("column1", "");
                dataGridView1.Columns[i].Width = 30;
            }
            for (int i = 0; i < s1; i++)
                dataGridView1.Rows.Add();
            
        }
        public class Simplex
        {
            //source - симплекс таблица без базисных переменных
            double[,] table; //симплекс таблица

            int m, n;

            List<int> basis; //список базисных переменных

            public Simplex(double[,] source)
            {
                m = source.GetLength(0);
                n = source.GetLength(1);
                table = new double[m, n + m - 1];
                basis = new List<int>();

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < table.GetLength(1); j++)
                    {
                        if (j < n)
                            table[i, j] = source[i, j];
                        else
                            table[i, j] = 0;
                    }
                    //выставляем коэффициент 1 перед базисной переменной в строке
                    if ((n + i) < table.GetLength(1))
                    {
                        table[i, n + i] = 1;
                        basis.Add(n + i);
                    }
                }

                n = table.GetLength(1);
            }

            //result - в этот массив будут записаны полученные значения X
            public double[,] Calculate(double[] result)
            {
                int mainCol, mainRow; //ведущие столбец и строка

                while (!IsItEnd())
                {
                    mainCol = findMainCol();
                    mainRow = findMainRow(mainCol);
                    basis[mainRow] = mainCol;

                    double[,] new_table = new double[m, n];

                    for (int j = 0; j < n; j++)
                        new_table[mainRow, j] = table[mainRow, j] / table[mainRow, mainCol];

                    for (int i = 0; i < m; i++)
                    {
                        if (i == mainRow)
                            continue;

                        for (int j = 0; j < n; j++)
                            new_table[i, j] = table[i, j] - table[i, mainCol] * new_table[mainRow, j];
                    }
                    table = new_table;
                }

                //заносим в result найденные значения X
                for (int i = 0; i < result.Length; i++)
                {
                    int k = basis.IndexOf(i + 1);
                    if (k != -1)
                        result[i] = table[k, 0];
                    else
                        result[i] = 0;
                }

                return table;
            }

            private bool IsItEnd()
            {
                bool flag = true;

                for (int j = 1; j < n; j++)
                {
                    if (table[m - 1, j] < 0)
                    {
                        flag = false;
                        break;
                    }
                }

                return flag;
            }

            private int findMainCol()
            {
                int mainCol = 1;

                for (int j = 2; j < n; j++)
                    if (table[m - 1, j] < table[m - 1, mainCol])
                        mainCol = j;

                return mainCol;
            }

            private int findMainRow(int mainCol)
            {
                int mainRow = 0;

                for (int i = 0; i < m - 1; i++)
                    if (table[i, mainCol] > 0)
                    {
                        mainRow = i;
                        break;
                    }

                for (int i = mainRow + 1; i < m - 1; i++)
                    if ((table[i, mainCol] > 0) && ((table[i, 0] / table[i, mainCol]) < (table[mainRow, 0] / table[mainRow, mainCol])))
                        mainRow = i;

                return mainRow;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            datagrid();
            button1.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text= 4.ToString();
            s2 = s1 = 4;

            button1.Visible = true;

            dataGridView1.Columns.Clear();
            for (int i = 0; i < 4; i++)
            {
                dataGridView1.Columns.Add("column1", "");
                dataGridView1.Columns[i].Width = 30;
            }
            for (int j = 0; j < 4; j++)
                dataGridView1.Rows.Add();
            //double[,] table = { {30, 1,  1, 3}, //x1+x2+3х3<=30
            //                    {24, 2, 2, 5},//2x1+2x2+5х3<=24
            //                    {36,  4, 1, 2},//4x1+х2+2х3<=36
            //                    { 0, -3, -1, -2} }; //z= 3x1 + x2+2х3->max

            dataGridView1[0, 0].Value = 30;
            dataGridView1[1, 0].Value = 1;
            dataGridView1[2, 0].Value = 1;
            dataGridView1[3, 0].Value = 3;

            dataGridView1[0, 1].Value = 24;
            dataGridView1[1, 1].Value = 2;
            dataGridView1[2, 1].Value = 2;
            dataGridView1[3, 1].Value = 5;

            dataGridView1[0, 2].Value = 36;
            dataGridView1[1, 2].Value = 4;
            dataGridView1[2, 2].Value = 1;
            dataGridView1[3, 2].Value = 2;

            dataGridView1[0, 3].Value = 0;
            dataGridView1[1, 3].Value = -3;
            dataGridView1[2, 3].Value = -1;
            dataGridView1[3, 3].Value = -2;
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Columns.Clear();

            dataGridView3.Columns.Clear();
            dataGridView3.Columns.Add("column1", "");
            dataGridView2.ReadOnly = true;
            dataGridView3.ReadOnly = true;
            dataGridView3.Columns[0].Width = 30;

            double[,] table = new double[s1, s2];
            for (int i = 0; i < s1; i++)
                for (int j = 0; j < s2; j++)
                    table[i, j] = Convert.ToDouble(dataGridView1[j, i].Value);

            double[] result = new double[s2 - 1];                    

            double[,] table_result;
            Simplex S = new Simplex(table);
            table_result = S.Calculate(result);

            for (int i = 0; i < table_result.GetLength(1); i++)
            {
                dataGridView2.Columns.Add("column1", "");
                dataGridView2.Columns[i].Width = 30;
            }
            for (int j = 0; j < table_result.GetLength(0); j++)
                dataGridView2.Rows.Add();
            
            for (int i = 0; i < table_result.GetLength(0); i++)
            {
                for (int j = 0; j < table_result.GetLength(1); j++)
                    dataGridView2[j, i].Value = table_result[i, j];                    
            }

            for (int i = 0; i < result.Length; i++)
            {
                dataGridView3.Rows.Add();
                dataGridView3[0, i].Value = result[i];
            }
        }
    }
}
