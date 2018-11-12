
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmBearingDesignDetails                '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  31OCT18                                '
//                                                                              '
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Collections;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Data.OleDb;

namespace BearingCAD22
{
    public partial class frmBearingDesignDetails : Form
    {
        private const double mcEPS = 0.0001;                //....An aribitrarily small number.

        #region "MEMBER VARIABLES:"
        //************************

            //.....Local Bearing Object.    
            private clsBearing_Radial_FP mBearing_Radial_FP;
            private clsEndPlate[] mEndPlate = new clsEndPlate[2];

            //....Header:
            private Boolean mblnL_ManuallyChanged = false;
            private Boolean mblnDepth_EndPlate_F_ManuallyChanged = false;
            private Boolean mblnDepth_EndPlate_B_ManuallyChanged = false;
      

            private Label[] mlblMetric;     

            //....Tab: Oil-Inlet           
            private Boolean mblnAnnulus_Depth_ManuallyChanged = false;
            private Boolean mblnAnnulus_Dia_ManuallyChanged = false;
            private Boolean mblnOilInlet_Annulus_Loc_Back_ManuallyChanged = false;

            //....Tab: Web Relief
            private TextBox[] mtxtAxialSealGap;

            //....Tab: S/L
            private Boolean mblnSL_LScrew_Loc_Center_ManuallyChanged = false;
            private Boolean mblnSL_RScrew_Loc_Center_ManuallyChanged = false;

            //....Tab: Mount
            private TextBox[] mTxtMount_HolesAngBet_Front;
            private TextBox[] mTxtMount_HolesAngBet_Back;
            private Boolean mblnMount_Front_Copy = false;
            private Boolean mblnMount_Holes_Count_Front_ManuallyChanged = false;
            private Boolean mblnMount_Holes_Count_Back_ManuallyChanged = false;


            private double[] mEndPlate_OD_ULimit = new double[2];               
            private double[] mEndPlate_OD_LLimit = new double[2];

            private Boolean mblnMount_EndConfig_OD_Front = false;
            private Boolean mblnMount_EndConfig_OD_Back = false;

            private double[] mMount_DBC_LLimit = new double[2];     
            private double[] mMount_DBC_ULimit = new double[2];

            private Boolean mblnEndConfig_DBC_Front = false;
            private Boolean mblnEndConfig_DBC_Back = false;

            
        #endregion


        #region "FORM CONSTRUCTOR:"
        //*************************

            public frmBearingDesignDetails()
            //=============================
            {
                InitializeComponent();
                mtxtAxialSealGap = new TextBox[] { txtAxialSealGap_Front, txtAxialSealGap_Back };
                mlblMetric = new Label[] { lblOilInlet_Orifice_D_Unit, lblOilInlet_Orifice_D, lblOilInlet_Orifice_DDrill_CBore_Unit, lblOilInlet_Orifice_DDrill_CBore, 
                                           lblMount_EndConfig_OD_Front_Unit, lblMount_EndConfig_OD_Front_MM, lblMount_WallT_Front_Unit, lblMount_WallT_Front, 
                                           lblEndConfig_DBC_Front_Unit, lblEndConfig_DBC_Front, lblMount_EndConfig_OD_Back_Unit, lblMount_EndConfig_OD_Back_MM, 
                                           lblMount_WallT_Back_Unit, lblMount_WallT_Back, lblEndConfig_DBC_Back_Unit, lblEndConfig_DBC_Back };
            }

        #endregion


        #region "FORM EVENT ROUTINES:"
        //****************************

            private void frmBearingDesgnDetails_Load(object sender, EventArgs e)
            //==================================================================    
            {
                Cursor = Cursors.WaitCursor;

                mblnL_ManuallyChanged = false;
                mblnDepth_EndPlate_F_ManuallyChanged = false;
                mblnDepth_EndPlate_B_ManuallyChanged  = false;

                double pARP_Dowel_Hole_Depth_Low = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Dowel.Hole.Depth_Low;
                int pMountHoleCount_Front = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].Count;
                double pMountHole_AngStart_Front = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngStart;
                int pMountHoleCount_Back = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].Count;
                double pMountHole_AngStart_Back = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngStart;
                               
                      
                //....Local Object.
                SetLocalObject();

                for (int i = 0; i < 2; i++)
                {
                    mEndPlate_OD_ULimit[i] = mEndPlate[i].OD_ULimit(modMain.gProject.Product);
                }

                //....Initialize 
                InitializeControls();

                //  Tab: OilInlet:
                //  --------
                Load_OilInlet_cmbOrificeStartPos();
                Load_OilInlet_Orifice_CBoreDia();       //AES 05OCT18    
                //Load_OilInlet_Orifice_Dist_Holes();

                cmbOilInlet_Orifice_LD.Items.Clear();
                cmbOilInlet_Orifice_LD.Items.Add("1");
                cmbOilInlet_Orifice_LD.Items.Add("1.5");
                cmbOilInlet_Orifice_LD.Items.Add("2");
                cmbOilInlet_Orifice_LD.SelectedIndex = 1;

                //  Tab: S/L Hardware:
                //  -------------
                Load_SL_HardWare();

                //  Tab: ARP:
                //  ----
                Load_ARP();
                optBearingOD.Checked = true;

                //  Tab: Mounting:       
                //  ---------
                Load_MountingDetails("Front");
                Load_MountingDetails("Back");
                
                //....Set Control.
                SetControls();

                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Dowel.Hole_Depth_Low = pARP_Dowel_Hole_Depth_Low;

                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].Count = pMountHoleCount_Front;
                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngStart = pMountHole_AngStart_Front;
                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].Count = pMountHoleCount_Back;
                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngStart = pMountHole_AngStart_Back;

                //....Local Object.
                SetLocalObject();       

               
                //for (int i = 0; i < 2; i++)
                //{
                //    mEndPlate_OD_ULimit[i] = mEndPlate[i].OD_ULimit(modMain.gProject.Product);   
                //}

                //....Display Data.
                DisplayData();

                Cursor = Cursors.Default;
            }

            private void frmBearingDesignDetails_Activated(object sender, EventArgs e)
            //========================================================================
            {
               
            }

            private void SetLocalObject()
            //===========================
            {
               mBearing_Radial_FP = (clsBearing_Radial_FP)((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Clone();

               for (int i = 0; i < 2; i++)
               {
                   if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                   {                      
                       mEndPlate[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
                   }
               }
            }


            #region "....INITIALIZE CONTROLS ROUTINES:"

                private void InitializeControls()
                //===============================
                {
                    SetTabPages(mBearing_Radial_FP.SplitConfig, tabSplitLineHardware);

                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB &&
                        modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                    {
                        //....Both End Configs are Thrust Bearings. Temp. Sensor doesn't exist.
                        SetTabPages(false, tabTempSensor);
                    }
                    else
                    {   //....At least, one of the End Configs is a Seal.
                        ////SetTabPages(true, tabTempSensor);     
                    }
                    
                    SetTabPages(false, tabTempSensor);
                    SetTabPages(false, tabEDM);

                    //....Initialize Checkboxes.
                    //
                        //....Mount:                        
                        chkMount_Screw_LenLim_Front.Checked = false;
                        chkMount_Screw_LenLim_Back.Checked = false;
                    
                        lblMsg_Mount_EquiSpaced_Front.Visible = false;
                        lblMsg_Mount_EquiSpaced_Back.Visible = false;

                        //....S/L Hardware:                        
                        chkSL_Dowel_LenLim.Checked = false;
                        chkSL_Screw_LenLim.Checked = false;


                    //....Create TextBox Arrays:        
                    mTxtMount_HolesAngBet_Front = new TextBox[] {txtMount_HolesAngBet1_Front,txtMount_HolesAngBet2_Front,
                                                                 txtMount_HolesAngBet3_Front,txtMount_HolesAngBet4_Front,
                                                                 txtMount_HolesAngBet_Front,txtMount_HolesAngBet6_Front,
                                                                 txtMount_HolesAngBet7_Front};

                    mTxtMount_HolesAngBet_Back = new TextBox[] {txtMount_HolesAngBet1_Back,txtMount_HolesAngBet2_Back,
                                                                txtMount_HolesAngBet3_Back,txtMount_HolesAngBet4_Back,
                                                                txtMount_HolesAngBet5_Back,txtMount_HolesAngBet6_Back,
                                                                txtMount_HolesAngBet7_Back};
                    //....Show Labels for Metric 
                    for (int i = 0; i < mlblMetric.Length - 1; i++)
                    {
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            mlblMetric[i].Visible = true;
                        }
                        else
                        {
                            mlblMetric[i].Visible = false;
                        }
                    }
                }


                private void SetTabPages(Boolean Checked_In, TabPage TabPage_In)
                //==============================================================
                {                    
                    TabPage pTabOilInlet = tabOilInlet;
                    TabPage pTabWebRelief = tabWebRelief;
                    TabPage pTabAntiRotationPin = tabAntiRotationPin;
                    TabPage pTabSplitLineHardware = tabSplitLineHardware;  
                    TabPage pTabFlange = tabFlange;
                    TabPage pTabMountingHoles = tabMounting;
                    TabPage pTabTempSensorHoles = tabTempSensor;
                    TabPage pTabEDM = tabEDM;

                    if (!Checked_In)
                    {   tbBearingDesignDetails.TabPages.Remove(TabPage_In);}

                    Boolean pTab_Exist = false;
                    foreach (TabPage pTp in tbBearingDesignDetails.TabPages)
                    {
                        if (pTp.Text == TabPage_In.Text)
                        {
                            pTab_Exist = true;
                        }
                    }
                   
                    if ((Checked_In) && (!pTab_Exist))
                    {
                        tbBearingDesignDetails.TabPages.Clear();
                        tbBearingDesignDetails.TabPages.Add(pTabOilInlet);
                        tbBearingDesignDetails.TabPages.Add(pTabWebRelief);
                        tbBearingDesignDetails.TabPages.Add(pTabAntiRotationPin);       
                        tbBearingDesignDetails.TabPages.Add(pTabSplitLineHardware);
                        tbBearingDesignDetails.TabPages.Add(pTabFlange);
                        tbBearingDesignDetails.TabPages.Add(pTabMountingHoles);
                        tbBearingDesignDetails.TabPages.Add(pTabTempSensorHoles);
                        tbBearingDesignDetails.TabPages.Add(pTabEDM);
                    }

                    tbBearingDesignDetails.Refresh();
                }

            #endregion


            #region "....SET CONTROLS ROUTINES:"

                private void SetControls()
                //=======================                           
                {
                    Boolean pblnSet = false;

                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.English)
                    {
                        lblFlowReqd_Unit.Text = "gpm";
                    }
                    else                                              //....Metric
                    {
                        lblFlowReqd_Unit.Text = "LPM";
                    }

                    //....Oil Inlet
                    if (mBearing_Radial_FP.OilInlet.Orifice_Exists_2ndSet())
                    {
                        pblnSet = true;
                    }
                    else
                    {
                        pblnSet = false;
                    }
                    
                    lblSeparator.Visible = pblnSet;
                    //lblOrificeHoles1.Visible = pblnSet;
                    //lblOrificeHoles2.Visible = pblnSet;
                    //lblOrificeHoles3.Visible = pblnSet;
                    //lblOilInlet_DistBetFeedHoles.Visible = pblnSet;
                    //txtOilInlet_Orifice_Dist_Holes.Visible = pblnSet;

                    //....Annulus
                    grpOilInlet_Annulus.Visible = chkOilInlet_Annulus_Exists.Checked;

                    //....Flange
                    SetControls_Flange();

                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal &&
                        modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                    {
                        lblDepth_Front.Text = "Seal";
                        lblDepth_Back.Text  = "Seal";
                        lblMount_EndConfig_OD_Front.Text = "Seal OD";
                        lblMount_EndConfig_OD_Back.Text  = "Seal OD";
                    }
                    else if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal &&
                             modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                    {
                        lblDepth_Front.Text = "Seal";
                        lblDepth_Back.Text  = "T/B";
                        lblMount_EndConfig_OD_Front.Text = "Seal OD";
                        lblMount_EndConfig_OD_Back.Text  = "T/B OD";
                    }
                    else if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB &&
                             modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                    {
                        lblDepth_Front.Text = "T/B";
                        lblDepth_Back.Text  = "Seal";
                        lblMount_EndConfig_OD_Front.Text = "T/B OD";
                        lblMount_EndConfig_OD_Back.Text  = "Seal OD";
                    }
                    else if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB &&
                             modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                    {
                        lblDepth_Front.Text = "T/B";
                        lblDepth_Back.Text  = "T/B";
                        lblMount_EndConfig_OD_Front.Text = "T/B OD";
                        lblMount_EndConfig_OD_Back.Text  = "T/B OD";
                    }
                }

                #region "WEB RELIEF:"

                    private void SetControl_MillRelief()        
                    //======================================
                    {
                        lblMillRelief_D.Visible = chkMillRelief_Exists.Checked;              
                        cmbMillRelief_D_Desig.Visible = chkMillRelief_Exists.Checked;
                    }
                #endregion


                #region "FLANGE:"

                    private void SetControls_Flange()
                    //================================
                    {
                        lblFlange_D.Visible = chkFlange_Exists.Checked;
                        txtFlange_D.Visible = chkFlange_Exists.Checked;
                        lblFlange_Wid.Visible = chkFlange_Exists.Checked;
                        txtFlange_Wid.Visible = chkFlange_Exists.Checked;
                        lblFlange_DimStart_Back.Visible = chkFlange_Exists.Checked;
                        txtFlange_DimStart_Back.Visible = chkFlange_Exists.Checked;
                        grpInsertedOn.Visible = chkFlange_Exists.Checked;
                    }
               #endregion

            #endregion


            #region "Display Data:"

                private void DisplayData()      
                //========================
                {
                    //....Reset Msg Text.
                    lblMsg1.Text = "";
               
                    //....Set TabPage Index.
                    tbBearingDesignDetails.SelectedIndex = 0;

                    #region "Header:"
                    //---------------
                        //  Bearing Length:
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)      // PB 21OCT18. Have English Unit first and then Metric to maintain consistency. 
                        {
                            txtL.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.L));
                        }
                        else
                        {
                            txtL.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.L);
                        }

                        //  Depths:
                        //  -------
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            if (mBearing_Radial_FP.Depth_EndPlate[0] != 0.0)
                            {
                                if (Math.Abs(mBearing_Radial_FP.Depth_EndPlate[0] - mBearing_Radial_FP.Calc_Depth_EndPlate()) > modMain.gcEPS)
                                {
                                    txtDepth_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Depth_EndPlate[0]));
                                }
                                else
                                {
                                    txtDepth_EndConfig_Front.Text =modMain.gProject.PNR.Unit.WriteInUserL( modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Calc_Depth_EndPlate()));
                                }
                            }

                            if (mBearing_Radial_FP.Depth_EndPlate[1] != 0.0)
                            {
                                if (Math.Abs(mBearing_Radial_FP.Depth_EndPlate[1] - mBearing_Radial_FP.Calc_Depth_EndPlate()) > modMain.gcEPS)
                                {
                                    txtDepth_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Depth_EndPlate[1]));
                                }
                                else
                                {
                                    txtDepth_EndConfig_Back.Text =modMain.gProject.PNR.Unit.WriteInUserL( modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Calc_Depth_EndPlate()));
                                }
                            }
                        }
                        else
                        {
                            if (mBearing_Radial_FP.Depth_EndPlate[0] != 0.0)
                            {
                                if (Math.Abs(mBearing_Radial_FP.Depth_EndPlate[0] - mBearing_Radial_FP.Calc_Depth_EndPlate()) > modMain.gcEPS)
                                {
                                    txtDepth_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Depth_EndPlate[0]);
                                }
                                else
                                {
                                    txtDepth_EndConfig_Front.Text =modMain.gProject.PNR.Unit.WriteInUserL( mBearing_Radial_FP.Calc_Depth_EndPlate());
                                }
                            }

                            if (mBearing_Radial_FP.Depth_EndPlate[1] != 0.0)
                            {
                                if (Math.Abs(mBearing_Radial_FP.Depth_EndPlate[1] - mBearing_Radial_FP.Calc_Depth_EndPlate()) > modMain.gcEPS)
                                {
                                    txtDepth_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Depth_EndPlate[1]);
                                }
                                else
                                {
                                    txtDepth_EndConfig_Back.Text =modMain.gProject.PNR.Unit.WriteInUserL( mBearing_Radial_FP.Calc_Depth_EndPlate());
                                }
                            }
                        }

                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            txtLength_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].L));
                            txtLength_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].L));
                        }
                        else
                        {
                            txtLength_EndConfig_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[0].L);
                            txtLength_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mEndPlate[1].L);
                        }

                    #endregion


                    #region  "Tab: OilInlet"
                    //  -------------------

                        string pOilInlet_LD = mBearing_Radial_FP.OilInlet.Orifice.Ratio_L_D.ToString();

                        Boolean pLD_Exists = false;
                        for (int i = 0; i < cmbOilInlet_Orifice_LD.Items.Count; i++)
                        {
                            if (cmbOilInlet_Orifice_LD.Items[i].ToString() == pOilInlet_LD)
                            {
                                pLD_Exists = true;
                                break;
                            }
                        }
                        if (!pLD_Exists)
                        {
                            cmbOilInlet_Orifice_LD.Items.Add(pOilInlet_LD);
                        }


                        if (mBearing_Radial_FP.Pad.Count == mBearing_Radial_FP.OilInlet.Orifice.Count)
                        {
                            cmbOilInlet_Count_MainOilSupply.SelectedIndex = 0;
                        }
                        else
                        {
                            cmbOilInlet_Count_MainOilSupply.SelectedIndex = 1;
                        }

                        cmbOilInlet_Count_MainOilSupply.Enabled = false;

                        txtOilInlet_Orifice_D.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing_Radial_FP.OilInlet.Orifice.D);

                        int pIndx = cmbOilInlet_Orifice_LD.Items.IndexOf(mBearing_Radial_FP.OilInlet.Orifice.Ratio_L_D.ToString());
           

                        //....Flow Reqd
                        if (modMain.gProject.Product.Unit.System == clsUnit.eSystem.Metric)
                        {
                            txtFlowReqd_gpm_Radial.Text = modMain.ConvDoubleToStr(modMain.gProject.Product.Unit.CFac_GPM_EngToMet(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.FlowReqd), "#0.00");
                        }
                        else
                        {
                            txtFlowReqd_gpm_Radial.Text = modMain.ConvDoubleToStr(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PerformData.FlowReqd, "#0.00");
                        }

                        //....Orifice
                            cmbOilInlet_Orifice_StartPos.Text = mBearing_Radial_FP.OilInlet.Orifice.StartPos.ToString();
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                //txtOilInlet_Orifice_DDrill_CBore.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.OilInlet.Orifice.D_CBore));
                                //cmbOilInlet_Orifice_CBoreDia.Text = 
                                txtOilInlet_Orifice_Loc_BackFace.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.OilInlet.Orifice.Loc_Back));
                                txtOilInlet_Orifice_L.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.OilInlet.Calc_Orifice_L()));                                
                            }
                            else
                            {
                                //txtOilInlet_Orifice_DDrill_CBore.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.OilInlet.Orifice.D_CBore);
                                txtOilInlet_Orifice_Loc_BackFace.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.OilInlet.Orifice.Loc_Back);
                                txtOilInlet_Orifice_L.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.OilInlet.Calc_Orifice_L());
                            }

                            for (int i = 0; i < cmbOilInlet_Orifice_CBoreDia.Items.Count; i++)
                            {
                                string pVal = cmbOilInlet_Orifice_CBoreDia.Items[i].ToString();
                                Double pNumerator, pDenominator;
                                Double pFinal = 0.0;

                                if (pVal.ToString() != "1")
                                {
                                    pVal = pVal.Remove(pVal.Length - 1);
                                    pNumerator = Convert.ToInt32(modMain.ExtractPreData(pVal, "/"));
                                    pDenominator = Convert.ToInt32(modMain.ExtractPostData(pVal, "/"));
                                    pFinal = Convert.ToDouble(pNumerator / pDenominator);
                                }
                                else
                                {
                                    pFinal = Convert.ToDouble(pVal);
                                }

                                if (Math.Abs(pFinal - mBearing_Radial_FP.OilInlet.Orifice.D_CBore)<modMain.gcEPS)
                                {
                                    cmbOilInlet_Orifice_CBoreDia.SelectedIndex = i;
                                    break;
                                }
                            }
                  
                            cmbOilInlet_Orifice_LD.SelectedIndex = -1;
                            cmbOilInlet_Orifice_LD.SelectedIndex = pIndx;


                            mBearing_Radial_FP.OilInlet.Annulus_Exists = true;      //AES 14AUG18
                            chkOilInlet_Annulus_Exists.Checked = mBearing_Radial_FP.OilInlet.Annulus.Exists;
                            chkOilInlet_Annulus_Wid.Checked = true;

                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                if (chkOilInlet_Annulus_Exists.Checked)
                                {
                                    txtOilInlet_Annulus_Area_Reqd.Text = modMain.gProject.PNR.Unit.WriteInUserL(Math.Ceiling( modMain.gProject.PNR.Unit.CFac_Area_EngToMet(mBearing_Radial_FP.OilInlet.Annulus.Area)));

                                    txtOilInlet_Annulus_Wid.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.OilInlet.Annulus.Wid));
                                    txtOilInlet_Annulus_Depth.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.OilInlet.Annulus.Depth));
                                    txtOilInlet_Annulus_Dia.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.OilInlet.Annulus.D));                                   
                                    txtOilInlet_Annulus_Loc_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.OilInlet.Annulus.Loc_Back));
                                }

                                //....If Orifice_Count =   Count_Pad, Dist_FeedHole = 0
                                //                     = 2*Count_Pad, Dist_FeedHole = non-zero.
                                //if (mBearing_Radial_FP.OilInlet.Orifice_Exists_2ndSet())
                                //{
                                //    txtOilInlet_Orifice_Dist_Holes.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.OilInlet.Orifice.Dist_Holes));
                                //}
                            }
                            else
                            {
                                if (chkOilInlet_Annulus_Exists.Checked)
                                {
                                    txtOilInlet_Annulus_Area_Reqd.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.OilInlet.Annulus.Area);
                                    txtOilInlet_Annulus_Wid.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.OilInlet.Annulus.Wid);
                                    txtOilInlet_Annulus_Depth.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.OilInlet.Annulus.Depth);
                                    txtOilInlet_Annulus_Dia.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.OilInlet.Annulus.D);                                  
                                    txtOilInlet_Annulus_Loc_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.OilInlet.Annulus.Loc_Back);
                                }

                                //....If Orifice_Count =   Count_Pad, Dist_FeedHole = 0
                                //                     = 2*Count_Pad, Dist_FeedHole = non-zero.
                                //if (mBearing_Radial_FP.OilInlet.Orifice_Exists_2ndSet())
                                //{
                                //    txtOilInlet_Orifice_Dist_Holes.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.OilInlet.Orifice.Dist_Holes);
                                //}
                            }

                    #endregion


                    #region "Tab: Web Relief:"
                    //  -----------------
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            txtMillRelief_D_PadRelief.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.MillRelief.D_PadRelief()));
                            chkMillRelief_Exists.Checked = mBearing_Radial_FP.MillRelief.Exists;
                            SetControl_MillRelief();
                            cmbMillRelief_D_Desig.Text = mBearing_Radial_FP.MillRelief.D_Desig;

                            mtxtAxialSealGap[0].Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.MillRelief.AxialSealGap[0]));
                        }
                        else
                        {
                            txtMillRelief_D_PadRelief.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.MillRelief.D_PadRelief());
                            chkMillRelief_Exists.Checked = mBearing_Radial_FP.MillRelief.Exists;
                            SetControl_MillRelief();
                            cmbMillRelief_D_Desig.Text = mBearing_Radial_FP.MillRelief.D_Desig;

                            mtxtAxialSealGap[0].Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.MillRelief.AxialSealGap[0]);
                        }

                    #endregion

                    
                    #region "Tab: S/L Hardware:"
                    //  -------------------  
                        //....Screw:
                        //
                        cmbSL_Screw_Spec_UnitSystem.Text = mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString();
                        double pScrewL = mBearing_Radial_FP.SL.Screw.Spec.L;
                        string pstrScrew_L = mBearing_Radial_FP.SL.Screw.Spec.L.ToString("#0");

                        double pSL_Dowel_HoleDepthUp = mBearing_Radial_FP.SL.Dowel.Hole.Depth_Up;
                        double pSL_Dowel_HoleDepthLow = mBearing_Radial_FP.SL.Dowel.Hole.Depth_Low;

                        if (mBearing_Radial_FP.SL.Screw.Spec.Type != null && mBearing_Radial_FP.SL.Screw.Spec.Type != "")
                            cmbSL_Screw_Spec_Type.Text = mBearing_Radial_FP.SL.Screw.Spec.Type;
                        else if (cmbSL_Screw_Spec_Type.Items.Count > 0)
                            cmbSL_Screw_Spec_Type.SelectedIndex = 0;

                        if (mBearing_Radial_FP.SL.Screw.Spec.Mat != null && mBearing_Radial_FP.SL.Screw.Spec.Mat != "")
                            cmbSL_Screw_Spec_Mat.Text = mBearing_Radial_FP.SL.Screw.Spec.Mat;
                        else if (cmbSL_Screw_Spec_Mat.Items.Count > 0)
                            cmbSL_Screw_Spec_Mat.SelectedIndex = 0;

                        if (mBearing_Radial_FP.SL.Screw.Spec.D_Desig != null && mBearing_Radial_FP.SL.Screw.Spec.D_Desig != "")
                            cmbSL_Screw_Spec_D_Desig.Text = mBearing_Radial_FP.SL.Screw.Spec.D_Desig;
                        else if (cmbSL_Screw_Spec_D_Desig.Items.Count > 0)
                            cmbSL_Screw_Spec_D_Desig.SelectedIndex = 0;

                        if (mBearing_Radial_FP.SL.Screw.Spec.Pitch != 0.0F)
                            cmbSL_Screw_Spec_Pitch.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Screw.Spec.Pitch);
                        else if (cmbSL_Screw_Spec_Pitch.Items.Count > 0)
                            cmbSL_Screw_Spec_Pitch.SelectedIndex = 0;                       

                        Update_SL_Screw_L();

                        if (pScrewL > mcEPS)
                        {
                            Boolean pSrew_L_Exists = false;
                            for (int i = 0; i < cmbSL_Screw_Spec_L.Items.Count; i++)
                            {
                                if (cmbSL_Screw_Spec_L.Items[i].ToString() == pstrScrew_L)
                                {
                                    pSrew_L_Exists = true;
                                    break;
                                }
                            }
                            if (!pSrew_L_Exists)
                            {
                                cmbSL_Screw_Spec_L.Items.Add(pstrScrew_L);
                            }
                            mBearing_Radial_FP.SL.Screw.Spec_L = pScrewL;
                        }
                                       
                        //Check_SpLineThread_LLim(mBearing.SL.Screw_Spec.L, Get_SL_Screw_L(mBearing));         

                        if (mBearing_Radial_FP.SL.Screw.Spec.L != 0.0F)
                        {                            
                            cmbSL_Screw_Spec_L.Text = Convert.ToInt16(mBearing_Radial_FP.SL.Screw.Spec.L).ToString();
                            //int pIndex = cmbSL_Screw_Spec_L.Items.IndexOf(Convert.ToInt16(mBearing_Radial_FP.SL.Screw.Spec.L).ToString());
                            //if (pIndex > -1)
                            //{
                            //    cmbSL_Screw_Spec_L.SelectedIndex = pIndex;
                            //}
                            //else
                            //{
                            //    cmbSL_Screw_Spec_L.Text = "";
                            //    cmbSL_Screw_Spec_L.Text = Convert.ToInt16(mBearing_Radial_FP.SL.Screw.Spec.L).ToString();
                            //}
                        }
                        else if (cmbSL_Screw_Spec_L.Items.Count > 0)
                            cmbSL_Screw_Spec_L.SelectedIndex = 0;

                        //Check_SL_Screw_LLim(mBearing_Radial_FP.SL.Screw.Spec.L, mBearing_Radial_FP.SL.Thread_L_LowerLimit());

                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            txtSL_LScrew_Loc_Center.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.LScrew.Center));
                            txtSL_LScrew_Loc_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.LScrew.Back));

                            txtSL_RScrew_Loc_Center.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.RScrew.Center));
                            txtSL_RScrew_Loc_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.RScrew.Back));

                            //....CBore
                            txtSL_CBore_Depth.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw.Hole.CBore.Depth));
                            txtSL_CBore_Dia.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw.Hole.CBore.D));
                            txtSL_CBore_DDrill.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw.Hole.D_Drill));

                            //....Depth
                            txtSL_Depth_TapDrill.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw.Hole.Depth.TapDrill));
                            txtSL_Depth_Tap.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw.Hole.Depth.Tap));
                            txtSL_Depth_Engagement.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw.Hole.Depth.Engagement));
                        }
                        else
                        {
                            txtSL_LScrew_Loc_Center.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.LScrew.Center);
                            txtSL_LScrew_Loc_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.LScrew.Back);

                            txtSL_RScrew_Loc_Center.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.RScrew.Center);
                            txtSL_RScrew_Loc_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.RScrew.Back);

                            txtSL_CBore_Depth.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Screw.Hole.CBore.Depth);
                            txtSL_CBore_Dia.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Screw.Hole.CBore.D);
                            txtSL_CBore_DDrill.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Screw.Hole.D_Drill);

                            //....Depth
                            txtSL_Depth_TapDrill.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Screw.Hole.Depth.TapDrill);
                            txtSL_Depth_Tap.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Screw.Hole.Depth.Tap);
                            txtSL_Depth_Engagement.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Screw.Hole.Depth.Engagement);
                        }      

                        //....Dowel:
                        //
                        cmbSL_Dowel_Spec_UnitSystem.Text = mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString();
                        double pSLDowel_L = mBearing_Radial_FP.SL.Dowel.Spec.L;
                        string pSL_Dowel_L = mBearing_Radial_FP.SL.Dowel.Spec.L.ToString("#0");
       
                        if (mBearing_Radial_FP.SL.Dowel.Spec.Type != null && mBearing_Radial_FP.SL.Dowel.Spec.Type != "")
                            cmbSL_Dowel_Spec_Type.Text = mBearing_Radial_FP.SL.Dowel.Spec.Type;
                        else if (cmbSL_Dowel_Spec_Type.Items.Count > 0)
                            cmbSL_Dowel_Spec_Type.SelectedIndex = 0;

                        if (mBearing_Radial_FP.SL.Dowel.Spec.Mat != null && mBearing_Radial_FP.SL.Dowel.Spec.Mat != "")
                            cmbSL_Dowel_Spec_Mat.Text = mBearing_Radial_FP.SL.Dowel.Spec.Mat;
                        else if (cmbSL_Dowel_Spec_Mat.Items.Count > 0)
                            cmbSL_Dowel_Spec_Mat.SelectedIndex = 0;

                        if (mBearing_Radial_FP.SL.Dowel.Spec.D_Desig != null && mBearing_Radial_FP.SL.Dowel.Spec.D_Desig != "")
                            cmbSL_Dowel_Spec_D_Desig.Text = mBearing_Radial_FP.SL.Dowel.Spec.D_Desig;
                        else if (cmbSL_Dowel_Spec_D_Desig.Items.Count >0)
                            cmbSL_Dowel_Spec_D_Desig.SelectedIndex = 0;
                                    
                        Update_SL_Dowel_L();

                        if (pSLDowel_L > mcEPS)
                        {
                            Boolean pSrew_DowelL_Exists = false;
                            for (int i = 0; i < cmbSL_Dowel_Spec_L.Items.Count; i++)
                            {
                                if (cmbSL_Dowel_Spec_L.Items[i].ToString() == pSL_Dowel_L)
                                {
                                    pSrew_DowelL_Exists = true;
                                    break;
                                }
                            }
                            if (!pSrew_DowelL_Exists)
                            {
                                cmbSL_Dowel_Spec_L.Items.Add(pSL_Dowel_L);
                            }
                        }

                        if (mBearing_Radial_FP.SL.Dowel.Spec.L != 0.0F)
                            cmbSL_Dowel_Spec_L.Text = Convert.ToInt16(mBearing_Radial_FP.SL.Dowel.Spec.L).ToString();
                            //cmbSL_Dowel_Spec_L.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Dowel.Spec.L);
                        else if (cmbSL_Dowel_Spec_L.Items.Count > 0)
                            cmbSL_Dowel_Spec_L.SelectedIndex = 0;

                        //Check_SL_Dowel_LLim(mBearing_Radial_FP.SL.Dowel.Spec.L, mBearing_Radial_FP.SL.Pin_L_LowerLimit());

                        mBearing_Radial_FP.SL.Dowel.Hole_Depth_Up = pSL_Dowel_HoleDepthUp;
                        mBearing_Radial_FP.SL.Dowel.Hole_Depth_Low = pSL_Dowel_HoleDepthLow;

                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            txtSL_LDowel_Loc_Center.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.LDowel_Loc.Center));
                            txtSL_LDowel_Loc_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.LDowel_Loc.Back));

                            txtSL_RDowel_Loc_Center.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.RDowel_Loc.Center));
                            txtSL_RDowel_Loc_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.RDowel_Loc.Back));
                      
                            //txtSL_Dowel_Depth.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Dowel_Depth));
                            txtSL_Dowel_Depth_Up.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Dowel.Hole.Depth_Up));
                            txtSL_Dowel_Depth_Low.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Dowel.Hole.Depth_Low));
                        }
                        else
                        {
                            txtSL_LDowel_Loc_Center.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.LDowel_Loc.Center);
                            txtSL_LDowel_Loc_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.LDowel_Loc.Back);

                            txtSL_RDowel_Loc_Center.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.RDowel_Loc.Center);
                            txtSL_RDowel_Loc_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.RDowel_Loc.Back);

                            //txtSL_Dowel_Depth.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Dowel_Depth);
                            txtSL_Dowel_Depth_Up.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Dowel.Hole.Depth_Up);
                            txtSL_Dowel_Depth_Low.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Dowel.Hole.Depth_Low);
                        }
                
                    #endregion


                    #region "Tab: Flange:"
                    //  ------
                        chkFlange_Exists.Checked = mBearing_Radial_FP.Flange.Exists;
                        if (mBearing_Radial_FP.Flange.Exists)
                        {
                            txtFlange_D.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Flange.D, "#0.000");
                            txtFlange_Wid.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Flange.Wid, "#0.000");
                            txtFlange_DimStart_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Flange.DimStart_Back, "#0.000");
                        }

                    #endregion
                    
                
                    #region "Tab: Anti Rotation Pin:"
                    //  -----------------------

                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            txtARP_Loc_Dist_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Loc_Back));
                        }
                        else
                        {
                            txtARP_Loc_Dist_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.ARP.Loc_Back);
                        }

                        txtARP_Loc_Angle.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.ARP.Ang_Casing_SL, "");

                        //if (mBearing_Radial_FP.Flange.Exists)
                        //{
                        //    grpInsertedOn.Visible = true;
                        //    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.InsertedOn == clsBearing_Radial_FP.clsAntiRotPin.eInsertedOn.FD)
                        //    {
                        //        optBearingOD.Checked = true;
                        //    }
                        //    else
                        //    {
                        //        optFlange.Checked = true;
                        //    }
                        //}
                        //else
                        //{
                        //    grpInsertedOn.Visible = false;
                        //}
      
                        //cmbAntiRotPin_Loc_CasingSL.Text = mBearing_Radial_FP.ARP.Loc_Casing_SL.ToString();

                        //...ARP Hardware
                        double pARP_Dowel_L = mBearing_Radial_FP.ARP.Dowel.Spec.L;
                        string pstrDowel_L = mBearing_Radial_FP.ARP.Dowel.Spec.L.ToString("#0");

                        

                        double pARP_Hole_Depth = mBearing_Radial_FP.ARP.Dowel.Hole.Depth_Low;
                        double pARP_Loc_Offset = mBearing_Radial_FP.ARP.Offset;

                        cmbARP_Spec_UnitSystem.Text = mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System.ToString();              

                        if (mBearing_Radial_FP.ARP.Dowel.Spec.Type != null && mBearing_Radial_FP.ARP.Dowel.Spec.Type != "")
                            cmbARP_Spec_Type.Text = mBearing_Radial_FP.ARP.Dowel.Spec.Type;
                        else if (cmbARP_Spec_Type.Items.Count > 0)
                            cmbARP_Spec_Type.SelectedIndex = 0;

                        if (mBearing_Radial_FP.ARP.Dowel.Spec.Mat != null && mBearing_Radial_FP.ARP.Dowel.Spec.Mat != "")
                            cmbARP_Spec_Mat.Text = mBearing_Radial_FP.ARP.Dowel.Spec.Mat;
                        else if (cmbARP_Spec_Mat.Items.Count > 0)
                            cmbARP_Spec_Mat.SelectedIndex = 0;

                        if (mBearing_Radial_FP.ARP.Dowel.Spec.D_Desig != null && mBearing_Radial_FP.ARP.Dowel.Spec.D_Desig != "")
                        {
                            int pIndex = cmbARP_Spec_D_Desig.Items.IndexOf(mBearing_Radial_FP.ARP.Dowel.Spec.D_Desig);
                            cmbARP_Spec_D_Desig.SelectedIndex = -1;
                            cmbARP_Spec_D_Desig.SelectedIndex = pIndex;
                            //cmbAntiRotPin_Spec_D_Desig.Text = mBearing_Radial_FP.ARP.Dowel.Spec.D_Desig;
                        }

                        else if (cmbARP_Spec_D_Desig.Items.Count > 0)
                            cmbARP_Spec_D_Desig.SelectedIndex = 0;

                        Boolean pDowel_L_Exists = false;
                        for (int i = 0; i < cmbARP_Spec_L.Items.Count; i++)
                        {
                            if (cmbARP_Spec_L.Items[i].ToString() == pstrDowel_L)
                            {
                                pDowel_L_Exists = true;
                                break;
                            }
                        }
                        if (!pDowel_L_Exists)
                        {
                            cmbARP_Spec_L.Items.Add(pstrDowel_L);
                        }

                        mBearing_Radial_FP.ARP.Dowel.Spec_L = pARP_Dowel_L;

                        if (mBearing_Radial_FP.ARP.Dowel.Spec.L != 0.0F)
                        {
                            //cmbARP_Spec_L.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.ARP.Dowel.Spec.L);
                            cmbARP_Spec_L.Text = mBearing_Radial_FP.ARP.Dowel.Spec.L.ToString("#0");
                        }
                        else if (cmbARP_Spec_L.Items.Count > 0)
                            cmbARP_Spec_L.SelectedIndex = 0;

                        mBearing_Radial_FP.ARP.Dowel.Hole_Depth_Low = pARP_Hole_Depth;

                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            //txtAntiRotPin_Stickout.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Stickout));
                            if (mBearing_Radial_FP.ARP.Dowel.Hole.Depth_Low > modMain.gcEPS)
                            {
                                txtARP_Depth.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Dowel.Hole.Depth_Low));
                            }
                            else
                            {
                                txtARP_Depth.Text = ""; 
                            }
                        }
                        else
                        {
                            txtARP_Depth.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.ARP.Dowel.Hole.Depth_Low);
                            //txtAntiRotPin_Stickout.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.ARP.Stickout);
                            if (mBearing_Radial_FP.ARP.Dowel.Hole.Depth_Low > modMain.gcEPS)
                            {
                                txtARP_Depth.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.ARP.Dowel.Hole.Depth_Low);
                            }
                            else
                            {
                                txtARP_Depth.Text = "";
                            }
                        }

                        mBearing_Radial_FP.ARP.Offset = pARP_Loc_Offset;
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            txtARP_Loc_Offset.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Offset));
                        }
                        else
                        {
                            txtARP_Loc_Offset.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.ARP.Offset);
                        }
                        txtARP_Ang_Horz.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.ARP.Ang_Horz(), "");

                        if (mBearing_Radial_FP.ARP.Offset > modMain.gcEPS)
                        {
                            cmbARP_Loc_CasingSL.SelectedIndex =0;
                            cmbARP_Loc_CasingSL.SelectedIndex = 1;
                            txtARP_Loc_Offset.Visible = true;
                            cmbARP_Loc_Offset_Direction.Visible = true;
                        }
                        else
                        {
                            cmbARP_Loc_CasingSL.SelectedIndex = 1;
                            cmbARP_Loc_CasingSL.SelectedIndex = 0;
                            txtARP_Loc_Offset.Visible = false;
                            cmbARP_Loc_Offset_Direction.Visible = false;
                        }

                        if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Offset_Direction != null)
                        {
                            cmbARP_Loc_Offset_Direction.Text = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Offset_Direction;
                        }

                    #endregion
                                         

                    #region "Tab: Mounting Holes:"
                    //  --------------------
                        //....GoThru'
                        //mBearing_Radial_FP.Mount.Holes_GoThru = false;
                        //chkMount_Holes_GoThru.Checked = false;
                        //SetControls_Mount_Holes_GoThru(chkMount_Holes_GoThru.Checked);

                        //chkMount_Holes_GoThru.Checked = mBearing_Radial_FP.Mount.Holes_GoThru;
                        //SetControls_Mount_Holes_GoThru(mBearing_Radial_FP.Mount.Holes_GoThru);


                        for (int i = 0; i < 2; i++)     
                        {
                            //  FRONT:
                            //  -----
                            //
                            if (chkMountBolting_Front.Checked && i == 0)                       
                            {
                                double pStartAngle = mBearing_Radial_FP.Mount.BC[i].AngStart;

                                //.....DBC.
                                if (mBearing_Radial_FP.Mount.BC[0].D > modMain.gcEPS)
                                {
                                    txtEndConfig_DBC_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing_Radial_FP.Mount.BC[0].D);
                                }
                                else
                                {
                                    Display_Mount_DBC(ref txtEndConfig_DBC_Front);                  
                                }

                                //....Seal OD. 
                                if (mEndPlate[0].OD > modMain.gcEPS)
                                {
                                    txtMount_EndConfig_OD_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mEndPlate[0].OD);
                                }
                                else
                                {
                                    Display_EndPlate_OD(ref txtMount_EndConfig_OD_Front, 0);               
                                }
                                txtMount_WallT_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing_Radial_FP.TWall_CB_EndPlate(i));                
                                cmbMount_HolesCount_Front.Text = modMain.ConvIntToStr(mBearing_Radial_FP.Mount.BC[i].Count);               //....Count.
                                chkMountHoleEquispaced_Front.Checked = mBearing_Radial_FP.Mount.BC[i].EquiSpaced;                        //....EquiSpaced.

                                if (pStartAngle > mcEPS)
                                {
                                    mBearing_Radial_FP.Mount.BC[i].AngStart = pStartAngle;
                                }
                                txtMount_HolesAngStart_Front.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.BC[i].AngStart, "");  //....Angle Start. 
                                Display_AnglesBet(mBearing_Radial_FP, i);                                                                         

                                //....Thread:                       
                                //........Type.
                                if (mBearing_Radial_FP.Mount.Screw[i].Spec.Type != null && mBearing_Radial_FP.Mount.Screw[i].Spec.Type != "")
                                    cmbMount_Screw_Type_Front.Text = mBearing_Radial_FP.Mount.Screw[i].Spec.Type;
                                else if (cmbMount_Screw_Type_Front.Items.Count > 0)
                                    cmbMount_Screw_Type_Front.SelectedIndex = 0;

                                //........Material.
                                if (mBearing_Radial_FP.Mount.Screw[i].Spec.Mat != null && mBearing_Radial_FP.Mount.Screw[i].Spec.Mat != "")
                                    cmbMount_Screw_Mat_Front.Text = mBearing_Radial_FP.Mount.Screw[i].Spec.Mat;
                                else if (cmbMount_Screw_Mat_Front.Items.Count > 0)
                                    cmbMount_Screw_Mat_Front.SelectedIndex = 0;

                                ////Populate_Screw_D_Desig(ref cmbMount_Screw_D_Desig_Front,
                                ////                        mBearing_Radial_FP.Mount.Screw[i].Screw_Spec.Type,
                                ////                        mBearing_Radial_FP.Mount.Screw[i].Screw_Spec.Mat,
                                ////                        mBearing_Radial_FP.SL.Screw_Spec.Unit.System, 
                                ////                        mBearing_Radial_FP.Mount.Screw[i].Screw_Spec.D_Desig);

                                //....L
                                double pMountScrew_L_Front = mBearing_Radial_FP.Mount.Screw[i].Spec.L;
                                string pstrScrew_L_Front = mBearing_Radial_FP.Mount.Screw[i].Spec.L.ToString("#0");

                                //........D_Desig.
                                if (mBearing_Radial_FP.Mount.Screw[i].Spec.D_Desig != null && mBearing_Radial_FP.Mount.Screw[i].Spec.D_Desig != "")
                                    cmbMount_Screw_D_Desig_Front.Text = mBearing_Radial_FP.Mount.Screw[i].Spec.D_Desig;
                                else
                                    if(cmbMount_Screw_D_Desig_Front.Items.Count>0) cmbMount_Screw_D_Desig_Front.SelectedIndex = 0;

                                //........Length.
                                ////if (pMountScrew_L_Front > mcEPS)
                                ////{
                                ////    Boolean pScrew_L_Front_Exists = false;
                                ////    for (i = 0; i < cmbMount_Screw_L_Front.Items.Count; i++)
                                ////    {
                                ////        if (cmbMount_Screw_L_Front.Items[i].ToString() == pstrScrew_L_Front)
                                ////        {
                                ////            pScrew_L_Front_Exists = true;
                                ////            break;
                                ////        }
                                ////    }
                                ////    if (!pScrew_L_Front_Exists)
                                ////    {
                                ////        cmbMount_Screw_L_Front.Items.Add(pstrScrew_L_Front);
                                ////    }

                                ////    mBearing_Radial_FP.Mount.Screw[i].Spec_L = pMountScrew_L_Front;
                                ////}

                                mBearing_Radial_FP.Mount.Screw[i].Spec_L = pMountScrew_L_Front;

                                if (mBearing_Radial_FP.Mount.Screw[i].Spec.L != 0.0F)
                                    cmbMount_Screw_L_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Spec.L);
                                else if (cmbMount_Screw_L_Front.Items.Count > 0)
                                    cmbMount_Screw_L_Front.SelectedIndex = 0;

                                lblMount_Unit_Front.Text = cmbSL_Screw_Spec_UnitSystem.Text;

                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    txtMountHoles_CBore_DDrill_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[i].Hole.D_Drill));
                                    txtMountHoles_CBore_Dia_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[i].Hole.CBore.D));
                                    txtMountHoles_CBore_Depth_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[i].Hole.CBore.Depth));

                                    txtMount_Holes_Depth_TapDrill_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[i].Hole.Depth.TapDrill));
                                    txtMount_Holes_Depth_Tap_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[i].Hole.Depth.Tap));
                                    txtMount_Holes_Depth_Engagement_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[i].Hole.Depth.Engagement));
                                }
                                else
                                {
                                    txtMountHoles_CBore_DDrill_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Hole.D_Drill);
                                    txtMountHoles_CBore_Dia_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Hole.CBore.D);
                                    txtMountHoles_CBore_Depth_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Hole.CBore.Depth);

                                    txtMount_Holes_Depth_TapDrill_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Hole.Depth.TapDrill);
                                    txtMount_Holes_Depth_Tap_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Hole.Depth.Tap);
                                    txtMount_Holes_Depth_Engagement_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Hole.Depth.Engagement);
                                }

                                //lblEndConfig_DBC_LLim_Front.Text = modMain.gProject.Product.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw_Hole_DBC_LLimit(0));
                                //lblEndConfig_DBC_Ulim_Front.Text = modMain.gProject.Product.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw_Hole_DBC_ULimit(0));

                                Double pULim = mMount_DBC_ULimit[0];
                                Double pLLim = mMount_DBC_LLimit[0];
                                Double pMean_Lim =0.5* (pULim + pLLim);
                                Double pDBCFront_Val =modMain.ConvTextToDouble(txtEndConfig_DBC_Front.Text);

                                if (Math.Abs(pDBCFront_Val - pMean_Lim) < mcEPS)
                                {
                                    chkMount_DBC_Front.Checked = true;
                                }
                                else
                                {
                                    chkMount_DBC_Front.Checked = false;
                                }

                            }


                            //....Back: 
                            //
                            if (chkMountBolting_Back.Checked && i == 1)                        
                            {
                                    //.....DBC.
                                    if (mBearing_Radial_FP.Mount.BC[1].D > modMain.gcEPS)
                                    {
                                        txtEndConfig_DBC_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing_Radial_FP.Mount.BC[1].D);
                                    }
                                    else
                                    {
                                        Display_Mount_DBC(ref txtEndConfig_DBC_Back);
                                    }

                                    //....Seal OD. 
                                  
                                    if (mEndPlate[1].OD > modMain.gcEPS)
                                    {
                                        txtMount_EndConfig_OD_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mEndPlate[1].OD);
                                    }
                                    else
                                    {
                                        Display_EndPlate_OD(ref txtMount_EndConfig_OD_Back, 1);
                                    }

                                    txtMount_WallT_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing_Radial_FP.TWall_CB_EndPlate(i));     //....Wall thick
                                    cmbMount_HolesCount_Back.Text = modMain.ConvIntToStr(mBearing_Radial_FP.Mount.BC[i].Count);    //....Count.                                    
                                    chkMountHoleEquispaced_Back.Checked = mBearing_Radial_FP.Mount.BC[i].EquiSpaced;               //....EquiSpaced.                                    
                                    txtMount_HolesAngStart_Back.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.BC[i].AngStart, "");  //....Angle Bet. 
                                    Display_AnglesBet(mBearing_Radial_FP, i);            //....Angle Bet.


                                    //....Thread.                           
                                    //........Type.
                                    if (mBearing_Radial_FP.Mount.Screw[i].Spec.Type != null && mBearing_Radial_FP.Mount.Screw[i].Spec.Type != "")
                                        cmbMount_Screw_Type_Back.Text = mBearing_Radial_FP.Mount.Screw[i].Spec.Type;
                                    else if (cmbMount_Screw_Type_Back.Items.Count > 0)
                                        cmbMount_Screw_Type_Back.SelectedIndex = 0;

                                    //........Material.
                                    if (mBearing_Radial_FP.Mount.Screw[i].Spec.Mat != null && mBearing_Radial_FP.Mount.Screw[i].Spec.Mat != "")
                                        cmbMount_Screw_Mat_Back.Text = mBearing_Radial_FP.Mount.Screw[i].Spec.Mat;
                                    else if (cmbMount_Screw_Mat_Back.Items.Count > 0)
                                        cmbMount_Screw_Mat_Back.SelectedIndex = 0;

                                    ////Populate_Screw_D_Desig(ref cmbMount_Screw_D_Desig_Back,
                                    ////                        mBearing_Radial_FP.Mount.Screw[i].Spec.Type,
                                    ////                        mBearing_Radial_FP.Mount.Screw[i].Spec.Mat,
                                    ////                        mBearing_Radial_FP.SL.Screw.Spec.Unit.System,
                                    ////                        mBearing_Radial_FP.Mount.Screw[i].Spec.D_Desig);


                                    //........D_Desig.
                                    double pMountScrew_L_Back = mBearing_Radial_FP.Mount.Screw[i].Spec.L;
                                    string pstrScrew_L_Back = mBearing_Radial_FP.Mount.Screw[i].Spec.L.ToString("#0");

                                    if (mBearing_Radial_FP.Mount.Screw[i].Spec.D_Desig != null && mBearing_Radial_FP.Mount.Screw[i].Spec.D_Desig != "")
                                        cmbMount_Screw_D_Desig_Back.Text = mBearing_Radial_FP.Mount.Screw[i].Spec.D_Desig;
                                    else
                                    {
                                        cmbMount_Screw_D_Desig_Back.SelectedIndex = 0;
                                    }

                                    //........Length.
                                    ////if (pMountScrew_L_Back > mcEPS)
                                    ////{
                                    ////    Boolean pScrew_L_Back_Exists = false;
                                    ////    for (i = 0; i < cmbMount_Screw_L_Back.Items.Count; i++)
                                    ////    {
                                    ////        if (cmbMount_Screw_L_Back.Items[i].ToString() == pstrScrew_L_Back)
                                    ////        {
                                    ////            pScrew_L_Back_Exists = true;
                                    ////            break;
                                    ////        }
                                    ////    }
                                    ////    if (!pScrew_L_Back_Exists)
                                    ////    {
                                    ////        cmbMount_Screw_L_Back.Items.Add(pstrScrew_L_Back);
                                    ////    }

                                    ////    mBearing_Radial_FP.Mount.Screw[i].Spec_L = pMountScrew_L_Back;
                                    ////}

                                    mBearing_Radial_FP.Mount.Screw[i].Spec_L = pMountScrew_L_Back;

                                    if (mBearing_Radial_FP.Mount.Screw[i].Spec.L != 0.0F)
                                        cmbMount_Screw_L_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Spec.L);
                                    else if (cmbMount_Screw_L_Back.Items.Count > 0)
                                        cmbMount_Screw_L_Back.SelectedIndex = 0;

                                lblMount_Unit_Back.Text = cmbSL_Screw_Spec_UnitSystem.Text;

                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    txtMountHoles_CBore_DDrill_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[i].Hole.D_Drill));
                                    txtMountHoles_CBore_Dia_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[i].Hole.CBore.D));
                                    txtMountHoles_CBore_Depth_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[i].Hole.CBore.Depth));

                                    txtMount_Holes_Depth_TapDrill_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[i].Hole.Depth.TapDrill));
                                    txtMount_Holes_Depth_Tap_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[i].Hole.Depth.Tap));
                                    txtMount_Holes_Depth_Engagement_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[i].Hole.Depth.Engagement));
                                }
                                else
                                {
                                    txtMountHoles_CBore_DDrill_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Hole.D_Drill);
                                    txtMountHoles_CBore_Dia_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Hole.CBore.D);
                                    txtMountHoles_CBore_Depth_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Hole.CBore.Depth);

                                    txtMount_Holes_Depth_TapDrill_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Hole.Depth.TapDrill);
                                    txtMount_Holes_Depth_Tap_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Hole.Depth.Tap);
                                    txtMount_Holes_Depth_Engagement_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[i].Hole.Depth.Engagement);
                                }

                                //lblEndConfig_DBC_LLim_Back.Text = modMain.gProject.Product.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw_Hole_DBC_LLimit(1));
                                //lblEndConfig_DBC_Ulim_Back.Text = modMain.gProject.Product.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw_Hole_DBC_ULimit(1));

                                Double pULim = mMount_DBC_ULimit[1];
                                Double pLLim = mMount_DBC_LLimit[1];
                                Double pMean_Lim = 0.5 * (pULim + pLLim);
                                Double pDBCFront_Val = modMain.ConvTextToDouble(txtEndConfig_DBC_Back.Text);

                                if (Math.Abs(pDBCFront_Val - pMean_Lim) < mcEPS)
                                {
                                    chkMount_DBC_Back.Checked = true;
                                }
                                else
                                {
                                    chkMount_DBC_Back.Checked = false;
                                }
                            }
                        }  

                    #endregion   
                }

            #endregion


            #region "CHECK LENGTH LIMITS:"

                private void Check_Screw_LLims(Double Thread_L, Double Thread_LLower,Double Thread_LUpper, int Indx_In)     
                //=============================================================================================================      
                {
                    Boolean pblnChecked = false;

                    if (Thread_L > Thread_LLower && Thread_L < Thread_LUpper)
                        pblnChecked = true;
                    else
                        pblnChecked = false;

                    switch (Indx_In)
                    {
                        case 0:
                            chkMount_Screw_LenLim_Front.Checked = pblnChecked;
                            break;

                        case 1:
                            chkMount_Screw_LenLim_Back.Checked = pblnChecked;
                            break;
                    }
                }

                private void Check_SL_Screw_LLim(Double Thread_L, Double Thread_LLower)
                //=====================================================================    
                {
                    if (Thread_L != 0.0F)
                    {
                        if (Thread_L >= Thread_LLower)
                            chkSL_Screw_LenLim.Checked = true;
                        else
                            chkSL_Screw_LenLim.Checked = false;
                    }
                    else
                    {
                        chkSL_Screw_LenLim.Checked = true;
                    }
                }

                private void Check_SL_Dowel_LLim(Double Pin_L, Double Pin_LLower)
                //================================================================  
                {
                    if (Pin_L != 0.0F)
                    {
                        if (Pin_L > Pin_LLower)
                            chkSL_Dowel_LenLim.Checked = true;
                        else
                            chkSL_Dowel_LenLim.Checked = false;
                    }
                    else
                        chkSL_Dowel_LenLim.Checked = true;
                }

            #endregion

                    
            #region "ROUTINES RELATED To CALCULATED FIELD:"
            
                private void IsOilInlet_Annulus_D_Calc(ref TextBox TxtBox_In)
                //===========================================================
                {
                    clsBearing_Radial_FP pTempBearing ;
                    
                    pTempBearing =( clsBearing_Radial_FP) mBearing_Radial_FP.Clone();
                    pTempBearing.OilInlet.Annulus_Ratio_Wid_Depth =  mBearing_Radial_FP.OilInlet.Annulus.Ratio_Wid_Depth;
                    pTempBearing.OilInlet.Calc_Annulus_Params();

                    int pRet = 0;
                    if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus.D > modMain.gcEPS)
                    {
                        if (modMain.CompareVar(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus.D,
                                               pTempBearing.OilInlet.Annulus.D, 4, pRet) > 0)
                            TxtBox_In.ForeColor = Color.Black;
                        else
                            TxtBox_In.ForeColor = Color.Blue;
                    }
                    else
                        TxtBox_In.ForeColor = Color.Blue;
                }

                private void Display_Mount_DBC(ref TextBox TxtBox_In)        
                //==========================================================
                {
                    TxtBox_In.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing_Radial_FP.Mount.BC[0].D);
                    //TxtBox_In.ForeColor = Color.Black;
                }

                private void Display_EndPlate_OD(ref TextBox TxtBox_In, int Index_In)  
                //=====================================================================
                {
                    TxtBox_In.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mEndPlate_OD_LLimit[Index_In]);                    
                }

            #endregion


            #region "UTILITY LOAD ROUTINES"
                //=========================

                private void LoadUnit(ComboBox CmbBox_In)
                //=======================================
                {
                    if (CmbBox_In.Items.Count <= 0)
                    {
                        CmbBox_In.Items.Clear();
                        CmbBox_In.Items.Add(clsUnit.eSystem.English.ToString());
                        CmbBox_In.Items.Add(clsUnit.eSystem.Metric.ToString());

                        switch (CmbBox_In.Name)
                        {
                            case "cmbSL_Screw_Spec_UnitSystem":
                                if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString() != "")
                                    CmbBox_In.Text = mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString();
                                else
                                    CmbBox_In.SelectedIndex = 0;
                                break;

                            case "cmbSL_Dowel_Spec_UnitSystem":
                                if (mBearing_Radial_FP.SL.Dowel.Spec.Unit.System.ToString() != "")
                                    CmbBox_In.Text = mBearing_Radial_FP.SL.Dowel.Spec.Unit.System.ToString();
                                else
                                    CmbBox_In.SelectedIndex = 0;
                                break;

                            case "cmbARP_Spec_UnitSystem":
                                if (mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System.ToString() != "")
                                    CmbBox_In.Text = mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System.ToString();
                                else
                                    CmbBox_In.SelectedIndex = 0;
                                break;
                        }
                    }
                }


                #region "OilInlet:"

                    private void Load_OilInlet_cmbOrificeStartPos()
                    //=============================================
                    {
                        cmbOilInlet_Orifice_StartPos.Items.Clear();
                        cmbOilInlet_Orifice_StartPos.Items.Add(clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.On);
                        cmbOilInlet_Orifice_StartPos.Items.Add(clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.Above);
                        cmbOilInlet_Orifice_StartPos.Items.Add(clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.Below);

                        if (mBearing_Radial_FP.OilInlet.Orifice.StartPos == clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.On)
                            cmbOilInlet_Orifice_StartPos.SelectedIndex = 0;
                        else if (mBearing_Radial_FP.OilInlet.Orifice.StartPos == clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.Above)
                            cmbOilInlet_Orifice_StartPos.SelectedIndex = 1;
                        else if (mBearing_Radial_FP.OilInlet.Orifice.StartPos == clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.Below)
                            cmbOilInlet_Orifice_StartPos.SelectedIndex = 2;
                        else
                            cmbOilInlet_Orifice_StartPos.SelectedIndex = 0;
                    }


                    private void Load_OilInlet_Orifice_CBoreDia()
                    //===========================================
                    {
                        int pIndx = 0;
                        cmbOilInlet_Orifice_CBoreDia.Items.Clear();
                        
                         //....EXCEL File: StdToolData
                        string pSelect = "Select D_Desig ";
                        string pWHERE = " WHERE Orifice_CB = 'Y' or Orifice_CB = 'YP'";
                        
                        string pSQL = pSelect + pWHERE;

                        int pDesig_RecCount = modMain.gDB.PopulateCmbBox(cmbOilInlet_Orifice_CBoreDia, modMain.gFiles.FileTitle_EXCEL_StdToolData, "[Drill$]", "D_Desig", pWHERE, true);

                        Double pMin_D_CBore = mBearing_Radial_FP.OilInlet.Calc_Orifice_D_CBore();

                        List<string> pCBoreDia_Val = new List<string>();
                        for (int i = 0; i < cmbOilInlet_Orifice_CBoreDia.Items.Count; i++)
                        {
                            string pVal = cmbOilInlet_Orifice_CBoreDia.Items[i].ToString();
                            Double pNumerator, pDenominator;
                            Double pFinal = 0.0;

                            if (pVal.ToString() != "1")
                            {
                                pVal = pVal.Remove(pVal.Length - 1);
                                pNumerator = Convert.ToInt32(modMain.ExtractPreData(pVal, "/"));
                                pDenominator = Convert.ToInt32(modMain.ExtractPostData(pVal, "/"));
                                pFinal = Convert.ToDouble(pNumerator / pDenominator);
                            }
                            else
                            {
                                pFinal = Convert.ToDouble(pVal);
                            }
                                                       
                            if ( pFinal > pMin_D_CBore)
                            {
                                pCBoreDia_Val.Add(cmbOilInlet_Orifice_CBoreDia.Items[i].ToString());
                            }
                        }

                        cmbOilInlet_Orifice_CBoreDia.Items.Clear();
                        for (int i = 0; i < pCBoreDia_Val.Count; i++)
                        {
                            cmbOilInlet_Orifice_CBoreDia.Items.Add(pCBoreDia_Val[i]);
                        }


                        if (pDesig_RecCount > 0)
                        {
                            if (mBearing_Radial_FP.OilInlet.Orifice.D_CBore != null)
                            {
                                if (cmbOilInlet_Orifice_CBoreDia.Items.Contains(mBearing_Radial_FP.OilInlet.Orifice.D_CBore))
                                {
                                    pIndx = cmbOilInlet_Orifice_CBoreDia.Items.IndexOf(mBearing_Radial_FP.OilInlet.Orifice.D_CBore);
                                    cmbOilInlet_Orifice_CBoreDia.SelectedIndex = pIndx;
                                }
                                else
                                    cmbOilInlet_Orifice_CBoreDia.SelectedIndex = 0;
                            }
                            else
                            {
                                cmbOilInlet_Orifice_CBoreDia.SelectedIndex = 0;
                            }
                        }
                    }
                 

                    //private void Load_OilInlet_Orifice_Dist_Holes()
                    ////=============================================   
                    //{
                    //    if (mBearing_Radial_FP.OilInlet.Orifice_Exists_2ndSet())
                    //    {
                    //        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    //        {
                    //            txtOilInlet_Orifice_Dist_Holes.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.OilInlet.Orifice.Dist_Holes), "#0.000");
                    //        }
                    //        else
                    //        {
                    //            txtOilInlet_Orifice_Dist_Holes.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.OilInlet.Orifice.Dist_Holes, "#0.000");
                    //        }
                    //    }

                    //    else
                    //    {
                    //        txtOilInlet_Orifice_Dist_Holes.Text = modMain.ConvDoubleToStr(0.0F, "#0.000");
                    //    }                     
                    //}

                #endregion


                #region "SL"

                    private void Load_SL_HardWare()
                    //=============================              
                    {
                        //...Screw
                        LoadUnit(cmbSL_Screw_Spec_UnitSystem);
                        Populate_SL_Details(cmbSL_Screw_Spec_Type);

                        //....Dowel                      
                        LoadUnit(cmbSL_Dowel_Spec_UnitSystem);
                        Populate_SL_Details(cmbSL_Dowel_Spec_Type);       
                    }


                    private void Populate_SL_Details(ComboBox CmbBox_In)
                    //==================================================
                    {                     
                        int pIndx = 0;                       
                        String pUnit    = "";                     
                        //BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                        string pstrFIELDS, pstrFROM, pstrSQL, pstrWHERE, pstrORDERBY;
                        OleDbDataReader pobjDR = null;
                        switch (CmbBox_In.Name)
                        {
                            case "cmbSL_Screw_Spec_Type":
                                //-----------------------
                               if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString() != "")
                                      pUnit = mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString().Substring(0, 1);                       
                                  
                                //var pQry_Manf_Screw = (from pRec in pBearingDBEntities.tblManf_Screw where pRec.fldUnit == pUnit orderby pRec.fldType ascending select pRec.fldType).Distinct().ToList();

                                //if (pQry_Manf_Screw.Count() > 0)
                                //{
                                //    for (int i = 0; i < pQry_Manf_Screw.Count; i++)
                                //    {
                                //        CmbBox_In.Items.Add(pQry_Manf_Screw[i]);
                                //    }

                                //    if (CmbBox_In.Items.Contains(mBearing_Radial_FP.SL.Screw_Spec.Type))
                                //    {
                                //        pIndx = CmbBox_In.Items.IndexOf(mBearing_Radial_FP.SL.Screw_Spec.Type);
                                //        CmbBox_In.SelectedIndex = pIndx;
                                //    }
                                //    else
                                //        CmbBox_In.SelectedIndex = 0;
                                //}
                                
                                //....EXCEL File: StdPartsData
                               OleDbConnection pConnection = null;          
                               //pstrFIELDS = "Select Distinct Type";
                               pstrFIELDS = "Select Distinct Type";
                               pstrFROM = " FROM [Screw_Metric$]";
                               //pstrWHERE = " WHERE  Unit = '" + pUnit + "' and Type = 'SHCS' or Type = 'BHCS'";
                               //pstrWHERE = " WHERE  Type = 'SHCS' or Type = 'BHCS'";
                               pstrWHERE = " WHERE  Type = 'SHCS'";
                               //pstrWHERE = " WHERE  Unit = '" + pUnit + "'";
                               pstrORDERBY = " Order by Type ASC";

                               pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE + pstrORDERBY;
                               pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdPartsData, ref pConnection);     

                               CmbBox_In.Items.Clear();

                               while (pobjDR.Read())
                               {
                                   CmbBox_In.Items.Add(pobjDR["Type"].ToString());

                                   if (CmbBox_In.Items.Contains(mBearing_Radial_FP.SL.Screw.Spec.Type))
                                   {
                                       pIndx = CmbBox_In.Items.IndexOf("SHCS");
                                       CmbBox_In.SelectedIndex = pIndx;
                                   }
                                   else
                                       CmbBox_In.SelectedIndex = 0;
                               }

                               pobjDR.Dispose();
                               pConnection.Close();
                         
                               break;

                            case "cmbSL_Dowel_Spec_Type":
                                //-----------------------
                                if (mBearing_Radial_FP.SL.Dowel.Spec.Unit.System.ToString() != "")
                                    pUnit = mBearing_Radial_FP.SL.Dowel.Spec.Unit.System.ToString().Substring(0, 1);

                                //var pQry_Manf_Pin = (from pRec in pBearingDBEntities.tblManf_Pin where pRec.fldUnit == pUnit orderby pRec.fldType ascending select pRec.fldType).Distinct().ToList();
                                //CmbBox_In.Items.Clear();
                                //if (pQry_Manf_Pin.Count() > 0)
                                //{
                                //    for (int i = 0; i < pQry_Manf_Pin.Count; i++)
                                //    {
                                //        CmbBox_In.Items.Add(pQry_Manf_Pin[i]);
                                //    }

                                //    if (CmbBox_In.Items.Contains(mBearing_Radial_FP.SL.Dowel_Spec.Type))
                                //    {
                                //        pIndx = CmbBox_In.Items.IndexOf(mBearing_Radial_FP.SL.Dowel_Spec.Type);
                                //        CmbBox_In.SelectedIndex = pIndx;
                                //    }
                                //    else
                                //        CmbBox_In.SelectedIndex = 0;
                                          
                                //} 
                                //....EXCEL File: StdPartsData
                                OleDbConnection pConnection1 = null;
                                pstrFIELDS = "Select Distinct Type";
                                pstrFROM = " FROM [Pin_Metric$]";
                                //pstrWHERE = " WHERE  Unit = '" + pUnit + "' and Type = 'SHCS' or Type = 'BHCS'";
                                pstrWHERE = " WHERE  Type = 'P'";
                                //pstrWHERE = " WHERE  Unit = '" + pUnit + "'";
                                pstrORDERBY = " Order by Type ASC";

                                pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE + pstrORDERBY;

                                pobjDR = null;
                                pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdPartsData, ref pConnection1);     

                                CmbBox_In.Items.Clear();
                                while (pobjDR.Read())
                                {
                                    CmbBox_In.Items.Add(pobjDR["Type"].ToString());

                                    if (CmbBox_In.Items.Contains(mBearing_Radial_FP.SL.Dowel.Spec.Type))
                                    {
                                        pIndx = CmbBox_In.Items.IndexOf(mBearing_Radial_FP.SL.Dowel.Spec.Type);
                                        CmbBox_In.SelectedIndex = pIndx;
                                    }
                                    else
                                        CmbBox_In.SelectedIndex = 0;
                                }
                                pobjDR.Dispose();
                                pConnection1.Close();
                                break;
                        }
                    }
               
                #endregion


                #region "ARP:"

                    private void Load_ARP()
                    //============================              
                    {
                        Load_ARP_Loc_CasingSL();     
                        LoadUnit(cmbARP_Spec_UnitSystem);
                        Populate_ARP_Spec_Details();                       
                    }


                    private void Populate_ARP_Spec_Details()
                    //===============================================
                    {
                        ////BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                        ////String pUnit = "";
                        ////int pIndx = 0;
                        ////if (mBearing_Radial_FP.AntiRotPin.Spec.Unit.System.ToString() != "")
                        ////    pUnit = mBearing_Radial_FP.AntiRotPin.Spec.Unit.System.ToString().Substring(0, 1);

                        ////var pQry_Manf_Pin = (from pRec in pBearingDBEntities.tblManf_Pin
                        ////                     where pRec.fldUnit == pUnit
                        ////                     orderby
                        ////                         pRec.fldType ascending
                        ////                     select pRec.fldType).Distinct().ToList();
                        ////cmbAntiRotPin_Spec_Type.Items.Clear();

                        ////if (pQry_Manf_Pin.Count() > 0)
                        ////{
                        ////    for (int i = 0; i < pQry_Manf_Pin.Count; i++)
                        ////    {
                        ////        cmbAntiRotPin_Spec_Type.Items.Add(pQry_Manf_Pin[i]);
                        ////    }

                        ////    if (cmbAntiRotPin_Spec_Type.Items.Contains(mBearing_Radial_FP.AntiRotPin.Spec.Type))
                        ////    {
                        ////        pIndx = cmbAntiRotPin_Spec_Type.Items.IndexOf(mBearing_Radial_FP.AntiRotPin.Spec.Type);
                        ////        cmbAntiRotPin_Spec_Type.SelectedIndex = pIndx;
                        ////    }
                        ////    else
                        ////        cmbAntiRotPin_Spec_Type.SelectedIndex = 0;
                        ////}
                       
                        String pUnit = "";
                        if (mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System.ToString() != "")
                        {
                            pUnit = mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System.ToString().Substring(0, 1);                           
                        }

                        int pIndx = 0;
                        cmbARP_Spec_Type.Items.Clear();

                        //....EXCEL File: StdPartsData
                        string pSelect = "Select Type ";
                        //string pWHERE = " WHERE Unit = '" + pUnit + "' and Type = 'P'";
                        string pWHERE = " WHERE Type = 'P'";
                        string pORDERBY = " Order by Type";
                        string pSQL = pSelect + pWHERE + pORDERBY;

                        int pType_RecCount = modMain.gDB.PopulateCmbBox(cmbARP_Spec_Type, modMain.gFiles.FileTitle_EXCEL_StdPartsData, "[Pin_Metric$]", "Type", pWHERE, true);

                        if (pType_RecCount > 0)
                        {
                            if (cmbARP_Spec_Type.Items.Contains(mBearing_Radial_FP.ARP.Dowel.Spec.Type))
                            {
                                pIndx = cmbARP_Spec_Type.Items.IndexOf(mBearing_Radial_FP.ARP.Dowel.Spec.Type);
                                cmbARP_Spec_Type.SelectedIndex = pIndx;
                            }
                            else
                                cmbARP_Spec_Type.SelectedIndex = 0;
                        }
                    }

                    private void Load_ARP_Loc_CasingSL()
                    //==================================
                    {
                        cmbARP_Loc_CasingSL.Items.Clear();
                        cmbARP_Loc_CasingSL.Items.Add("Center");
                        cmbARP_Loc_CasingSL.Items.Add("Offset");

                        cmbARP_Loc_CasingSL.SelectedIndex = 1;
                    }
                  
                #endregion


                #region "Mounting:"

                    private void Load_MountingDetails(string MountBolting_In)                
                    //=====================================================       
                    {
                        //Mounting Holes:
                        //---------------
                        switch (MountBolting_In)
                        {
                            case "Front":
                            //----------
                                if (mBearing_Radial_FP.Mount.Screw[0].Spec.Type == ""
                                    || cmbMount_Screw_Type_Front.Items.Count == 0)
                                {
                                    cmbMount_Screw_Type_Front.Items.Clear();
                                    cmbMount_Screw_Type_Front.Items.Add("SHCS");
                                    //cmbMount_Screw_Type_Front.Items.Add("BHCS");
                                    cmbMount_Screw_Type_Front.SelectedIndex = 0;
                                    
                                    cmbMount_Screw_Mat_Front.Items.Clear();
                                    cmbMount_Screw_Mat_Front.Items.Add("STEEL");
                                    cmbMount_Screw_Mat_Front.SelectedIndex = 0;
                                }

                                cmbMount_HolesCount_Front.Items.Clear();
                                cmbMount_HolesCount_Front.Items.Add("4");
                                cmbMount_HolesCount_Front.Items.Add("6");
                                cmbMount_HolesCount_Front.Items.Add("8");
                                cmbMount_HolesCount_Front.SelectedIndex = 0;

                                break;

                            case "Back":
                            //----------
                                if (mBearing_Radial_FP.Mount.Screw[1].Spec.Type == ""
                                   || cmbMount_Screw_Type_Back.Items.Count == 0)
                                {
                                    cmbMount_Screw_Type_Back.Items.Clear();
                                    cmbMount_Screw_Type_Back.Items.Add("SHCS");
                                    //cmbMount_Screw_Type_Back.Items.Add("BHCS");
                                    cmbMount_Screw_Type_Back.SelectedIndex = 0;

                                    cmbMount_Screw_Mat_Back.Items.Clear();
                                    cmbMount_Screw_Mat_Back.Items.Add("STEEL");
                                    cmbMount_Screw_Mat_Back.SelectedIndex = 0;
                                }

                                cmbMount_HolesCount_Back.Items.Clear();
                                cmbMount_HolesCount_Back.Items.Add("4");
                                cmbMount_HolesCount_Back.Items.Add("6");
                                cmbMount_HolesCount_Back.Items.Add("8");
                                cmbMount_HolesCount_Back.SelectedIndex = 0;
                            break;
                        }
                    }

                #endregion


                //#region "TempSensor:"

                //    private void Load_TempSensor_Count()
                //    //=================================
                //    {
                //        cmbTempSensor_Count.Items.Clear();
                //        if (mBearing_Radial_FP.Pad.Count != 0)
                //        {
                //            for (int i = 0; i < mBearing_Radial_FP.Pad.Count; i++)
                //            {
                //                cmbTempSensor_Count.Items.Add(i + 1);  
                //            }

                //            //if (modMain.gProject.Product.Accessories.TempSensor.Count > 0)
                //            //    cmbTempSensor_Count.Text = modMain.gProject.Product.Accessories.TempSensor.Count.ToString();
                //            //else if (mBearing_Radial_FP.TempSensor.Count > 0)
                //            //    cmbTempSensor_Count.Text = mBearing_Radial_FP.TempSensor.Count.ToString();
                //            //else
                //            //    cmbTempSensor_Count.SelectedIndex = 0;

                //            if (mBearing_Radial_FP.TempSensor.Count > 0)
                //                cmbTempSensor_Count.Text = mBearing_Radial_FP.TempSensor.Count.ToString();
                //            else
                //                cmbTempSensor_Count.SelectedIndex = 0;
                //        }
                //    }


                //    private void Load_TempSensor_Loc()
                //    //===============================
                //    {                       
                //        if (modMain.gProject.Product.EndConfig[0].Type == clsEndPlate.eType.Seal &&
                //            modMain.gProject.Product.EndConfig[1].Type == clsEndPlate.eType.Seal)
                //        {
                //            cmbTempSensor_Loc.Items.Clear();
                //            cmbTempSensor_Loc.Items.Add(clsBearing_Radial_FP.eBolting.Front);
                //            cmbTempSensor_Loc.Items.Add(clsBearing_Radial_FP.eBolting.Back);
                //        }

                //        else if (modMain.gProject.Product.EndConfig[0].Type == clsEndPlate.eType.Seal)
                //        {
                //            cmbTempSensor_Loc.Items.Clear();
                //            cmbTempSensor_Loc.Items.Add(clsBearing_Radial_FP.eBolting.Front);
                //        }

                //        else if (modMain.gProject.Product.EndConfig[1].Type == clsEndPlate.eType.Seal)
                //        {
                //            cmbTempSensor_Loc.Items.Clear();
                //            cmbTempSensor_Loc.Items.Add(clsBearing_Radial_FP.eBolting.Back);
                //        }
                   

                //        if(cmbTempSensor_Loc.Items.Count == 2 )
                //        {
                //            if (mBearing_Radial_FP.TempSensor.Loc == clsBearing_Radial_FP.eBolting.Front)
                //                cmbTempSensor_Loc.SelectedIndex = 0;

                //            else if (mBearing_Radial_FP.TempSensor.Loc == clsBearing_Radial_FP.eBolting.Back)
                //                cmbTempSensor_Loc.SelectedIndex = 1;

                //            else
                //                cmbTempSensor_Loc.SelectedIndex = 0;
                //        }

                //        else if (cmbTempSensor_Loc.Items.Count == 1)
                //            cmbTempSensor_Loc.SelectedIndex = 0;   
                //    }

                //#endregion

            #endregion

        #endregion


        #region "FORM CONTROLS EVENT ROUTINES:"
        //************************************     

            #region "CHECKBOX RELATED ROUTINES:"
            //---------------------------------   

                private void ChkBox_CheckedChanged(object sender, EventArgs e)  
                //============================================================
                {
                    int pIndx = 0;
                    CheckBox pChkBox = (CheckBox)sender;

                    TabPage [] pTabPageMountingHoles = new TabPage[]{ tabFront, tabBack};
                   
                    switch (pChkBox.Name)
                    {
                        // Tab: Oil Inlet.
                        // ----------
                        case "chkOilInlet_Annulus_Exists":
                            mBearing_Radial_FP.OilInlet.Annulus_Exists = chkOilInlet_Annulus_Exists.Checked;
                            grpOilInlet_Annulus.Enabled = chkOilInlet_Annulus_Exists.Checked;
                            break;

                        //  Tab: Web Relief.
                        //  ----------
                        case "chkMillRelief_Exists":
                            //....Update Related Web Mill Relief.                          
                            mBearing_Radial_FP.MillRelief.Exists = chkMillRelief_Exists.Checked;
                            SetControl_MillRelief();
                            Populate_MillRelief_D_Desig();

                            //....Update related D_PadRelief.
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                txtMillRelief_D_PadRelief.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.MillRelief.D_PadRelief()), "#0.000");
                            }
                            else
                            {
                                txtMillRelief_D_PadRelief.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.MillRelief.D_PadRelief(), "#0.000");
                            }
                            break;

                        //  Split Line Hardware.
                        //  -------------------
                        ////case "chkSL_Screw_LenLim":
                        ////    Populate_SL_Screw_L();
                        ////    break;

                        ////case "chkSL_Dowel_LenLim":
                        ////    Populate_SL_Dowel_L();
                        ////    break;

                        // Tab: Flange.
                        // -------
                        case "chkFlange_Exists":
                            SetControls_Flange();
                            break;

                        // Tab: Mounting       
                        // ----------
                        case "chkMountBolting_Front":                    
                            //-------------------------------
                            chkMountBolting_Front.Checked = true;
                            break;

                        case "chkMountBolting_Back":
                            //------------------------------
                            chkMountBolting_Back.Checked = true;
                            break;

                        case "chkMountHoleEquispaced_Front":             
                            //-------------------------------------
                            mBearing_Radial_FP.Mount.BC[0].EquiSpaced = chkMountHoleEquispaced_Front.Checked;
                            Display_AnglesBet(mBearing_Radial_FP, 0);              
                            break;

                        case "chkMountHoleEquispaced_Back":
                            //-------------------------------------
                            mBearing_Radial_FP.Mount.BC[1].EquiSpaced = chkMountHoleEquispaced_Back.Checked;
                            Display_AnglesBet(mBearing_Radial_FP, 1);
                            break;

                        case "chkMount_Screw_LenLim_Front":
                            //----------------------------------
                            //Populate_Mount_Fixture_Thread_L(0);
                            break;

                        case "chkMount_Screw_LenLim_Back":
                            //----------------------------------
                            //Populate_Mount_Fixture_Thread_L(1);
                            break;
                    }
                }

            #endregion


            #region"COMMAND BUTTON EVENT ROUTINE:"
            //------------------------------------  

                private void cmdPrint_Click(object sender, EventArgs e)    
                //======================================================
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(modMain.printDocument1_PrintPage);

                    modMain.CaptureScreen(this);
                    pd.Print();
                }
     
                private void cmdOK_Click(object sender, EventArgs e)
                //===================================================
                {
                    CloseForm(); }

                private void cmdCancel_Click(object sender, EventArgs e)
                //======================================================
                {                    
                    this.Hide(); }  

                private void CloseForm()
                //======================
                {   
                    SaveData();

                    //....Local Object.
                    SetLocalObject();

                    if (!mblnMount_Front_Copy)
                    {
                        mBearing_Radial_FP.Mount.Screw[1] = (clsScrew)((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Clone();                       
                        mBearing_Radial_FP.Mount.EndPlateOD[1] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.EndPlateOD[0];
                        mBearing_Radial_FP.Mount.BC[0] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0];
                        mBearing_Radial_FP.Mount.BC[1] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0];
                        mEndPlate[1].OD = mBearing_Radial_FP.Mount.EndPlateOD[1];
                        DisplayData();
                    }

                    SaveData();

                    this.Hide();

                    if (modMain.gProject != null)
                    {
                        if (modMain.gblnSealDesignDetails)
                        {
                            modMain.gfrmSealDesignDetails.ShowDialog();
                        }
                        else
                        {
                            modMain.gfrmCreateDataSet.ShowDialog();
                        }
                        //modMain.gfrmCreateDataSet.ShowDialog();
                    }

                    ////if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                    ////{                        
                    ////        modMain.gfrmSealDesignDetails.ShowDialog();
                        
                    ////}
                    ////else if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB)
                    ////{
                    ////    modMain.gfrmThrustBearingDesignDetails.ShowDialog();
                    ////}
                }

                private void SaveData()
                //=====================
                {
                    //....Header
                    //  Length
                    //  -------
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtL.Text));
                    }
                    else
                    {
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).L = modMain.ConvTextToDouble(txtL.Text);
                    }

                    //  Depth End Plates
                    //  -----------------
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Depth_EndPlate[0] =
                                                                    modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text));
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Depth_EndPlate[1] =
                                                                    modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text));
                    }
                    else
                    {
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Depth_EndPlate[0] =
                                                                    modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text);
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Depth_EndPlate[1] =
                                                                    modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text);
                    }

                    //  Length End Plates
                    //  ------------------
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        modMain.gProject.Product.EndPlate[0].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLength_EndConfig_Front.Text));
                        modMain.gProject.Product.EndPlate[1].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLength_EndConfig_Back.Text));                       
                    }
                    else
                    {
                        modMain.gProject.Product.EndPlate[0].L = modMain.ConvTextToDouble(txtLength_EndConfig_Front.Text);
                        modMain.gProject.Product.EndPlate[1].L = modMain.ConvTextToDouble(txtLength_EndConfig_Back.Text); 
                    }
                    

                    #region "Tab: Oil Inlet:"
                    
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Count_MainOilSupply = 
                                                                        modMain.ConvTextToInt(cmbOilInlet_Count_MainOilSupply.Text);
                    
                        if (cmbOilInlet_Orifice_StartPos.Text != "")
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_StartPos =
                                                                        (clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos)
                                                                        Enum.Parse(typeof(clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos), 
                                                                        cmbOilInlet_Orifice_StartPos.Text);

                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_Loc_Back =
                                                                            modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Orifice_Loc_BackFace.Text));
                        }
                        else
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_Loc_Back =
                                                                            modMain.ConvTextToDouble(txtOilInlet_Orifice_Loc_BackFace.Text);
                        }

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_Ratio_L_D = modMain.ConvTextToDouble(cmbOilInlet_Orifice_LD.Text);

                        //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //{

                        //    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_D_CBore = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(lblOilInlet_Orifice_DDrill_CBore.Text));
                        //}
                        string pVal = cmbOilInlet_Orifice_CBoreDia.Text;
                        Double pNumerator, pDenominator;
                        Double pFinal = 0.0;

                        if (pVal.ToString() != "1")
                        {
                            pVal = pVal.Remove(pVal.Length - 1);
                            pNumerator = Convert.ToInt32(modMain.ExtractPreData(pVal, "/"));
                            pDenominator = Convert.ToInt32(modMain.ExtractPostData(pVal, "/"));
                            pFinal = Convert.ToDouble(pNumerator / pDenominator);
                        }
                        else
                        {
                            pFinal = Convert.ToDouble(pVal);
                        }
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_D_CBore = pFinal;


                        //  Annulus
                        //  -------
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Exists = chkOilInlet_Annulus_Exists.Checked;

                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            if (chkOilInlet_Annulus_Exists.Checked)
                            {
                                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Ratio_W_D =
                                //                                            modMain.ConvTextToDouble(cmbOilInlet_Annulus_Ratio_L_H.Text);

                                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_D =
                                //                                            modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Annulus_D.Text));

                                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Wid =
                                //                                            modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(txtOilInlet_Annulus_L.Text));

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Wid =
                                                                           modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Annulus_Wid.Text));

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Depth =
                                                                          modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Annulus_Depth.Text));

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_D =
                                                                          modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Annulus_Dia.Text));

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Loc_Back =
                                                                             modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Annulus_Loc_Back.Text));
                            }

                            //if (mBearing_Radial_FP.OilInlet.Orifice_Exists_2ndSet())
                            //    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_Dist_Holes =
                            //                                                 modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Orifice_Dist_Holes.Text));
                        }
                        else
                        {
                            if (chkOilInlet_Annulus_Exists.Checked)
                            {
                                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Ratio_W_D =
                                //                                            modMain.ConvTextToDouble(cmbOilInlet_Annulus_Ratio_L_H.Text);

                                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_D =
                                //                                            modMain.ConvTextToDouble(txtOilInlet_Annulus_D.Text);

                                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Wid =
                                //                                            modMain.ConvTextToDouble(txtOilInlet_Annulus_L.Text);

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Wid =
                                                                           modMain.ConvTextToDouble(txtOilInlet_Annulus_Wid.Text);

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Depth =
                                                                          modMain.ConvTextToDouble(txtOilInlet_Annulus_Depth.Text);

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_D =
                                                                          modMain.ConvTextToDouble(txtOilInlet_Annulus_Dia.Text);

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus_Loc_Back =
                                                                            modMain.ConvTextToDouble(txtOilInlet_Annulus_Loc_Back.Text);
                            }

                            //if (mBearing_Radial_FP.OilInlet.Orifice_Exists_2ndSet())
                            //    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice_Dist_Holes =
                                                                            //modMain.ConvTextToDouble(txtOilInlet_Orifice_Dist_Holes.Text);
                        }

                    #endregion


                    #region  "Tab: WebRelief"
                    //-----------------
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).MillRelief.Exists = chkMillRelief_Exists.Checked;
                        if (mBearing_Radial_FP.MillRelief.Exists)
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).MillRelief.D_Desig = cmbMillRelief_D_Desig.Text;   
                        else
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).MillRelief.D_Desig = "";

                        //....Axial Seal Gap
                        for (int i = 0; i < 2; i++)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).MillRelief.AxialSealGap[i] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(mtxtAxialSealGap[0].Text));
                            }
                            else
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).MillRelief.AxialSealGap[i] = modMain.ConvTextToDouble(mtxtAxialSealGap[0].Text);
                            }
                        }
                    
                    #endregion

                    
                    #region "Tab: S/L HardWare"
                    //--------------------
                        //.....Thread.
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Spec.Unit.System = 
                                                        (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbSL_Screw_Spec_UnitSystem.Text);            

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Spec_Type = cmbSL_Screw_Spec_Type.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Spec_D_Desig = cmbSL_Screw_Spec_D_Desig.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Spec_Pitch = modMain.ConvTextToDouble(cmbSL_Screw_Spec_Pitch.Text);
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Spec_L = modMain.ConvTextToDouble(cmbSL_Screw_Spec_L.Text);
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Spec_Mat = cmbSL_Screw_Spec_Mat.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.PN = txtSL_Screw_Spec_PN.Text;

                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LScrew_Center =
                                                                            modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_LScrew_Loc_Center.Text));

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LScrew_Back =
                                                                            modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_LScrew_Loc_Back.Text));

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RScrew_Center =
                                                                            modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_RScrew_Loc_Center.Text));

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RScrew_Back =
                                                                            modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_RScrew_Loc_Back.Text));

                            //....CBore
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Hole_CBore_Depth = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_CBore_Depth.Text));
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Hole_CBore_D = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_CBore_Dia.Text));
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Hole_D_Drill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_CBore_DDrill.Text));

                          
                            //....Depth
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Hole_Depth_TapDrill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_Depth_TapDrill.Text));
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Hole_Depth_Tap = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_Depth_Tap.Text));
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Hole_Depth_Engagement = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_Depth_Engagement.Text));
                        }
                        else
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LScrew_Center =
                                                                            modMain.ConvTextToDouble(txtSL_LScrew_Loc_Center.Text);

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LScrew_Back =
                                                                            modMain.ConvTextToDouble(txtSL_LScrew_Loc_Back.Text);

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RScrew_Center =
                                                                            modMain.ConvTextToDouble(txtSL_RScrew_Loc_Center.Text);

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RScrew_Back =
                                                                            modMain.ConvTextToDouble(txtSL_RScrew_Loc_Back.Text);
                            //....CBore
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Hole_CBore_Depth = modMain.ConvTextToDouble(txtSL_CBore_Depth.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Hole_CBore_D = modMain.ConvTextToDouble(txtSL_CBore_Dia.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Hole_D_Drill = modMain.ConvTextToDouble(txtSL_CBore_DDrill.Text);

                            //....Depth
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Hole_Depth_TapDrill = modMain.ConvTextToDouble(txtSL_Depth_TapDrill.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Hole_Depth_Tap = modMain.ConvTextToDouble(txtSL_Depth_Tap.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Hole_Depth_Engagement = modMain.ConvTextToDouble(txtSL_Depth_Engagement.Text);
                        }

                        //.....Dowel.
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel.Spec.Unit.System =
                                                           (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbSL_Dowel_Spec_UnitSystem.Text);             

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel.Spec_Type = cmbSL_Dowel_Spec_Type.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel.Spec_D_Desig = cmbSL_Dowel_Spec_D_Desig.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel.Spec_L = modMain.ConvTextToDouble(cmbSL_Dowel_Spec_L.Text);
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel.Spec_Mat = cmbSL_Dowel_Spec_Mat.Text;
                       ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel.PN = txtSL_Dowel_Spec_PN.Text;

                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LDowel_Loc_Center =
                                                                            modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_LDowel_Loc_Center.Text));

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LDowel_Loc_Back =
                                                                            modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_LDowel_Loc_Back.Text));

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RDowel_Loc_Center =
                                                                            modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_RDowel_Loc_Center.Text));

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RDowel_Loc_Back =
                                                                            modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_RDowel_Loc_Back.Text));


                            //....Depth
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel.Hole_Depth_Up = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_Dowel_Depth_Up.Text));
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel.Hole_Depth_Low = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_Dowel_Depth_Low.Text));
                          
                        }
                        else
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LDowel_Loc_Center =
                                                                            modMain.ConvTextToDouble(txtSL_LDowel_Loc_Center.Text);

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LDowel_Loc_Back =
                                                                            modMain.ConvTextToDouble(txtSL_LDowel_Loc_Back.Text);

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RDowel_Loc_Center =
                                                                            modMain.ConvTextToDouble(txtSL_RDowel_Loc_Center.Text);

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RDowel_Loc_Back =
                                                                            modMain.ConvTextToDouble(txtSL_RDowel_Loc_Back.Text);

                            //....Depth
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel.Hole_Depth_Up = modMain.ConvTextToDouble(txtSL_Dowel_Depth_Up.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel.Hole_Depth_Low = modMain.ConvTextToDouble(txtSL_Dowel_Depth_Low.Text);
                        }

                    #endregion


                    #region  "Tab: Flange"
                    //--------------
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Flange.Exists = chkFlange_Exists.Checked;

                        if(chkFlange_Exists.Checked)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Flange.D =  modMain.ConvTextToDouble(txtFlange_D.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Flange.Wid =  modMain.ConvTextToDouble(txtFlange_Wid.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Flange.DimStart_Back=  modMain.ConvTextToDouble(txtFlange_DimStart_Back.Text);
                        }
                    #endregion


                    #region  "Tab: ARP"
                    //------------------------- 
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Loc_Back = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtARP_Loc_Dist_Front.Text));
                        }
                        else
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Loc_Back = modMain.ConvTextToDouble(txtARP_Loc_Dist_Front.Text);
                        }

                        if (cmbARP_Loc_CasingSL.Text == "Center")
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Offset = 0.0;
                        }
                        else
                        {

                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Offset =
                                                                modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtARP_Loc_Offset.Text));
                            }
                            else
                            {
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Offset =
                                                                modMain.ConvTextToDouble(txtARP_Loc_Offset.Text);
                            }

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Offset_Direction = cmbARP_Loc_Offset_Direction.Text;
                        }
                      

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Ang_Casing_SL = modMain.ConvTextToDouble(txtARP_Loc_Angle.Text);
            
                        if(cmbARP_Spec_UnitSystem.Text!="")
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Dowel.Spec.Unit.System =
                                                        (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbARP_Spec_UnitSystem.Text);          

                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Dowel.Spec_Type = cmbARP_Spec_Type.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Dowel.Spec_D_Desig = cmbARP_Spec_D_Desig.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Dowel.Spec_L = modMain.ConvTextToDouble(cmbARP_Spec_L.Text);
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Dowel.Spec_Mat = cmbARP_Spec_Mat.Text;
                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Dowel.PN = txtARP_Spec_PN.Text;

                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Dowel.Hole_Depth_Low = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtARP_Depth.Text));
                            //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Stickout = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtAntiRotPin_Stickout.Text));
                        }
                        else
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Dowel.Hole_Depth_Low = modMain.ConvTextToDouble(txtARP_Depth.Text);
                            //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Stickout = modMain.ConvTextToDouble(txtAntiRotPin_Stickout.Text);
                        }

                        

                    #endregion


                    #region "Tab: Mounting:"
                    //  --------------
                        if (chkMountBolting_Front.Checked)               
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting = clsBearing_Radial_FP.eBolting.Front;

                        else if (chkMountBolting_Back.Checked)
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting = clsBearing_Radial_FP.eBolting.Back;
                        else
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting = clsBearing_Radial_FP.eBolting.Both;
                        }

                        //....Front Bolting:
                        //
                        if (chkMountBolting_Front.Checked)
                        {
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].D = modMain.ConvTextToDouble(txtEndConfig_DBC_Front.Text);
                            modMain.gProject.Product.EndPlate[0].OD = modMain.ConvTextToDouble(txtMount_EndConfig_OD_Front.Text);

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].Count = modMain.ConvTextToInt(cmbMount_HolesCount_Front.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].EquiSpaced = chkMountHoleEquispaced_Front.Checked;
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngStart = modMain.ConvTextToDouble(txtMount_HolesAngStart_Front.Text);

                            Double[] pMount_HolesAngBet = new Double[mBearing_Radial_FP.COUNT_MOUNT_HOLES_ANG_OTHER_MAX];

                            //if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].EquiSpaced)
                                for (int i = 0; i < ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].Count - 1; i++)
                                    pMount_HolesAngBet[i] = modMain.ConvTextToDouble(mTxtMount_HolesAngBet_Front[i].Text);

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet = pMount_HolesAngBet;

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Spec_D_Desig = cmbMount_Screw_D_Desig_Front.Text;
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Spec_Pitch = modMain.ConvTextToDouble(cmbMount_Screw_Pitch_Front.Text);
                          

                            //....Screw
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Spec_Type = cmbMount_Screw_Type_Front.Text;
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Spec_L = modMain.ConvTextToDouble(cmbMount_Screw_L_Front.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Spec_Mat = cmbMount_Screw_Mat_Front.Text;

                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Thread_Depth[0] =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(txtMount_Holes_Depth_TapDrill_Front.Text));

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole_D_Drill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_DDrill_Front.Text));
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole_CBore_D = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_Dia_Front.Text));
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole_CBore_Depth = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Front.Text));

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole_Depth_TapDrill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMount_Holes_Depth_TapDrill_Front.Text));
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole_Depth_Tap = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMount_Holes_Depth_Tap_Front.Text));
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole_Depth_Engagement = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMount_Holes_Depth_Engagement_Front.Text));

                            }
                            else
                            {
                                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Thread_Depth[0] = modMain.ConvTextToDouble(txtMount_Holes_Depth_TapDrill_Front.Text);

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole_D_Drill = modMain.ConvTextToDouble(txtMountHoles_CBore_DDrill_Front.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole_CBore_D = modMain.ConvTextToDouble(txtMountHoles_CBore_Dia_Front.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole_CBore_Depth = modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Front.Text);

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole_Depth_TapDrill = modMain.ConvTextToDouble(txtMount_Holes_Depth_TapDrill_Front.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole_Depth_Tap = modMain.ConvTextToDouble(txtMount_Holes_Depth_Tap_Front.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole_Depth_Engagement = modMain.ConvTextToDouble(txtMount_Holes_Depth_Engagement_Front.Text);
                            }
                        }

                        //....Back Bolting:
                        //
                        if (chkMountBolting_Back.Checked)
                        {

                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].D = modMain.ConvTextToDouble(txtEndConfig_DBC_Back.Text);
                            modMain.gProject.Product.EndPlate[1].OD = modMain.ConvTextToDouble(txtMount_EndConfig_OD_Back.Text);

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].Count = modMain.ConvTextToInt(cmbMount_HolesCount_Back.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].EquiSpaced = chkMountHoleEquispaced_Back.Checked;
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngStart = modMain.ConvTextToDouble(txtMount_HolesAngStart_Back.Text);

                                Double[] pMount_HolesAngBet = new Double[mBearing_Radial_FP.COUNT_MOUNT_HOLES_ANG_OTHER_MAX];//SG 21AUG12

                                //if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].EquiSpaced)
                                    for (int i = 0; i < ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].Count - 1; i++)
                                        pMount_HolesAngBet[i] = modMain.ConvTextToDouble(mTxtMount_HolesAngBet_Front[i].Text);

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet = pMount_HolesAngBet;

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Spec_D_Desig = cmbMount_Screw_D_Desig_Back.Text;
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Spec_Pitch = modMain.ConvTextToDouble(cmbMount_Screw_Pitch_Back.Text);
                            ////}

                            //....Thread
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Spec_Type = cmbMount_Screw_Type_Back.Text;
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Spec_L = modMain.ConvTextToDouble(cmbMount_Screw_L_Back.Text);
                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Spec_Mat = cmbMount_Screw_Mat_Back.Text;

                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Thread_Depth[0] =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(txtMount_Holes_Depth_TapDrill_Front.Text));

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole_D_Drill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_DDrill_Back.Text));
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole_CBore_D = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_Dia_Back.Text));
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole_CBore_Depth = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Back.Text));

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole_Depth_TapDrill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMount_Holes_Depth_TapDrill_Back.Text));
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole_Depth_Tap = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMount_Holes_Depth_Tap_Back.Text));
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole_Depth_Engagement = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMount_Holes_Depth_Engagement_Back.Text));

                            }
                            else
                            {
                                //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Thread_Depth[0] = modMain.ConvTextToDouble(txtMount_Holes_Depth_TapDrill_Front.Text);

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole_D_Drill = modMain.ConvTextToDouble(txtMountHoles_CBore_DDrill_Back.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole_CBore_D = modMain.ConvTextToDouble(txtMountHoles_CBore_Dia_Back.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole_CBore_Depth = modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Back.Text);

                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole_Depth_TapDrill = modMain.ConvTextToDouble(txtMount_Holes_Depth_TapDrill_Back.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole_Depth_Tap = modMain.ConvTextToDouble(txtMount_Holes_Depth_Tap_Back.Text);
                                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole_Depth_Engagement = modMain.ConvTextToDouble(txtMount_Holes_Depth_Engagement_Back.Text);
                            }
                        }

                    #endregion

                    //#region  EDM Pad
                    ////  -----------
                    //    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).EDM_Pad.RFillet_Back = modMain.ConvTextToDouble(txtEDM_Pad_RFilletBack.Text);
                    //#endregion
                }

            #endregion


            #region "COMBOBOX RELATED ROUTINES:"
            //----------------------------------  

                private void ComboBox_MouseDown(object sender, MouseEventArgs e)
                //===============================================================
                {
                     ComboBox pCmbBox = (ComboBox)sender;

                     switch (pCmbBox.Name)
                     {  
                         case "cmbARP_Spec_D_Desig":
                             //----------------------------
                             //mblnARP_Stickout_Changed_ManuallyChanged = false;        
                             //mblnARP_Spec_D_Desig_ManuallyChanged = true;
                             break;
                      
                     }
                }
             

                private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)  
                //====================================================================
                {                    
                    ComboBox pCmbBox = (ComboBox)sender;

                    switch(pCmbBox.Name)
                    {
                        #region "Tab: OilInlet:"
                        //-----------------

                            case "cmbOilInlet_Count_MainOilSupply":
                            //-------------------------------------
                                mBearing_Radial_FP.OilInlet.Count_MainOilSupply = modMain.ConvTextToInt(pCmbBox.Text);
                                     
                                break;

                            case "cmbOilInlet_Orifice_StartPos":
                            //---------------------------------
                                mBearing_Radial_FP.OilInlet.Orifice_StartPos = (clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos)
                                    Enum.Parse(typeof(clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos), pCmbBox.Text);
                                txtARP_Ang_Horz.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.ARP.Ang_Horz(), "");
                                break;

                            case "cmbOilInlet_Orifice_CBoreDia":
                            //--------------------------------------
                                string pVal = pCmbBox.Text;
                                Double pNumerator, pDenominator;
                                Double pFinal = 0.0;

                                if (pVal.ToString() != "1")
                                {
                                    pVal = pVal.Remove(pVal.Length - 1);
                                    pNumerator = Convert.ToInt32(modMain.ExtractPreData(pVal, "/"));
                                    pDenominator = Convert.ToInt32(modMain.ExtractPostData(pVal, "/"));
                                    pFinal = Convert.ToDouble(pNumerator / pDenominator);
                                }
                                else
                                {
                                    pFinal = Convert.ToDouble(pVal);
                                }

                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    if (pFinal > modMain.gcEPS)
                                    {
                                        lblOilInlet_Orifice_DDrill_CBore.Visible = true;
                                        lblOilInlet_Orifice_DDrill_CBore.Text = "(" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(pFinal)) + ")";
                                    }
                                    else
                                    {
                                        lblOilInlet_Orifice_DDrill_CBore.Visible = false;
                                    }
                                }

                                break;

                        #endregion


                        #region  "Tab: Web MillRelief:"
                        //------------------------

                            case "cmbMillRelief_D":
                                mBearing_Radial_FP.MillRelief.D_Desig = pCmbBox.Text;
                                txtMillRelief_D_PadRelief.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.MillRelief.D_PadRelief(), "#0.000");
                                break;

                        #endregion


                        #region "Tab: S/L Hardware:"
                        //---------------------

                            case "cmbSL_Screw_Spec_UnitSystem":
                                //----------------------------
                                if (pCmbBox.Text != "")
                                {
                                    pCmbBox.SelectedIndex = 1;
                                    mBearing_Radial_FP.SL.Screw.Spec.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pCmbBox.Text);                               
                                    Populate_SL_Details(cmbSL_Screw_Spec_Type);

                                    cmbSL_Dowel_Spec_UnitSystem.Text = cmbSL_Screw_Spec_UnitSystem.Text;        
                                   
                                    //....Front
                                    Populate_Screw_D_Desig(ref cmbMount_Screw_D_Desig_Front,
                                                            mBearing_Radial_FP.Mount.Screw[0].Spec.Type,
                                                            mBearing_Radial_FP.Mount.Screw[0].Spec.Mat,
                                                            mBearing_Radial_FP.SL.Screw.Spec.Unit.System,
                                                            mBearing_Radial_FP.Mount.Screw[0].Spec.D_Desig);

                                    lblMount_Unit_Front.Text = cmbSL_Screw_Spec_UnitSystem.Text;

                                    //....Back
                                    Populate_Screw_D_Desig(ref cmbMount_Screw_D_Desig_Back,
                                                            mBearing_Radial_FP.Mount.Screw[1].Spec.Type,
                                                            mBearing_Radial_FP.Mount.Screw[1].Spec.Mat,
                                                            mBearing_Radial_FP.SL.Screw.Spec.Unit.System,
                                                            mBearing_Radial_FP.Mount.Screw[1].Spec.D_Desig);

                                    lblMount_Unit_Back.Text = cmbSL_Screw_Spec_UnitSystem.Text;                
                                }
                                break;

                            case "cmbSL_Screw_Spec_Type":
                            //-------------------------------
                                mBearing_Radial_FP.SL.Screw.Spec_Type = pCmbBox.Text;
                                Populate_Screw_Mat(ref cmbSL_Screw_Spec_Mat, pCmbBox.Text, mBearing_Radial_FP.SL.Screw.Spec.Unit.System);
                                break;

                            case "cmbSL_Screw_Spec_D_Desig":
                            //-------------------------------    
                                mBearing_Radial_FP.SL.Screw.Spec_D_Desig = pCmbBox.Text;
                                mBearing_Radial_FP.SL.Screw.GetPitch(cmbSL_Screw_Spec_Pitch, mBearing_Radial_FP.SL.Screw.Spec.D_Desig, 
                                                                     mBearing_Radial_FP.SL.Screw.Spec.Type, mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString());
                                //Retrieve_SL_Screw_Spec_PN();

                                Update_SL_Screw_L();
                                Retrieve_SL_Screw_Spec_PN();

                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    //txtSL_LScrew_Loc_Center.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Calc_Screw_Loc_Center()), "#0.000");
                                    //txtSL_RScrew_Loc_Center.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Calc_Screw_Loc_Center()), "#0.000");

                                    //txtSL_CBore_Depth.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Thread_Depth), "#0.000");
                                }
                                else
                                {
                                    //txtSL_LScrew_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Center(), "#0.000");
                                    //txtSL_RScrew_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_Screw_Loc_Center(), "#0.000");

                                    //txtSL_CBore_Depth.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Thread_Depth, "#0.000");
                                }
                                Get_CBore_Depth_SL(mBearing_Radial_FP.SL.Screw.Spec.Unit.System);
                                SetBackColor_SL_Screw_Loc_Center();
                                break;

                            case "cmbSL_Screw_Spec_Pitch":
                            //----------------------------
                                mBearing_Radial_FP.SL.Screw.Spec_Pitch = modMain.ConvTextToDouble(pCmbBox.Text);
                                Update_SL_Screw_L();
                                Retrieve_SL_Screw_Spec_PN();
                                break;

                            case "cmbSL_Screw_Spec_L":  
                                //--------------------      
                                mBearing_Radial_FP.SL.Screw.Spec_L = modMain.ConvTextToDouble(pCmbBox.Text);
                                Retrieve_SL_Screw_Spec_PN();
                                //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                //{
                                //    txtSL_CBore_Dia.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw_Spec.CBore.Depth), "#0.000");
                                //}
                                //else
                                //{
                                //    txtSL_CBore_Dia.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Screw_Spec.CBore.Depth, "#0.000");
                                //}
                                break;

                            case "cmbSL_Screw_Spec_Mat":
                            //--------------------------
                                mBearing_Radial_FP.SL.Screw.Spec_Mat = pCmbBox.Text;
                                Populate_Screw_D_Desig(ref cmbSL_Screw_Spec_D_Desig, mBearing_Radial_FP.SL.Screw.Spec.Type,
                                                                                     cmbSL_Screw_Spec_Mat.Text,
                                                                                     mBearing_Radial_FP.SL.Screw.Spec.Unit.System,
                                                                                     mBearing_Radial_FP.SL.Screw.Spec.D_Desig); 
                                break;

                            case "cmbSL_Dowel_Spec_UnitSystem":
                                //-----------------------------
                                if (pCmbBox.Text != "")
                                {                               
                                    mBearing_Radial_FP.SL.Dowel.Spec.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pCmbBox.Text);
                                    Populate_SL_Details(cmbSL_Dowel_Spec_Type);
                                }
                                break;

                            case "cmbSL_Dowel_Spec_Type":
                            //---------------------------
                                mBearing_Radial_FP.SL.Dowel.Spec_Type = pCmbBox.Text;
                                Populate_Pin_Mat(ref cmbSL_Dowel_Spec_Mat, pCmbBox.Text, mBearing_Radial_FP.SL.Dowel.Spec.Unit.System);
                                break;

                            case "cmbSL_Dowel_Spec_D_Desig":
                            //------------------------------
                                mBearing_Radial_FP.SL.Dowel.Spec_D_Desig = pCmbBox.Text;
                                Update_SL_Dowel_L();
                                Retrieve_SL_Dowel_Spec_PN();
                                Retrieve_SL_Dowel_Spec_Depth();
                                break;

                            case "cmbSL_Dowel_Spec_L":
                                //------------------------      
                                mBearing_Radial_FP.SL.Dowel.Spec_L = modMain.ConvTextToDouble(pCmbBox.Text);
                                Retrieve_SL_Dowel_Spec_PN();
                                Retrieve_SL_Dowel_Spec_Depth();
                                //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                //{
                                //    txtSL_Dowel_Depth.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Dowel_Depth), "#0.000");
                                //}
                                //else
                                //{
                                //    txtSL_Dowel_Depth.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Dowel_Depth, "#0.000");
                                //}
                                break;

                            case "cmbSL_Dowel_Spec_Mat":
                            //-------------------------
                                mBearing_Radial_FP.SL.Dowel.Spec_Mat = pCmbBox.Text;
                                Populate_Pin_D_Desig(ref cmbSL_Dowel_Spec_D_Desig, cmbSL_Dowel_Spec_Type.Text,
                                                     cmbSL_Dowel_Spec_Mat.Text, mBearing_Radial_FP.SL.Dowel.Spec.Unit.System,mBearing_Radial_FP.SL.Dowel.Spec.D_Desig );         //BG 26MAR12
                                break;
                        
                        #endregion


                        #region "Tab: ARP:"
                        //--------------------------

                            case "cmbARP_Spec_Type":             
                            //-------------------------
                                mBearing_Radial_FP.ARP.Dowel.Spec_Type = pCmbBox.Text;
                                Populate_Pin_Mat(ref cmbARP_Spec_Mat, pCmbBox.Text, mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System);
                                break;

                            case "cmbARP_Spec_D_Desig":
                            //---------------------------------

                                mBearing_Radial_FP.ARP.Dowel.Spec_D_Desig = pCmbBox.Text;

                                if (cmbARP_Spec_Type.Text != ""
                                    && cmbARP_Spec_D_Desig.Text != "")
                                {
                                    Populate_ARP_L();
                                    Retrieve_ARP_PN();
                                    Retrieve_ARP_Dowel_Spec_Depth();
                                }
                          
                                //mBearing_Radial_FP.ARP.Dowel.Hole_Depth=0.0;

                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    txtARP_Loc_Offset.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Dowel.D()), "#0.000"); 
                                    //txtAntiRotPin_Depth.Text = modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Dowel.Hole.Depth).ToString("#0.000");
                                    //txtARP_Depth.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Dowel.D()), "#0.000");                                         
                                }
                                else
                                {
                                    txtARP_Loc_Offset.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.ARP.Dowel.D(), "#0.000");
                                    // txtAntiRotPin_Depth.Text = mBearing_Radial_FP.ARP.Dowel.Hole.Depth.ToString("#0.000");
                                    //txtARP_Depth.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.ARP.Dowel.D(), "#0.000");
                                }
                                
                                //if (mblnAntiRotPin_Spec_D_Desig_ManuallyChanged)
                                //{
                                //    //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                //    //{
                                //    //    txtSL_LDowel_Loc_Center.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Calc_LDowel_Loc_Center()), "#0.000");         
                                //    //    txtSL_RDowel_Loc_Center.Text = modMain.ConvDoubleToStr(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Calc_RDowel_Loc_Center()), "#0.000");         
                                //    //}
                                //    //else
                                //    //{
                                //    //    txtSL_LDowel_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_LDowel_Loc_Center(), "#0.000");
                                //    //    txtSL_RDowel_Loc_Center.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Calc_RDowel_Loc_Center(), "#0.000");   
                                //    //}
                                //    mblnAntiRotPin_Spec_D_Desig_ManuallyChanged = false;
                                //}

                                break;

                            case "cmbARP_Spec_L":
                            //--------------------------
                                Double pPrevVal_ARP_Spec_L = mBearing_Radial_FP.ARP.Dowel.Spec.L;
                                mBearing_Radial_FP.ARP.Dowel.Spec_L = modMain.ConvTextToDouble(pCmbBox.Text);

                                Retrieve_ARP_PN();
                                Retrieve_ARP_Dowel_Spec_Depth();

                                if (pPrevVal_ARP_Spec_L != mBearing_Radial_FP.ARP.Dowel.Spec.L)
                                {
                                    double pL = 0.0;
                                    if (mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pL = mBearing_Radial_FP.ARP.Dowel.Spec.L / 25.4;
                                        txtARP_Stickout.Text =modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Stickout(pL)));
                                    }
                                    else
                                    {
                                        pL = mBearing_Radial_FP.ARP.Dowel.Spec.L;
                                        txtARP_Stickout.Text =modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.ARP.Stickout(pL));
                                    }
                                }                                
                                break;

                            case "cmbARP_Spec_Mat":
                            //-----------------------
                                mBearing_Radial_FP.ARP.Dowel.Spec_Mat = pCmbBox.Text;
                                Populate_Pin_D_Desig(ref cmbARP_Spec_D_Desig, cmbARP_Spec_Type.Text,
                                                            cmbARP_Spec_Mat.Text, mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System, mBearing_Radial_FP.ARP.Dowel.Spec.D_Desig);         
                                break;

                            case "cmbARP_Spec_UnitSystem":
                            //-----------------------------
                                if (pCmbBox.Text != "")
                                {
                                    pCmbBox.SelectedIndex = 1;
                                    mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pCmbBox.Text);
                                    Populate_ARP_Spec_Details();
                                }
                                break;

                            case "cmbARP_Loc_CasingSL":
                            //-------------------------
                                if (cmbARP_Loc_CasingSL.Text == "Offset")
                                {
                                    cmbARP_Loc_Offset_Direction.Visible = true;
                                    cmbARP_Loc_Offset_Direction.SelectedIndex = 0;
                                    txtARP_Loc_Offset.Visible = true;
                                }
                                else
                                {
                                    cmbARP_Loc_Offset_Direction.Visible = false;
                                    txtARP_Loc_Offset.Visible = false;
                                    //txtARP_Loc_Offset.Text = "0.0";
                                }
                                break;

                        #endregion


                        #region  "Tab: Mount:"
                        //-----------------------
                          
                            //  Screw:
                            //  ------
                            case "cmbMount_Screw_Type_Front":
                                //---------------------------------------
                                mBearing_Radial_FP.Mount.Screw[0].Spec_Type = pCmbBox.Text;
                                Populate_Screw_D_Desig(ref cmbMount_Screw_D_Desig_Front, mBearing_Radial_FP.Mount.Screw[0].Spec.Type,
                                                                                                     cmbMount_Screw_Mat_Front.Text,
                                                                                                     mBearing_Radial_FP.SL.Screw.Spec.Unit.System,
                                                                                                     mBearing_Radial_FP.Mount.Screw[0].Spec.D_Desig);
                                break;

                            case "cmbMount_Screw_Type_Back":
                                //-----------------------------------
                                mBearing_Radial_FP.Mount.Screw[1].Spec_Type = pCmbBox.Text;
                                Populate_Screw_D_Desig(ref cmbMount_Screw_D_Desig_Back, mBearing_Radial_FP.Mount.Screw[1].Spec.Type,
                                                            cmbMount_Screw_Mat_Back.Text, mBearing_Radial_FP.SL.Screw.Spec.Unit.System,
                                                            mBearing_Radial_FP.Mount.Screw[1].Spec.D_Desig);
                                break;

                            case "cmbMount_Screw_Mat_Front":
                                //--------------------------------------
                                mBearing_Radial_FP.Mount.Screw[0].Spec_Mat = pCmbBox.Text;
                                Populate_Screw_D_Desig(ref cmbMount_Screw_D_Desig_Front,
                                                        mBearing_Radial_FP.Mount.Screw[0].Spec.Type,
                                                        cmbMount_Screw_Mat_Front.Text,
                                                        mBearing_Radial_FP.SL.Screw.Spec.Unit.System,
                                                        mBearing_Radial_FP.Mount.Screw[0].Spec.D_Desig);
                                break;

                            case "cmbMount_Screw_Mat_Back":
                                //---------------------------------
                                mBearing_Radial_FP.Mount.Screw[1].Spec_Mat = pCmbBox.Text;
                                Populate_Screw_D_Desig(ref cmbMount_Screw_D_Desig_Back,
                                                        mBearing_Radial_FP.Mount.Screw[1].Spec.Type,
                                                        cmbMount_Screw_Mat_Back.Text,
                                                        mBearing_Radial_FP.SL.Screw.Spec.Unit.System,
                                                        mBearing_Radial_FP.Mount.Screw[1].Spec.D_Desig);
                                break;



                            case "cmbMount_Screw_D_Desig_Front":
                                //----------------------------------
                                txtMountHoles_CBore_DDrill_Front.Text = "";
                                txtMountHoles_CBore_Dia_Front.Text = "";
                                txtMountHoles_CBore_Depth_Front.Text = "";

                                txtMount_Holes_Depth_TapDrill_Front.Text = "";
                                txtMount_Holes_Depth_Tap_Front.Text = "";
                                txtMount_Holes_Depth_Engagement_Front.Text = "";

                                txtMount_EndConfig_OD_Front.BackColor = Color.White;
                                txtEndConfig_DBC_Front.BackColor = Color.White;

                                mBearing_Radial_FP.Mount.Screw[0].Spec_D_Desig = pCmbBox.Text;
                                mBearing_Radial_FP.Mount.Screw[0].GetPitch(cmbMount_Screw_Pitch_Front, mBearing_Radial_FP.Mount.Screw[0].Spec.D_Desig,
                                                                                 cmbMount_Screw_Type_Front.Text, mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString());
                                Populate_Mount_Screw_L(0, cmbMount_Screw_L_Front);
                                Get_CBore_Depth_Mount(mBearing_Radial_FP.SL.Screw.Spec.Unit.System, 0, txtMountHoles_CBore_Dia_Front, txtMountHoles_CBore_DDrill_Front,
                                                      txtMount_Holes_Depth_TapDrill_Front, txtMount_Holes_Depth_Tap_Front, txtMount_Holes_Depth_Engagement_Front);

                                modMain.gProject.Product.Bearing =(clsBearing_Radial_FP) mBearing_Radial_FP.Clone();
                                mMount_DBC_LLimit[0] = mBearing_Radial_FP.Mount.DBC_LLimit(modMain.gProject.Product, 0);
                                lblEndConfig_DBC_LLim_Front.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(mMount_DBC_LLimit[0]);

                                mEndPlate_OD_LLimit[0] = mEndPlate[0].OD_LLimit(modMain.gProject.Product, 0);
                                Display_EndPlate_OD(ref txtMount_EndConfig_OD_Front, 0);


                                double pMountHole_DBC_MeanFront = (mBearing_Radial_FP.Mount.DBC_LLimit(modMain.gProject.Product,0) + mBearing_Radial_FP.Mount.DBC_ULimit(0)) / 2; 
                                if (mBearing_Radial_FP.Mount.BC[0].D > modMain.gcEPS)
                                {
                                    txtEndConfig_DBC_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing_Radial_FP.Mount.BC[0].D);
                                }
                                else
                                {
                                    txtEndConfig_DBC_Front.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(pMountHole_DBC_MeanFront);
                                }
                                SetBackColor_SealOD_Front();
                                break;


                            case "cmbMount_Screw_D_Desig_Back":
                                //-----------------------------
                                //....Populate Pitch Array & Pitch Type.  
                                txtMountHoles_CBore_DDrill_Back.Text = "";
                                txtMountHoles_CBore_Dia_Back.Text = "";
                                txtMountHoles_CBore_Depth_Back.Text = "";

                                txtMount_Holes_Depth_TapDrill_Back.Text = "";
                                txtMount_Holes_Depth_Tap_Back.Text = "";
                                txtMount_Holes_Depth_Engagement_Back.Text = "";

                                txtMount_EndConfig_OD_Back.BackColor = Color.White;
                                txtEndConfig_DBC_Back.BackColor = Color.White;

                                mBearing_Radial_FP.Mount.Screw[1].Spec_D_Desig = pCmbBox.Text;
                                mBearing_Radial_FP.Mount.Screw[1].GetPitch(cmbMount_Screw_Pitch_Back, mBearing_Radial_FP.Mount.Screw[1].Spec.D_Desig,
                                                                                 mBearing_Radial_FP.Mount.Screw[1].Spec.Type, mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString());
                               
                                Populate_Mount_Screw_L(1, cmbMount_Screw_L_Back);
                                Get_CBore_Depth_Mount(mBearing_Radial_FP.SL.Screw.Spec.Unit.System, 1, txtMountHoles_CBore_Dia_Back, txtMountHoles_CBore_DDrill_Back,
                                                         txtMount_Holes_Depth_TapDrill_Back, txtMount_Holes_Depth_Tap_Back, txtMount_Holes_Depth_Engagement_Back);

                                modMain.gProject.Product.Bearing = (clsBearing_Radial_FP)mBearing_Radial_FP.Clone();
                                mMount_DBC_LLimit[1] = mBearing_Radial_FP.Mount.DBC_LLimit(modMain.gProject.Product, 1);
                                lblEndConfig_DBC_LLim_Back.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(mMount_DBC_LLimit[1]);

                                if (!mblnMount_Front_Copy)
                                {
                                    mEndPlate_OD_LLimit[1] = mEndPlate[1].OD_LLimit(modMain.gProject.Product, 1);
                                    Display_EndPlate_OD(ref txtMount_EndConfig_OD_Back, 1);


                                    double pMountHole_DBC_MeanBack = (mBearing_Radial_FP.Mount.DBC_LLimit(modMain.gProject.Product, 1) + mBearing_Radial_FP.Mount.DBC_ULimit(1)) / 2;
                                    if (mBearing_Radial_FP.Mount.BC[1].D > modMain.gcEPS)
                                    {
                                        txtEndConfig_DBC_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing_Radial_FP.Mount.BC[1].D);
                                    }
                                    else
                                    {
                                        txtEndConfig_DBC_Back.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(pMountHole_DBC_MeanBack);
                                    }
                                }
                                SetBackColor_SealOD_Back();
                                break;

                            case "cmbMount_Screw_Pitch_Front":
                                mBearing_Radial_FP.Mount.Screw[0].Spec_Pitch = modMain.ConvTextToDouble(pCmbBox.Text);
                                Populate_Mount_Screw_L(0, cmbMount_Screw_L_Front);
                                break;

                            case "cmbMount_Screw_Pitch_Back":
                                mBearing_Radial_FP.Mount.Screw[1].Spec_Pitch = modMain.ConvTextToDouble(pCmbBox.Text);
                                Populate_Mount_Screw_L(1, cmbMount_Screw_L_Back);
                                break;

                            case "cmbMount_Screw_L_Front":
                                //------------------------------------
                                mBearing_Radial_FP.Mount.Screw[0].Spec_L = modMain.ConvTextToDouble(pCmbBox.Text);
                                break;

                            case "cmbMount_Screw_L_Back":
                                //-------------------------------
                                mBearing_Radial_FP.Mount.Screw[1].Spec_L = modMain.ConvTextToDouble(pCmbBox.Text);
                                break;      

                            case "cmbMount_HolesCount_Front":
                            //-------------------------------
                               int pCount = modMain.ConvTextToInt(cmbMount_HolesCount_Front.Text);
                               mBearing_Radial_FP.Mount.BC[0].Count = modMain.ConvTextToInt(cmbMount_HolesCount_Front.Text);

                               if (mblnMount_Holes_Count_Front_ManuallyChanged)
                               {
                                   if (pCount == 4)
                                   {
                                       mBearing_Radial_FP.Mount.BC[0].AngStart = 45;
                                       txtMount_HolesAngStart_Front.Text = mBearing_Radial_FP.Mount.BC[0].AngStart.ToString("#0.#");
                                   }
                                   else
                                   {
                                       mBearing_Radial_FP.Mount.BC[0].AngStart = 30;
                                       txtMount_HolesAngStart_Front.Text = mBearing_Radial_FP.Mount.BC[0].AngStart.ToString("#0.#");
                                   }

                                   mblnMount_Holes_Count_Front_ManuallyChanged = false;
                               }
                               //....Angle other.
                               Display_AnglesBet(mBearing_Radial_FP, 0);  
                               break;

                            case "cmbMount_HolesCount_Back":
                            //-------------------------------
                                pCount = modMain.ConvTextToInt(cmbMount_HolesCount_Back.Text); 
                                mBearing_Radial_FP.Mount.BC[1].Count = modMain.ConvTextToInt(cmbMount_HolesCount_Back.Text);

                                if (mblnMount_Holes_Count_Back_ManuallyChanged)
                                {
                                    if (pCount == 4)
                                    {
                                        mBearing_Radial_FP.Mount.BC[1].AngStart = 45;
                                        txtMount_HolesAngStart_Back.Text = mBearing_Radial_FP.Mount.BC[1].AngStart.ToString("#0.#");
                                    }
                                    else
                                    {
                                        mBearing_Radial_FP.Mount.BC[1].AngStart = 30;
                                        txtMount_HolesAngStart_Back.Text = mBearing_Radial_FP.Mount.BC[1].AngStart.ToString("#0.#");
                                    }
                                    mblnMount_Holes_Count_Back_ManuallyChanged = false;
                                }
                                    //....Angle other.
                                Display_AnglesBet(mBearing_Radial_FP, 1); 
                                break;


                            case "cmbMount_HolesAngStart_Front":
                            //----------------------------------
                                String pMount_Sel_HolesAngStart = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.BC[0].AngStart, "");
                                mBearing_Radial_FP.Mount.BC[0].AngStart = modMain.ConvTextToDouble(pCmbBox.Text);
                                break;

                            case "cmbMount_HolesAngStart_Back":
                                //-----------------------------
                                pMount_Sel_HolesAngStart = modMain.ConvDoubleToStr(mBearing_Radial_FP.Mount.BC[1].AngStart, "");
                                mBearing_Radial_FP.Mount.BC[1].AngStart = modMain.ConvTextToDouble(pCmbBox.Text);
                                break;                           

                        #endregion
                    }
                }

                private void cmbOilInlet_Orifice_LD_SelectedIndexChanged(object sender, EventArgs e)
                //==================================================================================
                {
                    mBearing_Radial_FP.OilInlet.Orifice_Ratio_L_D = modMain.ConvTextToDouble(cmbOilInlet_Orifice_LD.Text);

                    double pOrifice_L = 0;

                    if (mBearing_Radial_FP.OilInlet.Orifice.Ratio_L_D > modMain.gcEPS)
                    {
                        pOrifice_L = mBearing_Radial_FP.OilInlet.Orifice.Ratio_L_D * mBearing_Radial_FP.OilInlet.Orifice.D;
                    }
                    if (modMain.gProject.Product.Unit.System == clsUnit.eSystem.Metric)
                    {
                        txtOilInlet_Orifice_L.Text = modMain.gProject.Product.Unit.WriteInUserL(modMain.gProject.Product.Unit.CEng_Met(pOrifice_L));
                    }
                    else
                    {
                        txtOilInlet_Orifice_L.Text = modMain.gProject.Product.Unit.WriteInUserL(pOrifice_L);
                    }
                }


                private void Get_CBore_Depth_SL(clsUnit.eSystem Unit_In)
                //====================================================
                {
                    string pUnitSystem = "";

                    if (Unit_In == clsUnit.eSystem.English)
                        pUnitSystem = "E";
                    else if (Unit_In == clsUnit.eSystem.Metric)
                        pUnitSystem = "M";

                    //....EXCEL File: StdPartsData
                    OleDbDataReader pobjDR = null;
                    OleDbConnection pConnection = null;
                    String pstrFIELDS = "Select *";
                    String pstrFROM = " FROM [Screw_D$]";
                    String pstrWHERE = " WHERE  Unit = '" + pUnitSystem + "' and Type = '" + mBearing_Radial_FP.SL.Screw.Spec.Type + "' and D_Desig = '" + mBearing_Radial_FP.SL.Screw.Spec.D_Desig + "'";   

                    String pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE;
                    pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdPartsData, ref pConnection);

                    if (pobjDR.Read())
                    {
                        int pColFldName = 0;
                        mBearing_Radial_FP.SL.Screw.Hole_CBore_D = 0;
                        mBearing_Radial_FP.SL.Screw.Hole_D_Drill = 0;
                        mBearing_Radial_FP.SL.Screw.Hole_Depth_TapDrill = 0;
                        mBearing_Radial_FP.SL.Screw.Hole_Depth_Tap = 0;
                        mBearing_Radial_FP.SL.Screw.Hole_Depth_Engagement = 0;

                        pColFldName = pobjDR.GetOrdinal("D_CBore");
                        if (pobjDR.IsDBNull(pColFldName) == false)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_CBore_D = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pobjDR["D_CBore"].ToString()));
                                txtSL_CBore_Dia.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw.Hole.CBore.D));
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_CBore_D = modMain.ConvTextToDouble(pobjDR["D_CBore"].ToString());
                                txtSL_CBore_Dia.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Screw.Hole.CBore.D);
                            }
                        }

                        pColFldName = pobjDR.GetOrdinal("D_Drill");
                        if (pobjDR.IsDBNull(pColFldName) == false)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_D_Drill =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(pobjDR["D_Drill"].ToString()));
                                txtSL_CBore_DDrill.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw.Hole.D_Drill));
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_D_Drill = modMain.ConvTextToDouble(pobjDR["D_Drill"].ToString());
                                txtSL_CBore_DDrill.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Screw.Hole.D_Drill);
                            }
                        }

                        pColFldName = pobjDR.GetOrdinal("Depth_TapDrill");
                        if (pobjDR.IsDBNull(pColFldName) == false)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_Depth_TapDrill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pobjDR["Depth_TapDrill"].ToString()));
                                txtSL_Depth_TapDrill.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw.Hole.Depth.TapDrill));
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_Depth_TapDrill = modMain.ConvTextToDouble(pobjDR["Depth_TapDrill"].ToString());
                                txtSL_Depth_TapDrill.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Screw.Hole.Depth.TapDrill);
                            }
                        }

                        pColFldName = pobjDR.GetOrdinal("Depth_Tap");
                        if (pobjDR.IsDBNull(pColFldName) == false)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_Depth_Tap =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(pobjDR["Depth_Tap"].ToString()));
                                txtSL_Depth_Tap.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw.Hole.Depth.Tap));
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_Depth_Tap = modMain.ConvTextToDouble(pobjDR["Depth_Tap"].ToString());
                                txtSL_Depth_Tap.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Screw.Hole.Depth.Tap);
                            }
                        }

                        pColFldName = pobjDR.GetOrdinal("Min_Engagement");
                        if (pobjDR.IsDBNull(pColFldName) == false)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_Depth_Engagement = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pobjDR["Min_Engagement"].ToString()));
                                txtSL_Depth_Engagement.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw.Hole.Depth.Engagement));
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_Depth_Engagement = modMain.ConvTextToDouble(pobjDR["Min_Engagement"].ToString());
                                txtSL_Depth_Engagement.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Screw.Hole.Depth.Engagement);
                            }
                        }
                    }

                    pobjDR.Dispose();
                    pConnection.Close();
                }

                private void Get_CBore_Depth_Mount(clsUnit.eSystem Unit_In, int Pos_In, TextBox TxtCBore_Dia_In, TextBox TxtCBore_DDrill_In,
                                                   TextBox TxtDepth_TapDrill, TextBox TxtDepth_Tap, TextBox TxtDepth_Engagement)
                //===========================================================================================================================
                {
                    string pUnitSystem = "";

                    if (Unit_In == clsUnit.eSystem.English)
                        pUnitSystem = "E";
                    else if (Unit_In == clsUnit.eSystem.Metric)
                        pUnitSystem = "M";

                    //....EXCEL File: StdPartsData
                    OleDbDataReader pobjDR = null;
                    OleDbConnection pConnection = null;
                    String pstrFIELDS = "Select *";
                    String pstrFROM = " FROM [Screw_D$]";
                    String pstrWHERE = " WHERE  Unit = '" + pUnitSystem + "' and Type = '" + mBearing_Radial_FP.SL.Screw.Spec.Type + "' and D_Desig = '" + mBearing_Radial_FP.Mount.Screw[Pos_In].Spec.D_Desig + "'";

                    String pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE;
                    pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdPartsData, ref pConnection);

                    if (pobjDR.Read())
                    {
                        int pColFldName = 0;

                        mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_CBore_D = 0;
                        mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_D_Drill = 0;
                        mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_Depth_TapDrill = 0;
                        mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_Depth_Tap = 0;
                        mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_Depth_Engagement = 0;

                        pColFldName = pobjDR.GetOrdinal("D_CBore");
                        if (pobjDR.IsDBNull(pColFldName) == false)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {                                
                                mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_CBore_D = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pobjDR["D_CBore"].ToString()));
                                TxtCBore_Dia_In.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[Pos_In].Hole.CBore.D));
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_CBore_D = modMain.ConvTextToDouble(pobjDR["D_CBore"].ToString());
                                TxtCBore_Dia_In.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[Pos_In].Hole.CBore.D);
                            }
                        }

                        pColFldName = pobjDR.GetOrdinal("D_Drill");
                        if (pobjDR.IsDBNull(pColFldName) == false)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_D_Drill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pobjDR["D_Drill"].ToString()));
                                TxtCBore_DDrill_In.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[Pos_In].Hole.D_Drill));
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_D_Drill = modMain.ConvTextToDouble(pobjDR["D_Drill"].ToString());
                                TxtCBore_DDrill_In.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[Pos_In].Hole.D_Drill);
                            }
                        }

                        pColFldName = pobjDR.GetOrdinal("Depth_TapDrill");
                        if (pobjDR.IsDBNull(pColFldName) == false)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_Depth_TapDrill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pobjDR["Depth_TapDrill"].ToString()));
                                TxtDepth_TapDrill.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[Pos_In].Hole.Depth.TapDrill));
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_Depth_TapDrill = modMain.ConvTextToDouble(pobjDR["Depth_TapDrill"].ToString());
                                TxtDepth_TapDrill.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[Pos_In].Hole.Depth.TapDrill);
                            }
                        }

                        pColFldName = pobjDR.GetOrdinal("Depth_Tap");
                        if (pobjDR.IsDBNull(pColFldName) == false)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_Depth_Tap = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pobjDR["Depth_Tap"].ToString()));
                                TxtDepth_Tap.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[Pos_In].Hole.Depth.Tap));
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_Depth_Tap = modMain.ConvTextToDouble(pobjDR["Depth_Tap"].ToString());
                                TxtDepth_Tap.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[Pos_In].Hole.Depth.Tap);
                            }
                        }

                        pColFldName = pobjDR.GetOrdinal("Min_Engagement");
                        if (pobjDR.IsDBNull(pColFldName) == false)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_Depth_Engagement = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pobjDR["Min_Engagement"].ToString()));
                                TxtDepth_Engagement.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.Screw[Pos_In].Hole.Depth.Engagement));
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[Pos_In].Hole_Depth_Engagement = modMain.ConvTextToDouble(pobjDR["Min_Engagement"].ToString());
                                TxtDepth_Engagement.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw[Pos_In].Hole.Depth.Engagement);
                            }
                        }
                    }

                    pobjDR.Dispose();
                    pConnection.Close();
                }

                private void cmbBox_MouseHover(object sender, EventArgs e)
                //=========================================================
                {
                    ComboBox pcmbBox = (ComboBox)sender;

                    switch (pcmbBox.Name)
                    {
                        case "cmbSL_Screw_Spec_Mat":
                            toolTip1.SetToolTip(cmbSL_Screw_Spec_Mat, cmbSL_Screw_Spec_Mat.Text);
                            break;

                        case "cmbSL_Dowel_Spec_Mat":
                            toolTip1.SetToolTip(cmbSL_Dowel_Spec_Mat, cmbSL_Dowel_Spec_Mat.Text);
                            break;

                        case "cmbARP_Spec_Mat":
                            toolTip1.SetToolTip(cmbARP_Spec_Mat, cmbARP_Spec_Mat.Text);
                            break;

                        case "cmbMount_Screw_Mat_Front":
                            toolTip1.SetToolTip(cmbMount_Screw_Mat_Front, cmbMount_Screw_Mat_Front.Text);
                            break;

                        case "cmbMount_Screw_Mat_Back":
                            toolTip1.SetToolTip(cmbMount_Screw_Mat_Front, cmbMount_Screw_Mat_Front.Text);
                            break;
                    }
                }
                

                //private void cmbMountFixture_PartNo_DrawItem(object sender, DrawItemEventArgs e)
                ////==============================================================================     
                //{
                //    ////if (e.Index < 0) return;
                    
                //    ////ComboBox pCmbBox = (ComboBox)sender;
                //    ////e.DrawBackground();
                //    ////Brush pBrush = Brushes.Black;
                                        
                //    ////if (mBearing_Radial_FP.Mount.Fixture_Candidates.Hole.EquiSpaced[e.Index])
                //    ////    pBrush = Brushes.OrangeRed;

                //    ////e.Graphics.DrawString(pCmbBox.Items[e.Index].ToString(),
                //    ////    e.Font, pBrush, e.Bounds, StringFormat.GenericDefault);
                   
                //    ////e.DrawFocusRectangle();
                //}

            #endregion


            #region "TEXTBOX RELATED ROUTINES:"
            //--------------------------------

                private void TextBox_KeyDown(object sender, KeyEventArgs e)
                //=========================================================
                {
                    TextBox pTxtBox = (TextBox)sender;
                    
                    if (!pTxtBox.ReadOnly)
                        pTxtBox.ForeColor = Color.Black;

                    switch (pTxtBox.Name)
                    {
                        case "txtL":
                            //------
                             mblnL_ManuallyChanged = true;
                             break;

                        case "txtDepth_EndConfig_Front":
                            //--------------------------
                            mblnDepth_EndPlate_F_ManuallyChanged = true;

                            pTxtBox.ForeColor = Color.Black;
                            txtDepth_EndConfig_Back.ForeColor = Color.Blue;
                            break;

                        case "txtDepth_EndConfig_Back":
                            //-------------------------
                            mblnDepth_EndPlate_B_ManuallyChanged = true;

                            txtDepth_EndConfig_Front.ForeColor = Color.Blue;
                            pTxtBox.ForeColor = Color.Black;
                            break;


                        case "txtOilInlet_Annulus_Loc_Back":
                            //------------------------------
                            mblnOilInlet_Annulus_Loc_Back_ManuallyChanged = true;
                            break;

                        case "txtARP_Loc_Dist_Front":
                            //------------------------------
                            //mblnAntiRotPin_Loc_Dist_Front_ManuallyChanged = true;
                            break;                      

                        case "txtARP_Depth":
                            //---------------------
                            //mblnAntiRotPin_Depth_Changed_ManuallyChanged = true;
                            break;

                        //case "txtAntiRotPin_Stickout":
                        //    //------------------------
                        //    mblnAntiRotPin_Stickout_Changed_ManuallyChanged = true;
                        //    break;

                        case "txtSL_LScrew_Loc_Center":
                            //------------------------------
                            mblnSL_LScrew_Loc_Center_ManuallyChanged = true;
                            break;

                        case "txtSL_RScrew_Loc_Center":
                            //------------------------------
                            mblnSL_RScrew_Loc_Center_ManuallyChanged = true;
                            break;


                        case "txtMount_Holes_Thread_Depth_Front":
                        case "txtMount_Holes_Thread_Depth_Back":
                            //------------------------------------
                            //mblnMount_Holes_Thread_Depth_ManuallyChanged = true;
                            break;

                        case "txtMount_EndConfig_OD_Front":
                            mblnMount_EndConfig_OD_Front = true;
                            break;

                        case "txtMount_EndConfig_OD_Back":
                            mblnMount_EndConfig_OD_Back = true;
                            break;

                        case "txtEndConfig_DBC_Front":
                            mblnEndConfig_DBC_Front = true;
                            break;

                        case "txtEndConfig_DBC_Back":
                            mblnEndConfig_DBC_Back = true;
                            break;

                        case "txtTempSensor_CanLength":
                            //-------------------------
                            //mblnTempSensor_CanLength_ManuallyChanged = true;
                            break;
                    }
                }

                private void txtOilInlet_Annulus_Wid_KeyDown(object sender, KeyEventArgs e)
                //=============================================================================
                {
                    //mblnAnnulus_Wid_ManuallyChanged = true;
                }

                private void txtOilInlet_Annulus_Depth_KeyDown(object sender, KeyEventArgs e)
                //=============================================================================
                {
                    mblnAnnulus_Depth_ManuallyChanged = true;
                }

                private void txtOilInlet_Annulus_Dia_KeyDown(object sender, KeyEventArgs e)
                //=============================================================================
                {
                    mblnAnnulus_Dia_ManuallyChanged = true;
                }

                private void txtLength_EndConfig_Front_TextChanged(object sender, EventArgs e)
                //=============================================================================
                {
                    mEndPlate[0].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLength_EndConfig_Front.Text));
                    double pDepth = modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Calc_Depth_EndPlate());
                    Double pVal = modMain.ConvTextToDouble(txtLength_EndConfig_Front.Text);

                    if (Math.Abs(pVal - pDepth) > modMain.gcEPS)
                    {
                        txtLength_EndConfig_Front.ForeColor = Color.Black;
                    }
                    else
                    {
                        txtLength_EndConfig_Front.ForeColor = Color.Blue;
                    }

                }

                private void txtLength_EndConfig_Back_TextChanged(object sender, EventArgs e)
                //===========================================================================
                {
                    mEndPlate[1].L = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtLength_EndConfig_Back.Text));
                    double pDepth = modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Calc_Depth_EndPlate());

                    Double pVal = modMain.ConvTextToDouble(txtLength_EndConfig_Back.Text);

                    if (Math.Abs(pVal - pDepth) > modMain.gcEPS)
                    {
                        txtLength_EndConfig_Back.ForeColor = Color.Black;
                    }
                    else
                    {
                        txtLength_EndConfig_Back.ForeColor = Color.Blue;
                    }
                }
               

                private void TextBox_TextChanged(object sender, EventArgs e)       
                //==========================================================            
                {
                    TextBox pTxtBox = (TextBox)sender;
                    Double pVal = 0.0;

                    switch(pTxtBox.Name)
                    {
                        case "txtL":                                //  Bearing Length:
                        //----------
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.L =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(txtL.Text));

                                if (mblnL_ManuallyChanged)
                                {
                                    //....The following special actions to taken when L is manually changed.
                                    //

                                    //....End Plates Depth:        
                                    //
                                    double pDepth =modMain.gProject.PNR.Unit.CEng_Met( mBearing_Radial_FP.Calc_Depth_EndPlate());      //....Symmetrical Depths.

                                    //....FRONT:                                    //
                                    txtDepth_EndConfig_Front.Text = pDepth.ToString("#0.000");
                                    txtDepth_EndConfig_Front.ForeColor = Color.Blue;
                                    mblnDepth_EndPlate_F_ManuallyChanged = false;

                                    mBearing_Radial_FP.Depth_EndPlate[0] = modMain.gProject.PNR.Unit.CMet_Eng(pDepth);

                                    if (modMain.gProject.Product.EndPlate[0].L < modMain.gcEPS)
                                    {
                                        txtLength_EndConfig_Front.Text =modMain.gProject.PNR.Unit.WriteInUserL(pDepth);
                                        txtLength_EndConfig_Front.ForeColor = Color.Blue;
                                    }
                                    else
                                    {
                                        txtLength_EndConfig_Front.ForeColor = Color.Black;
                                    }

                                    //....BACK:
                                    txtDepth_EndConfig_Back.Text = pDepth.ToString("#0.000");
                                    txtDepth_EndConfig_Back.ForeColor = Color.Blue;
                                    mblnDepth_EndPlate_B_ManuallyChanged = false;

                                    mBearing_Radial_FP.Depth_EndPlate[1] = modMain.gProject.PNR.Unit.CMet_Eng(pDepth);

                                    if (modMain.gProject.Product.EndPlate[1].L < modMain.gcEPS)
                                    {
                                        //modMain.gProject.Product.EndPlate[1].L = pDepth;
                                        txtLength_EndConfig_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL(pDepth);
                                        txtLength_EndConfig_Back.ForeColor = Color.Blue;
                                    }
                                    else
                                    {
                                        txtLength_EndConfig_Back.ForeColor = Color.Black;
                                    }


                                    //  Reset the state. 
                                    //  ---------------
                                    mblnL_ManuallyChanged = false;
                                }
                            }
                            else
                            {
                                mBearing_Radial_FP.L = modMain.ConvTextToDouble(txtL.Text);

                                if (mblnL_ManuallyChanged)
                                {
                                    //....The following special actions to taken when L is manually changed.
                                    //
                                    //....End-Configs Depth:
                                    //
                                    double pDepth = mBearing_Radial_FP.Calc_Depth_EndPlate();      //....Symmetrical Depths.

                                    //....FRONT:
                                    //
                                    txtDepth_EndConfig_Front.Text = pDepth.ToString("#0.000");
                                    txtDepth_EndConfig_Front.ForeColor = Color.Blue;
                                    mblnDepth_EndPlate_F_ManuallyChanged = false;

                                    mBearing_Radial_FP.Depth_EndPlate[0] = pDepth;

                                    //....BACK:
                                    //
                                    txtDepth_EndConfig_Back.Text = pDepth.ToString("#0.000");
                                    txtDepth_EndConfig_Back.ForeColor = Color.Blue;
                                    mblnDepth_EndPlate_B_ManuallyChanged = false;

                                    mBearing_Radial_FP.Depth_EndPlate[1] = pDepth;

                                    //  Reset the state. 
                                    //  ---------------
                                    mblnL_ManuallyChanged = false;
                                }
                            }

                            break;


                        #region "Tab: OilInlet:"
                        //------------------

                            case "txtOilInlet_Orifice_D":
                            //-------------------------
                            mBearing_Radial_FP.OilInlet.Orifice_D = modMain.ConvTextToDouble(pTxtBox.Text);

                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                if (mBearing_Radial_FP.OilInlet.Orifice.D > modMain.gcEPS)
                                {
                                    lblOilInlet_Orifice_D.Visible = true;
                                    lblOilInlet_Orifice_D.Text = "(" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.OilInlet.Orifice.D)) + ")";
                                }
                                else
                                {
                                    lblOilInlet_Orifice_D.Visible = false;
                                }
                            }
                            break;

                            case "txtOilInlet_Orifice_Loc_BackFace":
                            //--------------------------------------
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    mBearing_Radial_FP.OilInlet.Orifice_Loc_Back = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Orifice_Loc_BackFace.Text));
                                }
                                else
                                {
                                    mBearing_Radial_FP.OilInlet.Orifice_Loc_Back = modMain.ConvTextToDouble(txtOilInlet_Orifice_Loc_BackFace.Text);
                                }
                             
                             break;
                          
                        case "txtOilInlet_Annulus_Loc_Back":
                            //------------------------------
                                double pPrevVal = mBearing_Radial_FP.OilInlet.Annulus.Loc_Back;
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {                                    
                                    mBearing_Radial_FP.OilInlet.Annulus_Loc_Back = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Annulus_Loc_Back.Text));                                         
                                }
                                else
                                {
                                    mBearing_Radial_FP.OilInlet.Annulus_Loc_Back = modMain.ConvTextToDouble(txtOilInlet_Annulus_Loc_Back.Text);  
                                }

                                //if (mblnOilInlet_Annulus_Loc_Back_ManuallyChanged)
                                //{
                                //    Calc_Wid(mBearing_Radial_FP.OilInlet.Annulus.Loc_Back);
                                //    mblnOilInlet_Annulus_Loc_Back_ManuallyChanged = false;
                                //}

                                if (Math.Abs(pPrevVal - mBearing_Radial_FP.OilInlet.Annulus.Loc_Back) > mcEPS)
                                {
                                    txtOilInlet_Annulus_Loc_Back.ForeColor = Color.Black;
                                }
                                else
                                {
                                    txtOilInlet_Annulus_Loc_Back.ForeColor = Color.Blue;
                                }
                            break;


                        #endregion


                        #region "Tab: Web Relief:"
                        //------------------

                        case "txtAxialSealGap_Front":
                                //====================
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.MillRelief.AxialSealGap[0] =modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(pTxtBox.Text));
                                mBearing_Radial_FP.MillRelief.AxialSealGap[1] = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                            }
                            else
                            {
                                mBearing_Radial_FP.MillRelief.AxialSealGap[0] = modMain.ConvTextToDouble(pTxtBox.Text);                              
                            }

                           break;

                        #endregion


                        #region "Tab: Anti Rotation Pin:"
                        //------------------------

                            case "txtARP_Loc_Angle":
                                //-------------------------
                                mBearing_Radial_FP.ARP.Ang_Casing_SL = modMain.ConvTextToDouble(txtARP_Loc_Angle.Text);
                                txtARP_Ang_Horz.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.ARP.Ang_Horz(), "");

                                break;

                            case "txtARP_Loc_Dist_Front":
                                //------------------------------
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    mBearing_Radial_FP.ARP.Loc_Back = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtARP_Loc_Dist_Front.Text));

                                }
                                else
                                {
                                    mBearing_Radial_FP.ARP.Loc_Back = modMain.ConvTextToDouble(txtARP_Loc_Dist_Front.Text);

                                }

                                break;

                            case "txtARP_Loc_Offset":
                                //-------------------------- 
                                //Double pValTemp = 0.0;
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    mBearing_Radial_FP.ARP.Offset = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtARP_Loc_Offset.Text));
                                }
                                else
                                {
                                    mBearing_Radial_FP.ARP.Offset = modMain.ConvTextToDouble(txtARP_Loc_Offset.Text);
                                }

                                //pValTemp = mBearing_Radial_FP.ARP.Loc.Offset;

                                if (mBearing_Radial_FP.ARP.Offset > modMain.gcEPS)
                                {
                                    //cmbARP_Loc_CasingSL.SelectedIndex = -1;
                                    cmbARP_Loc_CasingSL.SelectedIndex = 1;
                                }
                                else
                                {
                                   // cmbARP_Loc_CasingSL.SelectedIndex = -1;
                                    cmbARP_Loc_CasingSL.SelectedIndex = 0;
                                }

                                //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                //{
                                //    txtARP_Loc_Offset.Text =modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(pValTemp));
                                //}
                                //else
                                //{
                                //    txtARP_Loc_Offset.Text = modMain.gProject.PNR.Unit.WriteInUserL(pValTemp);
                                //}

                                Double pVal_Org = 0.0;
                                Double pVal_Cur = 0.0;
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    pVal_Org = modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Dowel.D());
                                    pVal_Cur = modMain.ConvTextToDouble(txtARP_Loc_Offset.Text);

                                }
                                else
                                {
                                    pVal_Org = mBearing_Radial_FP.ARP.Dowel.D();
                                    pVal_Cur = modMain.ConvTextToDouble(txtARP_Loc_Offset.Text);
                                }
                                if (Math.Abs(pVal_Org - pVal_Cur) < mcEPS)
                                {
                                    txtARP_Loc_Offset.ForeColor = Color.Blue;
                                }
                                else
                                {
                                    txtARP_Loc_Offset.ForeColor = Color.Black;
                                }
                            
                                
                                //if (Math.Abs(Math.Round(mBearing_Radial_FP.ARP.Loc.Offset, 3) - 
                                //    Math.Round(mBearing_Radial_FP.ARP.Dowel.Spec.D(), 3)) < modMain.gcEPS)
                                //    txtAntiRotPin_Loc_Offset.ForeColor = Color.Magenta;
                                //else
                                //    txtAntiRotPin_Loc_Offset.ForeColor = Color.Black;

                                break;

                            case "txtARP_Depth":
                                //---------------------
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    mBearing_Radial_FP.ARP.Dowel.Hole_Depth_Low = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtARP_Depth.Text));

                                    //if (Math.Abs(Math.Round(mBearing_Radial_FP.ARP.Dowel..Hole.Depth, 3) -
                                    //    Math.Round(mBearing_Radial_FP.ARP.Dowel.D, 3)) < modMain.gcEPS)
                                    //{
                                    //    txtAntiRotPin_Depth.ForeColor = Color.Magenta;
                                    //}

                                    //if (mblnAntiRotPin_Depth_Changed_ManuallyChanged)
                                    //{
                                    //    //txtAntiRotPin_Stickout.Text = modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Stickout).ToString("#0.000");
                                    //    //txtAntiRotPin_Stickout.Text = modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Calc_Stickout()).ToString("#0.000");
                                    //    //txtAntiRotPin_Stickout.ForeColor = Color.Blue;
                                    //    mblnAntiRotPin_Depth_Changed_ManuallyChanged = false;
                                    //}

                                    double pL = 0.0;
                                    if (mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pL = mBearing_Radial_FP.ARP.Dowel.Spec.L / 25.4;
                                        txtARP_Stickout.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Stickout(pL)));
                                    }
                                    else
                                    {
                                        pL = mBearing_Radial_FP.ARP.Dowel.Spec.L;
                                        txtARP_Stickout.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.ARP.Stickout(pL));
                                    }

                                    
                                    txtARP_Stickout.ForeColor = Color.Blue;
                                }
                                else
                                {
                                    mBearing_Radial_FP.ARP.Dowel.Hole_Depth_Low = modMain.ConvTextToDouble(txtARP_Depth.Text);

                                    //if (Math.Abs(Math.Round(mBearing_Radial_FP.ARP.Dowel..Hole.Depth, 3) -
                                    //    Math.Round(mBearing_Radial_FP.ARP.Dowel.D, 3)) < modMain.gcEPS)
                                    //{
                                    //    txtAntiRotPin_Depth.ForeColor = Color.Magenta;
                                    //}

                                    //if (mblnAntiRotPin_Depth_Changed_ManuallyChanged)
                                    //{
                                    //    //txtAntiRotPin_Stickout.Text = mBearing_Radial_FP.ARP.Stickout.ToString("#0.000");
                                    //    //txtAntiRotPin_Stickout.Text = mBearing_Radial_FP.ARP.Calc_Stickout().ToString("#0.000");
                                    //    //txtAntiRotPin_Stickout.ForeColor = Color.Blue;
                                    //    mblnAntiRotPin_Depth_Changed_ManuallyChanged = false;
                                    //}

                                    double pL = 0.0;
                                    if (mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pL = mBearing_Radial_FP.ARP.Dowel.Spec.L / 25.4;
                                        txtARP_Stickout.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Stickout(pL)));
                                    }
                                    else
                                    {
                                        pL = mBearing_Radial_FP.ARP.Dowel.Spec.L;
                                        txtARP_Stickout.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.ARP.Stickout(pL));
                                    }
                                    //txtARP_Stickout.Text = mBearing_Radial_FP.ARP.Stickout().ToString("#0.000");
                                    txtARP_Stickout.ForeColor = Color.Blue;
                                }

                                break;


                            case "txtARP_Stickout":
                                //------------------------
                                //txtARP_Depth.ForeColor = Color.Blue;

                                //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                //{                                  
                                //        txtARP_Depth.ForeColor = Color.Blue;
                                //        txtARP_Depth.Text =modMain.gProject.PNR.Unit.CEng_Met( mBearing_Radial_FP.ARP.Dowel.Hole.Depth).ToString("#0.000");
                                //}
                                //else
                                //{
                                //        txtARP_Depth.Text = mBearing_Radial_FP.ARP.Dowel.Hole.Depth.ToString("#0.000");
                                //}

                                break;

                        #endregion


                        #region "Tab: Mounting:"
                        //-----------------  

                            case "txtMount_EndConfig_OD_Front":
                                //---------------------------
                                mEndPlate[0].OD = modMain.ConvTextToDouble(pTxtBox.Text);
                                mBearing_Radial_FP.Mount.EndPlateOD[0] = mEndPlate[0].OD;

                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    if (mEndPlate[0].OD > modMain.gcEPS)
                                    {
                                        lblMount_EndConfig_OD_Front_MM.Visible = true;
                                        lblMount_EndConfig_OD_Front_MM.Text = "(" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[0].OD)) + ")";
                                    }
                                    else
                                    {
                                        lblMount_EndConfig_OD_Front_MM.Visible = false;
                                    }
                                }
                                
                                //....Wall thick
                                txtMount_WallT_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing_Radial_FP.TWall_CB_EndPlate( 0));
                            
                                modMain.gProject.Product.Bearing =(clsBearing_Radial_FP) mBearing_Radial_FP.Clone();
                                
                                mMount_DBC_ULimit[0] = mBearing_Radial_FP.Mount.DBC_ULimit(0);
                                lblEndConfig_DBC_Ulim_Front.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(mMount_DBC_ULimit[0]);

                                double pDBC_Mean = (mBearing_Radial_FP.Mount.DBC_LLimit(modMain.gProject.Product, 0) + mBearing_Radial_FP.Mount.DBC_ULimit(0)) / 2;
                                txtEndConfig_DBC_Front.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(pDBC_Mean);

                                txtEndConfig_DBC_Front.BackColor = Color.White;

                                Double pValOD_Org = 0.0;
                                Double pValOD_Cur = 0.0;
                                
                                pValOD_Org = mEndPlate[0].OD_LLimit(modMain.gProject.Product, 0);
                                pValOD_Cur = modMain.ConvTextToDouble(txtMount_EndConfig_OD_Front.Text);

                                if (Math.Abs(pValOD_Org - pValOD_Cur) < mcEPS)
                                {
                                    txtMount_EndConfig_OD_Front.ForeColor = Color.Blue;
                                }
                                else
                                {
                                    txtMount_EndConfig_OD_Front.ForeColor = Color.Black;
                                }

                                SetBackColor_SealOD_Front();
                            
                                break;

                            case "txtMount_EndConfig_OD_Back":
                                //---------------------------
                                mEndPlate[1].OD = modMain.ConvTextToDouble(pTxtBox.Text);
                                mBearing_Radial_FP.Mount.EndPlateOD[1] = mEndPlate[1].OD;

                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    if (mEndPlate[1].OD > modMain.gcEPS)
                                    {
                                        lblMount_EndConfig_OD_Back_MM.Visible = true;
                                        lblMount_EndConfig_OD_Back_MM.Text = "(" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mEndPlate[1].OD)) + ")";
                                    }
                                    else
                                    {
                                        lblMount_EndConfig_OD_Back_MM.Visible = false;
                                    }
                                }

                                
                                //....Wall thick
                                txtMount_WallT_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing_Radial_FP.TWall_CB_EndPlate(1));
                               

                                //lblEndConfig_DBC_LLim_Back.Text = modMain.gProject.Product.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw_Hole_DBC_LLimit(1));
                                //lblEndConfig_DBC_Ulim_Back.Text = modMain.gProject.Product.Unit.WriteInUserL(mBearing_Radial_FP.Mount.Screw_Hole_DBC_ULimit(1));

                                modMain.gProject.Product.Bearing = (clsBearing_Radial_FP)mBearing_Radial_FP.Clone();
                                
                                mMount_DBC_ULimit[1] = mBearing_Radial_FP.Mount.DBC_ULimit( 1);
                                lblEndConfig_DBC_Ulim_Back.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(mMount_DBC_ULimit[1]);

                                double pDBC_Mean_Back = (mBearing_Radial_FP.Mount.DBC_LLimit(modMain.gProject.Product, 1) + mBearing_Radial_FP.Mount.DBC_ULimit(1)) / 2;
                                txtEndConfig_DBC_Back.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(pDBC_Mean_Back);

                                txtEndConfig_DBC_Back.BackColor = Color.White;

                                pValOD_Org = 0.0;
                                pValOD_Cur = 0.0;
                               
                                pValOD_Org = mEndPlate[1].OD_LLimit(modMain.gProject.Product, 1);
                                pValOD_Cur = modMain.ConvTextToDouble(txtMount_EndConfig_OD_Back.Text);

                                if (Math.Abs(pValOD_Org - pValOD_Cur) < mcEPS)
                                {
                                    txtMount_EndConfig_OD_Back.ForeColor = Color.Blue;
                                }
                                else
                                {
                                    txtMount_EndConfig_OD_Back.ForeColor = Color.Black;
                                }

                                SetBackColor_SealOD_Back();
                                break;

                            case "txtMount_WallT_Front":
                                //---------------------------
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    
                                    if (mBearing_Radial_FP.TWall_CB_EndPlate(0) > modMain.gcEPS)
                                    {
                                        lblMount_WallT_Front.Visible = true;
                                        lblMount_WallT_Front.Text = "(" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.TWall_CB_EndPlate(0))) + ")";
                                    }
                                    else
                                    {
                                        lblMount_WallT_Front.Visible = false;
                                    }
                                }

                                break;

                            case "txtMount_WallT_Back":
                                //---------------------------
                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    
                                    if (mBearing_Radial_FP.TWall_CB_EndPlate(1) > modMain.gcEPS)
                                    {
                                        lblMount_WallT_Back.Visible = true;
                                        lblMount_WallT_Back.Text = "(" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.TWall_CB_EndPlate(1))) + ")";
                                    }
                                    else
                                    {
                                        lblMount_WallT_Back.Visible = false;
                                    }
                                }
                                break;

                            case "txtEndConfig_DBC_Front":
                                //---------------------------
                                mBearing_Radial_FP.Mount.BC[0].D = modMain.ConvTextToDouble(pTxtBox.Text);

                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    if (mBearing_Radial_FP.Mount.BC[0].D > modMain.gcEPS)
                                    {
                                        lblEndConfig_DBC_Front.Visible = true;
                                        lblEndConfig_DBC_Front.Text = "(" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.BC[0].D)) + ")";
                                    }
                                    else
                                    {
                                        lblEndConfig_DBC_Front.Visible = false;
                                    }
                                }
                                                        

                                Double pULim = mMount_DBC_ULimit[0];
                                Double pLLim = mMount_DBC_LLimit[0];
                                Double pMean_Lim = 0.5 * (pULim + pLLim);

                                pVal = modMain.ConvTextToDouble(txtEndConfig_DBC_Front.Text);

                                if (Math.Abs(pVal - pMean_Lim) < mcEPS)
                                {
                                    txtEndConfig_DBC_Front.ForeColor = Color.Blue;
                                }
                                else
                                {
                                    txtEndConfig_DBC_Front.ForeColor = Color.Black;
                                }

                                SetBackColor_MountDBC_Front();

                                break;

                            case "txtEndConfig_DBC_Back":
                                //---------------------------
                                mBearing_Radial_FP.Mount.BC[1].D = modMain.ConvTextToDouble(pTxtBox.Text);

                                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                {
                                    if (mBearing_Radial_FP.Mount.BC[1].D > modMain.gcEPS)
                                    {
                                        lblEndConfig_DBC_Back.Visible = true;
                                        lblEndConfig_DBC_Back.Text = "(" + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.Mount.BC[1].D)) + ")";
                                    }
                                    else
                                    {
                                        lblEndConfig_DBC_Back.Visible = false;
                                    }
                                }

                                pULim = mMount_DBC_ULimit[1];
                                pLLim = mMount_DBC_LLimit[1];
                                pMean_Lim = 0.5 * (pULim + pLLim);

                                pVal = modMain.ConvTextToDouble(txtEndConfig_DBC_Back.Text);

                                if (Math.Abs(pVal - pMean_Lim) < mcEPS)
                                {
                                    txtEndConfig_DBC_Back.ForeColor = Color.Blue;
                                }
                                else
                                {
                                    txtEndConfig_DBC_Back.ForeColor = Color.Black;
                                }

                                SetBackColor_MountDBC_Back();
                                break;
                          

                            case "txtMount_HolesAngStart_Front":
                                //------------------------------

                                if (mBearing_Radial_FP.Mount.BC[0].Count == 4 && modMain.ConvTextToDouble(pTxtBox.Text) == 45)
                                {
                                    pTxtBox.ForeColor = Color.Blue;
                                }
                                else
                                {
                                    pTxtBox.ForeColor = Color.Black;
                                }

                                break;

                            case "txtMount_HolesAngStart_Back":
                                //-----------------------------
                                if (mBearing_Radial_FP.Mount.BC[1].Count == 4 && modMain.ConvTextToDouble(pTxtBox.Text) == 45)
                                {
                                    pTxtBox.ForeColor = Color.Blue;
                                }
                                else
                                {
                                    pTxtBox.ForeColor = Color.Black;
                                }
                                break;


                            case "txtMount_HolesAngBet1_Front":
                                //---------------------------------------
                                if (!mBearing_Radial_FP.Mount.BC[0].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[0].AngBet[0] = modMain.ConvTextToDouble(txtMount_HolesAngBet1_Front.Text);
                                }
                               
                                break;

                            case "txtMount_HolesAngBet1_Back":
                                //-------------------------------------
                                if (!mBearing_Radial_FP.Mount.BC[1].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[1].AngBet[0] =
                                                                        modMain.ConvTextToDouble(txtMount_HolesAngBet1_Back.Text);

                                }
                                break;

                            case "txtMount_HolesAngBet2_Front":
                                //----------------------------------------
                                if (!mBearing_Radial_FP.Mount.BC[0].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[0].AngBet[1] =
                                                                        modMain.ConvTextToDouble(txtMount_HolesAngBet2_Front.Text);
                                }
                                break;

                            case "txtMount_HolesAngBet2_Back":
                                //--------------------------------------
                                if (!mBearing_Radial_FP.Mount.BC[1].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[1].AngBet[1] =
                                                                        modMain.ConvTextToDouble(txtMount_HolesAngBet2_Back.Text);
                                }
                                break;


                            case "txtMount_HolesAngBet3_Front":
                                //--------------------------
                                if (!mBearing_Radial_FP.Mount.BC[0].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[0].AngBet[2] =
                                                                        modMain.ConvTextToDouble(txtMount_HolesAngBet3_Front.Text);
                                }

                                break;

                            case "txtMount_HolesAngBet3_Back":
                                //-----------------------------
                                if (!mBearing_Radial_FP.Mount.BC[1].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[1].AngBet[2] =
                                                                        modMain.ConvTextToDouble(txtMount_HolesAngBet3_Back.Text);
                                }

                                break;

                            case "txtMount_HolesAngBet4_Front":
                                //-------------------------------------
                                if (!mBearing_Radial_FP.Mount.BC[0].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[0].AngBet[3] =
                                                                        modMain.ConvTextToDouble(txtMount_HolesAngBet4_Front.Text);
                                }
                                break;

                            case "txtMount_HolesAngBet4_Back":
                                //--------------------------------------
                                if (!mBearing_Radial_FP.Mount.BC[1].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[1].AngBet[3] =
                                                                        modMain.ConvTextToDouble(txtMount_HolesAngBet4_Back.Text);
                                }
                                break;

                            case "txtMount_HolesAngBet5_Front":
                                //--------------------------
                                if (!mBearing_Radial_FP.Mount.BC[0].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[0].AngBet[4] =
                                                                        modMain.ConvTextToDouble(txtMount_HolesAngBet_Front.Text);
                                }

                                break;

                            case "txtMount_HolesAngBet5_Back":
                                //--------------------------------------
                                if (!mBearing_Radial_FP.Mount.BC[1].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[1].AngBet[4] =
                                                                        modMain.ConvTextToDouble(txtMount_HolesAngBet5_Back.Text);
                                }
                                break;

                            case "txtMount_HolesAngBet6_Front":
                                //--------------------------
                                if (!mBearing_Radial_FP.Mount.BC[0].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[0].AngBet[5] =
                                                                        modMain.ConvTextToDouble(txtMount_HolesAngBet6_Front.Text);
                                }

                                break;

                            case "txtMount_HolesAngBet6_Back":
                                //--------------------------------------
                                if (!mBearing_Radial_FP.Mount.BC[1].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[1].AngBet[5] = modMain.ConvTextToDouble(txtMount_HolesAngBet6_Back.Text);
                                }
                                break;

                            case "txtMount_HolesAngBet7_Front":
                                //-------------------------------------
                                if (!mBearing_Radial_FP.Mount.BC[0].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[0].AngBet[6] =
                                                                        modMain.ConvTextToDouble(txtMount_HolesAngBet7_Front.Text);
                                }
                                break;

                            case "txtMount_HolesAngBet7_Back":
                                //---------------------------------------
                                if (!mBearing_Radial_FP.Mount.BC[1].EquiSpaced)
                                {
                                    mBearing_Radial_FP.Mount.BC[1].AngBet[6] =
                                                                        modMain.ConvTextToDouble(txtMount_HolesAngBet7_Back.Text);
                                }
                                break;

                        

                        #endregion


                        //#region "TempSensor:"

                        //    case "txtTempSensor_AngStart":
                        //        //------------------------------
                        //        mBearing_Radial_FP.TempSensor.AngStart =
                        //            modMain.ConvTextToDouble(txtTempSensor_AngStart.Text);

                        //        break;


                        //    case "txtTempSensor_CanLength":
                        //     //-------------------------   
                        //        ////mBearing_Radial_FP.TempSensor.CanLength = modMain.ConvTextToDouble(pTxtBox.Text);
                        //        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        //        {
                        //            mBearing_Radial_FP.TempSensor.CanLength = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pTxtBox.Text));
                        //        }
                        //        else
                        //        {
                        //            mBearing_Radial_FP.TempSensor.CanLength = modMain.ConvTextToDouble(pTxtBox.Text);
                        //        }
                                 

                        //        if (Math.Abs(mBearing_Radial_FP.TempSensor.CanLength - mBearing_Radial_FP.TempSensor.CAN_LENGTH) < modMain.gcEPS)
                        //        {
                        //            txtTempSensor_CanLength.ForeColor = Color.Magenta;
                        //        }
                        //        else
                        //        {
                        //            txtTempSensor_CanLength.ForeColor = Color.Black;
                        //        }

                        //        if (mblnTempSensor_CanLength_ManuallyChanged)
                        //        {
                        //            txtTempSensor_Depth.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.TempSensor.Calc_Depth(),
                        //                                                                         modMain.gUnit.MFormat);
                        //            txtTempSensor_Depth.ForeColor = Color.Blue;
                        //            mblnTempSensor_CanLength_ManuallyChanged= false;
                        //        }

                        //        SetColor_TempSensor_Depth();
                        //        break;


                        //    case "txtTempSensor_Depth":
                        //    //-------------------------
                        //        mBearing_Radial_FP.TempSensor.Depth = modMain.ConvTextToDouble(pTxtBox.Text);
                        //        SetColor_TempSensor_Depth();
                        //        break;

                        //#endregion
                  
                    }
                }

                private void txtOilInlet_Annulus_Wid_TextChanged(object sender, EventArgs e)
                //===========================================================================
                {
                    mBearing_Radial_FP.OilInlet.Annulus_Wid = modMain.gProject.Product.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Annulus_Wid.Text));
                    if (chkOilInlet_Annulus_Wid.Checked)
                    {
                        Calc_Depth_Dia(mBearing_Radial_FP.OilInlet.Annulus.Wid);
                    }
                    Calc_OilInlet_Annulus_Loc(mBearing_Radial_FP.OilInlet.Annulus.Wid);
                    double pArea_Act = mBearing_Radial_FP.OilInlet.Annulus.Wid * mBearing_Radial_FP.OilInlet.Annulus.Depth;
                    txtOilInlet_Annulus_Area_Act.Text = modMain.gProject.Product.Unit.WriteInUserL(modMain.NInt(modMain.gProject.Product.Unit.CFac_Area_EngToMet(pArea_Act)));


                    double pReqd = modMain.ConvTextToDouble(txtOilInlet_Annulus_Area_Reqd.Text);
                    double pAct = modMain.ConvTextToDouble(txtOilInlet_Annulus_Area_Act.Text);

                    if (pAct < pReqd)
                    {
                        txtOilInlet_Annulus_Area_Act.BackColor = Color.Red;
                    }
                    else
                    {
                        txtOilInlet_Annulus_Area_Act.BackColor = Color.White;
                    }

                    double pWD = mBearing_Radial_FP.OilInlet.Annulus.Wid / mBearing_Radial_FP.OilInlet.Annulus.Depth;
                    txtOilInlet_Annulus_WD.Text = pWD.ToString("#0.0");
                }

                private Double Calc_OilInlet_Annulus_Loc(Double Wid_In)
                //===================================================
                {
                    double pLoc = 0;
                    if (Wid_In>mcEPS)
                    {
                        pLoc = 0.5*(mBearing_Radial_FP.L - Wid_In);
                        txtOilInlet_Annulus_Loc_Back.Text =modMain.gProject.Product.Unit.WriteInUserL( modMain.gProject.Product.Unit.CEng_Met(pLoc));
                        txtOilInlet_Annulus_Loc_Back.ForeColor = Color.Blue;
                    }
                    return pLoc;
                }

                //private Double Calc_Wid(Double Loc_In)
                ////====================================
                //{
                //    double pWid = 0;
                //    if (Loc_In > mcEPS)
                //    {
                //        pWid = (mBearing_Radial_FP.L - 2*Loc_In);
                //        txtOilInlet_Annulus_Wid.Text = modMain.gProject.Product.Unit.WriteInUserL(modMain.gProject.Product.Unit.CEng_Met(pWid));
                //    }
                //    return pWid;
                //}

                private void Calc_Depth_Dia(Double Wid_In)
                //=======================================
                {
                    double pDepth = 0;
                    double pDia = 0;
                    if (Wid_In > modMain.gcEPS)
                    {
                        pDepth = mBearing_Radial_FP.OilInlet.Annulus.Area / Wid_In;
                        pDia = mBearing_Radial_FP.OD() - 2 * pDepth;
                        txtOilInlet_Annulus_Depth.Text = modMain.gProject.Product.Unit.WriteInUserL(modMain.NInt(modMain.gProject.Product.Unit.CEng_Met(pDepth)));
                        txtOilInlet_Annulus_Dia.Text = modMain.gProject.Product.Unit.WriteInUserL(modMain.NInt(modMain.gProject.Product.Unit.CEng_Met(pDia)));
                        txtOilInlet_Annulus_Depth.ForeColor = Color.Blue;
                        txtOilInlet_Annulus_Depth.ReadOnly = false;
                        txtOilInlet_Annulus_Dia.ForeColor = Color.Blue;
                        txtOilInlet_Annulus_Dia.ReadOnly = false;
                    }
                    else
                    {
                        txtOilInlet_Annulus_Depth.ForeColor = Color.Black;
                        txtOilInlet_Annulus_Depth.ReadOnly = true;
                        txtOilInlet_Annulus_Dia.ForeColor = Color.Black;
                        txtOilInlet_Annulus_Dia.ReadOnly = true;
                    }
                }

                private void txtOilInlet_Annulus_Depth_TextChanged(object sender, EventArgs e)
                //==============================================================================
                {
                    double pWid = 0;
                    double pDia = 0;
                    mBearing_Radial_FP.OilInlet.Annulus_Depth = modMain.gProject.Product.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Annulus_Depth.Text));
                    if (chkOilInlet_Annulus_Wid.Checked)
                    {
                        if (mblnAnnulus_Depth_ManuallyChanged)
                        {
                            pDia = mBearing_Radial_FP.OD() - 2 * mBearing_Radial_FP.OilInlet.Annulus.Depth;
                            txtOilInlet_Annulus_Dia.Text = modMain.gProject.Product.Unit.WriteInUserL(modMain.NInt(modMain.gProject.Product.Unit.CEng_Met(pDia)));
                            txtOilInlet_Annulus_Depth.ForeColor = Color.Black;
                            txtOilInlet_Annulus_Dia.ForeColor = Color.Blue;
                            txtOilInlet_Annulus_Dia.ReadOnly = false;
                        }
                    }
                    else
                    {
                        if (mblnAnnulus_Depth_ManuallyChanged)
                        {
                            pWid = mBearing_Radial_FP.OilInlet.Annulus.Area / mBearing_Radial_FP.OilInlet.Annulus.Depth;
                            pDia = mBearing_Radial_FP.OD() - 2 * mBearing_Radial_FP.OilInlet.Annulus.Depth;
                            txtOilInlet_Annulus_Depth.ForeColor = Color.Black;
                            txtOilInlet_Annulus_Wid.Text = modMain.gProject.Product.Unit.WriteInUserL(modMain.NInt(modMain.gProject.Product.Unit.CEng_Met(pWid)));
                            txtOilInlet_Annulus_Dia.Text = modMain.gProject.Product.Unit.WriteInUserL(modMain.NInt(modMain.gProject.Product.Unit.CEng_Met(pDia)));
                            txtOilInlet_Annulus_Dia.ForeColor = Color.Blue;
                            txtOilInlet_Annulus_Dia.ReadOnly = false;
                        }
                    }

                    double pArea_Act = mBearing_Radial_FP.OilInlet.Annulus.Wid * mBearing_Radial_FP.OilInlet.Annulus.Depth;
                    txtOilInlet_Annulus_Area_Act.Text = modMain.gProject.Product.Unit.WriteInUserL(modMain.NInt(modMain.gProject.Product.Unit.CFac_Area_EngToMet(pArea_Act)));


                    double pReqd = modMain.ConvTextToDouble(txtOilInlet_Annulus_Area_Reqd.Text);
                    double pAct = modMain.ConvTextToDouble(txtOilInlet_Annulus_Area_Act.Text);
                    if (pAct < pReqd)
                    {
                        txtOilInlet_Annulus_Area_Act.BackColor = Color.Red;
                    }
                    else
                    {
                        txtOilInlet_Annulus_Area_Act.BackColor = Color.White;
                    }

                    double pWD = mBearing_Radial_FP.OilInlet.Annulus.Wid / mBearing_Radial_FP.OilInlet.Annulus.Depth;
                    txtOilInlet_Annulus_WD.Text = pWD.ToString("#0.0");
                }

                private void txtOilInlet_Annulus_Dia_TextChanged(object sender, EventArgs e)
                //===========================================================================
                {
                    double pWid = 0;
                    double pDepth = 0;
                    mBearing_Radial_FP.OilInlet.Annulus_D = modMain.gProject.Product.Unit.CMet_Eng(modMain.ConvTextToDouble(txtOilInlet_Annulus_Dia.Text));
                    if (chkOilInlet_Annulus_Wid.Checked)
                    {
                        if (mblnAnnulus_Dia_ManuallyChanged)
                        {
                            pDepth = (mBearing_Radial_FP.OD() - mBearing_Radial_FP.OilInlet.Annulus.D) / 2;
                            txtOilInlet_Annulus_Dia.ForeColor = Color.Black;
                            txtOilInlet_Annulus_Depth.Text = modMain.gProject.Product.Unit.WriteInUserL(modMain.NInt(modMain.gProject.Product.Unit.CEng_Met(pDepth)));
                            txtOilInlet_Annulus_Depth.ForeColor = Color.Blue;
                            txtOilInlet_Annulus_Depth.ReadOnly = false;
                        }
                    }
                    else
                    {
                        if (mblnAnnulus_Dia_ManuallyChanged)
                        {
                            pDepth = (mBearing_Radial_FP.OD() - mBearing_Radial_FP.OilInlet.Annulus.D) / 2;
                            pWid = mBearing_Radial_FP.OilInlet.Annulus.Area / pDepth;
                            txtOilInlet_Annulus_Dia.ForeColor = Color.Black;
                            txtOilInlet_Annulus_Wid.Text = modMain.gProject.Product.Unit.WriteInUserL(modMain.NInt(modMain.gProject.Product.Unit.CEng_Met(pWid)));
                            txtOilInlet_Annulus_Depth.Text = modMain.gProject.Product.Unit.WriteInUserL(modMain.NInt(modMain.gProject.Product.Unit.CEng_Met(pDepth)));
                            txtOilInlet_Annulus_Depth.ForeColor = Color.Blue;
                            txtOilInlet_Annulus_Depth.ReadOnly = false;
                        }
                    }
                    double pArea_Act = mBearing_Radial_FP.OilInlet.Annulus.Wid * mBearing_Radial_FP.OilInlet.Annulus.Depth;
                    txtOilInlet_Annulus_Area_Act.Text = modMain.gProject.Product.Unit.WriteInUserL(modMain.NInt(modMain.gProject.Product.Unit.CFac_Area_EngToMet(pArea_Act)));


                    double pReqd = modMain.ConvTextToDouble(txtOilInlet_Annulus_Area_Reqd.Text);
                    double pAct = modMain.ConvTextToDouble(txtOilInlet_Annulus_Area_Act.Text);

                    if (pAct < pReqd)
                    {
                        txtOilInlet_Annulus_Area_Act.BackColor = Color.Red;
                    }
                    else
                    {
                        txtOilInlet_Annulus_Area_Act.BackColor = Color.White;
                    }

                    double pWD = mBearing_Radial_FP.OilInlet.Annulus.Wid / mBearing_Radial_FP.OilInlet.Annulus.Depth;
                    txtOilInlet_Annulus_WD.Text = pWD.ToString("#0.0");
                }

                //BG 28MAR13  As per HK's instruction in email dated 27MAR13.
                private void TextBox_Validating(object sender, CancelEventArgs e)
                //================================================================
                {
                    TextBox pTxtBox = (TextBox)sender;
                    Double pVal = 0.0;

                    switch (pTxtBox.Name)
                    {
                        case "txtDepth_EndConfig_Front":
                            //------------------------------
                            Double pPreVal = mBearing_Radial_FP.Depth_EndPlate[0];

                            //if (mblnL_ManuallyChanged)    //PB 17JAN13
                            //{
                            //    mBearing_Radial_FP.Depth_EndConfig[0] = modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text);  
                            //}

                            if (mblnDepth_EndPlate_F_ManuallyChanged && txtDepth_EndConfig_Front.Text != "")
                            {
                                if (!mblnDepth_EndPlate_B_ManuallyChanged)
                                {
                                    double pDepthF;
                                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        //....Retrieve from Text Box.
                                        pDepthF = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text));
                                    }
                                    else
                                    {
                                        //....Retrieve from Text Box.
                                        pDepthF = modMain.ConvTextToDouble(txtDepth_EndConfig_Front.Text);
                                    }                               

                                    //....Assign. 
                                    mBearing_Radial_FP.Depth_EndPlate[0] = pDepthF;

                                    //....Update the Depth Back.
                                    Update_Depth_EndConfig(txtDepth_EndConfig_Front, txtDepth_EndConfig_Back);

                                    //  Reset the state. 
                                    //  ---------------
                                    mblnDepth_EndPlate_F_ManuallyChanged = false;
                                }

                                if (Math.Abs(pPreVal - mBearing_Radial_FP.Depth_EndPlate[0]) > modMain.gcEPS)
                                {
                                    txtOilInlet_Orifice_Loc_BackFace.ForeColor = Color.Blue;
                                }
                            }
                            break;

                        case "txtDepth_EndConfig_Back":
                            //-------------------------
                            //if (mblnL_ManuallyChanged)    //PB 17JAN13
                            //{
                            //    mBearing_Radial_FP.Depth_EndConfig[1] = modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text);
                            //}

                            if (mblnDepth_EndPlate_B_ManuallyChanged && txtDepth_EndConfig_Back.Text != "")
                            {
                                if (!mblnDepth_EndPlate_F_ManuallyChanged)
                                {
                                    //....Retrieve from Text Box.
                                    double pDepthB;
                                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pDepthB = modMain.gProject.PNR.Unit.CEng_Met(modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text));
                                    }
                                    else
                                    {
                                        pDepthB = modMain.ConvTextToDouble(txtDepth_EndConfig_Back.Text);
                                    }

                                    //....Assign. 
                                    mBearing_Radial_FP.Depth_EndPlate[1] = pDepthB;

                                    //....Update the Depth Front.
                                    Update_Depth_EndConfig(txtDepth_EndConfig_Back, txtDepth_EndConfig_Front);
                                }

                                //  Reset the state. 
                                //  ---------------
                                mblnDepth_EndPlate_B_ManuallyChanged = false;
                            }
                            break;

                            //case "txtSL_LScrew_Loc_Center":

                            //     mBearing_Radial_FP.SL.LScrew_Center = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_LScrew_Loc_Center.Text));

                            //    if (mblnSL_LScrew_Loc_Center_ManuallyChanged && txtSL_LScrew_Loc_Center.Text != "")
                            //    {
                            //        Double pLScrew_Loc_Center_ULimit = mBearing_Radial_FP.SL.Screw_Loc_Center_ULimit();

                            //        if (mBearing_Radial_FP.SL.LScrew.Center > pLScrew_Loc_Center_ULimit)
                            //        {
                            //            txtSL_LScrew_Loc_Center.BackColor = Color.Red;
                            //        }
                            //        else
                            //        {
                            //            txtSL_LScrew_Loc_Center.BackColor = Color.White;
                            //        }

                            //        mblnSL_LScrew_Loc_Center_ManuallyChanged = false;
                            //    }
                                 
                            //break;

                            //case "txtSL_RScrew_Loc_Center":

                            //mBearing_Radial_FP.SL.RScrew_Center = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_RScrew_Loc_Center.Text));

                            //if (mblnSL_RScrew_Loc_Center_ManuallyChanged && txtSL_RScrew_Loc_Center.Text != "")
                            //{
                            //    Double pRScrew_Loc_Center_ULimit = mBearing_Radial_FP.SL.Screw_Loc_Center_ULimit();

                            //    if (mBearing_Radial_FP.SL.RScrew.Center > pRScrew_Loc_Center_ULimit)
                            //    {
                            //        txtSL_RScrew_Loc_Center.BackColor = Color.Red;
                            //    }
                            //    else
                            //    {
                            //        txtSL_RScrew_Loc_Center.BackColor = Color.White;
                            //    }

                            //    mblnSL_RScrew_Loc_Center_ManuallyChanged = false;
                            //}

                            //break;

                            //case "txtMount_EndConfig_OD_Front":
                                
                            //    if (mblnMount_EndConfig_OD_Front && txtMount_EndConfig_OD_Front.Text != "")
                            //    {
                            //        Double pLLimit = Math.Round(mEndPlate_OD_LLimit[0], 4);
                            //        Double pULimit = Math.Round(mEndPlate_OD_ULimit[0], 4);

                            //        Double pVal_Front_End_Config_OD = modMain.ConvTextToDouble(txtMount_EndConfig_OD_Front.Text);

                            //        if (pVal_Front_End_Config_OD < pLLimit || pVal_Front_End_Config_OD > pULimit)
                            //        {
                            //            txtMount_EndConfig_OD_Front.BackColor = Color.Red;
                            //        }
                            //        else
                            //        {
                            //            txtMount_EndConfig_OD_Front.BackColor = Color.White;
                            //        }

                            //        mblnMount_EndConfig_OD_Front = false;
                            //    }

                            //break;

                            //case "txtMount_EndConfig_OD_Back":

                            //    if (mblnMount_EndConfig_OD_Back && txtMount_EndConfig_OD_Back.Text != "")
                            //    {
                            //        Double pLLimit = Math.Round(mEndPlate_OD_LLimit[1], 4);
                            //        Double pULimit = Math.Round(mEndPlate_OD_ULimit[1], 4);  

                            //        Double pVal_Back_End_Config_OD = modMain.ConvTextToDouble(txtMount_EndConfig_OD_Back.Text);

                            //        if (pVal_Back_End_Config_OD < pLLimit || pVal_Back_End_Config_OD > pULimit)
                            //        {
                            //            txtMount_EndConfig_OD_Back.BackColor = Color.Red;
                            //        }
                            //        else
                            //        {
                            //            txtMount_EndConfig_OD_Back.BackColor = Color.White;
                            //        }

                            //        mblnMount_EndConfig_OD_Back = false;
                            //    }

                            //break;

                            //case "txtEndConfig_DBC_Front":
                            //    if (mblnEndConfig_DBC_Front && txtEndConfig_DBC_Front.Text != "")
                            //    {
                            //        Double pLLimit = Math.Round(mMount_DBC_LLimit[0], 4);
                            //        Double pULimit = Math.Round(mMount_DBC_ULimit[0], 4);

                            //        Double pVal_End_Config_DBC = modMain.ConvTextToDouble(txtEndConfig_DBC_Front.Text);

                            //        if (pVal_End_Config_DBC < pLLimit || pVal_End_Config_DBC > pULimit)
                            //        {
                            //            txtEndConfig_DBC_Front.BackColor = Color.Red;
                            //        }
                            //        else
                            //        {
                            //            txtEndConfig_DBC_Front.BackColor = Color.White;
                            //        }

                            //        mblnEndConfig_DBC_Front = false;
                            //    }
                            //break;

                           // case "txtEndConfig_DBC_Back":
                           //     if (mblnEndConfig_DBC_Back && txtEndConfig_DBC_Back.Text != "")
                           //     {
                           //         Double pLLimit = Math.Round(mMount_DBC_LLimit[0], 4);
                           //         Double pULimit = Math.Round(mMount_DBC_ULimit[0], 4);

                           //         Double pVal_End_Config_DBC = modMain.ConvTextToDouble(txtEndConfig_DBC_Back.Text);

                           //         if (pVal_End_Config_DBC < pLLimit || pVal_End_Config_DBC > pULimit)
                           //         {
                           //             txtEndConfig_DBC_Back.BackColor = Color.Red;
                           //         }
                           //         else
                           //         {
                           //             txtEndConfig_DBC_Back.BackColor = Color.White;
                           //         }

                           //         mblnMount_EndConfig_OD_Front = false;
                           //     }
                           //break;
                    }
                }

                

                private void txtMount_EndConfig_OD_Front_MouseEnter(object sender, EventArgs e)
                //==============================================================================
                {
                    string pLLimit = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mEndPlate_OD_LLimit[0]);// OD_LLimit(0));
                    string pUlimit = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mEndPlate_OD_ULimit[0]);   //OD_ULimit());
                    string pText = "(" + pLLimit + ", " + pUlimit + ")  ";
                    toolTip2.ForeColor = Color.Blue;
                    toolTip2.SetToolTip(txtMount_EndConfig_OD_Front, pText);
                }

                private void txtMount_EndConfig_OD_Back_MouseEnter(object sender, EventArgs e)
                //=============================================================================
                {
                    string pLLimit = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mEndPlate_OD_LLimit[1]);
                    string pUlimit = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mEndPlate_OD_ULimit[1]);
                    string pText = "(" + pLLimit + ", " + pUlimit + ")  ";
                    toolTip2.ForeColor = Color.Blue;
                    toolTip2.SetToolTip(txtMount_EndConfig_OD_Back, pText);
                }

                private void SetBackColor_SealOD_Front()
                //======================================
                {
                    Double pLLimit = Math.Round(mEndPlate_OD_LLimit[0], 4);
                    Double pULimit = Math.Round(mEndPlate_OD_ULimit[0], 4);

                    if (mEndPlate[0].OD < pLLimit || mEndPlate[0].OD > pULimit)
                    {
                        txtMount_EndConfig_OD_Front.BackColor = Color.Red;
                    }
                    else
                    {
                        txtMount_EndConfig_OD_Front.BackColor = Color.White;
                    }
                }

                private void SetBackColor_SealOD_Back()
                //======================================
                {
                    Double pLLimit = Math.Round(mEndPlate_OD_LLimit[1], 4);
                    Double pULimit = Math.Round(mEndPlate_OD_ULimit[1], 4);

                    if (mEndPlate[1].OD < pLLimit || mEndPlate[1].OD > pULimit)
                    {
                        txtMount_EndConfig_OD_Back.BackColor = Color.Red;
                    }
                    else
                    {
                        txtMount_EndConfig_OD_Back.BackColor = Color.White;
                    }
                }

                private void SetBackColor_MountDBC_Front()
                //======================================
                {
                    Double pLLimit = Math.Round(mBearing_Radial_FP.Mount.DBC_LLimit(modMain.gProject.Product, 0), 4);
                    Double pULimit = Math.Round(mBearing_Radial_FP.Mount.DBC_ULimit(0), 4);

                    if (mBearing_Radial_FP.Mount.BC[0].D < pLLimit || mBearing_Radial_FP.Mount.BC[0].D > pULimit)
                    {
                        txtEndConfig_DBC_Front.BackColor = Color.Red;
                    }
                    else
                    {
                        txtEndConfig_DBC_Front.BackColor = Color.White;
                    }
                }

                private void SetBackColor_MountDBC_Back()
                //======================================
                {
                    Double pLLimit = Math.Round(mBearing_Radial_FP.Mount.DBC_LLimit(modMain.gProject.Product, 1), 4);
                    Double pULimit = Math.Round(mBearing_Radial_FP.Mount.DBC_ULimit(1), 4);

                    if (mBearing_Radial_FP.Mount.BC[1].D < pLLimit || mBearing_Radial_FP.Mount.BC[1].D > pULimit)
                    {
                        txtEndConfig_DBC_Back.BackColor = Color.Red;
                    }
                    else
                    {
                        txtEndConfig_DBC_Back.BackColor = Color.White;
                    }
                }
            

                private void txtSL_LScrew_Loc_Center_MouseEnter(object sender, EventArgs e)
                //==========================================================================
                {
                    string pText = "(ULimit: " + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw_Loc_Center_ULimit())) + ")  ";
                    toolTip2.ForeColor = Color.Blue;
                    toolTip2.SetToolTip(txtSL_LScrew_Loc_Center, pText);
                }

                private void txtSL_RScrew_Loc_Center_MouseEnter(object sender, EventArgs e)
                //=========================================================================
                {
                    string pText = "(ULimit: " + modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Screw_Loc_Center_ULimit())) + ")  ";
                    toolTip2.ForeColor = Color.Blue;
                    toolTip2.SetToolTip(txtSL_RScrew_Loc_Center, pText);
                }

                private void toolTip2_Draw(object sender, DrawToolTipEventArgs e)
                //================================================================
                {
                    Font f = new Font("Verdana", 8.0f);
                    //toolTip2.ForeColor = System.Drawing.Color.Blue;
                    e.DrawBackground();
                    e.DrawBorder();
                    e.Graphics.DrawString(e.ToolTipText, f, Brushes.Red, new PointF(6, 2));

                }

                ////private void toolTip2_Popup(object sender, PopupEventArgs e)
                //////================================================================
                ////{
                ////    string pLLimit = modMain.gProject.PNR.Unit.WriteInUserL_Eng(OD_LLimit(1));
                ////    string pUlimit = modMain.gProject.PNR.Unit.WriteInUserL_Eng(OD_ULimit());
                ////    string pText = "(" + pLLimit + ", " + pUlimit + ")";
                ////    e.ToolTipSize = TextRenderer.MeasureText(pText, new Font("Verdana", 9.5f));
                ////}

                #region "Helper Routines:"
                //************************                  

                    private void SetForeColor_Depth_EndConfig(TextBox TxtBox_In, int Indx_In)
                    //========================================================================
                    {
                        if (Math.Abs(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Depth_EndPlate[Indx_In] -
                                                           mBearing_Radial_FP.Depth_EndPlate[Indx_In]) < modMain.gcEPS)
                        {
                            TxtBox_In.ForeColor = Color.Blue;
                        }
                        else
                        {
                            TxtBox_In.ForeColor = Color.Black;
                        }
                    }


                    private void Update_Depth_EndConfig(TextBox Txt_In, TextBox Txt_Out)
                    //===================================================================
                    {
                        Double pDepth_Tot = mBearing_Radial_FP.L - (mBearing_Radial_FP.Pad.L + 
                                                                    mBearing_Radial_FP.MillRelief.AxialSealGap[0] + 
                                                                    mBearing_Radial_FP.MillRelief.AxialSealGap[1]);  
                      
                        if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                        {

                            Double pDepth_Other = pDepth_Tot -modMain.gProject.PNR.Unit.CMet_Eng( modMain.ConvTextToDouble(Txt_In.Text));
                            Txt_Out.Text = modMain.gProject.PNR.Unit.CEng_Met(pDepth_Other).ToString("#0.000");

                            //if (pDepth_Other >= mBearing_Radial_FP.DEPTH_END_CONFIG_MIN)
                            //{
                            //    Txt_Out.ForeColor = Color.Blue;
                            //}
                            //else
                            //{
                            //    Txt_Out.ForeColor = Color.Red;
                            //}
                        }
                        else
                        {
                            Double pDepth_Other = pDepth_Tot - modMain.ConvTextToDouble(Txt_In.Text);
                            Txt_Out.Text = pDepth_Other.ToString("#0.000");

                            //if (pDepth_Other >= mBearing_Radial_FP.DEPTH_END_CONFIG_MIN)
                            //{
                            //    Txt_Out.ForeColor = Color.Blue;
                            //}
                            //else
                            //{
                            //    Txt_Out.ForeColor = Color.Red;
                            //}
                        }
                    }
                #endregion

            #endregion


            #region "TAB CONTROL RELATED ROUTINE"
            //===================================              
            
                private void tbBearingDesignDetails_SelectedIndexChanged(object sender, EventArgs e)
                //==================================================================================
                {
                    //  Index Mounting Tab is 5
                    //  -------------------------

                    //mSealOD_ULimit = new double[2];

                    //if (tbBearingDesignDetails.SelectedIndex == 4)
                    ////--------------------------------------------
                    //{
                    //    for (int i = 0; i < 2; i++)
                    //    {
                    //        mSealOD_ULimit[i] = mEndPlate[i].OD_ULimit();   // PB 21OCT18. BG, you may move this assignment to the form load event. Place at proper place.
                    //    }
                    //    ////Upadate_Fixture_Selection(mBearing_Radial_FP, false);                       

                    //}   
                    SaveData();
                
                }

                private void tabControl_Mount_Holes_SelectedIndexChanged(object sender, EventArgs e)
                //===================================================================================
                {
                    SaveData();

                    if (tabControl_Mount_Holes.SelectedIndex == 1)
                    {
                        int pAns = (int)MessageBox.Show("Do you want Front Mount Data to be copied on to Back Mount Data?", "Mount Data Copying",
                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        const int pAnsY = 6;    //....Integer value of MessageBoxButtons.Yes.

                        if (pAns == pAnsY)
                        {
                            Cursor = Cursors.WaitCursor;
                            mblnMount_Front_Copy = true;
                            mBearing_Radial_FP.Mount.Screw[1] = (clsScrew)((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Clone();
                            //mBearing_Radial_FP.Mount.EndPlateOD[1] = mBearing_Radial_FP.Mount.EndPlateOD[0];
                            //mBearing_Radial_FP.Mount.BC[1] = mBearing_Radial_FP.Mount.BC[0];
                            mBearing_Radial_FP.Mount.EndPlateOD[1] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.EndPlateOD[0];
                            mBearing_Radial_FP.Mount.BC[0] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0];
                            mBearing_Radial_FP.Mount.BC[1] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0];
                            mEndPlate[1].OD = mBearing_Radial_FP.Mount.EndPlateOD[1];
                            DisplayData();                          
                            tbBearingDesignDetails.SelectedIndex = 5;
                            tabControl_Mount_Holes.Select();
                            tabBack.Select();
                            tabControl_Mount_Holes.SelectedIndex = 1;
                            Cursor = Cursors.Default;
                        }
                        else
                        {
                            mblnMount_Front_Copy = false;
                        }
                    }
                    else
                    {
                        mblnMount_Front_Copy = false;
                    }

                }           

            #endregion
        

            #region"GROUP BOX RELATED ROUTINE:"
            //----------------------------

                private void grpScrew_Paint(object sender, PaintEventArgs e)
                //===========================================================
                {
                    Graphics gfx = e.Graphics;
                    Pen p = new Pen(Color.White, 1);
                    gfx.DrawLine(p, 0, 5, 0, e.ClipRectangle.Height - 2);
                    gfx.DrawLine(p, 0, 5, 10, 5);
                    gfx.DrawLine(p, 0, 5, e.ClipRectangle.Width - 2, 5);
                    gfx.DrawLine(p, e.ClipRectangle.Width - 2, 5, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
                    gfx.DrawLine(p, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2);
                }

                private void groDowel_Paint(object sender, PaintEventArgs e)
                //===========================================================
                {
                    Graphics gfx = e.Graphics;
                    Pen p = new Pen(Color.White, 1);
                    gfx.DrawLine(p, 0, 5, 0, e.ClipRectangle.Height - 2);
                    gfx.DrawLine(p, 0, 5, 10, 5);
                    gfx.DrawLine(p, 0, 5, e.ClipRectangle.Width - 2, 5);
                    gfx.DrawLine(p, e.ClipRectangle.Width - 2, 5, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
                    gfx.DrawLine(p, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2);
                }

                private void grpMount_Front_Paint(object sender, PaintEventArgs e)
                //========================================================================
                {
                    Graphics gfx = e.Graphics;
                    Pen p = new Pen(Color.White, 1);
                    gfx.DrawLine(p, 0, 5, 0, e.ClipRectangle.Height - 2);
                    gfx.DrawLine(p, 0, 5, 10, 5);
                    gfx.DrawLine(p, 0, 5, e.ClipRectangle.Width - 2, 5);
                    gfx.DrawLine(p, e.ClipRectangle.Width - 2, 5, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
                    gfx.DrawLine(p, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2);
                }

                private void grpMount_Back_Paint(object sender, PaintEventArgs e)
                //========================================================================
                {
                    Graphics gfx = e.Graphics;
                    Pen p = new Pen(Color.White, 1);
                    gfx.DrawLine(p, 0, 5, 0, e.ClipRectangle.Height - 2);
                    gfx.DrawLine(p, 0, 5, 10, 5);
                    gfx.DrawLine(p, 0, 5, e.ClipRectangle.Width - 2, 5);
                    gfx.DrawLine(p, e.ClipRectangle.Width - 2, 5, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
                    gfx.DrawLine(p, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2);
                }
            #endregion



        #endregion


        #region "UTILITY ROUTINES:"
                //************************          

            #region "Tab: WEB RELIEF:"

                private void Populate_MillRelief_D_Desig()
                //========================================     
                {
                    //BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                    StringCollection pMillRelief_D = new StringCollection();

                    //....Base Material.
                    //var pQryManfDrill = (from pRec in pBearingDBEntities.tblManf_Drill
                    //                     where pRec.fldCons_MillRelief == "Y"
                    //                     select pRec).ToList(); 

                    //if (pQryManfDrill.Count() > 0)
                    //{
                    //    for (int i = 0; i < pQryManfDrill.Count; i++)
                    //    {
                    //        pMillRelief_D.Add(pQryManfDrill[i].fldD_Desig);
                    //    }
                    //}

                    //....EXCEL File: StdToolData
                    string pstrFIELDS, pstrFROM, pstrWHERE, pstrORDERBY, pstrSQL;
                    OleDbDataReader pobjDR = null;
                    OleDbConnection pConnection = null;
                    pstrFIELDS = "Select D_Desig";
                    pstrFROM = " FROM [Drill$]";
                    pstrWHERE = " WHERE MillRelief = 'Y' or MillRelief = 'YP'";
                    pstrORDERBY = " Order by D_Desig ASC";

                    pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE + pstrORDERBY;
                    pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdToolData, ref pConnection);

                    while (pobjDR.Read())
                    {
                        pMillRelief_D.Add(pobjDR["D_Desig"].ToString());
                    }
                    pobjDR.Close();
                    pConnection.Close();

                    StringCollection pMillRelief_DwoIn = new StringCollection();
                    Double pNumerator, pDenominator;
                    Double pFinal;

                    for (int i = 0; i < pMillRelief_D.Count; i++)
                        pMillRelief_D[i] = pMillRelief_D[i].Remove(pMillRelief_D[i].Length - 1);

                    for (int i = 0; i < pMillRelief_D.Count; i++)
                        if (pMillRelief_D[i].Contains("/"))
                        {
                            if (pMillRelief_D[i].ToString() != "1")
                            {
                                pNumerator = Convert.ToInt32(modMain.ExtractPreData(pMillRelief_D[i], "/"));
                                pDenominator = Convert.ToInt32(modMain.ExtractPostData(pMillRelief_D[i], "/"));
                                pFinal = Convert.ToDouble(pNumerator / pDenominator);

                                //BG 02JUL13
                                if (pFinal > mBearing_Radial_FP.FlexurePivot.Web.H)
                                {
                                    pMillRelief_DwoIn.Add(pFinal.ToString());
                                }
                            }
                            else
                            {
                                //BG 02JUL13
                                pFinal = Convert.ToDouble(pMillRelief_D[i]);

                                if (pFinal > mBearing_Radial_FP.FlexurePivot.Web.H)
                                {
                                    pMillRelief_DwoIn.Add(pFinal.ToString());
                                }

                                //pMillRelief_DwoIn.Add(pMillRelief_D[i]);
                            }
                        }

                    modMain.SortNumberwoHash(ref pMillRelief_DwoIn, true);

                    pMillRelief_D.Clear();
                    for (int i = 0; i < pMillRelief_DwoIn.Count; i++)
                        pMillRelief_D.Add(pMillRelief_DwoIn[i] + "\"");

                    cmbMillRelief_D_Desig.Items.Clear();
                    if (pMillRelief_D.Count > 0)
                        for (int i = 0; i < pMillRelief_D.Count; i++)
                            cmbMillRelief_D_Desig.Items.Add(pMillRelief_D[i]);

                    if (cmbMillRelief_D_Desig.Items.Count > 0)
                    {
                        int pIndx = 0;
                        if (cmbMillRelief_D_Desig.Items.Contains(mBearing_Radial_FP.ARP.Dowel.Spec.Type))
                        {
                            pIndx = cmbMillRelief_D_Desig.Items.IndexOf(mBearing_Radial_FP.MillRelief.D_Desig);
                            cmbMillRelief_D_Desig.SelectedIndex = pIndx;
                        }
                        else
                            cmbMillRelief_D_Desig.SelectedIndex = 0;
                    }
                }

        

            #endregion
        

            #region "Tab: S/L HARDWARE:"

                private void Update_SL_Screw_L()
                //==============================
                {
                    if (cmbSL_Screw_Spec_Type.Text != ""
                                && cmbSL_Screw_Spec_D_Desig.Text != "")
                        Populate_SL_Screw_L();

                    string pFormat = null;

                    //if (cmbSL_Screw_Spec_Type.Text != "" && cmbSL_Screw_Spec_D_Desig.Text != "")
                    //{
                    //    if (mBearing_Radial_FP.SL.Screw_Spec.Unit.System == clsUnit.eSystem.Metric)      
                    //    {
                    //        //pFormat = "#0";
                    //        //lblSL_Screw_LLim.Text = modMain.ConvDoubleToStr(Math.Ceiling(mBearing_Radial_FP.SL.Thread_L_LowerLimit()), pFormat);
                    //        //pFormat = "#0";
                    //        lblSL_Screw_LLim.Text = mBearing_Radial_FP.SL.Screw_Spec.Unit.WriteInUserL((Math.Ceiling(mBearing_Radial_FP.SL.Thread_L_LowerLimit())));     //AES 18SEP18
                    //    }
                    //    else if (mBearing_Radial_FP.SL.Screw_Spec.Unit.System == clsUnit.eSystem.English) //BG 26MAR12
                    //    {
                    //        //pFormat = "#0.000";
                    //        //lblSL_Screw_LLim.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Thread_L_LowerLimit(), pFormat);
                    //        lblSL_Screw_LLim.Text = mBearing_Radial_FP.SL.Screw_Spec.Unit.WriteInUserL((Math.Ceiling(mBearing_Radial_FP.SL.Thread_L_LowerLimit())));     //AES 18SEP18
                    //    }
                    //}
                    //else
                    //    lblSL_Screw_LLim.Text = "";
                }


                private void Populate_SL_Screw_L()
                //================================
                {
                    //BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                    string pSL_Screw_Unit = mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString().Substring(0, 1);
                    string pSL_Screw_D = mBearing_Radial_FP.SL.Screw.Spec.D_Desig; //cmbSL_Screw_Spec_D_Desig.Text;
                    string pSL_Screw_Type = mBearing_Radial_FP.SL.Screw.Spec.Type;//cmbSL_Screw_Spec_Type.Text;                    
                    string pSL_Screw_Mat = "STEEL";//mBearing_Radial_FP.SL.Screw.Spec.Mat; //cmbSL_Screw_Spec_Mat.Text;
                    decimal pSL_Screw_Pitch = (Decimal)mBearing_Radial_FP.SL.Screw.Spec.Pitch;
                    
                    string pWHERE = " WHERE Type = '" + pSL_Screw_Type + "' and Mat = '" + pSL_Screw_Mat + "' and D_Desig = '" + pSL_Screw_D + "' and Pitch = '" + pSL_Screw_Pitch + "'";
                    int pL_RecCount = modMain.gDB.PopulateCmbBox(cmbSL_Screw_Spec_L, modMain.gFiles.FileTitle_EXCEL_StdPartsData, "[Screw_Metric$]", "L", pWHERE, true);

                    if (pL_RecCount > 0)
                    {
                        cmbSL_Screw_Spec_L.Text = "";
                        if (mBearing_Radial_FP.SL.Screw.Spec.L == 0)
                        {
                            cmbSL_Screw_Spec_L.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        cmbSL_Screw_Spec_L.Text = "";
                    }
                }

                private void Populate_Mount_Screw_L(int Index_In, ComboBox CmbBox_In)
                //==================================================================
                {
                    //BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                    string pSL_Screw_Unit = mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString().Substring(0, 1);
                    string pSL_Screw_D = mBearing_Radial_FP.Mount.Screw[Index_In].Spec.D_Desig; //cmbSL_Screw_Spec_D_Desig.Text;
                    string pSL_Screw_Type = mBearing_Radial_FP.Mount.Screw[Index_In].Spec.Type;//cmbSL_Screw_Spec_Type.Text;                    
                    string pSL_Screw_Mat = mBearing_Radial_FP.Mount.Screw[Index_In].Spec.Mat; //cmbSL_Screw_Spec_Mat.Text;
                    double pSL_Screw_Pitch = mBearing_Radial_FP.Mount.Screw[Index_In].Spec.Pitch;
                    ////Decimal pThread_L_LowerLimit = (Decimal)mBearing_Radial_FP.SL.Thread_L_LowerLimit();

                    //string pWHERE = " WHERE Type = '" + pSL_Screw_Type + "' and Unit = '" + pSL_Screw_Unit + "' and Mat = '" + pSL_Screw_Mat + "' and D_Desig = '" + pSL_Screw_D + "' and Pitch = '" + pSL_Screw_Pitch + "'";
                    string pWHERE = " WHERE Type = '" + pSL_Screw_Type + "' and Mat = '" + pSL_Screw_Mat + "' and D_Desig = '" + pSL_Screw_D + "' and Pitch = '" + pSL_Screw_Pitch + "'";
                    int pL_RecCount = modMain.gDB.PopulateCmbBox(CmbBox_In, modMain.gFiles.FileTitle_EXCEL_StdPartsData, "[Screw_Metric$]", "L", pWHERE, true);

                    if (pL_RecCount > 0)
                    {
                        CmbBox_In.SelectedIndex = 0;
                    }
                }

                private void ChangeCheck_SL_Screw()
                //=================================
                {
                    //....Caption & Message.
                    String pMsg = "For the selected Type, Material and Diameter no thread length" + System.Environment.NewLine +
                                 "is found in " + "\"" + "Screw" + "\""+
                                 " table in the database that statisfies" + System.Environment.NewLine +
                                 "the given limit constraint. Hence limit can not be imposed.";

                    String pCaption = "Information";

                    //....Show message box.
                    MessageBox.Show(pMsg,pCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);        

                    //....Checked = false.
                    chkSL_Screw_LenLim.Checked = false;   
                }
                

                private void Update_SL_Dowel_L()
                //==============================
                {
                    if (cmbSL_Dowel_Spec_Type.Text != "" && cmbSL_Dowel_Spec_D_Desig.Text != "")
                        Populate_SL_Dowel_L();

                    string pFormat = null;

                    ////if (cmbSL_Dowel_Spec_Type.Text != "" && cmbSL_Dowel_Spec_D_Desig.Text != "")
                    ////{
                    ////    if (mBearing_Radial_FP.SL.Dowel_Spec.Unit.System == clsUnit.eSystem.Metric)           
                    ////    {
                    ////        //pFormat = "#0";
                    ////        //lblSL_Dowel_LLim.Text = modMain.ConvDoubleToStr(Math.Ceiling(mBearing_Radial_FP.SL.Pin_L_LowerLimit()), pFormat);
                    ////        lblSL_Dowel_LLim.Text = mBearing_Radial_FP.SL.Dowel_Spec.Unit.WriteInUserL(Math.Ceiling(mBearing_Radial_FP.SL.Pin_L_LowerLimit()));
                    ////    }
                    ////    else if (mBearing_Radial_FP.SL.Dowel_Spec.Unit.System == clsUnit.eSystem.English)     
                    ////    {
                    ////        //pFormat = "#0.000";
                    ////        //lblSL_Dowel_LLim.Text = modMain.ConvDoubleToStr(mBearing_Radial_FP.SL.Pin_L_LowerLimit(), pFormat);
                    ////        lblSL_Dowel_LLim.Text = mBearing_Radial_FP.SL.Dowel_Spec.Unit.WriteInUserL(mBearing_Radial_FP.SL.Pin_L_LowerLimit());
                    ////    }
                    ////}
                    ////else
                    ////    lblSL_Dowel_LLim.Text = "";
                }


                private void Populate_SL_Dowel_L()
                //================================         
                {
                    //BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                    string pSL_Dowel_Unit = mBearing_Radial_FP.SL.Dowel.Spec.Unit.System.ToString().Substring(0, 1);
                    string pSL_Dowel_D = mBearing_Radial_FP.SL.Dowel.Spec.D_Desig;
                    string pSL_Dowel_Type = mBearing_Radial_FP.SL.Dowel.Spec.Type;
                    string pSL_Dowel_Mat = mBearing_Radial_FP.SL.Dowel.Spec.Mat;
                    ////Decimal pSL_Pin_L_LowerLimit = (Decimal)mBearing_Radial_FP.SL.Pin_L_LowerLimit();

                    //string pWHERE = " WHERE Type = '" + pSL_Dowel_Type + "' and Unit = '" + pSL_Dowel_Unit + "' and Mat = '" + pSL_Dowel_Mat + "' and D_Desig = '" + pSL_Dowel_D + "'";
                    string pWHERE = " WHERE Type = '" + pSL_Dowel_Type + "' and Mat = '" + pSL_Dowel_Mat + "' and D_Desig = '" + pSL_Dowel_D + "'";
                    int pL_RecCount = modMain.gDB.PopulateCmbBox(cmbSL_Dowel_Spec_L, modMain.gFiles.FileTitle_EXCEL_StdPartsData, "[Pin_Metric$]", "L", pWHERE, true);

                    if (pL_RecCount > 0)
                    {
                        if (mBearing_Radial_FP.SL.Dowel.Spec.L == 0)
                        {
                            cmbSL_Dowel_Spec_L.SelectedIndex = 0;
                        }
                    }

                    ////StringCollection pDowel_L = new StringCollection();
                    ////if (chkSL_Dowel_LenLim.Checked)
                    ////{
                    ////    var pQryManfPin_LenLim = (from pRec in pBearingDBEntities.tblManf_Pin
                    ////                              where pRec.fldUnit == pSL_Dowel_Unit && pRec.fldType == pSL_Dowel_Type &&
                    ////                                pRec.fldD_Desig == pSL_Dowel_D && pRec.fldMat == pSL_Dowel_Mat
                    ////                                && pRec.fldL > pSL_Pin_L_LowerLimit
                    ////                              orderby pRec.fldL ascending
                    ////                              select pRec.fldL).Distinct().ToList();

                    ////    if (pQryManfPin_LenLim.Count() > 0)
                    ////    {
                    ////        for (int i = 0; i < pQryManfPin_LenLim.Count; i++)
                    ////        {
                    ////            pDowel_L.Add(pQryManfPin_LenLim[i].ToString());
                    ////        }
                    ////    }

                    ////}
                    ////else
                    ////{
                    ////    var pQryManfPin = (from pRec in pBearingDBEntities.tblManf_Pin
                    ////                       where pRec.fldUnit == pSL_Dowel_Unit && pRec.fldType == pSL_Dowel_Type &&
                    ////                       pRec.fldD_Desig == pSL_Dowel_D && pRec.fldMat == pSL_Dowel_Mat
                    ////                       orderby pRec.fldL ascending
                    ////                       select pRec.fldL).Distinct().ToList();

                    ////    if (pQryManfPin.Count() > 0)
                    ////    {
                    ////        for (int i = 0; i < pQryManfPin.Count; i++)
                    ////        {
                    ////            pDowel_L.Add(pQryManfPin[i].ToString());
                    ////        }
                    ////    }
                    ////}

                    ////if (pDowel_L.Count > 0)
                    ////{
                    ////    cmbSL_Dowel_Spec_L.Items.Clear();
                    ////    for (int i = 0; i < pDowel_L.Count; i++)
                    ////    {
                    ////        Double pVal = Convert.ToDouble(pDowel_L[i]);
                    ////        //cmbSL_Dowel_Spec_L.Items.Add(modMain.ConvDoubleToStr(pVal, "#0.00#"));
                    ////        cmbSL_Dowel_Spec_L.Items.Add(modMain.gProject.PNR.Unit.WriteInUserL(pVal));      //AES 18SEP18
                    ////    }
                    ////}

                    ////if (cmbSL_Dowel_Spec_L.Items.Count > 0)
                    ////{
                    ////    if (mBearing_Radial_FP.SL.Dowel_Spec.L > modMain.gcEPS)

                    ////        if (cmbSL_Dowel_Spec_L.Items.Contains(mBearing_Radial_FP.SL.Dowel_Spec.L.ToString("#0.00#")))
                    ////            //cmbSL_Dowel_Spec_L.SelectedIndex = cmbSL_Dowel_Spec_L.Items.IndexOf(mBearing_Radial_FP.SL.Dowel_Spec.L.ToString("#0.00#"));
                    ////            cmbSL_Dowel_Spec_L.SelectedIndex = cmbSL_Dowel_Spec_L.Items.IndexOf(modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Dowel_Spec.L));    //AES 18SEP18
                    ////        else
                    ////            cmbSL_Dowel_Spec_L.SelectedIndex = 0;
                    ////}
                    ////else
                    ////{
                    ////    ////ChangeCheck_SL_Dowel();
                    ////}
                }


                private void ChangeCheck_SL_Dowel()
                //=================================
                {
                    //....Caption & Message.
                    String pMsg = "For the selected Type, Material and Diameter no Pin length" + System.Environment.NewLine +
                                 "is found in " + "\"" + "Pin" + "\"" +
                                 " table in the database that statisfies" + System.Environment.NewLine +
                                 "the given limit constraint. Hence limit can not be imposed.";

                    String pCaption = "Information";

                    //....Show message box.
                    MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);         //SB 10AUG09

                    chkSL_Dowel_LenLim.Checked = false;
                }

            #endregion


            #region "Anti-Rotation Pin:"

                private void Populate_ARP_L()
                //==================================
                {
                    //BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                    string pARP_Unit = mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System.ToString().Substring(0, 1);
                    string pARP_D = mBearing_Radial_FP.ARP.Dowel.Spec.D_Desig;
                    string pARP_Type = "P";//mBearing_Radial_FP.ARP.Dowel.Spec.Type;
                    string pARP_Mat = mBearing_Radial_FP.ARP.Dowel.Spec.Mat;
                    StringCollection pARP_L = new StringCollection();

                    if (pARP_Unit == "M")
                    {
                      
                        string pstrFIELDS, pstrFROM, pstrSQL, pstrWHERE, pstrORDERBY;

                        //....EXCEL File: StdPartsData
                        OleDbConnection pConnection = null;
                        pstrFIELDS = "Select Distinct L";
                        pstrFROM = " FROM [Pin_Metric$]";
                        pstrWHERE = " WHERE Type ='" + pARP_Type + "' and  D_Desig = '" + pARP_D + "' and Mat = '" + pARP_Mat + "'";
                        pstrORDERBY = " Order by L ASC";

                        pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE + pstrORDERBY;

                        OleDbDataReader pobjDR = null;
                        pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdPartsData, ref pConnection);

                        while (pobjDR.Read())
                        {
                            pARP_L.Add(pobjDR["L"].ToString());
                        }
                        pobjDR.Dispose();
                        pConnection.Close();
                        ////var pQryManfPin = (from pRec in pBearingDBEntities.tblManf_Pin
                        ////                   where pRec.fldUnit == pAntiRotPin_Unit && pRec.fldType == pAntiRotPin_Type &&
                        ////                   pRec.fldD_Desig == pAntiRotPin_D && pRec.fldMat == pAntiRotPin_Mat
                        ////                   orderby pRec.fldL ascending
                        ////                   select pRec.fldL).Distinct().ToList();

                        ////if (pQryManfPin.Count() > 0)
                        ////{
                        ////    for (int i = 0; i < pQryManfPin.Count; i++)
                        ////    {
                        ////        pAntiRotPin_L.Add(pQryManfPin[i].ToString());
                        ////    }
                        ////}

                        cmbARP_Spec_L.Items.Clear();
                        for (int i = 0; i < pARP_L.Count; i++)
                        {
                            if (pARP_L[i] != "")
                            {
                                Double pVal = Convert.ToDouble(pARP_L[i]);

                                //cmbAntiRotPin_Spec_L.Items.Add(modMain.ConvDoubleToStr(pVal, "#0.00#"));
                                //cmbARP_Spec_L.Items.Add(modMain.gProject.PNR.Unit.WriteInUserL(pVal));       //AES 18SEP18
                                cmbARP_Spec_L.Items.Add(pVal.ToString("#0"));       //AES 18SEP18
                            }
                            else
                            {
                                cmbARP_Spec_L.Text = "";
                            }
                        }

                        if (cmbARP_Spec_L.Items.Count > 0)
                        {
                            if (mBearing_Radial_FP.ARP.Dowel.Spec.L > modMain.gcEPS &&
                                            cmbARP_Spec_L.Items.Contains(mBearing_Radial_FP.ARP.Dowel.Spec.L.ToString("#0")))

                                //cmbAntiRotPin_Spec_L.Text = mBearing_Radial_FP.AntiRotPin.Spec.L.ToString("#0.00#");
                                cmbARP_Spec_L.Text = mBearing_Radial_FP.ARP.Dowel.Spec.L.ToString("#0");   //AES 18SEP18
                            else
                                cmbARP_Spec_L.SelectedIndex = 0;
                        }
                    }
                }

                private void Retrieve_ARP_PN()
                //==================================
                {
                    string pARP_Unit = mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System.ToString().Substring(0, 1);
                    string pARP_D = mBearing_Radial_FP.ARP.Dowel.Spec.D_Desig;
                    string pARP_Type = "P";//mBearing_Radial_FP.ARP.Dowel.Spec.Type;
                    string pARP_Mat = mBearing_Radial_FP.ARP.Dowel.Spec.Mat;
                    string pARP_L = mBearing_Radial_FP.ARP.Dowel.Spec.L.ToString();
                    
                    string pARP_PN = "";

                    if (pARP_Unit == "M")
                    {
                        //....EXCEL File: StdPartsData
                        string pstrFIELDS, pstrFROM, pstrSQL, pstrWHERE;
                        OleDbConnection pConnection = null;
                        pstrFIELDS = "Select PN_WBC";
                        pstrFROM = " FROM [Pin_Metric$]";
                        //pstrWHERE = " WHERE Type ='" + pARP_Type + "' and  Unit = '" + pARP_Unit + "' and D_Desig = '" + pARP_D + "' and Mat = '" + pARP_Mat + "'";
                        pstrWHERE = " WHERE Type ='" + pARP_Type + "' and  D_Desig = '" + pARP_D + "' and Mat = '" + pARP_Mat + "' and L = '" + pARP_L + "'";
                       
                        pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE;

                        OleDbDataReader pobjDR = null;
                        pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdPartsData, ref pConnection);

                        if (pobjDR.Read())
                        {
                            pARP_PN = pobjDR["PN_WBC"].ToString();
                        }
                        pobjDR.Dispose();
                        pConnection.Close();
                        txtARP_Spec_PN.Text = pARP_PN;                       
                    }
                }

                private void Retrieve_SL_Screw_Spec_PN()
                //=======================================
                {
                    string pSL_Screw_Spec_Unit = mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString().Substring(0, 1);
                    string pSL_Screw_Spec_D = mBearing_Radial_FP.SL.Screw.Spec.D_Desig;
                    string pSL_Screw_Spec_Type = mBearing_Radial_FP.SL.Screw.Spec.Type;
                    string pSL_Screw_Spec_Mat = mBearing_Radial_FP.SL.Screw.Spec.Mat;
                    string pSL_Screw_L = mBearing_Radial_FP.SL.Screw.Spec.L.ToString();
                    string pSL_Screw_Spec_PN = "";

                    if (pSL_Screw_Spec_Unit == "M")
                    {
                        //....EXCEL File: StdPartsData
                        string pstrFIELDS, pstrFROM, pstrSQL, pstrWHERE;
                        OleDbConnection pConnection = null;
                        pstrFIELDS = "Select PN_WBC";
                        pstrFROM = " FROM [Screw_Metric$]";
                        pstrWHERE = " WHERE Type ='" + pSL_Screw_Spec_Type + "' and  D_Desig = '" + pSL_Screw_Spec_D + "' and Mat = '" + pSL_Screw_Spec_Mat + "' and L = '" + pSL_Screw_L + "'";

                        pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE;

                        OleDbDataReader pobjDR = null;
                        pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdPartsData, ref pConnection);

                        if (pobjDR.Read())
                        {
                            pSL_Screw_Spec_PN = pobjDR["PN_WBC"].ToString();
                        }
                        pobjDR.Dispose();
                        pConnection.Close();
                        txtSL_Screw_Spec_PN.Text = pSL_Screw_Spec_PN;
                    }
                }

                private void Retrieve_SL_Dowel_Spec_PN()
                //=======================================
                {
                    string pSL_Dowel_Spec_Unit = mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString().Substring(0, 1);
                    string pSL_Dowel_Spec_D = mBearing_Radial_FP.SL.Dowel.Spec.D_Desig;
                    string pSL_Dowel_Spec_Type = mBearing_Radial_FP.SL.Dowel.Spec.Type;
                    string pSL_Dowel_Spec_Mat = mBearing_Radial_FP.SL.Dowel.Spec.Mat;
                    string pSL_Dowel_Spec_L = mBearing_Radial_FP.SL.Dowel.Spec.L.ToString();
                    string pSL_Dowel_Spec_PN = "";


                    if (pSL_Dowel_Spec_Unit == "M")
                    {
                        //....EXCEL File: StdPartsData
                        string pstrFIELDS, pstrFROM, pstrSQL, pstrWHERE;
                        OleDbConnection pConnection = null;
                        pstrFIELDS = "Select PN_WBC";
                        pstrFROM = " FROM [Pin_Metric$]";
                        //pstrWHERE = " WHERE Type ='" + pSL_Dowel_Spec_Type + "' and  Unit = '" + pSL_Dowel_Spec_Unit + "' and D_Desig = '" + pSL_Dowel_Spec_D + "' and Mat = '" + pSL_Dowel_Spec_Mat + "'";
                        pstrWHERE = " WHERE Type ='" + pSL_Dowel_Spec_Type + "' and  D_Desig = '" + pSL_Dowel_Spec_D + "' and Mat = '" + pSL_Dowel_Spec_Mat + "' and L = '" + pSL_Dowel_Spec_L + "'";

                        pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE;

                        OleDbDataReader pobjDR = null;
                        pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdPartsData, ref pConnection);

                        if (pobjDR.Read())
                        {
                            pSL_Dowel_Spec_PN = pobjDR["PN_WBC"].ToString();
                        }
                        pobjDR.Dispose();
                        pConnection.Close();
                        txtSL_Dowel_Spec_PN.Text = pSL_Dowel_Spec_PN;
                    }
                }

                private void Retrieve_ARP_Dowel_Spec_Depth()
                //=========================================
                {
                    string pARP_Dowel_Spec_Unit = mBearing_Radial_FP.ARP.Dowel.Spec.Unit.System.ToString().Substring(0, 1);
                    string pARP_Dowel_Spec_D = mBearing_Radial_FP.ARP.Dowel.Spec.D_Desig;
                    string pARP_Dowel_Spec_Type = mBearing_Radial_FP.ARP.Dowel.Spec.Type;
                    string pARP_Dowel_Spec_Mat = mBearing_Radial_FP.ARP.Dowel.Spec.Mat;
                    string pARP_Dowel_Spec_L = mBearing_Radial_FP.ARP.Dowel.Spec.L.ToString();


                    //....EXCEL File: StdPartsData
                    string pstrFIELDS, pstrFROM, pstrSQL, pstrWHERE;
                    OleDbConnection pConnection = null;
                    pstrFIELDS = "Select *";
                    pstrFROM = " FROM [Pin_Metric$]";

                    pstrWHERE = " WHERE Type ='" + pARP_Dowel_Spec_Type + "' and  D_Desig = '" + pARP_Dowel_Spec_D + "' and Mat = '" + pARP_Dowel_Spec_Mat + "' and L = '" + pARP_Dowel_Spec_L + "'";

                    pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE;

                    OleDbDataReader pobjDR = null;
                    pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdPartsData, ref pConnection);

                    if (pobjDR.Read())
                    {
                        int pColFldName = 0;
                        mBearing_Radial_FP.ARP.Dowel.Hole_Depth_Low = 0.0;
                        txtARP_Depth.Text = "";

                        pColFldName = pobjDR.GetOrdinal("Lower Depth");
                        if (pobjDR.IsDBNull(pColFldName) == false)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.ARP.Dowel.Hole_Depth_Low = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pobjDR["Lower Depth"].ToString()));
                                txtARP_Depth.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.ARP.Dowel.Hole.Depth_Low));
                            }
                            else
                            {
                                mBearing_Radial_FP.ARP.Dowel.Hole_Depth_Low = modMain.ConvTextToDouble(pobjDR["Lower Depth"].ToString());
                                txtARP_Depth.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.ARP.Dowel.Hole.Depth_Low);
                            }
                        }
                    }
                    pobjDR.Dispose();
                    pConnection.Close();

                }

                private void Retrieve_SL_Dowel_Spec_Depth()
                //=========================================
                {
                    string pSL_Dowel_Spec_Unit = mBearing_Radial_FP.SL.Screw.Spec.Unit.System.ToString().Substring(0, 1);
                    string pSL_Dowel_Spec_D = mBearing_Radial_FP.SL.Dowel.Spec.D_Desig;
                    string pSL_Dowel_Spec_Type = mBearing_Radial_FP.SL.Dowel.Spec.Type;
                    string pSL_Dowel_Spec_Mat = mBearing_Radial_FP.SL.Dowel.Spec.Mat;
                    string pSL_Dowel_Spec_L = mBearing_Radial_FP.SL.Dowel.Spec.L.ToString();
                 

                    //....EXCEL File: StdPartsData
                    string pstrFIELDS, pstrFROM, pstrSQL, pstrWHERE;
                    OleDbConnection pConnection = null;
                    pstrFIELDS = "Select *";
                    pstrFROM = " FROM [Pin_Metric$]";

                    pstrWHERE = " WHERE Type ='" + pSL_Dowel_Spec_Type + "' and  D_Desig = '" + pSL_Dowel_Spec_D + "' and Mat = '" + pSL_Dowel_Spec_Mat + "' and L = '" + pSL_Dowel_Spec_L + "'";

                    pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE;

                    OleDbDataReader pobjDR = null;
                    pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdPartsData, ref pConnection);

                    if (pobjDR.Read())
                    {
                        int pColFldName = 0;

                        mBearing_Radial_FP.SL.Dowel.Hole_Depth_Up = 0.0;
                        txtSL_Dowel_Depth_Up.Text = "";

                        pColFldName = pobjDR.GetOrdinal("Upper Depth");
                        if (pobjDR.IsDBNull(pColFldName) == false)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.SL.Dowel.Hole_Depth_Up = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pobjDR["Upper Depth"].ToString()));
                                txtSL_Dowel_Depth_Up.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Dowel.Hole.Depth_Up));
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Dowel.Hole_Depth_Up = modMain.ConvTextToDouble(pobjDR["D_CBore"].ToString());
                                txtSL_Dowel_Depth_Up.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Dowel.Hole.Depth_Up);
                            }
                        }

                        mBearing_Radial_FP.SL.Dowel.Hole_Depth_Low = 0.0;
                        txtSL_Dowel_Depth_Low.Text = "";

                        pColFldName = pobjDR.GetOrdinal("Lower Depth");
                        if (pobjDR.IsDBNull(pColFldName) == false)
                        {
                            if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                            {
                                mBearing_Radial_FP.SL.Dowel.Hole_Depth_Low = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(pobjDR["Lower Depth"].ToString()));
                                txtSL_Dowel_Depth_Low.Text = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CEng_Met(mBearing_Radial_FP.SL.Dowel.Hole.Depth_Low));
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Dowel.Hole_Depth_Low = modMain.ConvTextToDouble(pobjDR["Lower Depth"].ToString());
                                txtSL_Dowel_Depth_Low.Text = modMain.gProject.PNR.Unit.WriteInUserL(mBearing_Radial_FP.SL.Dowel.Hole.Depth_Low);
                            }
                        }                           
                    }

                    pobjDR.Dispose();
                    pConnection.Close();

                }

            #endregion


            #region "END CONFIGS MOUNT:"

                #region "THREAD RELATED ROUTINES:"                    

                    private void ChangeCheck_SealMount_Thread(CheckBox ChkBox_In )
                    //=================================================================
                    {
                        //....Caption & Message.
                        String pMsg = "For the selected Type, Material and Diameter no thread length" + System.Environment.NewLine +
                                     "is found in " + "\"" + "Screw" + "\"" +
                                     " table in the database that statisfies" + System.Environment.NewLine +
                                     "the given limit constraints. Hence limit can not be imposed.";

                        String pCaption = "Information";

                        //....Show message box.
                        MessageBox.Show(pMsg, pCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);         

                        //....Checked = false.
                        ChkBox_In.Checked = false;
                    }
                 
                #endregion


                #region "Utility Routine:"  


                    private void Display_AnglesBet(clsBearing_Radial_FP Bearing_In, Int32 MountHoles_Pos_In)
                    //=======================================================================================
                    {
                        //....If Holes are not equispaced then retrieve data from database.
                        //....Show other angle.
                        switch (MountHoles_Pos_In)
                        {
                            case 0:
                                //-------
                                for (int i = 0; i < mTxtMount_HolesAngBet_Front.Length; i++)
                                {
                                    mTxtMount_HolesAngBet_Front[i].Visible = false;
                                }

                                if (!Bearing_In.Mount.BC[0].EquiSpaced)
                                {
                                    for (int i = 0; i < Bearing_In.Mount.BC[0].Count - 1; i++)
                                    {
                                        mTxtMount_HolesAngBet_Front[i].ReadOnly = false;
                                            mTxtMount_HolesAngBet_Front[i].BackColor = Color.White;
                                            mTxtMount_HolesAngBet_Front[i].ForeColor = Color.Black;
                                      
                                        mTxtMount_HolesAngBet_Front[i].Visible = true;

                                        //....Set Text Values.
                                        mTxtMount_HolesAngBet_Front[i].Text =
                                            modMain.ConvDoubleToStr(Bearing_In.Mount.BC[0].AngBet[i], "#0");
                                    }
                                }
                                else
                                    for (int i = 0; i < Bearing_In.Mount.BC[0].Count - 1; i++)
                                    {
                                        //....Set Controls for Angle Bet.
                                        mTxtMount_HolesAngBet_Front[i].Visible = true;
                                        mTxtMount_HolesAngBet_Front[i].ReadOnly = true;
                                        mTxtMount_HolesAngBet_Front[i].BackColor = txtMount_WallT_Front.BackColor;
                                        mTxtMount_HolesAngBet_Front[i].ForeColor = Color.Blue;

                                        //....Set Text Values.
                                        Double pOtherAngle;
                                        pOtherAngle = Bearing_In.Mount.Mount_Sel_AngBet(0);
                                        mTxtMount_HolesAngBet_Front[i].Text = modMain.ConvDoubleToStr(pOtherAngle, "#0");
                                    }

                                break;

                            case 1:
                                //-----------
                                for (int i = 0; i < mTxtMount_HolesAngBet_Back.Length; i++)
                                {
                                    mTxtMount_HolesAngBet_Back[i].Visible = false;
                                }

                                if (!Bearing_In.Mount.BC[1].EquiSpaced)
                                {
                                    for (int i = 0; i < Bearing_In.Mount.BC[1].Count - 1; i++)
                                    {
                                        //....Set Controls for Angle Bet.                                       
                                        mTxtMount_HolesAngBet_Back[i].ReadOnly = false;
                                            mTxtMount_HolesAngBet_Back[i].BackColor = Color.White;
                                            mTxtMount_HolesAngBet_Back[i].ForeColor = Color.Black;
                                       
                                        mTxtMount_HolesAngBet_Back[i].Visible = true;

                                        //....Set Text Values.
                                        mTxtMount_HolesAngBet_Back[i].Text =
                                            modMain.ConvDoubleToStr(Bearing_In.Mount.BC[1].AngBet[i], "#0");
                                    }
                                }
                                else
                                    for (int i = 0; i < Bearing_In.Mount.BC[1].Count - 1; i++)
                                    {
                                        //....Set Controls for Angle Bet.
                                        mTxtMount_HolesAngBet_Back[i].Visible = true;
                                        mTxtMount_HolesAngBet_Back[i].ReadOnly = true;
                                        mTxtMount_HolesAngBet_Back[i].BackColor = txtMount_WallT_Back.BackColor;
                                        mTxtMount_HolesAngBet_Back[i].ForeColor = Color.Blue;

                                        //....Set Text Values.
                                        Double pOtherAngle;
                                        pOtherAngle = Bearing_In.Mount.Mount_Sel_AngBet(1);
                                        mTxtMount_HolesAngBet_Back[i].Text = modMain.ConvDoubleToStr(pOtherAngle, "#0");
                                    }
                                break;
                        }
                    }  


                    private void SwapVal(ref Double[] Sng_In)
                    //=======================================
                    {
                        if (Sng_In.Length == 2)
                        {
                            if (Sng_In[0] > Sng_In[1])
                            {
                                Double pAny;
                                pAny = Sng_In[0];
                                Sng_In[0] = Sng_In[1];
                                Sng_In[1] = pAny;
                            }
                        }
                    }

                #endregion

            #endregion


            #region "TEMP SENSOR:"

                private void SetControl_TempSensor()
                //==================================
                {
                    lblTempSensor_CanLength.Visible = chkTempSensor_Exists.Checked;
                    txtTempSensor_CanLength.Visible = chkTempSensor_Exists.Checked;

                    lblTempSensor_Count.Visible = chkTempSensor_Exists.Checked;
                    cmbTempSensor_Count.Visible = chkTempSensor_Exists.Checked;

                    lblTempSensor_Loc.Visible = chkTempSensor_Exists.Checked;
                    cmbTempSensor_Loc.Visible = chkTempSensor_Exists.Checked;

                    lblTempSensor_D.Visible = chkTempSensor_Exists.Checked;
                    txtTempSensor_D.Visible = chkTempSensor_Exists.Checked;

                    lblTempSensor_Depth.Visible = chkTempSensor_Exists.Checked;
                    txtTempSensor_Depth.Visible = chkTempSensor_Exists.Checked;  

                    lblTempSensor_Angles.Visible = chkTempSensor_Exists.Checked;
                    lblTempSensor_txt.Visible = chkTempSensor_Exists.Checked;   

                    lblTempSensor_Ang_Start.Visible = chkTempSensor_Exists.Checked;
                    txtTempSensor_AngStart.Visible = chkTempSensor_Exists.Checked;

                    lblTempSensor_AngBet.Visible = chkTempSensor_Exists.Checked;
                    txtTempSensor_AngBet.Visible = chkTempSensor_Exists.Checked;
                }

            #endregion


            #region "POPULATE PIN & THREAD:"

                private void Populate_Pin_D_Desig(ref ComboBox CmbD_In,String Pin_Type_In,
                                                  String Pin_Mat_In, clsUnit.eSystem Unit_In, string D_Desig_In )
                //===============================================================================================
                {
                    //BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                    //.....Populate Dia_Desig ComboBox.
                    string pUnitSystem = "";

                    if (Unit_In == clsUnit.eSystem.English)
                        pUnitSystem = "E";
                    else if (Unit_In == clsUnit.eSystem.Metric)
                        pUnitSystem = "M";

                    string pPin_Type = Pin_Type_In;

                    if (Pin_Mat_In == "")
                        return;

                    //....EXCEL File: StdPartsData
                    string pstrFIELDS, pstrFROM, pstrSQL, pstrWHERE, pstrORDERBY;
                    OleDbConnection pConnection = null;
                    pstrFIELDS = "Select Distinct D_Desig";
                    pstrFROM = " FROM [Pin_Metric$]";
                    //pstrWHERE = " WHERE Type ='" + Pin_Type_In + "' and  Unit = '" + pUnitSystem + "' and Mat = '" + Pin_Mat_In + "'";
                    pstrWHERE = " WHERE Type ='" + Pin_Type_In + "' and  Mat = '" + Pin_Mat_In + "'";
                    pstrORDERBY = " Order by D_Desig ASC";

                    pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE + pstrORDERBY;

                    OleDbDataReader pobjDR = null;
                    pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdPartsData, ref pConnection);

                    StringCollection pDia_Desig = new StringCollection();

                    while (pobjDR.Read())
                    {
                        pDia_Desig.Add(pobjDR["D_Desig"].ToString());
                    }
                    pobjDR.Dispose();
                    pConnection.Close();

                    if (pUnitSystem == "E")
                    {                       

                        //....Initialize String Collection.
                        StringCollection pDia_DwHash = new StringCollection();  //....Dia_Desig with # symbol.
                        StringCollection pDia_DwoHash = new StringCollection(); //....Dia_Desig without # symbol. 

                        Double pNumerator, pDenominator;
                        String pFinal;

                        for (int i = 0; i < pDia_Desig.Count; i++)
                        {
                            if (pDia_Desig[i].Contains("#"))
                            {
                                pDia_DwHash.Add(pDia_Desig[i].Remove(0, 1));

                            }
                            else
                            {
                                if (pDia_Desig[i].ToString() != "1")
                                {
                                    pNumerator = Convert.ToInt32(modMain.ExtractPreData(pDia_Desig[i], "/"));
                                    pDenominator = Convert.ToInt32(modMain.ExtractPostData(pDia_Desig[i], "/"));
                                    pFinal = Convert.ToDouble(pNumerator / pDenominator).ToString();
                                    pDia_DwoHash.Add(pFinal);
                                }
                                else
                                    pDia_DwoHash.Add(pDia_Desig[i]);
                            }
                        }

                        //....Sort Dia_Desig with # symbol.
                        //SortNumberwHash(ref pDia_DwHash);
                        modMain.SortNumberwHash(ref pDia_DwHash);

                        //....Sort Dia_Desig without # symbol.
                        //SortNumberwoHash(ref pDia_DwoHash, true);
                        modMain.SortNumberwoHash(ref pDia_DwoHash, true);

                        //....Concatinate # symbol with pDia_DwHash.
                        for (int i = 0; i < pDia_DwHash.Count; i++)
                        {
                            pDia_DwHash[i] = "#" + pDia_DwHash[i];
                        }

                        //....Clear Combo Box Split Line Hardware Thread Dia_desig.
                        CmbD_In.Items.Clear();

                        //....Populate Combo Box Split Line Hardware Thread Dia_Desig.
                        for (int i = 0; i < pDia_DwHash.Count; i++)
                        {
                            CmbD_In.Items.Add(pDia_DwHash[i]);
                        }

                        for (int i = 0; i < pDia_DwoHash.Count; i++)
                        {
                            CmbD_In.Items.Add(pDia_DwoHash[i]);
                        }
                    }
                    else if (pUnitSystem == "M")
                    {
                        //////....Populate Dia Desig.
                        ////StringCollection pDia_Desig = new StringCollection();

                        ////var pQry_Manf_Pin = (from pRec in pBearingDBEntities.tblManf_Pin
                        ////                     where pRec.fldType == pPin_Type &&
                        ////                     pRec.fldUnit == pUnitSystem &&
                        ////                     pRec.fldMat == Pin_Mat_In
                        ////                     orderby pRec.fldD_Desig ascending
                        ////                     select pRec.fldD_Desig).Distinct().ToList();

                        ////if (pQry_Manf_Pin.Count() > 0)
                        ////{
                        ////    for (int i = 0; i < pQry_Manf_Pin.Count; i++)
                        ////    {
                        ////        pDia_Desig.Add(pQry_Manf_Pin[i]);
                        ////    }
                        ////}

                        //....Initialize String Collection.
                        StringCollection pDia_D = new StringCollection();  //....Dia_Desig with # symbol.

                        for (int i = 0; i < pDia_Desig.Count; i++)
                        {
                            if (pDia_Desig[i].Contains("M"))
                            {
                                pDia_D.Add(pDia_Desig[i].Remove(0, 1));
                            }
                        }

                        //....Sort Dia_Desig without # symbol.
                        //SortNumberwoHash(ref pDia_D, false);
                        modMain.SortNumberwoHash(ref pDia_D, false);

                        CmbD_In.Items.Clear();

                        //....Concatinate # symbol with pDia_DwHash.
                        for (int i = 0; i < pDia_D.Count; i++)
                        {
                            pDia_D[i] = "M" + pDia_D[i];
                        }

                        for (int i = 0; i < pDia_D.Count; i++)
                        {
                            CmbD_In.Items.Add(pDia_D[i]);
                        }
                    }

                    if (CmbD_In.Items.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(D_Desig_In) && CmbD_In.Items.Contains(D_Desig_In))
                            CmbD_In.Text = D_Desig_In;
                        else
                            CmbD_In.SelectedIndex = 0;
                    }
                }
               
                private void Populate_Pin_Mat(ref ComboBox CmbMat_In, String Pin_Type_In, clsUnit.eSystem Unit_In)
                //================================================================================================
                {
                    ////    BearingDBEntities pBearingDBEntities = new BearingDBEntities();
                    ////    //.....Populate Dia_Desig ComboBox.
                    ////    string pUnitSystem = "";

                    ////    if (Unit_In == clsUnit.eSystem.English)
                    ////        pUnitSystem = "E";
                    ////    else if (Unit_In == clsUnit.eSystem.Metric)
                    ////        pUnitSystem = "M";

                    ////    var pQry_Manf_Pin = (from pRec in pBearingDBEntities.tblManf_Pin
                    ////                         where pRec.fldType == Pin_Type_In &&
                    ////                             pRec.fldUnit == pUnitSystem
                    ////                         orderby pRec.fldMat ascending
                    ////                         select pRec.fldMat).Distinct().ToList();
                    ////    CmbMat_In.Items.Clear();
                    ////    if (pQry_Manf_Pin.Count() > 0)
                    ////    {
                    ////        for (int i = 0; i < pQry_Manf_Pin.Count; i++)
                    ////        {
                    ////            CmbMat_In.Items.Add(pQry_Manf_Pin[i]);
                    ////        }
                    ////    }

                    ////    if (CmbMat_In.Items.Count > 0)
                    ////    {
                    ////        int pIndx = -1;

                    ////        if (CmbMat_In.Items.Contains("STL"))
                    ////        {
                    ////            pIndx = CmbMat_In.Items.IndexOf("STL");

                    ////            if (pIndx != -1)
                    ////                CmbMat_In.SelectedIndex = pIndx;
                    ////            else
                    ////                CmbMat_In.SelectedIndex = 0;
                    ////        }
                    ////    }

                    string pUnitSystem = "";

                    if (Unit_In == clsUnit.eSystem.English)
                        pUnitSystem = "E";
                    else if (Unit_In == clsUnit.eSystem.Metric)
                        pUnitSystem = "M";

                    //string pStdParts_Data_FileName = "D:\\BearingCAD\\Program Data Files\\StdParts_Data_03OCT18.xlsx";
                    
                    if (pUnitSystem == "M")
                    {
                        //string pWHERE = " WHERE Type = '" + Pin_Type_In + "' and Unit = '" + pUnitSystem + "' and Mat = 'STEEL" + "'";
                        string pWHERE = " WHERE Type = '" + Pin_Type_In + "' and Mat = 'STEEL" + "'";
                        int pMat_Name_RecCount = modMain.gDB.PopulateCmbBox(CmbMat_In, modMain.gFiles.FileTitle_EXCEL_StdPartsData, "[Pin_Metric$]", "Mat", pWHERE, true);

                        if (pMat_Name_RecCount > 0)
                        {
                            int pIndx = -1;

                            if (CmbMat_In.Items.Contains("STEEL"))
                            {
                                pIndx = CmbMat_In.Items.IndexOf("STEEL");

                                if (pIndx != -1)
                                    CmbMat_In.SelectedIndex = pIndx;
                                else
                                    CmbMat_In.SelectedIndex = 0;
                            }
                        }
                    }
                }


                private void Populate_Screw_D_Desig(ref ComboBox CmbD_In, String Screw_Type_In,
                                                     String Screw_Mat_In, clsUnit.eSystem Unit_In, string D_Desig_In)
                //===================================================================================================
                {
                    //BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                    //.....Populate Dia_Desig ComboBox.
                    string pUnitSystem = "";

                    if (Unit_In == clsUnit.eSystem.English)
                        pUnitSystem = "E";
                    else if (Unit_In == clsUnit.eSystem.Metric)
                        pUnitSystem = "M";

                    if (Screw_Mat_In == "")
                        return;

                    ////StringCollection pDia_Desig = new StringCollection();
                    ////var pQry_Manf_Screw = (from pRec in pBearingDBEntities.tblManf_Screw
                    ////                       where pRec.fldType == Screw_Type_In &&
                    ////                       pRec.fldUnit == pUnitSystem && pRec.fldMat == Screw_Mat_In
                    ////                       orderby pRec.fldD_Desig ascending
                    ////                       select pRec.fldD_Desig).Distinct().ToList();

                

                    ////pDia_Desig.Clear();
                    ////if (pQry_Manf_Screw.Count() > 0)
                    ////{
                    ////    for (int i = 0; i < pQry_Manf_Screw.Count; i++)
                    ////    {
                    ////        pDia_Desig.Add(pQry_Manf_Screw[i]);
                    ////    }
                    ////}

                    //....EXCEL File: StdPartsData
                    string pstrFIELDS, pstrFROM, pstrSQL, pstrWHERE, pstrORDERBY;
                    OleDbConnection pConnection = null;
                    pstrFIELDS = "Select Distinct D_Desig";
                    pstrFROM = " FROM [Screw_Metric$]";
                    //pstrWHERE = " WHERE Type ='" + Screw_Type_In + "' and  Unit = '" + pUnitSystem + "' and Mat = '" + Screw_Mat_In + "'";
                    pstrWHERE = " WHERE Type ='" + Screw_Type_In + "' and  Mat = '" + Screw_Mat_In + "'";
                    pstrORDERBY = " Order by D_Desig ASC";

                    pstrSQL = pstrFIELDS + pstrFROM + pstrWHERE + pstrORDERBY;

                    OleDbDataReader pobjDR = null;
                    pobjDR = modMain.gDB.GetDataReader(pstrSQL, modMain.gFiles.FileTitle_EXCEL_StdPartsData, ref pConnection);

                    StringCollection pDia_Desig = new StringCollection();
                    while (pobjDR.Read())
                    {
                        pDia_Desig.Add(pobjDR["D_Desig"].ToString());
                    }

                    pobjDR.Dispose();
                    pConnection.Close();

                    if (pUnitSystem == "E")
                    {
                        //....Initialize String Collection.
                        StringCollection pDia_DwHash = new StringCollection();  //....Dia_Desig with # symbol.
                        StringCollection pDia_DwoHash = new StringCollection(); //....Dia_Desig without # symbol. 

                        Double pNumerator, pDenominator;
                        String pFinal;

                        for (int i = 0; i < pDia_Desig.Count; i++)
                        {
                            if (pDia_Desig[i].Contains("#"))
                            {
                                pDia_DwHash.Add(pDia_Desig[i].Remove(0, 1));

                            }
                            else if (pDia_Desig[i].Contains("/"))
                            {
                                if (pDia_Desig[i].ToString() != "1")
                                {
                                    pNumerator = Convert.ToInt32(modMain.ExtractPreData(pDia_Desig[i], "/"));
                                    pDenominator = Convert.ToInt32(modMain.ExtractPostData(pDia_Desig[i], "/"));
                                    pFinal = Convert.ToDouble(pNumerator / pDenominator).ToString();
                                    pDia_DwoHash.Add(pFinal);
                                }
                                else
                                    pDia_DwoHash.Add(pDia_Desig[i]);
                            }
                        }

                        //....Sort Dia_Desig with # symbol.
                        modMain.SortNumberwHash(ref pDia_DwHash);

                        //....Sort Dia_Desig without # symbol.
                        modMain.SortNumberwoHash(ref pDia_DwoHash, true);

                        //....Concatinate # symbol with pDia_DwHash.
                        for (int i = 0; i < pDia_DwHash.Count; i++)
                        {
                            pDia_DwHash[i] = "#" + pDia_DwHash[i];
                        }

                        //....Clear Combo Box Split Line Hardware Thread Dia_desig.
                        CmbD_In.Items.Clear();

                        //....Populate Combo Box Split Line Hardware Thread Dia_Desig.

                        for (int i = 0; i < pDia_DwHash.Count; i++)
                        {
                            CmbD_In.Items.Add(pDia_DwHash[i]);
                        }

                        for (int i = 0; i < pDia_DwoHash.Count; i++)
                        {
                            CmbD_In.Items.Add(pDia_DwoHash[i]);
                        }
                    }
                    else if (pUnitSystem == "M")
                    {
                        //....Initialize String Collection.
                        StringCollection pDia_D = new StringCollection();

                        for (int i = 0; i < pDia_Desig.Count; i++)
                        {
                            if (pDia_Desig[i].Contains("M"))
                            {
                                {
                                    pDia_D.Add(pDia_Desig[i].Remove(0, 1));
                                }
                            }
                        }
                        //....Sort Dia_Desig without # symbol.
                        modMain.SortNumberwoHash(ref pDia_D, false);

                        CmbD_In.Items.Clear();

                        //....Concatinate M symbol with pDia_DwHash.
                        for (int i = 0; i < pDia_D.Count; i++)
                        {
                            pDia_D[i] = "M" + pDia_D[i];
                        }

                        for (int i = 0; i < pDia_D.Count; i++)
                        {
                            CmbD_In.Items.Add(pDia_D[i]);
                        }
                    }

                    if (CmbD_In.Items.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(D_Desig_In) && CmbD_In.Items.Contains(D_Desig_In))
                            CmbD_In.Text = D_Desig_In;
                        else
                            CmbD_In.SelectedIndex = 0;
                    }
                }


                private void Populate_Screw_Mat(ref ComboBox CmbMat_In, String Thread_Type_In, clsUnit.eSystem Unit_In)
                //=====================================================================================================
                {
                    //BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                    //.....Populate Dia_Desig ComboBox.
                    string pUnitSystem = "";

                    if (Unit_In == clsUnit.eSystem.English)
                        pUnitSystem = "E";
                    else if (Unit_In == clsUnit.eSystem.Metric)
                        pUnitSystem = "M";

                    ////var pQry_Manf_Screw = (from pRec in pBearingDBEntities.tblManf_Screw
                    ////                       where pRec.fldType == Thread_Type_In && pRec.fldUnit == pUnitSystem
                    ////                       orderby pRec.fldMat ascending
                    ////                       select pRec.fldMat).Distinct().ToList();
                    ////CmbMat_In.Items.Clear();
                    ////if (pQry_Manf_Screw.Count() > 0)
                    ////{
                    ////    for (int i = 0; i < pQry_Manf_Screw.Count; i++)
                    ////    {
                    ////        CmbMat_In.Items.Add(pQry_Manf_Screw[i]);
                    ////    }
                    ////}

                    ////if (CmbMat_In.Items.Count > 0)
                    ////{
                    ////    int pIndx = -1;

                    ////    if (CmbMat_In.Items.Contains("STL"))
                    ////        pIndx = CmbMat_In.Items.IndexOf("STL");
                    ////    if (pIndx != -1)
                    ////        CmbMat_In.SelectedIndex = pIndx;
                    ////    else
                    ////        CmbMat_In.SelectedIndex = 0;
                    ////}

                    CmbMat_In.Items.Clear();
                    CmbMat_In.Items.Add("STEEL");
                    CmbMat_In.SelectedIndex = 0;

                    ////string pWHERE = " WHERE Type = '" + Thread_Type_In + "' and Unit = '" + pUnitSystem + "'";
                    //string pWHERE = " WHERE Type = '" + Thread_Type_In + "'";
                    ////string pWHERE = " WHERE Type = '" + Thread_Type_In + "' and Unit = '" + pUnitSystem + "' and Mat = 'STEEL" + "'";
                    //int pMat_Name_RecCount = modMain.gDB.PopulateCmbBox(CmbMat_In, modMain.gFiles.FileTitle_EXCEL_StdPartsData, "[Screw_Metric$]", "Mat", pWHERE, true);

                    //if (pMat_Name_RecCount > 0)
                    //{
                    //    int pIndx = -1;

                    //    if (CmbMat_In.Items.Contains("STEEL"))
                    //    {
                    //        pIndx = CmbMat_In.Items.IndexOf("STEEL");

                    //        if (pIndx != -1)
                    //            CmbMat_In.SelectedIndex = pIndx;
                    //        else
                    //            CmbMat_In.SelectedIndex = 0;
                    //    }
                    //}
                }
               
            #endregion

                private void chkMount_DBC_Front_CheckedChanged(object sender, EventArgs e)
                //========================================================================
                {
                    Double pULim = mMount_DBC_ULimit[0];
                    Double pLLim = mMount_DBC_LLimit[0];
                    Double pMean_Lim = 0.5 * (pULim + pLLim);

                    Double pVal = modMain.ConvTextToDouble(txtEndConfig_DBC_Front.Text);

                    if (chkMount_DBC_Front.Checked)
                    {
                        txtEndConfig_DBC_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(pMean_Lim);
                        txtEndConfig_DBC_Front.ReadOnly = true;
                        txtEndConfig_DBC_Front.ForeColor = Color.Blue;
                        txtEndConfig_DBC_Front.BackColor = txtMount_WallT_Front.BackColor;
                    }
                    else
                    {
                        txtEndConfig_DBC_Front.ReadOnly = false;

                        if (Math.Abs(pVal - pMean_Lim) < mcEPS)
                        {
                            txtEndConfig_DBC_Front.ForeColor = Color.Blue;
                        }
                        else
                        {
                            txtEndConfig_DBC_Front.ForeColor = Color.Black;
                        }
                        txtEndConfig_DBC_Front.BackColor = Color.White;
                    }
                }

                private void chkMount_DBC_Back_CheckedChanged(object sender, EventArgs e)
                //=======================================================================
                {
                    Double pULim = mMount_DBC_ULimit[1];
                    Double pLLim = mMount_DBC_LLimit[1];
                    Double pMean_Lim = 0.5 * (pULim + pLLim);

                    Double pVal = modMain.ConvTextToDouble(txtEndConfig_DBC_Back.Text);

                    if (chkMount_DBC_Back.Checked)
                    {
                        txtEndConfig_DBC_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(pMean_Lim);
                        txtEndConfig_DBC_Back.ReadOnly = true;
                        txtEndConfig_DBC_Back.ForeColor = Color.Blue;
                        txtEndConfig_DBC_Back.BackColor = txtMount_WallT_Front.BackColor;
                    }
                    else
                    {
                        txtEndConfig_DBC_Back.ReadOnly = false;

                        if (Math.Abs(pVal - pMean_Lim) < mcEPS)
                        {
                            txtEndConfig_DBC_Back.ForeColor = Color.Blue;
                        }
                        else
                        {
                            txtEndConfig_DBC_Back.ForeColor = Color.Black;
                        }
                        txtEndConfig_DBC_Back.BackColor = Color.White;
                    }
                }

                //private void cmbMount_HolesCount_Back_KeyDown(object sender, KeyEventArgs e)
                //{
                //    mblnMount_Holes_Count_Back_ManuallyChanged = true;
                //}

                //private void cmbMount_HolesCount_Front_KeyDown(object sender, KeyEventArgs e)
                //{
                //    mblnMount_Holes_Count_Front_ManuallyChanged = true;
                //}

                private void cmbMount_HolesCount_Front_MouseDown(object sender, MouseEventArgs e)
                //================================================================================
                {
                    mblnMount_Holes_Count_Front_ManuallyChanged = true;

                }

                private void cmbMount_HolesCount_Back_MouseDown(object sender, MouseEventArgs e)
                //================================================================================
                {
                    mblnMount_Holes_Count_Back_ManuallyChanged = true;
                }

                private void SL_TextChanged(object sender, EventArgs e)
                //========================================================
                {       
                    TextBox pTxtBox = (TextBox)sender;

                    switch (pTxtBox.Name)
                    {
                        case "txtSL_LScrew_Loc_Center":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.LScrew_Center = modMain.ConvTextToDouble(txtSL_LScrew_Loc_Center.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.LScrew_Center = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_LScrew_Loc_Center.Text));
                            }

                            SetBackColor_SL_Screw_Loc_Center();
                            break;

                        case "txtSL_LScrew_Loc_Back":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.LScrew_Back = modMain.ConvTextToDouble(txtSL_LScrew_Loc_Back.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.LScrew_Back = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_LScrew_Loc_Back.Text));
                            }

                            break;

                        case "txtSL_RScrew_Loc_Center":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.RScrew_Center = modMain.ConvTextToDouble(txtSL_RScrew_Loc_Center.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.RScrew_Center = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_RScrew_Loc_Center.Text));
                            }
                            SetBackColor_SL_Screw_Loc_Center();
                            break;

                        case "txtSL_RScrew_Loc_Back":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.RScrew_Back = modMain.ConvTextToDouble(txtSL_RScrew_Loc_Back.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.RScrew_Back = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_RScrew_Loc_Back.Text));
                            }
                            break;

                        case "txtSL_Screw_Spec_PN":
                            //-------------------------
                            mBearing_Radial_FP.SL.Screw.PN = txtSL_Screw_Spec_PN.Text;                       
                            break;

                        case "txtSL_CBore_Dia":
                            //-----------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_CBore_D = modMain.ConvTextToDouble(txtSL_CBore_Dia.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_CBore_D = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_CBore_Dia.Text));
                            }
                            SetBackColor_SL_Screw_Loc_Center();
                            break;

                        case "txtSL_CBore_DDrill":
                            //--------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_D_Drill = modMain.ConvTextToDouble(txtSL_CBore_DDrill.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_D_Drill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_CBore_DDrill.Text));
                            }
                            break;

                        case "txtSL_CBore_Depth":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_CBore_Depth = modMain.ConvTextToDouble(txtSL_CBore_Depth.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_CBore_Depth = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_CBore_Depth.Text));
                            }
                            SetBackColor_SL_Screw_Loc_Center();
                            break;

                        case "txtSL_Depth_TapDrill":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_Depth_TapDrill = modMain.ConvTextToDouble(txtSL_Depth_TapDrill.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_Depth_TapDrill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_Depth_TapDrill.Text));
                            }
                            break;

                        case "txtSL_Depth_Tap":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_Depth_Tap = modMain.ConvTextToDouble(txtSL_Depth_Tap.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_Depth_Tap = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_Depth_Tap.Text));
                            }
                            break;

                        case "txtSL_Depth_Engagement":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_Depth_Engagement = modMain.ConvTextToDouble(txtSL_Depth_Engagement.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Screw.Hole_Depth_Engagement = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_Depth_Engagement.Text));
                            }
                            break;

                        case "txtSL_LDowel_Loc_Center":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.LDowel_Loc_Center = modMain.ConvTextToDouble(txtSL_LDowel_Loc_Center.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.LDowel_Loc_Center = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_LDowel_Loc_Center.Text));
                            }
                            break;

                        case "txtSL_LDowel_Loc_Back":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.LDowel_Loc_Back = modMain.ConvTextToDouble(txtSL_LDowel_Loc_Back.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.LDowel_Loc_Back = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_LDowel_Loc_Back.Text));
                            }
                            break;

                        case "txtSL_RDowel_Loc_Center":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.RDowel_Loc_Center = modMain.ConvTextToDouble(txtSL_RDowel_Loc_Center.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.RDowel_Loc_Center = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_RDowel_Loc_Center.Text));
                            }
                            break;

                        case "txtSL_RDowel_Loc_Back":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.RDowel_Loc_Back = modMain.ConvTextToDouble(txtSL_RDowel_Loc_Back.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.RDowel_Loc_Back = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_RDowel_Loc_Back.Text));
                            }
                            break;

                        case "txtSL_Dowel_Depth_Up":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.Dowel.Hole_Depth_Up = modMain.ConvTextToDouble(txtSL_Dowel_Depth_Up.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Dowel.Hole_Depth_Up = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_Dowel_Depth_Up.Text));
                            }
                            break;

                        case "txtSL_Dowel_Depth_Low":
                            //-------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.SL.Dowel.Hole_Depth_Low = modMain.ConvTextToDouble(txtSL_Dowel_Depth_Low.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.SL.Dowel.Hole_Depth_Low = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtSL_Dowel_Depth_Low.Text));
                            }
                            break;
                    }
                }


                private void SetBackColor_SL_Screw_Loc_Center()
                //=============================================
                {
                    Double pULim = mBearing_Radial_FP.SL.Screw_Loc_Center_ULimit();
                    
                    if (mBearing_Radial_FP.SL.LScrew.Center > pULim)
                    {
                        txtSL_LScrew_Loc_Center.BackColor = Color.Red;
                    }
                    else
                    {
                        txtSL_LScrew_Loc_Center.BackColor = Color.White;
                    }

                    if (mBearing_Radial_FP.SL.RScrew.Center > pULim)
                    {
                        txtSL_RScrew_Loc_Center.BackColor = Color.Red;
                    }
                    else
                    {
                        txtSL_RScrew_Loc_Center.BackColor = Color.White;
                    }
                }

                private void Mounting_Front_TextChanged(object sender, EventArgs e)
                //=================================================================
                {
                    TextBox pTxtBox = (TextBox)sender;

                    switch (pTxtBox.Name)
                    {
                        case "txtMountHoles_CBore_DDrill_Front":
                            //----------------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.Mount.Screw[0].Hole_D_Drill = modMain.ConvTextToDouble(txtMountHoles_CBore_DDrill_Front.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[0].Hole_D_Drill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_DDrill_Front.Text));
                            }
                            break;

                        case "txtMountHoles_CBore_Dia_Front":
                            //-------------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.Mount.Screw[0].Hole_CBore_D = modMain.ConvTextToDouble(txtMountHoles_CBore_Dia_Front.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[0].Hole_CBore_D = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_Dia_Front.Text));
                            }
                            modMain.gProject.Product.Bearing = (clsBearing_Radial_FP)mBearing_Radial_FP.Clone();
                            mEndPlate_OD_LLimit[0] = mEndPlate[0].OD_LLimit(modMain.gProject.Product, 0);
                            SetBackColor_SealOD_Front();

                            mMount_DBC_ULimit[0] = mBearing_Radial_FP.Mount.DBC_ULimit(0);
                            lblEndConfig_DBC_Ulim_Front.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(mMount_DBC_ULimit[0]);

                            double pMountHole_DBC_MeanFront = (mBearing_Radial_FP.Mount.DBC_LLimit(modMain.gProject.Product, 0) + mBearing_Radial_FP.Mount.DBC_ULimit(0)) / 2;
                            if (chkMount_DBC_Front.Checked)
                            {
                                txtEndConfig_DBC_Front.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(pMountHole_DBC_MeanFront);
                            }
                            else
                            {
                                if (mBearing_Radial_FP.Mount.BC[0].D > modMain.gcEPS)
                                {
                                    txtEndConfig_DBC_Front.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing_Radial_FP.Mount.BC[0].D);
                                }
                                else
                                {
                                    txtEndConfig_DBC_Front.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(pMountHole_DBC_MeanFront);
                                }
                            }
                            SetBackColor_MountDBC_Front();

                            break;

                        case "txtMountHoles_CBore_Depth_Front":
                            //--------------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.Mount.Screw[0].Hole_CBore_Depth = modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Front.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[0].Hole_CBore_Depth = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Front.Text));
                            }

                            break;

                        case "txtMount_Holes_Depth_TapDrill_Front":
                            //-------------------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.Mount.Screw[0].Hole_Depth_TapDrill = modMain.ConvTextToDouble(txtMount_Holes_Depth_TapDrill_Front.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[0].Hole_Depth_TapDrill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMount_Holes_Depth_TapDrill_Front.Text));
                            }
                            break;


                        case "txtMount_Holes_Depth_Tap_Front":
                            //--------------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.Mount.Screw[0].Hole_Depth_Tap = modMain.ConvTextToDouble(txtMount_Holes_Depth_Tap_Front.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[0].Hole_Depth_Tap = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMount_Holes_Depth_Tap_Front.Text));
                            }
                            break;

                        case "txtMount_Holes_Depth_Engagement_Front":
                            //---------------------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.Mount.Screw[0].Hole_Depth_Engagement = modMain.ConvTextToDouble(txtMount_Holes_Depth_Engagement_Front.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[0].Hole_Depth_Engagement = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMount_Holes_Depth_Engagement_Front.Text));
                            }
                            break;
                    }

                }

                private void Mounting_Back_TextChanged(object sender, EventArgs e)
                //=================================================================
                {
                    TextBox pTxtBox = (TextBox)sender;

                    switch (pTxtBox.Name)
                    {
                        case "txtMountHoles_CBore_DDrill_Back":
                            //--------------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.Mount.Screw[1].Hole_D_Drill = modMain.ConvTextToDouble(txtMountHoles_CBore_DDrill_Back.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[1].Hole_D_Drill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_DDrill_Back.Text));
                            }
                            break;

                        case "txtMountHoles_CBore_Dia_Back":
                            //-----------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.Mount.Screw[1].Hole_CBore_D = modMain.ConvTextToDouble(txtMountHoles_CBore_Dia_Back.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[1].Hole_CBore_D = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_Dia_Back.Text));
                            }
                            modMain.gProject.Product.Bearing = (clsBearing_Radial_FP)mBearing_Radial_FP.Clone();
                            mEndPlate_OD_LLimit[1] = mEndPlate[1].OD_LLimit(modMain.gProject.Product, 1);
                            SetBackColor_SealOD_Back();

                             mMount_DBC_ULimit[1] = mBearing_Radial_FP.Mount.DBC_ULimit(1);
                            lblEndConfig_DBC_Ulim_Back.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(mMount_DBC_ULimit[1]);

                            double pMountHole_DBC_MeanBack = (mBearing_Radial_FP.Mount.DBC_LLimit(modMain.gProject.Product, 1) + mBearing_Radial_FP.Mount.DBC_ULimit(1)) / 2;
                            if (chkMount_DBC_Back.Checked)
                            {
                                txtEndConfig_DBC_Back.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(pMountHole_DBC_MeanBack);
                            }
                            else
                            {
                                if (mBearing_Radial_FP.Mount.BC[1].D > modMain.gcEPS)
                                {
                                    txtEndConfig_DBC_Back.Text = modMain.gProject.PNR.Unit.WriteInUserL_Eng(mBearing_Radial_FP.Mount.BC[1].D);
                                }
                                else
                                {
                                    txtEndConfig_DBC_Back.Text = modMain.gProject.Product.Unit.WriteInUserL_Eng(pMountHole_DBC_MeanBack);
                                }
                            }
                            SetBackColor_MountDBC_Back();
                            break;

                        case "txtMountHoles_CBore_Depth_Back":
                            //--------------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.Mount.Screw[1].Hole_CBore_Depth = modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Back.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[1].Hole_CBore_Depth = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMountHoles_CBore_Depth_Back.Text));
                            }
                            break;

                        case "txtMount_Holes_Depth_TapDrill_Back":
                            //-------------------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.Mount.Screw[1].Hole_Depth_TapDrill = modMain.ConvTextToDouble(txtMount_Holes_Depth_TapDrill_Back.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[1].Hole_Depth_TapDrill = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMount_Holes_Depth_TapDrill_Back.Text));
                            }
                            break;


                        case "txtMount_Holes_Depth_Tap_Back":
                            //-------------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.Mount.Screw[1].Hole_Depth_Tap = modMain.ConvTextToDouble(txtMount_Holes_Depth_Tap_Back.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[1].Hole_Depth_Tap = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMount_Holes_Depth_Tap_Back.Text));
                            }
                            break;

                        case "txtMount_Holes_Depth_Engagement_Back":
                            //-------------------------------------
                            if (mBearing_Radial_FP.SL.Screw.Spec.Unit.System == clsUnit.eSystem.English)
                            {
                                mBearing_Radial_FP.Mount.Screw[1].Hole_Depth_Engagement = modMain.ConvTextToDouble(txtMount_Holes_Depth_Engagement_Back.Text);
                            }
                            else
                            {
                                mBearing_Radial_FP.Mount.Screw[1].Hole_Depth_Engagement = modMain.gProject.PNR.Unit.CMet_Eng(modMain.ConvTextToDouble(txtMount_Holes_Depth_Engagement_Back.Text));
                            }
                            break;
                    }

                }

        #endregion

              
        #region "VALIDATION ROUTINES:"     
         

        #endregion         

           
      
    }
}
