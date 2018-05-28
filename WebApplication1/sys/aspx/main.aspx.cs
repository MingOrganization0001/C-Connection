using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Data;
using ClassStore;
using System.Reflection;//我的类库

namespace FirstTest
{
    public partial class main : System.Web.UI.Page
    {
        public List<Goods> list = new List<Goods>();
        protected void Page_Load(object sender, EventArgs e)
        {
            //sql语句
            string sqlstr = "select * from tb_goods";
            //实例化类库中的类
            ClassStore.SqlFunction ds = new ClassStore.SqlFunction();
            //使用方法 得到查询的dataset 并转化为DataTable
            DataTable tb = ds.GetDs(sqlstr).Tables[0];
            //临时变量
            string name = string.Empty;
            foreach (DataRow rw in tb.Rows)
            {
                //Goods 模型
                Goods g = new Goods();
                //获取公共属性
                PropertyInfo[] prop = g.GetType().GetProperties();
                //遍历属性tbname
                foreach (PropertyInfo p in prop)
                {
                    name = p.Name;
                    //判断属性是否可写   否则进行下一次循环
                    if (!p.CanWrite) continue;
                    object val = rw[name];
                    if (val != DBNull.Value)
                    {
                        p.SetValue(g, val, null);
                    }
                }
                list.Add(g);
            }

        }
        protected void button_click(object sender, EventArgs e)
        {

        }
    }
}