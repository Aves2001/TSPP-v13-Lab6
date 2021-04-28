using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Lab6.Task2;
using System.Linq;

namespace Завдання_3
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CitySearch()
        {
            TouringTrip trip = new TouringTrip();
            List<TouringTrip> list = new List<TouringTrip>();

            List<string> str = new List<string>
            {
               "1; Бутень; Мельник; Чернівці; 2020; 1",
               "2; Аммі; Моргун; Снятин; 2021; 4",
               "3; Бузок; Дяків; Чернівці; 1999; 3"
            };

            foreach (string s in str)
            {
                string[] x = s.Split(';').Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                list.Add(new TouringTrip(x[0], x[1], x[2], x[3], x[4], x[5]));
            }

            TouringTrip res = trip.CitySearch(list, "Снятин")[0];

            Assert.AreEqual(res, list[1]);
        }
    }
}
