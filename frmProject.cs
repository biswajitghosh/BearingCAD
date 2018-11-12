﻿
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmProject                             '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  30OCT18                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//....Class Constructor.
//       Public Sub        New                                 ()

//   METHODS:
//   -------
//       Private Sub       frmProject_Load                     ()
//       Private Sub       DisplayData                         ()

//       Private Sub       cmdClose_Click                      ()
//       Private Sub       SaveData                            ()
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;
using EXCEL = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Drawing.Printing;
using System.Collections.Specialized;
using System.Linq;
using System.Collections.Specialized;

namespace BearingCAD22
{
    public partial class frmProject : Form
    {

        #region "MEMBER VARIABLE"
        //***********************

            //....Local Class Objects:
            private clsProject mProject;
            private clsOpCond mOpCond;      //....Used in Read_CustEnqySheet().
 
            private Boolean mbln_NewProject;

            StringCollection mNo_Suffix; // = new StringCollection();
            private Boolean mbln_TxtNoValidated;

        #endregion


        #region "FORM CONSTRUCTOR:"
        //*************************

            public frmProject()
            //==================
            {
                //....Constructor is called only once during its creation in modMain. 
                InitializeComponent();

                //cmdEnquiry.Visible = false;
                mbln_NewProject    = false;
                //optActive.Checked  = false; 

                Initialize_LocalObjects();

                Populate_CmbBoxes_All();
                
            }


            #region "Helper Routines:"
            //************************

                private void Initialize_LocalObjects()
                //=====================================
                {
                    //....PB 22JAN13. The following "Unit" assignment should be cascaded down automatically from mProject.
                    //....Project.
                    mProject         = new clsProject(clsUnit.eSystem.English);                   

                    //....Operating Conditions.
                    mOpCond = new clsOpCond();
                }
               

                private void Populate_CmbBoxes_All()
                //==================================
                {
                    LoadProducts();
                    LoadTypes();  
              
                    LoadUnits(); 

                    LoadEndConfigs(cmbEndConfig_Front);     
                    LoadEndConfigs(cmbEndConfig_Back);

                }


                #region "Sub-Helper Routines:"
                //***************************

                    private void LoadProducts()
                    //=========================
                    {
                        //.....Products: "Radial, Thrust".
                        cmbProduct.Items.Clear();
                        cmbProduct.DataSource = Enum.GetValues(typeof(clsProduct.eType));
                        cmbProduct.SelectedIndex = 0;           //....Default: Radial.
                    }


                    private void LoadTypes()
                    //======================
                    {
                        //....Radial Bearing Type: "Flexture_Pivot, Tilting_Pad, Sleeve".
                        Array pArray = Enum.GetValues(typeof(clsBearing_Radial.eDesign));
                        modMain.LoadCmbBox(cmbDesign, pArray);    //....Selected Index = 0; Default: Flexure_Pivot.
                    }


                    private void LoadUnits()
                    //======================
                    {
                        //....Units: English, Metric.
                        cmbUnitSystem.Items.Clear();
                        cmbUnitSystem.DataSource = Enum.GetValues(typeof(clsUnit.eSystem));
                        cmbUnitSystem.SelectedIndex = 0;        //Default: English  
                    }                  


                    private void LoadEndConfigs(ComboBox cmbBox_In)
                    //=============================================         
                    {
                        //....End Configs: "Seal , Thrust Brearing TL"
                        Array pArray = Enum.GetValues(typeof(clsEndPlate.eType));
                        modMain.LoadCmbBox(cmbBox_In, pArray);      //....Selected Index = 0; Default: Seal.
                    }        

                #endregion

            #endregion

        #endregion


        #region "FORM EVENT ROUTINES:"
        //*****************************

            private void frmProject_Load(object sender, EventArgs e)
            //======================================================
            {
                optOrder.Checked = false;
                optOrder.Checked = true;
                DisplayData();
            }


            private void frmProject_Activated(object sender, EventArgs e)
            //============================================================
            {
               
            }

        #endregion


        #region"DISPLAY DATA:"
        //********************

            private void DisplayData()
            //=========================
            {
                if (modMain.gProject != null)
                {
                    mProject =(clsProject) modMain.gProject.Clone();
                }

                //  Unit
                //  ----
                cmbUnitSystem.Text = mProject.PNR.Unit.System.ToString();

                if (mProject.SalesOrder.Type == clsProject.clsSalesOrder.eType.Order)
                {
                    optOrder.Checked = true;
                }
                else if (mProject.SalesOrder.Type == clsProject.clsSalesOrder.eType.Proposal)
                {
                    optProposal.Checked = true;
                }

                //mProject.Customer.Unit = mProject.Unit.System.ToString();
                if (mProject.SalesOrder.No != null && mProject.SalesOrder.No != "")
                {
                    //txtSONo_Part1.Text = mProject.SalesOrder.No.Substring(0, 2);
                    string pPart1 = mProject.SalesOrder.No.Substring(0, 2);

                    Boolean pValExists = false;
                    for (int i = 0; i < cmbSONo_Part1.Items.Count; i++)
                    {
                        if (cmbSONo_Part1.Items[i].ToString() == pPart1)
                        {
                            pValExists = true;
                            break;
                        }
                    }
                    if (!pValExists)
                    {
                        cmbSONo_Part1.Items.Add(pPart1);
                    }
                    cmbSONo_Part1.Text = mProject.SalesOrder.No.Substring(0, 2);
                    txtSONo_Part2.Text = mProject.SalesOrder.No.Substring(3, mProject.SalesOrder.No.Length - 3);
                    txtSONo_Part3.Text = mProject.SalesOrder.LineNo;
                }

                //  Customer
                //  --------
                txtCustName.Text = mProject.SalesOrder.Customer.Name;
                txtCustOrderNo.Text = mProject.SalesOrder.Customer.OrderNo;
                txtCustMachineName.Text = mProject.SalesOrder.Customer.MachineName;
                cmbUnitSystem.Text = mProject.PNR.Unit.System.ToString();

               

                //txtSONo.Text = mProject.SalesOrder.No + "-" + mProject.SalesOrder.LineNo;
                txtRelatedSONo.Text = mProject.SalesOrder.RelatedNo;
                txtPartNo.Text = mProject.PNR.No;
            
                cmbEndConfig_Front.Text = mProject.Product.EndPlate[0].Type.ToString().Replace("_", " ");
                cmbEndConfig_Back.Text = mProject.Product.EndPlate[1].Type.ToString().Replace("_", " ");
            }

        #endregion


        #region "CONTROL EVENT ROUTINES:"
        //******************************


            #region "TEXTBOX RELATED:"
            //------------------------

                private void txtBox_TextChanged(object sender, EventArgs e)
                //=========================================================
                {
                    TextBox pTxtBox = (TextBox)sender;

                    switch (pTxtBox.Name)
                    {
                        //case "txtNo":
                        //    //-------
                        //    mProject.No = txtSONo.Text;
                        //    break;

                        ////case "txtNo_Suffix":
                        ////    //---------------
                        ////    mProject.No_Suffix = txtNo_Suffix.Text;
                        ////    break;

                        //case "txtCustomer_Name":
                        //    //------------------
                        //    mProject.Customer_Name = cmbCustName.Text;
                        //    break;
                    }
                }

                private void optButton_CheckedChanged(object sender, EventArgs e)
                //===============================================================
                {
                    RadioButton pRadioButton = (RadioButton)sender;
                    switch (pRadioButton.Name)
                    {
                        case "optOrder":
                            //------------
                            cmbSONo_Part1.Items.Clear();
                            cmbSONo_Part1.Items.Add("SA");
                            cmbSONo_Part1.Items.Add("SG");
                            cmbSONo_Part1.Items.Add("SM");
                            cmbSONo_Part1.SelectedIndex = 0;
                            break;

                        case "optProposal":
                            //------------
                            cmbSONo_Part1.Items.Clear();
                            cmbSONo_Part1.Items.Add("EA");
                            cmbSONo_Part1.SelectedIndex = 0;
                            break;
                    }

                }
        

            #region "COMBOBOX RELATED ROUTINES:"
            //----------------------------------

                private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
                //======================================================================
                {
                    if (cmbProduct.SelectedIndex != 0)
                    {
                        string pstrMsg = "In this version 'Thrust Bearing' is not supported.";
                        string pstrCaption = "Project Information";
                        MessageBox.Show(pstrMsg, pstrCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmbProduct.Text = "Radial";
                        cmbProduct.SelectedIndex = 0;
                    }
                    

                    mProject.Product.Type = (clsProduct.eType)Enum.Parse(typeof(clsProduct.eType), cmbProduct.Text);
                }


                private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
                //====================================================================
                {
                    string pType= cmbDesign.Text;

                    if (cmbDesign.SelectedIndex != 0)                
                    {
                        string pstrMsg = "In this Version '" + cmbDesign.SelectedItem.ToString() + "' is not Supported";
                        string pstrCaption = "Project Information";
                        MessageBox.Show(pstrMsg, pstrCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmbDesign.SelectedIndex = 0;
                    }


                    if (pType == clsBearing_Radial.eDesign.Flexure_Pivot.ToString().Replace("_", " "))
                    {
                        ((clsBearing_Radial_FP)mProject.Product.Bearing).Design = clsBearing_Radial_FP.eDesign.Flexure_Pivot;
                    }
                }


                private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
                //===================================================================
                {
                    //if (cmbUnitSystem.SelectedIndex != 1)
                    //{
                    //    string pstrMsg = "In this version 'English' unit is not supported.";
                    //    string pstrCaption = "Project Information";
                    //    MessageBox.Show(pstrMsg, pstrCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);                        
                    //}
                    cmbUnitSystem.SelectedIndex = 1;
                    mProject.PNR.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbUnitSystem.Text);
                }
              
        
                private void cmbEndConfig_SelectedIndexChanged(object sender, EventArgs e)
                //=========================================================================
                {
                    clsEndPlate.eType[] pEndConfig_Type_Existing = new clsEndPlate.eType[]{mProject.Product.EndPlate[0].Type,
                                                                                             mProject.Product.EndPlate[1].Type};
                    clsEndPlate.eType[] pEndConfig_Type_Current  = new clsEndPlate.eType[2];


                    ComboBox pCmbBox = (ComboBox)sender;
                    string pName = pCmbBox.Name;
                    int index = 0;

                    switch (pName)
                    {
                        case "cmbEndConfig_Front":
                            index = 0;
                            break;

                        case "cmbEndConfig_Back":
                            index = 1;
                            break;
                    }
                    if (pCmbBox.SelectedIndex != 0)
                    {
                        string pstrMsg = "In this version 'TL TB' is not supported.";
                        string pstrCaption = "Project Information";
                        MessageBox.Show(pstrMsg, pstrCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    pCmbBox.SelectedIndex = 0;
                    pEndConfig_Type_Current[index] = (clsEndPlate.eType)Enum.Parse(typeof(clsEndPlate.eType), pCmbBox.Text.Replace(" ", "_"));

                    if (pEndConfig_Type_Current[index] != pEndConfig_Type_Existing[index])
                    {
                        if (pEndConfig_Type_Current[index] == clsEndPlate.eType.Seal)
                        {
                            mProject.Product.EndPlate[index] = new clsSeal(mProject.PNR.Unit.System, mProject.Product);
                        }
                        else
                        {
                            mProject.Product.EndPlate[index] = new clsBearing_Thrust_TL(mProject.PNR.Unit.System,  mProject.Product);
                        }
                    }
                   
                }
        
            #endregion
        
        
        #endregion

        
            #region "FORM CLOSE RELATED:"

                private void cmdClose_Click(object sender, EventArgs e)
                //=====================================================
                {
                    Button pCmdBtn = (Button)sender;

                    switch (pCmdBtn.Name)
                    {
                        case "cmdOK":
                            //-------
                             SaveData();
                             //SaveToDB_Project_ORM(modMain.gProject);
                             this.Hide();
                             modMain.gfrmMain.UpdateDisplay(modMain.gfrmMain);
                             modMain.gfrmOperCond.ShowDialog();    
                             break;

                        case "cmdCancel":
                            //-----------

                            this.Hide();                               
                            break;
                    }
                }

                private void SaveData()
                //======================
                { 

                    Boolean pNewProject = true;

                    //AES 18OCT18
                    if (modMain.gProject != null)
                    {
                        if (modMain.gProject.PNR.Unit.System != (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbUnitSystem.Text))
                        {
                            pNewProject = true;
                        }
                        else
                        {
                            clsEndPlate.eType[] pType = new clsEndPlate.eType[2];
                            pType[0] = (clsEndPlate.eType)Enum.Parse(typeof(clsEndPlate.eType), cmbEndConfig_Front.Text.Replace(" ", "_"));
                            pType[1] = (clsEndPlate.eType)Enum.Parse(typeof(clsEndPlate.eType), cmbEndConfig_Back.Text.Replace(" ", "_"));

                            for (int i = 0; i < 2; i++)
                            {
                                if (modMain.gProject.Product.EndPlate[i].Type != pType[i])
                                {
                                    pNewProject = true;
                                    break;
                                }
                                else
                                {
                                    pNewProject = false;
                                }
                            }
                        }
                    }

                    if (pNewProject)
                    {

                        clsUnit.eSystem pUnitSystem = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbUnitSystem.Text);

                        modMain.gProject = new clsProject(pUnitSystem);

                        //....Customer
                        modMain.gProject.SalesOrder.Customer.Name = txtCustName.Text;
                        modMain.gProject.SalesOrder.Customer.OrderNo = txtCustOrderNo.Text;
                        modMain.gProject.SalesOrder.Customer.MachineName = txtCustMachineName.Text;
                        modMain.gProject.PNR.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbUnitSystem.Text);

                        //....Sales Order
                        //string pSONo = txtSONo.Text;
                        if (cmbSONo_Part1.Text != "" && txtSONo_Part2.Text != "" && txtSONo_Part3.Text != "")
                        {
                            //modMain.gProject.SalesOrder.No = txtSONo_Part1.Text + " " + txtSONo_Part2.Text;
                            modMain.gProject.SalesOrder.No = cmbSONo_Part1.Text + " " + txtSONo_Part2.Text;
                            modMain.gProject.SalesOrder.LineNo = txtSONo_Part3.Text;
                        }


                        modMain.gProject.SalesOrder.RelatedNo = txtRelatedSONo.Text;
                        modMain.gProject.PNR.No = txtPartNo.Text;
                        modMain.gProject.Status = "Open";

                        if (optOrder.Checked)
                        {
                            modMain.gProject.SalesOrder.Type = clsProject.clsSalesOrder.eType.Order;
                        }
                        else if (optProposal.Checked)
                        {
                            modMain.gProject.SalesOrder.Type = clsProject.clsSalesOrder.eType.Proposal;
                        }

                        //....Product
                        //........Bearing 
                        modMain.gProject.Product.Type = (clsProduct.eType)Enum.Parse(typeof(clsProduct.eType), cmbProduct.Text);
                        ((clsBearing_Radial)modMain.gProject.Product.Bearing).Design = (clsBearing_Radial.eDesign)Enum.Parse(typeof(clsBearing_Radial.eDesign), cmbDesign.Text.ToString().Replace(" ", "_"));

                        clsEndPlate.eType[] pType = new clsEndPlate.eType[2];
                        pType[0] = (clsEndPlate.eType)Enum.Parse(typeof(clsEndPlate.eType), cmbEndConfig_Front.Text.Replace(" ", "_"));
                        pType[1] = (clsEndPlate.eType)Enum.Parse(typeof(clsEndPlate.eType), cmbEndConfig_Back.Text.Replace(" ", "_"));

                        for (int i = 0; i < 2; i++)
                        {
                            if (pType[i] == clsEndPlate.eType.Seal)
                            {
                                modMain.gProject.Product.EndPlate[i] = new clsSeal(modMain.gProject.PNR.Unit.System, modMain.gProject.Product);
                            }
                            else if (pType[i] == clsEndPlate.eType.TL_TB)
                            {
                                modMain.gProject.Product.EndPlate[i] = new clsBearing_Thrust_TL(modMain.gProject.PNR.Unit.System, modMain.gProject.Product);
                            }
                        }
                    }
                    else
                    {
                        //....Customer
                        modMain.gProject.SalesOrder.Customer.Name = txtCustName.Text;
                        modMain.gProject.SalesOrder.Customer.OrderNo = txtCustOrderNo.Text;
                        modMain.gProject.SalesOrder.Customer.MachineName = txtCustMachineName.Text;
                        modMain.gProject.PNR.Unit.System = (clsUnit.eSystem)Enum.Parse(typeof(clsUnit.eSystem), cmbUnitSystem.Text);

                        //....Sales Order   
                        if (cmbSONo_Part1.Text != "" && txtSONo_Part2.Text != "" && txtSONo_Part3.Text != "")
                        {
                            //modMain.gProject.SalesOrder.No = txtSONo_Part1.Text + " " + txtSONo_Part2.Text;
                            modMain.gProject.SalesOrder.No = cmbSONo_Part1.Text + " " + txtSONo_Part2.Text;
                            modMain.gProject.SalesOrder.LineNo = txtSONo_Part3.Text;
                        }


                        modMain.gProject.SalesOrder.RelatedNo = txtRelatedSONo.Text;
                        modMain.gProject.PNR.No = txtPartNo.Text;
                        modMain.gProject.Status = "Open";

                        if (optOrder.Checked)
                        {
                            modMain.gProject.SalesOrder.Type = clsProject.clsSalesOrder.eType.Order;
                        }
                        else if (optProposal.Checked)
                        {
                            modMain.gProject.SalesOrder.Type = clsProject.clsSalesOrder.eType.Proposal;
                        }
                    }                   
                }


                private void cmdImport_DDR_Click(object sender, EventArgs e)
                //===========================================================
                {
                    string pWordFileName = "";
                    openFileDialog1.Filter = "DDR files|*.doc;*.docx";
                    openFileDialog1.FilterIndex = 1;
                    openFileDialog1.InitialDirectory = modMain.gFiles.File_InputPath; 
                    openFileDialog1.Title = "Open";
                    openFileDialog1.FileName = " ";


                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;

                        txtCustName.Text = "";
                        txtCustOrderNo.Text = "";
                        txtCustMachineName.Text = "";
                        cmbSONo_Part1.Text = "";
                        txtSONo_Part2.Text = "";
                        txtSONo_Part3.Text = "";
                        txtRelatedSONo.Text = "";
                        txtPartNo.Text = "";
                        pWordFileName = openFileDialog1.FileName;
                        modMain.gProject = new clsProject(clsUnit.eSystem.Metric);
                        modMain.gFiles.Import_DDR_Data(pWordFileName, ref modMain.gProject);

                        Cursor = Cursors.Default;
                        //}

                        if (modMain.gProject.SalesOrder.No != "")
                        {
                            txtCustName.Text = modMain.gProject.SalesOrder.Customer.Name;
                            if (modMain.gProject.SalesOrder.Type == clsProject.clsSalesOrder.eType.Order)
                            {
                                optOrder.Checked = true;
                            }
                            else
                            {
                                optProposal.Checked = true;
                            }
                            txtCustOrderNo.Text = modMain.gProject.SalesOrder.Customer.OrderNo;
                            txtCustMachineName.Text = modMain.gProject.SalesOrder.Customer.MachineName;
                            txtPartNo.Text = modMain.gProject.PNR.No;


                            string pSO_No = modMain.gProject.SalesOrder.No;
                            cmbSONo_Part1.Text = pSO_No.Substring(0, 2);
                            if (pSO_No.Contains("-"))
                            {
                                txtSONo_Part2.Text = modMain.ExtractMidData(pSO_No, " ", "-");
                            }
                            else
                            {
                                txtSONo_Part2.Text = pSO_No.Substring(3);
                                //txtSONo_Part2.Text = modMain.ExtractPostData(pSO_No, " ");
                            }

                            string pTemp = modMain.ExtractPostData(pSO_No, "-");

                            Boolean pIsNumeric = false;
                            foreach (char value in pTemp)
                            {
                                pIsNumeric = char.IsDigit(value);
                            }

                            if (pIsNumeric)
                            {
                                txtSONo_Part3.Text = Convert.ToString(System.Text.RegularExpressions.Regex.Replace(pTemp, "[^0-9]+", string.Empty));
                            }

                            txtRelatedSONo.Text = modMain.gProject.SalesOrder.RelatedNo;
                        }
                    }
                }

                private void cmdImport_XLRadial_Click(object sender, EventArgs e)
                //================================================================
                {
                    SaveData();
                    Import_Analytical_Data();
                }

                private void Import_Analytical_Data()
                //===================================
                {
                    string pExcelFileName = "";
                    ////openFileDialog1.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
                    openFileDialog1.Filter = "XLRadial files|*.xls;*.xlsx";
                    openFileDialog1.FilterIndex = 1;
                    openFileDialog1.InitialDirectory = modMain.gFiles.File_InputPath; //"C:\\";
                    openFileDialog1.Title = "Open";
                    openFileDialog1.FileName = " ";

                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        pExcelFileName = openFileDialog1.FileName;

                        //....EndConfig: Seal
                        clsSeal[] mEndSeal = new clsSeal[2];
                        for (int i = 0; i < 2; i++)
                        {
                            if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                            {
                                mEndSeal[i] = (clsSeal)((clsSeal)(modMain.gProject.Product.EndPlate[i])).Clone();
                            }
                        }

                        modMain.gFiles.Retrieve_Params_XLRadial(pExcelFileName, mProject.PNR.Unit.System,  mOpCond, (clsBearing_Radial_FP)modMain.gProject.Product.Bearing, mEndSeal);

                        for (int i = 0; i < 2; i++)
                        {
                            if (modMain.gProject.Product.EndPlate[i].Type == clsEndPlate.eType.Seal)
                            {
                                modMain.gProject.Product.EndPlate[i] = (clsSeal)mEndSeal[i].Clone();
                            }
                        }

                        modMain.gOpCond =(clsOpCond)mOpCond.Clone();

                        Cursor = Cursors.Default;

                        DisplayData();
                    }
                }

                private void cmbCustName_MouseEnter(object sender, EventArgs e)
                //==============================================================
                {
                    toolTip1.SetToolTip(txtCustName, txtCustName.Text);
                }

                private void txtCustOrderNo_MouseEnter(object sender, EventArgs e)
                //================================================================
                {
                    toolTip1.SetToolTip(txtCustOrderNo, txtCustOrderNo.Text);
                }

                private void txtCustMachineName_MouseEnter(object sender, EventArgs e)
                //=====================================================================
                {
                    toolTip1.SetToolTip(txtCustMachineName, txtCustMachineName.Text);
                }

                private void txtRelatedSONo_MouseEnter(object sender, EventArgs e)
                //==================================================================
                {
                    toolTip1.SetToolTip(txtRelatedSONo, txtRelatedSONo.Text);
                }


              
                    ////private void SaveToDB_Project_ORM(clsProject Project_In)
                    //////======================================================        
                    ////{
                    ////    BearingDBEntities pBearingDBEntities = new BearingDBEntities();

                    ////    //....Customer
                    ////    int pCustCount = (from pRec in pBearingDBEntities.tblCustomer
                    ////                         where pRec.fldName == Project_In.Customer.Name select pRec).Count();

                    ////    int pCustID = 0;

                    ////    if (pCustCount > 0)
                    ////    {

                    ////        //....Record already exists Update record
                    ////        var pCust = (from pRec in pBearingDBEntities.tblCustomer where pRec.fldName == Project_In.Customer.Name  select pRec).First();
                    ////        pCustID =(int) pCust.fldID;
                    ////        pCust.fldName = cmbCustName.Text;
                    ////        pCust.fldOrderNo = Project_In.Customer.OrderNo;
                    ////        pCust.fldMachineName = Project_In.Customer.MachineName;
                    ////        pCust.fldUnit = Project_In.Customer.Unit;
                    ////        pBearingDBEntities.SaveChanges();
                    ////    }
                    ////    else
                    ////    {
                    ////        var pCust = (from pRec in pBearingDBEntities.tblCustomer orderby pRec.fldID descending select pRec).ToList();
                    ////        if (pCust.Count > 0)
                    ////        {
                    ////            pCustID = (int)pCust[0].fldID + 1;
                    ////        }
                    ////        else
                    ////        {
                    ////            pCustID = pCustID + 1;
                    ////        }

                    ////        //....New Record
                    ////        tblCustomer pCustomer = new tblCustomer();
                    ////        pCustomer.fldID = pCustID;
                    ////        pCustomer.fldName = Project_In.Customer.Name;
                    ////        pCustomer.fldOrderNo = Project_In.Customer.OrderNo;
                    ////        pCustomer.fldMachineName = Project_In.Customer.MachineName;
                    ////        pCustomer.fldUnit = Project_In.Customer.Unit;

                    ////        pBearingDBEntities.AddTotblCustomer(pCustomer);
                    ////        pBearingDBEntities.SaveChanges();
                    ////    }


                    ////    //....Sales Order
                    ////    int pSOCount = (from pRec in pBearingDBEntities.tblSalesOrder
                    ////                      where pRec.fldNo == Project_In.SalesOrder.No
                    ////                      select pRec).Count();


                    ////    if (pSOCount > 0)
                    ////    {
                    ////        //....Record already exists Update record
                    ////        var pSONo = (from pRec in pBearingDBEntities.tblSalesOrder where pRec.fldNo == Project_In.SalesOrder.No
                    ////                     select pRec).First();

                    ////        pSONo.fldRelatedSONo= Project_In.SalesOrder.RelatedNo;
                    ////        pSONo.fldType = Project_In.SalesOrder.Type;
                            
                    ////        pBearingDBEntities.SaveChanges();

                    ////        //LineNo
                    ////        ////string pSalesOrderNo = txtSONo.Text;
                    ////        if (txtSONo_Part1.Text != "" && txtSONo_Part2.Text != "" && txtSONo_Part3.Text != "")
                    ////        {
                    ////            modMain.gProject.SalesOrder.No = txtSONo_Part1.Text + " " + txtSONo_Part2.Text;
                    ////            modMain.gProject.SalesOrder.LineNo = txtSONo_Part3.Text;
                    ////        }

                    ////        int pLineCount = (from pRec in pBearingDBEntities.tblLine
                    ////                          where pRec.fldSONo == Project_In.SalesOrder.No && pRec.fldNo == Project_In.SalesOrder.LineNo select pRec).Count();
                    ////        if (pLineCount > 0)
                    ////        {
                    ////            //....Record already exists Update record
                    ////            var pLineNo = (from pRec in pBearingDBEntities.tblLine
                    ////                           where pRec.fldSONo == Project_In.SalesOrder.No && pRec.fldNo == Project_In.SalesOrder.LineNo
                    ////                           select pRec).First();
                    ////            pLineNo.fldSONo = Project_In.SalesOrder.No;
                    ////            pLineNo.fldNo = Project_In.SalesOrder.LineNo;
                    ////            pBearingDBEntities.SaveChanges();
                    ////        }
                    ////    }
                    ////    else
                    ////    {
                    ////        //....New Record
                    ////        tblSalesOrder pSO = new tblSalesOrder();
                    ////        pSO.fldNo = Project_In.SalesOrder.No;
                    ////        pSO.fldRelatedSONo = Project_In.SalesOrder.RelatedNo;
                    ////        pSO.fldType = Project_In.SalesOrder.Type;

                    ////        pBearingDBEntities.AddTotblSalesOrder(pSO);
                    ////        pBearingDBEntities.SaveChanges();

                    ////        //....LineNo
                            
                    ////        tblLine pLine = new tblLine();
                    ////        pLine.fldSONo = Project_In.SalesOrder.No;
                    ////        pLine.fldNo = Project_In.SalesOrder.LineNo;

                    ////        pBearingDBEntities.AddTotblLine(pLine);
                    ////        pBearingDBEntities.SaveChanges();
                    ////    }



                    ////    int pProjectCount = (from pRec in pBearingDBEntities.tblProject where pRec.fldSONo == Project_In.SalesOrder.No &&
                    ////                         pRec.fldLineNo == Project_In.SalesOrder.LineNo && pRec.fldPartNo == Project_In.PartNo
                    ////                         select pRec).Count();


                       
                    ////    //int pProjectCount  = (from pRec in pBearingDBEntities.tblProject_Details where pRec.fldNo == Project_In.No && pRec.fldNo_Suffix == pNoSuffix select pRec).Count();

                    ////    if (pProjectCount > 0)
                    ////    {
                    ////        //....Record already exists Update record
                    ////        var pProject = (from pRec in pBearingDBEntities.tblProject
                    ////                        where pRec.fldSONo == Project_In.SalesOrder.No &&
                    ////                            pRec.fldLineNo == Project_In.SalesOrder.LineNo && pRec.fldPartNo == Project_In.PartNo
                    ////                        select pRec).First();

                    ////        pProject.fldPartNo = Project_In.PartNo;
                    ////        pProject.fldSONo = Project_In.SalesOrder.No;
                    ////        pProject.fldLineNo = Project_In.SalesOrder.LineNo;
                    ////        pProject.fldStatus = Project_In.Status;
                    ////        pProject.fldCustID = pCustID;
                          

                    ////        pBearingDBEntities.SaveChanges();
                    ////    }
                    ////    else
                    ////    {
                    ////        int pProject_ID = 0;
                    ////        var pProject_Count = (from pRec in pBearingDBEntities.tblProject orderby pRec.fldID descending select pRec).ToList();
                    ////        if (pProject_Count.Count > 0)
                    ////        {
                    ////            pProject_ID = (int)pProject_Count[0].fldID + 1;
                    ////        }
                    ////        else
                    ////        {
                    ////            pProject_ID = pProject_ID + 1;
                    ////        }

                    ////        //....New Record
                    ////        tblProject pProject = new tblProject();
                    ////        pProject.fldID = pProject_ID;
                    ////        pProject.fldPartNo = Project_In.PartNo;
                    ////        pProject.fldSONo = Project_In.SalesOrder.No;
                    ////        pProject.fldLineNo = Project_In.SalesOrder.LineNo;
                    ////        pProject.fldStatus = Project_In.Status;
                    ////        pProject.fldCustID = pCustID;

                    ////        pBearingDBEntities.AddTotblProject(pProject);
                    ////        pBearingDBEntities.SaveChanges();
                    ////    }   
                    ////}
            #endregion

                   

        #endregion    
         
    }
}
