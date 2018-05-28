using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassStore
{
    public class Goods
    {
        int _id;
        string _name;//名称
        string _unit;//单位
        //float price;//价格 
        //int store;//库存

        public int id {
            get { return _id; }
            set { _id = value; }
        }
        public string name {
            get { return _name; }
            set { _name = value; }
        }
        public string unit {
            get { return _unit; }
            set { _unit = value; }
        }
        public float price { get; set; }
        public int store { get; set; }
        public string place { get; set; }
    }
}
