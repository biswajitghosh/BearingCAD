﻿
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                      CLASS MODULE  :  frmCreatedataSet                       '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  31OCT18                                '
//                                                                              '
//===============================================================================
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EXCEL = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Diagnostics;
using System.IO;
//using Inventor;
//using System.Runtime.InteropServices;

namespace BearingCAD22
{
    public partial class frmCreateDataSet : Form
    {
        public frmCreateDataSet()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        //===================================================
        {
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        //======================================================
        {
            this.Close();
        }

        private void cmdBrowse_FilePath_Project_Click(object sender, EventArgs e)
        //========================================================================
        {
            folderBrowserDialog1.SelectedPath = modMain.gFiles.File_InputPath;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFilePath_Project.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void cmdCreateParameterList_Click(object sender, EventArgs e)
        //===================================================================
        {
            if (txtFilePath_Project.Text != "")
            {
                //MessageBox.Show("All open Excel files will be closed automatically.\nPlase save before proceeding.", "Warning: Excel Files!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CloseInventor();
                CloseExcelFiles();
                string pFileName = txtFilePath_Project.Text;

                Cursor = Cursors.WaitCursor;
                //CreateParameter_Driver();
                modMain.gFiles.Write_Parameter_Complete(modMain.gProject, pFileName, true);                
                Copy_Inventor_Model_Files(modMain.gProject, modMain.gFiles, txtFilePath_Project.Text);               
                Cursor = Cursors.Default;
            }
        }

        //private void CreateParameter_Complete()
        //    //================================
        //{
        //    object mobjMissing = Missing.Value;              //....Missing object.
        //    EXCEL.Application pApp = null;
        //    pApp = new EXCEL.Application();


        //    //....Open Original WorkBook.
        //    EXCEL.Workbook pWkbOrg = null;

        //    pWkbOrg = pApp.Workbooks.Open(modMain.gFiles.FileTitle_Template_EXCEL_Parameter_Complete, mobjMissing, false,
        //                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
        //                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
        //                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing);

        //    //....Open WorkSheet - 'Complete ASSY'            
        //    EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Complete Assy"];
        //    Write_Parameter_Complete_Assy(modMain.gProject, modMain.gOpCond, pWkSheet);

        //    //....Open WorkSheet - 'Radial Bearing'            
        //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Radial Bearing"];
        //    Write_Parameter_Complete_Radial(modMain.gProject, pWkSheet);

        //    //....Open WorkSheet - 'Mounting'    
        //    pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Mounting"];
        //    Writer_Parameter_Complete_Mounting(modMain.gProject, pWkSheet);

        //    //....EndPlate: Seal
        //    clsSeal[] mEndSeal = new clsSeal[2];
        //    for (int i = 0; i < 2; i++)
        //    {
        //        if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
        //        {
        //            mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
        //        }
        //    }


        //    //....Open WorkSheet - 'Front Config - Seal' 
        //    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //    {
        //        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Seal"];
        //        Write_Parameter_Complete_Seal_Front(modMain.gProject, mEndSeal[0], pWkSheet);
        //    }
        //    else
        //    {
        //        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Seal"];
        //        pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
        //    }

        //    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //    {
        //        //....Open WorkSheet - 'Back Config - Seal'    
        //        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Seal"];
        //        Write_Parameter_Complete_Seal_Back(modMain.gProject, mEndSeal[1], pWkSheet);
        //    }
        //    else
        //    {
        //        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Seal"];
        //        pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
        //    }


        //    //.............
        //    //....EndPlate: Thurst Bearing
        //    clsBearing_Thrust_TL[] mEndTB = new clsBearing_Thrust_TL[2];
        //    for (int i = 0; i < 2; i++)
        //    {
        //        if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.TL_TB)
        //        {
        //            mEndTB[i] = (clsBearing_Thrust_TL)((clsBearing_Thrust_TL)(modMain.gProject.Product.EndPlate[i])).Clone();
        //        }
        //    }

        //    //....Open WorkSheet - 'Front TL Thurst Bearing' 
        //    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB)
        //    {
        //        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Thurst Bearing TL"];
        //        Write_Parameter_Complete_Thrust_Front(modMain.gProject, mEndTB[0], pWkSheet);
        //    }
        //    else
        //    {
        //        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front - Thurst Bearing TL"];
        //        pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
        //    }

        //    //....Open WorkSheet - 'Back TL Thurst Bearing' 
        //    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
        //    {
        //        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Thurst Bearing TL"];
        //        Write_Parameter_Complete_Thrust_Back(modMain.gProject, mEndTB[1], pWkSheet);
        //    }
        //    else
        //    {
        //        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back - Thurst Bearing TL"];
        //        pWkSheet.Visible = EXCEL.XlSheetVisibility.xlSheetHidden;
        //    }

        //    //..............

        //    //pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Accessories"];
        //    //Write_Parameter_Complete_Accessories(modMain.gProject, modMain.gProject.Product.Accessories, pWkSheet);

        //    DateTime pDate = DateTime.Now;
        //    String pFileName = txtFilePath_Project.Text + "\\CAD Neutral Data Set_" + pDate.ToString("ddMMMyyyy").ToUpper() + ".xlsx";

        //    EXCEL.XlSaveAsAccessMode pAccessMode = Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive;
        //    pWkbOrg.SaveAs(pFileName, mobjMissing, mobjMissing,
        //                        mobjMissing, mobjMissing, mobjMissing, pAccessMode,
        //                        mobjMissing, mobjMissing, mobjMissing,
        //                        mobjMissing, mobjMissing);

        //    pApp.Visible = true;
        //}

        //private void CreateParameter_Driver()
        ////===================================
        //{
        //    //CloseExcelFiles();

        //    object mobjMissing = Missing.Value;              //....Missing object.
        //    EXCEL.Application pApp = null;
        //    pApp = new EXCEL.Application();

        //    //....Open Original WorkBook.
        //    EXCEL.Workbook pWkbOrg = null;

        //    pWkbOrg = pApp.Workbooks.Open(modMain.gFiles.FileTitle_Template_EXCEL_Parameter_Driver, mobjMissing, false,
        //                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
        //                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing,
        //                                  mobjMissing, mobjMissing, mobjMissing, mobjMissing);

        //    //....Open 'Sketchs' WorkSheets.
        //    EXCEL.Worksheet pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Radial Bearing"];
        //    EXCEL.Range pExcelCellRange = pWkSheet.UsedRange;
        //    Write_Parameter_Driver_Radial(modMain.gProject, pWkSheet);

        //    //....EndPlate: Seal
        //    clsSeal[] mEndSeal = new clsSeal[2];
        //    for (int i = 0; i < 2; i++)
        //    {
        //        if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
        //        {
        //            mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
        //        }
        //    }

        //    //....Open WorkSheet - 'Front Config - Seal' 
        //    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //    {
        //        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front Config - Seal"];
        //        Write_Parameter_Driver_Seal(modMain.gProject, mEndSeal[0], pWkSheet);
        //    }

        //    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //    {
        //        //....Open WorkSheet - 'Back Config - Seal'    
        //        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back Config - Seal"];
        //        Write_Parameter_Driver_Seal(modMain.gProject, mEndSeal[1], pWkSheet);
        //    }

        //    //....EndPlate: Thurst Bearing
        //    clsBearing_Thrust_TL[] mEndTB = new clsBearing_Thrust_TL[2];
        //    for (int i = 0; i < 2; i++)
        //    {
        //        if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.TL_TB)
        //        {
        //            mEndTB[i] = (clsBearing_Thrust_TL)((clsBearing_Thrust_TL)(modMain.gProject.Product.EndPlate[i])).Clone();
        //        }
        //    }

        //    //....Open WorkSheet - 'Front TL Thurst Bearing' 
        //    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB)
        //    {
        //        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Front TL Thurst Bearing"];
        //        Write_Parameter_Driver_Thrust(modMain.gProject, mEndTB[0], pWkSheet);
        //    }

        //    //....Open WorkSheet - 'Back TL Thurst Bearing' 
        //    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
        //    {
        //        pWkSheet = (EXCEL.Worksheet)pWkbOrg.Sheets["Back TL Thurst Bearing"];
        //        Write_Parameter_Driver_Thrust(modMain.gProject, mEndTB[1], pWkSheet);
        //    }

        //    DateTime pDate = DateTime.Now;
        //    String pFileName = txtFilePath_Project.Text + "\\ParameterV21_Driver_RevA.xlsx";

        //    EXCEL.XlSaveAsAccessMode pAccessMode = Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive;
        //    pWkbOrg.SaveAs(pFileName, mobjMissing, mobjMissing,
        //                        mobjMissing, mobjMissing, mobjMissing, pAccessMode,
        //                        mobjMissing, mobjMissing, mobjMissing,
        //                        mobjMissing, mobjMissing);

        //    pApp.Visible = false;
        //    pWkbOrg.Close();
        //    pApp = null;
        //}

        ////private void Write_Parameter_Complete_Radial(clsProject Project_In, EXCEL.Worksheet WorkSheet_In)
        //////================================================================================================
        ////{
        ////    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

        ////    int pRowCount = pExcelCellRange.Rows.Count;
        ////    string pVarName = "";
        ////    Double pConvF = 1;
        ////    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
        ////    {
        ////        pConvF = 25.4;
        ////    }
        ////    for (int i = 3; i <= pRowCount; i++)
        ////    {
        ////        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
        ////        {
        ////            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

        ////            switch (pVarName)
        ////            {
        ////                //....Material:
        ////                case "Bearing.Mat.Base":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Base; break;


        ////                //....Geometry:

        ////                //....Diameter:
        ////                case "Bearing.Mat.Lining":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mat.Lining;
        ////                    break;

        ////                case "Bearing.LiningT":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).LiningT * pConvF);
        ////                    break;

        ////                case "Bearing.OD()":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OD() * pConvF);
        ////                    break;


        ////                case "Bearing.PadBore()":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).PadBore() * pConvF);
        ////                    break;

        ////                case "Bearing.Bore()":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Bore() * pConvF);                           
        ////                    break;

        ////                case "Bearing.DShaft()":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft() * pConvF);                            
        ////                    break;

        ////                //....Length:
        ////                case "L_Available":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.L_Available * pConvF);                            
        ////                    break;

        ////                case "L_Tot()":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.L_Tot() * pConvF);                           
        ////                    break;

        ////                //case "Dist_ThrustFace[0/1]":
        ////                //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
        ////                //    {
        ////                //        if (Project_In.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB ||
        ////                //           Project_In.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
        ////                //        {
        ////                //            //....Dist_ThrustFace
        ////                //            if ((Project_In.Product.Dist_ThrustFace[0] > modMain.gcEPS) && (Project_In.Product.Dist_ThrustFace[1] < modMain.gcEPS))
        ////                //            {
        ////                //                WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( Project_In.Product.Dist_ThrustFace[0] * pConvF);

        ////                //            }

        ////                //            else if ((Project_In.Product.Dist_ThrustFace[0] < modMain.gcEPS) && (Project_In.Product.Dist_ThrustFace[1] > modMain.gcEPS))
        ////                //            {
        ////                //                WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( Project_In.Product.Dist_ThrustFace[1] * pConvF);

        ////                //            }

        ////                //            else if ((Project_In.Product.Dist_ThrustFace[0] > modMain.gcEPS) && (Project_In.Product.Dist_ThrustFace[1] > modMain.gcEPS))
        ////                //            {
        ////                //                WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( Project_In.Product.Dist_ThrustFace[0] * pConvF);

        ////                //            }
        ////                //        }
        ////                //    }
        ////                //    else
        ////                //    {

        ////                //        if (Project_In.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB ||
        ////                //            Project_In.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
        ////                //        {
        ////                //            //....Dist_ThrustFace
        ////                //            if ((Project_In.Product.Dist_ThrustFace[0] > modMain.gcEPS) && (Project_In.Product.Dist_ThrustFace[1] < modMain.gcEPS))
        ////                //            {
        ////                //                WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( Project_In.Product.Dist_ThrustFace[0]);

        ////                //            }

        ////                //            else if ((Project_In.Product.Dist_ThrustFace[0] < modMain.gcEPS) && (Project_In.Product.Dist_ThrustFace[1] > modMain.gcEPS))
        ////                //            {
        ////                //                WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( Project_In.Product.Dist_ThrustFace[1]);

        ////                //            }

        ////                //            else if ((Project_In.Product.Dist_ThrustFace[0] > modMain.gcEPS) && (Project_In.Product.Dist_ThrustFace[1] > modMain.gcEPS))
        ////                //            {
        ////                //                WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( Project_In.Product.Dist_ThrustFace[0]);

        ////                //            }
        ////                //        }
        ////                //    }

        ////                //    break;

        ////                //....Pad:
        ////                case "Bearing.Pad.Type":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Type.ToString();
        ////                    break;

        ////                case "Bearing.Pad.Count":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Count;
        ////                    break;

        ////                case "Bearing.Pad.L":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.L * pConvF);                            
        ////                    break;

        ////                case "Bearing.Pad.Angle":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Angle;
        ////                    break;

        ////                //....Pad Pivot:
        ////                case "Bearing.Pad.Pivot.Offset":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.Offset);
        ////                    break;

        ////                case "Bearing.Pad.Pivot.AngStart":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.Pivot.AngStart;
        ////                    break;

        ////                //....Pad Thickness:
        ////                case "Bearing.Pad.T.Lead":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Lead * pConvF);
        ////                    break;

        ////                case "Bearing.Pad.T.Pivot":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Pivot * pConvF);
        ////                    break;

        ////                case "Bearing.Pad.T.Trail":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.T.Trail * pConvF);                           
        ////                    break;

        ////                case "Bearing.Pad.Rfillet":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.RFillet * pConvF);
        ////                    break;

        ////                //....Flexure Pivot:
        ////                case "Bearing.FlexurePivot.Web.T":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.T * pConvF);
        ////                    break;

        ////                case "Bearing.FlexurePivot.Web.RFillet":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.RFillet * pConvF);                          
        ////                    break;

        ////                case "Bearing.FlexurePivot.Web.H":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.Web.H * pConvF);      
        ////                    break;

        ////                case "Bearing.FlexurePivot.GapEDM":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).FlexurePivot.GapEDM * pConvF);
        ////                    break;
                       

        ////                case "Bearing.D_PadRelief() ":                           
        ////                     WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.D_PadRelief() * pConvF);
        ////                    break;

        ////                case "Bearing.AxialSealGap[0]":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.AxialSealGap[0] * pConvF);
        ////                    break;

        ////                case "Bearing.MillRelief.Exists":
        ////                    String pVal = "";
        ////                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.Exists)
        ////                    {
        ////                        pVal = "Y";
        ////                    }
        ////                    else
        ////                    {
        ////                        pVal = "N";
        ////                    }
        ////                    WorkSheet_In.Cells[i, 4] = pVal;
        ////                    break;

        ////                case "Bearing.MillRelief.D()":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).MillRelief.D() * pConvF);
        ////                    break;

        ////                //....DESIGN DETAILS:
        ////                case "Bearing.L":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).L * pConvF);
        ////                    break;

        ////                case "Bearing.Depth_EndPlate[0]":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndPlate[0] * pConvF);
        ////                    break;

        ////                case "Bearing.Depth_EndPlate[1]":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Depth_EndPlate[1] * pConvF);
        ////                    break;
                            
        ////                case "EndPlate[0].L":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[0].L * pConvF);
        ////                    break;

        ////                case "EndPlate[1].L":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[0].L * pConvF);
        ////                    break;

        ////                //....Oil inlet:
        ////                case "Bearing.OilInlet.Count_MainOilSupply":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Count_MainOilSupply;
        ////                    break;

        ////                //....Orifice:
        ////                case "Bearing.OilInlet.Orifice.Count":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Count;
        ////                    break;

        ////                case "Bearing.OilInlet.Orifice.D":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D * pConvF);
        ////                    break;

        ////                case "Bearing.OilInlet.Orifice.StartPos":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.StartPos.ToString();
        ////                    break;

        ////                case "Bearing.OilInlet.Orifice.D_Cbore":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.D_CBore * pConvF);
        ////                    break;

        ////                case "Bearing.OilInlet.Orifice.Loc_Back":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.Loc_Back * pConvF);
        ////                    break;

        ////                case "Bearing.OilInlet.Orifice.L":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Orifice.L * pConvF);
        ////                    break;                      

        ////                //....Annulus:     

        ////                case "Bearing.OilInlet.Annulus.Exists":
        ////                    String pAnnulus_Exists = "";
        ////                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Exists)
        ////                    {
        ////                        pAnnulus_Exists = "Y";
        ////                    }
        ////                    else
        ////                    {
        ////                        pAnnulus_Exists = "N";
        ////                    }

        ////                    WorkSheet_In.Cells[i, 4] = pAnnulus_Exists;
        ////                    break;

        ////                case "Bearing.OilInlet.Annulus.Area_Reqd":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Area * pConvF);
        ////                    break;

        ////                case "Bearing.OilInlet.Annulus.Wid":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Wid * pConvF);
        ////                    break;

        ////                case "Bearing.OilInlet.Annulus.Depth":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Depth * pConvF);
        ////                    break;

        ////                case "Bearing.OilInlet.Annulus.D":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.D * pConvF);
        ////                    break;

        ////                case "Bearing.OilInlet.Annulus.Loc_Back":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Loc_Back * pConvF);
        ////                    break;

        ////                //case "Bearing.OilInlet.Annulus_V()":
        ////                //    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus_V(((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.D , ((clsBearing_Radial_FP)Project_In.Product.Bearing).OilInlet.Annulus.Wid );
        ////                //    break;

        ////                //....Flange:      

        ////                //case "Bearing.Flange.Exists":
        ////                //    String pFlange_Exists = "";
        ////                //    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Exists)
        ////                //    {
        ////                //        pFlange_Exists = "Y";
        ////                //    }
        ////                //    else
        ////                //    {
        ////                //        pFlange_Exists = "N";
        ////                //    }

        ////                //    WorkSheet_In.Cells[i, 4] = pFlange_Exists;
        ////                //    break;

        ////                //case "Bearing.Flange.D":
        ////                //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.D * pConvF);
        ////                //    break;

        ////                //case "Bearing.Flange.Wid":
        ////                //    WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.Wid * pConvF);
        ////                //    break;

        ////                //case "Bearing.Flange.DimStart_Front":
        ////                //    WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)Project_In.Product.Bearing).Flange.DimStart_Back * pConvF);
        ////                //    break;

        ////                //....Anti-Rotation Pin:      

        ////                //....Hardware:  
        ////                case "Bearing.ARP.Spec.Unit.System":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.Spec.Unit.System.ToString();
        ////                    break;

        ////                case "Bearing.ARP.Spec.Type":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.Spec.Type;
        ////                    break;

        ////                case "Bearing.ARP.Spec.Mat":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.Spec.Mat;
        ////                    break;

        ////                case "Bearing.ARP.Spec.D":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.D());
        ////                    break;

        ////                case "Bearing.ARP.Spec.L":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.Spec.L);
        ////                    break;

        ////                case "Bearing.ARP.PN":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.PN;
        ////                    break;

        ////                case "Bearing.ARP.Hole.Depth":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Dowel.Hole.Depth * pConvF);
        ////                    break;

        ////                case "Bearing.ARP.Stickout":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Stickout * pConvF);
        ////                    break;

        ////                case "Bearing.ARP.Loc.Dist_Back":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Loc.Dist_Back * pConvF);
        ////                    break;

        ////                case "Bearing.ARP.Loc.Angle_Casing_SL":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Loc.Angle_Casing_SL;
        ////                    break;

        ////                case "Bearing.ARP.Loc.Offset":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Loc.Offset * pConvF);
        ////                    break;


        ////                case "Bearing.ARP.Loc.Offset_Direction":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Loc.Offset_Direction.ToString();
        ////                    break;

        ////                case "Bearing.ARP.Angle_Horz":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).ARP.Angle_Horz;
        ////                    break;
                   

        ////                //....S/L Hardware:      

        ////                //....Screw:  

        ////                case "Bearing.SL.Screw.Spec.Unit.System":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Spec.Unit.System.ToString();
        ////                    break;

        ////                case "Bearing.SL.Screw.Spec.Type":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Spec.Type;
        ////                    break;

        ////                case "Bearing.SL.Screw.Spec.Mat":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Spec.Mat;
        ////                    break;


        ////                case "Bearing.SL.Screw.Spec.D":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.D());
        ////                    break;

        ////                case "Bearing.SL.Screw.Spec.Pitch":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Spec.Pitch);
        ////                    break;

        ////                case "Bearing.SL.Screw.Spec.L":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Spec.L);
        ////                    break;

        ////                case "Bearing.SL.Screw.PN":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.PN;
        ////                    break;

        ////                case "Bearing.SL.Screw.Hole.CBore.D":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Hole.CBore.D);
        ////                    break;

        ////                case "Bearing.SL.Screw.Hole.CBore.D_Drill":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Hole.CBore.D_Drill);
        ////                    break;

        ////                case "Bearing.SL.Screw.Hole.CBore.Depth":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Hole.CBore.Depth);
        ////                    break;

        ////                case "Bearing.SL.Screw.Hole.Depth.TapDrill":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Hole.Depth.TapDrill);
        ////                    break;

        ////                case "Bearing.SL.Screw.Hole.Depth.Tap":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Hole.Depth.Tap);
        ////                    break;

        ////                case "Bearing.SL.Screw.Hole.Depth.Engagement":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Screw.Hole.Depth.Engagement);
        ////                    break;
                     

        ////                //....Left Location:     

        ////                case "Bearing.SL.LScrew.Loc_Center":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Center * pConvF);
        ////                    break;

        ////                case "Bearing.SL.LScrew.Loc_Back":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LScrew_Loc.Back * pConvF);
        ////                    break;

        ////                //....Right Location:     

        ////                case "Bearing.SL.RScrew.Loc_Center":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Center * pConvF);
        ////                    break;

        ////                case "Bearing.SL.RScrew.Loc_Back":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RScrew_Loc.Back * pConvF);
        ////                    break;

        ////                //....Dowel:      

        ////                case "Bearing.SL.Dowel.Spec.Type":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel.Spec.Type;
        ////                    break;

        ////                case "Bearing.SL.Dowel.Spec.Mat":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel.Spec.Mat;
        ////                    break;

        ////                case "Bearing.SL.Dowel.Spec.D":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel.D());
        ////                    break;

        ////                case "Bearing.SL.Dowel.Spec.L":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel.Spec.L);
        ////                    break;

        ////                case "Bearing.SL.Dowel.PN":
        ////                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_PN;
        ////                    break;

        ////                case "Bearing.SL.Dowel.HoleDepth":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.Dowel_Depth * pConvF);
        ////                    break;

        ////                //....Left Location:     

        ////                case "Bearing.SL.LDowel.Loc_Center":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Center * pConvF);
        ////                    break;

        ////                case "Bearing.SL.LDowel.Loc_Back":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.LDowel_Loc.Back * pConvF);
        ////                    break;

        ////                //....Right Location:      

        ////                case "Bearing.SL.RDowel.Loc_Center":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Center * pConvF);
        ////                    break;

        ////                case "Bearing.SL.RDowel.Loc_Back":
        ////                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).SL.RDowel_Loc.Back * pConvF);
        ////                    break;
        ////            }
        ////        }
        ////    }

        ////}

        //private void Writer_Parameter_Complete_Mounting(clsProject Project_In, EXCEL.Worksheet WorkSheet_In)
        ////=============================================================================================
        //{
        //    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

        //    int pRowCount = pExcelCellRange.Rows.Count;
        //    string pVarName = "";
        //    Double pConvF = 1;
        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
        //    {
        //        pConvF = 25.4;
        //    }
        //    for (int i = 2; i <= pRowCount; i++)
        //    {
        //        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
        //        {
        //            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

        //            switch (pVarName)
        //            {
        //                //case "Bearing.Mount.Holes_GoThru":
        //                //    String pHoles_GoThru = "";
        //                //    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_GoThru)
        //                //    {
        //                //        pHoles_GoThru = "Y";
        //                //    }
        //                //    else
        //                //    {
        //                //        pHoles_GoThru = "N";
        //                //    }
        //                //    WorkSheet_In.Cells[i, 4] = pHoles_GoThru;
        //                //    break;

        //                case "Bearing.Mount.Bolting":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Bolting.ToString();
        //                    break;

        //                case "Bearing.TWall_EndPlateCB(0)":

        //                    break;

        //                //....Front End Config:
        //                //case "Bearing_In.TWall_BearingCB(0)":
        //                //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).TWall_BearingCB(0)*pConvF);
        //                //    break;

        //                case "Bearing.Mount.Screw[0].Hole.DBC":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DFit_EndConfig(0) * pConvF);
        //                    break;

        //                case "Bearing.Mount.Screw[0].Spec.Type":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].D_Finish * pConvF);
        //                    break;

        //                case "Bearing.Mount.Screw[0].Spec.Mat":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].DBC * pConvF);
        //                    break;

        //                case "Bearing.Mount.Screw[0].Spec.D":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count;
        //                    break;

        //                case "Bearing.Mount.Screw[0].Spec.Pitch":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngStart;
        //                    break;

        //                case "Bearing.Mount.Screw[0].Spec.L":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngStart;
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.Count":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count;
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.AngStart":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngStart;
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.AngBet[0]":
        //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[0];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[0];
        //                    }
        //                    else
        //                    {
        //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 1)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.AngBet[1]":
        //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[1];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[1];
        //                    }
        //                    else
        //                    {
        //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 2)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.AngBet[2]":
        //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[2];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[2];
        //                    }
        //                    else
        //                    {
        //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 3)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.AngBet[3]":
        //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[3];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[3];
        //                    }
        //                    else
        //                    {
        //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 4)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.AngBet[4]":
        //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[4];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[4];
        //                    }
        //                    else
        //                    {
        //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 5)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.AngBet[5]":
        //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[5];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[5];
        //                    }
        //                    else
        //                    {
        //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 6)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.AngBet[6]":
        //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].HolesAngOther[6];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[6];
        //                    }
        //                    else
        //                    {
        //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 7)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                //....Thread:
        //                case "Bearing.Mount.Screw[0].Hole.Type":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Screw_Spec.Type;
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.CBore.D_Drill":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Screw_Spec.Spec_Mat;
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.CBore.D":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Screw_Spec.Unit.System.ToString();
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.CBore.Depth":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Screw_Spec.Spec_D);
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.Depth.TapDrill":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Screw_Spec.Spec_Pitch);
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.Depth.Tap":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Screw_Spec.Spec_L);
        //                    break;

        //                case "Bearing.Mount.Screw[0].Hole.Depth.Engagement":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[0] * pConvF);
        //                    break;

        //                case "EndPlate[1].OD":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Screw_Spec.D_Thru);
        //                    break;

        //                case "Bearing.TWall_EndPlateCB(1)":
        //                    WorkSheet_In.Cells[i, 4] = Project_In.Product.EndPlate[0].MountHoles.Type.ToString();
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.DBC":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[0].MountHoles.Screw.D_Thru);
        //                    break;

        //                case "Bearing.Mount.Screw[1].Spec.Type":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[0].MountHoles.Screw.D_CBore * pConvF) + ", " +
        //                                               modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[0].MountHoles.Depth_CBore * pConvF);
        //                    break;

        //                //case "EndSeal[0].MountHoles.Thread_Thru":
        //                //    String pThread_Thru = "";
        //                //    if (Project_In.Product.EndPlate[0].MountHoles.Thread_Thru)
        //                //    {
        //                //        pThread_Thru = "Y";
        //                //    }
        //                //    else
        //                //    {
        //                //        pThread_Thru = "N";
        //                //    }
        //                //    WorkSheet_In.Cells[i, 4] = pThread_Thru;
        //                //    break;

        //                case "Bearing.Mount.Screw[1].Spec.Mat":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[0].MountHoles.Depth_Thread * pConvF);
        //                    break;

        //                //....Back End Config                          
        //                case "Bearing.Mount.Screw[1].Spec.D":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).TWall_BearingCB(1) * pConvF);
        //                    break;

        //                case "Bearing.Mount.Screw[1].Spec.Pitch":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.DFit_EndConfig(1) * pConvF);
        //                    break;

        //                case "Bearing.Mount.Screw[1].Spec.L":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].D_Finish * pConvF);
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.Count":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].DBC * pConvF);
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.AngStart":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count;
        //                    break;

        //                case "Bearing.Mount.Screw[1].HolesAngStart":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].HolesAngStart;
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.AngBet[0]":
        //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].HolesAngOther[0];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[0];
        //                    }
        //                    else
        //                    {
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 1)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.AngBet[1]":
        //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].HolesAngOther[1];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[1];
        //                    }
        //                    else
        //                    {
        //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 2)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.AngBet[2]":
        //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].HolesAngOther[2];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[2];
        //                    }
        //                    else
        //                    {
        //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 3)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.AngBet[3]":
        //                    // WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].HolesAngOther[3];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[3];
        //                    }
        //                    else
        //                    {
        //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 4)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.AngBet[4]":
        //                    // WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].HolesAngOther[4];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[4];
        //                    }
        //                    else
        //                    {
        //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 5)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.AngBet[5]":
        //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].HolesAngOther[5];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[5];
        //                    }
        //                    else
        //                    {
        //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 6)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.AngBet[6]":
        //                    // WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].HolesAngOther[6];
        //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[6];
        //                    }
        //                    else
        //                    {
        //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
        //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 7)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
        //                        }
        //                        else
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = 0;
        //                        }
        //                    }
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.Type":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Screw_Spec.Type;
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.CBore.D_Drill":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Screw_Spec.Spec_Mat;
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.CBore.D":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Screw_Spec.Unit.System.ToString();
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.CBore.Depth":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Screw_Spec.Spec_D);
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.Depth.TapDrill":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Screw_Spec.Spec_Pitch);
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.Depth.Tap":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Screw_Spec.Spec_L);
        //                    break;

        //                case "Bearing.Mount.Screw[1].Hole.Depth.Engagement":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Holes_Thread_Depth[1] * pConvF);
        //                    break;

        //                //case "Bearing.Mount.Screw[1].Screw_Spec.D_Thru":
        //                //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Screw_Spec.D_Thru);
        //                //    break;

        //                //case "EndPlate[1].MountHoles.Type":
        //                //    WorkSheet_In.Cells[i, 4] = Project_In.Product.EndPlate[1].MountHoles.Type.ToString();
        //                //    break;

        //                //case "EndSeal[1].MountHoles.Screw_Spec.D_Thru":
        //                //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[1].MountHoles.Screw_Spec.D_Thru);
        //                //    break;

        //                //case "EndSeal[1].MountHoles.Screw_Spec.D_Cbore, EndSeal[0].MountHoles.Depth_Cbore":
        //                //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[1].MountHoles.Screw_Spec.D_CBore * pConvF) + ", " + 
        //                //                               modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[0].MountHoles.Depth_CBore * pConvF);
        //                //    break;

        //                //case "EndSeal[1].MountHoles.Thread_Thru":
        //                //    String pThread_Thru_Back = "";
        //                //    if (Project_In.Product.EndPlate[1].MountHoles.Thread_Thru)
        //                //    {
        //                //        pThread_Thru_Back = "Y";
        //                //    }
        //                //    else
        //                //    {
        //                //        pThread_Thru_Back = "N";
        //                //    }
        //                //    WorkSheet_In.Cells[i, 4] = pThread_Thru_Back;
        //                //    break;

        //                //case "EndSeal[1].MountHoles.Depth_Thread":
        //                //    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Project_In.Product.EndPlate[1].MountHoles.Depth_Thread * pConvF);
        //                //    break;
        //            }
        //        }
        //    }
        //}

        //private void Write_Parameter_Complete_Seal_Front(clsProject Project_In, clsSeal Seal_In, EXCEL.Worksheet WorkSheet_In)
        ////====================================================================================================================
        //{
        //    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

        //    int pRowCount = pExcelCellRange.Rows.Count;
        //    string pVarName = "";
        //    Double pConvF = 1;
        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
        //    {
        //        pConvF = 25.4;
        //    }

        //    for (int i = 2; i <= pRowCount; i++)
        //    {
        //        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
        //        {
        //            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

        //            switch (pVarName)
        //            {

        //                case "EndPlate[0].Mat.Base":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.Mat.Base;
        //                    break;

        //                case "EndPlate[0].Mat.Lining":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.Mat.Lining ;
        //                    break;

        //                case "EndPlate[0].LiningT":
        //                    WorkSheet_In.Cells[i, 4] =  modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Mat_LiningT * pConvF);
        //                    break;

        //                case "EndPlate[0].Design":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.Design.ToString();
        //                    break;

        //                case "EndPlate[0].OD":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.OD * pConvF);
        //                    break;

        //                case "EndPlate[0].DBore":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.DBore() * pConvF);
        //                    break;

        //                case "EndPlate[0].L":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.L * pConvF);
        //                    break;

        //                case "EndPlate[0].Blade.Count":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.Blade.Count;
        //                    break;

        //                case "EndPlate[0].Blade.T":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Blade.T * pConvF);
        //                    break;

        //                case "EndPlate[0].Blade.AngTaper":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Blade.AngTaper * pConvF);
        //                    break;

        //                case "EndPlate[0].Blade.T_Count2":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Blade.T * pConvF);
        //                    break;

        //                case "EndPlate[0].DrainHoles.Annulus_Ratio_L_H":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.Annulus.Ratio_L_H;
        //                    break;

        //                case "EndPlate[0].DrainHoles.Annulus.D":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.DrainHoles.Annulus.D * pConvF);
        //                    break;

        //                case "EndPlate[0].DrainHoles.AngBet":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.AngBet;
        //                    break;

        //                case "EndPlate[0].DrainHoles.AngStart_Horz":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.AngStart_Horz;
        //                    break;

        //                case "EndPlate[0].DrainHoles.AngExit":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.AngExit;
        //                    break;

        //            }
        //        }
        //    }
        //}

        //private void Write_Parameter_Complete_Seal_Back(clsProject Project_In, clsSeal Seal_In, EXCEL.Worksheet WorkSheet_In)
        ////====================================================================================================================
        //{
        //    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

        //    int pRowCount = pExcelCellRange.Rows.Count;
        //    string pVarName = "";
        //    Double pConvF = 1;
        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
        //    {
        //        pConvF = 25.4;
        //    }

        //    for (int i = 2; i <= pRowCount; i++)
        //    {
        //        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
        //        {
        //            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

        //            switch (pVarName)
        //            {
        //                case "EndPlate[1].Mat.Base":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.Mat.Base;
        //                    break;

        //                case "EndPlate[1].Mat.Lining":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.Mat.Lining;
        //                    break;

        //                case "EndPlate[1].LiningT":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Mat_LiningT * pConvF);
        //                    break;

        //                case "EndPlate[1].Design":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.Design.ToString();
        //                    break;

        //                case "EndPlate[1].OD":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.OD * pConvF);
        //                    break;

        //                case "EndPlate[1].DBore":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.DBore() * pConvF);
        //                    break;

        //                case "EndPlate[1].L":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.L * pConvF);
        //                    break;

        //                case "EndPlate[1].Blade.Count":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.Blade.Count;
        //                    break;

        //                case "EndPlate[1].Blade.T":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Blade.T * pConvF);
        //                    break;

        //                case "EndPlate[1].Blade.AngTaper":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Blade.AngTaper * pConvF);
        //                    break;

        //                case "EndPlate[1].Blade.T_Count2":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.Blade.T * pConvF);
        //                    break;


        //                case "EndPlate[1].DrainHoles.Annulus_Ratio_L_H":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.Annulus.Ratio_L_H;
        //                    break;

        //                case "EndPlate[1].DrainHoles.Annulus.D":
        //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(Seal_In.DrainHoles.Annulus.D * pConvF);
        //                    break;


        //                case "EndPlate[1].DrainHoles.AngBet":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.AngBet;
        //                    break;

        //                case "EndPlate[1].DrainHoles.AngStart_Horz":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.AngStart_Horz;
        //                    break;

        //                case "EndPlate[1].DrainHoles.AngExit":
        //                    WorkSheet_In.Cells[i, 4] = Seal_In.DrainHoles.AngExit;
        //                    break;

        //            }
        //        }
        //    }
        //}

        private void Write_Parameter_Complete_Thrust_Front(clsProject Project_In, clsBearing_Thrust_TL ThrustTL_In, EXCEL.Worksheet WorkSheet_In)
        //========================================================================================================================================
        {
            //EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

            //int pRowCount = pExcelCellRange.Rows.Count;
            //string pVarName = "";
            //Double pConvF = 1;
            //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
            //{
            //    pConvF = 25.4;
            //}

            //for (int i = 3; i <= pRowCount; i++)
            //{
            //    if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
            //    {
            //        pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

            //        switch (pVarName)
            //        {
            //            case "EndPlate[0].Mat.Base":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.Mat.Base;
            //                break;

            //            case "EndPlate[0].Mat.Lining":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.Mat.Lining;
            //                break;

            //            case "EndPlate[0].LiningT.Face":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.LiningT.Face * pConvF);
            //                break;

            //            case "EndPlate[0].LiningT.ID":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.LiningT.ID * pConvF);
            //                break;

            //            case "EndPlate[0].DirectionType":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.DirectionType.ToString();
            //                break;

            //            case "EndPlate[0].DBore()":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.DBore() * pConvF);
            //                break;

            //            case "EndPlate[0].LandL":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.LAND_L * pConvF);
            //                break;

            //            case "EndPlate[0].L":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.L * pConvF);
            //                break;

            //            case "EndPlate[0].LFlange":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.LFlange * pConvF);
            //                break;

            //            case "EndPlate[0].DimStart()":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.DimStart() * pConvF);
            //                break;

            //            case "EndPlate[0].FaceOff_Assy":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FaceOff_Assy * pConvF);
            //                break;

            //            case "EndPlate[0].BackRelief.Reqd":
            //                String pReqd = "N";
            //                if (ThrustTL_In.BackRelief.Reqd)
            //                {
            //                    pReqd = "Y";
            //                }

            //                WorkSheet_In.Cells[i, 4] = pReqd;
            //                break;

            //            case "EndPlate[0].BackRelief.D":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.BackRelief.D * pConvF);
            //                break;

            //            case "EndPlate[0].BackRelief.Depth":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.BackRelief.Depth * pConvF);
            //                break;

            //            case "EndPlate[0].BackRelief.Fillet":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.BackRelief.Fillet * pConvF);
            //                break;

            //            case "EndPlate[0].Pad_Count":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.Pad_Count;
            //                break;

            //            case "EndPlate[0].PadD[1]":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.PadD[1] * pConvF);
            //                break;

            //            case "EndPlate[0].PadD[0]":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.PadD[0] * pConvF);
            //                break;

            //            case "EndPlate[0].Taper.Depth_OD":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.Taper.Depth_OD * pConvF);
            //                break;

            //            case "EndPlate[0].Taper.Depth_ID":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.Taper.Depth_ID * pConvF);
            //                break;

            //            case "EndPlate[0].Taper.Angle":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.Taper.Angle;
            //                break;

            //            case "EndPlate[0].FeedGroove.Type":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.FeedGroove.Type;
            //                break;

            //            case "EndPlate[0].FeedGroove.Wid":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FeedGroove.Wid * pConvF);
            //                break;

            //            case "EndPlate[0].FeedGroove.Depth":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FeedGroove.Depth * pConvF);
            //                break;

            //            case "EndPlate[0].FeedGroove.DBC":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FeedGroove.DBC * pConvF);
            //                break;

            //            case "EndPlate[0].FeedGroove.Dist_Chamf":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FeedGroove.Dist_Chamf * pConvF);
            //                break;

            //            case "EndPlate[0].WeepSlot.Type":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.WeepSlot.Type;
            //                break;

            //            case "EndPlate[0].WeepSlot.Wid":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.WeepSlot.Wid * pConvF);
            //                break;

            //            case "EndPlate[0].WeepSlot.Depth":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.WeepSlot.Depth * pConvF);
            //                break;

            //            case "EndPlate[0].Shroud.Ro":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.Shroud.Ro * pConvF);
            //                break;

            //            case "EndPlate[0].Shroud.Ri":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.Shroud.Ri * pConvF);
            //                break;

            //        }
            //    }
            //}
        }

        private void Write_Parameter_Complete_Thrust_Back(clsProject Project_In, clsBearing_Thrust_TL ThrustTL_In, EXCEL.Worksheet WorkSheet_In)
        //========================================================================================================================================
        {
            //EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

            //int pRowCount = pExcelCellRange.Rows.Count;
            //string pVarName = "";
            //Double pConvF = 1;
            //if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
            //{
            //    pConvF = 25.4;
            //}

            //for (int i = 3; i <= pRowCount; i++)
            //{
            //    if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
            //    {
            //        pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

            //        switch (pVarName)
            //        {
            //            case "EndPlate[1].Mat.Base":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.Mat.Base;
            //                break;

            //            case "EndPlate[1].Mat.Lining":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.Mat.Lining;
            //                break;

            //            case "EndPlate[1].LiningT.Face":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.LiningT.Face * pConvF;
            //                break;

            //            case "EndPlate[1].LiningT.ID":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.LiningT.ID * pConvF);
            //                break;

            //            case "EndPlate[1].DirectionType":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.DirectionType.ToString();
            //                break;

            //            case "EndPlate[1].DBore()":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.DBore() * pConvF);
            //                break;

            //            case "EndPlate[1].LandL":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.LAND_L * pConvF);
            //                break;

            //            case "EndPlate[1].L":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.L * pConvF);
            //                break;

            //            case "EndPlate[1].LFlange":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.LFlange * pConvF);
            //                break;

            //            case "EndPlate[1].DimStart()":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.DimStart() * pConvF);
            //                break;

            //            case "EndPlate[1].FaceOff_Assy":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FaceOff_Assy * pConvF);
            //                break;

            //            case "EndPlate[1].BackRelief.Reqd":
            //                String pReqd = "N";
            //                if (ThrustTL_In.BackRelief.Reqd)
            //                {
            //                    pReqd = "Y";
            //                }

            //                WorkSheet_In.Cells[i, 4] = pReqd;
            //                break;

            //            case "EndPlate[1].BackRelief.D":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.BackRelief.D * pConvF);
            //                break;

            //            case "EndPlate[1].BackRelief.Depth":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.BackRelief.Depth * pConvF);
            //                break;

            //            case "EndPlate[1].BackRelief.Fillet":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.BackRelief.Fillet * pConvF);
            //                break;

            //            case "EndPlate[1].Pad_Count":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.Pad_Count;
            //                break;

            //            case "EndPlate[1].PadD[1]":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.PadD[1] * pConvF);
            //                break;

            //            case "EndPlate[1].PadD[0]":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.PadD[0] * pConvF);
            //                break;

            //            case "EndPlate[1].Taper.Depth_OD":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.Taper.Depth_OD * pConvF);
            //                break;

            //            case "EndPlate[1].Taper.Depth_ID":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.Taper.Depth_ID * pConvF);
            //                break;

            //            case "EndPlate[1].Taper.Angle":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.Taper.Angle;
            //                break;

            //            case "EndPlate[1].FeedGroove.Type":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.FeedGroove.Type;
            //                break;

            //            case "EndPlate[1].FeedGroove.Wid":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FeedGroove.Wid * pConvF);
            //                break;

            //            case "EndPlate[1].FeedGroove.Depth":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FeedGroove.Depth * pConvF);
            //                break;

            //            case "EndPlate[1].FeedGroove.DBC":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FeedGroove.DBC * pConvF);
            //                break;

            //            case "EndPlate[1].FeedGroove.Dist_Chamf":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.FeedGroove.Dist_Chamf * pConvF);
            //                break;

            //            case "EndPlate[1].WeepSlot.Type":
            //                WorkSheet_In.Cells[i, 4] = ThrustTL_In.WeepSlot.Type;
            //                break;

            //            case "EndPlate[1].WeepSlot.Wid":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.WeepSlot.Wid * pConvF);
            //                break;

            //            case "EndPlate[1].WeepSlot.Depth":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.WeepSlot.Depth * pConvF);
            //                break;

            //            case "EndPlate[1].Shroud.Ro":
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(ThrustTL_In.Shroud.Ro * pConvF);
            //                break;

            //            case "EndPlate[1].Shroud.Ri":
            //                WorkSheet_In.Cells[i, 4] = (ThrustTL_In.Shroud.Ri * pConvF);
            //                break;

            //        }
            //    }
            //}
        }


        //private void Write_Parameter_Complete_Accessories(clsProject Project_In, clsAccessories Accessories_In,
        //                                                  EXCEL.Worksheet WorkSheet_In)
        ////======================================================================================================
        //{
        //    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

        //    int pRowCount = pExcelCellRange.Rows.Count;
        //    string pVarName = "";
        //    Double pConvF = 1;
        //    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
        //    {
        //        pConvF = 25.4;
        //    }

        //    //....EndPlate: Seal
        //    clsSeal[] mEndSeal = new clsSeal[2];
        //    for (int i = 0; i < 2; i++)
        //    {
        //        if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
        //        {
        //            mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
        //        }
        //    }

        //    for (int i = 3; i <= pRowCount; i++)
        //    {
        //        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
        //        {
        //            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

        //            switch (pVarName)
        //            {
        //                case "Bearing.TempSensor.Exists":
        //                    String pTemp_Exists = "";
        //                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Exists)
        //                    {
        //                        pTemp_Exists = "Y";
        //                    }
        //                    else
        //                    {
        //                        pTemp_Exists = "N";
        //                    }
        //                    WorkSheet_In.Cells[i, 4] = pTemp_Exists;
        //                    break;

        //                case "Bearing.TempSensor.Count":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Count;
        //                    break;

        //                case "Accessories.TempSensor.Name":
        //                    WorkSheet_In.Cells[i, 4] = Accessories_In.TempSensor.Name.ToString();
        //                    break;

        //                case "Accessories.TempSensor.Type":
        //                    WorkSheet_In.Cells[i, 4] = Accessories_In.TempSensor.Type.ToString();
        //                    break;

        //                case "Bearing.TempSensor.D":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.D * pConvF;
        //                    break;

        //                case "Bearing.TempSensor.CanLength":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.CanLength * pConvF;
        //                    break;

        //                case "Bearing.TempSensor.Loc":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Loc.ToString();
        //                    break;

        //                case "Bearing.TempSensor.Depth":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.Depth * pConvF;
        //                    break;

        //                case "Bearing.TempSensor.AngStart":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).TempSensor.AngStart;
        //                    break;

        //                case "Bearing.Pad.AngBetween()":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Pad.AngBetween();
        //                    break;

        //                case "EndSeal[0].TempSensor_D_ExitHole":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].TempSensor_D_ExitHole * pConvF;
        //                    }

        //                    break;

        //                case "EndSeal[0].TempSensor_DBC_Hole()":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].TempSensor_DBC_Hole() * pConvF;
        //                    }
        //                    break;

        //                case "EndSeal[1].TempSensor_D_ExitHole":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].TempSensor_D_ExitHole * pConvF;
        //                    }
        //                    break;

        //                case "EndSeal[1].TempSensor_DBC_Hole()":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].TempSensor_DBC_Hole() * pConvF;
        //                    }
        //                    break;

        //                case "Accessories.WireClip.Supplied":
        //                    String pSupplied = "";
        //                    if (Accessories_In.WireClip.Supplied)
        //                    {
        //                        pSupplied = "Y";
        //                    }
        //                    else
        //                    {
        //                        pSupplied = "N";
        //                    }
        //                    WorkSheet_In.Cells[i, 4] = pSupplied;
        //                    break;

        //                case "Accessories.WireClip.Count":
        //                    WorkSheet_In.Cells[i, 4] = Accessories_In.WireClip.Count;
        //                    break;

        //                case "Accessories.WireClip.Size":
        //                    WorkSheet_In.Cells[i, 4] = Accessories_In.WireClip.Size.ToString();
        //                    break;

        //                //....Front
        //                case "EndSeal[0].WireClipHoles.DBC":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.DBC * pConvF;
        //                    }
        //                    break;

        //                case "EndSeal[0].Unit.System":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].Unit.System.ToString();
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.Screw_Spec.D_Desig":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.Screw_Spec.D_Desig;
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.Screw_Spec.Pitch":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.Screw_Spec.Pitch;
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.ThreadDepth":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.ThreadDepth * pConvF;
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.AngStart":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.AngStart;
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.AngOther(0)":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        if (mEndSeal[0].WireClipHoles.AngOther.Length > 0)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.AngOther[0];
        //                        }
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.AngOther(1)":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        if (mEndSeal[0].WireClipHoles.AngOther.Length > 1)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.AngOther[1];
        //                        }
        //                    }
        //                    break;

        //                case "EndPlate[0].WireClipHoles.AngOther(2)":
        //                    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        if (mEndSeal[0].WireClipHoles.AngOther.Length > 2)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = mEndSeal[0].WireClipHoles.AngOther[2];
        //                        }
        //                    }
        //                    break;

        //                //....Back
        //                case "EndSeal[1].WireClipHoles.DBC":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.DBC * pConvF;
        //                    }
        //                    break;

        //                case "EndSeal[1].Unit.System":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].Unit.System.ToString();
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.Screw_Spec.D_Desig":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.Screw_Spec.D_Desig;
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.Screw_Spec.Pitch":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.Screw_Spec.Pitch;
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.ThreadDepth":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.ThreadDepth * pConvF;
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.AngStart":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.AngStart;
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.AngOther(0)":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        if (mEndSeal[1].WireClipHoles.AngOther.Length > 0)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.AngOther[0];
        //                        }
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.AngOther(1)":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        if (mEndSeal[1].WireClipHoles.AngOther.Length > 1)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.AngOther[1];
        //                        }
        //                    }
        //                    break;

        //                case "EndPlate[1].WireClipHoles.AngOther(2)":
        //                    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
        //                    {
        //                        if (mEndSeal[1].WireClipHoles.AngOther.Length > 2)
        //                        {
        //                            WorkSheet_In.Cells[i, 4] = mEndSeal[1].WireClipHoles.AngOther[2];
        //                        }
        //                    }
        //                    break;

        //            }
        //        }
        //    }
        //}

        //private void Write_Parameter_Complete_Assy(clsProject Project_In, clsOpCond OpCond_In,
        //                                                  EXCEL.Worksheet WorkSheet_In)
        ////=======================================================================================================
        //{
        //    EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

        //    int pRowCount = pExcelCellRange.Rows.Count;
        //    string pVarName = "";
        //    Double pConv_InchToMM = 25.4;

        //    ////....EndPlate: Seal
        //    //clsSeal[] mEndSeal = new clsSeal[2];
        //    //for (int i = 0; i < 2; i++)
        //    //{
        //    //    if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
        //    //    {
        //    //        mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
        //    //    }
        //    //}

        //    for (int i = 3; i <= pRowCount; i++)
        //    {
        //        if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
        //        {
        //            pVarName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 2]).Value2);

        //            switch (pVarName)
        //            {
        //                case "SalesOrder.Customer.Name":
        //                    WorkSheet_In.Cells[i, 4] = Project_In.SalesOrder.Customer.Name;
        //                    break;

        //                case "SalesOrder.Customer.OrderNo":
        //                    WorkSheet_In.Cells[i, 4] = Project_In.SalesOrder.Customer.OrderNo;
        //                    break;

        //                case "SalesOrder.Customer.MachineName":
        //                    WorkSheet_In.Cells[i, 4] = Project_In.SalesOrder.Customer.MachineName;
        //                    break;

        //                case "SalesOrder.No":
        //                    WorkSheet_In.Cells[i, 4] = Project_In.SalesOrder.No;
        //                    break;

        //                case "SalesOrder.RelatedNo":
        //                    WorkSheet_In.Cells[i, 4] = Project_In.SalesOrder.RelatedNo;
        //                    break;

        //                case "PNR.No":
        //                    WorkSheet_In.Cells[i, 4] = Project_In.PNR.No;
        //                    break;

        //                case "Bearing.Design":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Design.ToString();
        //                    break;

        //                case "Bearing.SplitConfig":
        //                    String pSplitConfig = "";
        //                    if (((clsBearing_Radial_FP)Project_In.Product.Bearing).SplitConfig)
        //                    {
        //                        pSplitConfig = "Y";
        //                    }
        //                    else
        //                    {
        //                        pSplitConfig = "N";
        //                    }
        //                    WorkSheet_In.Cells[i, 4] = pSplitConfig;
        //                    break;

        //                case "Bearing.DShaft_Range[0], Bearing.DShaft_Range[1]":
        //                    if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.Metric)
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0] * pConv_InchToMM) + ", " +
        //                                                  modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1] * pConv_InchToMM);
        //                    }
        //                    else
        //                    {
        //                        WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[0]) + ", " +
        //                                                    modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)Project_In.Product.Bearing).DShaft_Range[1]);
        //                    }
        //                    break;

        //                case "EndPlate[0].Type":
        //                    WorkSheet_In.Cells[i, 4] = Project_In.Product.EndPlate[0].Type.ToString().Replace("_", " ");
        //                    break;

        //                case "EndPlate[1].Type":
        //                    WorkSheet_In.Cells[i, 4] = Project_In.Product.EndPlate[1].Type.ToString().Replace("_", " ");
        //                    break;

        //                case "OpCond.Speed":
        //                    WorkSheet_In.Cells[i, 4] = OpCond_In.Speed;
        //                    break;

        //                case "OpCond.Rot_Directionality":
        //                    WorkSheet_In.Cells[i, 4] = "";
        //                    break;

        //                case "OpCond.Radial_Load":
        //                    WorkSheet_In.Cells[i, 4] = OpCond_In.Radial_Load;
        //                    break;

        //                case "OpCond.Radial_LoadAng_Casing_SL":
        //                    WorkSheet_In.Cells[i, 4] = OpCond_In.Radial_LoadAng_Casing_SL;
        //                    break;

        //                //case "OpCond.Thrust_Load_Range[0]":
        //                //    if (modMain.gProject.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB)
        //                //    {
        //                //        WorkSheet_In.Cells[i, 4] = OpCond_In.Thrust_Load_Range[0];
        //                //    }
        //                //    break;

        //                //case "OpCond.Thrust_Load_Range[1]":
        //                //    if (modMain.gProject.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
        //                //    {
        //                //        WorkSheet_In.Cells[i, 4] = OpCond_In.Thrust_Load_Range[1];
        //                //    }
        //                //    break;

        //                case "OpCond.OilSupply.Lube_Type":
        //                    WorkSheet_In.Cells[i, 4] = OpCond_In.OilSupply.Lube_Type;
        //                    break;

        //                case "OpCond.OilSupply.Reqd_Flow":
        //                    WorkSheet_In.Cells[i, 4] = OpCond_In.OilSupply.Lube_Type;
        //                    break;

        //                case "OpCond.OilSupply.Press":
        //                    WorkSheet_In.Cells[i, 4] = OpCond_In.OilSupply.Press;
        //                    break;

        //                case "OpCond.OilSupply.Temp":
        //                    WorkSheet_In.Cells[i, 4] = OpCond_In.OilSupply.Temp;
        //                    break;

        //                case "Bearing.PerformData.Power":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.Power;
        //                    break;

        //                case "Bearing.PerformData.TempRise":
        //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)Project_In.Product.Bearing).PerformData.FlowReqd;
        //                    break;
        //            }
        //        }
        //    }
        //}

        private void Write_Parameter_Driver_Radial(clsProject Project_In, EXCEL.Worksheet WorkSheet_In)
        //===============================================================================================
        {
            //EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

            //int pRowCount = pExcelCellRange.Rows.Count;
            //string pParamName = "";
            //Double pTol_DFit_Low = 0.0, pTol_DFit_Up = 0.0;
            //Double pTol_DSet_Low = 0.0, pTol_DSet_Up = 0.0;
            //Double pTol_DPad_Low = 0.0, pTol_DPad_Up = 0.0;
            //Double pConvF = 25.4;
            ////if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.English)
            ////{
            ////    pConvF = 1;
            ////}

            //for (int i = 97; i <= pRowCount; i++)
            //{
            //    if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
            //    {
            //        pParamName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2);
            //    }
            //    switch (pParamName)
            //    {
            //        case "d95":
            //            pTol_DFit_Low = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OD_Range[0] - ((clsBearing_Radial_FP)Project_In.Product.Bearing).OD();
            //            pTol_DFit_Up = ((clsBearing_Radial_FP)Project_In.Product.Bearing).OD_Range[1] - ((clsBearing_Radial_FP)Project_In.Product.Bearing).OD();
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OD() * pConvF);
            //            WorkSheet_In.Cells[i, 5] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DFit_Up * pConvF);
            //            WorkSheet_In.Cells[i, 6] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DFit_Low * pConvF);
            //            break;

            //        case "d97":
            //            ////  if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eBolting.Front  ||
            //            ////    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            ////    {
            //            ////         WorkSheet_In.Cells[i, 4] = (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).DFit() - (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TWall_BearingCB(0) * 2)) * pConv_MmToInch;
                             
            //            ////    }

            //            ////if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //            ////    ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Holes_Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            ////    {                               
            //            ////         WorkSheet_In.Cells[i, 4] = (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).DFit() - (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TWall_BearingCB(1) * 2)) * pConv_MmToInch;
                               
            //            ////    }   
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front)
            //            {
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OD() - (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TWall_BearingCB(0) * 2)) * pConvF);                           
            //            }
            //            else if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back)
            //            {
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OD() - (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TWall_BearingCB(1) * 2)) * pConvF);
            //            }
            //            else
            //            {
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OD() - (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TWall_BearingCB(0) * 2)) * pConvF);
            //            }   
            //            break;

            //        case "d98":
            //            //WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).DFit() + 
            //            //                            (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T.Pivot +
            //            //                            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web.H + 0.025) * 2) * pConvF);
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).MillRelief.D_PadRelief() * pConvF);
            //            break;

            //        case "d99":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).L) * pConvF);
            //            break;

            //        case "d101":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus.Wid) * pConvF);
            //            break;

            //        case "d105":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Bore() - 
            //                                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.GapEDM) * pConvF);
            //            break;

            //        case "d106":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Bore() - 
            //                                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).MillRelief.AxialSealGap[0]) * pConvF);
            //            break;

            //        case "d107":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(2.5);     //....LandL?
            //            break;

            //        case "d108":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(1.3);      //....EDM-BackFillet?
            //            break;

            //        case "d133":
            //            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Angle;
            //            break;

            //        case "d134":
            //            WorkSheet_In.Cells[i, 4] = ((360 / ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Count) - 
            //                                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Angle) / 2;
            //            break;

            //        case "d135":
            //            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Angle * (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Pivot.Offset/100);
            //            break;

            //        case "d136":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.GapEDM / 2) * pConvF);
            //            break;

            //        case "d138":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.GapEDM / 2) * pConvF);
            //            break;

            //        case "d139":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.GapEDM * pConvF);
            //            break;

            //        case "d140":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Bore() +
            //                                       ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T.Trail) / 2) * pConvF);
            //            break;

            //        case "d145":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.GapEDM * pConvF);
            //            break;

            //        case "d153":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Bore() / 2) - 0.2) * pConvF);
            //            break;

            //        case "d154":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web.T / 2) * pConvF);
            //            break;

            //        case "d159":
            //            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Count;
            //            break;

            //        case "d163":                        
            //            pTol_DPad_Low = ((clsBearing_Radial_FP)Project_In.Product.Bearing).PadBore_Range[0] - ((clsBearing_Radial_FP)Project_In.Product.Bearing).PadBore();
            //            pTol_DPad_Up = ((clsBearing_Radial_FP)Project_In.Product.Bearing).PadBore_Range[1] - ((clsBearing_Radial_FP)Project_In.Product.Bearing).PadBore();
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PadBore() * pConvF);
            //            WorkSheet_In.Cells[i, 5] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DPad_Up * pConvF);
            //            WorkSheet_In.Cells[i, 6] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DPad_Low * pConvF);
            //            break;

            //        case "d164":  
            //         pTol_DSet_Low = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Bore_Range[0] - ((clsBearing_Radial_FP)Project_In.Product.Bearing).Bore();
            //            pTol_DSet_Up = ((clsBearing_Radial_FP)Project_In.Product.Bearing).Bore_Range[1] - ((clsBearing_Radial_FP)Project_In.Product.Bearing).Bore();
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Bore() * pConvF);
            //            WorkSheet_In.Cells[i, 5] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DSet_Up * pConvF);
            //            WorkSheet_In.Cells[i, 6] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DSet_Low * pConvF);
            //            break;

            //        case "d170":
            //            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).DSet() / 2 + ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).AntiRotPin.of;
            //            break;
            //            //((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).L/2)-
            //        case "ARP_Offset_From_Center":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL(((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).L)/2 - ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Loc_Dist_Back) * pConvF);
            //            break;

            //        case "d175":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Loc_Offset * pConvF);
            //            break;

            //        case "d176":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.HoleDepth * pConvF);
            //            break;

            //        case "d177":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Spec.L);
            //            break;

            //        case "d178":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Spec.D * pConvF);
            //            break;

            //        case "d212":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice.Count);
            //            break;

            //        case "d240":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LScrew_Loc.Center * pConvF);
            //            break;

            //        case "d449":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LScrew_Loc.Back * pConvF);
            //            break;

            //        case "d241":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RScrew_Loc.Center * pConvF);
            //            break;

            //        case "d322":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.RScrew_Loc.Back * pConvF);
            //            break;

            //        case "d324":
            //            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Thread_Depth * pConv_InchToMM;
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(0.177 * pConvF);
            //            break;

            //        case "d276":
            //            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.CBore_Depth * pConv_InchToMM;
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(0.281 * pConvF);
            //            break;

            //        case "d283":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Bore() +
            //                                       (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).LiningT * 2)) * pConvF);
            //            break;

            //        case "d284":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).LiningT * pConvF);
            //            break;

            //        case "d291":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.L * pConvF);
            //            break;

            //        case "d295":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OD() * pConvF);
            //            WorkSheet_In.Cells[i, 5] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DFit_Up);
            //            WorkSheet_In.Cells[i, 6] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DFit_Low);
            //            break;

            //        case "d310":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel_Spec.D * pConvF);
            //            break;

            //        case "d308":
            //            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel_Depth * pConv_InchToMM;
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(0.197 * pConvF);
            //            break;

            //        case "d315":
            //            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel_Spec.L - (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Dowel_Depth * pConv_InchToMM);
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(0.236 * pConvF);
            //            break;

            //        case "d317":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OD() * 1.25) * pConvF);
            //            break;

            //        case "d321":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.LDowel_Loc.Back) * pConvF);
            //            break;

            //        case "d328":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Spec_L);
            //            break;

            //        case "d330":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).SL.Screw.Spec_D * pConvF);
            //            break;

            //        case "Bearing_Bore":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Bore() * pConvF);
            //            WorkSheet_In.Cells[i, 5] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DSet_Up * pConvF);
            //            WorkSheet_In.Cells[i, 6] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DSet_Low * pConvF);
            //            break;

            //        case "Pad_Bore":
            //            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PadBore() * pConvF;
            //            WorkSheet_In.Cells[i, 5] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DPad_Up * pConvF);
            //            WorkSheet_In.Cells[i, 6] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DPad_Low * pConvF);
            //            break;

            //        case "Pad_Arc_Length":
            //            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Angle;
            //            break;

            //        case "Pad_THK":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T.Pivot * pConvF);
            //            break;

            //        case "d361":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OD() * pConvF);
            //            WorkSheet_In.Cells[i, 5] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DFit_Up * pConvF);
            //            WorkSheet_In.Cells[i, 6] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DFit_Low * pConvF);
            //            break;

            //        case "d375":
            //            WorkSheet_In.Cells[i, 4] = -((360 / ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Count) -
            //                                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Angle) / 2;
            //            break;

            //        case "Bearing_OD":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OD() * pConvF);
            //            WorkSheet_In.Cells[i, 5] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DFit_Up * pConvF);
            //            WorkSheet_In.Cells[i, 6] = modMain.gProject.PNR.Unit.WriteInUserL(pTol_DFit_Low * pConvF);
            //            break;

            //        case "OAL":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).L * pConvF);
            //            break;

            //        case "Pivot_Offset":
            //            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Pivot.Offset/100;
            //            break;

            //        case "Pad_Length":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.L * pConvF);
            //            break;

            //        case "d386":
            //            ////WorkSheet_In.Cells[i, 4] = ((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).L - 
            //            ////                           ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.L) / 2 +
            //            ////                           ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.L / 2.5) * pConv_InchToMM;
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Count_MainOilSupply == 1)
            //            {
            //                WorkSheet_In.Cells[i, 4] = 0;
            //            }
            //            else
            //            {
            //                //WorkSheet_In.Cells[i, 4] = ((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).L -
            //                //                           ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.L) / 2 +
            //                //                           ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.L / 2.5) * pConv_InchToMM;
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(17.2);
            //            }
            //            break;

            //        case "d387":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Count_MainOilSupply == 1)
            //            {
            //                WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice.Loc_Back * pConvF);
            //            }
            //            else
            //            {
            //                WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( 22.8);
            //            }
                        
            //            break;

            //        case "d388":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Count_MainOilSupply == 1)
            //            {
            //                WorkSheet_In.Cells[i, 4] = 0;
            //            }
            //            else
            //            {
            //                WorkSheet_In.Cells[i, 4] = 12;
            //            }

            //            break;

            //        case "d390":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice.D * pConvF);
            //            break;

            //        case "d392":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(2.385);//(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice.L) * pConv_InchToMM;
            //            break;

            //        ////case "d393":
            //        ////    WorkSheet_In.Cells[i, 4] = (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice.D_CBore) * pConvF;
            //        ////    //WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(3.175);
            //        ////    break;

            //        case "d416":
            //            WorkSheet_In.Cells[i, 4] = ((360 / ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Count) -
            //                                        ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Angle);
            //            break;

            //        case "d423":
            //            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Angle * (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Pivot.Offset/100);
            //            break;

            //        case "d440":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Bore() * pConvF);
            //            WorkSheet_In.Cells[i, 5] =modMain.gProject.PNR.Unit.WriteInUserL( pTol_DSet_Up * pConvF);
            //            WorkSheet_In.Cells[i, 6] =modMain.gProject.PNR.Unit.WriteInUserL( pTol_DSet_Low * pConvF);
            //            break;

            //        case "d441":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).PadBore() * pConvF);
            //            WorkSheet_In.Cells[i, 5] =modMain.gProject.PNR.Unit.WriteInUserL( pTol_DPad_Up * pConvF);
            //            WorkSheet_In.Cells[i, 6] =modMain.gProject.PNR.Unit.WriteInUserL( pTol_DPad_Low * pConvF);
            //            break;

            //        case "Pad_THK_Leading":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T.Lead * pConvF);
            //            break;

            //        case "Pad_THK_Trailing":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T.Trail * pConvF);
            //            break;

            //        case "d443":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T.Pivot * pConvF);
            //            break;

            //        case "d446":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T.Trail * pConvF);
            //            break;

            //        case "d447":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web.RFillet * pConvF);
            //            break;

            //        case "Web_Fillet":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web.RFillet * pConvF);
            //            break;

            //        case "d448":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.T.Lead * pConvF);
            //            break;

            //        case "d451":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice.D * pConvF);
            //            break;

            //        case "d457":
            //            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.L;
            //            break;

            //        case "d464":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.L / 2) * pConvF);
            //            break;

            //        case "d215":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Radial Bearing")
            //                {
            //                    WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].DBC * pConvF);
            //                }
            //            }
            //            break;

            //        case "d230":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Radial Bearing")
            //                {
            //                    WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].DBC * pConvF);
            //                }
            //            }
            //            break;

            //        case "d466":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OD() - 
            //                                       ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus.D) * pConvF);
            //            break;

            //        case "d467":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Radial Bearing")
            //                {
            //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngStart;
            //                }
            //            }
            //            break;

            //        case "d468":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Radial Bearing")
            //                {
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[0];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 1)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "d469":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Radial Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[1];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[1];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 2)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "d470":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Radial Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[2];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[2];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 3)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "d471":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Radial Bearing")
            //                {
            //                   // WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[3];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[3];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 4)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "d472":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Radial Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[4];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[4];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 5)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "d473":
            //             if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front  ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Radial Bearing")
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngStart;
            //                    }
            //                }
            //            break;

            //        case "d364":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Radial Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[0];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[0];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 1)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "d418":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Radial Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[1];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[1];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 2)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "d419":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Radial Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[2];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[2];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 3)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "d366":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Radial Bearing")
            //                {
            //                   // WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[3];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[3];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 4)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "d420":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //               ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Radial Bearing")
            //                {
            //                   //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[4];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[4];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 5)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "Rec_Lining_THK":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).LiningT * pConvF);
            //            break;

            //        case "d481":
            //            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Pivot.AngStart;
            //            break;

            //        case "Oil_Nozzle_DIA":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice.D * pConvF);
            //            break;

            //        case "d482":
            //            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Angle;
            //            break;

            //        case "d487":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).L / 2) * pConvF);
            //            break;

            //        case "EDM_Cut_Width":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.GapEDM * pConvF);
            //            break;

            //        case "Web_THK":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web.T * pConvF);
            //            break;

            //        case "Web_Height":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web.H * pConvF);
            //            break;

            //        case "d498":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web.H * pConvF);
            //            break;

            //        case "d499":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.Web.T * pConvF);
            //            break;

            //        case "Pad_Edge_Round":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.RFillet * pConvF);
            //            break;

            //        case "Oil_Annulus_Width":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL(((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus.Wid * pConvF);
            //            break;

            //        case "Oil_Annulus_Depth":
            //            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Annulus.D;
            //            break;

            //        case "Seal_Mount_Wall_THK":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front  ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Front Config - Seal")
            //                    {
            //                        WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TWall_BearingCB(0)) * pConvF);
            //                    }                            
            //                }

            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Back Config - Seal")
            //                    {
            //                        WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).TWall_BearingCB(1)) * pConvF);
            //                    }
            //                }    
                                             
            //            break;

            //        ////case "Oil_Nozzle_CDrill_DIA":
            //        ////    WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OilInlet.Orifice.D_CBore * pConvF);
            //        ////    break;

            //        case "Pivot_Start_Angle":
            //            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Pad.Pivot.AngStart;
            //            break;

            //        case "ARP_Offset":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Loc_Offset * pConvF);
            //            break;

            //        case "ARP_DIA":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).ARP.Spec.D * pConvF);
            //            break;

            //        case "d501":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).OD() * pConvF);
            //            WorkSheet_In.Cells[i, 5] =modMain.gProject.PNR.Unit.WriteInUserL( pTol_DFit_Up * pConvF);
            //            WorkSheet_In.Cells[i, 6] =modMain.gProject.PNR.Unit.WriteInUserL( pTol_DFit_Low * pConvF);
            //            break;

            //        case "d502":
            //            WorkSheet_In.Cells[i, 4] =modMain.gProject.PNR.Unit.WriteInUserL( ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Bore() * pConvF);
            //            WorkSheet_In.Cells[i, 5] =modMain.gProject.PNR.Unit.WriteInUserL( pTol_DSet_Up * pConvF);
            //            WorkSheet_In.Cells[i, 6] =modMain.gProject.PNR.Unit.WriteInUserL( pTol_DSet_Low * pConvF);
            //            break;

            //        case "d505":
            //            WorkSheet_In.Cells[i, 4] = modMain.gProject.PNR.Unit.WriteInUserL((((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).FlexurePivot.GapEDM / 2) * pConvF);
            //            break;

            //    }
                
                
            //}

        }

        private void Write_Parameter_Driver_Seal(clsProject Project_In, clsSeal Seal_In, EXCEL.Worksheet WorkSheet_In)
        //============================================================================================================
        {
            //EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

            //int pRowCount = pExcelCellRange.Rows.Count;
            //string pParamName = "";
            //Double pTol_DSet_Low = 0.0, pTol_DSet_Up = 0.0;
            //Double pConvF = 25.4;
            ////if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.English)
            ////{
            ////    pConvF = 1;
            ////}

            //for (int i = 97; i <= pRowCount; i++)
            //{
            //    if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
            //    {
            //        pParamName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2);
            //    }
            //    switch (pParamName)
            //    {
            //        case "d95":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front  ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Front Config - Seal")
            //                {
            //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].D_Finish * pConvF;
            //                }                            
            //            }

            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Back Config - Seal")
            //                {
            //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].D_Finish * pConvF;
            //                }
            //            }
                        
            //            break;

            //        case "d96":
            //            pTol_DSet_Low = Seal_In.DBore_Range[0] - Seal_In.DBore();
            //            pTol_DSet_Up = Seal_In.DBore_Range[1] - Seal_In.DBore();
            //            WorkSheet_In.Cells[i, 4] = Seal_In.DBore() * pConvF;
            //            WorkSheet_In.Cells[i, 5] =pTol_DSet_Up * pConvF;
            //            WorkSheet_In.Cells[i, 6] = pTol_DSet_Low * pConvF;
            //            break;

            //        case "d97":
            //            WorkSheet_In.Cells[i, 4] = Seal_In.L * pConvF;
            //            break;

            //        case "d99":
            //            WorkSheet_In.Cells[i, 4] =Seal_In.Blade.T * pConvF;
            //            break;

            //        case "d105":
            //                if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front  ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Front Config - Seal")
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngStart;
            //                    }                            
            //                }

            //                if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Back Config - Seal")
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngStart;
            //                    }
            //                }
                       
            //            break;

            //        case "d106":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front  ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Front Config - Seal")
            //                    {
            //                       // WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[0];
            //                        if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[0];
            //                        }
            //                        else
            //                        {
            //                            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 1)
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                            }
            //                            else
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = 0;
            //                            }
            //                        }
            //                    }                            
            //                }

            //                if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Back Config - Seal")
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[0];
            //                        if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[0];
            //                        }
            //                        else
            //                        {
            //                            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 1)
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                            }
            //                            else
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = 0;
            //                            }
            //                        }
            //                    }
            //                }
                   
            //            break;

            //        case "d107":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front  ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Front Config - Seal")
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[1];
            //                        if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[1];
            //                        }
            //                        else
            //                        {
            //                            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 2)
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                            }
            //                            else
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = 0;
            //                            }
            //                        }
            //                    }                            
            //                }

            //                if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Back Config - Seal")
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[1];
            //                        if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[1];
            //                        }
            //                        else
            //                        {
            //                            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 2)
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                            }
            //                            else
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = 0;
            //                            }
            //                        }
            //                    }
            //                }
            //            break;

            //        case "d108":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front  ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Front Config - Seal")
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[2];
            //                        if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[2];
            //                        }
            //                        else
            //                        {
            //                            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 3)
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                            }
            //                            else
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = 0;
            //                            }
            //                        }
            //                    }                            
            //                }

            //                if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Back Config - Seal")
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[2];
            //                        if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[2];
            //                        }
            //                        else
            //                        {
            //                            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 3)
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                            }
            //                            else
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = 0;
            //                            }
            //                        }
            //                    }
            //                }
            //            break;

            //        case "d109":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front  ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Front Config - Seal")
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[3];
            //                        if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[3];
            //                        }
            //                        else
            //                        {
            //                           // WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 4)
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                            }
            //                            else
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = 0;
            //                            }
            //                        }
            //                    }                            
            //                }

            //                if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Back Config - Seal")
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[3];
            //                        if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[3];
            //                        }
            //                        else
            //                        {
            //                            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 4)
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                            }
            //                            else
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = 0;
            //                            }
            //                        }
            //                    }
            //                }
            //            break;

            //        case "d110":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front  ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Front Config - Seal")
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[4];
            //                        if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[4];
            //                        }
            //                        else
            //                        {
            //                            //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 5)
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                            }
            //                            else
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = 0;
            //                            }
            //                        }
            //                    }                            
            //                }

            //                if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //                {
            //                    if (WorkSheet_In.Name == "Back Config - Seal")
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[4];
            //                        if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[4];
            //                        }
            //                        else
            //                        {
            //                           // WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                            if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 5)
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                            }
            //                            else
            //                            {
            //                                WorkSheet_In.Cells[i, 4] = 0;
            //                            }
            //                        }
            //                    }
            //                }
            //            break;

            //        case "d113":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Front Config - Seal")
            //                {
            //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].DBC * pConvF;
            //                }
            //            }

            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Back Config - Seal")
            //                {
            //                    WorkSheet_In.Cells[i, 4] =((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].DBC * pConvF;
            //                }
            //            }
            //            break;

            //        case "d114":
            //            WorkSheet_In.Cells[i, 4] =Seal_In.MountHoles.Screw.D_Thru * pConvF;
            //            break;

            //        case "d116":
            //            WorkSheet_In.Cells[i, 4] = Seal_In.MountHoles.Screw.D_CBore * pConvF;
            //            break;

            //        case "d117":
            //            WorkSheet_In.Cells[i, 4] = Seal_In.MountHoles.Depth_CBore * pConvF;
            //            break;

            //        case "d141":
            //            WorkSheet_In.Cells[i, 4] = Seal_In.Blade.AngTaper;
            //            break;                        
            //    }

            //}

        }


        private void Write_Parameter_Driver_Thrust(clsProject Project_In, clsBearing_Thrust_TL Thrust_In, EXCEL.Worksheet WorkSheet_In)
        //===========================================================================================================================   AES 25JUL18
        {
            //EXCEL.Range pExcelCellRange = WorkSheet_In.UsedRange;

            //int pRowCount = pExcelCellRange.Rows.Count;
            //string pParamName = "";
            //Double pTol_DSet_Low = 0.0, pTol_DSet_Up = 0.0;
            //Double pConvF = 25.4;
            ////if (modMain.gProject.PNR.Unit.System == clsUnit.eSystem.English)
            ////{
            ////    pConvF = 1;
            ////}

            //for (int i = 2; i <= pRowCount; i++)
            //{
            //    if (((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2 != null || Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2) != "")
            //    {
            //        pParamName = Convert.ToString(((EXCEL.Range)pExcelCellRange.Cells[i, 1]).Value2);
            //    }
            //    switch (pParamName)
            //    {
            //        case "d95":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Front TL Thurst Bearing")
            //                {
            //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].D_Finish * pConvF;
            //                }
            //            }

            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Back TL Thurst Bearing")
            //                {
            //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].D_Finish * pConvF;
            //                }
            //            }

            //            break;

            //        case "d96":
            //            WorkSheet_In.Cells[i, 4] = Thrust_In.PadD[1] * pConvF;
            //            break;

            //        case "d97":
            //            WorkSheet_In.Cells[i, 4] = Thrust_In.PadD[0] * pConvF;
            //            break;

            //        case "d98":
            //            WorkSheet_In.Cells[i, 4] = Thrust_In.DBore() * pConvF;
            //            break;

            //        case "d102":
            //            WorkSheet_In.Cells[i, 4] = Thrust_In.LAND_L * pConvF;
            //            break;

            //        case "d111":
            //            WorkSheet_In.Cells[i, 4] = Thrust_In.L * pConvF;
            //            break;

            //        case "d112":
            //            WorkSheet_In.Cells[i, 4] = Thrust_In.FeedGroove.Depth * pConvF;
            //            break;

            //        case "d113":
            //            WorkSheet_In.Cells[i, 4] = Thrust_In.FeedGroove.Wid * pConvF;
            //            break;

            //        case "d120":
            //            WorkSheet_In.Cells[i, 4] = Thrust_In.Pad_Count;
            //            break;

            //        case "d135":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Front TL Thurst Bearing")
            //                {
            //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].DBC * pConvF;
            //                }
            //            }

            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Back TL Thurst Bearing")
            //                {
            //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].DBC * pConvF;
            //                }
            //            }
            //            break;

            //        case "d136":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Front TL Thurst Bearing")
            //                {
            //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngStart;
            //                }
            //            }

            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Back TL Thurst Bearing")
            //                {
            //                    WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngStart;
            //                }
            //            }

            //            break;

            //        case "d137":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Front TL Thurst Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[0];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[0];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 1)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }

            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Back TL Thurst Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[0];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[0];
            //                    }
            //                    else
            //                    {
            //                       // WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 1)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }

            //            break;

            //        case "d138":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Front TL Thurst Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[1];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[1];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 2)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }

            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Back TL Thurst Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[1];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[1];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 2)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "d139":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Front TL Thurst Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[2];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[2];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 3)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }

            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Back TL Thurst Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[2];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[2];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 3)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "d140":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Front TL Thurst Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[3];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[3];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 4)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }

            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Back TL Thurst Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[3];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[3];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 4)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "d141":
            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Front ||
            //                ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Front TL Thurst Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[4];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[0].HolesAngOther[4];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);
            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[0].Hole.Count > 5)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }

            //            if (((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Back ||
            //            ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Bolting == clsBearing_Radial_FP.eBolting.Both)
            //            {
            //                if (WorkSheet_In.Name == "Back TL Thurst Bearing")
            //                {
            //                    //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[4];
            //                    if (!((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].Hole.EquiSpaced)
            //                    {
            //                        WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Screw[1].HolesAngOther[4];
            //                    }
            //                    else
            //                    {
            //                        //WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.MountFixture_Sel_AngOther(0);

            //                        if (((clsBearing_Radial_FP)Project_In.Product.Bearing).Mount.Screw[1].Hole.Count > 5)
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = ((clsBearing_Radial_FP)modMain.gProject.Product.Bearing).Mount.Mount_Sel_AngBet(0);
            //                        }
            //                        else
            //                        {
            //                            WorkSheet_In.Cells[i, 4] = 0;
            //                        }
            //                    }
            //                }
            //            }
            //            break;

            //        case "d144":
            //            WorkSheet_In.Cells[i, 4] = Thrust_In.MountHoles.Screw.D_Thru * pConvF;
            //            break;

            //        case "d146":
            //            WorkSheet_In.Cells[i, 4] = Thrust_In.MountHoles.Screw.D_CBore * pConvF;
            //            break;

            //        case "d147":
            //            WorkSheet_In.Cells[i, 4] = Thrust_In.MountHoles.Depth_CBore * pConvF;
            //            break;

            //        case "d159":
            //            WorkSheet_In.Cells[i, 4] = Thrust_In.FeedGroove.Wid * pConvF;
            //            break;
            //    }

            //}

        }

        private void Copy_Inventor_Model_Files(clsProject Project_In, clsFiles Files_In, String FilePath_In)
        //===================================================================================================
        {
            try
            {
                //  MODEL FILES.
                //  -----------
                //
                ////....Complete Assy: 
                ////
                //string pFileName_CompleteAssy = FilePath_In + "\\" + System.IO.Path.GetFileName(Files_In.FileTitle_Template_Inventor_Complete);

                //if (System.IO.File.Exists(pFileName_CompleteAssy))
                //    System.IO.File.Delete(pFileName_CompleteAssy);

                //System.IO.File.Copy(Files_In.FileTitle_Template_Inventor_Complete, pFileName_CompleteAssy);


                ////....Radial Assy:
                ////
                //string pFileName_RadialAssy = FilePath_In + "\\" + Path.GetFileName(Files_In.FileTitle_Template_Inventor_Radial_Assy);

                //if (File.Exists(pFileName_RadialAssy))
                //    File.Delete(pFileName_RadialAssy);

                //File.Copy(Files_In.FileTitle_Template_Inventor_Radial_Assy, pFileName_RadialAssy);


                //....Radial:
                //
                string pFileName_Radial = FilePath_In + "\\" + System.IO.Path.GetFileName(Files_In.FileTitle_Template_Inventor_Radial);

                if (System.IO.File.Exists(pFileName_Radial))
                    System.IO.File.Delete(pFileName_Radial);
                System.IO.File.Copy(Files_In.FileTitle_Template_Inventor_Radial, pFileName_Radial);


                ////....Seal Front:
                ////
                //if (Project_In.Product.EndPlate[0].Type == clsEndPlate.eType.Seal)
                //{
                //    string pFileName_Seal_Front = FilePath_In + "\\" + System.IO.Path.GetFileName(Files_In.FileTitle_Template_Inventor_Seal_Front);
                //    if (System.IO.File.Exists(pFileName_Seal_Front))
                //        System.IO.File.Delete(pFileName_Seal_Front);
                //    System.IO.File.Copy(Files_In.FileTitle_Template_Inventor_Seal_Front, pFileName_Seal_Front);
                //}

                ////....Seal Back:
                ////
                //if (Project_In.Product.EndPlate[1].Type == clsEndPlate.eType.Seal)
                //{
                //    string pFileName_Seal_Back = FilePath_In + "\\" + System.IO.Path.GetFileName(Files_In.FileTitle_Template_Inventor_Seal_Back);
                //    if (System.IO.File.Exists(pFileName_Seal_Back))
                //        System.IO.File.Delete(pFileName_Seal_Back);
                //    System.IO.File.Copy(Files_In.FileTitle_Template_Inventor_Seal_Back, pFileName_Seal_Back);
                //}

                ////....Thrust Bearing Front: 
                ////
                //if (Project_In.Product.EndPlate[0].Type == clsEndPlate.eType.TL_TB)
                //{
                //    string pFileName_Thrust_Front = FilePath_In + "\\" + System.IO.Path.GetFileName(Files_In.FileTitle_Template_Inventor_Thrust_Front);
                //    if (System.IO.File.Exists(pFileName_Thrust_Front))
                //        System.IO.File.Delete(pFileName_Thrust_Front);
                //    System.IO.File.Copy(Files_In.FileTitle_Template_Inventor_Thrust_Front, pFileName_Thrust_Front);
                //}

                ////....Thrust Bearing Back: 
                ////
                //if (Project_In.Product.EndPlate[1].Type == clsEndPlate.eType.TL_TB)
                //{
                //    string pFileName_Thrust_Back = FilePath_In + "\\" + System.IO.Path.GetFileName(Files_In.FileTitle_Template_Inventor_Thrust_Back);
                //    if (System.IO.File.Exists(pFileName_Thrust_Back))
                //        System.IO.File.Delete(pFileName_Thrust_Back);
                //    System.IO.File.Copy(Files_In.FileTitle_Template_Inventor_Thrust_Back, pFileName_Thrust_Back);
                //}
            }

            catch (Exception ex)
            {
                MessageBox.Show("Unable to copy Inventor File.Please close Inventor Files.");
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

        private void cmdOpen_CompleteAssy_Click(object sender, EventArgs e)
        //==================================================================
        {
            if (txtFilePath_Project.Text != "")
            {
                CloseInventor();
                CloseExcelFiles();
                string pFileName = txtFilePath_Project.Text;

                Cursor = Cursors.WaitCursor;
                //CreateParameter_Driver();
                modMain.gFiles.Write_Parameter_Complete(modMain.gProject, pFileName, false);
                Copy_Inventor_Model_Files(modMain.gProject, modMain.gFiles, txtFilePath_Project.Text);                

                Process.Start(txtFilePath_Project.Text);
                Cursor = Cursors.Default;                
            }
        }

        public void CloseInventor()
        //===========================      
        {
            Process[] pProcesses = Process.GetProcesses();

            try
            {
                foreach (Process p in pProcesses)
                    if (p.ProcessName == "Inventor")
                        p.Kill();
            }
            catch (Exception pEXP)
            {

            }
        }

        
    }
}
