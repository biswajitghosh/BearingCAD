
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing                             '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                              '
//                                                                              '
//===============================================================================
// PB 07OCT18. Just cleaned up.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BearingCAD22
{
     [Serializable]
    public abstract class clsBearing
    {  

        #region "ENUMERATION TYPES:"
        //==========================
               
        #endregion


        #region "MEMBER VARIABLES:"
        //=========================           
            private clsUnit mUnit = new clsUnit();          
            protected clsDB mDB;
        #endregion


        #region "CLASS PROPERTY ROUTINES:"
        //================================

            public clsUnit Unit
            {
                get { return mUnit; }
                set 
                {
                    mUnit = value;                   
                }
            }
  

        #endregion

        
        //....Class Constructor
        public clsBearing(clsUnit.eSystem UnitSystem_In, clsBearing_Radial_FP.eDesign Design_In)
        //======================================================================================
        {
            mUnit.System = UnitSystem_In;            
            //mDB = DB_In;
        }
       
    }
}
