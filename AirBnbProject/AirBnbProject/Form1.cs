using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace AirBnbProject
{
    public partial class Form1 : Form
    {
        private List<Accommodation> accommodations;

        internal List<Accommodation> Accommodations { get => accommodations; set => accommodations = value; }

        public Form1()
        {
            InitializeComponent();
            Accommodations = new List<Accommodation>();

        }




        private void LoadSQLData()
        {
            SQLClass mySql = new SQLClass();
            Accommodations = mySql.GetListAccomodation(this);



            City amsterdam = new City
            {
                CityName = "Amsterdam",
                Population = 3000000,
                TouristsPerYear = 700000,
                AvgIncome = 30000
            };
            //filtrerar listan och stoppar in alla amsterdam i amsterdamlistan
            List<Accommodation> amstlist = new List<Accommodation>();
            amstlist = Accommodations.Where(a => a.CityName == "Amsterdam").ToList();


            City barcelona = new City
            {
                CityName = "Barcelona",
                Population = 2000000,
                TouristsPerYear = 1000000,
                AvgIncome = 20000
            };

            List<Accommodation> barcalist = new List<Accommodation>();
            barcalist = Accommodations.Where(b => b.CityName == "Barcelona").ToList();
            City boston = new City
            {
                CityName = "Boston",
                Population = 4000000,
                TouristsPerYear = 4000,
                AvgIncome = 50000
            };

            List<Accommodation> bostonlist = new List<Accommodation>();
            bostonlist = Accommodations.Where(b => b.CityName == "Boston").ToList();


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            chart1.Hide();
            LoadSQLData();
            progressBar1.Hide();
            chart1.Series.Clear();
            chart1.Show();
            PopulateBoxes();

        }
        private void PopulateBoxes()
        {

            List<Accommodation> accToFoolaroundWith = new List<Accommodation>();
            cbX.Items.Add(new CbBoxItem("Price", "Price"));
            cbX.Items.Add(new CbBoxItem("Overall Satisfaction", "Overall Satisfaction"));
            cbX.Items.Add(new CbBoxItem("Room type", "Room type"));

            cbY.Items.Add(new CbBoxItem("Price", "Price"));
            cbY.Items.Add(new CbBoxItem("Overall Satisfaction", "Overall Satisfaction"));
            cbY.Items.Add(new CbBoxItem("Units", "Units", "Units"));

            //Hämtar menyer av typen count of x_room type 
            accToFoolaroundWith = Accommodations.GroupBy(a => a.Room_type).Select(a => a.FirstOrDefault()).ToList();
            string[] xsall = accToFoolaroundWith.Select(a => a.Room_type).ToArray();
            //rensar bort nullvärden och gör om det till en lista
            List<string> roomtypes = GetCleanListOfOccurencies(xsall);
            foreach (string s in roomtypes)
            {
                cbY.Items.Add(new CbBoxItem("Count of " + s, s, "Room type"));

            }



            cbCity.Items.Add(new CbBoxItem("All Cities", "All Cities"));
            cbCity.Items.Add(new CbBoxItem("Amsterdam", "Amsterdam"));
            cbCity.Items.Add(new CbBoxItem("Barcelona", "Barcelona"));
            cbCity.Items.Add(new CbBoxItem("Boston", "Boston"));
            cbCity.SelectedIndex = 0;
            cbGraphType.Items.Add(new CbBoxItem("Scatterplot", "Scatterplot"));
            cbGraphType.Items.Add(new CbBoxItem("Bar", "Bar"));
            cbGraphType.SelectedIndex = 0;
        }

        private void btnPlot_Click(object sender, EventArgs e)
        {
            if (cbY.SelectedIndex != -1 || cbX.SelectedIndex != -1)
            {
                PrintChart();
            }
            else
            {
                MessageBox.Show("Please make your selections");
            }
        }




        private void PrintChart()
        {



            //Hämtar komboxesvalen
            bool add = checkBox1.Checked;
            CbBoxItem CBXX = (CbBoxItem)cbX.SelectedItem;
            string pickedX = CBXX.Value;
            CbBoxItem CBYY = (CbBoxItem)cbY.SelectedItem;
            string pickedY = CBYY.Value;
            CbBoxItem CBCity1 = (CbBoxItem)cbCity.SelectedItem;
            string pickedCity = CBCity1.Value;
            CbBoxItem CBGraph = (CbBoxItem)cbGraphType.SelectedItem;
            string pickedGraph = CBGraph.Value;
            bool barIsPossible = true;
            //Tar reda på om Bar är möjlig

            foreach (CbBoxItem bi in cbY.Items)
            {
                if (CBXX.Value == bi.Value)
                {
                    //Vald x finns i y, Kollar upp om det är valt
                    foreach (CbBoxItem bit in cbX.Items)
                    {
                        if (CBYY.Value == bit.Value)
                        {
                            //Bar är inte möjlig

                            barIsPossible = false;
                            if (pickedGraph == "Bar")
                            {
                                MessageBox.Show("Bar is not possible");
                            }
                        }
                    }
                }
            }
            List<Accommodation> sortedAcc = new List<Accommodation>();
            List<Accommodation> accBigData = new List<Accommodation>();
            accBigData = Accommodations;

            //tar fram rätt stad
            if (pickedCity != "All Cities")
            {
                accBigData = accBigData.Where(a => a.CityName == pickedCity).ToList();
            }

            //Den ska sortera på x axeln, gör en switch
            //tar reda på vad det är för datatyp vald i x axeln
            double[] x = { };
            List<string> xs = new List<string>();
            bool graphHasStringValuesOnXAxis = false;
            switch (pickedX)
            {
                case "Price":
                    sortedAcc = accBigData.OrderBy(a => a.Price).ToList();
                    x = sortedAcc.Select(a => (double)a.Price).ToArray();

                    break;
                case "Overall Satisfaction":
                    sortedAcc = accBigData.OrderBy(a => a.Overall_satisfaction).ToList();
                    x = sortedAcc.Select(a => a.Overall_satisfaction).ToArray();

                    break;
                case "Room type":
                    graphHasStringValuesOnXAxis = true;
                    sortedAcc = accBigData.GroupBy(a => a.Room_type).Select(a => a.FirstOrDefault()).ToList();
                    string[] xsall = sortedAcc.Select(a => a.Room_type).ToArray();
                    //rensar bort nullvärden och gör om det till en lista
                    xs = GetCleanListOfOccurencies(xsall);

                    break;
                default:
                    MessageBox.Show("Something went wrong. Pick X axis");

                    break;
            }

            //Hämtar y
            //Den ska sortera på x axeln, gör en switch
            double[] y = { };
            if (!graphHasStringValuesOnXAxis)
            {
                //tar reda på om det är en count nån klickat i
                if (CBYY.ToString().Contains("Count of") || CBYY.ToString().Contains("Units"))
                {
                    //Man har valt ett count of objekt i Y, tar reda på vilket o räknar
                    switch (CBYY.AccObjName)
                    {
                        case "Room type":
                            string comparestring = CBYY.Value;


                            List<Accommodation> myCalcListAcc = sortedAcc.Where(a => a.Room_type == comparestring).ToList();
                            List<double> ys = new List<double>();
                            foreach (Accommodation a in sortedAcc)
                            {
                                switch (pickedX)
                                {
                                    case "Price":
                                        //För varje x räkna antalet y och stoppa in i ys
                                        ys.Add((double)myCalcListAcc.Where(p => p.Price == a.Price).Count());
                                        break;
                                    case "Overall Satisfaction":
                                        //För varje x räkna antalet y och stoppa in i ys
                                        ys.Add((double)myCalcListAcc.Where(p => p.Overall_satisfaction == a.Overall_satisfaction).Count());
                                        break;
                                    default:
                                        break;
                                }

                            }
                            y = ys.ToArray();

                            break;
                        case "Units":
                            List<double> ys1 = new List<double>();
                            foreach (Accommodation a in sortedAcc)
                            {
                                switch (pickedX)
                                {
                                    case "Price":
                                        //För varje x räkna antalet y och stoppa in i ys
                                        ys1.Add((double)sortedAcc.Where(p => p.Price == a.Price).Count());
                                        break;
                                    case "Overall Satisfaction":
                                        //För varje x räkna antalet y och stoppa in i ys
                                        ys1.Add((double)sortedAcc.Where(p => p.Overall_satisfaction == a.Overall_satisfaction).Count());
                                        break;
                                    default:
                                        break;
                                }

                            }
                            y = ys1.ToArray();
                            break;
                    }

                }
                else //Vanlig picked
                {
                    switch (pickedY)
                    {
                        case "Price":
                            y = sortedAcc.Select(a => (double)a.Price).ToArray();
                            break;
                        case "Overall Satisfaction":
                            y = sortedAcc.Select(a => a.Overall_satisfaction).ToArray();
                            break;
                        default:
                            MessageBox.Show("Something went wrong. Pick Y axis");
                            break;
                    }
                }

            }
            else
            {
                //implementera count av sitt slag per room_type som kommit in i xs.

                List<double> yvalues = new List<double>();
                foreach (string x1 in xs)
                {
                    //räknar
                    yvalues.Add(accBigData.Where(a => a.Room_type == x1).Count());



                }
                y = yvalues.ToArray();

            }



            Series series = new Series(pickedCity + " " + pickedY);

            //Sätter datapunkter
            if (graphHasStringValuesOnXAxis)
            {
                series.Points.DataBindXY(xs, y);

            }
            else
            {
                series.Points.DataBindXY(x, y);

            }


            //Sätter namn på axlarna
            chart1.ChartAreas[0].AxisX.Title = pickedX;
            chart1.ChartAreas[0].AxisY.Title = pickedY;

            if (barIsPossible && pickedGraph == "Bar")
            {
                if (add == true && chart1.Series.IndexOf(series) == -1)
                {
                    //serien finns inte och man vill lägga till, lägger till
                    chart1.ResetAutoValues();
                    chart1.Series.Add(series);

                }
                else
                {

                    //ritar ny
                    chart1.Invalidate();
                    chart1.Series.Clear();
                    chart1.ResetAutoValues();
                    chart1.Series.Add(series);

                }
                //sätter alla series att de ska va scatterplots
                for (int i = 0; i < chart1.Series.Count(); i++)
                {
                    chart1.Series[i].ChartType = SeriesChartType.Column;

                }

            }
            else if (pickedGraph == "Scatterplot")
            {
                if (add == true && chart1.Series.IndexOf(series) == -1)
                {
                    //serien finns inte och man vill lägga till, lägger till
                    chart1.ResetAutoValues();
                    chart1.Series.Add(series);

                }
                else
                {

                    //ritar ny
                    chart1.Invalidate();
                    chart1.Series.Clear();
                    chart1.ResetAutoValues();
                    chart1.Series.Add(series);

                }
                //sätter alla series att de ska va scatterplots
                for (int i = 0; i < chart1.Series.Count(); i++)
                {
                    chart1.Series[i].ChartType = SeriesChartType.Point;

                }
            }

            label1.Text = "Number of records " + sortedAcc.Count().ToString();

        }








        public List<string> GetCleanListOfOccurencies(string[] xsall)
        {
            List<string> xs = new List<string>();
            //Loopar igenom och tar bort felaktig data
            for (int i = 0; i < xsall.Count(); i++)
            {
                if (!IsNumeric(xsall[i]) && xsall[i] != "")
                {
                    //verkar vara ett riktigt ord
                    //stoppar in
                    xs.Add(xsall[i]);

                }
            }
            return xs;
        }


        private void chart1_Click(object sender, EventArgs e)
        {

        }

        public static bool IsNumeric(string s)
        {
            float output;
            return float.TryParse(s, out output);
        }

        private void cbX_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbX.Text == "Room type")
            {
                cbY.SelectedIndex = cbY.FindStringExact("Units");
                cbY.Enabled = false;
            }
            else
            {
                cbY.Enabled = true;

            }
        }






        private void button2_Click(object sender, EventArgs e)
        {
            //Sätter zooom xmax o ymax
            double d = 100;
            bool gick = double.TryParse(textBox1.Text, out d);
            if (gick)
            {
                chart1.ChartAreas[0].AxisX.Maximum = d;
            }
            gick = double.TryParse(textBox2.Text, out d);
            if (gick)
            {
                chart1.ChartAreas[0].AxisY.Maximum = d;
            }

        }
    }


    class CbBoxItem
    {
        private string name;
        private string value;
        private string accObjName;
        public CbBoxItem(string n, string v)
        {
            this.Name = n;
            this.Value = n;

        }

        public CbBoxItem(string n, string v, string a)
        {
            this.Name = n;
            this.Value = v;
            this.AccObjName = a;

        }
        public override string ToString()
        {
            return this.Name;
        }


        public string Name { get => name; set => name = value; }
        public string Value { get => value; set => this.value = value; }
        public string AccObjName { get => accObjName; set => accObjName = value; }
    }


}
