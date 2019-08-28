using System;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;

namespace QZWebService.ServiceInterface.SqlHelper
{
    public abstract class SqlHelper
    {
        public static readonly string ConnectionStringMSSQL = ConfigurationManager.ConnectionStrings["QZQrCodeConnection"].ConnectionString.ToString();
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        public static DataTable SearchDataTable(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStringMSSQL))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds.Tables[0];
            }
        }

        public static int ExecuteNonQuery(string sql)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionStringMSSQL))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                int val = 0;
                bool iserror = false;
                string strerror = "";
                SqlTransaction SqlTran = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandTimeout = 60 * 5;
                    cmd.CommandText = sql;
                    cmd.Transaction = SqlTran;
                    cmd.CommandType = CommandType.Text;
                    val = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    val = 0;
                    iserror = true;
                    strerror = ex.Message.ToString() + ex.StackTrace.ToString();
                }
                finally
                {

                    if (iserror)
                    {
                        SqlTran.Rollback();
                    }
                    else
                    {
                        SqlTran.Commit();
                    }
                }
                return val;
            }
        }

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(ConnectionStringMSSQL))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static bool ExecuteNonQuery(List<string> sqlList)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionStringMSSQL))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                bool iserror = false;
                string strerror = "";
                SqlTransaction SqlTran = conn.BeginTransaction();
                try
                {
                    for (int i = 0; i < sqlList.Count; i++)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 60 * 5;
                        cmd.CommandText = sqlList[i].ToString();
                        cmd.Transaction = SqlTran;
                        cmd.CommandType = CommandType.Text;
                        int val = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    iserror = true;
                    strerror = ex.Message;
                }
                finally
                {

                    if (iserror)
                    {
                        SqlTran.Rollback();
                        throw new Exception(strerror);
                    }
                    else
                    {
                        SqlTran.Commit();
                    }
                }
                if (iserror)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static bool ExecuteNonQuery(List<string> sqlList,List<SqlParameter[]> commandParameters)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionStringMSSQL))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                bool iserror = false;
                string strerror = "";
                SqlTransaction SqlTran = conn.BeginTransaction();
                try
                {
                    for (int i = 0; i < sqlList.Count; i++)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 60 * 5;
                        cmd.CommandText = sqlList[i].ToString();
                        cmd.Transaction = SqlTran;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(commandParameters[i]);
                        //var cmdParms = commandParameters[i];
                        //if (cmdParms != null)
                        //{
                        //    foreach (SqlParameter parm in cmdParms)
                        //        cmd.Parameters.Add(parm);
                        //}
                        int val = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    iserror = true;
                    strerror = ex.Message;
                }
                finally
                {

                    if (iserror)
                    {
                        SqlTran.Rollback();
                        throw new Exception(strerror);
                    }
                    else
                    {
                        SqlTran.Commit();
                    }
                }
                if (iserror)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static bool ExecuteNonQuery(ArrayList sqlList)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionStringMSSQL))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                bool iserror = false;
                string strerror = "";
                SqlTransaction SqlTran = conn.BeginTransaction();
                try
                {
                    for (int i = 0; i < sqlList.Count; i++)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 60 * 5;
                        cmd.CommandText = sqlList[i].ToString();
                        cmd.Transaction = SqlTran;
                        cmd.CommandType = CommandType.Text;
                        int val = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    iserror = true;
                    strerror = ex.Message;
                }
                finally
                {

                    if (iserror)
                    {
                        SqlTran.Rollback();
                        throw new Exception(strerror);
                    }
                    else
                    {
                        SqlTran.Commit();
                    }
                }
                if (iserror)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                else if (connection.State == ConnectionState.Broken)
                {
                    try
                    {
                        connection.Close();
                    }
                    catch
                    {
                        //return -1;
                    }
                    connection.Open();
                }
            }
            catch
            {
            }

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConnectionStringMSSQL);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet ExecuteDataSet(string cmdText)
        {
            DataSet __ds = new DataSet();
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(cmdText, ConnectionStringMSSQL);
                sda.Fill(__ds);
            }
            catch
            {
                throw;
            }
            return __ds;
        }
        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            DataSet __ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConnectionStringMSSQL);
            SqlDataAdapter sda = new SqlDataAdapter();
            using (SqlConnection connection = new SqlConnection(ConnectionStringMSSQL))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                sda.SelectCommand = cmd;
                sda.Fill(__ds);
                cmd.Parameters.Clear();
                return __ds;
            }
                
        }
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(ConnectionStringMSSQL))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static int UpdateByteField(SqlConnection connection, string p_TableName, string p_FieldName, string whereStr, byte[] p_bDatas)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
            }
            catch
            {
            }
            if (p_bDatas == null || p_bDatas.Length == 0)
            {
                return ExecuteNonQuery(connection, CommandType.Text, string.Format("update {0} set {1}=NULL where {2}", p_TableName, p_FieldName, whereStr), null);
            }
            string strSql = string.Format("update {0} set {1}=@image where {2}", p_TableName, p_FieldName, whereStr);
            SqlCommand myCommand = new SqlCommand(strSql, connection);
            myCommand.CommandTimeout = 60 * 5;
            myCommand.Parameters.Add(new SqlParameter("@image", SqlDbType.Image, int.MaxValue));
            myCommand.Parameters["@image"].Value = p_bDatas;
            int val = myCommand.ExecuteNonQuery();
            return val;
        }

        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];
            if (cachedParms == null)
                return null;
            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();
            return clonedParms;
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}