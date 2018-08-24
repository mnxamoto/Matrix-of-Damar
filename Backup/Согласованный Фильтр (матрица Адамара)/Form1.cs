using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Согласованный_Фильтр__матрица_Адамара_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int N;
        long[,] A = new long[16383, 16383];

        private void PodgotovTabl()
        {
            A[0, 0] = 1;
            A[0, 1] = 1;
            A[1, 0] = 1;
            A[1, 1] = -1;

            Matrix_Adamara(2);
            dataGridView1.Columns.Clear();
            dataGridView3.Columns.Clear();
            dataGridView4.Columns.Clear();
            for (int i = 0; i < N; i++)
            {
                dataGridView1.Columns.Add("Column1_" + Convert.ToString(i), Convert.ToString(i));
                dataGridView1.Columns[i].Width = 23;
                dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dataGridView3.Columns.Add("Column3_" + Convert.ToString(i), Convert.ToString(i));
                dataGridView3.Columns[i].Width = 50;
                dataGridView3.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dataGridView4.Columns.Add("Column4_" + Convert.ToString(i), "Uвых" + Convert.ToString(i + 1));
                dataGridView4.Columns[i].Width = 50;
                dataGridView4.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(N);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = A[i, j];

                    if (A[i, j] == (-1))
                    {
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Black;
                        dataGridView1.Rows[i].Cells[j].Style.ForeColor = Color.White;
                    }
                }
            }

            dataGridView1.Columns.Add("Column1Var", "");
            dataGridView1.Columns[N].Width = 23;
            dataGridView1.Columns[N].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[N].DefaultCellStyle.BackColor = Color.Lime;
            for (int i = 0; i < N; i++)
            {
                dataGridView1.Rows[i].Cells[N].Value = Convert.ToString(i + 1);
            }

            dataGridView3.Columns.Add("Column3Var", "");
            dataGridView3.Columns[N].Width = 100;
            dataGridView3.Columns[N].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView3.Columns[N].DefaultCellStyle.BackColor = Color.Lime;

            dataGridView4.Columns.Add("Column4_t", "t");
            dataGridView4.Columns[N].Width = 100;
            dataGridView4.Columns[N].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView4.Columns[N].DefaultCellStyle.BackColor = Color.Lime;

            O4istkaTbl();
        }

        private void Matrix_Adamara(int M)
        {
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    A[i + M, j] = A[i, j];
                    A[i, j + M] = A[i, j];
                    A[i + M, j + M] = A[i, j] * (-1);
                }
            }
            if (M < (N / 2))
            {
                Matrix_Adamara(M * 2);
            }
        }

        private void O4istkaTbl()
        {
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();

            dataGridView3.Rows.Add(1);
            dataGridView4.Rows.Add((N * 2) - 1);

            for (int i = 0; i < (N * 2) - 1; i++)
            {
                dataGridView4.Rows[i].Cells[N].Value = Convert.ToString(i + 1);
            }
            dataGridView3.Rows[0].Cells[N].Value = "Обратная строка";
            dataGridView4.Rows[N - 1].DefaultCellStyle.BackColor = Color.Orange;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            O4istkaTbl();

            dataGridView4.Columns[e.RowIndex].DefaultCellStyle.BackColor = Color.GreenYellow;

            for (int i = 0; i < N; i++)
            {
                dataGridView3.Rows[0].Cells[N - 1 - i].Value = Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[i].Value);
            }

            for (int k = 0; k < N; k++)
            {
                for (int i = 0; i < (N * 2) - 1; i++)
                {
                    for (int j = 0; j < i + 1; j++)
                    {
                        if (((i - j) < N) && (j < N))
                        {
                            dataGridView4.Rows[i].Cells[k].Value = Convert.ToInt16(dataGridView4.Rows[i].Cells[k].Value) + Convert.ToInt16(dataGridView3.Rows[0].Cells[j].Value) * Convert.ToInt16(dataGridView1.Rows[k].Cells[i - j].Value);
                        }
                    }
                }
            }

            tabControl1.SelectTab(1);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if ((Math.Log(Convert.ToInt32(textBox1.Text), 2) % 1) == 0)
                {
                    N = Convert.ToInt32(textBox1.Text);
                    
                    PodgotovTabl();
                    Bitmap bmp = new Bitmap(N, N);     //изображение
                    pictureBox1.Image = bmp;
                    Graphics g = Graphics.FromImage(pictureBox1.Image);
                    g.Clear(Color.Gray);
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            if (A[i, j] == (1))
                            {
                                g.FillRectangle(Brushes.White, i, j, 1, 1);
                            }
                            else
                            {
                                g.FillRectangle(Brushes.Black, i, j, 1, 1);
                            }
                        }
                    }
                    pictureBox1.Image.Save("moto " + N + "x" + N + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                }
                else
                {
                    MessageBox.Show("Введено не корректное число.\r\nРазмерность матрицы должна быть равна степени 2.\r\nБлижайшее число: " + Convert.ToString(Math.Pow(2,(Convert.ToInt32(Math.Log(Convert.ToInt32(textBox1.Text), 2) / 1)))), "Варнинг!!!");
                }
            }
        }
    }
}
