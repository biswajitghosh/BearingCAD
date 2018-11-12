
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  clsFiles                               '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  31OCT18                                '
//                                                                              '
//===============================================================================


//    FILE NAMING CONVENTIONS:
//    -----------------------
//    ....FileName  ==>  Path, File, Extn
//    ....FileTitle ==>        File, Extn
//    ....File      ==>        File

//    *******************************************************************************
//    *          CLASS FOR  FILE MANIPULATION - READ & WRITE AND DELETE.            *
//    *******************************************************************************

using System;
using System.Globalization;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Xml;
using System.Linq;
using System.Configuration;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Word = Microsoft.Office.Interop.Word;
using EXCEL = Microsoft.Office.Interop.Excel;
using System.Collections.Specialized;

namespace BearingCAD22
{
    public class clsFiles
    {
        #region "FILE DEFINITIONS"
            //=====================
            private const int mcObjFile_Count = 2;

            //File Directories & Names:
            //=========================
            private const string mcDriveRoot_Client = "C:";                                       

            //  Installation:
            //  -------------
            //
            //....Root Directory of Client Machine:  
            private const string mcDirRoot = "\\BearingCAD\\";
                   
            //....Config File Name of Client Machine.               
            private const string mcConfigFile_Client ="BearingCAD22_Client.config";

            //....Config File Name of Client Machine.
            private const string mcConfigFile_Server = "BearingCAD22_Server.config";
                    
            //....LogoFile.     
            private const string mcLogo_Title = "Waukesha Logo.bmp"; 

        #endregion


        #region "MEMBER VARIABLE DECLARATIONS"
            //================================

            //....DriveRoot
            private string mDriveRoot;

            //....DB FileName and Type
            private static string mDBFileName, mDBServerName;                   
           
            //....Program Data File
            private string mFilePath_ProgramDataFile_EXCEL;
            private string mFileTitle_EXCEL_MatData;            
            private string mFileTitle_EXCEL_StdPartsData;
            private string mFileTitle_EXCEL_StdToolData;
        

            //....Design Tables.

            //....Directory of Design Table Template.
            private string mFilePath_Template_EXCEL;
            private string mFileTitle_Template_EXCEL_Parameter_Complete;
            

            //....Inventor Files.  
            //....Directory of Design Table Template.
            private string mFilePath_Template_Inventor;

            //....Project Dependent Files.
            private string mFileTitle_Template_Inventor_Radial;
            private string mFileTitle_Template_Inventor_Seal_Front;
            private string mFileTitle_Template_Inventor_Seal_Back;
            private string mFileTitle_Template_Inventor_Thrust_Front;
            private string mFileTitle_Template_Inventor_Thrust_Back;
            private string mFileTitle_Template_Inventor_Complete;
           
            private string mFileName_BearingCAD = "";

           
         #endregion


        #region "CLASS PROPERTY ROUTINES"
            //===========================

            public string FileName_BearingCAD
            {
                get { return mFileName_BearingCAD; }
                set { mFileName_BearingCAD = value; }
            }

            //READ-ONLY PROPERTIES:
            //=====================

            public string Logo
            //=================
            {
                get
                { 
                    return mcDriveRoot_Client + mcDirRoot + "Images\\" + mcLogo_Title; 
                }   
            }

            public static string DBFileName
            //==============================
            {
                get { return mDBFileName; }
            }


            public static string DBServerName
            //===============================
            {
                get { return mDBServerName; }
            }

            //....Program Data File
            public string FileTitle_EXCEL_MatData
            //===================================    
            {
                get
                {
                    return mDriveRoot + "\\" + mFilePath_ProgramDataFile_EXCEL + "\\" + mFileTitle_EXCEL_MatData;
                }
            }

            public string File_InputPath
            //==========================   
            {
                get
                {
                    return mcDriveRoot_Client + mcDirRoot  +"Projects\\V22";
                }
            }


            public string FileTitle_EXCEL_StdPartsData
            //===========================================    
            {
                get
                {
                    return mDriveRoot + "\\" + mFilePath_ProgramDataFile_EXCEL + "\\" + mFileTitle_EXCEL_StdPartsData;
                }
            }

            public string FileTitle_EXCEL_StdToolData
            //===========================================    
            {
                get
                {
                    return mDriveRoot + "\\" + mFilePath_ProgramDataFile_EXCEL + "\\" + mFileTitle_EXCEL_StdToolData;
                }
            }


                //public string FileTitle_Template_DDR
                ////==================================
                //{
                //    get
                //    { 
                //        return mDriveRoot + "\\" + mFilePath_Template_WORD + "\\" + mFileTitle_Template_DDR; 
                //    }
                //}


                public string FileTitle_Template_EXCEL_Parameter_Complete
                //========================================================    
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_EXCEL + "\\" + mFileTitle_Template_EXCEL_Parameter_Complete;
                    }
                }
        

                //....Inventor Files        
                public string FileTitle_Template_Inventor_Radial
                //===============================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Radial;
                    }
                }

                public string FileTitle_Template_Inventor_Seal_Front
                //==================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Seal_Front;
                    }
                }

                public string FileTitle_Template_Inventor_Seal_Back
                //==================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Seal_Back;
                    }
                }

                public string FileTitle_Template_Inventor_Thrust_Front
                //==================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Thrust_Front;
                    }
                }

                public string FileTitle_Template_Inventor_Thrust_Back
                //==================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Thrust_Back;
                    }
                }

                public string FileTitle_Template_Inventor_Complete
                //=================================================
                {
                    get
                    {
                        return mDriveRoot + "\\" + mFilePath_Template_Inventor + "\\" + mFileTitle_Template_Inventor_Complete;
                    }
                }

        #endregion

        public clsFiles()
        //===============
        {
            //....Reads Configuration File.
            ReadConfigFile();
        }

        #region "CLASS METHODS"

            //---------------------------------------------------------------------------
            //                      UTILITY ROUTINES - BEGIN                             '
            //---------------------------------------------------------------------------                 

            private void ReadConfigFile()
            //==========================
            {
                try      
                {
                    //  READ CLIENT CONFIGURATION FILE:
                    //  -------------------------------

                        string pConfigFileName_Client = mcDriveRoot_Client + mcDirRoot + mcConfigFile_Client;

                        FileStream pSW = new FileStream(pConfigFileName_Client, FileMode.Open,
                                                        FileAccess.Read, FileShare.ReadWrite);

                        //....Create the xmldocument
                            System.Xml.XmlDocument pXML = new System.Xml.XmlDocument();

                        //....Root Node of XML.
                            XmlNode pRoot;
                            pXML.Load(pSW);
                            pRoot = pXML.DocumentElement;

                        //....Child Node.
                            XmlNode pRootChild = pRoot.FirstChild;

                        //.....Get Installation Directory Of Server Configuration.
                            mDriveRoot = pRootChild.InnerText;
                            pXML = null;
                            pSW.Close();

                    //  READ SERVER CONFIGURATION FILE:
                    //  -------------------------------

                        string pConfigFileName_Server = mDriveRoot + mcDirRoot + mcConfigFile_Server;

                        if (!File.Exists(pConfigFileName_Server))
                        {
                            MessageBox.Show("Please Specify Proper Root Installation Directory in Client configuration file.", "Error");
                            System.Environment.Exit(0);
                        }

                        pSW = new FileStream(pConfigFileName_Server, FileMode.Open,
                                                            FileAccess.Read, FileShare.ReadWrite);

                        //....Create the xmldocument
                            pXML = new System.Xml.XmlDocument();

                        //....Root Node of XML.
                            pXML.Load(pSW);
                            pRoot = pXML.DocumentElement;

                            foreach (XmlNode pRChild in pRoot.ChildNodes)       
                            {
                                //.....Mapping Rules Implementation.
                                switch (pRChild.Name)
                                {
                                    case "SEREVERName":
                                        //-----------------
                                        mDBServerName = pRChild.InnerText;
                                        break;

                                    case "DataBaseName":
                                        //--------------
                                        mDBFileName = pRChild.InnerText;
                                        break;


                                    case "FilePath_ProgramDataFile_EXCEL":
                                        //-------------------------
                                        mFilePath_ProgramDataFile_EXCEL = pRChild.InnerText;
                                        break;

                                    case "FileTitle_EXCEL_MatData":
                                        //-------------------------
                                        mFileTitle_EXCEL_MatData = pRChild.InnerText;
                                        break;


                                    case "FileTitle_EXCEL_StdPartsData":
                                        //------------------------------
                                        mFileTitle_EXCEL_StdPartsData = pRChild.InnerText;
                                        break;

                                    case "FileTitle_EXCEL_StdToolData":
                                        //-------------------------
                                        mFileTitle_EXCEL_StdToolData = pRChild.InnerText;
                                        break;

                                    case "FilePath_Template_EXCEL":
                                        //-------------------------
                                        mFilePath_Template_EXCEL = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_EXCEL_Parameter_Complete":
                                        mFileTitle_Template_EXCEL_Parameter_Complete = pRChild.InnerText;
                                        break;


                                    case "FilePath_Template_Inventor":
                                        //----------------------
                                        mFilePath_Template_Inventor = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Radial":
                                        //------------------------------
                                        mFileTitle_Template_Inventor_Radial = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Seal_Front":
                                        //----------------------------
                                        mFileTitle_Template_Inventor_Seal_Front = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Seal_Back":
                                        //---------------------------------
                                        mFileTitle_Template_Inventor_Seal_Back = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Thrust_Front":
                                        //----------------------------
                                        mFileTitle_Template_Inventor_Thrust_Front = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Thrust_Back":
                                        //---------------------------------
                                        mFileTitle_Template_Inventor_Thrust_Back = pRChild.InnerText;
                                        break;

                                    case "FileTitle_Template_Inventor_Complete":
                                        //------------------------------------
                                        mFileTitle_Template_Inventor_Complete = pRChild.InnerText;
                                        break;
                                }
                            }
                            pXML = null;
                            pSW.Close();

                           // UpdateAppConfig(mDBServerName);

                }

                catch (FileNotFoundException pEXP)      //BG 13JUL09
                {
                    MessageBox.Show(pEXP.Message, "File Error");        
                }

            }

            #region "INPUT DATA:"
            //--------------------

            public void Import_DDR_Data(string FileName_In, ref clsProject Project_In)
            //======================================================================
            {
                //MessageBox.Show("All open Word files will be closed automatically.\nPlase save before proceeding.", "Warning: Word Files!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                CloseWordFiles();
                string pWordFileName = FileName_In;
               
                Word.Document pDoc = null;
                Word.ContentControls pContentControls = null;

                Word.Application pApp = new Word.Application();
                pApp.Documents.Open(FileName_In, Missing.Value, Missing.Value, false, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value);

                int pText6_Occur = 0, pText2_Occur = 0, pDropDown1_Occur = 0, pText7_Occur = 0, pText8_Occur = 0, pText5_Occur = 0;
                string pSO_No = "";
                string pQuoteNo = "";
                string pRNo = "";
                try
                {
                    pDoc = pApp.ActiveDocument;
                    pContentControls = pDoc.ContentControls;

                    foreach (Word.FormField pField in pDoc.FormFields)
                    {
                        string pVal = pField.Name + " " + pField.Result;

                        switch (pField.Name)
                        {
                            case "Text6":
                                //------------
                                pText6_Occur++;
                                if (pText6_Occur == 1)
                                {
                                    //txtCustName.Text = pField.Result;
                                    Project_In.SalesOrder.Customer.Name = pField.Result;
                                }
                                break;

                            case "Text2":
                                //------------
                                pText2_Occur++;
                                if (pText2_Occur == 1)
                                {
                                    pQuoteNo = pField.Result;
                                }
                                break;

                            case "Dropdown1":
                                //------------
                                pDropDown1_Occur++;
                                if (pDropDown1_Occur == 1)
                                {
                                    if (pField.Result == "Order")
                                    {
                                        Project_In.SalesOrder.Type = clsProject.clsSalesOrder.eType.Order;
                                    }
                                    else
                                    {
                                        Project_In.SalesOrder.Type = clsProject.clsSalesOrder.eType.Proposal;
                                    }
                                }
                                break;

                            case "Text7":
                                //------------
                                pText7_Occur++;
                                if (pText7_Occur == 1)
                                {
                                    //txtPartNo.Text = pField.Result;
                                    Project_In.PNR.No = pField.Result;
                                }
                                break;

                            case "Text8":
                                //------------
                                pText8_Occur++;
                                if (pText8_Occur == 2)
                                {
                                    if (pField.Result.Contains((char)13))
                                    {
                                        string pCustoOrderNo = "";
                                        string[] pLines = pField.Result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                                        for (int i = 0; i < pLines.Length; i++)
                                        {
                                            if (i == pLines.Length - 1)
                                            {
                                                pCustoOrderNo = pCustoOrderNo + pLines[i].Trim();
                                            }
                                            else
                                            {
                                                pCustoOrderNo = pCustoOrderNo + pLines[i].Trim() + ", ";
                                            }
                                        }
                                        //txtCustOrderNo.Text = pCustoOrderNo;
                                        Project_In.SalesOrder.Customer.OrderNo = pCustoOrderNo;
                                    }
                                    else
                                    {
                                        //txtCustOrderNo.Text = pField.Result;
                                        Project_In.SalesOrder.Customer.OrderNo = pField.Result;
                                    }
                                }
                                else if (pText8_Occur == 3)
                                {
                                    //txtCustMachineName.Text = pField.Result;
                                    Project_In.SalesOrder.Customer.MachineName = pField.Result;
                                }

                                break;

                            case "Text5":
                                //------------
                                pText5_Occur++;
                                if (pText5_Occur == 1)
                                {
                                    pSO_No = pField.Result;
                                    string[] pTemp_SO_First_Array = null;
                                    string[] pTemp_SO_Sub_Array = null;
                                    StringCollection pRelatedSO_No = new StringCollection();

                                    if (pSO_No.Contains("&"))
                                    {
                                        pTemp_SO_First_Array = pSO_No.Split('&');

                                        for (int i = 0; i < pTemp_SO_First_Array.Length; i++)
                                        {
                                            pTemp_SO_First_Array[i] = pTemp_SO_First_Array[i].Trim();
                                            if (pTemp_SO_First_Array[i].Contains(","))
                                            {
                                                pTemp_SO_Sub_Array = pTemp_SO_First_Array[i].Split(',');
                                                pSO_No = pTemp_SO_Sub_Array[0].Trim();

                                                for (int j = 0; j < pTemp_SO_Sub_Array.Length; j++)
                                                {
                                                    if (j > 0)
                                                    {
                                                        if (pTemp_SO_Sub_Array[j] != "")
                                                        {
                                                            if (!pTemp_SO_Sub_Array[j].Contains("-"))
                                                            {
                                                                string pSO_Val = modMain.ExtractPreData(pSO_No, "-") + "-" + pTemp_SO_Sub_Array[j].Trim();
                                                                pRelatedSO_No.Add(pSO_Val);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                pRelatedSO_No.Add(pTemp_SO_First_Array[i]);
                                            }

                                        }
                                    }

                                    Boolean pFlag = false;

                                    if (pSO_No == "" || pSO_No == "N/A")
                                    {
                                        pSO_No = pQuoteNo;
                                        pFlag = true;
                                    }

                                    if (pFlag == false && pQuoteNo != "" && pQuoteNo != "N/A")
                                    {
                                        pRelatedSO_No.Add(pQuoteNo);
                                    }

                                    for (int j = 0; j < pRelatedSO_No.Count; j++)
                                    {
                                        if (j == pRelatedSO_No.Count - 1)
                                        {
                                            pRNo = pRNo + pRelatedSO_No[j];
                                        }
                                        else
                                        {
                                            pRNo = pRNo + pRelatedSO_No[j] + ", ";
                                        }
                                    }
                                }

                                break;
                        }

                    }

                    if (pSO_No != "")
                    {
                        Project_In.SalesOrder.No = pSO_No;
                        Project_In.SalesOrder.RelatedNo = pRNo;
                    }

                    ////if (pSO_No != "")
                    ////{
                    ////    cmbSONo_Part1.Text = pSO_No.Substring(0, 2);
                    ////    if (pSO_No.Contains("-"))
                    ////    {
                    ////        txtSONo_Part2.Text = modMain.ExtractMidData(pSO_No, " ", "-");
                    ////    }
                    ////    else
                    ////    {
                    ////        txtSONo_Part2.Text = pSO_No.Substring(3);
                    ////        //txtSONo_Part2.Text = modMain.ExtractPostData(pSO_No, " ");
                    ////    }

                    ////    string pTemp = modMain.ExtractPostData(pSO_No, "-");

                    ////    Boolean pIsNumeric = false;
                    ////    foreach (char value in pTemp)
                    ////    {
                    ////        pIsNumeric = char.IsDigit(value);
                    ////    }

                    ////    if (pIsNumeric)
                    ////    {
                    ////        txtSONo_Part3.Text = Convert.ToString(System.Text.RegularExpressions.Regex.Replace(pTemp, "[^0-9]+", string.Empty));
                    ////    }

                    ////    txtRelatedSONo.Text = pRNo;
                    ////}
                    MessageBox.Show("Data have been imported successfully from \n '" + Path.GetFileName(pWordFileName) + "'", "Data Import from DDR", MessageBoxButtons.OK);

                }
                catch (Exception pExp)
                {
                    MessageBox.Show("Input Data is not in currect format.", "Error - Import Data - DDR", MessageBoxButtons.OK);
                }
                finally
                {
                    pDoc.Close();
                    pApp = null;
                    
                }

                //Cursor = Cursors.Default;
                //}
            }

            public void Retrieve_Params_XLRadial(string ExcelFileName_In, clsUnit.eSystem UnitSystem_In, clsOpCond OpCond_In,
                                                 clsBearing_Radial_FP Bearing_In, clsSeal[] Seal_In)
            //=================================================================================================================
            {
                //MessageBox.Show("All open Excel files will be closed automatically.\nPlase save before proceeding.", "Warning: Excel Files!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CloseExcelFiles();

                EXCEL.Application pApp = null;
                pApp = new EXCEL.Application();

                pApp.DisplayAlerts = false; //Don't want Excel to display error messageboxes

                //....Open Load.xls WorkBook.
                EXCEL.Workbook pWkbOrg = null;
                pWkbOrg = pApp.Workbooks.Open(ExcelFileName_In, Missing.Value, false, Missing.Value, Missing.Value, Missing.Value,
                                                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                                Missing.Value, Missing.Value, Missing.Value);

                string pVal = "";
                double pVal_Out = 0.0F;

                EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["XLRadial SI"];
                EXCEL.Range pExcelCellRange = null;



                pVal = Convert.ToString(pWkSheet.Cells[3, 12].value);

                if (pVal != "")
                {
                    OpCond_In.Speed = Convert.ToInt32(pVal);
                }

                //....Unit
                pVal = Convert.ToString(pWkSheet.Cells[4, 13].value);

                if (pVal.Trim() == "mm")
                {
                    pVal = "Metric";
                }
                else
                {
                    pVal = "English";
                }

                if (pVal != "")
                {
                    if (UnitSystem_In != (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pVal))
                    {
                        modMain.gProject.PNR.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), pVal.Trim());
                    }
                }

                //....Conversion Factor
                Double pConvF = 1;
                if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                {
                    pConvF = 25.4;
                }

                pVal = Convert.ToString(pWkSheet.Cells[5, 12].value);

                if (pVal != "")
                {
                    Bearing_In.Pad.L = Convert.ToDouble(pVal) / pConvF;
                }

                pVal = Convert.ToString(pWkSheet.Cells[6, 12].value);

                if (pVal != "")
                {
                    OpCond_In.Radial_Load = Convert.ToDouble(pVal) / 1000;
                }

                pVal = Convert.ToString(pWkSheet.Cells[7, 12].value);

                if (pVal != "")
                {
                    OpCond_In.Radial_LoadAng_Casing_SL = Convert.ToDouble(pVal);
                }

                pVal = Convert.ToString(pWkSheet.Cells[8, 12].value);

                if (pVal != "")
                {
                    Bearing_In.Pad.Type = (clsBearing_Radial_FP.clsPad.eLoadPos)
                                                                       Enum.Parse(typeof(clsBearing_Radial_FP.clsPad.eLoadPos), pVal);
                }

                pVal = Convert.ToString(pWkSheet.Cells[9, 12].value);

                if (pVal != "")
                {
                    Bearing_In.Pad.Count = Convert.ToInt32(pVal);
                }

                pVal = Convert.ToString(pWkSheet.Cells[10, 12].value);

                if (pVal != "")
                {
                    Bearing_In.Pad.Angle = Convert.ToInt32(pVal);
                }
               

                pVal = Convert.ToString(pWkSheet.Cells[11, 12].value);

                if (pVal != "")
                {
                    double pPivot_Offset = Convert.ToDouble(pVal) * 100;
                    Bearing_In.Pad.Pivot_Offset = pPivot_Offset;
                }

                pVal = Convert.ToString(pWkSheet.Cells[12, 12].value);

                if (pVal != "")
                {
                    if (pVal == "Split")
                    {
                        Bearing_In.SplitConfig = true;
                    }
                    else
                    {
                        Bearing_In.SplitConfig = false;
                    }
                }

                pVal = Convert.ToString(pWkSheet.Cells[13, 12].value);

                if (pVal != "")
                {
                    Bearing_In.ARP.Ang_Casing_SL = Convert.ToDouble(pVal);
                }

                pVal = Convert.ToString(pWkSheet.Cells[14, 12].value);

                if (pVal != "")
                {
                    Bearing_In.Pad.Pivot_AngStart_Casing_SL = Convert.ToDouble(pVal);
                }

                pVal = Convert.ToString(pWkSheet.Cells[18, 12].value);

                if (pVal != "")
                {
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        Bearing_In.PerformData.Power = modMain.gUnit.CFac_Power_MetToEng(Convert.ToDouble(pVal));
                    }
                    else
                    {
                        Bearing_In.PerformData.Power = Convert.ToDouble(pVal);
                    }

                }

                pVal = Convert.ToString(pWkSheet.Cells[19, 12].value);

                if (pVal != "")
                {
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        Bearing_In.PerformData.TempRise = modMain.gUnit.CFac_Temp_MetToEng(Convert.ToDouble(pVal));
                    }
                    else
                    {
                        Bearing_In.PerformData.TempRise = Convert.ToDouble(pVal);
                    }
                }

                pVal = Convert.ToString(pWkSheet.Cells[20, 12].value);

                if (pVal != "")
                {
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        Bearing_In.PerformData.FlowReqd = modMain.gUnit.CFac_LPM_MetToEng(Convert.ToDouble(pVal));
                    }
                    else
                    {
                        Bearing_In.PerformData.FlowReqd = Convert.ToDouble(pVal);
                    }
                }


                //....OilInlet Rerd Area
                pVal = Convert.ToString(pWkSheet.Cells[21, 12].value);

                if (pVal != "")
                {
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        Bearing_In.OilInlet.Annulus_Area = modMain.gUnit.CFac_Area_MetToEng(Convert.ToDouble(pVal));
                    }
                    else
                    {
                        Bearing_In.OilInlet.Annulus_Area = Convert.ToDouble(pVal);
                    }
                }

                //...Shaft Dia
                pVal = Convert.ToString(pWkSheet.Cells[12, 15].value);

                if (pVal != "")
                {
                    Bearing_In.DShaft_Range[0] = Convert.ToDouble(pVal) / pConvF;
                }

                pVal = Convert.ToString(pWkSheet.Cells[12, 16].value);

                if (pVal != "")
                {
                    Bearing_In.DShaft_Range[1] = Convert.ToDouble(pVal) / pConvF;
                }

                //....Bearing Bore
                pVal = Convert.ToString(pWkSheet.Cells[13, 15].value);

                if (pVal != "")
                {
                    Bearing_In.Bore_Range[0] = Convert.ToDouble(pVal) / pConvF;
                }

                pVal = Convert.ToString(pWkSheet.Cells[13, 16].value);

                if (pVal != "")
                {
                    Bearing_In.Bore_Range[1] = Convert.ToDouble(pVal) / pConvF;
                }

                //....Pad Bore
                pVal = Convert.ToString(pWkSheet.Cells[14, 15].value);

                if (pVal != "")
                {
                    Bearing_In.PadBore_Range[0] = Convert.ToDouble(pVal) / pConvF;
                }

                pVal = Convert.ToString(pWkSheet.Cells[14, 16].value);

                if (pVal != "")
                {
                    Bearing_In.PadBore_Range[1] = Convert.ToDouble(pVal) / pConvF;
                }

                //....Seal Bore - Front
                if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                {
                    pVal = Convert.ToString(pWkSheet.Cells[19, 15].value);

                    if (pVal != "")
                    {
                        Seal_In[0].DBore_Range[0] = Convert.ToDouble(pVal) / pConvF;
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[19, 16].value);

                    if (pVal != "")
                    {
                        Seal_In[0].DBore_Range[1] = Convert.ToDouble(pVal) / pConvF;
                    }
                }

                //....Seal Bore - Back
                if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                {
                    pVal = Convert.ToString(pWkSheet.Cells[20, 15].value);

                    if (pVal != "")
                    {
                        Seal_In[1].DBore_Range[0] = Convert.ToDouble(pVal) / pConvF;
                    }

                    pVal = Convert.ToString(pWkSheet.Cells[20, 16].value);

                    if (pVal != "")
                    {
                        Seal_In[1].DBore_Range[1] = Convert.ToDouble(pVal) / pConvF;
                    }
                }

                //....Bearing OD
                pVal = Convert.ToString(pWkSheet.Cells[21, 15].value);

                if (pVal != "")
                {
                    Bearing_In.OD_Range[0] = Convert.ToDouble(pVal) / pConvF;
                }

                pVal = Convert.ToString(pWkSheet.Cells[21, 16].value);

                if (pVal != "")
                {
                    Bearing_In.OD_Range[1] = Convert.ToDouble(pVal) / pConvF;
                }

                //....Oil Supply Type
                pVal = Convert.ToString(pWkSheet.Cells[4, 21].value);

                if (pVal != "")
                {
                    OpCond_In.OilSupply_Lube_Type = pVal;
                }

                //....Oil Supply Pressure
                pVal = Convert.ToString(pWkSheet.Cells[5, 21].value);

                if (pVal != "")
                {
                    OpCond_In.OilSupply_Press = Convert.ToDouble(pVal);
                }

                //....Oil Supply Temp
                pVal = Convert.ToString(pWkSheet.Cells[6, 21].value);

                if (pVal != "")
                {
                    OpCond_In.OilSupply_Temp = Convert.ToDouble(pVal);
                }

                //....Oil Noozle Dia
                pVal = Convert.ToString(pWkSheet.Cells[7, 21].value);

                if (pVal != "")
                {
                    Bearing_In.OilInlet.Orifice_D = Convert.ToDouble(pVal) / pConvF;
                }

                //....Number of Nozzle
                pVal = Convert.ToString(pWkSheet.Cells[8, 21].value);

                if (pVal != "")
                {
                    Bearing_In.OilInlet.Orifice_Count = Convert.ToInt32(pVal);
                }

                //....Pad Shape
                pVal = Convert.ToString(pWkSheet.Cells[11, 21].value);

                if (pVal != "")
                {
                    if (pVal.Contains("Uniform"))
                    {
                        Bearing_In.Pad.T_Pivot_Checked = true;
                    }
                    else
                    {
                        Bearing_In.Pad.T_Pivot_Checked = false;
                    }
                }

                //....Pad Thick (Leading)
                pVal = Convert.ToString(pWkSheet.Cells[12, 21].value);

                if (pVal != "")
                {
                    Bearing_In.Pad.T_Lead = Convert.ToDouble(pVal) / pConvF;
                }

                //....Pad Thick (Pivot)
                pVal = Convert.ToString(pWkSheet.Cells[13, 21].value);

                if (pVal != "")
                {
                    Bearing_In.Pad.T_Pivot = Convert.ToDouble(pVal) / pConvF;
                }

                //....Pad Thick (Trailing)
                pVal = Convert.ToString(pWkSheet.Cells[14, 21].value);

                if (pVal != "")
                {
                    Bearing_In.Pad.T_Trail = Convert.ToDouble(pVal) / pConvF;
                }

                //....Lining T
                pVal = Convert.ToString(pWkSheet.Cells[15, 21].value);

                if (pVal != "")
                {
                    Bearing_In.LiningT = Convert.ToDouble(pVal) / pConvF;
                }

                //....Pad RFillet
                pVal = Convert.ToString(pWkSheet.Cells[16, 21].value);

                if (pVal != "")
                {
                    Bearing_In.Pad.RFillet = Convert.ToDouble(pVal) / pConvF;
                }

                //....Axial Seal Gap
                pVal = Convert.ToString(pWkSheet.Cells[17, 21].value);

                if (pVal != "")
                {
                    Bearing_In.MillRelief.AxialSealGap[0] = Convert.ToDouble(pVal) / pConvF;
                    Bearing_In.MillRelief.AxialSealGap[1] = Convert.ToDouble(pVal) / pConvF;
                }

                //....Seal Blade Thickness   
                pVal = Convert.ToString(pWkSheet.Cells[18, 21].value);

                //....Seal Bore - Front
                if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                {

                    if (pVal != "")
                    {
                        Seal_In[0].Blade.T = Convert.ToDouble(pVal) / pConvF;
                    }
                }

                //....Seal Bore - Back
                if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                {

                    if (pVal != "")
                    {
                        Seal_In[1].Blade.T = Convert.ToDouble(pVal) / pConvF;
                    }
                }

                //....Web Thickness
                pVal = Convert.ToString(pWkSheet.Cells[19, 21].value);
                if (pVal != "")
                {
                    Bearing_In.FlexurePivot.Web_T = Convert.ToDouble(pVal) / pConvF;
                }

                //....Web Height
                pVal = Convert.ToString(pWkSheet.Cells[20, 21].value);
                if (pVal != "")
                {
                    Bearing_In.FlexurePivot.Web_H = Convert.ToDouble(pVal) / pConvF;
                }

                //....Web Fillet
                pVal = Convert.ToString(pWkSheet.Cells[21, 21].value);
                if (pVal != "")
                {
                    Bearing_In.FlexurePivot.Web_RFillet = Convert.ToDouble(pVal) / pConvF;
                }

                pWkbOrg.Close();
                pApp.Quit();

                MessageBox.Show("Data have been imported successfully from '" + Path.GetFileName(ExcelFileName_In) + "'", "Data Import from XLRadial", MessageBoxButtons.OK);
            }

            #endregion

            #region "OUTPUT DATA:"

            public void Write_Parameter_Complete(clsProject Project_In, string FileName_In, Boolean Visible_Status_In)
            //========================================================================================================
            {
                try
                {
                    object mobjMissing = Missing.Value;              //....Missing object.
                    EXCEL.Application pApp = null;
                    pApp = new EXCEL.Application();


                    //....Open Original WorkBook.
                    EXCEL.Workbook pWkbOrg = null;

                    pWkbOrg = pApp.Workbooks.Open(modMain.gFiles.FileTitle_Template_EXCEL_Parameter_Complete, mobjMissing, false,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
                                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing);

                    //....Open WorkSheet - 'Complete ASSY'            
                    EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Complete Assy"];
                    Write_Parameter_Complete_Assy(Project_In, modMain.gOpCond, pWkSheet);

                    //....Open WorkSheet - 'Radial Bearing'            
                    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Radial Bearing"];
                    Write_Parameter_Complete_Radial(Project_In, pWkSheet);

                    //....Open WorkSheet - 'Mounting'    
                    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Mounting"];
                    Writer_Parameter_Complete_Mounting(Project_In, pWkSheet);

                    //....EndPlate: Seal
                    clsSeal[] mEndSeal = new clsSeal[2];
                    for (int i = 0; i < 2; i++)
                    {
                        if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                        {
                            mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
                        }
                    }


                    //....Open WorkSheet - 'Front Config - Seal' 
                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                    {
                        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Seal"];
                        Write_Parameter_Complete_Seal_Front(Project_In, mEndSeal[0], pWkSheet);
                    }
                    else
                    {
                        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Seal"];
                        pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
                    }

                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                    {
                        //....Open WorkSheet - 'Back Config - Seal'    
                        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Seal"];
                        Write_Parameter_Complete_Seal_Back(Project_In, mEndSeal[0], pWkSheet);
                    }
                    else
                    {
                        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Seal"];
                        pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
                    }


                    //.............
                    //....EndPlate: Thurst Bearing
                    clsBearing_Thrust_TL[] mEndTB = new clsBearing_Thrust_TL[2];
                    for (int i = 0; i < 2; i++)
                    {
                        if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.TL_TB)
                        {
                            mEndTB[i] = (clsBearing_Thrust_TL)((clsBearing_Thrust_TL)(modMain.gProject.Product.EndPlate[i])).Clone();
                        }
                    }

                    ////....Open WorkSheet - 'Front TL Thurst Bearing' 
                    //if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB)
                    //{
                    //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Thurst Bearing TL"];
                    //    Write_Parameter_Complete_Thrust_Front(modMain.gProject, mEndTB[0], pWkSheet);
                    //}
                    //else
                    //{
                    //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Thurst Bearing TL"];
                    //    pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
                    //}

                    ////....Open WorkSheet - 'Back TL Thurst Bearing' 
                    //if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                    //{
                    //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Thurst Bearing TL"];
                    //    Write_Parameter_Complete_Thrust_Back(modMain.gProject, mEndTB[1], pWkSheet);
                    //}
                    //else
                    //{
                    //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Thurst Bearing TL"];
                    //    pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
                    //}

                    //..............

                    //pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Accessories"];
                    //Write_Parameter_Complete_Accessories(modMain.gProject, modMain.gProject.Product.Accessories, pWkSheet);

                    DateTime pDate = DateTime.Now;
                    //String pFileName = FileName_In + "\\CAD Neutral Data Set_" + pDate.ToString("ddMMMyyyy").ToUpper() + ".xlsx";
                    String pFileName = FileName_In + "\\CAD Neutral Data Set_RevA.xlsx";

                    EXCEL.XlSaveAsAccessMode pAccessMode = Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive;
                    pWkbOrg.SaveAs(pFileName, mobjMissing, mobjMissing,
                                        mobjMissing, mobjMissing, mobjMissing, pAccessMode,
                                        mobjMissing, mobjMissing, mobjMissing,
                                        mobjMissing, mobjMissing);

                    pApp.Visible = Visible_Status_In;
                    if (!Visible_Status_In)
                    {
                        pWkbOrg.Close();
                        pWkbOrg = null;
                        pApp = null;
                    }
                }
                catch
                {
                }
            }

            private void Write_Parameter_Complete_Assy(clsProject Project_In, clsOpCond OpCond_In,
                                                              EXCEL.Worksheet WorkSheet_In)
            //=======================================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    Double pConv_InchToMM = 25.4;

                    ////....EndPlate: Seal
                    //clsSeal[] mEndSeal = new clsSeal[2];
                    //for (int i = 0; i < 2; i++)
                    //{
                    //    if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                    //    {
                    //        mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
                    //    }
                    //}

                    for (int i = 3; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {
                                case "SalesOrder.Customer.Name":
                                    WorkSheet_In.Cells[i, 4] = Project_In.SalesOrder.Customer.Name;
                                    break;

                                case "SalesOrder.Customer.OrderNo":
                                    WorkSheet_In.Cells[i, 4] = Project_In.SalesOrder.Customer.OrderNo;
                                    break;

                                case "SalesOrder.Customer.MachineName":
                                    WorkSheet_In.Cells[i, 4] = Project_In.SalesOrder.Customer.MachineName;
                                    break;

                                case "SalesOrder.No":
                                    WorkSheet_In.Cells[i, 4] = Project_In.SalesOrder.No;
                                    break;

                                case "SalesOrder.RelatedNo":
                                    WorkSheet_In.Cells[i, 4] = Project_In.SalesOrder.RelatedNo;
                                    break;

                                case "PNR.No":
                                    WorkSheet_In.Cells[i, 4] = Project_In.PNR.No;
                                    break;

                                case "Bearing.Design":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Design.ToString();
                                    break;

                                case "Bearing.SplitConfig":
                                    String pSplitConfig = "";
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SplitConfig)
                                    {
                                        pSplitConfig = "Y";
                                    }
                                    else
                                    {
                                        pSplitConfig = "N";
                                    }
                                    WorkSheet_In.Cells[i, 4] = pSplitConfig;
                                    break;

                                //case "Bearing.DShaft_Range[0], Bearing.DShaft_Range[1]":
                                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                //    {
                                //        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0] * pConv_InchToMM) + ", " +
                                //                                  modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1] * pConv_InchToMM);
                                //    }
                                //    else
                                //    {
                                //        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0]) + ", " +
                                //                                    modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1]);
                                //    }
                                //    break;

                                case "EndPlate[0].Type":
                                    WorkSheet_In.Cells[i, 4] = Project_In.Product.EndPlate[0].Type.ToString().Replace("_", " ");
                                    break;

                                case "EndPlate[1].Type":
                                    WorkSheet_In.Cells[i, 4] = Project_In.Product.EndPlate[1].Type.ToString().Replace("_", " ");
                                    break;

                                case "OpCond.Speed":
                                    WorkSheet_In.Cells[i, 4] = OpCond_In.Speed;
                                    break;

                                case "OpCond.Rot_Directionality":
                                    WorkSheet_In.Cells[i, 4] = "";
                                    break;

                                case "OpCond.Radial_Load":
                                    WorkSheet_In.Cells[i, 4] = OpCond_In.Radial_Load;
                                    break;

                                case "OpCond.Radial_LoadAng_Casing_SL":
                                    WorkSheet_In.Cells[i, 4] = OpCond_In.Radial_LoadAng_Casing_SL;
                                    break;

                                //case "OpCond.Thrust_Load_Range[0]":
                                //    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB)
                                //    {
                                //        WorkSheet_In.Cells[i, 4] = OpCond_In.Thrust_Load_Range[0];
                                //    }
                                //    break;

                                //case "OpCond.Thrust_Load_Range[1]":
                                //    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                                //    {
                                //        WorkSheet_In.Cells[i, 4] = OpCond_In.Thrust_Load_Range[1];
                                //    }
                                //    break;

                                case "OpCond.OilSupply.Lube_Type":
                                    WorkSheet_In.Cells[i, 4] = OpCond_In.OilSupply.Lube_Type;
                                    break;

                                case "OpCond.OilSupply.Reqd_Flow":
                                    if (modMain.gProject.Product.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.ConvDoubleToStr(modMain.gProject.Product.Unit.CFac_GPM_EngToMet(OpCond_In.OilSupply.Flow_Reqd), "#0.00");
                                    }
                                    else
                                    {
                                        WorkSheet_In.Cells[i, 4] = OpCond_In.OilSupply.Flow_Reqd;
                                    }
                                   
                                    break;

                                case "OpCond.OilSupply.Press":
                                    WorkSheet_In.Cells[i, 4] = OpCond_In.OilSupply.Press;
                                    break;

                                case "OpCond.OilSupply.Temp":
                                    WorkSheet_In.Cells[i, 4] = OpCond_In.OilSupply.Temp;
                                    break;

                                case "Bearing.PerformData.Power":
                                    if (modMain.gProject.Product.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.ConvDoubleToStr(modMain.gProject.Product.Unit.CFac_Power_EngToMet(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power), "##0.00"); ;
                                    }
                                    else
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power;
                                    }
                                    break;

                                case "Bearing.PerformData.TempRise":
                                    if (modMain.gProject.Product.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.ConvDoubleToStr(modMain.gProject.Product.Unit.CFac_Temp_EngToMet(((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise), "#0.0");
                                    }
                                    else
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.TempRise;
                                    }
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                }
            }


            private void Write_Parameter_Complete_Radial(clsProject Project_In, EXCEL.Worksheet WorkSheet_In)
            //================================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    Double pConvF = 1;
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }
                    for (int i = 3; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {
                                //....Material:
                                case "Bearing.Mat.Base":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base + ": WBM " + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.WCode.Base; 
                                    break;

                                //....Geometry:

                                //....Diameter:
                                case "Bearing.Mat.Lining":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining;
                                    break;

                                case "Bearing.LiningT":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT * pConvF);
                                    break;

                                case "Bearing.OD()":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OD() * pConvF);
                                    break;


                                case "Bearing.PadBore()":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).PadBore() * pConvF);
                                    break;

                                case "Bearing.Bore()":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Bore() * pConvF);
                                    break;

                                case "Bearing.DShaft()":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft() * pConvF);
                                    break;

                                //....Length:
                                case "L_Available":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.L_Available * pConvF);
                                    break;

                                case "L_Tot()":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.L_Tot() * pConvF);
                                    break;

                                //case "Dist_ThrustFace[0/1]":
                                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                //    {
                                //        if (Project_In.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB ||
                                //           Project_In.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                                //        {
                                //            //....Dist_ThrustFace
                                //            if ((Project_In.Product.Dist_ThrustFace[0] > modMain.gcEPS) && (Project_In.Product.Dist_ThrustFace[1] < modMain.gcEPS))
                                //            {
                                //                WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( Project_In.Product.Dist_ThrustFace[0] * pConvF);

                                //            }

                                //            else if ((Project_In.Product.Dist_ThrustFace[0] < modMain.gcEPS) && (Project_In.Product.Dist_ThrustFace[1] > modMain.gcEPS))
                                //            {
                                //                WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( Project_In.Product.Dist_ThrustFace[1] * pConvF);

                                //            }

                                //            else if ((Project_In.Product.Dist_ThrustFace[0] > modMain.gcEPS) && (Project_In.Product.Dist_ThrustFace[1] > modMain.gcEPS))
                                //            {
                                //                WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( Project_In.Product.Dist_ThrustFace[0] * pConvF);

                                //            }
                                //        }
                                //    }
                                //    else
                                //    {

                                //        if (Project_In.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB ||
                                //            Project_In.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                                //        {
                                //            //....Dist_ThrustFace
                                //            if ((Project_In.Product.Dist_ThrustFace[0] > modMain.gcEPS) && (Project_In.Product.Dist_ThrustFace[1] < modMain.gcEPS))
                                //            {
                                //                WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( Project_In.Product.Dist_ThrustFace[0]);

                                //            }

                                //            else if ((Project_In.Product.Dist_ThrustFace[0] < modMain.gcEPS) && (Project_In.Product.Dist_ThrustFace[1] > modMain.gcEPS))
                                //            {
                                //                WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( Project_In.Product.Dist_ThrustFace[1]);

                                //            }

                                //            else if ((Project_In.Product.Dist_ThrustFace[0] > modMain.gcEPS) && (Project_In.Product.Dist_ThrustFace[1] > modMain.gcEPS))
                                //            {
                                //                WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( Project_In.Product.Dist_ThrustFace[0]);

                                //            }
                                //        }
                                //    }

                                //    break;

                                //....Pad:
                                case "Bearing.Pad.Type":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Type.ToString();
                                    break;

                                case "Bearing.Pad.Count":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count;
                                    break;

                                case "Bearing.Pad.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.L * pConvF);
                                    break;

                                case "Bearing.Pad.Angle":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Angle;
                                    break;

                                //....Pad Pivot:
                                case "Bearing.Pad.Pivot.Offset":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.Offset);
                                    break;

                                case "Bearing.Pad.Pivot.AngStart":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.AngStart_Casing_SL;
                                    break;

                                //....Pad Thickness:
                                case "Bearing.Pad.T.Lead":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Lead * pConvF);
                                    break;

                                case "Bearing.Pad.T.Pivot":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Pivot * pConvF);
                                    break;

                                case "Bearing.Pad.T.Trail":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Trail * pConvF);
                                    break;

                                case "Bearing.Pad.Rfillet":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.RFillet * pConvF);
                                    break;

                                //....Flexure Pivot:
                                case "Bearing.FlexurePivot.Web.T":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.T * pConvF);
                                    break;

                                case "Bearing.FlexurePivot.Web.RFillet":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.RFillet * pConvF);
                                    break;

                                case "Bearing.FlexurePivot.Web.H":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.H * pConvF);
                                    break;

                                case "Bearing.FlexurePivot.GapEDM":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.GapEDM * pConvF);
                                    break;


                                case "Bearing.MillRelief.D_PadRelief()":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.D_PadRelief() * pConvF);
                                    break;

                                case "Bearing.MillRelief.AxialSealGap[0]":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.AxialSealGap[0] * pConvF);
                                    break;

                                case "Bearing.MillRelief.Exists":
                                    String pVal = "";
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.Exists)
                                    {
                                        pVal = "Y";
                                    }
                                    else
                                    {
                                        pVal = "N";
                                    }
                                    WorkSheet_In.Cells[i, 4] = pVal;
                                    break;

                                case "Bearing.MillRelief.D":
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.Exists)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.D() * pConvF);
                                    }
                                    break;

                                //....DESIGN DETAILS:
                                case "Bearing.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).L * pConvF);
                                    break;

                                case "Bearing.Depth_EndPlate[0]":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndPlate[0] * pConvF);
                                    break;

                                case "Bearing.Depth_EndPlate[1]":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndPlate[1] * pConvF);
                                    break;

                                case "EndPlate[0].L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[0].L * pConvF);
                                    break;

                                case "EndPlate[1].L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[0].L * pConvF);
                                    break;

                                //....Oil inlet:
                                case "Bearing.OilInlet.Count_MainOilSupply":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Count_MainOilSupply;
                                    break;

                                //....Orifice:
                                case "Bearing.OilInlet.Orifice.Count":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Count;
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Count_MainOilSupply;
                                    break;

                                case "Bearing.OilInlet.Orifice.D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D * pConvF);
                                    break;

                                case "Bearing.OilInlet.Orifice.StartPos":

                                    Double pAng_Start_Pos = 0;
                                    int pPad_Count = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count;
                                    Double pPad_Angle = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Angle;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.StartPos == clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.Below)
                                    {

                                        pAng_Start_Pos = -(360 / pPad_Count - pPad_Angle) / 2;
                                    }
                                    else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.StartPos == clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.On)
                                    {

                                        pAng_Start_Pos = 0;
                                    }
                                    else if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.StartPos == clsBearing_Radial_FP.clsOilInlet.eOrificeStartPos.Above)
                                    {

                                        pAng_Start_Pos = (360 / pPad_Count - pPad_Angle) / 2;
                                    }

                                    WorkSheet_In.Cells[i, 4] = pAng_Start_Pos;
                                    break;

                                case "Bearing.OilInlet.Orifice.D_Cbore":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D_CBore * pConvF);
                                    break;

                                case "Bearing.OilInlet.Orifice.Loc_Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Loc_Back * pConvF);
                                    break;

                                case "Bearing.OilInlet.Orifice.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Calc_Orifice_L() * pConvF);
                                    break;

                                //....Annulus:     

                                case "Bearing.OilInlet.Annulus.Exists":
                                    String pAnnulus_Exists = "";
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Exists)
                                    {
                                        pAnnulus_Exists = "Y";
                                    }
                                    else
                                    {
                                        pAnnulus_Exists = "N";
                                    }

                                    WorkSheet_In.Cells[i, 4] = pAnnulus_Exists;
                                    break;

                                case "Bearing.OilInlet.Annulus.Area_Reqd":                                    
                                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(modMain.gProject.PNR.Unit.CFac_Area_EngToMet(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Area));
                                    }
                                    else
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Area);
                                    }
                                    
                                    break;

                                case "Bearing.OilInlet.Annulus.Wid":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Wid * pConvF);
                                    break;

                                case "Bearing.OilInlet.Annulus.Depth":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Depth * pConvF);
                                    break;

                                case "Bearing.OilInlet.Annulus.D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.D * pConvF);
                                    break;

                                case "Bearing.OilInlet.Annulus.Loc_Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Loc_Back * pConvF);
                                    break;

                                //case "Bearing.OilInlet.Annulus_V()":
                                //    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus_V(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.D , ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Wid );
                                //    break;

                                //....Flange:      

                                //case "Bearing.Flange.Exists":
                                //    String pFlange_Exists = "";
                                //    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Exists)
                                //    {
                                //        pFlange_Exists = "Y";
                                //    }
                                //    else
                                //    {
                                //        pFlange_Exists = "N";
                                //    }

                                //    WorkSheet_In.Cells[i, 4] = pFlange_Exists;
                                //    break;

                                //case "Bearing.Flange.D":
                                //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.D * pConvF);
                                //    break;

                                //case "Bearing.Flange.Wid":
                                //    WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Wid * pConvF);
                                //    break;

                                //case "Bearing.Flange.DimStart_Front":
                                //    WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.DimStart_Back * pConvF);
                                //    break;

                                //....Anti-Rotation Pin:      

                                //....Hardware:  
                                case "Bearing.ARP.Spec.Unit.System":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.Spec.Unit.System.ToString();
                                    break;

                                case "Bearing.ARP.Spec.Type": 
                                    WorkSheet_In.Cells[i, 4] =((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.Spec.Type;
                                    break;

                                case "Bearing.ARP.Spec.Mat":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.Spec.Mat;
                                    break;

                                case "Bearing.ARP.Spec.D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.D() * pConvF);
                                    break;

                                case "Bearing.ARP.Spec.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.Spec.L);
                                    break;

                                case "Bearing.ARP.PN":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.PN;
                                    break;

                                case "Bearing.ARP.Hole.Depth_Low":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.Hole.Depth_Low * pConvF);
                                    break;

                                case "Bearing.ARP.Stickout":
                                    Double pL = 0.0;
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pL = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.Spec.L / pConvF;
                                    }
                                    else
                                    {
                                        pL = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.Spec.L;
                                    }
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Stickout(pL) * pConvF);
                                    break;

                                case "Bearing.ARP.Loc_Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Loc_Back * pConvF);
                                    break;

                                case "Bearing.ARP.Ang_Casing_SL":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Ang_Casing_SL;
                                    break;

                                case "Bearing.ARP.InsertedOn":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.InsertedOn.ToString();
                                    break;

                                case "Bearing.ARP.Offset":
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Offset > modMain.gcEPS)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Offset * pConvF);
                                    }
                                    else
                                    {
                                        WorkSheet_In.Cells[i, 4] = 0;
                                    }
                                    break;

                                case "Bearing.ARP.Offset_Direction":
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Offset > modMain.gcEPS)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Offset_Direction.ToString();
                                    }
                                    else
                                    {
                                        WorkSheet_In.Cells[i, 4] = "None";
                                    }
                                    break;

                                case "Bearing.ARP.Angle_Horz":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Ang_Horz();
                                    break;


                                //....S/L Hardware:      

                                //....Screw:  

                                case "Bearing.SL.Screw.Spec.Unit.System":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Spec.Unit.System.ToString();
                                    break;

                                case "Bearing.SL.Screw.Spec.Type":
                                    String pSpec_Type = "";
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pSpec_Type = "Antigo ISO Metric Profile";
                                    }
                                    else
                                    {
                                        pSpec_Type = "ANSI Unified Screw Threads";
                                    }
                                    WorkSheet_In.Cells[i, 4] = pSpec_Type;//((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Spec.Type;
                                    break;

                                case "Bearing.SL.Screw.Spec.Mat":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Spec.Mat;
                                    break;

                                case "Bearing.SL.Screw.D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.D() * pConvF);
                                    break;

                                case "Bearing.SL.Screw.Spec.Pitch":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Spec.Pitch);
                                    break;

                                case "Bearing.SL.Screw.Spec.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Spec.L);
                                    break;

                                case "Bearing.SL.Screw.PN":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.PN;
                                    break;

                                case "Bearing.SL.Screw.Hole.CBore.D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Hole.CBore.D * pConvF);
                                    break;

                                case "Bearing.SL.Screw.Hole.CBore.D_Drill":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Hole.D_Drill * pConvF);
                                    break;

                                case "Bearing.SL.Screw.Hole.CBore.Depth":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Hole.CBore.Depth * pConvF);
                                    break;

                                case "Bearing.SL.Screw.Hole.Depth.TapDrill":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Hole.Depth.TapDrill * pConvF);
                                    break;

                                case "Bearing.SL.Screw.Hole.Depth.Tap":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Hole.Depth.Tap * pConvF);
                                    break;

                                case "Bearing.SL.Screw.Hole.Depth.Engagement":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Hole.Depth.Engagement * pConvF);
                                    break;


                                //....Left Location:     

                                case "Bearing.SL.LScrew.Center":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew.Center * pConvF);
                                    break;

                                case "Bearing.SL.LScrew.Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew.Back * pConvF);
                                    break;

                                //....Right Location:     

                                case "Bearing.SL.RScrew.Center":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew.Center * pConvF);
                                    break;

                                case "Bearing.SL.RScrew.Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew.Back * pConvF);
                                    break;

                                //....Dowel:      

                                case "Bearing.SL.Dowel.Spec.Type":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel.Spec.Type;
                                    break;

                                case "Bearing.SL.Dowel.Spec.Mat":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel.Spec.Mat;
                                    break;

                                case "Bearing.SL.Dowel.D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel.D() * pConvF);
                                    break;

                                case "Bearing.SL.Dowel.Spec.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel.Spec.L);
                                    break;

                                case "Bearing.SL.Dowel.PN":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel.PN;
                                    break;

                                case "Bearing.SL.Dowel.Hole.Depth_Up":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel.Hole.Depth_Up * pConvF);
                                    break;

                                case "Bearing.SL.Dowel.Hole.Depth_Low":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel.Hole.Depth_Low * pConvF);
                                    break;


                                //....Left Location:     

                                case "Bearing.SL.Ldowel_Loc.Center":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Center * pConvF);
                                    break;

                                case "Bearing.SL.Ldowel_Loc.Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Back * pConvF);
                                    break;

                                //....Right Location:      

                                case "Bearing.SL.Rdowel_Loc.Center":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Center * pConvF);
                                    break;

                                case "Bearing.SL.Rdowel_Loc.Back":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Back * pConvF);
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                }

            }


            private void Writer_Parameter_Complete_Mounting(clsProject Project_In, EXCEL.Worksheet WorkSheet_In)
            //=============================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    Double pConvF = 1;
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }
                    for (int i = 2; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {
                                case "Bearing.Mount_Bolting":
                                    WorkSheet_In.Cells[i, 4] = "Both";//((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Bolting.ToString();
                                    break;

                                case "Bearing.EndPlate[0].OD":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[0].OD * pConvF);
                                    break;

                                case "Bearing.TWall_CB_EndPlate(0)":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).TWall_CB_EndPlate(0) * pConvF);
                                    break;

                                //....Front End Config:
                                //case "Bearing_In.TWall_BearingCB(0)":
                                //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).TWall_BearingCB(0)*pConvF);
                                //    break;

                                case "Bearing.Mount.BC[0].D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[0].D * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.Type":
                                    String pSpec_Type = "";
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pSpec_Type = "Antigo ISO Metric Profile";
                                    }
                                    else
                                    {
                                        pSpec_Type = "ANSI Unified Screw Threads";
                                    }
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Spec.Type;
                                    WorkSheet_In.Cells[i, 4] = pSpec_Type;
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.Mat":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Spec.Mat + ": WBM" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.WCode.Base;
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].D() * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.Pitch":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Spec.Pitch );
                                    break;

                                case "Bearing.Mount.Screw[0].Spec.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Spec.L);
                                    break;

                                case "Bearing.Mount.BC[0].Count"://Bearing.Mount.Screw[0].Hole.Count
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[0].Count;
                                    break;

                                case "Bearing.Mount.BC[0].AngStart":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[0].AngStart;
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[0]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[0];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[0];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[0].Count > 1)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[1]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[1];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[1];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[0].Count > 2)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[2]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[2];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[2];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[0].Count > 3)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[3]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[3];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[3];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[0].Count > 4)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[4]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[4];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[4];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[0].Count > 5)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[5]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[5];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[5];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[0].Count > 6)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[0].AngBet[6]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[6];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[6];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[0].Count > 7)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[0].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                
                                case "Bearing.Mount.Screw[0].Hole.Mounting.Type":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Spec.Type;
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.D_Drill":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.D_Drill * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.CBore.D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.CBore.D * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.CBore.Depth":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.CBore.Depth * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.Depth.TapDrill":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Depth.TapDrill * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.Depth.Tap":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Depth.Tap * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[0].Hole.Depth.Engagement":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Depth.Engagement * pConvF);
                                    break;

                                case "Bearing.EndPlate[1].OD":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[1].OD * pConvF);
                                    break;

                                case "Bearing.TWall_CB_EndPlate(1)":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).TWall_CB_EndPlate(1) * pConvF);
                                    break;

                                //....Front End Config:
                                //case "Bearing_In.TWall_BearingCB(0)":
                                //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).TWall_BearingCB(0)*pConvF);
                                //    break;

                                case "Bearing.Mount.BC[1].D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[1].D * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[1].Spec.Type":
                                    pSpec_Type = "";
                                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Spec.Unit.System == clsUnit.eSystem.Metric)
                                    {
                                        pSpec_Type = "Antigo ISO Metric Profile";
                                    }
                                    else
                                    {
                                        pSpec_Type = "ANSI Unified Screw Threads";
                                    }
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Spec.Type;
                                    WorkSheet_In.Cells[i, 4] = pSpec_Type;    
                                //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Spec.Type;
                                    break;

                                case "Bearing.Mount.Screw[1].Spec.Mat":
                                   // WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Spec.Mat;
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Spec.Mat + ": WBM" + ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.WCode.Base;
                                    break;

                                case "Bearing.Mount.Screw[1].D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].D() * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[1].Spec.Pitch":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Spec.Pitch );
                                    break;

                                case "Bearing.Mount.Screw[1].Spec.L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Spec.L );
                                    break;

                                case "Bearing.Mount.BC[1].Count":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[1].Count;
                                    break;

                                case "Bearing.Mount.BC[1].AngStart":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[1].AngStart;
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[0]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[0];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[0];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[1].Count > 1)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[1]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[1];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[1];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[1].Count > 2)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[2]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[2];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[2];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[1].Count > 3)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[3]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[3];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[3];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[1].Count > 4)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[4]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[4];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[4];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[0].Count > 5)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[5]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[5];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[5];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[1].Count > 6)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                                case "Bearing.Mount.BC[1].AngBet[6]":
                                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[6];
                                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].EquiSpaced)
                                    {
                                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[6];
                                    }
                                    else
                                    {
                                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
                                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.BC[1].Count > 7)
                                        {
                                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.BC[1].AngBet[0];
                                        }
                                        else
                                        {
                                            WorkSheet_In.Cells[i, 4] = 0;
                                        }
                                    }
                                    break;

                            
                                case "Bearing.Mount.Screw[1].Hole.Mounting.Type":
                                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Spec.Type;
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.D_Drill":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.D_Drill * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.CBore.D":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.CBore.D * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.CBore.Depth":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.CBore.Depth * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.Depth.TapDrill":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Depth.TapDrill * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.Depth.Tap":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Depth.Tap * pConvF);
                                    break;

                                case "Bearing.Mount.Screw[1].Hole.Depth.Engagement":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Depth.Engagement * pConvF);
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            private void Write_Parameter_Complete_Seal_Front(clsProject Project_In, clsSeal Seal_In, EXCEL.Worksheet WorkSheet_In)
            //====================================================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    Double pConvF = 1;
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }

                    for (int i = 2; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {

                                case "EndPlate[0].Mat.Base":
                                    WorkSheet_In.Cells[i, 4] = Seal_In.Mat.Base;
                                    break;

                                case "EndPlate[0].Mat.Lining":
                                    WorkSheet_In.Cells[i, 4] = Seal_In.Mat.Lining;
                                    break;

                                case "EndPlate[0].LiningT":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Mat_LiningT * pConvF);
                                    break;

                                case "EndPlate[0].Design":
                                    WorkSheet_In.Cells[i, 4] = Seal_In.Design.ToString();
                                    break;

                                case "EndPlate[0].OD":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.OD * pConvF);
                                    break;

                                case "EndPlate[0].DBore":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.DBore() * pConvF);
                                    break;

                                case "EndPlate[0].L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.L * pConvF);
                                    break;

                                case "EndPlate[0].Blade.Count":
                                    WorkSheet_In.Cells[i, 4] = Seal_In.Blade.Count;
                                    break;

                                case "EndPlate[0].Blade.T":
                                    if (Seal_In.Blade.Count == 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Blade.T * pConvF);
                                    }
                                    break;

                                case "EndPlate[0].Blade.AngTaper":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Blade.AngTaper);
                                    break;

                                case "EndPlate[0].Blade_T":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Blade.T * pConvF);
                                    }                                    
                                    break;

                                case "EndPlate[0].DrainHoles.Annulus.Ratio_L_H":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.Annulus.Ratio_L_H;
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.Annulus.D":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.DrainHoles.Annulus.D * pConvF);
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.D()":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.DrainHoles.D() * pConvF);
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.Count":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.Count;
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.AngBet":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.AngBet;
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.AngStart_Horz":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.AngStart_Horz;
                                    }
                                    break;

                                case "EndPlate[0].DrainHoles.AngExit":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.AngExit;
                                    }
                                    break;

                            }
                        }
                    }
                }
                catch
                {
                }
            }

            private void Write_Parameter_Complete_Seal_Back(clsProject Project_In, clsSeal Seal_In, EXCEL.Worksheet WorkSheet_In)
            //====================================================================================================================
            {
                try
                {
                    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

                    int pRowCount = pExcelCellRange.Rows.Count;
                    string pVarName = "";
                    Double pConvF = 1;
                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
                    {
                        pConvF = 25.4;
                    }

                    for (int i = 2; i <= pRowCount; i++)
                    {
                        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
                        {
                            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

                            switch (pVarName)
                            {
                                case "EndPlate[1].Mat.Base":
                                    WorkSheet_In.Cells[i, 4] = Seal_In.Mat.Base;
                                    break;

                                case "EndPlate[1].Mat.Lining":
                                    WorkSheet_In.Cells[i, 4] = Seal_In.Mat.Lining;
                                    break;

                                case "EndPlate[1].LiningT":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Mat_LiningT * pConvF);
                                    break;

                                case "EndPlate[1].Design":
                                    WorkSheet_In.Cells[i, 4] = Seal_In.Design.ToString();
                                    break;

                                case "EndPlate[1].OD":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.OD * pConvF);
                                    break;

                                case "EndPlate[1].DBore":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.DBore() * pConvF);
                                    break;

                                case "EndPlate[1].L":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.L * pConvF);
                                    break;

                                case "EndPlate[1].Blade.Count":
                                    WorkSheet_In.Cells[i, 4] = Seal_In.Blade.Count;
                                    break;

                                case "EndPlate[1].Blade.T":
                                    if (Seal_In.Blade.Count == 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Blade.T * pConvF);
                                    }
                                    break;

                                case "EndPlate[1].Blade.AngTaper":
                                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Blade.AngTaper);
                                    break;

                                case "EndPlate[1].Blade_T":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Blade.T * pConvF);
                                    }
                                    break;
                                    
                                case "EndPlate[1].DrainHoles.Annulus.Ratio_L_H":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.Annulus.Ratio_L_H;
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.Annulus.D":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.DrainHoles.Annulus.D * pConvF);
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.D()":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.DrainHoles.D() * pConvF);
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.Count":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.Count;
                                    }
                                    break;


                                case "EndPlate[1].DrainHoles.AngBet":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.AngBet;
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.AngStart_Horz":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.AngStart_Horz;
                                    }
                                    break;

                                case "EndPlate[1].DrainHoles.AngExit":
                                    if (Seal_In.Blade.Count > 1)
                                    {
                                        WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.AngExit;
                                    }
                                    break;

                            }
                        }
                    }
                }
                catch
                {
                }
            }


            #endregion

            private void UpdateAppConfig(String DataSource_In)
            //================================================
            {
                Configuration pConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // ....First Connection String
                // ........Because it's an EF connection string it's not a normal connection string
                // ........so we pull it into the EntityConnectionStringBuilder instead
                EntityConnectionStringBuilder pEFB = new EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings["BearingDBEntities"].ConnectionString);

                // ....Then we extract the actual underlying provider connection string
                SqlConnectionStringBuilder pSQB = new SqlConnectionStringBuilder(pEFB.ProviderConnectionString);

                // ....Now we can set the datasource
                pSQB.DataSource = DataSource_In;

                // ....Pop it back into the EntityConnectionStringBuilder 
                pEFB.ProviderConnectionString = pSQB.ConnectionString;

                // ....And update
                pConfig.ConnectionStrings.ConnectionStrings["BearingDBEntities"].ConnectionString = pEFB.ConnectionString;

                pConfig.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("connectionStrings");
            }
       
            //---------------------------------------------------------------------------
            //                      UTILITY ROUTINES - END                              '
            //---------------------------------------------------------------------------
        #endregion


        #region "SESSION SAVE/RESTORE RELATED ROUTINES:"
            //-----------------------------------------

            #region "SAVE SESSION:"
            //--------------------
            public void Save_SessionData(clsProject Project_In, clsOpCond OpCond_In)
            //=================================================================================================
            {
                try
                {
                    string pFilePath = mFileName_BearingCAD.Remove(mFileName_BearingCAD.Length - 11);// mFileName_BearingCAD;

                    Boolean pProject =
                    Project_In.Serialize(pFilePath);

                    Boolean pOpCond =
                    OpCond_In.Serialize(pFilePath);
                                       

                    //....Merge two Binary files created for two different objects.
                    Merge_ObjFiles(pFilePath);

                    //....Delete two Binary files.
                    Delete_ObjFiles(pFilePath);
                }
                catch (Exception pEXP)
                {

                }
            }


            private void Merge_ObjFiles(string FilePath_In)
            //=============================================
            {
                byte[] pHeader;
                byte[] buffer;
                int count = 0;
                string pFileHeader = null;
                FileStream OpenFile = null;

                string pFileName_Out = FilePath_In + ".BearingCAD";
                FileStream OutputFile = new FileStream(pFileName_Out, FileMode.Create, FileAccess.Write);

                for (int index = 1; index <= mcObjFile_Count; index++)
                {
                    string pFileName = FilePath_In + index + ".BearingCAD";

                    OpenFile = new FileStream(pFileName, FileMode.Open, FileAccess.Read, FileShare.Read);

                    //....Initialize the buffer by the total byte length of the file.
                    buffer = new byte[OpenFile.Length];

                    //....Read the file and store it into the buffer.
                    OpenFile.Read(buffer, 0, buffer.Length);
                    count = OpenFile.Read(buffer, 0, buffer.Length);

                    //....Create a header for each file.
                    pFileHeader = "BeginFile" + index + "," + buffer.Length.ToString();

                    //....Transfer the header string into bytes.
                    pHeader = Encoding.Default.GetBytes(pFileHeader);

                    //....Write the header info. into file.
                    OutputFile.Write(pHeader, 0, pHeader.Length);

                    //....Write a Linefeed into file for seperating header info and file info.
                    OutputFile.WriteByte(10); // linefeed

                    //....Write buffer data into file.
                    OutputFile.Write(buffer, 0, buffer.Length);
                    OpenFile.Close();
                }

                OutputFile.Close();
            }

            private void Delete_ObjFiles(string FilePath_In)
            //==========================================
            {
                string pFileName = null;

                for (int index = 1; index <= mcObjFile_Count; index++)
                {
                    pFileName = FilePath_In + index + ".BearingCAD";
                    File.Delete(pFileName);
                }
            }

            #endregion


            #region "RESTORE SESSION:"
            //------------------------

            public void Restore_SessionData(ref clsProject Project_In, ref clsOpCond OpCond_In, string FilePath_In)
            //======================================================================================================
            {
                try
                {
                    Split_SessionFile();
                    Project_In = (clsProject)modMain.gProject.Deserialize(FilePath_In);
                    OpCond_In = (clsOpCond)modMain.gOpCond.Deserialize(FilePath_In);
                    Delete_ObjFiles(FilePath_In);
                }
                catch (Exception pEXP)
                {

                }
            }

            private void Split_SessionFile()
            //==============================
            {
                string line = null;
                Int32 pLength = 0;
                int pIndex = 1;

                FileStream OpenFile = null;
                OpenFile = new FileStream(mFileName_BearingCAD, FileMode.Open, FileAccess.Read, FileShare.Read);

                while (OpenFile.Position != OpenFile.Length)
                {
                    line = null;
                    while (string.IsNullOrEmpty(line) && OpenFile.Position != OpenFile.Length)
                    {
                        //....Read the header info.
                        line = ReadLine(OpenFile);
                    }

                    if (!string.IsNullOrEmpty(line) && OpenFile.Position != OpenFile.Length)
                    {
                        //....Store the total byte length of the file stored into the header.
                        pLength = GetLength(line);
                    }
                    if (!string.IsNullOrEmpty(line))
                    {
                        //....Write bin files from the marged file.
                        Write_ObjFiles(OpenFile, pLength, pIndex);
                        pIndex++;
                    }
                }
                OpenFile.Close();
            }


            private string ReadLine(FileStream fs)
            //===================================
            {
                string line = string.Empty;

                const int bufferSize = 4096;
                byte[] buffer = new byte[bufferSize];
                byte b = 0;
                byte lf = 10;
                int i = 0;

                while (b != lf)
                {
                    b = (byte)fs.ReadByte();
                    buffer[i] = b;
                    i++;
                }

                line = System.Text.Encoding.Default.GetString(buffer, 0, i - 1);

                return line;
            }


            private Int32 GetLength(string fileInfo)
            //=====================================
            {
                Int32 pLength = 0;
                if (!string.IsNullOrEmpty(fileInfo))
                {
                    //....get the file information
                    string[] info = fileInfo.Split(',');
                    if (info != null && info.Length == 2)
                    {
                        pLength = Convert.ToInt32(info[1]);
                    }
                }
                return pLength;
            }


            private void Write_ObjFiles(FileStream fs, int fileLength, int Index_In)
            //=====================================================================
            {
                FileStream fsFile = null;
                string pFilePath = "";
                if (mFileName_BearingCAD != "")
                {
                    pFilePath =  mFileName_BearingCAD.Remove(mFileName_BearingCAD.Length - 11);
                }

                try
                {
                    string pFileName_Out = pFilePath + Index_In + ".BearingCAD";

                    byte[] buffer = new byte[fileLength];
                    int count = fs.Read(buffer, 0, fileLength);
                    fsFile = new FileStream(pFileName_Out, FileMode.Create, FileAccess.Write, FileShare.None);
                    fsFile.Write(buffer, 0, buffer.Length);
                    fsFile.Write(buffer, 0, count);
                }
                catch (Exception ex1)
                {
                    // handle or display the error
                    throw ex1;
                }
                finally
                {
                    if (fsFile != null)
                    {
                        fsFile.Flush();
                        fsFile.Close();
                        fsFile = null;
                    }
                }
            }

            #endregion

            #region "UTILITY ROUTINES:"

            public void CloseWordFiles()
            //===========================     
            {
                Process[] pProcesses = Process.GetProcesses();

                try
                {
                    foreach (Process p in pProcesses)
                        if (p.ProcessName == "WINWORD")
                            p.Kill();
                }
                catch (Exception pEXP)
                {

                }
            }

                public void CloseExcelFiles()
                //===========================      
                {
                    Process[] pProcesses = Process.GetProcesses();

                    try
                    {
                        foreach (Process p in pProcesses)
                            if (p.ProcessName == "EXCEL")
                                p.Kill();
                    }
                    catch (Exception pEXP)
                    {

                    }
                }

            #endregion

            #endregion

    }
        
}




