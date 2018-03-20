using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirBnbProject
{
    class SQLClass
    {
        Form1 f1;
        SqlConnection myConnection = new SqlConnection("user id=mhovmark;" +
                                       "password=gurragnu;server=DESKTOP-CJC5LHJ\\SQL2017;" +
                                       "Trusted_Connection=yes;" +
                                       "database=myDW; " +
                                       "connection timeout=30");
        private List<Accommodation> allaAccommodations = new List<Accommodation>();

        public SQLClass()
        {
            try
            {
                myConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public List<Accommodation> GetListAccomodation(Form1 f)
        {
            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("select * from [myDW].[dbo].[airbnb] where active=1", myConnection);
                myReader = myCommand.ExecuteReader();
                f.progressBar1.Maximum = 3000;
                f.progressBar1.Step = 1;

                while (myReader.Read())
                {
                    f.progressBar1.PerformStep();
                    Accommodation a = new Accommodation();
                    a.Room_id = Int32.TryParse(myReader["room_id"].ToString(), out int r_id) ? r_id : 0;
                    a.CityName = myReader["City"].ToString();
                    a.Price = Int32.TryParse(myReader["price"].ToString(), out int p) ? p : 0;
                    a.Overall_satisfaction = Double.TryParse(myReader["overall_satisfaction"].ToString(), out Double o_s) ? o_s : 0;
                    a.Room_type = myReader["room_type"].ToString();

                    /*
                    o.Name = myReader["Seller"].ToString();
                    o.Value = float.Parse(myReader["extendeprice"].ToString());*/
                    allaAccommodations.Add(a);
                    Console.Write(a.Price.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
            return allaAccommodations;

        }

    }


}
