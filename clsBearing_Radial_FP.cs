
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP                   '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                                '
//                                                                              '
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using EXCEL = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Linq;

namespace BearingCAD22
{
     [Serializable]
    public partial class clsBearing_Radial_FP : clsBearing_Radial
    {
         #region "NAMED CONSTANTS:"
        //========================

            //DESIGN PARAMETERS (Commented-out ones are not used internally):
            //------------------
            //....EDM Relief (used in Main Class & clsOilInlet).
            //private const Double mc_DESIGN_EDM_RELIEF = 0.010D;       

            //....Min. End Config. Depth (used in Main Class).
            //private const Double mc_DEPTH_END_CONFIG_MIN = 0.125D;  

            //........Used in EndConfig_DO_Max (), DFit().
            private const Double mc_DESIGN_DCLEAR = 0.002F;    //....Diametral Clearance between Bearing CB & End Plate OD)
            private const Double mcTWall_CB_EndPlate_Min = modMain.gcSep_Min;   

            //OTHERS:
            //------
            //....Others Angle Count 
            private const int mc_COUNT_MOUNT_HOLES_ANG_OTHER_MAX = 7;     // PB 11OCT18. Not used anywhere. Suppress.
              
        #endregion


        #region "ENUMERATION TYPES:"
        //==========================
            public enum eBolting { Front, Back, Both };   
        #endregion


        #region "MEMBER VARIABLES:"
        //========================

            protected clsProduct mCurrentProduct;
            private bool mSplitConfig;                              

            #region "Diameters:"
                //....Min.= 0 & Max.= 1:
                //  
                private Double[] mOD_Range = new Double[2];           //....OD  
                private Double[] mPadBore_Range = new Double[2];                
                private Double[] mBore_Range   = new Double[2];
                private Double[] mDShaft_Range = new Double[2];
            #endregion

            #region "Lengths:"
                private Double mL;
                private double[] mEndPlate_Depth = new Double[2];    
            #endregion

            #region "Materials:"
                private clsMaterial mMat = new clsMaterial();
                private Double mLiningT;                                //....Not included in clsMaterial.
            #endregion

            private clsPerformData mPerformData;

            private clsPad mPad;
            private clsFlexurePivot mFlexurePivot;
            private clsOilInlet mOilInlet;

            #region "Detailed Design Data:"
                private clsMillRelief mMillRelief;
                private clsSL mSL;
                private clsFlange mFlange;
                private clsARP mARP;
                private clsMount mMount;
                private clsEDM_Pad mEDM_Pad;      
            #endregion

        #endregion


        #region "CLASS PROPERTY ROUTINES:"
        //==============================

            //#region "NAMED CONSTANTS:"

                public Double DESIGN_DCLEAR               
                //===========================
                {
                    get { return mc_DESIGN_DCLEAR; }
                } 

            //    public Double DEPTH_END_CONFIG_MIN
            //    //=================================
            //    {
            //        get { return mc_DEPTH_END_CONFIG_MIN; }
            //    }


            public int COUNT_MOUNT_HOLES_ANG_OTHER_MAX
            {
                get { return mc_COUNT_MOUNT_HOLES_ANG_OTHER_MAX; }
            } 

            //#endregion


            #region "Split Configuration:"

                public bool SplitConfig
                {
                    get { return mSplitConfig; }
                    set { mSplitConfig = value; }
                }
            #endregion

           
            #region "Diameters:"
         
                //....OD:
                public Double[] OD_Range
                {
                    get { return mOD_Range; }
                    set { mOD_Range = value; }
                }

                //.... Pad Bore:             
                public Double[] PadBore_Range
                {
                    get { return mPadBore_Range; }
                    set { mPadBore_Range = value; }
                }

                //.... Bore:            
                public Double[] Bore_Range
                {
                    get { return mBore_Range; }
                    set { mBore_Range = value; }
                }

                //....Shaft Dia:
                public Double[] DShaft_Range
                {
                    get { return mDShaft_Range; }
                    set
                    {
                        mDShaft_Range = value;
                    }
                }

            #endregion


            #region "Lengths:"

                public Double L
                {
                    get { return mL; }
                    set { mL = value; }
                }


                #region "Depth - End Plates:"

                    public double[] Depth_EndPlate
                    {
                        get
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                if (mEndPlate_Depth[i] < modMain.gcEPS)
                                {
                                    mEndPlate_Depth[i] = Calc_Depth_EndPlate();
                                }
                            }
                            return mEndPlate_Depth;
                        }   
                    }

                #endregion

            #endregion


            #region "Materials:"

                public clsMaterial Mat
                {
                    get { return mMat; }
                    set { mMat = value; }
                }

                //.... Lining Thickness.
                public Double LiningT
                {
                    get
                    {
                         return mLiningT;                   
                    }
                    set { mLiningT = value; }
                }

            #endregion


            #region "PAD:"
            //============  

                public clsPad Pad
                {
                    get { return mPad; }
                    set { mPad = value; }
                }

            #endregion


            #region "FLEXURE PIVOT:"
            // ====================

                public clsFlexurePivot FlexurePivot
                {
                    get { return mFlexurePivot; }
                    set { mFlexurePivot = value; }
                }

            #endregion


            #region "OIL INLET:"

                public clsOilInlet OilInlet
                {
                    get { return mOilInlet; }
                    set { mOilInlet = value; }
                }

            #endregion


            #region "MILL RELIEF:"
            //===================

                public clsMillRelief MillRelief
                {
                    get { return mMillRelief; }
                    set { mMillRelief = value; }
                }

            #endregion


            #region "SPLIT LINE HARDWARE:"
            //===========================

                public clsSL SL
                {
                    get { return mSL; }
                    set { mSL = value; }
                }
            #endregion


            #region "FLANGE:"
            //===============

                public clsFlange Flange
                {
                    get { return mFlange; }
                    set { mFlange = value; }
                }

            #endregion


            #region "ANTI ROTATION PIN:"
            //==========================

                public clsARP ARP
                {
                    get { return mARP; }
                    set { mARP = value; }
                }

            #endregion


            #region "MOUNTING DETAILS:"
            //========================                          
  
                public clsMount Mount
                {
                    get { return mMount; }
                    set { mMount = value; }
                }

            #endregion


            #region "TEMP SENSOR HOLES:"
            //=========================

                //public clsTempSensor TempSensor
                //{
                //    get { return mTempSensor; }
                //    set { mTempSensor = value; }
                //}

            #endregion


            #region "EDM Pad:"
            //===============

                public clsEDM_Pad EDM_Pad
                {
                    get { return mEDM_Pad; }
                    set { mEDM_Pad = value; }
                }

            #endregion


            #region "PERFORMANCE DATA:"
            //========================

                public clsPerformData PerformData
                {
                    get { return mPerformData; }
                    set { mPerformData = value; }
                }

            #endregion

        #endregion


        #region "CONSTRUCTOR:"

                public clsBearing_Radial_FP(clsUnit.eSystem UnitSystem_In, eDesign Design_In, clsProduct CurrentProduct_In)
                    : base(UnitSystem_In, Design_In)
                //=========================================================================================================
                {
                    //....Instantiate member class objects: 
                    mPad = new clsPad(this);
                    mFlexurePivot = new clsFlexurePivot();
                    mOilInlet = new clsOilInlet(this);
                    mFlange = new clsFlange(this);
                    //mTempSensor = new clsTempSensor(this);
                    mMillRelief = new clsMillRelief(this);
                    mSL = new clsSL(this);
                    mARP = new clsARP(this);
                    mEDM_Pad = new clsEDM_Pad(this);
                    mMount = new clsMount(CurrentProduct_In);
                    mPerformData = new clsPerformData();

                    //....Initialize: 
                    mSplitConfig = true;

                    //........Material.
                    mMat.WCode_Base = "1002-107";
                    mMat.LiningExists = true;
                    mMat.WCode_Lining = "1002-960";
                
                    mCurrentProduct = CurrentProduct_In;
                }

        #endregion


        #region "CLASS METHODS:"
        //*********************

            #region "REF. / DEPENDENT VARIABLES:"

                #region "LENGTHS:"

                    public Double Calc_Depth_EndPlate()
                    //----------------------------------
                    {
                        //........Assumes equal depth on both sides as a starting estimate. 
                        double pDepth = 0.0F;
                        pDepth = (mL - (mPad.L + mMillRelief.AxialSealGap[0] + mMillRelief.AxialSealGap[1])) * 0.5F;    
                        return pDepth;
                    }

                    public Double DCB_EndPlate_Max()
                    //==============================          
                    {
                        Double pDCB_Max = 0;   
                        pDCB_Max = OD() - 2 * mcTWall_CB_EndPlate_Min;

                        return pDCB_Max;
                    }

                    public Double DCB_EndPlate(int Indx_In)           
                    //================================                 
                    {                        
                        Double pDCB = 0.0;    
                        pDCB = mMount.EndPlateOD[Indx_In] + mc_DESIGN_DCLEAR;                        
                        return pDCB;
                    }

                    public Double TWall_CB_EndPlate(int Indx_In)      
                    //==========================================           
                    {                        
                        Double pTWall = 0;
                        pTWall = 0.5 * (OD() - DCB_EndPlate(Indx_In));

                        return pTWall;
                    }   

                #endregion


                #region "DIAMETERS:"

                    //....Nominal 
         
                    public Double OD()
                    //----------------
                    {
                        return modMain.Nom_Val(mOD_Range);
                    }

                    public Double PadBore()
                    //---------------------
                    {
                        return modMain.Nom_Val(mPadBore_Range);
                    }

                    public Double Bore()
                    //-------------------
                    {
                        return modMain.Nom_Val(mBore_Range);
                    }

                    public Double DShaft()
                    //---------------------
                    {
                        return modMain.Nom_Val(mDShaft_Range);
                    }    

                    //public Double Clearance()  
                    ////-----------------------
                    //{
                    //    return (mBore_Range[0] - mDShaft_Range[0]);
                    //}


                    //public Double PreLoad()
                    ////---------------------
                    //{
                    //    Double pPreLoad = 0.0f;
                    //    pPreLoad = (PadBore() - Bore()) / (PadBore() - DShaft());

                    //    return pPreLoad;
                    //}

                #endregion        

            #endregion      


            #region "VALIDATION ROUTINE:"

                //public Double Validate_Depth_EndConfig (Double Depth_In)
                ////======================================================
                //{
                //    if (Depth_In < mc_DEPTH_END_CONFIG_MIN)
                //    {
                //        string pMsg = " End config. depth should not be less than the design minimum value of, " 
                //                      + mc_DEPTH_END_CONFIG_MIN + ".";
                //        MessageBox.Show(pMsg);

                //        Depth_In = mc_DEPTH_END_CONFIG_MIN;
                //    }

                //    return Depth_In;
                //}

            #endregion


            #region"CLASS OBJECTS COPYING & COMPARISON ROUTINES:"

            public bool Compare(clsBearing_Radial_FP Bearing_In, string FormName_In)
            //=======================================================================   
            {
                bool mblnVal_Changed = false;
                //int pRetValue = 0;

                //if (FormName_In == "Bearing")
                //{

                //    if (modMain.CompareVar(Bearing_In.SplitConfig, mSplitConfig, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    for (int i = 0; i < 2; i++)
                //    {
                //        if (modMain.CompareVar(Bearing_In.DShaft_Range[i], mDShaft_Range[i], 4, pRetValue) > 0)
                //        {
                //            mblnVal_Changed = true;
                //        }

                //        if (modMain.CompareVar(Bearing_In.DFit_Range[i], mDFit_Range[i], 4, pRetValue) > 0)
                //        {
                //            mblnVal_Changed = true;
                //        }

                //        if (modMain.CompareVar(Bearing_In.DPad_Range[i], mDPad_Range[i], 4, pRetValue) > 0)
                //        {
                //            mblnVal_Changed = true;
                //        }

                //        if (modMain.CompareVar(Bearing_In.DSet_Range[i], mDSet_Range[i], 4, pRetValue) > 0)
                //        {
                //            mblnVal_Changed = true;
                //        }

                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.Count, mPad.Count, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.L, mPad.L, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    //if (modMain.CompareVar(Bearing_In.Pad.Ang, mPad.Ang, 0, pRetValue) > 0)
                //    //{
                //    //    mblnVal_Changed = true;
                //    //}

                //    //if (modMain.CompareVar(Bearing_In.Pad.RFillet, mPad.RFillet, 3, pRetValue) > 0)
                //    //{
                //    //    mblnVal_Changed = true;
                //    //}

                //    if (modMain.CompareVar(Bearing_In.Pad.Pivot.AngStart, mPad.Pivot.AngStart, 0, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.Pivot.Offset, mPad.Pivot.Offset, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.T.Lead, mPad.T.Lead, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.T.Pivot, mPad.T.Pivot, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.T.Trail, mPad.T.Trail, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.FlexurePivot.Web.T, mFlexurePivot.Web.T, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.FlexurePivot.Web.RFillet, mFlexurePivot.Web.RFillet, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.FlexurePivot.Web.H, mFlexurePivot.Web.H, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.FlexurePivot.GapEDM, mFlexurePivot.GapEDM, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.FlexurePivot.Rot_Stiff, mFlexurePivot.Rot_Stiff, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.OilInlet.Orifice.D, mOilInlet.Orifice.D, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Mat.Base, mMat.Base, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Mat.Lining, mMat.Lining, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Mat.LiningExists, mMat.LiningExists, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.LiningT, mLiningT, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad.Type.ToString(), mPad.Type.ToString(), pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    //if (modMain.CompareVar(Bearing_In.L_Tot, mL_Tot,4, pRetValue) > 0)
                //    //{
                //    //    mblnVal_Changed = true;         
                //    //}

                //}

                //else if (FormName_In == "Performance")
                //{

                //    if (modMain.CompareVar(Bearing_In.mPerformData.Power_HP, mPower_HP, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.FlowReqd_gpm, mFlowReqd_gpm, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempRise_F, mTempRise_F, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.TFilm_Min, mTFilm_Min, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad_Perform_Max.TempRise, mPad_Perform_Max.TempRise, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.Pad_Perform_Max.Press, mPad_Perform_Max.Press, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad_Perform_Max.Rot, mPad_Perform_Max.Rot, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.Pad_Perform_Max.Load, mPad_Perform_Max.Load, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.Pad_Perform_Max.Stress, mPad_Perform_Max.Stress, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //}

                //else if (FormName_In == "Bearing Design Details")
                //{

                //    if (modMain.CompareVar(Bearing_In.L, mL, 4, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.OilInlet.Count_MainOilSupply, mOilInlet.Count_MainOilSupply, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.OilInlet.Annulus.Ratio_L_H, mOilInlet.Annulus.Ratio_L_H, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.OilInlet.Annulus.L, mOilInlet.Annulus.L, 3, pRetValue) > 0) //SB 15MAR10
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.OilInlet.Annulus.D, mOilInlet.Annulus.D, 3, pRetValue) > 0) //SB 15MAR10
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.OilInlet.OrificeStartPos.ToString(), mOilInlet.OrificeStartPos.ToString(), pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.MillRelief.Exists, mMillRelief.Exists, pRetValue) > 0)    //SB 26MAY09
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    //if (modMain.CompareVar(Bearing_In.MillRelief.D, mMillRelief.D,3, pRetValue) > 0)  //SG 09JAN12 Review
                //    //{
                //    //    mblnVal_Changed = true;
                //    //}

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Screw_Spec.Type, mSplitLine_Screw_Spec.Type, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Screw_Spec.D_Desig, mSplitLine_Screw_Spec.D_Desig, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }
                //    if (modMain.CompareVar(Bearing_In.SplitLine_Screw_Spec.Pitch, mSplitLine_Screw_Spec.Pitch, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Screw_Spec.L, mSplitLine_Screw_Spec.L, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Screw_Spec.Mat, mSplitLine_Screw_Spec.Mat, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Dowel_Spec.Type, mSplitLine_Dowel_Spec.Type, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Dowel_Spec.D_Desig, mSplitLine_Dowel_Spec.D_Desig, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Dowel_Spec.L, mSplitLine_Dowel_Spec.L, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SplitLine_Dowel_Spec.Mat, mSplitLine_Dowel_Spec.Mat, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Spec.Type, mAntiRot_Pin_Spec.Type, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Spec.D_Desig, mAntiRot_Pin_Spec.D_Desig, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Spec.L, mAntiRot_Pin_Spec.L, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Spec.Mat, mAntiRot_Pin_Spec.Mat, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Loc.Angle, mAntiRot_Pin_Loc.Angle, 0, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Loc.Dist_Front, mAntiRot_Pin_Loc.Dist_Front, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    //if (modMain.CompareVar(Bearing_In.AntiRotPin.Loc.From_BearingSplit.ToString(), mAntiRotPin.Loc.From_BearingSplit.ToString(), pRetValue) > 0)
                //    //{
                //    //    mblnVal_Changed = true;
                //    //}

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Loc.Bearing_Vert.ToString(), mAntiRot_Pin_Loc.Bearing_Vert.ToString(), pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Loc.Casing_SL.ToString(), mAntiRot_Pin_Loc.Casing_SL.ToString(), pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.AntiRot_Pin_Loc.Offset, mAntiRot_Pin_Loc.Offset, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.FrontMountingHoles.GoThru, mEndConfig_MountingHoles_Front.GoThru, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.FrontMountingHoles.Depth, mEndConfig_MountingHoles_Front.Depth, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.PartNo, mEndConfig_MountFixture_Sel_Front.PartNo, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.DBC, mEndConfig_MountFixture_Sel_Front.DBC, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.D_Finish, mEndConfig_MountFixture_Sel_Front.D_Finish, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.PartNo, mEndConfig_MountFixture_Sel_Front.PartNo, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.Hole.Count, mEndConfig_MountFixture_Sel_Front.Hole.Count, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.HolesEqiSpaced, mEndConfig_MountFixture_Sel_Front.HolesEqiSpaced, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.HolesAngStart, mEndConfig_MountFixture_Sel_Front.HolesAngStart, 0, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.HolesAngStart_Comp_Chosen, mEndConfig_MountFixture_Sel_Front.HolesAngStart_Comp_Chosen, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    for (int i = 0; i < (mc_COUNT_MOUNT_HOLES_ANG_OTHER_MAX); i++)
                //        if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Front.HolesAngOther[i], mEndConfig_MountFixture_Sel_Front.HolesAngOther[i], 0, pRetValue) > 0)
                //        {
                //            mblnVal_Changed = true;
                //        }


                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Thread_Front.Type, mEndConfig_MountFixture_Sel_Front_Thread.Type, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Thread_Front.D_Desig, mEndConfig_MountFixture_Sel_Front_Thread.D_Desig, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Thread_Front.L, mEndConfig_MountFixture_Sel_Front_Thread.L, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Thread_Front.Pitch, mEndConfig_MountFixture_Sel_Front_Thread.Pitch, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.SealMountFixture_Sel_Thread_Front.Mat, mEndConfig_MountFixture_Sel_Front_Thread.Mat, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempSensorHoles.CanLength, mTempSensorHoles.CanLength, 3, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempSensorHoles.Exists, mTempSensorHoles.Exists, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempSensorHoles.Loc.ToString(), mTempSensorHoles.Loc.ToString(), pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempSensorHoles.Count, mTempSensorHoles.Count, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempSensorHoles.D, mTempSensorHoles.D, 4, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //    if (modMain.CompareVar(Bearing_In.TempSensorHoles.AngStart, mTempSensorHoles.AngStart, 0, pRetValue) > 0)
                //    {
                //        mblnVal_Changed = true;
                //    }

                //}

                return mblnVal_Changed;

            }


            //public void Clone(ref clsBearing_Radial_FP Bearing_In)
            ////=====================================================   
            //{
            //    //....Set SplitConfig.
            //    Bearing_In.SplitConfig = mSplitConfig;
            //    //Bearing_In.TypeRadial = mTypeRadial;   //SB 10JUL09
            //    //Bearing_In.Config = mConfig;           //SB 10JUL09

            //    Double[] pDShaft_Range = new Double[2];    //BG 24JUN11
            //    Double[] pDFit_Range = new Double[2];        //BG 24JUN11
            //    Double[] pDPad_Range = new Double[2];
            //    Double[] pDSet_Range = new Double[2];

            //    //....Set DFit,DSet,DShaft,DPad.
            //    for (int i = 0; i < 2; i++)
            //    {

            //        pDShaft_Range[i] = mDShaft_Range[i];
            //        pDFit_Range[i] = mDFit_Range[i];
            //        pDPad_Range[i] = mDPad_Range[i];
            //        pDSet_Range[i] = mDSet_Range[i];
            //    }

            //    Bearing_In.DShaft_Range = pDShaft_Range;
            //    Bearing_In.DFit_Range = pDFit_Range;
            //    Bearing_In.DPad_Range = pDPad_Range;
            //    Bearing_In.DSet_Range = pDSet_Range;
            //    Bearing_In.UnitSystem = mUnitSystem;
            //    //Bearing_In.L_Tot = mL_Tot;

            //    //  Pad
            //    //  ---
            //    Bearing_In.PadType = mPad.Type;
            //    Bearing_In.PadCount = mPad.Count;
            //    Bearing_In.PadL = mPad.L;
            //    //Bearing_In.PadAng = mPad.Ang;          
            //    Bearing_In.PadPivot_Offset = mPad.Pivot.Offset;
            //    Bearing_In.PadPivot_AngStart = mPad.Pivot.AngStart;
            //    Bearing_In.PadT_Lead = mPad.T.Lead;
            //    Bearing_In.PadT_Pivot = mPad.T.Pivot;
            //    Bearing_In.PadT_Trail = mPad.T.Trail;
            //    Bearing_In.PadRFillet = mPad.RFillet;
            //    Bearing_In.PadType = mPad.Type;

            //    //  Flexure Pivot
            //    //  -------------
            //    Bearing_In.FlexPivot_Web_T = mFlexPivot.Web.T;
            //    Bearing_In.FlexPivot_Web_H = mFlexPivot.Web.H;
            //    Bearing_In.FlexPivot_Web_RFillet = mFlexPivot.Web.RFillet;
            //    Bearing_In.FlexPivot_GapEDM = mFlexPivot.GapEDM;
            //    Bearing_In.FlexPivot_Rot_Stiff = mFlexPivot.Rot_Stiff;

            //    //  OilInlet
            //    //  --------
            //    Bearing_In.OilInlet_Orifice_D = mOilInlet.Orifice.D;

            //    //  Material
            //    //  ---------
            //    Bearing_In.Mat.Base = mMat.Base;
            //    Bearing_In.Mat.Lining_Mat = mMat.Lining.Lining;
            //    Bearing_In.Mat.Lining_Exists = mMat.Lining.Exists; //SB 08JUL09

            //    //....Lining Thickness
            //    Bearing_In.LiningT = mLiningT;

            //    // Performence Data
            //    // ----------------
            //    Bearing_In.Power_HP = mPower_HP;          //....Power
            //    Bearing_In.FlowReqd_gpm = mFlowReqd_gpm;  //....Flow Reqd
            //    Bearing_In.TempRise_F = mTempRise_F;      //....Temp Rise
            //    Bearing_In.TFilm_Min = mTFilm_Min;        //....TFilmMin

            //    //  Pad Max
            //    //  -------
            //    Bearing_In.Pad_Perform_MaxLoad = mPad_Perform_Max.Load;           //....Temp
            //    Bearing_In.Pad_Perform_MaxPress = mPad_Perform_Max.Press;         //....Pressure
            //    Bearing_In.Pad_Perform_MaxRot = mPad_Perform_Max.Rot;             //....Rotation
            //    Bearing_In.Pad_Perform_MaxStress = mPad_Perform_Max.Stress;       //....Load
            //    Bearing_In.Pad_Perform_MaxTempRise = mPad_Perform_Max.TempRise;   //....Stress


            //    //  OD Length
            //    //  ---------
            //    Bearing_In.L = mL;

            //    //  OilInlet
            //    //  --------
            //    Bearing_In.OilInlet_Count_MainOilSupply = mOilInlet.Count_MainOilSupply;
            //    Bearing_In.OilInlet_AnnulusRatio_L_H = mOilInlet.Annulus.Ratio_L_H;
            //    Bearing_In.OilInlet_Annulus_D = mOilInlet.Annulus.D;
            //    Bearing_In.OilInlet_Annulus_L = mOilInlet.Annulus.L;
            //    Bearing_In.OilInlet_OrificeStartPos = mOilInlet.OrificeStartPos;

            //    //  WebRelief
            //    //  ---------
            //    Bearing_In.MillRelief_Exists = mMillRelief.Exists;
            //    Bearing_In.MillRelief_D_Desig = mMillRelief.D_Desig;

            //    //  SplitLine HardWare
            //    //  ------------------
            //    //....Thread.
            //    Bearing_In.SplitLine_Screw_Spec.Type = mSplitLine_Screw_Spec.Type;
            //    Bearing_In.SplitLine_Screw_Spec.D_Desig = mSplitLine_Screw_Spec.D_Desig;
            //    Bearing_In.SplitLine_Screw_Spec.Pitch = mSplitLine_Screw_Spec.Pitch;
            //    Bearing_In.SplitLine_Screw_Spec.L = mSplitLine_Screw_Spec.L;
            //    Bearing_In.SplitLine_Screw_Spec.Mat = mSplitLine_Screw_Spec.Mat;

            //    //....Dowel Pin.
            //    Bearing_In.SplitLine_Dowel_Spec.Type = mSplitLine_Dowel_Spec.Type;
            //    Bearing_In.SplitLine_Dowel_Spec.D_Desig = mSplitLine_Dowel_Spec.D_Desig;
            //    Bearing_In.SplitLine_Dowel_Spec.L = mSplitLine_Dowel_Spec.L;
            //    Bearing_In.SplitLine_Dowel_Spec.Mat = mSplitLine_Dowel_Spec.Mat;

            //    //  Anti Rotation Pin
            //    //  ------------------
            //    Bearing_In.AntiRotPin_Loc_Angle = mAntiRot_Pin_Loc.Angle;
            //    //Bearing_In.AntiRot_PinLoc_Dist_FromFront = mAntiRot_PinLoc.Dist_Front;
            //    Bearing_In.AntiRotPin_Loc_Dist_Front = mAntiRot_Pin_Loc.Dist_Front;
            //    //Bearing_In.AntiRotPin_Loc_From_BearingSplit = mAntiRotPin.Loc.From_BearingSplit;
            //    Bearing_In.AntiRotPin_Loc_Bearing_Vert = mAntiRot_Pin_Loc.Bearing_Vert;
            //    Bearing_In.AntiRotPin_Loc_Casing_SL = mAntiRot_Pin_Loc.Casing_SL;
            //    //Bearing_In.AntiRot_PinLoc_Offset = mAntiRot_PinLoc.Offset;
            //    Bearing_In.AntiRotPin_Loc_Offset = mAntiRot_Pin_Loc.Offset;

            //    //....Anti Rotation Pin.
            //    Bearing_In.AntiRot_Pin_Spec.Type = mAntiRot_Pin_Spec.Type;
            //    Bearing_In.AntiRot_Pin_Spec.D_Desig = mAntiRot_Pin_Spec.D_Desig;
            //    Bearing_In.AntiRot_Pin_Spec.L = mAntiRot_Pin_Spec.L;
            //    Bearing_In.AntiRot_Pin_Spec.Mat = mAntiRot_Pin_Spec.Mat;

            //    //  Seal Mounting Hole
            //    //  ------------------

            //    Bearing_In.FrontMountingHoles_GoThru = mEndConfig_MountingHoles_Front.GoThru;
            //    Bearing_In.FrontMountingHoles_Depth = mEndConfig_MountingHoles_Front.Depth;

            //    //  Seal Mounting Hole Selected
            //    //  ----------------------------                       
            //    Bearing_In.SealMountFixture_Candidates_Chosen_Front = mEndConfig_MountFixture_Candidadtes_Chosen_Front;
            //    Bearing_In.Retrieve_CandidateMountFixtures(mEndCofig_MountFixture_Candidadtes_Front);

            //    Bearing_In.SealMountFixture_Sel_Front_PartNo = mEndConfig_MountFixture_Sel_Front.PartNo;
            //    Bearing_In.SealMountFixture_Sel_Front_DBC = mEndConfig_MountFixture_Sel_Front.DBC;
            //    Bearing_In.SealMountFixture_Sel_Front_D_Finish = mEndConfig_MountFixture_Sel_Front.D_Finish;
            //    Bearing_In.SealMountFixture_Sel_Front_HolesAngStart_Comp_Chosen = mEndConfig_MountFixture_Sel_Front.HolesAngStart_Comp_Chosen;
            //    Bearing_In.SealMountFixture_Sel_Front_HolesCount = mEndConfig_MountFixture_Sel_Front.Hole.Count;
            //    Bearing_In.SealMountFixture_Sel_Front_HolesEqiSpaced = mEndConfig_MountFixture_Sel_Front.HolesEqiSpaced;
            //    Bearing_In.SealMountFixture_Sel_Front_HolesAngStart = mEndConfig_MountFixture_Sel_Front.HolesAngStart;

            //    Double[] pSealMountFixture_Sel_HolesAngOther = new Double[7];
            //    for (int i = 0; i < Bearing_In.SealMountFixture_Sel_Front.Hole.Count - 1; i++)
            //        pSealMountFixture_Sel_HolesAngOther[i] = mEndConfig_MountFixture_Sel_Front.HolesAngOther[i];

            //    Bearing_In.SealMountFixture_Sel_Front_HolesAngOther = (Double[])pSealMountFixture_Sel_HolesAngOther.Clone(); //SB 09JUL09

            //    //....MountHole Thread
            //    Bearing_In.SealMountFixture_Sel_Front_Thread_Type = mEndConfig_MountFixture_Sel_Front_Thread.Type;
            //    Bearing_In.SealMountFixture_Sel_Front_D_Desig = mEndConfig_MountFixture_Sel_Front_Thread.D_Desig;
            //    Bearing_In.SealMountFixture_Sel_Front_Thread_Pitch = mEndConfig_MountFixture_Sel_Front_Thread.Pitch;
            //    Bearing_In.SealMountFixture_Sel_Front_Thread_L = mEndConfig_MountFixture_Sel_Front_Thread.L;
            //    Bearing_In.SealMountFixture_Sel_Front_Thread_Mat = mEndConfig_MountFixture_Sel_Front_Thread.Mat;

            //    //  Temp Sensor Holes
            //    //  -----------------
            //    Bearing_In.TempSensorHoles_Exists = mTempSensorHoles.Exists;
            //    Bearing_In.TempSensorHoles_Count = mTempSensorHoles.Count;
            //    Bearing_In.TempSensorHoles_D = mTempSensorHoles.D;
            //    Bearing_In.TempSensorHoles_Loc = mTempSensorHoles.Loc;
            //    Bearing_In.TempSensorHoles_AngStart = mTempSensorHoles.AngStart;
            //}

            #endregion


            #region "ICLONEABLE MEMBERS:"
            //==========================

                public object Clone()
                //===================
                {
                    //return this.MemberwiseClone();

                    BinaryFormatter pBinSerializer;
                    StreamingContext pStreamContext;

                    pStreamContext = new StreamingContext(StreamingContextStates.Clone);
                    pBinSerializer = new BinaryFormatter(null, pStreamContext);

                    MemoryStream pMemBuffer;
                    pMemBuffer = new MemoryStream();

                    //....Serialize the object into the memory stream
                    pBinSerializer.Serialize(pMemBuffer, this);

                    //....Move the stream pointer to the beginning of the memory stream
                    pMemBuffer.Seek(0, SeekOrigin.Begin);


                    //....Get the serialized object from the memory stream
                    Object pobjClone;
                    pobjClone = pBinSerializer.Deserialize(pMemBuffer);
                    pMemBuffer.Close();   //....Release the memory stream.

                    return pobjClone;    //.... Return the deeply cloned object.
                }

            #endregion


        #endregion
 

    }
}

