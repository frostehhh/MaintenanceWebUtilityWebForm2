﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MaintenanceWebUtilityWebForm2.DynamicMaintenance
{
    public partial class Create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["MaintenanceTableName"] = Session["MaintenanceTableName"].ToString();
                CreatePlaceHolderContents();
            }
        }
        protected void CreatePlaceHolderContents()
        {
            ArrayList tableColumnsSchema = GetTableColumnsSchema();
            PlaceHolder pl = EditRow_PlaceHolder;

            //PK or first column will not be rendered.
            pl.Controls.Add(new LiteralControl("<table>"));
            for(int i = 1; i<tableColumnsSchema.Count; i++)
            {
                ArrayList col = (ArrayList)tableColumnsSchema[i];
                string colName = col[0].ToString();
                string datatype = col[1].ToString();

                pl.Controls.Add(new LiteralControl("<tr><td>"));
                Label label = new Label() { Text = colName};
                pl.Controls.Add(label);
                pl.Controls.Add(new LiteralControl("</td><td>"));

                //if textbox, date, or checkbox
                string idText = (i < 10) ? "0" + i.ToString() : i.ToString();
                if (datatype == "date")
                {
                    TextBox tb = new TextBox()
                    {
                        ID = "columnName_Input_" + idText,
                        CssClass = "form-control valid"
                    };
                    tb.Attributes.Add("Type", "date");
                    pl.Controls.Add(tb);
                }
                else if (datatype == "datetime")
                {
                    TextBox tb = new TextBox()
                    {
                        ID = "columnName_Input_" + idText,
                        CssClass = "form-control valid"
                    };
                    tb.Attributes.Add("Type", "datetime-local");
                    pl.Controls.Add(tb);
                }
                else if(datatype == "bit")
                {
                    CheckBox cb = new CheckBox()
                    {
                        ID = "columnName_Input_" + idText
                    };
                    pl.Controls.Add(cb);
                }
                else
                {
                    TextBox tb = new TextBox()
                    {
                        ID = "columnName_Input_" + idText,
                        CssClass = "form-control valid"
                    };
                    pl.Controls.Add(tb);
                }
               
                pl.Controls.Add(new LiteralControl("</td></tr>"));
            }
            pl.Controls.Add(new LiteralControl("</table>"));
        }
        private ArrayList GetTableColumnsSchema()
        {
            ArrayList values = new ArrayList();
            string constr = ConfigurationManager.ConnectionStrings["MaintenanceWebUtilityDbEntitiesDataSource"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string sql = $"SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns WHERE TABLE_NAME = @TableName";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = ViewState["MaintenanceTableName"];
                    con.Open();
                    var data = cmd.ExecuteReader();
                    while (data.Read())
                    {
                        values.Add(new ArrayList() { data["COLUMN_NAME"].ToString(), data["DATA_TYPE"].ToString(), data["CHARACTER_MAXIMUM_LENGTH"].ToString() });
                    }
                }
            }
            return values;
        }
    }
}