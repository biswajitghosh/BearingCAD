﻿//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_4_Flange          '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                               '
//                                                                              '
//===============================================================================
//
//Routines
//--------                       
//===============================================================================

//PB 16APR12. Just cleaned up.

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
        public class clsFlange
        //======================
        {
            #region "MEMBER VARIABLES:"
            //=========================
                private clsBearing_Radial_FP mCurrent_Bearing_Radial_FP;

                private bool mExists;
                private Double mD;
                private Double mWid;
                private Double mDimStart_Back;

            #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //================================  

                public bool Exists
                {
                    get { return mExists; }
                    set { mExists = value; }
                }


                public Double D
                {
                    get
                    {
                        if (mExists == false)
                        {   //Design Table cols. requires a non-null value. 
                            //....Ref. Radial_Rev11_27OCT11: Col. DD. 
                            //mD = mCurrent_Bearing_Radial_FP.OD() + mc_DEPTH_FIXTURE_HOLE;
                            mD = mCurrent_Bearing_Radial_FP.OD() ;
                        }
                        return mD;
                    }

                    set { mD = value; }
                }


                public Double Wid
                {
                    get
                    {
                        if (mExists == false || mWid < modMain.gcEPS)
                        {
                            //Design Table cols. requires a non-null value. 
                            //....Ref. Radial_Rev11_27OCT11: Col. DF
                            mWid = 0.063;
                        }
                        return mWid;
                    }

                    set { mWid = value; }
                }


                public Double DimStart_Back
                {
                    get
                    {
                        if (mExists == false || mDimStart_Back < modMain.gcEPS)
                        {
                            //Design Table cols. requires a non-null value. 
                            //....Ref. Radial_Rev11_27OCT11: Col. DH
                            mDimStart_Back = 0.063;
                        }
                        return mDimStart_Back;
                    }

                    set { mDimStart_Back = value; }
                }

            #endregion


            #region "CONSTRUCTOR:"

                public clsFlange(clsBearing_Radial_FP Current_Bearing_Radial_FP_In)
                //=================================================================
                {
                    mCurrent_Bearing_Radial_FP = Current_Bearing_Radial_FP_In;
                }

            #endregion
        }

    }
}
