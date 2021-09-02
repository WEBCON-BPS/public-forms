using NUnit.Framework;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Service;

namespace WEBCON.FormsGenerator.Test.BusinessLogic
{
    [TestFixture]
    public class DataEncodingTest
    {
        [Test]
        public void EncodeTest()
        {
            DataEncodingService dataEncoding = new DataEncodingService()
            {
                AppKey = "t6w9z$C&F)J@McQf"
            };
            var result = dataEncoding.Encode("haslotestowe");
            Assert.AreEqual("qk5xf7C0R6POtKmEHkLf+TznelVkxYFMeWl4p25Ji8U=", result);
        }
        [Test]
        public void DecodeTest()
        {
            DataEncodingService dataEncoding = new DataEncodingService()
            {
                AppKey = "t6w9z$C&F)J@McQf"
            };
            var result = dataEncoding.Decode("qk5xf7C0R6POtKmEHkLf+TznelVkxYFMeWl4p25Ji8U=");
            Assert.AreEqual("haslotestowe", result);
        }
    }
}
