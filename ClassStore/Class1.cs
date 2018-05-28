using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Reflection;

namespace ClassStore
{
    public class SqlFunction
    {//连接数据库的操作
        public SqlFunction()
        {

        }
        public static string GetConn()
        {//连接数据库
            return ConfigurationManager.ConnectionStrings["mysqlsever"].ConnectionString;
        }
        public DataSet GetDs(string sql,params SqlParameter[] values)
        {//查询功能 select方法 查询DateSet
            SqlConnection conn = new SqlConnection(GetConn());//实例化数据库连接
            conn.Open();
            DataSet ds = new DataSet();//数据集
            SqlDataAdapter sda = new SqlDataAdapter();//适配器
            SqlCommand sc = new SqlCommand(sql, conn);
            try
            {
                sc.Parameters.AddRange(values);
                sda.SelectCommand = sc;
                sda.Fill(ds);
            }
            finally
            {
                conn.Close();
                sc.Parameters.Clear();
                conn.Dispose();//隐藏
            }
            return ds;
        }

        //删除指定数据
        public int DelById(string sql,params SqlParameter[] values)
        {
            SqlConnection conn = new SqlConnection(GetConn());
            conn.Open();
            SqlCommand sc = new SqlCommand(sql, conn);
            sc.Parameters.AddRange(values);
            int result = sc.ExecuteNonQuery();
            conn.Close();
            sc.Parameters.Clear();
            return result;
        }

        //新增
        public string InsMes(string sql,params SqlParameter[] values)
        {
            SqlConnection conn = new SqlConnection(GetConn());
            conn.Open();
            SqlCommand sc = new SqlCommand(sql, conn);
            sc.Parameters.AddRange(values);

            //返回新增数据的id
            string res = string.Empty;
            if (res != null) res = sc.ExecuteScalar().ToString();
            conn.Close();
            sc.Parameters.Clear();
            return res;
        }
        public string EdtMes(string procname, params SqlParameter[] values)
        {
            string res = string.Empty;
            SqlConnection conn = new SqlConnection(GetConn());
            conn.Open();
            SqlCommand sc = new SqlCommand(procname, conn);
            try
            {
                sc.CommandType = CommandType.StoredProcedure;
                sc.Parameters.AddRange(values);
                if (res != null) res = sc.ExecuteNonQuery().ToString();

            }
            catch {
                res = "error";
            }
            
            conn.Close();
            sc.Parameters.Clear();
            return res;
        }

    }
}
