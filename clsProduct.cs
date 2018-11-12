//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsProduct                             '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                                '
//                                                                              '
//===============================================================================
//  JOURNAL BEARING CLASS:
//  ---------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;  

namespace BearingCAD22
{
    [Serializable]
    public class clsProduct : ICloneable 
    {
        #region "ENUMERATION TYPES:"
        //==========================
            public enum eType { Journal, Thrust };
            public enum eEndPlatePos { Inside = 0, Overhang = 1 };      //....Inside = 0 (includes Flush), Overhung = 1
              
        #endregion
        
        #region "MEMBER VARIABLES:"
        //=========================
            private clsUnit mUnit; 
            private eType mType;           
            
        //COMPONENTS:
            //-----------
            private clsBearing mBearing;                                    //....Main Component.

            //....Front: 0, Back:1.
            private clsEndPlate[] mEndPlate;                            //....End Components.
           

            private Double mL_Available;                //....Constraint - Total available envelope length.
           
        #endregion


        #region "PROPERTIES:"
        //===================

            public clsUnit Unit
            {
                get { return mUnit; }
                set 
                {  
                    mUnit = value;
                    mBearing.Unit.System = mUnit.System;
                    mEndPlate[0].Unit.System = mUnit.System;
                    mEndPlate[1].Unit.System = mUnit.System;
                }
            }

            public eType Type
            {
                get { return mType; }
                set { mType = value; }
            }

            public clsBearing Bearing
            {
                get { return mBearing; }
                set { mBearing = value; }
            }

            public clsEndPlate[] EndPlate
            {
                get{return mEndPlate;}
                set { mEndPlate = value; }
            }

            public Double L_Available
            {
                get { return mL_Available; }
                set { mL_Available = value; }
            }
        #endregion
        
        
         public clsProduct(clsUnit.eSystem UnitSystem_In, clsDB DB_In)
        //============================================================
        {
            //PB 23JAN13. At the time of instantiation of this object, the Bearing Type & Bearing Design, EndConfig [0,1] Types are
            //....known. Hence, instantiate mBearing & mEndConfig [0,1] using the above info. To be done in the future. 

            //  Initialize.
            //  -----------
            //
            //....Unit System.
            mUnit        = new clsUnit();       //....Default unit = English (automatically).
            mUnit.System = UnitSystem_In;

            //....Bearing.
            mBearing = new clsBearing_Radial_FP(mUnit.System, clsBearing_Radial.eDesign.Flexure_Pivot, this);

            //....End Plates:
            clsEndPlate.eType[] pEndPlate = new clsEndPlate.eType[2];

            for (int i = 0; i < pEndPlate.Length; i++)
                pEndPlate[i] = clsEndPlate.eType.Seal;        //....Default cofigs = [Seal, Seal]


            mEndPlate = new clsEndPlate[2];

            for (int i = 0; i < 2; i++)
            {
                mEndPlate[i] = new clsSeal(mUnit.System, this);
            }

        }
        

        #region "CLASS METHODS:"
        //*********************

         public Double L_Tot()
         //-------------------
         {
             //....Relevant Radial Bearing Parameters:
             //....Keep the following commented lines for the sake of history.
             //double pEDM_Relief = ((clsBearing_Radial_FP)mBearing).DESIGN_EDM_RELIEF;
             //double pEDM_Relief_Tot = ((clsBearing_Radial_FP)mBearing).EDM_Relief[0] + 
             //                         ((clsBearing_Radial_FP)mBearing).EDM_Relief[1];     

             double pAxialSealGap_Tot = ((clsBearing_Radial_FP)mBearing).MillRelief.AxialSealGap[0] +
                                      ((clsBearing_Radial_FP)mBearing).MillRelief.AxialSealGap[1];

             double pBearing_Pad_L = ((clsBearing_Radial_FP)mBearing).Pad.L;

             double pL_Tot = 0;


             //....Store End Configs Depth & Lengths in local variables:
             //
             double[] pDepth_EndConfig = new double[2];
             double[] pL_EndConfig = new double[2];

             for (int i = 0; i < 2; i++)
             {
                 pDepth_EndConfig[i] = ((clsBearing_Radial_FP)mBearing).Depth_EndPlate[i];
                 pL_EndConfig[i] = mEndPlate[i].L;
             }


             //....Determine End Configs' State:   Overhang, Flush/Inside.
             //
             int[] pEndConfig_Pos = new int[2];
             for (int j = 0; j < 2; j++)
             {
                 if (pL_EndConfig[j] > pDepth_EndConfig[j])
                     pEndConfig_Pos[j] = (int)eEndPlatePos.Overhang;
                 else
                     pEndConfig_Pos[j] = (int)eEndPlatePos.Inside;    //....Also include Flush. // PB 02OCT18. Then don't have flush in enum. Outside = 1, Inside = 0 (includes flush).
             }


             //Calculate Total Length of the Product Assembly.
             //-----------------------------------------------
             //
             //....Case 1: Both End Configs are overhung. 
             //
             if (pEndConfig_Pos[0] == (int)eEndPlatePos.Overhang &&
                 pEndConfig_Pos[1] == (int)eEndPlatePos.Overhang)
             {
                 pL_Tot = pL_EndConfig[0] + pBearing_Pad_L + pAxialSealGap_Tot + pL_EndConfig[1];
             }

         //....Case 2: Both End Configs are Flush / Inside. 
             //
             else if (pEndConfig_Pos[0] == (int)eEndPlatePos.Inside &&
                 pEndConfig_Pos[1] == (int)eEndPlatePos.Inside)
             {
                 pL_Tot = pDepth_EndConfig[0] + pBearing_Pad_L + pAxialSealGap_Tot + pDepth_EndConfig[1];
             }

         //....Case 3: Front End Config = Inside & Back = Overhung.
             //
             else if (pEndConfig_Pos[0] == (int)eEndPlatePos.Inside &&
                 pEndConfig_Pos[1] == (int)eEndPlatePos.Overhang)
             {
                 pL_Tot = pDepth_EndConfig[0] + pBearing_Pad_L + pAxialSealGap_Tot + pL_EndConfig[1];
             }

         //....Case 4: Front End Config = Overhung & Back = Flush/Inside.
             else if (pEndConfig_Pos[0] == (int)eEndPlatePos.Overhang &&
                 pEndConfig_Pos[1] == (int)eEndPlatePos.Inside)
             {
                 pL_Tot = pL_EndConfig[0] + pBearing_Pad_L + pAxialSealGap_Tot + pDepth_EndConfig[1];
             }

             return pL_Tot;
         }

         //#endregion

         public Double Calc_L_EndPlate()        // PB 02OCT18. Will discuss where it is used. Not sure why L_available is used.
         //-----------------------------      
         {
             //....Default Case: Both End Configs' are of equal length.
             double pAxialSealGap_Tot = ((clsBearing_Radial_FP)mBearing).MillRelief.AxialSealGap[0] +
                                      ((clsBearing_Radial_FP)mBearing).MillRelief.AxialSealGap[1];
             double pBearing_Pad_L = ((clsBearing_Radial_FP)mBearing).Pad.L;

             Double pL = 0.0;
             pL = 0.5 * (mL_Available - (pBearing_Pad_L + pAxialSealGap_Tot));

             return pL;
         }

            #region "MOUNTING SCREWS:"

                //private void Set_Mount_ScrewSpecs_EndConfigs ()
                ////=============================================
                //{
                //    string pD_DesigF, pD_DesigB;

                //    //Boolean pGoThru                       = ((clsBearing_Radial_FP)mBearing).Mount.Holes_GoThru;
                //    clsBearing_Radial_FP.eBolting pBolting = ((clsBearing_Radial_FP)mBearing).Mount.Bolting;

                //    //if (pGoThru == true && pBolting == clsBearing_Radial_FP.eFaceID.Front)
                //    //{
                //    //    //pD_DesigF = ((clsBearing_Radial_FP)mBearing).Mount.Fixture_Screw_Spec[0].D_Desig;
                //    //    pD_DesigF = ((clsBearing_Radial_FP)mBearing).Mount.Screw[0].Screw_Spec.D_Desig;       //BG 24AUG12

                //    //    mEndConfig[0].MountHoles.Screw_Spec.D_Desig = pD_DesigF;
                //    //    mEndConfig[1].MountHoles.Screw_Spec.D_Desig = pD_DesigF;
                //    //}

                //    //else if (pGoThru == true && pBolting == clsBearing_Radial_FP.eFaceID.Back)
                //    //{
                //    //    //pD_DesigB = ((clsBearing_Radial_FP)mBearing).Mount.Fixture_Screw_Spec[1].D_Desig;
                //    //    pD_DesigB = ((clsBearing_Radial_FP)mBearing).Mount.Screw[1].Screw_Spec.D_Desig;      //BG 24AUG12

                //    //    mEndConfig[0].MountHoles.Screw_Spec.D_Desig = pD_DesigB;
                //    //    mEndConfig[1].MountHoles.Screw_Spec.D_Desig = pD_DesigB;
                //    //}

                //    //else if (pGoThru == false)
                //    //{
                //        //....Bolting = 'Both'.
                //        //pD_DesigF = ((clsBearing_Radial_FP)mBearing).Mount.Fixture_Screw_Spec[0].D_Desig;
                //        //pD_DesigB = ((clsBearing_Radial_FP)mBearing).Mount.Fixture_Screw_Spec[1].D_Desig;

                        
                //        pD_DesigF = ((clsBearing_Radial_FP)mBearing).Mount.Screw[0].Spec.D_Desig;
                //        pD_DesigB = ((clsBearing_Radial_FP)mBearing).Mount.Screw[1].Spec.D_Desig;

                //        //mEndPlate[0].MountHoles.Screw.Spec_D_Desig = pD_DesigF;
                //        //mEndPlate[1].MountHoles.Screw.Spec_D_Desig = pD_DesigB;
                //    //}
                //}

            #endregion

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
    }
}
