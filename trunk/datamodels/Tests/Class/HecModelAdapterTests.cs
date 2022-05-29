using Microsoft.VisualStudio.TestTools.UnitTesting;
using SY.HECModelAdapter;
using SY.Models.ModelBase;
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
            HecModelAdapter adapter = new HecModelAdapter("../../TestData/mzh");
            Console.WriteLine( adapter.BoundaryList.Count ) ;


            //单元测试1
            //adapter.BoundaryList[1].Value[1].Data = 999.0f;
            //adapter.SetBoundary(adapter.BoundaryList);


            //单元测试2
            //adapter.BoundaryList.RemoveAt(1);
            //adapter.SetBoundary(adapter.BoundaryList);

            //单元测试3
            Boundary boundary1 = adapter.BoundaryList[1];
            adapter.BoundaryList.RemoveAt(1);
            boundary1.Value[0].Data = 777;
            boundary1.Value[1].Data = 888;
            boundary1.Value[2].Data = 999;
            adapter.BoundaryList.Add(boundary1);
            adapter.SetBoundary(adapter.BoundaryList);



        }
    }
}