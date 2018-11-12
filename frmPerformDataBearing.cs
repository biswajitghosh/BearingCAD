
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmPerformance                         '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//....Class Constructor.
//       Public Sub        New                                 ()

//   METHODS:
//   -------
//       Private Sub       DisplayData                         ()

//       Private Sub       cmdClose_Click                      ()
//       Private Sub       SaveData                            ()
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace BearingCAD22
{
    public partial class frmPerformDataBearing : Form
    {
        #region "MEMBER VARIABLE DECLARATION"
        //***********************************

            private clsProduct mProduct;

        #endregion

        #region "FORM CONSTRUCTOR RELATED ROUTINE"
        //****************************************

            public frmPerformDataBearing()
            //======================================
            {
                InitializeComponent();
            }

        #endregion

        #region "FORM LOAD RELATED EVENT"

            private void frmPerformance_Load(object sender, EventArgs e)
            //===========================================================
            {
                Initialize_LocalObject();
                
                //....Data Display & Local Object.
                DisplayData();
               
                SetControl();          
           }

            private void Initialize_LocalObject()
            //===================================
            {
                mProduct = (clsProduct)((clsProduct)modMain.gProject.Product).Clone();
            }     
     

            private void SetControl()
            //=======================                           
            {
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.English)
                {
                    lblPower_HP_Unit.Text = "HP";
                }
                else
                {
                    lblPower_HP_Unit.Text = "kW";
                }

                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.English)
                {
                    lblTempRise_F_Radial_Unit.Text = Convert.ToString((char)176) + "F";
                }
                else
                {
                    lblTempRise_F_Radial_Unit.Text = Convert.ToString((char)176) + "C";
                }
            
            }


            private void DisplayData()
            //========================
            {
                //Radial Bearing
                //---------------

                
                if (modMain.gProject.Product.Unit.System == clsUnit.eSystem.Metric)
                {
                    //....Power
                    txtPower_HP_Radial.Text = modMain.ConvDoubleToStr(modMain.gProject.Product.Unit.CFac_Power_EngToMet(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.Power), "##0.00");

                    //....Temp Rise
                    txtTempRise_F_Radial.Text = modMain.ConvDoubleToStr(modMain.gProject.Product.Unit.CFac_Temp_EngToMet(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TempRise), "#0.0");
                }
                else
                {
                    //....Power
                    txtPower_HP_Radial.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.Power, "##0.00");

                    //....Temp Rise
                    txtTempRise_F_Radial.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TempRise, "#0.0");
                }

            }

        #endregion

        #region "CONTROL EVENT ROUTINE"
            //*************************

            #region "COMMAND BUTTON RELATED ROUTINE"
            //--------------------------------------

                private void cmdOK_Click(object sender, EventArgs e)
                //===================================================
                {
                    SaveData();
                    this.Hide();
                    modMain.gfrmBearing.ShowDialog();   
                    ////modMain.gfrmBearingDesignDetails.ShowDialog();                
                }


                private void SaveData()
                //=======================
                {
                    if (modMain.gProject.Product.Unit.System == clsUnit.eSystem.Metric)
                    {
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.Power = modMain.gProject.Product.Unit.CFac_Power_MetToEng(modMain.ConvTextToDouble(txtPower_HP_Radial.Text));
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TempRise = modMain.gProject.Product.Unit.CFac_Temp_MetToEng(modMain.ConvTextToDouble(txtTempRise_F_Radial.Text));
                    }
                    else
                    {
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.Power = modMain.ConvTextToDouble(txtPower_HP_Radial.Text);
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.TempRise = modMain.ConvTextToDouble(txtTempRise_F_Radial.Text);
                    }
                }


                private void cmdCancel_Click(object sender, EventArgs e)
                //======================================================
                {
                    this.Hide();
                }


            #endregion

            #region "TEXTBOX RELATED ROUTINE"
            //-------------------------------

                #region "POWER"

                     private void txtPower_TextChanged(object sender, EventArgs e)
                    //=============================================================
                    {
                        TextBox pTxtBox = (TextBox)sender;

                        switch (pTxtBox.Name)
                        {
                            case "txtPower_HP_Radial":
                                //--------------------
                                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.Power = modMain.ConvTextToDouble(txtPower_HP_Radial.Text);

                                //Double pTempRise_F = ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.TempRise_F;
                                //txtTempRise_F_Radial.Text = modMain.ConvDoubleToStr(pTempRise_F, "#0.0");

                                break; 
                        }                           
                    }                          

                #endregion




                #region " TEMP RISE"

                    private void txtTempRise_TextChanged(object sender, EventArgs e)
                    //==============================================================
                    {
                        TextBox pTxtBox = (TextBox)sender;

                        switch (pTxtBox.Name)
                        {
                            case "txtTempRise_F_Radial":
                                //----------------------
                                ((clsBearing_Radial_FP)mProduct.Bearing).PerformData.TempRise = modMain.ConvTextToDouble(txtTempRise_F_Radial.Text); 
                               
                                break;
                        }
                    }
                           

                #endregion



            #endregion

    
        #endregion


    }
}
