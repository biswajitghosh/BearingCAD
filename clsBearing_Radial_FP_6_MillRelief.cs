//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_6_MillRelief      '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                                '
//                                                                              '
//===============================================================================
// PB 12OCT18a

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
        public class clsMillRelief
        {
            #region "NAMED CONSTANTS:"
            //========================

                //DESIGN PARAMETERS:
                //------------------
                //....EDM Relief (used in Main Class & clsOilInlet).
                private const Double mc_DESIGN_EDM_RELIEF = 0.010D;        

            #endregion


            #region "MEMBER VARIABLES:"
            //=========================

                private clsBearing_Radial_FP mCurrent_Bearing_Radial_FP;

                private bool mExists;
                public string mD_Desig;
                //....D                      //....Method.   
                //....D_PadRelief            //....Method.

                private Double[] mAxialSealGap = new Double[2];           

            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //===============================

                #region "NAMED CONSTANTS:"
              
                    public Double DESIGN_EDM_RELIEF
                    //=============================    
                    {
                        get { return mc_DESIGN_EDM_RELIEF; }
                    }

                #endregion


                public bool Exists
                {
                    get { return mExists; }
                    set { mExists = value; }
                }

                public string D_Desig
                {
                    get { return mD_Desig; }
                    set{ mD_Desig = value;}
                }

                #region "Axial Seal Gap:"

                    public Double[] AxialSealGap              
                    {
                        get
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                if (mAxialSealGap[i] < modMain.gcEPS)
                                {
                                    mAxialSealGap[i] = mc_DESIGN_EDM_RELIEF;
                                }
                            }
                            return mAxialSealGap;
                        }
                    }

                #endregion

            #endregion


            #region "CONSTRUCTOR:"

                public clsMillRelief(clsBearing_Radial_FP Current_Bearing_Radial_FP_In)
                //======================================================================
                {
                    mCurrent_Bearing_Radial_FP = Current_Bearing_Radial_FP_In;
                }

            #endregion


            #region "CLASS METHODS":

                public Double D()
                //================
                {
                    if (mD_Desig != null && mD_Desig != "")     
                    {
                        return modMain.DVal(mD_Desig);
                    }
                    else
                        return 0;
                }

                public Double D_PadRelief()             
                //==========================
                {   
                    Double pD_PadReleif = 0.0;

                    Double pDBore = mCurrent_Bearing_Radial_FP.Bore();
                    Double pPad_TPivot = mCurrent_Bearing_Radial_FP.Pad.T.Pivot;
                    Double pWeb_H = mCurrent_Bearing_Radial_FP.FlexurePivot.Web.H;

                    if (!mExists)
                    {
                        pD_PadReleif = pDBore + 2 * (pPad_TPivot + pWeb_H + 0.020);
                    }
                    else if(mExists == true)
                    {
                        pD_PadReleif = pDBore + 2 * (pPad_TPivot + mAxialSealGap[0]) + 0.020;
                    }

                    return pD_PadReleif;
                }

            #endregion
        }

    }
}
