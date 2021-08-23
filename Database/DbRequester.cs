using BT.Database.DbModels;
using BT.Database.DbTools;
using BT.Database.Extensions;
using BT;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Database
{
    public static class DbRequester
    {
        public static SqliteConnection Connection;
        public static int ParameterIndex;

        public static void SetConnection()
        {
            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder();
            builder.DataSource = "./mp.db";
            Connection = new SqliteConnection(builder.ConnectionString);
        }

        public static DataTable SelectAll(string table)
        {
            string query = "SELECT * FROM " + table;
            return ExecuteSelectQuery(query);
        }

        public static DataTable SelectCountAll(string table)
        {
            string query = "SELECT COUNT(*) FROM " + table;
            return ExecuteSelectQuery(query);
        }

        public static DataTable Select(string from, List<Column> select = null, List<Join> joins = null, ConditionsCollection where = null, List<Conditions> where2 = null,
            List<Column> groupBy = null, List<Conditions> having = null, Column orderBy = null, int order = 0)
        {
            ParameterIndex = 0;
            string query = "SELECT ";
            string columns = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            //ajoute les colonnes à sélectionner, ainsi que leurs paramètres (aggregates, fonctions...). sélectionne toutes les colonnes par défaut
            if (select.IsNullOrEmpty())
                columns = "*";
            else
            {
                select.ReOrder();
                foreach (var key in select)
                    columns += key.GetSql() + ", ";
                columns = columns.Remove(columns.Length - 2);
            }

            query += columns + " FROM " + from;

            //ajoute des jointures si il y en a
            if (!joins.IsNullOrEmpty())
            {
                foreach (var join in joins)
                    query += join.GetSql();
            }

            List<Conditions> newHaving = new List<Conditions>();
            if (!where2.IsNullOrEmpty())
            {
                for (int i = 0; i < where2.Count; i++)
                {
                    if (where2[i].HasAggregates())
                    {
                        newHaving.Add(where2[i]);
                        where2[i] = null;
                    }
                }
                where2.RemoveAll(x => x == null);
            }

            //ajoute les conditions du where si il y en a
            if (where != null && !where.List.IsNullOrEmpty())
            {
                int notNull = 0;
                foreach (ConditionsCollection collection in where.List)
                {
                    if (!collection.Conditions.IsNullOrEmpty())
                        notNull++;
                }
                if (notNull == 1)
                    where.Operator = LogicalOperator.NONE;

                if (notNull > 0)
                {
                    query += " WHERE ";
                    int i = 0;
                    foreach (ConditionsCollection cc in where.List)
                    {
                        int j = 0;
                        if (!cc.Conditions.IsNullOrEmpty())
                        {
                            query += " (";
                            foreach (Conditions c in cc.Conditions)
                            {
                                query += c.GetSql();
                                if (c.Value != null)
                                {
                                    parameters.Add("@p" + ParameterIndex++, c.Value);
                                    if (c.Comparator == Comparator.BETWEEN && c.Value2 != null)
                                        parameters.Add("@p" + ParameterIndex++, c.Value2);
                                }
                                if (j++ < cc.Conditions.Count - 1 && cc.Operator != LogicalOperator.NONE)
                                    query += cc.Operator;
                            }
                            query += ") ";
                            if (i++ < where.List.Count - 1 && where.Operator != LogicalOperator.NONE)
                                query += where.Operator;
                        }
                    }
                }
            }
            else if (!where2.IsNullOrEmpty())
            {
                query += " WHERE ";
                int i = 0;
                foreach (Conditions condition in where2)
                {
                    query += condition.GetSql();
                    if (condition.Value != null)
                        parameters.Add("@p" + ParameterIndex++, condition.Value);
                    if (i++ < where2.Count - 1)
                        query += " AND ";
                }
            }

            //ajoute les arguments du group by si il y en a
            if (!groupBy.IsNullOrEmpty())
            {
                query += " GROUP BY ";
                foreach (var key in groupBy)
                    query += key.GetSql(false) + ", ";
                query = query.Remove(query.Length - 2);
            }

            if (!newHaving.IsNullOrEmpty())
            {
                if (having.IsNullOrEmpty())
                    having = newHaving;
                else
                {
                    having[0].LogicalOperator = LogicalOperator.AND;
                    foreach (Conditions c in having)
                        newHaving.Add(c);
                    having = newHaving;
                }
            }

            if (!having.IsNullOrEmpty())
            {
                query += " HAVING ";
                int j = 0;
                foreach (Conditions Hcondition in having)
                {
                    query += Hcondition.GetSql();
                    if (Hcondition.Value != null)
                        parameters.Add("@p" + ParameterIndex++, Hcondition.Value);
                    if (j++ < having.Count - 1)
                        query += " AND ";
                }
            }

            if (orderBy != null)
            {
                query += " ORDER BY " + orderBy.GetSql(false);
                if (order == 0)
                    query += " ASC";
                if (order == 1)
                    query += " DESC";
            }

            return ExecuteSelectQuery(query, parameters);
        }

        public static DataTable ExecuteSelectQuery(string query, Dictionary<string, object> parameters = null)
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            DataTable table = new DataTable();

            SqliteCommand command = Connection.CreateCommand();
            command.CommandText = query;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                table.Load(reader);
                Connection.Close();
            }
            catch (Exception e)
            {
                Program.PrintException(e);
            }
            return table;
        }

        #region writing

        /// <summary>
        /// permet d'ajouter des elements dans une table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="objects"></param>
        public static bool AddObjects(string table, SqlObject[] objects)
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            foreach (SqlObject obj in objects)
            {
                string query = "INSERT INTO " + table + "(";
                SqliteCommand command = new SqliteCommand(query, Connection);
                foreach (var column in obj.Fields.Keys)
                    command.CommandText += column + ", ";
                command.CommandText = command.CommandText.Remove(command.CommandText.Length - 2);
                command.CommandText += ") VALUES (";

                foreach (var key in obj.Fields.Keys)
                {
                    command.CommandText += "@" + key + ", ";
                    command.Parameters.AddWithValue("@" + key, obj.Fields[key]);
                }
                command.CommandText = command.CommandText.Remove(command.CommandText.Length - 2);
                command.CommandText += ")";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    if (Connection.State != ConnectionState.Closed)
                        Connection.Close();
                    Program.PrintException(e);
                    return false;
                }
            }
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();

            return true;
        }

        /// <summary>
        /// permet d'ajouter un élément dans une table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="obj"></param>
        public static bool AddObject(string table, SqlObject obj)
        {
            return AddObjects(table, new SqlObject[] { obj });
        }

        /// <summary>
        /// permet de modifier des éléments de la base de données
        /// </summary>
        /// <param name="table"></param>
        /// <param name="objects"></param>
        public static bool EditObjects(string table, SqlObject[] objects)
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            foreach (SqlObject obj in objects)
            {
                string query = "Update " + table + " set ";
                string idKey = obj.Fields.GetFirstKey().ToString();
                SqliteCommand command = new SqliteCommand(query, Connection);
                foreach (string key in obj.Fields.Keys)
                {
                    if (key != idKey)
                    {
                        command.CommandText += key + " = @" + key + ", ";
                        command.Parameters.AddWithValue("@" + key, obj.Fields[key]);
                    }
                }
                command.CommandText = command.CommandText.Remove(command.CommandText.Length - 2);
                command.CommandText += " where " + idKey + " = " + obj.Fields[idKey];

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("updated");
                }
                catch (Exception e)
                {
                    if (Connection.State != ConnectionState.Closed)

                        if (Connection.State != ConnectionState.Closed)
                            Connection.Close();
                    Program.PrintException(e);
                    return false;
                }
            }
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
            return true;
        }

        /// <summary>
        /// permet de modifier un élément de la base de données
        /// </summary>
        /// <param name="table"></param>
        /// <param name="obj"></param>
        public static bool EditObject(string table, SqlObject obj)
        {
            return EditObjects(table, new SqlObject[] { obj });
        }

        /// <summary>
        /// permet de supprimer des éléments de la base de données
        /// </summary>
        /// <param name="table"></param>
        /// <param name="objects"></param>
        public static bool DeleteObjects(string table, SqlObject[] objects)
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            foreach (SqlObject obj in objects)
            {
                try
                {
                    string idKey = obj.Fields.GetFirstKey().ToString();
                    SqliteCommand command = new SqliteCommand("DELETE FROM " + table + " WHERE " + idKey + " = @" + idKey, Connection);
                    int id = Convert.ToInt32(obj.Fields[idKey]);
                    command.Parameters.AddWithValue("@" + idKey, obj.Fields[idKey]);
                    string query = command.CommandText;
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    if (Connection.State != ConnectionState.Closed)
                        Connection.Close();
                    Program.PrintException(e);
                    return false;
                }
            }
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
            return true;
        }

        /// <summary>
        /// permet de supprimer un élément de la base de données
        /// </summary>
        /// <param name="table"></param>
        /// <param name="obj"></param>
        public static bool DeleteObject(string table, SqlObject obj)
        {
            return DeleteObjects(table, new SqlObject[] { obj });
        }

        #endregion
    }
}
