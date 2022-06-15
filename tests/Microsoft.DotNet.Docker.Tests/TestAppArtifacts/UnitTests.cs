using System.Globalization;
using System.Threading;
using Xunit;

namespace app
{
    /// <summary>
    /// This class contains unit tests that will be executed from within the sdk image.
    /// </summary>
    public class UnitTests
    {
        /// <summary>
        /// This ensures that localization is properly configured in the sdk image.
        /// </summary>
        /// <remarks>
        /// See https://github.com/dotnet/dotnet-docker/issues/3844
        /// </remarks>
        [Fact]
        public void CurrencyLocalization()
        {
            CultureInfo savedCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-de");
                string text = string.Format("{0:C}", 100);
                Assert.Equal("100,00 €", text);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = savedCulture;
            }
        }
    }
}
