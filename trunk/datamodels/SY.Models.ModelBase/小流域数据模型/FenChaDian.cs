using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SY.Models.ModelBase
{
    [DataContract]
    [Serializable]
    public class FenChaDian
    {
        private string _fenChaDianMingCheng;
        [DataMember]
        public string FenChaDianMingCheng
        {
            get { return _fenChaDianMingCheng; }
            set { _fenChaDianMingCheng = value; }
        }
        private long _fid;
        [DataMember]
        public long Fid
        {
            get { return _fid; }
            set { _fid = value; }
        }
        private string _fenChaDiancode;
        [DataMember]
        public string FenChaDiancode
        {
            get { return _fenChaDiancode; }
            set { _fenChaDiancode = value; }
        }
        private string _downstreamid;
        [DataMember]
        public string Downstreamid
        {
            get { return _downstreamid; }
            set { _downstreamid = value; }
        }
    }
}
