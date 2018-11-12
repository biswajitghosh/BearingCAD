
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsBearing_Radial                      '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                                '
//                                                                              '
//===============================================================================
// PB 14OCT18. Just cleaned up

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BearingCAD22
{
     [Serializable]
    public abstract class clsBearing_Radial: clsBearing 
    {
        #region "ENUMERATION TYPES:"
        //==========================
           public enum eDesign { Flexure_Pivot, Tilting_Pad, Sleeve };      
        #endregion

        #region "MEMBER VARIABLES:"
        //=========================
           private eDesign mDesign;
        #endregion

        #region "CLASS PROPERTY ROUTINES:"
        //================================
            public eDesign Design
            {
                get { return mDesign; }
                set { mDesign = value; }
            }
        #endregion

        //....Class Constructor
        public clsBearing_Radial(clsUnit.eSystem UnitSystem_In, eDesign Design_In)
            : base(UnitSystem_In, Design_In)
        //========================================================================
        {
            mDesign = Design_In;
        }
    }
}
