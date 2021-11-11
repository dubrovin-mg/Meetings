using NUnit.Framework;
using System;
using static Meetings.Meet;

namespace Meetings.Tests
{
    public class MeetTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /* Тест на коррекность ввода планируемой даты встречи в формате "dd.MM.yyyy HH:mm"
        * при CheckFileName == true - значение даты корректно */
        [TestCase("14.12", false)]
        [TestCase("14.12.2025", false)]
        [TestCase("14.12.2025 14:00", true)]
        [TestCase("14.12.2000 14:00", false)]
        public void DateValueTest(string inputText, bool expected)
        {
            DateTime resultDate;
            bool actual = CheckCorrectDate(inputText, out resultDate);
            Assert.AreEqual(expected, actual);
        }

    }
}