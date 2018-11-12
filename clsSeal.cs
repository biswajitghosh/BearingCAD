﻿
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsSeal                                '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                                '
//                                                                              '
//===============================================================================
//
// PB 25OCT18

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;   
using System.Runtime.Serialization;                     
using System.IO;  
                                      
namespace BearingCAD22
{
    [Serializable]
    public class clsSeal : clsEndPlate, ICloneable   
    {
        #region "NAMED CONSTANTS:"
        //=======================
            //private const Double mc_DESIGN_LINING_THICK = 0.030F;  // PB 07OCT18. May not be needed
        #endregion

        #region "ENUMERATION TYPES:"
        //==========================
            //[Serializable]
            //public enum eDesign {Fixed, Floating};  // PB 07OCT18. Have only Fixed for now here.
            [Serializable]
            public enum eDesign { Fixed};
        #endregion

        #region "MEMBER VARIABLES:"
        //=========================
            private eDesign mDesign;        
            private clsBlade mBlade;
            private clsDrainHoles mDrainHoles;
            //private clsWireClipHoles mWireClipHoles;     
            //private Double mTempSensor_D_ExitHole;   
        #endregion


        #region "PROPERTY ROUTINES:"
        //=========================
            //....Design:
            public eDesign Design
            {
                get { return mDesign; }
                set { mDesign = value; }
            }

            public clsBlade Blade
            {
                get { return mBlade; }
                set { mBlade = value; }
            }

            public clsDrainHoles DrainHoles
            {
                get { return mDrainHoles; }
                set { mDrainHoles = value; }
            }
 
            //#region "Wire Clip Holes:"

            //    public clsWireClipHoles WireClipHoles
            //    {
            //        get { return mWireClipHoles; }
            //        set { mWireClipHoles = value; }
            //    }
              
            //#endregion

            //#region "Temp Sensor Exit Holes:"

            //    public Double TempSensor_D_ExitHole
            //    {
            //        get { return mTempSensor_D_ExitHole; }
            //        set { mTempSensor_D_ExitHole = value; }
            //    }

            //#endregion

        #endregion


        #region "CONSTRUCTOR:"

                public clsSeal(clsUnit.eSystem UnitSystem_In, clsProduct CurrentProduct_In)
                              : base(UnitSystem_In, CurrentProduct_In)
                //==========================================================================
                {
                    //....Default Values:      
                    mDesign = eDesign.Fixed;

                    //....Instantiate member object variables:
                    //mCurrentProduct = CurrentProduct_In;
                    mBlade = new clsBlade();
                    mDrainHoles = new clsDrainHoles(this, CurrentProduct_In);
                    //mWireClipHoles = new clsWireClipHoles(this);
                }

        #endregion


        #region "CLASS METHODS:"
        //======================

            //#region "TEMP. SENSOR"

            //    public Double TempSensor_DBC_Hole()
            //    //=================================
            //    {
            //        //....Ref. Seal_Rev9_27OCT11: Col. BG                                               //Check RA Page: 34 
            //        Double pDBC = ((clsBearing_Radial_FP)mCurrentProduct.Bearing).Bore() + (.08 * 2)  + 
            //                      ((clsBearing_Radial_FP)mCurrentProduct.Bearing).TempSensor.D;
            //        return pDBC;
            //    }

            //#endregion

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

        
        #region "NESTED CLASSES:"

            #region "Class Blade":
            //--------------------

                [Serializable]
                public class clsBlade       
                //====================
                {
                    #region "NAMED CONSTANTS:"
                    //=======================
                        private const Double mc_DESIGN_BLADE_THICK = 0.060F;
                    #endregion

                    #region "MEMBER VARIABLES:"
                    //=========================
                        private int mCount;
                        private Double mT;
                        private Double mAngTaper;
                    #endregion

                    #region "CLASS PROPERTY ROUTINES:"
                    //================================  

                        public Double DESIGN_BLADE_THICK
                        {
                            get {return mc_DESIGN_BLADE_THICK; }
                        }

                        public int Count            
                        {
                            get { return mCount; }
                            set
                            {   int pCount = mCount;
                                mCount = value;
                            }
                        }
                        
                        public Double T        
                        {
                            get { return mT; }
                            set { mT = value; }        //....When mCount = 1, this is Land L.       
                        }

                        public Double AngTaper
                        {
                            get { return mAngTaper; }
                            set
                            {   if (mCount == 1)
                                { mAngTaper = value; }
                            }
                        }

                    #endregion

                    #region "CONSTRUCTOR:"

                        public clsBlade()
                        //==================
                        {
                            mCount = 1;   
                            mT     = mc_DESIGN_BLADE_THICK;
                            mAngTaper = 45;    
                        }

                    #endregion
                }

            #endregion 


            #region "Class DrainHoles":
            //-------------------------
            [Serializable]
                public class clsDrainHoles
                {
                    //#region "NAMED CONSTANTS:"        // PB 24OCT18 Commented out
                    ////=======================
                    //    //....Drain Holes: Minimum seperation distance between the end & begin points 
                    //    //........of two consecutive holes. 
                    //    private const Double mc_DESIGN_DRAINHOLE_SEP_DIST = 0.030F;
                    //#endregion

                    #region "USER-DEFINED STRUCTURES:"
                    //================================
                         [Serializable]
                        //....Annulus
                        public struct sAnnulus
                        {
                            public Double Ratio_L_H;
                            public Double D;                 //....Derived/User Input
                        }
                    #endregion

                    #region "MEMBER VARIABLES:"
                    //=========================
                        protected clsProduct mCurrentProduct;       //PB 25OCT18, to be passed thru' clsSeal constructor
                        private clsSeal mCurrent_Seal;

                        private sAnnulus mAnnulus;
                        private String mD_Desig;
                        //....D;                        //....Method.  

                        private int mCount;     //....Usually it is calculated. However, when the drain holes array crosses the Bearing S/L, 
                                                //.......the count is to be programmatically increased by 1 per HK's (KMC) instruction (DEC12).
                        //....V                 //....Method.         
                       
                        private Double mAngBet;
                        private Double mAngStart_Horz;               //....Calculated but can be overridable.               
                        //private Double mAngStart_OtherSide;
                        private Double mAngExit;

                    #endregion


                    #region "CLASS PROPERTY ROUTINES:"
                    //================================ 

                        #region "ANNULUS:"
                        //-----------------
                            public sAnnulus Annulus
                            {
                                get
                                {   if (Math.Abs (mAnnulus.D) < modMain.gcEPS)
                                        mAnnulus.D = Calc_Annulus_D();
                                    return mAnnulus;
                                }
                            }
                  
                            public Double Annulus_Ratio_L_H
                            {
                                set
                                {   Double pPrevVal = mAnnulus.Ratio_L_H;
                                    mAnnulus.Ratio_L_H = value;

                                    if (Math.Abs ( mAnnulus.Ratio_L_H - pPrevVal) > modMain.gcEPS)
                                    {
                                        //....The current Ratio is different from the previous value.
                                        //........Recalculate the D.
                                        mAnnulus.D = Calc_Annulus_D();
                                    }
                                }
                            }

                            public Double Annulus_D
                            {
                                set{ mAnnulus.D = value;}
                            }

                        #endregion


                        public String D_Desig         
                        {
                            get { return mD_Desig; }

                            set
                            {   mD_Desig = value;

                                //....Recalculate: 
                                mCount    = Calc_Count();          
                                mAngStart_Horz = Calc_AngStart_Horz();   
                            }
                        }

                        public int Count
                        {
                            get
                            {   if (mCount < modMain.gcEPS)
                                    mCount = Calc_Count();
                                
                                return mCount; }

                            set {mCount = value;}
                        }

                        #region "ANGLES:"
                        //---------------

                            public Double AngBet
                            {
                                get { return mAngBet; }

                                set 
                                { 
                                    mAngBet = value;
                                    mCount = Calc_Count();              //....Reset, in case it has been increased by 1 earlier. //PB 25OCT18. May need to be suppressed
                                    mAngStart_Horz = Calc_AngStart_Horz();        //....Recalculate.   
                                }
                            }


                            public Double AngStart_Horz
                            {
                                get
                                {   if (mAngStart_Horz < modMain.gcEPS)
                                        mAngStart_Horz = Calc_AngStart_Horz();        //....Used only for the very first case.

                                        return mAngStart_Horz; 
                                }

                                set { mAngStart_Horz = value; }
                            }

                    
                            //public Double AngStart_OtherSide                 //PB 28JAN13. 
                            //{
                            //    get {   if (mAngStart_OtherSide < modMain.gcEPS)
                            //                mAngStart_OtherSide = Calc_AngStart_OtherSide();

                            //              return mAngStart_OtherSide; }

                            //    set { mAngStart_OtherSide = value; }
                            //}


                            public Double AngExit
                            {
                                get { return mAngExit; }
                                set { mAngExit = value; }
                            }

                        #endregion

                    #endregion
                

                    #region "CONSTRUCTOR:"

                        public clsDrainHoles(clsSeal CurrentSeal_In, clsProduct CurrentProduct_In)
                        //========================================================================
                        {
                            mCurrentProduct = CurrentProduct_In; 
                            mCurrent_Seal = CurrentSeal_In;

                            //....Initialize: Default Values.
                            mAnnulus.Ratio_L_H = 2.0F;
                            mAngExit = 45.0F;            
                        }

                    #endregion

                    #region "CLASS METHODS":

                        public Double Calc_Annulus_Ratio_L_H ()
                        //=====================================          
                        {
                            Double pAnnulus_L = mCurrent_Seal.L - (mCurrent_Seal.Blade.Count * mCurrent_Seal.Blade.T);
                            Double pH         = 0.5 * (mCurrent_Seal.mDrainHoles.Annulus.D - mCurrent_Seal.DBore());

                            Double pAnnulus_L_H;
                            pAnnulus_L_H = pAnnulus_L / pH;

                            return pAnnulus_L_H;
                        }

                        public Double Calc_Annulus_D ()
                        //=============================
                        {
                            Double pAnnulus_L;
                            pAnnulus_L = mCurrent_Seal.L - (mCurrent_Seal.Blade.Count * mCurrent_Seal.Blade.T);

                            Double pH = 0.0F;
                            if (mAnnulus.Ratio_L_H != 0.0F)
                                pH = pAnnulus_L / mAnnulus.Ratio_L_H;

                            Double pD;
                            pD = mCurrent_Seal.DBore() + 2 * pH;

                            return pD;
                        }

                        public Double D()
                        //=================
                        {
                            if (mD_Desig != null && mD_Desig != "")
                            {
                                return modMain.DVal(mD_Desig);
                            }
                            else
                                return 0;
                        }

                        public Int32 Calc_Count()
                        //=======================                          
                        {   //....Ref.: Sizing & Qty. of Drain Holes 
                            //........Depends on FlowReqd_gpm & Drain hole D.

                            //....Flow reqd. GPM.                           
                            Double pFlowReqd_gpm = ((clsBearing_Radial_FP) mCurrentProduct.Bearing).PerformData.FlowReqd;
                            
                            Int32 pCount = 0;

                            Double pATot_Reqd;
                            pATot_Reqd = (231 * pFlowReqd_gpm * 0.5) / (60 * 12 * 2);

                            Double pA_Hole;
                            pA_Hole = 0.25 * Math.PI * Math.Pow(D(), 2);

                            if (pA_Hole != 0.0F)
                                pCount = (Int16)Math.Ceiling(pATot_Reqd / pA_Hole);

                            return pCount;
                        }

                        public Double V()
                        //=================                                 
                        {                            
                            Double pFlowReqd_gpm = ((clsBearing_Radial_FP) mCurrentProduct.Bearing).PerformData.FlowReqd;

                            Double pA_Hole;
                            pA_Hole = 0.25 * Math.PI * Math.Pow(D(), 2);

                            Double pATot;
                            pATot = mCount * pA_Hole;     //....Use always the calculated value of the # of holes.                    
                                                                //........Don't use the augmented count if the drain holes array crosses
                                                                //........Bearing S/L, as the extra hole will be deleted by the user later.
                            Double pV = 0.0F;
                            if (pATot != 0.0F)
                                pV = (0.5F * pFlowReqd_gpm * 231) / (60 * 12 * pATot);

                            return pV;
                        }


                        public Double AngBet_LLim()
                        //==========================               
                        {
                            //....This value depends on the D & Annulus_D.
                            //
                            Double pAng_Bet_LLim = 0.0F;
                         
                            if (D() != 0.0F)
                            {
                                Double pAnnulusR = 0.5 * mAnnulus.D;
                                Double pS = D() + modMain.gcSep_Min;             //....Arc length between the centers of two consecutive holes.

                                Double pAngBet_Rad = 0.0F;

                                if (pAnnulusR != 0.0F)
                                    pAngBet_Rad = (pS / pAnnulusR);                         //....Rad.

                                pAng_Bet_LLim = (pAngBet_Rad * (180.0F / Math.PI));         //....Deg.. 
                            }

                            return pAng_Bet_LLim;
                        }


                        public Double Calc_AngStart_Horz()        
                        //================================                                  
                        {
                             Double pAngStart = 0;
                            //....This calculation is based on the assumption of the symmetry of the drain holes array about
                            //........the Casing S/L vertical. 
                            //
                            //........This depends on the AngBet, D_Desig (which in turns triggers Calc_Count) & Anti-Rotation Pin Ang. 


                            ////Store relevant parameters from Bearing_Radial_FP object in local variables.
                            ////---------------------------------------------------------------------------
                            ////
                            ////....Anti Rotation Pin Location w.r.t. Bearing Datums:
                            //clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL pAntiRotPinLoc_BS =
                            //                        ((clsBearing_Radial_FP)mCurrent_Seal.mCurrentProduct.Bearing).AntiRotPin.Loc_Bearing_SL;
                            //clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert pAntiRotPinLoc_BV =
                            //                        ((clsBearing_Radial_FP)mCurrent_Seal.mCurrentProduct.Bearing).AntiRotPin.Loc_Bearing_Vert;

                            ////....Anti Rotation Pin Angle w.r.t Bearing S.L. 
                            //Double pAntiRot_Pin_Loc_Ang = ((clsBearing_Radial_FP)mCurrent_Seal.mCurrentProduct.Bearing).AntiRotPin.Loc_Angle;

                            //Determine Angle Start w.r.t Casing S/L:
                            //-----------------------------------------
                            //
                            //...w.r.t Casing Vertical.
                            //........Valid for either even or odd value of the "Count".
                            Double pAngStart_Casing_Vert = (0.5 * (mCount - 1)) * mAngBet;        

                            //...w.r.t Casing SplitLine.
                            Double pAngStart_Casing_SL = 90 - pAngStart_Casing_Vert;

                            //Convert Angle Start w.r.t Horizontal:
                            //-------------------------------------
                            // PB 24OCT18. BG, Bring this method from clsARP ==> Ang_Casing_SL_Horz () + pAngStart_Casing_SL
                            pAngStart = ((clsBearing_Radial_FP)mCurrentProduct.Bearing).ARP.Ang_Casing_SL_Horz() + pAngStart_Casing_SL;
                           
                            return pAngStart;
                        }


                        //public Double Calc_AngStart_OtherSide()
                        ////=====================================  
                        //{
                        //    //...w.r.t Bearing S/L.
                        //    Double pAng;
                        //    pAng = 180 - (mAngStart + ((mCount - 1) * mAngBet));

                        //    return pAng;
                        //}


                        //public Double AngStart_OtherSide ()       // PB 24OCT18
                        ////=================================  
                        //{
                        //    //...w.r.t Bearing S/L.
                        //    Double pAng;
                        //    pAng = 180 - (mAngStart_Horz + ((mCount - 1) * mAngBet));

                        //    return pAng;
                        //}
                        // PB 24OCT18. BG, suppress all ULim calculations. On the form, we will not display ULim now. It is not required now, may be later in the future. Just keep blank. Just check if
                        //the AngBet is >= than LLim. No need to check <= Ulim


                        public bool Sym_CasingSL_Vert ()
                        //=============================
                        {
                            if (Math.Abs (mAngStart_Horz - Calc_AngStart_Horz ()) <= modMain.gcEPS)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }


                        public Double AngBet_ULim_Sym()
                        //============================                     
                        {
                            Double pAng_Bet_ULim = 0.0F;

                            ////....This routine is valid when the drain holes are symmetric about the Casing S/L vertical 
                            ////........when Ang_Start is a dependent parameter.

                            ////Store relevant parameters from Bearing_Radial_FP object in local variables.
                            ////---------------------------------------------------------------------------
                            ////
                            ////....Anti Rotation Pin Location w.r.t. Bearing Datums:
                            //clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL pPinLoc_BS =
                            //                        ((clsBearing_Radial_FP)mCurrent_Seal.mCurrentProduct.Bearing).AntiRotPin.Loc_Bearing_SL;

                            //clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert pPinLoc_BV =
                            //                        ((clsBearing_Radial_FP)mCurrent_Seal.mCurrentProduct.Bearing).AntiRotPin.Loc_Bearing_Vert;

                            ////....Anti Rotation Pin Angle w.r.t Bearing S.L. 
                            //Double pPinLoc_Ang = ((clsBearing_Radial_FP)mCurrent_Seal.mCurrentProduct.Bearing).AntiRotPin.Loc_Angle;


                            ////....Drain hole Angle upper limit in degree.
                            //Double pAng_Bet_ULim = 0.0F;

                            //int pCount = Calc_Count();

                            //if (pCount > 1)
                            //{
                            //    if ((pPinLoc_BS == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Top &&
                            //            pPinLoc_BV == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.L)
                            //            ||
                            //        (pPinLoc_BS == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Bottom &&
                            //            pPinLoc_BV == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.R))
                            //        //------------------------------------------------------------------------------
                            //        pAng_Bet_ULim = (2 * (90 - pPinLoc_Ang) / (pCount - 1));


                            //    else if ((pPinLoc_BS == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Bottom &&
                            //            pPinLoc_BV == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.L)
                            //            ||
                            //        (pPinLoc_BS == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_SL.Top &&
                            //            pPinLoc_BV == clsBearing_Radial_FP.clsAntiRotPin.eLoc_Bearing_Vert.R))
                            //        //------------------------------------------------------------------------------
                            //        pAng_Bet_ULim = (2 * (90 + pPinLoc_Ang)) / (pCount - 1);
                            //}

                            return pAng_Bet_ULim;
                        }

                       
                        public Double AngBet_ULim_NonSym()
                        //================================                     
                        {
                            Double pAng_Bet_ULim = 0.0F; 
                            //....This routine is used when Ang_Start is a user input and the drain holes will no longer be 
                            //.........symmetric about the Casing S/L vertical 

                            //....This value depends on AngStart_Front & Count. This calculation loses its relevance
                            //........when the drain holes array crosses the Bearing S/L and hence, it is not used then.

                            ////....Drain hole Angle upper limit in degree.
                            //Double pAng_Bet_ULim = 0.0F;                            
                            //pAng_Bet_ULim        = (180 - mAngStart_Horz) / (Calc_Count () - 1);

                            return pAng_Bet_ULim;
                       }

                        public void UpdateCurrentSeal(clsProduct CurrentProduct_In)
                        //=========================================================
                        {
                            mCurrentProduct = CurrentProduct_In;                            
                        }

                    #endregion
                }

            #endregion
        
            //#region "Class WireClipHoles":

            //    [Serializable]
            //    public class clsWireClipHoles
            //    {
            //        #region "NAMED CONSTANTS:"
            //        //=======================
            //        private const int mc_COUNT_WIRE_CLIP_HOLES_MAX = 3;// 6;    

                   
            //        #endregion

            //        #region "MEMBER VARIABLES:"
            //        //=========================
            //            private bool mExists;
            //            private int mCount;
            //            private Double mDBC;
            //            private clsScrew mScrew_Spec;           

            //            //private string mThreadDia_Desig;      //PB 03FEB12. we may add clsScrew here as in Mounting Holes. 
            //            //private Double mThread_Pitch;
            //            private Double mThread_Depth;
            //            private Double mAngStart;
            //            private Double[] mAngOther;

            //            private clsUnit mUnit = new clsUnit();   //BG 03JUL13

            //        #endregion
                    

            //        #region "CLASS PROPERTY ROUTINES:"
            //        //================================ 
            //            public int COUNT_WIRE_CLIP_HOLES_MAX
            //            {
            //                get { return mc_COUNT_WIRE_CLIP_HOLES_MAX; }
            //            } 

            //            public bool Exists
            //            {
            //                get { return mExists; }
            //                set { mExists = value; }
            //            }

            //            public int Count
            //            {
            //                get { return mCount; }
            //                set { mCount = value; }
            //            }

            //            public Double DBC
            //            {
            //                get { return mDBC; }
            //                set { mDBC = value; }
            //            }

            //            public clsScrew Screw_Spec                      //PB 22APR12
            //            {
            //                get { return mScrew_Spec; }
            //                set { mScrew_Spec = value; }
            //            }

            //            //public string ThreadDia_Desig                 //PB 22APR12
            //            //{
            //            //    get { return mThreadDia_Desig; }
            //            //    set { mThreadDia_Desig = value; }
            //            //}

            //            //public Double Thread_Pitch
            //            //{
            //            //    get { return mThread_Pitch; }
            //            //    set { mThread_Pitch = value; }
            //            //}

            //            public Double ThreadDepth
            //            {
            //                get { return mThread_Depth; }
            //                set { mThread_Depth = value; }
            //            }

            //            public Double AngStart
            //            {
            //                get { return mAngStart; }
            //                set { mAngStart = value; }
            //            }

            //            public Double[] AngOther
            //            {
            //                get { return mAngOther; }
            //                set { mAngOther = value; }
            //            }

            //            public clsUnit Unit
            //            {
            //                get { return mUnit; }
            //                set { mUnit = value; }
            //            }

            //        #endregion


            //        #region "CONSTRUCTOR:"

            //            public clsWireClipHoles(clsSeal CurrentSeal_In)
            //            //=============================================
            //            {
            //                mScrew_Spec = new clsScrew(CurrentSeal_In.Unit.System);             
            //                mAngOther = new Double[mc_COUNT_WIRE_CLIP_HOLES_MAX - 1];

            //                //....Initialize. Default Values.
            //                mExists = false; // true;
            //                mCount = 1;
            //                //mScrew_Spec.D_Desig = "M3";             //PB 23APR12
            //                //mThreadDia_Desig = "M3";    

            //                mUnit.System = CurrentSeal_In.Unit.System;          //BG 03JUL13
            //            }

            //        #endregion
            //    }

            //#endregion

        #endregion

    }
}
