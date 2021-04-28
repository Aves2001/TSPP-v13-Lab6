using System.Collections.Generic;

namespace Lab6.Task2
{
    /// <summary>
    /// Інтерфейс з методами:
    /// <seealso cref="CitySearch"/>
    /// та <seealso cref="MaximumNumberConcerts"/>
    /// </summary>
    internal interface ITouringTrip
    {
        /// <summary>
        /// Список гастрольних поїздок у певне місто
        /// </summary>
        /// <param name="list">Колекція гастрольних поїздок</param>
        /// <param name="str">Місто яке потрібно знайти</param>
        /// <returns>Повертає колекцію знайдених гастрольних поїздок у певне місто</returns>
        List<TouringTrip> CitySearch(List<TouringTrip> list = null, string str = null);
        /// <summary>
        /// Гастрольні поїздки з максимальною кількістю концертів
        /// </summary>
        void MaximumNumberConcerts();
    }
}
