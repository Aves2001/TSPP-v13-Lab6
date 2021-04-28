using System;
using System.Collections.Generic;

namespace Lab6.Task1
{
    internal interface IMeteorologicalObservations : IFormattable
    {
        /// <summary>
        /// Виводить масив у вигляді таблиці
        /// </summary>
        /// <param name="metObs">Масив з метеорологічними даними</param>
        void ViewArray(MeteorologicalObservations[] metObs);

        /// <summary>
        /// Повертає два дні з найбільшим перепадом тиску
        /// </summary>
        /// <param name="metObs">Масив з метеорологічними даними</param>
        /// <returns></returns>
        MeteorologicalObservations[] TheLargestPressureDrop(MeteorologicalObservations[] metObs);

        /// <summary>
        /// Порівняння дати
        /// </summary>
        public int CompareDate(MeteorologicalObservations a, MeteorologicalObservations b);
        /// <summary>
        /// Порівняння атмосферного тиску
        /// </summary>
        public int CompareAtmosphericPressure(MeteorologicalObservations a, MeteorologicalObservations b);

        /// <summary>
        /// Перевірка дати на правильний формат
        /// </summary>
        /// <param name="s">Дата</param>
        void TestIsDate(string s);

        /// <summary>
        /// Перевіряє чи рядок складається лише з чисел, якщо ні - створює виключення з текстом ErrorMessage
        /// </summary>
        /// <param name="str">Рядок який перевіряється</param>
        /// <param name="ErrorMessage">Текст яки потрібно передати у виключення</param>
        void TestIsNumber(string str, string ErrorMessage);

        /// <summary>
        /// Імпортує дані з файла
        /// </summary>
        /// <returns>Повертає колекцію метеорологічних даних</returns>
        List<MeteorologicalObservations> Input();
    }
}
