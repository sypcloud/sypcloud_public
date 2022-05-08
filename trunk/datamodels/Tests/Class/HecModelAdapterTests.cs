using Microsoft.VisualStudio.TestTools.UnitTesting;
using SY.HECModelAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.HECModelAdapter.Tests
{
    [TestClass()]
    public class HecModelAdapterTests
    {
        [TestMethod()]
        public void RunModelTest()
        {
            HecModelAdapter hma = null;
            try
            {
                hma = new HecModelAdapter(@"E:\Projects\songmingming-water2022\1river-MCRS(1)(1)\1river-MCRS");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            Assert.Fail();
        }
    }
}