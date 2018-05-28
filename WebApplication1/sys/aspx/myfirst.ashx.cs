using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;
using ClassStore;
using System.Reflection;
using System.Data.SqlClient;
using System.Dynamic;

namespace FirstTest
{
    /// <summary>
    /// myfirst 的摘要说明
    /// </summary>
    public class myfirst : IHttpHandler
    {
        public List<Goods> list = new List<Goods>();
        ClassStore.SqlFunction cg = new ClassStore.SqlFunction();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            /*  操作 search:查询
                操作 ins:新增
                操作 del:删除
             */
            string opt = context.Request["opt"];
            /*  查找方法 list:全部列表查询 name:通过名称模糊查询
                删除方法 id:通过数据id删除
             */
            string type = context.Request["type"];
            /*判断操作*/
            switch (opt)
            {
                case "search":
                    {//查询
                        loadme(context, type);
                        break;
                    }
                case "ins":
                case "edt":
                    //新增
                    //编辑
                    {
                        inslist(context, opt);
                        break;
                    }
                case "del":
                    {//删除
                        DelById(context);
                        break;
                    }
            }

        }

        //新增
        private void inslist(HttpContext context, string type)
        {
            string sql = string.Empty,
                res = string.Empty;
            SqlParameter[] param = 
            {
                new SqlParameter("@name", SqlDbType.VarChar, 30),
                new SqlParameter("@price",SqlDbType.Float),
                new SqlParameter("@unit", SqlDbType.VarChar, 30),
                new SqlParameter("@place", SqlDbType.VarChar, 30),
                new SqlParameter("@store", SqlDbType.Int),
                new SqlParameter("@id", SqlDbType.Int)
            };
            param[0].Value = context.Request["name"];
            param[1].Value = float.Parse(context.Request["price"]);
            param[2].Value = context.Request["unit"];
            param[3].Value = context.Request["place"];
            param[4].Value = int.Parse(context.Request["store"]);
            param[5].Value = int.Parse(context.Request["id"]);

            if (type == "ins")
            {
                sql = "insert into tb_goods(name,price,place,store,unit) values(@name,@price,@place,@store,@unit);select @@identity";
                res = cg.InsMes(sql, param);
            }
            else
            {
                res = cg.EdtMes("edt_goods", param);
            }

            context.Response.Write(res);
        }

        //删除指定数据
        public void DelById(HttpContext context)
        {
            string sql = "delete from tb_goods where id=@id";
            SqlParameter[] param = 
            {
                new SqlParameter("@id", SqlDbType.Int),
            };
            param[0].Value = int.Parse(context.Request["id"]);

            int res = cg.DelById(sql, param);
            context.Response.Write(res);
        }


        /*默认读数据列表*/
        public void loadme(HttpContext context, string type)
        {
            string sql = string.Empty;
            switch (type)
            {
                case "list"://全部数据
                    {
                        sql = "select * from tb_goods ";
                        break;
                    }
                case "key1"://模糊查询
                    {
                        sql = "select * from tb_goods where name like @keyword or place like @keyword";
                        break;
                    }
                case "info":// ByID查询 单个数据
                    {
                        sql = "select * from tb_goods where id=@id";
                        break;
                    }
            }
            string callback = context.Request["callback"];

            //多条件查询
            SqlParameter[] param = 
            {
                new SqlParameter("@keyword", SqlDbType.VarChar, 30),
                new SqlParameter("@id", SqlDbType.Int),
            };
            param[0].Value = '%' + context.Request["keyword"] + '%';
            param[1].Value = context.Request["id"];

            DataTable tb = cg.GetDs(sql, param).Tables[0];//得到datatable
            string jsonString = DtToJson(tb);
            context.Response.Write(callback + "(" + jsonString + ")");
        }


        //转化数据格式
        private string DtToJson(DataTable tb)
        {
            //字典泛型
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in tb.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in tb.Columns)
                {
                    dic[dc.ColumnName] = dr[dc];
                }
                list.Add(dic);
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(list);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}