using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyPost.Tests.Net
{
    [TestClass]
    public class InsuranceTest
    {
        [TestInitialize]
        public void Initialize()
        {
            VCR.SetUp(VCRApiKey.Test, "insurance", true);
        }

        private static Insurance CreateBasicInsurance()
        {
            return Insurance.Create(Fixture.BasicInsurance);
        }

        [TestMethod]
        public void TestCreate()
        {
            VCR.Replay("create");

            Insurance insurance = CreateBasicInsurance();

            Assert.IsInstanceOfType(insurance, typeof(Insurance));
            Assert.IsTrue(insurance.id.StartsWith("ins_"));
            // TODO: amount really should be a number, not a string
            Assert.AreEqual("100.00000", insurance.amount);
        }

        [TestMethod]
        public void TestRetrieve()
        {
            VCR.Replay("retrieve");


            Insurance insurance = CreateBasicInsurance();

            Insurance retrievedInsurance = Insurance.Retrieve(insurance.id);
            Assert.IsInstanceOfType(retrievedInsurance, typeof(Insurance));
            Assert.AreEqual(insurance.id, retrievedInsurance.id);
        }

        [TestMethod]
        public void TestAll()
        {
            VCR.Replay("all");

            InsuranceCollection insuranceCollection = Insurance.All(new Dictionary<string, object>
            {
                {
                    "page_size", Fixture.PageSize
                }
            });

            List<Insurance> insurances = insuranceCollection.insurances;

            Assert.IsTrue(insurances.Count <= Fixture.PageSize);
            Assert.IsNotNull(insuranceCollection.has_more);
            foreach (var item in insurances)
            {
                Assert.IsInstanceOfType(item, typeof(Insurance));
            }
        }
    }
}
