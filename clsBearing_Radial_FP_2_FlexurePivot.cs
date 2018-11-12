//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial_FP_2_FlexurePivot    '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                                '
//                                                                              '
//===============================================================================
// PB 12OCT18

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
        public class clsFlexurePivot
        //==========================
        {
            #region "USER-DEFINED STRUCTURES:"
            //================================
                [Serializable]
                //.....Web
                public struct sWeb
                {
                    public Double T;
                    public Double H;
                    public Double RFillet;
                }
            #endregion


            #region "MEMBER VARIABLES:"
            //=========================
                private sWeb mWeb;
                private Double mGapEDM;
           #endregion


            #region "CLASS PROPERTY ROUTINES:"
            //================================  

                #region "Web:"
                //------------
                    public sWeb Web
                    {
                        get
                        {
                            if (mWeb.RFillet < modMain.gcEPS)
                            {
                                mWeb.RFillet = Calc_Web_RFillet();
                            }
                            return mWeb; 
                        }
                    }


                    public Double Web_T
                    {
                        set { mWeb.T = value; }
                    }

                    public Double Web_H
                    {
                        set { mWeb.H = value; }
                    }

                    public Double Web_RFillet
                    {
                        set { mWeb.RFillet = value; }
                    }

                #endregion


                public Double GapEDM
                {
                    get { return mGapEDM; }
                    set { mGapEDM = value; }
                }

            #endregion


            #region "CONSTRUCTOR:"

                public clsFlexurePivot()
                //======================
                {

                }

            #endregion

            #region "CLASS METHODS":

                public Double  Calc_Web_RFillet()      
                //=================================
                {                   
                     Double pWeb_RFillet = modMain.MRound(mWeb.T, 0.005);              
                     return pWeb_RFillet;
                }
               
            #endregion
        }
     
    }
}
