using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tutorial.ProfanityFilter.Svc.Test
{
    /// <summary>
    /// This class contains all test cases for the Tutorial.ProfanityFilter.Svc.ProfanityFilterService
    /// </summary>
    [TestClass]
    public class ProfanityFilterServiceTest
    {
        /// <summary>
        /// Tests the filter with profanity.
        /// </summary>
        [TestMethod]
        public void Test_Filter_With_Profanity()
        {
            // Arrange
            var input = "What the fuck is this?";
            var expected = "What the **** is this?";

            // Act
            var resultTask = ProfanityFilterService.Filter(input);
            resultTask.Wait();

            var result = resultTask.Result;

            // Assert
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests the filter without profanity.
        /// </summary>
        [TestMethod]
        public void Test_Filter_Without_Profanity()
        {
            // Arrange
            var input = "This is awesome.";
            var expected = input;

            // Act
            var resultTask = ProfanityFilterService.Filter(input);
            resultTask.Wait();

            var result = resultTask.Result;

            // Assert
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests the filter with empty input.
        /// </summary>
        [TestMethod]
        public void Test_Filter_With_Empty_Input()
        {
            // Arrange
            var input = " ";
            var expected = input;

            // Act
            var resultTask = ProfanityFilterService.Filter(input);
            resultTask.Wait();

            var result = resultTask.Result;

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
