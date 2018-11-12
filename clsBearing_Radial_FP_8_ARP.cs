//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_8_AntiRotPin      '
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

namespace BearingCAD22
{
    public partial class clsBearing_Radial_FP : clsBearing_Radial
    {
        [Serializable]
        public class clsARP
        {
            #region "ENUMERATION TYPES:"
            //==========================
                public enum eInsertedOn { BearingOD, Flange };
            #endregion

            #region "MEMBER VARIABLES:"
            //==========================
                private clsBearing_Radial_FP mCurrent_Bearing_Radial_FP;

                private Double mLoc_Back;
                private Double mAng_Casing_SL;
                private Double mOffset;
                private String mOffset_Direction;
                private clsPin mDowel;
                private eInsertedOn mInsertedOn;

            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //===============================

                public Double Loc_Back
                {
                    get { return mLoc_Back; }
                    set { mLoc_Back = value; }
                }

                public Double Ang_Casing_SL
                {
                    get { return mAng_Casing_SL; }
                    set { mAng_Casing_SL = value; }
                }

                public Double Offset
                {
                    get { return mOffset; }
                    set { mOffset = value; }
                }

                public String Offset_Direction
                {
                    get { return mOffset_Direction; }
                    set { mOffset_Direction = value; }
                }
       
                public clsPin Dowel
                {
                    get { return mDowel; }
                    set { mDowel = value; }
                }

                public eInsertedOn InsertedOn
                {
                    get { return mInsertedOn; }
                    set { mInsertedOn = value; }
                }

            #endregion


            #region "CONSTRUCTOR:"

                public clsARP(clsBearing_Radial_FP Current_Bearing_Radial_FP_In)
                //=====================================================================
                {
                    mCurrent_Bearing_Radial_FP = Current_Bearing_Radial_FP_In;

                    mDowel = new clsPin(mCurrent_Bearing_Radial_FP.Unit.System);         
                    mDowel.Spec_Type = "P";
                    mDowel.Spec_Mat = "STEEL";
                    mOffset_Direction = "CCW";
                    mInsertedOn = eInsertedOn.BearingOD;
                }

            #endregion


            #region "CLASS METHODS":
 
                public double Ang_Casing_SL_Horz()            
                //================================
                {
                    double pPivot_AngStart = mCurrent_Bearing_Radial_FP.Pad.Pivot.AngStart_Casing_SL;     //....w.r.t Casing S/L
                    double pPadAng = mCurrent_Bearing_Radial_FP.Pad.Angle;
                    int pPadCount = mCurrent_Bearing_Radial_FP.Pad.Count;
                    double pPivotOffset = mCurrent_Bearing_Radial_FP.Pad.Pivot.Offset;

                    //double pPad_AngStart_Casing_SL = pPivot_AngStart - 0.5 * pPadAng;     // PB 29OCT18, BG Replace  0.5 *pPadAng ==> (pPivotOffset/ 100) * pPadAng ;
                    double pPad_AngStart_Casing_SL = pPivot_AngStart - (pPivotOffset/100) * pPadAng;
                    double pPad_AngBet = 360 / pPadCount - pPadAng;

                    //....Calculate Pad Start Angle w.r.t. Horizontal 
                    double pPad_AngStart_Horz = 0;

                    if (mCurrent_Bearing_Radial_FP.OilInlet.Orifice.StartPos == clsOilInlet.eOrificeStartPos.Below)
                    {
                        pPad_AngStart_Horz = 0;
                    }
                    else if (mCurrent_Bearing_Radial_FP.OilInlet.Orifice.StartPos == clsOilInlet.eOrificeStartPos.On)
                    {
                        pPad_AngStart_Horz = 0.5 * pPad_AngBet;
                    }
                    else if (mCurrent_Bearing_Radial_FP.OilInlet.Orifice.StartPos == clsOilInlet.eOrificeStartPos.Above)
                    {
                        pPad_AngStart_Horz = pPad_AngBet;
                    }

                    //....Angle - Casing S/L w.r.t. Horizontal.
                    double pAng_Casing_SL_Horz = pPad_AngStart_Horz - pPad_AngStart_Casing_SL;
                    return pAng_Casing_SL_Horz;
                }


                public double Ang_Horz()          
                //======================
                {
                    double pAngle_Horz = 0;
                    pAngle_Horz = Ang_Casing_SL_Horz() + mAng_Casing_SL;
                    return pAngle_Horz;
                }

                //private Double Depth_DefVal ()              // PB 12OCT18a. 
                ////============================
                //{
                //    //....Ref. Radial_Rev11_27OCT11: Col. GI
                //    return mDowel.D();
                //}

                public Double Stickout(Double L_In)       
                //=================================
                { 
                    Double pStickout = 0.0F;
                    pStickout = L_In - mDowel.Hole.Depth_Low;

                    return pStickout;
                }

            #endregion
        }
    }
}
