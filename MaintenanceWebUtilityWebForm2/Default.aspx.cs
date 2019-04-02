﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MaintenanceWebUtilityWebForm2
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<int, bool> empAccessiblesDict = new Dictionary<int, bool>();
            if (Session[SessionKey.UserId] == null)
            {
                Response.Redirect("~/Account/Login");
            }
            else
            {
                //get empAccessibles
                //make stored procedure for empAccessibles
                string constr = ConfigurationManager.ConnectionStrings["MaintenanceWebUtilityDbEntitiesDataSource"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {

                    using (SqlCommand cmd = new SqlCommand("uspGetEmpAccessibles"))
                    {
                        int facilityId = 0;
                        bool isAccessible = false;
                        String facilityLiteral = "";

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", Session[SessionKey.UserId]);
                        cmd.Connection = con;
                        con.Open();
                        var empAccessibles = cmd.ExecuteReader();
                        if (empAccessibles.HasRows)
                        {
                            while (empAccessibles.Read())
                            {
                                facilityId = empAccessibles.GetInt32(0);
                                isAccessible = empAccessibles.GetBoolean(1);
                                empAccessiblesDict.Add(facilityId, isAccessible);
                            }
                        }
                        con.Close();


                        if (empAccessiblesDict[1] == true)
                        {
                            facilityLiteral = @"<div class=""col-xl-3 col-sm-6 mb-3"">
                                                        <div class=""card text-white bg-primary o-hidden h-100"">
                                                            <div class=""card-body"">
                                                                <div class=""card-body-icon"">
                                                                    <i class=""fas fa-fw fa-comments""></i>
                                                                </div>
                                                                <div class=""mr -5"">IT</div>
                                                            </div>
                                                            <a class=""card-footer text-white clearfix small z-1"" href=""#"">
                                                                <span class=""float-left"">View Details</span>
                                                                <span class=""float-right"">
                                                                    <i class=""fas fa-angle-right""></i>
                                                                </span>
                                                            </a>
                                                        </div>
                                                    </div>";
                            empAccessibleLiteral.Text = String.Concat(empAccessibleLiteral.Text, facilityLiteral);
                        }
                        if (empAccessiblesDict[2] == true)
                        {
                            facilityLiteral = @"<div class=""col-xl-3 col-sm-6 mb-3"">
                                                        <div class=""card text-white bg-warning o-hidden h-100"">
                                                            <div class=""card-body"">
                                                                <div class=""card-body-icon"">
                                                                    <i class=""fas fa-fw fa-comments""></i>
                                                                </div>
                                                                <div class=""mr -5"">SHS</div>
                                                            </div>
                                                            <a class=""card-footer text-white clearfix small z-1"" runat=""server"" href=""./Facilities/Index.aspx?facilityId=2"">
                                                                <span class=""float-left"">View Details</span>
                                                                <span class=""float-right"">
                                                                    <i class=""fas fa-angle-right""></i>
                                                                </span>
                                                            </a>
                                                        </div>
                                                    </div>";
                            empAccessibleLiteral.Text = String.Concat(empAccessibleLiteral.Text, facilityLiteral);
                        }
                        if (empAccessiblesDict[3] == true)
                        {
                            facilityLiteral = @"<div class=""col-xl-3 col-sm-6 mb-3"">
                                                        <div class=""card text-white bg-success o-hidden h-100"">
                                                            <div class=""card-body"">
                                                                <div class=""card-body-icon"">
                                                                    <i class=""fas fa-fw fa-comments""></i>
                                                                </div>
                                                                <div class=""mr -5"">CS</div>
                                                            </div>
                                                            <a class=""card-footer text-white clearfix small z-1"" href =""#"">
                                                                <span class=""float-left"">View Details</span>
                                                                <span class=""float-right"">
                                                                    <i class=""fas fa-angle-right""></i>
                                                                </span>
                                                            </a>
                                                        </div>
                                                    </div>";
                            empAccessibleLiteral.Text = String.Concat(empAccessibleLiteral.Text, facilityLiteral);
                        }
                        if (empAccessiblesDict[4] == true)
                        {
                            facilityLiteral = @"<div class=""col-xl-3 col-sm-6 mb-3"">
                                                        <div class=""card text-white bg-danger o-hidden h-100"">
                                                            <div class=""card-body"">
                                                                <div class=""card-body-icon"">
                                                                    <i class=""fas fa-fw fa-comments""></i>
                                                                </div>
                                                                <div class=""mr -5"">PSY</div>
                                                            </div>
                                                            <a class=""card-footer text-white clearfix small z-1"" href =""#"">
                                                                <span class=""float-left"">View Details</span>
                                                                <span class=""float-right"">
                                                                    <i class=""fas fa-angle-right""></i>
                                                                </span>
                                                            </a>
                                                        </div>
                                                    </div>";
                            empAccessibleLiteral.Text = String.Concat(empAccessibleLiteral.Text, facilityLiteral);
                        }
                    }
                }
            }
        }
    }
}