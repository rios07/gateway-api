using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Track3_Api_Interfaces_Core.Aplication.DATA;


namespace Infraestruture.DAO
{
    public interface IOracleDao
    {

        public abstract void CrearConnection(string strConnection);
        public abstract void CerrarConexion();
        public abstract DataTable ExcecuteAdapterFill(string query, List<OracleParameter> parameterCollection);
        public abstract int ExcecuteEscalar(string query);
        public abstract string GetStringParam(string query);
        public abstract string AddOrUpdate(string query, List<OracleParameter> parameterCollection);
        public abstract DataTable ExceuteStoreProcedure(string procedure, List<OracleParameter> parameterCollection);
        public void Dispose();
        public abstract void RollBack();
        public abstract void Commit();

    }
    //public interface ISqlDao
    //{
    //    public abstract void CrearConnection(string strConnection);
    //    public abstract void CerrarConexion();
    //    public abstract DataTable ExcecuteAdapterFill(string query, List<SqlParameter> parameterCollection);
    //    public abstract object ExcecuteEscalar(string query);
    //    public abstract Task<string> AddOrUpdate(string query, List<SqlParameter> parameterCollection);
    //    public abstract DataTable ExceuteStoreProcedure(string procedure, List<SqlParameter> parameterCollection);
    //    public abstract void RollBack();
    //    public abstract void IniciarTransaccion();
    //    public abstract void Commit();

    //}
    public class OracleDao : IOracleDao, IDisposable
    {
        public OracleConnection connection { get; set; }
        public OracleCommand oracleCommand { get; set; }

        public OracleDao()
        {
            
        }
        public OracleDao(string ConnectionString)
        {
            connection = new OracleConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            this.oracleCommand = new OracleCommand();
            this.oracleCommand.Connection = connection;
        }
        public virtual void CerrarConexion()
        {
            if (this.connection != null)
            {
                this.connection.Close();
                OracleConnection.ClearPool(this.connection);
            }
        }
        public virtual DataTable ExcecuteAdapterFill(string query, List<OracleParameter> parameterCollection)
        {
            DataTable dt = new DataTable();

          
            this.oracleCommand.CommandText = query;
            this.oracleCommand.CommandType = CommandType.Text;
            this.oracleCommand.Parameters.Clear();
            if (parameterCollection != null)
            {
                foreach (OracleParameter item in parameterCollection.ToList())
                {
                    oracleCommand.Parameters.Add(item);
                }
            }
            using OracleDataAdapter adapter = new OracleDataAdapter(this.oracleCommand);
            adapter.Fill(dt);

            return dt;
        }
        public virtual string AddOrUpdate(string query, List<OracleParameter> parameterCollection)
        {
            try
            {
                oracleCommand.CommandType = CommandType.Text;
                oracleCommand.CommandText = query;
                oracleCommand.Parameters.Clear();
                if (parameterCollection != null)
                {
                    foreach (var item in parameterCollection)
                    {
                        oracleCommand.Parameters.Add(item);
                    }
                }
                oracleCommand.ExecuteNonQuery();
                oracleCommand.Parameters.Clear();
                return BASICMESSAGES.OK;

            }
            catch (Exception ex)
            {
                string innerException = (ex.InnerException != null) ? ex.InnerException.Message : "";
                return $"error- {query} - {ex.Message} - {innerException}";
            }
        }

        public virtual DataTable ExceuteStoreProcedure(string procedure, List<OracleParameter> parameterCollection)
        {

            DataTable dt = new DataTable();
            oracleCommand.CommandText = procedure;
            oracleCommand.CommandType = CommandType.StoredProcedure;
            oracleCommand.Parameters.Clear();
            if (parameterCollection != null)
            {
                foreach (var item in parameterCollection)
                {
                    oracleCommand.Parameters.Add(item);
                }
            }
            using OracleDataAdapter adapter = new OracleDataAdapter(this.oracleCommand);
            adapter.Fill(dt);

            return dt;

        }
        public void Dispose()
        {
            this.CerrarConexion();

        }
        public virtual void EmpezarTransaccion(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            this.oracleCommand.Transaction = connection.BeginTransaction(level);
        }
        public void CrearConnection(string strConnection)
        {
            connection = new OracleConnection();
            connection.ConnectionString = strConnection;
            connection.Open();

            this.oracleCommand = new OracleCommand();
            this.oracleCommand.Connection = connection;
            this.oracleCommand.Transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public int ExcecuteEscalar(string query)
        {
            this.oracleCommand.CommandType = CommandType.Text;
            this.oracleCommand.CommandText = query;


            int sec = Convert.ToInt32(this.oracleCommand.ExecuteScalar());
            return sec;
        }
        public int ExcecuteEscalar(string query, List<OracleParameter> parameterCollection)
        {
            this.oracleCommand.CommandType = CommandType.Text;
            this.oracleCommand.CommandText = query;
            this.oracleCommand.Parameters.Clear();

            if (parameterCollection != null)
            {
                foreach (var item in parameterCollection)
                {
                    oracleCommand.Parameters.Add(item);
                }
            }

            int sec = Convert.ToInt32(this.oracleCommand.ExecuteScalar());
            return sec;
        }
        public string GetStringParam(string query)
        {
            this.oracleCommand.CommandType = CommandType.Text;
            this.oracleCommand.CommandText = query;


            string valor = this.oracleCommand.ExecuteScalar().ToString();

            return valor;
        }

        public void Commit()
        {
            if (this.oracleCommand.Transaction != null)
                this.oracleCommand.Transaction.CommitAsync();
        }
        public void RollBack()
        {
            if (this.oracleCommand.Transaction != null)
                this.oracleCommand.Transaction.RollbackAsync();
        }

        public OracleCommand getCommand()
        {
            return this.oracleCommand;
        }
        ~OracleDao()
        {
        }
    }
    //public class SqlDao : ISqlDao, IDisposable
    //{
    //    public SqlConnection connection { get; set; }
    //    public SqlCommand SqlCommand { get; set; }

    //    public SqlDao() { }
    //    public SqlDao(string ConnectionString)
    //    {
    //        connection = new SqlConnection();
    //        connection.ConnectionString = ConnectionString;
    //        connection.Open();

    //        this.SqlCommand = new SqlCommand();
    //        this.SqlCommand.Connection = connection;
    //    }

    //    public virtual void EmpezarTransaccion(IsolationLevel level = IsolationLevel.ReadCommitted)
    //    {
    //        this.SqlCommand.Transaction = connection.BeginTransaction(level);
    //    }
    //    public void CerrarConexion()
    //    {
    //        if (this.connection != null)
    //        {
    //            this.connection.Close();
    //            SqlConnection.ClearPool(this.connection);
    //        }
    //    }
    //    public DataTable ExcecuteAdapterFill(string query, List<SqlParameter> parameterCollection)
    //    {
    //        DataTable dt = new DataTable();

    //        this.SqlCommand.CommandType = CommandType.Text;
    //        this.SqlCommand.CommandText = query;
    //        SqlCommand.Parameters.Clear();
    //        foreach (SqlParameter item in parameterCollection)
    //        {
    //            SqlCommand.Parameters.Add(item);
    //        }

    //        using SqlDataAdapter adapter = new SqlDataAdapter(this.SqlCommand);
    //        adapter.Fill(dt);

    //        return dt;
    //    }



    //    public async Task<string> AddOrUpdate(string query, List<SqlParameter> parameterCollection)
    //    {

    //        SqlTransaction tran = null;
    //        SqlConnection conn = new SqlConnection();
    //        try
    //        {
    //            SqlCommand.CommandType = CommandType.Text;
    //            SqlCommand.CommandText = query;
    //            SqlCommand.Parameters.Clear();
    //            foreach (SqlParameter item in parameterCollection)
    //            {
    //                SqlCommand.Parameters.Add(item);
    //            }
    //            SqlCommand.ExecuteNonQuery();
    //            return BASICMESSAGES.OK;
    //        }
    //        catch (Exception ex)
    //        {
    //            string innerException = (ex.InnerException != null) ? ex.InnerException.Message : "";
    //            return $"error - {ex.Message} - {innerException}";
    //        }
    //    }

    //    public DataTable ExceuteStoreProcedure(string procedure, List<SqlParameter> parameterCollection)
    //    {
    //        DataTable dt = new DataTable();

    //        SqlCommand.CommandText = procedure;
    //        SqlCommand.CommandType = CommandType.StoredProcedure;
    //        SqlCommand.Parameters.Clear();

    //        foreach (SqlParameter item in parameterCollection)
    //        {
    //            SqlCommand.Parameters.Add(item);
    //        }

    //        dt.Load(SqlCommand.ExecuteReader());

    //        return dt;
    //    }

    //    public void RollBack()
    //    {
    //        this.SqlCommand.Transaction.Rollback();
    //        this.Dispose();
    //    }

    //    public void Commit()
    //    {
    //        this.SqlCommand.Transaction.Commit();
    //    }

    //    public void Dispose()
    //    {
    //        this.CerrarConexion();
    //    }

    //    public void CrearConnection(string strConnection)
    //    {
    //        connection = new SqlConnection();
    //        connection.ConnectionString = strConnection;
    //        connection.Open();
           
    //        this.SqlCommand = new SqlCommand();
    //        this.SqlCommand.Connection = connection;
    //    }

    //    public void IniciarTransaccion()
    //    {
    //        this.SqlCommand.Transaction = this.SqlCommand.Connection.BeginTransaction();
    //    }
    //    public object ExcecuteEscalar(string query)
    //    {
    //        this.SqlCommand.CommandType = CommandType.Text;
    //        this.SqlCommand.CommandText = query;
    //        return this.SqlCommand.ExecuteScalar();
    //    }

    //    ~SqlDao()
    //    {
    //    }
    //}
}
