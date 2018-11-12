
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsProject                             '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                                '
//                                                                              '
//===============================================================================
// PB 08OCT18. Just cleaned

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data.Sql;
using iTextSharp.text.pdf;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace BearingCAD22
{
    [Serializable]
    public class clsProject  
    {
        #region "MEMBER VARIABLE DECLARATIONS:"
        //=====================================
            private clsProduct mProduct;           
            
            private clsSalesOrder mSalesOrder = new clsSalesOrder();        // PB 08OCT18. May be moved to structure later
            private clsPNR mPNR = new clsPNR();                             // -do-
    
            private string mStatus;
                
        #endregion


        #region "CLASS PROPERTY ROUTINES:"
            //============================

            //.... Product
            public clsProduct Product
            {
                get { return mProduct; }
                set { mProduct = value; }
            }
        
            public string Status          
            {
                get { return mStatus; }
                set { mStatus = value; }
            }

            #region "SalesOrder:"
            //===================

                public clsSalesOrder SalesOrder
                {
                    get { return mSalesOrder; }
                    set { mSalesOrder = value; }
                }    

            #endregion

            #region "PNR:"
            //============

                public clsPNR PNR
                {
                    get { return mPNR; }
                    set { mPNR = value; }
                }

             #endregion

        #endregion


        #region "CLASS CONSTRUCTOR:"

            public clsProject(clsUnit.eSystem UnitSystem_In)
            //==============================================
            {
                mPNR.Unit.System = UnitSystem_In;
                mProduct = new clsProduct(mPNR.Unit.System, modMain.gDB);       
               
            }

        #endregion


        #region "CLASS METHODS:"
        //=====================           

            #region "NESTED CLASS:"
        
                [Serializable]
                public class clsSalesOrder
                {

                    #region "ENUMERATION TYPES:"
                    //==========================
                        public enum eType { Order, Proposal };                

                    #endregion

                    private string mNo;
                    private string mLineNo;
                    private string mRelatedNo;
                    private eType mType;
                    private clsCustomer mCustomer = new clsCustomer();

                    //.... Name
                    public string No
                    {
                        get { return mNo; }
                        set { mNo = value; }
                    }

                    //.... LineNo
                    public string LineNo
                    {
                        get { return mLineNo; }
                        set { mLineNo = value; }
                    }

                    //.... RelatedNo
                    public string RelatedNo
                    {
                        get { return mRelatedNo; }
                        set { mRelatedNo = value; }
                    }

                    //.... Type   
                    public eType Type
                    {
                        get { return mType; }
                        set { mType = value; }
                    }

                    #region "Customer:"
                    //=================

                        public clsCustomer Customer
                        {
                            get { return mCustomer; }
                            set { mCustomer = value; }
                        }

                    #endregion 


                    #region "NESTED CLASS:"

                    [Serializable]
                    public class clsCustomer
                    {

                        private string mName;
                        private string mOrderNo;
                        private string mMachineName;

                        //.... Name
                        public string Name
                        {
                            get { return mName; }
                            set { mName = value; }
                        }

                        //.... OrderNo
                        public string OrderNo
                        {
                            get { return mOrderNo; }
                            set { mOrderNo = value; }
                        }

                        //.... MachineName
                        public string MachineName
                        {
                            get { return mMachineName; }
                            set { mMachineName = value; }
                        }
                    }

                    #endregion
                }


                [Serializable]
                public class clsPNR
                {
                    private string mNo;
                    private string mRevNo;
                    private clsUnit mUnit = new clsUnit();                   


                    //.... Name
                    public string No
                    {
                        get { return mNo; }
                        set { mNo = value; }
                    }

                    //.... RevNo
                    public string RevNo
                    {
                        get { return mRevNo; }
                        set { mRevNo = value; }
                    }

                    public clsUnit Unit
                    {
                        get { return mUnit; }

                        set
                        {
                            mUnit = value;                           
                        }
                    }
                }

            #endregion
                
            #region "SERIALIZE-DESERIALIZE:"
                //-------------------------

                public Boolean Serialize(string FilePath_In)
                //==========================================
                {
                    try
                    {
                        IFormatter serializer = new BinaryFormatter();
                        string pFileName = FilePath_In + "1.BearingCAD";

                        FileStream saveFile = new FileStream(pFileName, FileMode.Create, FileAccess.Write);

                        serializer.Serialize(saveFile, this);

                        saveFile.Close();

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

                public object Deserialize(string FilePath_In)
                //===========================================
                {
                    IFormatter serializer = new BinaryFormatter();
                    string pFileName = FilePath_In + "1.BearingCAD";
                    FileStream openFile = new FileStream(pFileName, FileMode.Open, FileAccess.Read);
                    object pObj;
                    pObj = serializer.Deserialize(openFile);

                    openFile.Close();

                    return pObj;
                }

                #endregion

            #region "ICLONEABLE MEMBERS"

                public object Clone()
                //===================           
                {
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

        #endregion

       
    }
}
