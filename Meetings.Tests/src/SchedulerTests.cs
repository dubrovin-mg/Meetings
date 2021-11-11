using NUnit.Framework;
using static Meetings.Scheduler;

namespace Meetings.Tests
{
    public class SchedulerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /* Тест на коррекность ввода имени файла для экспорта 
        * при CheckFileName == true - имя файла корректно */
        [TestCase("", false)]
        [TestCase("File<>Name", false)]
        [TestCase("FileName", true)]
        public void FileNameTest(string inputText, bool expected)
        {
            string fileName;
            bool actual = CheckFileName(inputText,out fileName);
            Assert.AreEqual(expected, actual);
        }

    }
}