﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MaintenanceWebUtilityWebForm2
{
    public partial class CreateMaintenance : System.Web.UI.Page
    {
        private int controlId = 0;
        private ArrayList controlIDArrayList = new ArrayList();

        protected void Page_Init(object sender, EventArgs e)
        {
            List<string> sqlDataTypes = GetSqlDataTypes();
            foreach(string str in sqlDataTypes)
            {
                DataType_Row_PK.Items.Add(new ListItem(str, str));
            }
            DataType_Row_PK.SelectedValue = "int";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                RecreateTableRows();
            }
        }

        public void DataType_Row_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            string ID = ddl.ID.Substring(ddl.ID.Length - 2);
            TextBox tb = (TextBox)PlaceHolder1.FindControl("DataTypeNum_Row_" + ID);
            CheckDataType(ddl,tb);
        }
        public void AddRowBtn_OnClick(object sender, EventArgs e)
        {
            InsertTableRow();
        }
        public void CreateBtn_OnClick(object sender, EventArgs e)
        {
           CreateSqlMaintenance();
        }

        private List<string> GetSqlDataTypes()
        {
            return new List<string>()
            {
                "bigint",
                "binary",
                "bit",
                "char",
                "date",
                "datetime",
                "datetime2",
                "datetimeoffset",
                "decimal",
                "float",
                "image",
                "int",
                "money",
                "nchar",
                "ntext",
                "numeric",
                "nvarchar",
                "nvarchar",
                "real",
                "rowversion",
                "smalldatetime",
                "smallint",
                "smallmoney",
                "sql_variant",
                "text",
                "time",
                "timestamp",
                "tinyint",
                "uniqueidentifier",
                "varbinary",
                "varbinary",
                "varchar",
                "xml"
            };
        }
        public DropDownList AddSqlDataTypesToDropDownList(DropDownList ddl)
        {
            List<string> sqlDataTypes = GetSqlDataTypes();
            foreach(string str in sqlDataTypes)
            {
                ddl.Items.Add(new ListItem(str, str));
            }

            return ddl;
        }
        private void CheckDataType(DropDownList ddl, TextBox tb)
        {
            /* Checks if DataType dropdownlist datatype includes extra parameters enclosed in ()
             * If true, enable DataTypeNum textbox,
             * else, enable = false
             */
            //values wherein () is required 
            if (ddl.SelectedValue.Contains("binary") || //50, 10, 7, 7, (18,0), 10, (18,0), 50, 7, 50 ,50
                ddl.SelectedValue.Contains("char") ||
                ddl.SelectedValue.Contains("datetime2") ||
                ddl.SelectedValue.Contains("datetimeoffset") ||
                ddl.SelectedValue.Contains("char") ||
                ddl.SelectedValue.Contains("decimal") ||
                ddl.SelectedValue.Contains("numeric") ||
                ddl.SelectedValue.Contains("varbinary") ||
                ddl.SelectedValue.Contains("time") &&
                ddl.SelectedValue != "datetime"
                )
            {
                tb.Enabled = true;
                //set default () for each datatype
                string datatypeNum = GetDefaultParameterValueSqlDataType(ddl.SelectedValue);
                tb.Attributes.Add("placeholder", datatypeNum);
            }
            else
            {
                tb.Text = "";
                tb.Enabled = false;
            }
        }
        private string GetDefaultParameterValueSqlDataType(string datatype)
        {
            if (datatype.Contains("binary")) { return "(50)"; }
            else if (datatype.Contains("char")) { return "(10)"; }
            else if (datatype.Contains("datetime2")) { return "(7)"; }
            else if (datatype.Contains("datetimeoffset")) { return "(7)"; }
            else if (datatype.Contains("decimal")) { return "(18, 0)"; }
            else if (datatype.Contains("numeric")) { return "(18,0)"; }
            else if (datatype.Contains("time")) { return "(7)"; }
            else if (datatype.Contains("varbinary")) { return "(50)"; }
            else return "";
        }

        private void RecreateTableRows()
        {
            string literal;
            TextBox tb;
            DropDownList ddl;
            CheckBox cb;
            ArrayList existingControlIDArrayList = new ArrayList();
            bool controlsArrayExists = false;
            List<string> SqlDataTypes = GetSqlDataTypes();
            string controlIdStr = "";

            //run if there are dynamically added rows
            //if ViewState.Keys contains controlIDArrayList
            foreach (string str in ViewState.Keys)
            {
                if (str == "controlIDArrayList")
                {
                    controlsArrayExists = true;
                    break;
                }
            }

            if (controlsArrayExists)
            {
                existingControlIDArrayList = ViewState["controlIDArrayList"] as ArrayList;
                //read arraylist
                //dynamically recreate rows
                foreach (string str in existingControlIDArrayList)
                {
                    if (str.StartsWith("Name_Row_"))
                    {
                        controlId = Convert.ToInt16(str.Substring(str.Length - 2));
                        if (controlId <= 9)
                        {
                            controlIdStr = "0" + controlId;
                        }
                        else
                        {
                            controlIdStr = controlId.ToString();
                        }
                        literal = "<tr><td></td><td>";
                        PlaceHolder1.Controls.Add(new LiteralControl(literal));

                        tb = new TextBox() { ID = "Name_Row_" + controlIdStr };
                        tb.CssClass = "form-control";
                        PlaceHolder1.Controls.Add(tb);
                        controlIDArrayList.Add(tb.ID.ToString());

                        literal = "</td><td><div class='container'><div class='row'>";
                        PlaceHolder1.Controls.Add(new LiteralControl(literal));
                    }
                    else if (str.StartsWith("DataType_Row_"))
                    {
                        controlId = Convert.ToInt16(str.Substring(str.Length - 2));
                        if (controlId <= 9)
                        {
                            controlIdStr = "0" + controlId;
                        }
                        else
                        {
                            controlIdStr = controlId.ToString();
                        }

                        ddl = new DropDownList();
                        ddl = AddSqlDataTypesToDropDownList(ddl);
                        ddl.ID = "DataType_Row_" + controlIdStr;
                        ddl.CssClass = "form-control col-sm-9";
                        ddl.AutoPostBack = true;
                        ddl.SelectedIndexChanged += DataType_Row_OnSelectedIndexChanged;
                        PlaceHolder1.Controls.Add(ddl);
                        controlIDArrayList.Add(ddl.ID.ToString());

                        tb = new TextBox() { ID = "DataTypeNum_Row_" + controlIdStr };
                        tb.CssClass = "form-control col-sm-2";
                        CheckDataType(ddl, tb);
                        PlaceHolder1.Controls.Add(tb);
                        controlIDArrayList.Add(tb.ID.ToString());

                        literal = "</div></div></td><td>";
                        PlaceHolder1.Controls.Add(new LiteralControl(literal));
                    }
                    else if (str.StartsWith("AllowNulls_Row_"))
                    {
                        controlId = Convert.ToInt16(str.Substring(str.Length - 2));
                        if (controlId <= 9)
                        {
                            controlIdStr = "0" + controlId;
                        }
                        else
                        {
                            controlIdStr = controlId.ToString();
                        }
                        cb = new CheckBox() { ID = "AllowNulls_Row_" + controlIdStr };
                        PlaceHolder1.Controls.Add(cb);
                        controlIDArrayList.Add(cb.ID.ToString());

                        literal = "</td><td>";
                        PlaceHolder1.Controls.Add(new LiteralControl(literal));
                    }
                    else if (str.StartsWith("Default_Row_"))
                    {
                        controlId = Convert.ToInt16(str.Substring(str.Length - 2));
                        if (controlId <= 9)
                        {
                            controlIdStr = "0" + controlId;
                        }
                        else
                        {
                            controlIdStr = controlId.ToString();
                        }
                        tb = new TextBox() { ID = "Default_Row_" + controlIdStr };
                        tb.CssClass = "form-control";
                        PlaceHolder1.Controls.Add(tb);
                        controlIDArrayList.Add(tb.ID.ToString());

                        literal = "</td></tr>";
                        PlaceHolder1.Controls.Add(new LiteralControl(literal));
                    }
                }
            }
        }
        private void InsertTableRow()
        {

            string literal;
            TextBox tb;
            DropDownList ddl;
            CheckBox cb;
            ArrayList existingControlIDArrayList = new ArrayList();
            string controlIdStr;

            
            //run if no rows have been dynamically added
            //get last control id +1
            controlId++;
            if(controlId <= 9)
            {
                controlIdStr = "0" + controlId;
            }
            else
            {
                controlIdStr = controlId.ToString();
            }
            literal = "<tr><td></td><td>";
            PlaceHolder1.Controls.Add(new LiteralControl(literal));

            tb = new TextBox() { ID = "Name_Row_" + controlIdStr };
            tb.CssClass = "form-control";
            PlaceHolder1.Controls.Add(tb);
            controlIDArrayList.Add(tb.ID.ToString());

            literal = "</td><td><div class='container'><div class='row'>";
            PlaceHolder1.Controls.Add(new LiteralControl(literal));

            ddl = new DropDownList();
            ddl = AddSqlDataTypesToDropDownList(ddl);
            ddl.ID = "DataType_Row_" + controlIdStr;
            ddl.CssClass = "form-control col-sm-9";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += DataType_Row_OnSelectedIndexChanged;

            PlaceHolder1.Controls.Add(ddl);
            controlIDArrayList.Add(ddl.ID.ToString());

            tb = new TextBox() { ID = "DataTypeNum_Row_" + controlIdStr };
            tb.CssClass = "form-control col-sm-2";
            tb.Enabled = false;
            PlaceHolder1.Controls.Add(tb);
            controlIDArrayList.Add(tb.ID.ToString());

            literal = "</div></div></td><td>";
            PlaceHolder1.Controls.Add(new LiteralControl(literal));

            cb = new CheckBox() { ID = "AllowNulls_Row_" + controlIdStr };
            PlaceHolder1.Controls.Add(cb);
            controlIDArrayList.Add(cb.ID.ToString());

            literal = "</td><td>";
            PlaceHolder1.Controls.Add(new LiteralControl(literal));

            tb = new TextBox() { ID = "Default_Row_" + controlIdStr };
            tb.CssClass = "form-control";
            PlaceHolder1.Controls.Add(tb);
            controlIDArrayList.Add(tb.ID.ToString());

            literal = "</td></tr>";
            PlaceHolder1.Controls.Add(new LiteralControl(literal));

            ViewState["controlIDArrayList"] = controlIDArrayList;
        }
        private void CreateSqlMaintenance()
        {
            ArrayList existingControlIDArrayList = new ArrayList();
            bool controlsArrayExists = false;

            //run if there are dynamically added rows
            //if ViewState.Keys contains controlIDArrayList
            foreach (string str in ViewState.Keys)
            {
                if (str == "controlIDArrayList")
                {
                    controlsArrayExists = true;
                    break;
                }
            }

            //construct sql create table string
            if (controlsArrayExists)
            {
                string sqlCreateQueryStart = "CREATE TABLE [dbo].[" + MaintenanceName.Text + "](";
                string sqlCreateQueryContent = "";
                string sqlCreateQueryEnd = ")";
                string sqlCreateQueryFinal = "";
                List<string> sqlCreateQueryEntryList = new List<string>();
                TextBox tb;
                existingControlIDArrayList = ViewState["controlIDArrayList"] as ArrayList;
                string name = "", datatype = "", datatypeNum = "", allowNull = "", defaultVal = "", columnEntry = "";

                //get pk row
                //read PK row,
                name = Name_Row_PK.Text;
                datatype = DataType_Row_PK.SelectedValue;
                allowNull = AllowNulls_Row_PK.Checked ? "NULL" : "NOT NULL";
                defaultVal = Default_Row_PK.Text;
                if (string.IsNullOrWhiteSpace(defaultVal))
                {
                    defaultVal = "";
                }
                else
                {
                    defaultVal = "DEFAULT " + defaultVal;
                }
                columnEntry = name + " " + datatype + datatypeNum + " " + allowNull + " PRIMARY KEY " + defaultVal;
                sqlCreateQueryEntryList.Add(columnEntry);

                //get dynamically created rows
                //construct each columnEntryString
                int i = 0;
                foreach (string str in existingControlIDArrayList)
                {
                    if (str.StartsWith("Name_Row_"))
                    {
                        name = ((TextBox)PlaceHolder1.FindControl(str)).Text;
                    }
                    else if (str.StartsWith("DataType_Row_"))
                    {
                        datatype = ((DropDownList)PlaceHolder1.FindControl(str)).SelectedValue;
                    }
                    else if (str.StartsWith("DataTypeNum_Row_"))
                    {
                        tb = (TextBox)PlaceHolder1.FindControl(str);
                        if (tb.Enabled)
                        {
                            if (string.IsNullOrWhiteSpace(tb.Text))
                            {
                                //consider default value
                                datatypeNum = GetDefaultParameterValueSqlDataType(datatype);
                            }
                            else
                            {
                                datatypeNum = tb.Text;
                            }
                        }
                        else
                        {
                            datatypeNum = "";
                        }
                    }
                    else if (str.StartsWith("AllowNulls_Row_"))
                    {
                        allowNull = ((CheckBox)PlaceHolder1.FindControl(str)).Checked ? "NULL" : "NOT NULL";
                    }
                    else if (str.StartsWith("Default_Row_"))
                    {
                        defaultVal = ((TextBox)PlaceHolder1.FindControl(str)).Text;
                        if (string.IsNullOrWhiteSpace(defaultVal))
                        {
                            defaultVal = "";
                            columnEntry = name + " " + datatype + datatypeNum + " " + allowNull + " " + defaultVal;
                            sqlCreateQueryEntryList.Add(columnEntry);
                        }
                        else
                        {
                            defaultVal = "DEFAULT " + defaultVal;
                            columnEntry = name + " " + datatype + datatypeNum + " " + allowNull + " " + defaultVal;
                            sqlCreateQueryEntryList.Add(columnEntry);
                        }
                        //if not empty, add default
                    }
                }

                //append to sqlCreateQueryContent
                foreach (string str in sqlCreateQueryEntryList)
                {
                    sqlCreateQueryContent = string.Concat(sqlCreateQueryContent, str, ", ");
                }
                //remove comma at end
                sqlCreateQueryContent = sqlCreateQueryContent.Substring(0, sqlCreateQueryContent.Length - 2);
                sqlCreateQueryFinal = sqlCreateQueryStart + sqlCreateQueryContent + sqlCreateQueryEnd;

                //send createquery to sql server
                string constr = ConfigurationManager.ConnectionStrings["MaintenanceWebUtilityDbEntitiesDataSource"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {

                    using (SqlCommand cmd = new SqlCommand(sqlCreateQueryFinal))
                    {
                        cmd.Connection = con;
                        con.Open();
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("e: " + e);
                        }
                    }
                }
            }
        }
    }
}
 