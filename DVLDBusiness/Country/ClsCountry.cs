using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDdataAccess;
using static DVLDLogic.ClsPerson;

namespace DVLDbusiness
{
    public class ClsCountry
    {
        public int ID { set; get; }
        public string Name { set; get; }

        private ClsCountry(int ID, string CountryName)

        {
            this.ID = ID;
            this.Name = CountryName;


        }
        public static ClsCountry Find(int ID)
        {

            string Name = "";


            if (ClsCountryAccess.GetCountryInfoByID(ID, ref Name))

                return new ClsCountry(ID, Name);
            else
                return null;

        }

        public static ClsCountry Find(string Name)
        {

            int ID = -1;


            if (ClsCountryAccess.GetCountryInfoByName(ref ID, Name))

                return new ClsCountry(ID, Name);
            else
                return null;

        }

    }
}
