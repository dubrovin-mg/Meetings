using NUnit.Framework;
using static Meetings.MainMenu;

namespace Meetings.Tests
{
    public class MainMenuTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /* ���� �� ������������ ����� ������ ����, 
        * ��� CheckCorrectMenuItem == true - ����� ���� ���������� */
        [TestCase("2", true)]
        [TestCase("f", false)]
        [TestCase("12", false)]
        public void MenuItemsTest(string inputText, bool expected)
        {
            int itemIndex;
            bool actual = CheckCorrectMenuItem(inputText, out itemIndex);
            Assert.AreEqual(expected, actual);
        }

    }
}