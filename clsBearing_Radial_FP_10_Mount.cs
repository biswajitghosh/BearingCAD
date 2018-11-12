//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_10_Mount          '
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
    public partial class clsBearing_Radial_FP : clsBearing_Radial
    {
        [Serializable]
        public class clsMount
        //====================
        {
            #region "NAMED CONSTANTS:"
            ////========================

            //DESIGN PARAMETERS:           
            //------------------
            //....CLEARANCE BETWEEN SCREW & HOLES:
            //
                //....Diametral Clearance between the Mounting Screw Head & Counter Bore.         
                //private const Double mc_DESIGN_CBORE_CLEARANCE = 0.030F;

                ////....Diametral Clearance between the Mounting Screw & Thru' Hole Dia.       
                //private const Double mc_DESIGN_THRU_HOLE_CLEARANCE = 0.030F;

            ////....COUNTER BORE DEPTH LIMIT MARGINS:
            //
            ////....Lower Limit Margin: w.r.t Mounting Screw Head Height.
            //private const Double mc_DESIGN_CBORE_DEPTH_MARGIN_LOWER_LIM = 0.015F;

            ////....Upper Limit Margin: w.r.t Seal Length.
            //private const Double mc_DESIGN_CBORE_DEPTH_MARGIN_UPPER_LIM = 0.030F;

            #endregion

            #region "USER-DEFINED STRUCTURES:"
            //================================

            [Serializable]
            public struct sBC                   
            {
                public Double D;
                public int Count;
                public bool EquiSpaced;
                public Double AngStart;
                public Double[] AngBet;
            }

            #endregion

            #region "MEMBER VARIABLES:"
            //=========================
                private clsProduct mCurrentProduct;
                private eBolting mBolting;              

                //....If GoThru = TRUE,  Depth = 0 (irrelevant).
                //........        FALSE, Depth > 0. 
                //
                public sBC[] mBC = new sBC[2];    
                private clsScrew[] mScrew = new clsScrew[2];
                private double[] mEndPlateOD = new double[2];   // PB 21OCT18. May not be necessary. Keep it for now and review later.    
            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //===============================
                public eBolting Bolting
                {
                    get { return mBolting; }
                    set { mBolting = value; }
                }

                public clsScrew[] Screw             
                {
                    get { return mScrew; }
                }

                public sBC[] BC
                {
                    get { return mBC; }
                    set { mBC = value; }
                }

                public double[] EndPlateOD
                {
                    get { return mEndPlateOD; }
                    set { mEndPlateOD = value; }
                }

            #endregion


            #region "CONSTRUCTOR:"

                public clsMount(clsProduct CurrentProduct_In)
                //===========================================
                {
                    mCurrentProduct = CurrentProduct_In;
                    mBolting = eBolting.Both;  
                       
                    //....Initialize:
                    for (int i = 0; i < 2; i++)                     //....i = 0: FRONT, = 1: BACK.
                    {
                        mScrew[i] = new clsScrew(mCurrentProduct.Unit.System);
                        mBC[i].EquiSpaced = true;
                        mBC[i].Count = 4;
                        mBC[i].AngStart = 45;                        
                    } 
                }

            #endregion


            #region "CLASS METHODS":

                public Double DBC_LLimit(clsProduct Product_In, int Indx_In)
                //===========================================================                
                {
                    mCurrentProduct = Product_In;
                    return (((clsBearing_Radial_FP)mCurrentProduct.Bearing).MillRelief.D_PadRelief() + 2 * modMain.gcSep_Min + mScrew[Indx_In].D());
                }

                public Double DBC_ULimit(int Indx_In)
                //====================================                 
                {                    
                    return (mEndPlateOD[Indx_In] - 2 * modMain.gcSep_Min - mScrew[Indx_In].Hole.CBore.D);
                   // return (mCurrentProduct.EndPlate[Indx_In].OD - 2 * modMain.gcSep_Min - mScrew[Indx_In].Hole.CBore.D);
                }

                public Double Mount_Sel_AngBet(int Indx_In)
                //==========================================
                {
                    return (360.0F / mBC[Indx_In].Count);
                }

                public Double Screw_L_ULimit(int Indx_In)       
                //========================================   
                {
                    //PB 10FEB12. This routine is valid for Go Thru' only. Ask Harout.

                    //clsScrew pScrew_Spec = mFixture_Screw_Spec[Indx_In];
                    clsScrew pScrew = mScrew[Indx_In];        

                    //....Get Screw Head Height.
                    //
                    Double pL_UpperLimit = 0.0F;
                    //string pD_Desig = pScrew.Spec.D_Desig;
                    //Double pHead_H = pScrew.Head_H;
                    

                    //if (pD_Desig != "" && pD_Desig != null)
                    //{

                    //    if (pD_Desig.Contains("M"))     //....Metric
                    //    {
                    //        //....Convert Head_H from mm ==> in.
                    //        pHead_H = pHead_H / 25.4F;                          //....in.
                    //        pL_UpperLimit = mCurrent_Bearing_Radial_FP.mCurrentProduct.L_Tot() - pHead_H - (2 * 0.020F);
                    //        pL_UpperLimit = pL_UpperLimit * 25.4F;              //....mm
                    //    }

                    //    else                            //....English.
                    //    {
                    //        pL_UpperLimit = mCurrent_Bearing_Radial_FP.mCurrentProduct.L_Tot() - pHead_H - (2 * 0.020F);   //....in
                    //    }
                    //}

                    return pL_UpperLimit;
                }


                public Double Screw_L_LLimit(int Indx_In)              
                //=========================================
                {
                    //....Relevant Radial Bearing Parameter:
                    //clsScrew pScrew_Spec = Fixture_Screw_Spec[Indx_In];
                    clsScrew pScrew_Spec = mScrew[Indx_In]; 

                    Double pLowerLimit = 0.0F;
                    //double pBearing_Pad_L = mCurrent_Bearing_Radial_FP.Pad.L;

                    ////....Relevant End Config Parameter:
                    //double pEndConfigL = mCurrent_Bearing_Radial_FP.mCurrentProduct.EndPlate[Indx_In].L;

                    ////....Mounting Screw Parameters:
                    //string pD_Desig = pScrew_Spec.Spec_D_Desig;
                    //Double pHead_H = pScrew_Spec.Head_H;
                    //Double pD = pScrew_Spec.Spec_D;

                    //if (pD_Desig != "" && pD_Desig != null)
                    //{
                    //    if (pD_Desig.Contains("M"))                             //....Metric
                    //    {
                    //        //....Convert from mm ==> in.
                    //        pHead_H = pHead_H / 25.4F;          //....in 
                    //        pD = pD / 25.4F;

                    //        pLowerLimit = (pEndConfigL + pBearing_Pad_L) - pHead_H - 0.020F + 1.5F * pD;
                    //        pLowerLimit = pLowerLimit * 25.4F;  //....mm
                    //    }

                    //    else                                                    //....English.
                    //    {
                    //        pLowerLimit = (pEndConfigL + pBearing_Pad_L) - pHead_H - 0.020F + 1.5F * pD;
                    //    }
                    //}

                    return pLowerLimit;
                }

                public Double Screw_Hole_CBore_LLimit()                   // PB 12OCT18. Not used yet. To be modified. Screw_Hole_CBore_LLimit
                //-------------------------------------         
                {
                    //Double pHead_H;

                    //if (mScrew_Spec.D_Desig.Contains("M"))          //....Metric.
                    //    //....Convert from mm ==> in.
                    //    pHead_H = mScrew_Spec.Head_H / 25.4F;

                    //else                                            //....English.
                    //    pHead_H = mScrew_Spec.Head_H;


                    //Double pLowerLimit = 0.0F;
                    //Double pMargin = mc_DESIGN_CBORE_DEPTH_MARGIN_LOWER_LIM;

                    //pLowerLimit = pHead_H + pMargin;

                    //return pLowerLimit;

                    return 0;
                }

                public Double Screw_Hole_CBore_ULimit()                   // PB 12OCT18. Not used yet. To be modified. 
                //------------------------------------   
                {
                    //Double pUpperLimit;
                    //Double pMargin = mc_DESIGN_CBORE_DEPTH_MARGIN_UPPER_LIM;
                    //pUpperLimit = mCurrentEndConfig.L - pMargin;

                    //return pUpperLimit;
                    return 0;
                }

                #region "VALIDATION ROUTINE:"

                    public Double Validate_Holes_Thread_Depth (int Indx_In, Double Depth_In)
                    //======================================================================
                    {
                        //....This function is used only when Go Thru' = No;
                        double pDepth_Lim = Depth_In;

                        ////....Establish the lower & upper limits of the thread depth.
                        //double pDepth_LLim, pDepth_ULim;
                        //pDepth_LLim = 1.5 * mFixture[Indx_In].Screw_Spec.Spec_D;

                        //double pDepth_TapDrill_Max;
                        //pDepth_TapDrill_Max = 0.5F * mCurrent_Bearing_Radial_FP.Pad.L - 0.125;
                        //pDepth_ULim         = pDepth_TapDrill_Max - 0.0625;

                       

                        //string pMsg;

                        //if (Depth_In < pDepth_LLim)
                        //{
                        //    pMsg = "Mount holes thread depth should not be less than 1.5 X Thread Dia.";
                        //    MessageBox.Show(pMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    pDepth_Lim = pDepth_LLim;
                        //}

                        //if (Depth_In > pDepth_ULim)
                        //{
                        //    pMsg = "Mount holes thread depth should not exceed the mid-point of the pad length.";
                        //    MessageBox.Show(pMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    pDepth_Lim = pDepth_ULim;
                        //}

                        return pDepth_Lim;
                    }

                #endregion

            #endregion
        }
    }
}
