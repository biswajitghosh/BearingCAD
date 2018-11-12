//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsEndConfig                           '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                                '
//                                                                              '
//===============================================================================

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace BearingCAD22
{
     [Serializable]
    public abstract class clsEndPlate
    {
        #region "Main Class:"

            #region "ENUMERATION TYPES:"
            //==========================
                public enum eType { Seal, TL_TB };
            #endregion

            #region "MEMBER VARIABLES:"
            //========================
                protected clsProduct mCurrentProduct;           

                private clsUnit mUnit = new clsUnit();              
                private eType mType;
                private clsMaterial mMat = new clsMaterial();       //...Materials - Base & Lining.
                private Double mLiningT;                            

                //....Envelope Geometry:
                private Double mOD;
                private Double[] mDBore_Range = new Double[2];
                private Double mL;
            #endregion


            #region "PROPERTY ROUTINES:"
            //=========================

                public clsUnit Unit
                {
                    get { return mUnit; }
                }

                public eType Type
                {
                    get { return mType; }
                    set { mType = value; }            
                }
               
                // Base & Lining Materials:
                //------------------------
                public clsMaterial Mat
                {
                    get { return mMat; }
                    set { mMat = value; }
                }

                public Double Mat_LiningT
                {
                    get { return mLiningT; }
                    set { mLiningT = value; }
                }

                #region "Envelope Geometry:"

                    //....OD  
                    public Double OD
                    {
                        get { return mOD; }
                        set { mOD = value; }
                    }


                    //....Bore Dia 
                    public Double[] DBore_Range
                    {
                        get { return mDBore_Range; }
                        set { mDBore_Range = value;}
                    }

                    //....Length                   
                    public Double L
                    {
                        get
                        {   if (mL < modMain.gcEPS)
                            {   
                                mL = mCurrentProduct.Calc_L_EndPlate();
                            }
                            return mL; 
                        }
                        set { mL = value; }
                    }

                #endregion
         
            #endregion


            #region "Constructor:"

                    public clsEndPlate(clsUnit.eSystem UnitSystem_In, clsProduct CurrentProduct_In)
                    //=============================================================================
                    {
                        mUnit.System = UnitSystem_In;
                       
                        mMat.LiningExists = false;
                        mMat.Lining = "None";          

                        mCurrentProduct = CurrentProduct_In;
                    }

            #endregion


            #region "CLASS METHODS:"
            //---------------------
       

                public Double OD_ULimit(clsProduct Product_In)      
                //============================================
                {
                    mCurrentProduct = Product_In;
                    double pD_CB_Max = ((clsBearing_Radial_FP)mCurrentProduct.Bearing).DCB_EndPlate_Max();
                    double pDESIGN_DCLEAR = ((clsBearing_Radial_FP)mCurrentProduct.Bearing).DESIGN_DCLEAR;
                    return (pD_CB_Max - pDESIGN_DCLEAR);
                }
                
                public Double OD_LLimit(clsProduct Product_In, int Indx_In)        
                //==================================
                {
                    mCurrentProduct = Product_In;
                    double pScrew_Hole_DBC_Min = ((clsBearing_Radial_FP)mCurrentProduct.Bearing).Mount.DBC_LLimit(mCurrentProduct, Indx_In);  
                    double pHole_CBore_D = ((clsBearing_Radial_FP)mCurrentProduct.Bearing).Mount.Screw[Indx_In].Hole.CBore.D;
                    return (pScrew_Hole_DBC_Min + 2 * modMain.gcSep_Min + pHole_CBore_D);
                }

                public Double DBore()
                //===================
                {
                    return modMain.Nom_Val(mDBore_Range);
                }

                public Double Clearance()
                //=======================              
                {   
                    //....Diametral Clearance.
                    Double pClear;
                    pClear = DBore() - ((clsBearing_Radial_FP)mCurrentProduct.Bearing).DShaft();
                    return pClear;
                }

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

